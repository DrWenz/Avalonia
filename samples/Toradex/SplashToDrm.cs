using System;
using Avalonia;
using Avalonia.LinuxFramebuffer.Output;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Surfaces;
using SkiaSharp;

namespace Toradex;

public class SplashToDrm : IDisposable
{
    private readonly DrmOutput _drmOutput;
    private IGlPlatformSurfaceRenderingSession? _draw;
    private GRContext? _grContext;
    private GRGlInterface? _iface;
    private GRBackendRenderTarget? _renderTarget;

    private SKSurface? _surface;
    private IGlPlatformSurfaceRenderTarget? _target;

    public SplashToDrm(DrmOutput drmOutput)
    {
        _drmOutput = drmOutput;
        Initialise();
    }

    public SKCanvas Canvas => _surface?.Canvas ?? throw new Exception("Cannot init splash");
    public Size Size => new(_draw.Size.Width, _draw.Size.Height);

    public void Dispose()
    {
        _surface?.Canvas?.Dispose();
        _surface?.Dispose();
        _renderTarget?.Dispose();
        _grContext?.Dispose();
        _iface?.Dispose();
        _draw?.Dispose();
        _target?.Dispose();
    }

    private void Initialise()
    {
        _target = _drmOutput.CreateGlRenderTarget();
        _draw = _target.BeginDraw();

        var context = _draw.Context;
        //context.GlInterface.ClearColor(0, 0, 0.3f, 1);
        context.GlInterface.Clear(GlConsts.GL_COLOR_BUFFER_BIT | GlConsts.GL_STENCIL_BUFFER_BIT);

        _iface = context.Version.Type == GlProfileType.OpenGL
            ? GRGlInterface.CreateOpenGl(proc => context.GlInterface.GetProcAddress(proc))
            : GRGlInterface.CreateGles(proc => context.GlInterface.GetProcAddress(proc));
        _grContext = GRContext.CreateGl(
            _iface,
            new GRContextOptions { AvoidStencilBuffers = true });

        var size = _draw.Size;
        var disp = _draw.Context;
        disp.GlInterface.GetIntegerv(GlConsts.GL_FRAMEBUFFER_BINDING, out var fb);

        _renderTarget =
            new GRBackendRenderTarget(size.Width, size.Height, disp.SampleCount, disp.StencilSize,
                new GRGlFramebufferInfo((uint)fb, SKColorType.Rgba8888.ToGlSizedFormat()));

        _surface = SKSurface.Create(
            _grContext,
            _renderTarget,
            _draw.IsYFlipped ? GRSurfaceOrigin.TopLeft : GRSurfaceOrigin.BottomLeft,
            SKColorType.Rgba8888);
    }
}

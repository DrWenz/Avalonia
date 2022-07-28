using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.LinuxFramebuffer;
using Avalonia.LinuxFramebuffer.Output;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using SkiaSharp;
using SkiaSharp.Skottie;

namespace Toradex.NetCore;

internal class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static int Main(string[] args)
    {
        try
        {
            PerformanceCounter.Step("Application start ...");
            if (args.Contains("--drm"))
            {
                Animation animation = null;
                var lottieTask = Task.Run(() =>
                {
                    animation = Animation.Create(new MemoryStream(Encoding.UTF8.GetBytes(AppResources.SplashScreen)));
                    animation.Seek(0);
                });
                PerformanceCounter.Step("Start creating DrmOutput, lottie init started parallel");
                var drmOutput = new DrmOutput(GetArgumentValue(args, "--card","/dev/dri/card0"), false,
                    new DrmOutputOptions { InitialBufferSwappingColor = Colors.White, EnableInitialBufferSwapping = true });
                PerformanceCounter.Step("DrmOutput created");
                var lottie = new MySplash(drmOutput);
                if (!lottieTask.IsCompleted || !lottieTask.IsCanceled)
                    lottieTask.Wait();
                lottie.Load(animation);
                PerformanceCounter.Step("Splash loaded");
                return BuildAvaloniaApp().StartLinuxDirect(args, drmOutput);
            }

            if (args.Contains("--fbdev"))
                BuildAvaloniaApp().StartLinuxFbDev(args);

            return BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            if (e.StackTrace != null)
                Console.WriteLine(e.StackTrace);
            if (e.InnerException != null)
                Console.WriteLine(e.InnerException);
            return -1;
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .AfterSetup(builder =>
            {
                builder.UseReactiveUI();
            });
    }

    private static string GetArgumentValue(string[] args, string parameter, string defaultValue = "")
    {
        foreach (var arg in args)
            if (arg.StartsWith(parameter))
                return arg.Remove(0, parameter.Length + 1);

        return defaultValue;
    }

    private static double GetArgumentValue(string[] args, string parameter, double defaultValue = 1)
    {
        foreach (var arg in args)
            if (arg.StartsWith(parameter))
            {
                if (double.TryParse(arg.Remove(0, parameter.Length + 1), out var value))
                    return value;
                return defaultValue;
            }

        return defaultValue;
    }

    private class MySplash : LottieSplashToDrm
    {
        public MySplash(DrmOutput drmOutput) : base(drmOutput)
        {
        }

        protected override void Draw(SKCanvas canvas)
        {
            //canvas.DrawImage(SKImage.FromEncodedData("/home/pi/splash.png"), 0, 0);
            // canvas.Flush();
            //canvas.Clear(SKColors.White);
            //canvas.Clear(SKColors.White);
            base.Draw(canvas);
        }
    }
}

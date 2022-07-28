using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace Toradex.Views;

public partial class MainView  : UserControl
{
    private bool _isFirstTimeRendered = true;
    private double _offset;

    private bool _pointerDown;
    private Point? _startPoint;

    public MainView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        PerformanceCounter.Step("OnAttachedToVisualTree");
        e.Root.Renderer.DrawFps = true;
        //ChartHack();
        base.OnAttachedToVisualTree(e);
    }

    public override void Render(DrawingContext context)
    {
        if (_isFirstTimeRendered)
        {
            _isFirstTimeRendered = false;
            App.LottieCancelationSource?.Cancel();
            PerformanceCounter.Step("First time rendered");
        }

        base.Render(context);
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        _pointerDown = true;
        _startPoint = e.GetPosition(this);
    }

    private void InputElement_OnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (_pointerDown)
        {
            var currentPoint = e.GetPosition(this);
            double startY = _startPoint!.Value.Y;
            double currY = currentPoint.Y;
            if (startY > currY)
            {

            }
            else
            {

            }
            _offset = Math.Max(0, currentPoint.Y - _startPoint!.Value.Y);
            (sender as Rectangle).Height = _offset;
        }
    }

    private void InputElement_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (_offset > Bounds.Height * 0.45)
        {
            _offset = Bounds.Height;
        }
        else
        {
            _offset = 30;
        }
        (sender as Rectangle).Height = _offset;
        _pointerDown = false;
        _startPoint = null;
    }
}

using System;
using System.Timers;
using ReactiveUI;

namespace Toradex.ViewModels;

public class MainViewViewModel : ViewModelBase
{
    private string _currentTime = "";
    private Timer _timer;
    
    public string CurrentTime
    {
        get => _currentTime;
        set => this.RaiseAndSetIfChanged(ref _currentTime, value);
    }
    
    public MainViewViewModel()
    {
        _timer = new Timer();
        _timer.Interval = 1000;
        _timer.Elapsed += TimerOnElapsed;
        _timer.Start();
    }

    private void TimerOnElapsed(object sender, ElapsedEventArgs e)
    {
        CurrentTime = DateTime.Now.ToString("HH:mm:ss.ffff");
    }
}

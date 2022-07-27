using System;
using System.Diagnostics;

namespace Toradex;

public static class PerformanceCounter
{
    private static readonly Stopwatch _watch = new();
    private static long _counter = 0;

    public static void Start()
    {
        _watch.Start();
        _counter = 0;
    }

    public static void Stop()
    {
        _watch.Stop();
    }

    public static void Step(string message)
    {
        if (!_watch.IsRunning)
            Start();
        _counter += _watch.ElapsedMilliseconds;
        Console.WriteLine(
            $"Total:{_counter} ms Step:{_watch.ElapsedMilliseconds} ms: {message}");
        _watch.Restart();
    }
}

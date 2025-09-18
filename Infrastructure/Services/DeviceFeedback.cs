#if ANDROID
using System;
using MartaPol.Domain.Abstractions;
using Microsoft.Maui.Devices; // Vibration

namespace MartaPol.Infrastructure.Services;

public class DeviceFeedback : IDeviceFeedback
{
    public void Vibrate(int ms)
    {
        try { Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(Math.Max(1, ms))); } catch { }
    }

    public void Beep()
    {
        try
        {
            using var tg = new Android.Media.ToneGenerator(Android.Media.Stream.System, 100);
            tg.StartTone(Android.Media.Tone.PropBeep, 200);
        }
        catch { /* ignore */ }
    }
}
#else
using MartaPol.Domain.Abstractions;

namespace MartaPol.Infrastructure.Services;

public class DeviceFeedback : IDeviceFeedback
{
    public void Vibrate(int ms) { }
    public void Beep() { }
}
#endif

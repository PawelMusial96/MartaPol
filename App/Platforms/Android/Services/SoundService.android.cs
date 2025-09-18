//using Android.Media;
//using MartaPol.Domain.Abstractions;

//namespace MartaPol.App.Platforms.Android.Services;

//public class SoundServiceAndroid : ISoundService
//{
//    public void Beep()
//    {
//        try
//        {
//            using var tone = new ToneGenerator(Stream.System, 80);
//            tone.StartTone(Tone.PropBeep, 100);
//        }
//        catch { /* no-op */ }
//    }
//}


using Android.Media;
using MartaPol.Domain.Abstractions;
using MediaStream = Android.Media.Stream;

namespace MartaPol.App.Platforms.Android.Services;

public class SoundServiceAndroid : ISoundService
{
    public void Beep()
    {
        try
        {
            using var tone = new ToneGenerator(MediaStream.System, 80);
            tone.StartTone(Tone.PropBeep, 100);
        }
        catch { /* no-op */ }
    }
}


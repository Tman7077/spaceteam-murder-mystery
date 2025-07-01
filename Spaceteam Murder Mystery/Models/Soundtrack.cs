namespace SMM.Models;

using System.Windows.Media.Animation;
public class Soundtrack
{
    private readonly Uri _titleTheme = new($"{AssetHelper.AssetDirectory}/Audio/title.mp3");
    private readonly Uri _mainTheme  = new($"{AssetHelper.AssetDirectory}/Audio/mystery-in-the-stars.mp3");
    private double       _maxVolume  = AppSettings.Volume;
    private MediaPlayer  _mp;

    public Soundtrack(SoundtrackType soundtrack)
    {
        _mp = soundtrack switch
        {
            SoundtrackType.TitleTheme => Setup(_titleTheme),
            SoundtrackType.MainTheme  => Setup(_mainTheme),
            _ => throw new ArgumentException($"Unknown soundtrack: {soundtrack}")
        };
    }

    public async Task Start(double seconds = 1)
    {
        if (AppSettings.Muted) return;
        _mp.Volume = 0;
        _mp.Clock.Controller.Begin();
        await FadeVolumeAsync(FadeType.In, TimeSpan.FromSeconds(seconds));
    }

    public async Task Stop(double seconds = 1.5)
    {
        await FadeVolumeAsync(FadeType.Out, TimeSpan.FromSeconds(seconds));
        _mp.Clock.Controller.Stop();
    }

    public void Pause()  => _mp.Clock.Controller.Pause();

    public void Resume() => _mp.Clock.Controller.Resume();

    public async Task SwitchTrack(SoundtrackType soundtrack, double fadeOutSeconds = 1.5, double fadeInSeconds = 1)
    {
        await Stop(fadeOutSeconds);
        _mp = soundtrack switch
        {
            SoundtrackType.TitleTheme => Setup(_titleTheme),
            SoundtrackType.MainTheme  => Setup(_mainTheme),
            _ => throw new ArgumentException($"Unknown soundtrack: {soundtrack}")
        };
        await Start(fadeInSeconds);
    }

    public void ChangeVolume(double volume)
    {
        if (volume < 0 || volume > 1)
        { throw new ArgumentOutOfRangeException(nameof(volume), "Volume must be between 0 and 1"); }
        _mp.Volume = _maxVolume = volume;
    }

    public async Task Mute()
    {
        AppSettings.Muted = true;
        await Stop(0.1);
    }
    
    public async Task Unmute()
    {
        AppSettings.Muted = false;
        await Start(0.1);
    }

    private async Task FadeVolumeAsync(FadeType inOut, TimeSpan duration)
    {
        if (duration.TotalMilliseconds <= 0)
        { throw new ArgumentOutOfRangeException(nameof(duration), "Duration must be greater than zero"); }

        double steps = 20;
        int delay = (int)(duration.TotalMilliseconds / steps);

        for (int i = 1; i <= steps; i++)
        {
            double factor = inOut == FadeType.In
                ? i / steps
                : 1 - (i / steps);
            _mp.Volume = _maxVolume * factor;
            await Task.Delay(delay);
        }
    }

    private MediaPlayer Setup(Uri uri)
    {
        MediaTimeline timeline = new(uri) { RepeatBehavior = RepeatBehavior.Forever };
        MediaClock clock = timeline.CreateClock();
        clock.Controller.Stop();

        MediaPlayer player = new()
        {
            Clock = clock,
            Volume = _maxVolume
        };

        return player;
    }
}
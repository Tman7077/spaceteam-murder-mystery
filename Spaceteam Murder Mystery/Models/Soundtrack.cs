namespace SMM.Models;

using System.Windows.Media.Animation;

/// <summary>
/// A controller for background music.
/// </summary>
public class Soundtrack
{
    /// <summary>
    /// The path to the title theme.
    /// </summary>
    private readonly Uri _titleTheme = new($"{AssetHelper.AssetDirectory}/Audio/title.mp3");

    /// <summary>
    /// The path to the main game theme.
    /// </summary>
    private readonly Uri _mainTheme = new($"{AssetHelper.AssetDirectory}/Audio/mystery-in-the-stars.mp3");

    /// <summary>
    /// The maximum volume for the soundtrack, which is set to the app's default volume.
    /// This can be changed by the user through the settings.
    /// </summary>
    private double _maxVolume = AppSettings.Volume;

    /// <summary>
    /// The media player instance that plays the soundtrack.
    /// </summary>
    private MediaPlayer _mp;

    /// <summary>
    /// Initializes a new instance of the Soundtrack class with the specified soundtrack type.
    /// </summary>
    /// <param name="soundtrack">Which soundtrack to play immediately.</param>
    public Soundtrack(SoundtrackType soundtrack)
    {
        _mp = soundtrack switch
        {
            SoundtrackType.TitleTheme => Setup(_titleTheme),
            SoundtrackType.MainTheme  => Setup(_mainTheme),
            _ => throw new ArgumentException($"Unknown soundtrack: {soundtrack}")
        };
    }

    /// <summary>
    /// Starts playing the soundtrack with a fade-in effect.
    /// </summary>
    /// <param name="seconds">How long to fade in before the soundtrack is full volume.</param>
    public async Task Start(double seconds = 1)
    {
        if (AppSettings.Muted) return;
        _mp.Volume = 0;
        _mp.Clock.Controller.Begin();
        await FadeVolumeAsync(FadeType.In, TimeSpan.FromSeconds(seconds));
    }

    /// <summary>
    /// Fades out the soundtrack until it is muted.
    /// Stops the soundtrack after the fade-out is complete.
    /// </summary>
    /// <param name="seconds">How long to fade out before the soundtrack is muted.</param>
    public async Task Stop(double seconds = 1.5)
    {
        await FadeVolumeAsync(FadeType.Out, TimeSpan.FromSeconds(seconds));
        _mp.Clock.Controller.Stop();
    }

    /// <summary>
    /// Pauses the soundtrack.
    /// </summary>
    public void Pause() => _mp.Clock.Controller.Pause();

    /// <summary>
    /// Resumes the soundtrack.
    /// </summary>
    public void Resume() => _mp.Clock.Controller.Resume();

    /// <summary>
    /// Switches the current soundtrack to a new one with a fade-out and fade-in effect.
    /// </summary>
    /// <param name="soundtrack">The soundtrack to which to switch.</param>
    /// <param name="fadeOutSeconds">How long to fade out before switching tracks.</param>
    /// <param name="fadeInSeconds">How long to fade in after switching tracks.</param>
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

    /// <summary>
    /// Changes the volume of the soundtrack.
    /// </summary>
    /// <param name="volume">A number between 0 and 1 representing the volume percentage.</param>
    public void ChangeVolume(double volume)
    {
        if (volume < 0 || volume > 1)
        { throw new ArgumentOutOfRangeException(nameof(volume), "Volume must be between 0 and 1"); }
        _mp.Volume = _maxVolume = volume;
    }

    /// <summary>
    /// Mutes the soundtrack.
    /// </summary>
    public async Task Mute()
    {
        AppSettings.Muted = true;
        await Stop(0.1);
    }

    /// <summary>
    /// Unmutes the soundtrack, restoring it to the last set volume.
    /// </summary>
    public async Task Unmute()
    {
        AppSettings.Muted = false;
        await Start(0.1);
    }

    /// <summary>
    /// Fades the volume of the soundtrack in or out over a specified duration.
    /// </summary>
    /// <param name="inOut">Whether to fade in or out.</param>
    /// <param name="duration">A span of time over which to fade.</param>
    private async Task FadeVolumeAsync(FadeType inOut, TimeSpan duration)
    {
        if (duration.TotalMilliseconds <= 0)
        { throw new ArgumentOutOfRangeException(nameof(duration), "Duration must be greater than zero"); }

        double steps = 20;
        int delay    = (int)(duration.TotalMilliseconds / steps);

        for (int i = 1; i <= steps; i++)
        {
            double factor = inOut == FadeType.In
                ? i / steps
                : 1 - (i / steps);
            _mp.Volume = _maxVolume * factor;
            await Task.Delay(delay);
        }
    }

    /// <summary>
    /// Creates a MediaPlayer instance for the specified URI.
    /// The MediaPlayer is set up with a MediaClock
    /// that repeats forever and is stopped by default.
    /// </summary>
    /// <param name="uri">The track to play.</param>
    /// <returns>A complete, stopped MediaPlayer.</returns>
    private MediaPlayer Setup(Uri uri)
    {
        MediaTimeline timeline = new(uri) { RepeatBehavior = RepeatBehavior.Forever };
        MediaClock clock       = timeline.CreateClock();
        clock.Controller.Stop();

        MediaPlayer player = new()
        {
            Clock  = clock,
            Volume = _maxVolume
        };

        return player;
    }
}
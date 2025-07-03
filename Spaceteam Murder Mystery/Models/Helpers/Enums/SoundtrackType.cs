namespace SMM.Models.Helpers.Enums;

/// <summary>
/// Delineates between the two game soundtracks.
/// </summary>
public enum SoundtrackType : byte
{
    /// <summary>
    /// The song to play while on the title screen.
    /// </summary>
    TitleTheme,

    /// <summary>
    /// THe song to play during a game.
    /// </summary>
    MainTheme
}
namespace SMM.Models.Helpers.Enums;

/// <summary>
/// Represents whether a screen or
/// a soundtrack should fade in or out.
/// </summary>
public enum FadeType : byte
{
    /// <summary>
    /// Fade in, from 0 (volume or opacity) to 1.
    /// </summary>
    In,

    /// <summary>
    /// Fade out, from 1 (volume or opacity) to 0.
    /// </summary>
    Out
}
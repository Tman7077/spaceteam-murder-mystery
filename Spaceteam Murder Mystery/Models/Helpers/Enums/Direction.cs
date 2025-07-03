namespace SMM.Models.Helpers.Enums;

/// <summary>
/// Represents the direction that a character is facing,
/// to place them on the correct side of the screen
/// during their interview(s).
/// </summary>
public enum Direction : byte
{
    /// <summary>
    /// The character is facing left.
    /// </summary>
    Left,
    
    /// <summary>
    /// The character is facing right.
    /// </summary>
    Right
}
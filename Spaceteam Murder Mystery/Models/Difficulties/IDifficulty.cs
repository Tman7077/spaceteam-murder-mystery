namespace SMM.Models.Difficulties;

/// <summary>
/// An interface for classes containing methods specific to games on a given difficulty.
/// </summary>
public interface IDifficulty
{
    /// <summary>
    /// Selects random guilty character(s) depending on the difficulty.
    /// <code>Easy: 1</code>
    /// <code>Medium: 1</code>
    /// <code>Hard: 2</code>
    /// </summary>
    /// <param name="chars"></param>
    static void SelectGuilty(CharacterSet chars)
    {
        string key = chars.Keys.ElementAt(new Random().Next(chars.Count));
        chars[key].IsGuilty = true;
    }
    
    /// <summary>
    /// Selects clues to fill a crime scene for the given victim character.
    /// The method of clue selection is determined by the difficulty level.
    /// <code>Easy: Up to 4</code>
    /// <code>Medium: Up to 6</code>
    /// <code>Hard: Up to 8</code>
    /// More details on implementation visible in each difficulty class.
    /// </summary>
    /// <param name="clues">The empty set of clues to which to add.</param>
    /// <param name="chars">The current CharacterSet, containing innocence and life status.</param>
    /// <param name="victim">The name of the character for whose crime scene to select clues.</param>
    static abstract void SelectClues(HashSet<Clue> clues, CharacterSet chars, string victim);
}
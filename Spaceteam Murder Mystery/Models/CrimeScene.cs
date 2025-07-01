namespace SMM.Models;

using System.IO;

/// <summary>
/// Represents a complete crime scene.
/// Contains all clues relevant to a given victim's death and the scene's image path.
/// </summary>
public class CrimeScene
{
    public string        Victim { get; }
    public HashSet<Clue> Clues  { get; } = [];
    public GameState     State  { get; }

    /// <summary>
    /// Initializes a new instance of the CrimeScene class for the specified victim.
    /// </summary>
    /// <param name="victim">The character whose crime scene to initialize.</param>
    /// <param name="gameState">The current GameState.</param>
    public CrimeScene(string victim, GameState gameState)
    {
        Validator.ValidateShortCharacterName(victim);

        Victim = victim;
        State  = gameState;
        State.SelectClues(Clues, Victim);
    }
}
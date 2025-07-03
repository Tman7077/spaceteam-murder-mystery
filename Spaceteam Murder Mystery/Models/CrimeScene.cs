namespace SMM.Models;

/// <summary>
/// Represents a complete crime scene.
/// Contains all clues relevant to a given victim's death and the scene's image path.
/// </summary>
public class CrimeScene
{
    /// <summary>
    /// The short name of the character whose crime scene to display.
    /// </summary>
    public string Victim { get; }
    
    /// <summary>
    /// A collection of clues relevant to the crime scene of the victim.
    /// These clues are selected based on difficulty.
    /// </summary>
    public HashSet<Clue> Clues { get; } = [];
    
    /// <summary>
    /// The current GameState, used to validate the character and select clues.
    /// </summary>
    public GameState State { get; }

    /// <summary>
    /// Initializes a new instance of the CrimeScene class for the specified victim.
    /// </summary>
    /// <param name="victim">The character whose crime scene to initialize.</param>
    /// <param name="gameState">The current GameState.</param>
    public CrimeScene(string victim, GameState gameState)
    {
        Validator.ValidateCharacter(victim, gameState);

        Victim = victim;
        State  = gameState;
        State.SelectClues(Clues, Victim);
    }
}
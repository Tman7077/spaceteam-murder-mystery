namespace SMM.Models;

using System.IO;

/// <summary>
/// Represents a complete crime scene.
/// Contains all clues relevant to a given victim's death and the scene's image path.
/// </summary>
public class CrimeScene
{
    private readonly HashSet<Clue> _clues;

    public string Victim { get; }
    public string Scene { get => Path.Combine(PathHelper.GetAssetDirectory(), "Images", "Crime Scenes", $"{Victim}.png"); }
    public HashSet<Clue> Clues { get => _clues; }
    public GameState State { get; }

    /// <summary>
    /// Initializes a new instance of the CrimeScene class for the specified victim.
    /// </summary>
    /// <param name="victim">The character whose crime scene to initialize.</param>
    /// <param name="gameState">The current GameState.</param>
    public CrimeScene(string victim, GameState gameState)
    {
        _clues = [];
        Victim = victim;
        State = gameState;
        State.SelectClues(ref _clues, Victim);
    }
}
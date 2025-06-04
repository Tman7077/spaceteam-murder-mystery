using System.IO;
using SMM.Services;

namespace SMM.Models;

public class CrimeScene
{
    private readonly HashSet<Clue> _clues;
    
    public string Victim { get; }
    public string Scene { get => Path.Combine(PathHelper.GetAssetDirectory(), "Images", "Crime Scenes", $"{Victim}.png"); }
    public HashSet<Clue> Clues { get => _clues;}
    public GameState State { get; }

    public CrimeScene(string victim, GameState gameState)
    {
        _clues = [];
        Victim = victim;
        State = gameState;
        State.SelectClues(ref _clues, Victim);
    }
}
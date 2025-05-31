using System.IO;
using SMM.Services;

namespace SMM.Models;

public class CrimeScene(string victim, GameState gameState)
{
    private HashSet<Clue> _clues = [];
    
    public string Victim { get; } = victim;
    public string Scene { get => Path.Combine(PathHelper.GetAssetDirectory(), "Images", "Crime Scenes", $"{Victim}.png"); }
    public HashSet<Clue> Clues { get => _clues;}
    public GameState State { get; } = gameState;

    public void SelectClues()
    {
        State.SelectClues(ref _clues, Victim);
    }
}
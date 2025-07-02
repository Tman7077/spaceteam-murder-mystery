namespace SMM.Services;

/// <summary>
/// A general state tracker for various information pertaining to a game.
/// </summary>
public class GameState
{
    private string?    _lastVictim = null;

    public CharacterSet Characters { get; } = [];
    public Story        Story      { get; }
    public string       Difficulty { get; set; }
    public string       LastVictim
    {
        get => _lastVictim ?? throw new InvalidOperationException("No victims have been killed yet.");
        set => _lastVictim = value;
    }

    /// <summary>
    /// Initializes a GameState given a difficulty level.
    /// </summary>
    /// <param name="difficulty">The difficulty level on which to play the game.</param>
    public GameState(string difficulty)
    {
        Difficulty = difficulty;
        Story = Parser.ParseStoryData();
        LoadCharacters();
    }

    /// <summary>
    /// Kills a character. If a name is not provided, kill a random character.
    /// </summary>
    /// <param name="victim">The character to kill.</param>
    /// <param name="who">The short name of the character killed.</param>
    /// <returns>
    /// The short name of the character killed.
    /// This will be equal to the <b>name</b> argument, if it is provided.
    /// </returns>
    public void KillCharacter(Victim victim, out string who)
    {
        // Select a random character if no name is provided. Ignores guilty characters.
        List<string> livingInnocents = Characters.GetLivingNames(includeGuilty: false);

        // Make sure there are living characters available to kill.
        if (livingInnocents.Count == 0)
        { throw new InvalidOperationException("No living characters available to kill."); }

        // Assign the name based on victim type.
        string name = victim switch
        {
            // Random victim = random name (from living innocents).
            Victim.Random            => livingInnocents[new Random().Next(livingInnocents.Count)],
            Victim.ByName.Innocent v => v.Name,
            Victim.ByName.Voted    v => v.Name,
            _ => throw new ArgumentException($"Unknown victim type: {victim.GetType()}")
        };

        Validator.ValidateCharacter(name, this);

        // Kill the character.
        Character character = Characters[name];
        character.IsAlive = false;

        // Report the character's name.
        who = character.ShortName;

        // If the character was not voted out, add them to the list of victims.
        if (victim is not Victim.ByName.Voted)
        { LastVictim = who; }
    }

    public void KillCharacter(Victim.ByName.Voted victim) =>
        KillCharacter(victim, out _);

    /// <summary>
    /// Selects a subset of clues for a certain victim's crime scene
    /// based on the difficulty level and current living characters.
    /// </summary>
    /// <param name="clues">The set of clues to which to add.</param>
    /// <param name="victimName">The victim for whose crime scene to select clues.</param>
    public void SelectClues(HashSet<Clue> clues, string victimName)
    {
        Validator.ValidateCharacter(victimName, this);

        Difficulties.All[Difficulty].SelectClues(clues, Characters, victimName);
    }

    private void LoadCharacters()
    {
        string charDir = Path.Combine(AssetHelper.AssetDirectory, "Text", "Characters");

        foreach (string charFile in Directory.GetFiles(charDir, "*.md"))
        {
            string charName = Path.GetFileNameWithoutExtension(charFile);
            Character character = Parser.ParseCharacter(charName);
            Characters[charName] = character;
        }

        Difficulties.All[Difficulty].SelectGuilty(Characters);
    }
}
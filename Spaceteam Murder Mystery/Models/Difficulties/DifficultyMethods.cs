namespace SMM.Models.Difficulties;

public delegate void DSelectGuilty(CharacterSet chars);
public delegate void DSelectClues(HashSet<Clue> clues, CharacterSet chars, string victim);
public record DifficultyMethods(DSelectGuilty SelectGuilty, DSelectClues SelectClues);

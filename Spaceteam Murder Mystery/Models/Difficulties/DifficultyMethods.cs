namespace SMM.Models.Difficulties;

public delegate void SelectGuiltyDel(CharacterSet chars);
public delegate void SelectCluesDel(HashSet<Clue> clues, CharacterSet chars, string victim);
public record DifficultyMethods(SelectGuiltyDel SelectGuilty, SelectCluesDel SelectClues);

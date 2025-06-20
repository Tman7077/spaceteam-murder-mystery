namespace SMM.Models.Difficulties;

public delegate void SelectGuiltyDel(CharacterSet chars);
public delegate void SelectCluesDel(HashSet<Clue> clues, CharacterSet chars, string victim);
public delegate string GetResponseDel(Character interviewee, InterviewType type, string victim);
public record DifficultyMethods(SelectGuiltyDel SelectGuilty, SelectCluesDel SelectClues, GetResponseDel GetResponse);

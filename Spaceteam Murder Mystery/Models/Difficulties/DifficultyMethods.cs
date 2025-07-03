namespace SMM.Models.Difficulties;

// public delegate void SelectGuiltyDel(CharacterSet chars);
// public delegate void SelectCluesDel(HashSet<Clue> clues, CharacterSet chars, string victim);
// public delegate string GetResponseDel(Character interviewee, InterviewType type, string victim);
// public record DifficultyMethods(SelectGuiltyDel SelectGuilty, SelectCluesDel SelectClues, GetResponseDel GetResponse);

/// <summary>
/// A container for the three methods necessary for
/// a class implementing the IDifficulty interface.
/// </summary>
/// <param name="SelectGuilty">A reference to a method in a class implementing IDifficulty.</param>
/// <param name="SelectClues">A reference to a method in a class implementing IDifficulty.</param>
/// <param name="GetResponse">A reference to a method in a class implementing IDifficulty.</param>
public record DifficultyMethods
(
    Action<CharacterSet> SelectGuilty,
    Action<HashSet<Clue>, CharacterSet, string> SelectClues,
    Func<Character, InterviewType, string, string> GetResponse
);

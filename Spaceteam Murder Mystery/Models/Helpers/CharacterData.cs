namespace SMM.Models.Helpers;

/// <summary>
/// A wrapper for a group of data required in order to construct a Character.
/// </summary>
/// <param name="Name">The character's full name (First Last).</param>
/// <param name="Role">The character's flavor text/mini-description.</param>
/// <param name="Motto">The character's flavor quote/slogan.</param>
/// <param name="ProfileUri">The full path to the character's portrait.</param>
/// <param name="CrimeSceneUri">The full path to the character's crime scene image.</param>
/// <param name="Facing">The direction that the character's portrait is facing.</param>
/// <param name="Description">The full description of/introduction for the character.</param>
/// <param name="DeathStory">The full description of the character's death.</param>
/// <param name="Clues">A set of Clues that link the character to each other character's death.</param>
/// <param name="Interviews">A collection of the character's responses to being interviewed.</param>
/// <param name="Accusations">A collection of the character's responses to being accused.</param>
public record CharacterData
(
    string        Name,
    string        Role,
    string        Motto,
    Uri           ProfileUri,
    Uri           CrimeSceneUri,
    Direction     Facing,
    string        Description,
    string        DeathStory,
    HashSet<Clue> Clues,
    InterviewSet  Interviews,
    InterviewSet  Accusations
);
namespace SMM.Services;

public static class Validator
{
    /// <summary>
    /// Validates that the given character name is a valid short name.
    /// </summary>
    /// <param name="characterName">The short name of a character.</param>
    public static void ValidateShortCharacterName(string characterName)
    {
        if (string.IsNullOrWhiteSpace(characterName))
        { throw new ArgumentException("Character name cannot be null or whitespace.", nameof(characterName)); }

        if (characterName.Contains(' '))
        { throw new ArgumentException("Provide the short character name.", nameof(characterName)); }
    }

    /// <summary>
    /// Validates that the given character name exists in the game state.
    /// </summary>
    /// <param name="characterName">The short name of a character.</param>
    /// <param name="gameState">The current game state.</param>
    public static void ValidateCharacter(string characterName, GameState gameState)
    {
        ValidateShortCharacterName(characterName);
        if (!gameState.Characters.ContainsKey(characterName))
        { throw new ArgumentException($"Character '{characterName}' does not exist in game data.", nameof(characterName)); }
    }
}
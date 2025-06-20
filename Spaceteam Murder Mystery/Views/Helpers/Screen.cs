namespace SMM.Views.Helpers;

public record Screen
{
    public sealed record CrimeScene(string VictimName) : Screen;
    public sealed record Difficulty : Screen;
    public sealed record InspectionChar(InterviewType Type, string Interviewee, string Victim) : Screen;
    public sealed record InspectionClue(Clue Clue) : Screen;
    public sealed record NewGame : Screen;
    public sealed record Selection(InterviewType Type) : Screen;
    public sealed record Settings : Screen;
    public sealed record Title : Screen;
}
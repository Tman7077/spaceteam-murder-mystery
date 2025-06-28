namespace SMM.Views.Helpers;

public abstract record Screen
{
    public sealed record CrimeScene : Screen
    {
        public string VictimName { get; }
        public CrimeScene(string VictimName)
        {
            Validator.ValidateShortCharacterName(VictimName);
            this.VictimName = VictimName;
        }
    }
    public sealed record Difficulty  : Screen;
    public sealed record InspectChar : Screen
    {
        public InterviewType Type { get; }
        public string Interviewee { get; }
        public string Victim      { get; }
        public InspectChar(InterviewType Type, string Interviewee, string Victim)
        {
            Validator.ValidateShortCharacterName(Interviewee);
            Validator.ValidateShortCharacterName(Victim);
            this.Type        = Type;
            this.Interviewee = Interviewee;
            this.Victim      = Victim;
        }
    }
    public sealed record InspectClue(Clue Clue)        : Screen;
    public sealed record Selection(InterviewType Type) : Screen;
    public sealed record Settings                      : Screen;
    public sealed record Story(Advance Advance)        : Screen;
    public sealed record Title                         : Screen;
}
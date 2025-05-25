namespace SMM.Models
{
    public class Story(string intro, string firstMurder)
    {
        public string Intro { get; } = intro;
        public string FirstMurder { get; } = firstMurder;
    }
}
namespace SMM.Models.Helpers;

public readonly struct InterviewType
{
    public static readonly InterviewType Interview  = new("Interview");
    public static readonly InterviewType Accusation = new("Accusation");

    public string Type { get; }

    private InterviewType(string name) => Type = name;

    public override string ToString() => Type;

    public override bool Equals(object? obj) => obj is InterviewType other && Type == other.Type;

    public override int GetHashCode() => Type.GetHashCode();

    public static bool operator ==(InterviewType left, InterviewType right) => left.Equals(right);

    public static bool operator !=(InterviewType left, InterviewType right) => !(left == right);

    public static InterviewType operator !(InterviewType dir) => dir == "Interview" ? Accusation : Interview;

    public static implicit operator string(InterviewType d) => d.Type;
}
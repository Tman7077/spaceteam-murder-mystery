
namespace SMM.Models.Helpers;

public readonly struct Direction
{
    public static readonly Direction Left = new("Left");
    public static readonly Direction Right = new("Right");

    public string Dir { get; }

    private Direction(string name) => Dir = name;

    public override string ToString() => Dir;

    public override bool Equals(object? obj) => obj is Direction other && Dir == other.Dir;

    public override int GetHashCode() => Dir.GetHashCode();

    public static bool operator ==(Direction left, Direction right) => left.Equals(right);

    public static bool operator !=(Direction left, Direction right) => !(left == right);

    public static Direction operator !(Direction dir) =>
        dir.Dir == "Left" ? Right : Left;

    public static implicit operator string(Direction d) => d.Dir;
}
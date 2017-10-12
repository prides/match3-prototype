using System;

[Flags]
public enum Direction
{
    None = 0,
    Left = 1,
    Right = 2,
    Up = 4,
    Down = 8
}

public class DirectionHelper
{
    public static Direction GetOppositDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                return Direction.Right;
            case Direction.Right:
                return Direction.Left;
            case Direction.Up:
                return Direction.Down;
            case Direction.Down:
                return Direction.Up;
            default:
                return Direction.None;
        }
    }
}
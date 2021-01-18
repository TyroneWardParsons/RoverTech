namespace RoverTech.Logic.Models
{
    public class Position
    {
        public Point Point { get; set; }

        public Cardinal Direction { get; set; }

        public Position()
        {
            Point = new Point();
            Direction = default;
        }

        public Position(Point point, Cardinal direction)
        {
            Point = point;
            Direction = direction;
        }

        public void RotateLeft()
        {
            switch (Direction)
            {
                case Cardinal.North:
                    Direction = Cardinal.West;
                    break;
                case Cardinal.East:
                    Direction = Cardinal.North;
                    break;
                case Cardinal.South:
                    Direction = Cardinal.East;
                    break;
                case Cardinal.West:
                    Direction = Cardinal.South;
                    break;
            }
        }

        public void RotateRight()
        {
            switch (Direction)
            {
                case Cardinal.North:
                    Direction = Cardinal.East;
                    break;
                case Cardinal.East:
                    Direction = Cardinal.South;
                    break;
                case Cardinal.South:
                    Direction = Cardinal.West;
                    break;
                case Cardinal.West:
                    Direction = Cardinal.North;
                    break;
            }
        }

        public void MoveForward()
        {
            switch (Direction)
            {
                case Cardinal.North:
                    Point.Y += 1;
                    break;
                case Cardinal.East:
                    Point.X += 1;
                    break;
                case Cardinal.South:
                    Point.Y -= 1;
                    break;
                case Cardinal.West:
                    Point.X -= 1;
                    break;
            }
        }

        public override string ToString()
        {
            return $"[{Point}, {Direction}]";
        }
    }
}

namespace RoverTech.Logic.Models
{
    public class Rover
    {
        public Position Position { get; set; }

        public Rover()
        {
            Position = new Position();
        }

        public Rover(Position position)
        {
            Position = position;
        }

        public void RotateLeft() => Position.RotateLeft();

        public void RotateRight() => Position.RotateRight();

        public void MoveForward() => Position.MoveForward();
    }
}

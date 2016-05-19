using System;
using DisruptoLib.Factories;
using SharpKit.JavaScript;

namespace DisruptoLib.Entities
{
    [JsType(JsMode.Clr, Filename = "../res/Entities.js")]
    public class Character
    {
        private int _health = 100;

        public Guid Id { get; set; }

        public Point Position { get; set; }

        public Point Destination { get; set; }

        public Point LookTarget { get; set; }

        public float Speed { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public Character()
        {
            Position = new Point(0, 0);

            Destination = new Point(0, 0);

            LookTarget = new Point(0, 0);

            Speed = 1.0F;

            Width = 1;

            Height = 1;

            Id = Guid.Empty;
        }

        public int Health
        {
            get { return _health; }
            set
            {
                _health = value;

                if (_health < 1) CharacterFactory.KillCharacter(this);
            }
        }

        public Rectangle Area { get { return new Rectangle(Position.X, Position.Y, Width, Height); } }
    }
}
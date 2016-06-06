using System;
using SharpKit.JavaScript;

namespace DisruptoLib
{
    // Have to put this here because sharpkit is being dumb

    [JsType(JsMode.Clr, Filename = "res/Lib.js")]
    public class Point
    {
        public int X { get; set; }

        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;

            Y = y;
        }
    }

    [JsType(JsMode.Clr, Filename = "res/Lib.js")]
    public class Rectangle
    {
        public double X { get; set; }

        public double Y { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public double Right { get { return X + Width; } }

        public double Top { get { return Y + Height; } }

        public Rectangle(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public bool Contains(Rectangle rect)
        {
            if (rect.Right < this.X ||
                rect.X > this.Right ||
                rect.Y > this.Top ||
                rect.Top < this.Y)
            {
                return false;
            }

            return true;
        }
    }

    [JsType(JsMode.Clr, Filename = "res/Lib.js")]
    public struct ConnectPacket
    {
        public string SessionId;
        public Guid CharacterId;
    }
}
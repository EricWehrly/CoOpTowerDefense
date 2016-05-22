using System;
using SharpKit.Html;
using SharpKit.JavaScript;
using DisruptoLib.Entities;
using DisruptoLib.Factories;

namespace CoOpClient
{
    [JsType(JsMode.Clr, Filename = "res/ZombieGameClientScript.js")]
    public static class HeartBeat
    {
        private const int HeartBeatTimeOut = 200;   // Milliseconds
        private const int MaxFrameRate = 60;

        //static HeartBeat()
        public static void Initialize()
        {
            HtmlContext.window.setTimeout(MainLoop, 0);
            HtmlContext.window.setInterval(PhysicsLoop, HeartBeatTimeOut);
        }

        // TODO: This should be registered into...
        public static void PhysicsLoop()
        {
            PlayerInput.ProcessPlayerMovementInput();

            // (No need right now to calculate speed, but it would go here)

            foreach (var player in CharacterFactory.Characters)
            {
                //player.Position
                MoveEntityTowardDestination(player);
            }
        }

        private static void MoveEntityTowardDestination(Character character)
        {
            // Base class operator override would be nice, but this is one of those places we have to acquiesce to sharpkit
            if (character.Destination.X == character.Position.X && character.Destination.Y == character.Position.Y) return;

            var remainingSpeed = character.Speed;
            
            var xDestDiff = Math.Abs(character.Destination.X - character.Position.X);
            var yDestDiff = Math.Abs(character.Destination.Y - character.Position.Y);

            while (remainingSpeed > 0)
            {
                if (xDestDiff > yDestDiff)
                {
                    if (character.Destination.X > character.Position.X)
                    {
                        character.Position.X += 1;
                    }
                    else character.Position.X -= 1;

                    xDestDiff = Math.Abs(character.Destination.X - character.Position.X);
                }
                else
                {
                    if (character.Destination.Y > character.Position.Y)
                    {
                        character.Position.Y += 1;
                    }
                    else character.Position.Y -= 1;

                    yDestDiff = Math.Abs(character.Destination.Y - character.Position.Y);
                }

                remainingSpeed--;
            }
        }

        public static void MainLoop()
        {
            //get starting time            
            DateTime start = DateTime.Now;

            // Input stuff
            // If key is down, change destination

            //perform calculations

            //perform renders
            ClientRenderer.Render();

            //get frame interval millis
            double frameInterval = (1.0 / MaxFrameRate) * 1000;
            DateTime end = DateTime.Now;
            int frameTime = end.Subtract(start).Milliseconds;
            if (frameTime > frameInterval)
            {
                HtmlContext.window.setTimeout(MainLoop, 0);
            }
            else
            {
                HtmlContext.window.setTimeout(MainLoop, (int)frameInterval - frameTime);
            }
        }
    }
}
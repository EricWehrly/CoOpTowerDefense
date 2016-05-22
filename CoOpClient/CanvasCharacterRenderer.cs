using System;
using SharpKit.JavaScript;
using DisruptoLib;
using DisruptoLib.Entities;

namespace CoOpClient
{
    [JsType(JsMode.Clr, Filename = "res/ZombieGameClientScript.js")]
    public class CanvasCharacterRenderer
    {
        public Character Character { get; set; }

        //public int RenderX { get; private set; }

        //public int RenderY { get; private set; }

        public Rectangle RenderPosition { get; private set; }

        public int RenderRotation { get; private set; }

        public CanvasCharacterRenderer(Character character)
        {
            Character = character;

            RenderPosition = new Rectangle(-1, -1, 0, 0);

            RenderRotation = 0;
        }

        public void CalculateEntityDrawPosition()
        {
            // Calculate the X, Y, width, and height of the element in pixels
            RenderPosition.X = (int)(Character.Position.X * ClientRenderer.StageTileSize.Width);
            // The "1" comes from the height of the object...
            RenderPosition.Y = ((Character.Position.Y + 1) * ClientRenderer.StageTileSize.Height);

            RenderPosition.Width = 40;

            RenderPosition.Height = 40;

            CalculateRotation();

            // SetRectangleSizeFromSprite();

            // ApplyViewScrolling();

            // ApplyIsometricView();

            FlipYAxis();
        }

        private void CalculateRotation()
        {
            var xDiff = Character.LookTarget.X - Character.Position.X;
            var yDiff = Character.LookTarget.Y - Character.Position.Y;

            if (yDiff == 0)
            {
                if (xDiff > 0)
                {
                    RenderRotation = 0;
                    return;
                }
                else
                {
                    RenderRotation = 180;
                    return;
                }
            }

            if (xDiff == 0)
            {
                if (yDiff > 0)
                {
                    RenderRotation = 90;
                    return;
                }
                else
                {
                    RenderRotation = 270;
                    return;
                }
            }

            var arctan = Math.Atan(yDiff/xDiff);
            RenderRotation = (int)(1 - arctan) * 100;
            //RenderRotation = Character.LookTarget
        }

        private void SetRectangleSizeFromSprite()
        {
            /*
            if (gent.GetSprite() != null)
            {
                returnRect.width = (int)(gent.GetSprite().Size.width);// * Stage.CurrentStage.GetTileSize().width);
                returnRect.height = (int)(gent.GetSprite().Size.height);// * Stage.CurrentStage.GetTileSize().height);
            }
            else
            {
                returnRect.width = Stage.CurrentStage.GetTileSize().width;
                returnRect.height = Stage.CurrentStage.GetTileSize().height;
            }
            */
        }

        private void ApplyViewScrolling()
        {
            // Correct based on which portion of the view we're rendering...
            //returnRect.x -= View.GetMainView().OffsetX;
            //returnRect.y -= View.GetMainView().OffsetY;
        }

        private void FlipYAxis()
        {
            // Correct for Y 0 being at the top of the screen rather than the bottom.
            // Displace the Y the correct number of units from the top of the screen, as opposed to the bottom.

            // The Y is getting all messed up because we're flipping the Y axis, so we need a better translation ...

            //returnRect.y -= (int)(this.GetSize().height / 2);// -returnRect.y;
            RenderPosition.Y = ClientRenderer.WindowRenderSize.Height - RenderPosition.Y;
            //returnRect.y -= (int)(Stage.CurrentStage.Height* Stage.CurrentStage.GetTileSize().height);
        }
    }
}
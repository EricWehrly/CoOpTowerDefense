using System;
using System.Collections.Generic;
using SharpKit.Html;
using SharpKit.JavaScript;
using DisruptoLib;
using DisruptoLib.Entities;
using DisruptoLib.Factories;

namespace CoOpClient
{
    [JsType(JsMode.Clr, Filename = "res/CoOpClient.js")]
    public static class ClientRenderer // : IRenderer?
    {
        private static List<CanvasCharacterRenderer> _characterRenderers;
        //private static Dictionary<int, CanvasCharacterRenderer> _characterRenderers;

        public static DateTime LastUpdate { get; private set; }

        // Make some unit tests to make sure that this thing has comperable contents to the character list
        //public static List<CanvasCharacterRenderer> CharacterRenderers { get { return new List<CanvasCharacterRenderer>(_characterRenderers.Values); } }
        public static List<CanvasCharacterRenderer> CharacterRenderers { get { return _characterRenderers;  }}

        private static CanvasRenderingContext2D _canvasRenderingContext2D;

        public static Rectangle StageTileSize { get; set; }

        public static Rectangle WindowRenderSize { get; private set; }

        private static HtmlImageElement _playerImage;

        static ClientRenderer()
        {
            StageTileSize = new Rectangle(0, 0, 40, 40);

            WindowRenderSize = new Rectangle(0, 0, 1920, 1080);
        }

        public static void Initialize()
        {
            PrepareTheBody();

            CreateCanvasElement();

            WindowRenderSize = new Rectangle(0, 0, HtmlContext.window.innerWidth, HtmlContext.window.innerHeight);

            _playerImage = new HtmlImageElement();
            _playerImage.src = "res/Square.gif";
            //HtmlContext.document.body.appendChild(_playerImage);

            _characterRenderers = new List<CanvasCharacterRenderer>();

            Resize();
        }

        private static void PrepareTheBody()
        {
            // Empty body contents
            HtmlContext.document.body.innerHTML = "";

            // Set body appearance
            HtmlContext.document.body.style.margin = new JsString("0");
            HtmlContext.document.body.style.padding = new JsString("0");
            HtmlContext.document.body.style.overflow = new JsString("hidden");
            //HtmlContext.document.body.style.backgroundColor = new JsString("white");
        }

        private static void CreateCanvasElement()
        {
            var canvas = new HtmlCanvasElement { id = "canvas2D" };
            canvas.style.width = new JsString("100%");
            canvas.style.height = new JsString("100%");
            HtmlContext.document.body.appendChild(canvas);
            _canvasRenderingContext2D = canvas.As<HtmlCanvasElement>().getContext("2d").As<CanvasRenderingContext2D>();
        }

        public static void Resize()
        {
            var canvas = HtmlContext.document.getElementById("canvas2D").As<HtmlCanvasElement>();
            canvas.width = HtmlContext.window.innerWidth;
            canvas.height = HtmlContext.window.innerHeight;
        }


        private static void CreateCharacterRenderer(Character character)
        {
            if(_characterRenderers == null) _characterRenderers = new List<CanvasCharacterRenderer>();
            _characterRenderers.Add(new CanvasCharacterRenderer(character));
        }

        private static void ClearCharacterRenderers()
        {
            _characterRenderers = new List<CanvasCharacterRenderer>();
        }

        public static void Render()
        {
            ClearCanvas();

            SyncCharacterRenderers();

            foreach (var characterRenderer in _characterRenderers)
            {
                //DrawCharacter(characterRenderer);
                DrawRotatedCharacter(characterRenderer);
            }
        }

        public static void SyncCharacterRenderers()
        {
            if (LastUpdate == CharacterFactory.LastUpdate) return;

            ClearCharacterRenderers();

            foreach (var character in CharacterFactory.Characters)
            {
                CreateCharacterRenderer(character);
            }

            LastUpdate = DateTime.Now;
        }

        private static void ClearCanvas()
        {
            _canvasRenderingContext2D.clearRect(0, 0, HtmlContext.window.innerWidth, HtmlContext.window.innerHeight);
        }

        private static void DrawRotatedCharacter(CanvasCharacterRenderer characterRenderer)
        {
            characterRenderer.CalculateEntityDrawPosition();

            //var TO_RADIANS = JsMath.PI / 180;
            // save the current co-ordinate system 
            // before we screw with it
            _canvasRenderingContext2D.save();

            // move to the middle of where we want to draw our image
            _canvasRenderingContext2D.translate(characterRenderer.RenderPosition.X, characterRenderer.RenderPosition.Y);

            // rotate around that point, converting our 
            // angle from degrees to radians 
            _canvasRenderingContext2D.rotate(characterRenderer.RenderRotation);// * TO_RADIANS);

            // draw it up and to the left by half the width
            // and height of the image 
            // _canvasRenderingContext2D.drawImage(_playerImage, -(image.width / 2), -(image.height / 2));
            _canvasRenderingContext2D.drawImage(_playerImage, characterRenderer.RenderPosition.X,
                characterRenderer.RenderPosition.Y, characterRenderer.RenderPosition.Width, characterRenderer.RenderPosition.Height);

            // and restore the co-ords to how they were when we began
            _canvasRenderingContext2D.restore();
        }
    }
}
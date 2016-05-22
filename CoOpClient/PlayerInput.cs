using System;
using System.Collections.Generic;
using SharpKit.JavaScript;
using DisruptoLib;
using DisruptoLib.Factories;

namespace CoOpClient
{
    [JsType(JsMode.Clr, Filename = "res/ZombieGameClientScript.js")]
    public class PlayerInput
    {
        private static readonly Dictionary<string, int> _playerMovementKeys;
        private static List<int> _pressedKeys = new List<int>();
        private static bool _isMouseDown = false;

        static PlayerInput()
        {
            _playerMovementKeys = new Dictionary<string, int>();

            _playerMovementKeys["Left"] = 65;
            _playerMovementKeys["Right"] = 68;
            _playerMovementKeys["Up"] = 87;
            _playerMovementKeys["Down"] = 83;
        }

        public static void ProcessPlayerKeyboardInput(int keyCode, bool keyIsDown)
        {
            console.log(keyCode.ToString());

            /*
            if (IsPlayerMovementKey(keyCode))
            {
                ProcessPlayerMovementInput(keyCode);
            }
            */
            if (keyIsDown && (!_pressedKeys.Contains(keyCode))) _pressedKeys.Add(keyCode);
            else if (!keyIsDown && _pressedKeys.Contains(keyCode)) _pressedKeys.Remove(keyCode);
        }

        public static void ProcessPlayerMouseInput(int keyCode, int x, int y, bool isKeyDown)
        {
            _isMouseDown = isKeyDown;

            y = (int) ClientRenderer.WindowRenderSize.Height - y;

            x = ConvertXFromPixelsToTiles(x);
            y = ConvertYFromPixelsToTiles(y);

            CharacterFactory.GetLocalPlayerCharacter().Destination = new Point(x, y);
        }

        private static int ConvertXFromPixelsToTiles(int x)
        {
            x = (int)Math.Floor(x / ClientRenderer.StageTileSize.Width);
            return x;
        }

        private static int ConvertYFromPixelsToTiles(int y)
        {
            y = (int)Math.Floor(y / ClientRenderer.StageTileSize.Height);
            return y;

            //y = (int)(y / 40);
            //y = y/40;
            //y = JsMath.floor(y / 40);
        }

        public static void ProcessPlayerMouseMovement(int newX, int newY)
        {
            newY = (int)ClientRenderer.WindowRenderSize.Height - newY;

            CharacterFactory.GetLocalPlayerCharacter().LookTarget.X = newX / (int) ClientRenderer.StageTileSize.Width;
            CharacterFactory.GetLocalPlayerCharacter().LookTarget.Y = ConvertYFromPixelsToTiles(newY);
        }

        public static void ProcessPlayerMovementInput()
        {
            if (CharacterFactory.GetLocalPlayerCharacter() == null) return;

            if (_pressedKeys.Contains(_playerMovementKeys["Left"])) MovePlayerLeft();

            if (_pressedKeys.Contains(_playerMovementKeys["Right"])) MovePlayerRight();

            if (_pressedKeys.Contains(_playerMovementKeys["Up"])) MovePlayerUp();

            if (_pressedKeys.Contains(_playerMovementKeys["Down"])) MovePlayerDown();

            if (CharacterFactory.GetLocalPlayerCharacter().Destination.X < 0)
                CharacterFactory.GetLocalPlayerCharacter().Destination.X = 0;
            if (CharacterFactory.GetLocalPlayerCharacter().Destination.Y < 0)
                CharacterFactory.GetLocalPlayerCharacter().Destination.Y = 0;
        }

        private static void MovePlayerLeft()
        {
            CharacterFactory.GetLocalPlayerCharacter().Destination.X = CharacterFactory.GetLocalPlayerCharacter().Position.X -
                                                        (int)CharacterFactory.GetLocalPlayerCharacter().Speed;
        }

        private static void MovePlayerRight()
        {
            CharacterFactory.GetLocalPlayerCharacter().Destination.X = CharacterFactory.GetLocalPlayerCharacter().Position.X +
                                                        (int)CharacterFactory.GetLocalPlayerCharacter().Speed;
        }

        private static void MovePlayerUp()
        {
            CharacterFactory.GetLocalPlayerCharacter().Destination.Y = CharacterFactory.GetLocalPlayerCharacter().Position.Y +
                                                        (int)CharacterFactory.GetLocalPlayerCharacter().Speed;
        }

        private static void MovePlayerDown()
        {
            CharacterFactory.GetLocalPlayerCharacter().Destination.Y = CharacterFactory.GetLocalPlayerCharacter().Position.Y -
                                                        (int)CharacterFactory.GetLocalPlayerCharacter().Speed;
        }
    }
}
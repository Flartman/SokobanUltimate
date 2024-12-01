using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SokobanUltimate.Models;
using System.Text.RegularExpressions;

namespace SokobanUltimate.Controller
{
    public class KeyboardGameController : IGameController
    {
        private KeyboardState _oldKeyboardState;
        private readonly GameModel _model;
        private readonly Game1 _game;

        public KeyboardGameController(GameModel model, Game1 game)
        {
            _game = game;
            _model = model;
        }

        private bool ButtonWasPressed(KeyboardState newKeyboardState, Keys key)
        {
            return newKeyboardState.IsKeyDown(key)
                && !_oldKeyboardState.IsKeyDown(key);
        }

        public void GetInput()
        {
            var newKeyboardState = Keyboard.GetState();

            if (ButtonWasPressed(newKeyboardState, Keys.W))
            {
                _model.TryMovePlayer(0, -1);
            }

            if (ButtonWasPressed(newKeyboardState, Keys.S))
            {
                _model.TryMovePlayer(0, 1);
            }

            if (ButtonWasPressed(newKeyboardState, Keys.A))
            {
                _model.TryMovePlayer(-1, 0);
            }

            if (ButtonWasPressed(newKeyboardState, Keys.D))
            {
                _model.TryMovePlayer(1, 0);
            }

            if (ButtonWasPressed(newKeyboardState, Keys.R))
            {
                _game.StartGame();
            }

                _oldKeyboardState = newKeyboardState;
        }
    }
}

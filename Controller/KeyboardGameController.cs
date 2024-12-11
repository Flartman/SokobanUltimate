
using Microsoft.Xna.Framework.Input;
using static Sokoban.Shared;

namespace Sokoban.Controller
{
    public class KeyboardGameController : IGameController
    {
        private readonly App app;
        private KeyboardState _oldKeyboardState;

        public KeyboardGameController(App app)
        {
            this.app = app;
        }

        public void ProcessInput()
        {
            var newKeyboardState = Keyboard.GetState();

            if (ButtonWasPressed(newKeyboardState, Keys.W))
            {
                app.ProcessCommand(InputCommand.Up);
            }

            if (ButtonWasPressed(newKeyboardState, Keys.S))
            {
                app.ProcessCommand(InputCommand.Down);
            }

            if (ButtonWasPressed(newKeyboardState, Keys.A))
            {
                app.ProcessCommand(InputCommand.Left);
            }

            if (ButtonWasPressed(newKeyboardState, Keys.D))
            {
                app.ProcessCommand(InputCommand.Right);
            }

            if (ButtonWasPressed(newKeyboardState, Keys.R))
            {
                app.ProcessCommand(InputCommand.Restart);
            }

            if (ButtonWasPressed(newKeyboardState, Keys.Escape))
            {
                app.ProcessCommand(InputCommand.Back);
            }

            if (ButtonWasPressed(newKeyboardState, Keys.Enter))
            {
                app.ProcessCommand(InputCommand.Enter);
            }

            if (ButtonWasPressed(newKeyboardState, Keys.Back))
            {
                app.ProcessCommand(InputCommand.Edit);
            }

            if (ButtonWasPressed(newKeyboardState, Keys.N))
            {
                app.ProcessCommand(InputCommand.PrimaryAction);
            }

            if (ButtonWasPressed(newKeyboardState, Keys.M))
            {
                app.ProcessCommand(InputCommand.SecondaryAction);
            }

            _oldKeyboardState = newKeyboardState;
        }

        private bool ButtonWasPressed(KeyboardState newKeyboardState, Keys key)
        {
            return newKeyboardState.IsKeyDown(key)
                && !_oldKeyboardState.IsKeyDown(key);
        }
    }
}

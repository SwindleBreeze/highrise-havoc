using highrisehavoc.Source.Entities;
using highrisehavoc.Source.Renderers;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace highrisehavoc.Source.Controllers
{
    public class ButtonController
    {
        Button button;
        ButtonRenderer buttonRenderer;
        public ButtonController(Button button, ButtonRenderer buttonRenderer)
        {
            this.button = button;
            this.buttonRenderer = buttonRenderer;
        }

        public void setText(string text)
        {
            button.Text = text;
        }

        public void Draw()
        {
            buttonRenderer.Draw(button);
        }

        public void Update(MouseState mouseState, MouseState prevMouseState)
        {
            bool isMouseOver = button.Bounds.Contains(mouseState.Position);

            // Check if the button is pressed
            button.IsPressed = isMouseOver && mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using highrisehavoc.Source.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace highrisehavoc.Source.Entities
{
    public class Button
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle Bounds { get; set; }
        public bool IsPressed { get; set; }
        public Button(Rectangle bounds, Vector2 position, string text)
        {
            Bounds = bounds;
            Position = position;
            Text = text;
            IsPressed = false;
        }
    }
}

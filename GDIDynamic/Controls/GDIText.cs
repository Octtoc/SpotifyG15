using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDIDynamic.Controls
{
    public class GDIRollingText : IGDIControl
    {
        public GDIRollingText(Rectangle rectangle, string text)
        {
            this.Speed = 1;
            this.Direction = this.Speed;
            this.Rectangle = rectangle;
            this.Text = text;
        }

        public int Direction { get; set; }
        public Rectangle Rectangle { get; set; }
        public int Speed { get; set; }
        public string Text { get; set; }

        public void Draw(Graphics graphics, Brush currentBrush, Font currentFont, Pen currentPen)
        {
            if (Rectangle.X > 163)
            {
                Direction = -2;
            }
            else if (Rectangle.X < 0)
            {
                Direction = 2;
            }
            graphics.DrawString(Text, currentFont, currentBrush, Rectangle.X, Rectangle.Y);
            Rectangle = new Rectangle(Rectangle.X + Direction, Rectangle.Y, 0, 0);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDIMusic.Controls
{
    public class GDIRollingText : IGDIControl
    {
        public GDIRollingText(Rectangle rectangle, string text)
        {
            this.Rolling = true;
            this.Speed = -2;
            this.Direction = this.Speed;
            this.Rectangle = rectangle;
            this.Text = text;
        }

        public int Direction { get; set; }
        public Rectangle Rectangle { get; set; }
        public int Speed { get; set; }
        public string Text { get; set; }
        public bool Rolling { get; set; }

        public void Draw(Graphics graphics, Brush currentBrush, Font currentFont, Pen currentPen)
        {
            if (Rectangle.X > 20)
            {
                Direction = -2;
            }
            else if (Rectangle.X + graphics.MeasureString(Text, currentFont).Width < 140)
            {
                Direction = 2;
            }
            graphics.DrawString(Text, currentFont, currentBrush, Rectangle.X, Rectangle.Y);
            if(graphics.MeasureString(Text, currentFont).Width + 20 > 163)
            {
                Rectangle = new Rectangle(Rectangle.X + Direction, Rectangle.Y, 0, 0);
            }            
        }
    }
}
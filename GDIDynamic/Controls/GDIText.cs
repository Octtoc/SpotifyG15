using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDIMusic.Controls
{
    public class GDIText : IGDIControl
    {
        public GDIText(Rectangle rectangle, string text)
        {
            this.Rectangle = rectangle;
            this.Text = text;
        }
        public Rectangle Rectangle { get; set; }
        public string Text { get; set; }

        public void Draw(Graphics graphics, Brush currentBrush, Font currentFont, Pen currentPen)
        {
            graphics.DrawString(Text, currentFont, currentBrush, Rectangle.X, Rectangle.Y);
        }
    }
}
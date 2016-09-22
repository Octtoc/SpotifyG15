using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDIMusic.Controls
{
    public class GDIProgressBar : IGDIControl
    {
        private int _progress;

        public GDIProgressBar(Rectangle rectangle)
        {
            _progress = 0;
            Max = 100;
            OuterRectangle = rectangle;
            InnerRectangle = new Rectangle(OuterRectangle.X, OuterRectangle.Y, 0, 0);
        }

        public Rectangle InnerRectangle { get; set; }

        public int Max { get; set; }

        public Rectangle OuterRectangle { get; set; }

        public int Progress
        {
            get
            {
                return _progress;
            }
            set
            {
                if (Max != 0)
                {
                    float percent = (float)value / Max;
                    int width = Convert.ToInt32(OuterRectangle.Width * percent);
                    InnerRectangle = new Rectangle(new Point(OuterRectangle.X, OuterRectangle.Y), new Size(width, OuterRectangle.Height));
                }
                _progress = value;
            }
        }

        public void Draw(Graphics graphics, Brush currentBrush, Font currentFont, Pen currentPen)
        {
            graphics.DrawRectangle(currentPen, OuterRectangle);
            graphics.FillRectangle(currentBrush, InnerRectangle);
        }
    }
}
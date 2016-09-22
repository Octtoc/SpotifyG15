using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDIMusic.Controls
{
    public interface IGDIControl
    {
        void Draw(Graphics graphics, Brush currentBrush, Font currentFont, Pen currentPen);
    }
}
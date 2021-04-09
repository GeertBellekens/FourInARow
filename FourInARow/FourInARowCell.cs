using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourInARow
{
    public class FourInARowCell
    {
        public CellColor color { get; set; } = CellColor.empty;
    }
    public enum CellColor
    {
        empty,
        yellow,
        red
    }
}

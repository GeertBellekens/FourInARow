using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourInARow
{
    public class FourInARowColumn
    {
        private List<FourInARowCell> _cells;
        public FourInARowColumn()
        {
            //create cells and add all the cells
            this._cells = new List<FourInARowCell>();
            for (int i = 0; i < 6; i++)
            {
                _cells.Add(new FourInARowCell());
            }
        }
        public bool addDisk(CellColor color)
        {
            if (this.isFull) return false; //failed
            this._cells.First(x => x.color == CellColor.empty).color = color;
            return true;
        }
        public bool isFull
        {
            get => !this._cells.Any(x => x.color == CellColor.empty);
        }

        public List<FourInARowCell>cells
        {
            get => this._cells;
        }

        internal void removeDisk()
        {
            if (this.cells.Any(x => x.color != CellColor.empty))
            {
                this._cells.Last(x => x.color != CellColor.empty).color = CellColor.empty;
            }
        }
    }
}

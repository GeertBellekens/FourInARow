using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourInARow
{
    public class FourInARowGame
    {
        private List<FourInARowColumn> _columns;
        public List<FourInARowColumn> columns { get => this._columns; }

        public FourInARowGame()
        {
            this._columns = new List<FourInARowColumn>();
            for (int i = 0; i < 7; i++)
            {
                this._columns.Add(new FourInARowColumn());
            }
            //yellow always starts
            this.currentPlayer = CellColor.yellow;
        }
        
        public CellColor currentPlayer { get; private set; }
        private void switchPlayer()
        {
            this.currentPlayer = this.currentPlayer == CellColor.yellow ? CellColor.red : CellColor.yellow;
        }

        public void play(int columnIndex)
        {
            if (columnIndex < this._columns.Count
                && ! this._columns[columnIndex].isFull)
            {
                this._columns[columnIndex].addDisk(this.currentPlayer);
                this.switchPlayer();
            }
        }

    }
}

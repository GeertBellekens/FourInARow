using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace FourInARow
{
    public class FourInARowGame
    {
        private List<FourInARowColumn> _columns;
        public bool vsComputer { get; private set; }
        public List<FourInARowColumn> columns { get => this._columns; }

        public FourInARowGame(bool vsComputer)
        {
            this.vsComputer = vsComputer;
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
                this.determimeWinner();
                if (this.vsComputer
                    && this.currentPlayer == CellColor.red
                    && this.winner == CellColor.empty)
                {
                    this.playAsComputer();
                }
            }
        }
        private void playAsComputer()
        {
            //check if not everything is full already
            if (!this.columns.Any( x => x.isFull == false))
            {
                return;
            }
            //play random
            Random rand = new Random();
            bool played = false;
            while(! played)
            {
                var randomColumn = rand.Next(0, this.columns.Count); 
                if (! this.columns[randomColumn].isFull)
                {
                    this.play(randomColumn);
                    played = true;
                }
            }
        }
        private void determimeWinner()
        {
            for (int i = 0; i < this.columns.Count; i++)
            {
                for (int j = 0; j < this.columns[i].cells.Count; j++)
                {
                    if (this.columns[i].cells[j].color != CellColor.empty)
                    {
                        //check if there is a 4 in a row starting from this cell
                        if (this.isFourInARow(i, j))
                        {
                            //we have a winner
                            this.winner = this.columns[i].cells[j].color;
                            return;
                        }
                    }
                    else
                    {
                        break;//no sense in continuing upwards if they are empty
                    }
                }
            }
        }
        private bool isFourInARow(int i, int j)
        {
            
            var color = this.columns[i].cells[j].color;
            //search up
            for (int k = 1; k <= 3; k++)
            {
                if (j + k < this.columns[i].cells.Count )
                {
                    if (this.columns[i].cells[j+k].color != color)
                    {
                        //not 4 in a row, we can stop right here
                        break;
                    }
                    if (k == 3)
                    {
                        //found 4 in a row
                        return true;
                    }
                }
            }
            //search diagonal left
            for (int k = 1; k <= 3; k++)
            {
                if (j + k < this.columns[i].cells.Count
                    && i - k >= 0)
                {
                    if (this.columns[i - k].cells[j + k].color != color)
                    {
                        //not 4 in a row, we can stop right here
                        break;
                    }
                    if (k == 3)
                    {
                        //found 4 in a row
                        return true;
                    }
                }
            }
            //search diagonal right
            for (int k = 1; k <= 3; k++)
            {
                if (j + k < this.columns[i].cells.Count
                    && i + k < this.columns.Count)
                {
                    if (this.columns[i + k].cells[j + k].color != color)
                    {
                        //not 4 in a row, we can stop right here
                        break;
                    }
                    if (k == 3)
                    {
                        //found 4 in a row
                        return true;
                    }
                }
            }
            //search right
            for (int k = 1; k <= 3; k++)
            {
                if (i + k < this.columns.Count)
                {
                    if (this.columns[i + k].cells[j].color != color)
                    {
                        //not 4 in a row, we can stop right here
                        break;
                    }
                    if (k == 3)
                    {
                        //found 4 in a row
                        return true;
                    }
                }
            }
            return false;
        }
        public CellColor winner { get; private set; }

    }
}

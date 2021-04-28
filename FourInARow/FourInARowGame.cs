using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace FourInARow
{
    public class FourInARowGame
    {
        private List<FourInARowColumn> _columns;
        public bool vsComputer { get; private set; }
        public CellColor winner { get; private set; }
        public int getTotalScore()
        {
            var redsScore = this.getScore(CellColor.red);
            var yellowScore = this.getScore(CellColor.yellow);
            return redsScore - yellowScore *2;
        }
        public int getScore(CellColor color)
        {
            int score = 0;
            var combos = this.getCombos(color);
            score = combos[1] + combos[2] * 5 + combos[3] * 20 + combos[4] * 10000;
            return score;
        }

        private Dictionary<int, int> getCombos(CellColor color)
        {
            var combos = new Dictionary<int, int>();
            combos.Add(1, 0);
            combos.Add(2, 0);
            combos.Add(3, 0);
            combos.Add(4, 0);
            for (int i = 0; i < this.columns.Count; i++)
            {
                for (int j = 0; j < this.columns[i].cells.Count; j++)
                {
                    if (this.columns[i].cells[j].color == color
                        || this.columns[i].cells[j].color == CellColor.empty)
                    {
                        var newCombos = getCombosForCell(i, j, color);
                        combos[1] += newCombos[1];
                        combos[2] += newCombos[2];
                        combos[3] += newCombos[3];
                        combos[4] += newCombos[4];
                    }
                    if (this.columns[i].cells[j].color == CellColor.empty)
                    {
                        //if we have reached the end then we stop
                        break;
                    }
                }
            }
            return combos;
        }
        private Dictionary<int, int> getCombosForCell(int i, int j, CellColor color)
        {
            var combos = new Dictionary<int, int>();
            combos.Add(0, 0);
            combos.Add(1, 0);
            combos.Add(2, 0);
            combos.Add(3, 0);
            combos.Add(4, 0);
            
            //search NW
            if (i - 3 >= 0
            && j + 3 < this.columns[i].cells.Count())
            {
                int size = 0;//always at least one
                for (int k = 0; k <= 3; k++)
                {
                    var currentCellColor = this.columns[i - k].cells[j + k].color;
                    if (currentCellColor == color)
                    {
                        size++;
                    }
                    else if (currentCellColor != color
                        && currentCellColor != CellColor.empty)
                    {
                        //found the other color, no result
                        break;
                    }
                    if (k == 3)
                    {
                        //arrived at 4th cell, found a combo
                        combos[size]++;
                        break;
                    }
                }
            }
            //search N
            if (j + 3 < this.columns[i].cells.Count)
            {
                int size = 0;//always at least one
                for (int k = 0; k <= 3; k++)
                {
                    var currentCellColor = this.columns[i].cells[j + k].color;
                    if (currentCellColor == color)
                    {
                        size++;
                    }
                    else if (currentCellColor != color
                        && currentCellColor != CellColor.empty)
                    {
                        //found the other color, no result
                        break;
                    }
                    if (k == 3)
                    {
                        //arrived at 4th cell, found a combo
                        combos[size]++;
                        break;
                    }
                }
            }
            //search NE
            if (j + 3 < this.columns[i].cells.Count
                && i + 3 < this.columns.Count)
            {
                int size = 0;//always at least one
                for (int k = 0; k <= 3; k++)
                {
                    var currentCellColor = this.columns[i + k].cells[j + k].color;
                    if (currentCellColor == color)
                    {
                        size++;
                    }
                    else if (currentCellColor != color
                        && currentCellColor != CellColor.empty)
                    {
                        //found the other color, no result
                        break;
                    }
                    if (k == 3)
                    {
                        //arrived at 4th cell, found a combo
                        combos[size]++;
                        break;
                    }
                }
            }
            //search E
            if (i + 3 < this.columns.Count)
            {
                int size = 0;//always at least one
                for (int k = 0; k <= 3; k++)
                {
                    var currentCellColor = this.columns[i + k].cells[j].color;
                    if (currentCellColor == color)
                    {
                        size++;
                    }
                    else if (currentCellColor != color
                        && currentCellColor != CellColor.empty)
                    {
                        //found the other color, no result
                        break;
                    }
                    if (k == 3)
                    {
                        //arrived at 4th cell, found a combo
                        combos[size]++;
                        break;
                    }
                }
            }
            return combos;
        }

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
                && !this._columns[columnIndex].isFull)
            {
                this._columns[columnIndex].addDisk(this.currentPlayer);
                this.switchPlayer();
                this.determineWinner();
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
            if (!this.columns.Any(x => x.isFull == false))
            {
                return;
            }
            //find the best option
            int bestColumn = 0;
            int maximumScore = int.MinValue;
            for (int i = 0; i < this.columns.Count; i++)
            {
                if (!this.columns[i].isFull)
                {
                    this.columns[i].addDisk(CellColor.red);
                    var score = this.getTotalScore();
                    if (this.getWinner() == CellColor.red)
                    {
                        //red won, don't calculate further
                        bestColumn = i;
                        this.columns[i].removeDisk();
                        break;
                    }
                    else
                    {
                        score = calculateNextMove(CellColor.yellow);
                    }
                    if (score > maximumScore)
                    {
                        bestColumn = i;
                        maximumScore = score;
                    }
                    //remove again
                    this.columns[i].removeDisk();
                }
            }
            //play the best column
            this.play(bestColumn);
        }
        private int calculateNextMove(CellColor color)
        {
            //get the score with the lowest points
            int minimumScore = int.MaxValue;
            for (int i = 0; i < this.columns.Count; i++)
            {
                if (!this.columns[i].isFull)
                {
                    this.columns[i].addDisk(color);
                    var score = this.getTotalScore();
                    if (score < minimumScore)
                    {
                        minimumScore = score;
                    }
                    //remove again
                    this.columns[i].removeDisk();
                }
            }
            return minimumScore;
        }
        private void determineWinner()
        {
            this.winner = getWinner();
        }
        private CellColor getWinner()
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
                            return this.columns[i].cells[j].color;
                        }
                    }
                    else
                    {
                        break;//no sense in continuing upwards if they are empty
                    }
                }
            }
            return CellColor.empty;
        }
        private bool isFourInARow(int i, int j)
        {

            var color = this.columns[i].cells[j].color;
            //search up
            for (int k = 1; k <= 3; k++)
            {
                if (j + k < this.columns[i].cells.Count)
                {
                    if (this.columns[i].cells[j + k].color != color)
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


    }
}

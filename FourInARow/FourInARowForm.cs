using FourInARow.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FourInARow
{
    public partial class FourInARowForm : Form
    {
        private FourInARowGame game;
        private List<Button> _dropButtons;
        private List<Button> dropButtons
        {
            get
            {
                if (this._dropButtons == null)
                {
                    this._dropButtons = new List<Button>();
                    this._dropButtons.Add(dropAButton);
                    this._dropButtons.Add(dropBButton);
                    this._dropButtons.Add(dropCButton);
                    this._dropButtons.Add(dropDButton);
                    this._dropButtons.Add(dropEButton);
                    this._dropButtons.Add(dropFButton);
                    this._dropButtons.Add(dropGButton);
                }
                return this._dropButtons;
            }
        }
        private List<List<PictureBox>> _cells;
        private List<List<PictureBox>> cells
        {
            get
            {
                if (this._cells == null)
                {
                    this._cells = new List<List<PictureBox>>();
                    var columnA = new List<PictureBox>();
                    columnA.Add(this.A1);
                    columnA.Add(this.A2);
                    columnA.Add(this.A3);
                    columnA.Add(this.A4);
                    columnA.Add(this.A5);
                    columnA.Add(this.A6);
                    columnA.Reverse();
                    this._cells.Add(columnA);

                    var columnB = new List<PictureBox>();
                    columnB.Add(this.B1);
                    columnB.Add(this.B2);
                    columnB.Add(this.B3);
                    columnB.Add(this.B4);
                    columnB.Add(this.B5);
                    columnB.Add(this.B6);
                    columnB.Reverse();
                    this._cells.Add(columnB);

                    var columnC = new List<PictureBox>();
                    columnC.Add(this.C1);
                    columnC.Add(this.C2);
                    columnC.Add(this.C3);
                    columnC.Add(this.C4);
                    columnC.Add(this.C5);
                    columnC.Add(this.C6);
                    columnC.Reverse();
                    this._cells.Add(columnC);

                    var columnD = new List<PictureBox>();
                    columnD.Add(this.D1);
                    columnD.Add(this.D2);
                    columnD.Add(this.D3);
                    columnD.Add(this.D4);
                    columnD.Add(this.D5);
                    columnD.Add(this.D6);
                    columnD.Reverse();
                    this._cells.Add(columnD);

                    var columnE = new List<PictureBox>();
                    columnE.Add(this.E1);
                    columnE.Add(this.E2);
                    columnE.Add(this.E3);
                    columnE.Add(this.E4);
                    columnE.Add(this.E5);
                    columnE.Add(this.E6);
                    columnE.Reverse();
                    this._cells.Add(columnE);

                    var columnF = new List<PictureBox>();
                    columnF.Add(this.F1);
                    columnF.Add(this.F2);
                    columnF.Add(this.F3);
                    columnF.Add(this.F4);
                    columnF.Add(this.F5);
                    columnF.Add(this.F6);
                    columnF.Reverse();
                    this._cells.Add(columnF);

                    var columnG = new List<PictureBox>();
                    columnG.Add(this.G1);
                    columnG.Add(this.G2);
                    columnG.Add(this.G3);
                    columnG.Add(this.G4);
                    columnG.Add(this.G5);
                    columnG.Add(this.G6);
                    columnG.Reverse();
                    this._cells.Add(columnG);
                }
                return this._cells;
            }
        }
        public FourInARowForm(FourInARowGame game)
        {
            InitializeComponent();
            this.game = game;
            this.playAgainstComputerCheckbox.Checked = this.game.vsComputer;
            this.renderGame();
        }
        
        private void renderGame()
        {
            for (int i = 0; i < game.columns.Count(); i++)
            {
                //set dropButton status and color
                this.dropButtons[i].Enabled = !game.columns[i].isFull
                                               && (this.game.winner == CellColor.empty);
                if (!game.columns[i].isFull
                    && (this.game.winner == CellColor.empty))
                {
                    this.dropButtons[i].BackColor = this.game.currentPlayer == CellColor.yellow
                                                ? Color.Gold
                                                : Color.OrangeRed;
                }
                else
                {
                    this.dropButtons[i].BackColor = default;
                }
                //set cells
                for (int j = 0; j < game.columns[i].cells.Count(); j++)
                {
                    switch(game.columns[i].cells[j].color)
                    {
                        case CellColor.yellow:
                            this.cells[i][j].Image = Resources.yellowChip;
                            break;
                        case CellColor.red:
                            this.cells[i][j].Image = Resources.redChip;
                            break;
                        default:
                            this.cells[i][j].Image = null;
                            break;
                    }
                }
            }
            //check if there is a winner
            if (this.game.winner != CellColor.empty)
            {
                var winnerName = this.game.winner == CellColor.red
                                ? "Rood"
                                : "Geel";
                this.statusLabel.Text = $"De winnaar is {winnerName}!";
            }
            else
            {
                this.statusLabel.Text = string.Empty;
            }
        }

        private void playTurn(int columnIndex)
        {
            this.game.play(columnIndex);
            this.renderGame();
        }

        private void dropAButton_Click(object sender, EventArgs e)
        {
            this.playTurn(0);
        }

        private void dropBButton_Click(object sender, EventArgs e)
        {
            this.playTurn(1);
        }

        private void dropCButton_Click(object sender, EventArgs e)
        {
            this.playTurn(2);
        }

        private void dropDButton_Click(object sender, EventArgs e)
        {
            this.playTurn(3);
        }

        private void dropEButton_Click(object sender, EventArgs e)
        {
            this.playTurn(4);
        }

        private void dropFButton_Click(object sender, EventArgs e)
        {
            this.playTurn(5);
        }

        private void dropGButton_Click(object sender, EventArgs e)
        {
            this.playTurn(6);
        }

        private void newGameButton_Click(object sender, EventArgs e)
        {
            this.game = new FourInARowGame(this.playAgainstComputerCheckbox.Checked);
            this.renderGame();
        }
    }
}

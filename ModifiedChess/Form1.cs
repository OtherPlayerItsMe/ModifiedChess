using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModifiedChess
{
    public partial class Form1 : Form
    {
        public Button[,] buttons = new Button[16,16];
        public Image chessSprite;
        public Button prevButton;
        public int currPlayer;
        public bool isMoving = false;
        public int[,] map = new int[16, 16];
        public int isLifeKIng = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chessSprite = new Bitmap("C:\\Users\\nikip.LAPTOP-C46P1V71\\Desktop\\Game\\ModifiedChess\\sprite\\chess2.png");
            Image part = new Bitmap(50, 50);
            Graphics graphic = Graphics.FromImage(part);
            graphic.DrawImage(chessSprite, new Rectangle(0, 0, 50, 50), 0, 0, 150, 150, GraphicsUnit.Pixel);
            Initalization();
        }

        public void Initalization()
        {
        map = new int[16, 16]
        {
            {15,31,17,0,18,32,11,32,13,14,11,0,15,0,31,15 },
            {31,18,0,0,0,18,31,14,32,12,32,18,16,18,17,31 },
            {32,0,0,0,0,0,18,11,12,31,0,0,18,0,18,0 },
            {0,0,0,0,0,0,0,18,31,0,31,0,0,0,0,0 },
            {17,16,31,0,0,0,0,0,18,31,0,0,0,31,16,17},
            {11,31,0,0,0,0,0,0,0,18,0,0,0,0,31,11 },
            {32,0,0,0,0,0,0,0,0,0,0,0,0,0,0,32 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {32,0,0,0,0,0,0,0,0,0,0,0,0,0,0,32 },
            {21,31,0,0,0,0,0,0,0,0,0,0,0,0,31,21 },
            {27,26,31,0,0,0,0,28,0,0,0,0,0,31,26,27 },
            {0,0,0,0,0,0,0,31,28,0,0,0,0,0,0,0 },
            {0,28,0,28,0,0,31,22,21,28,0,0,0,0,0,32 },
            {31,27,28,26,28,32,22,32,24,31,28,0,0,0,28,31 },
            {25,31,0,25,0,21,24,23,32,21,32,28,0,27,31,25 }
        };
        currPlayer = 1;
        CreateMap();
        }

        public void CreateMap()
        {
            for (int i = 0; i < 16; i ++)
            { 
                for (int j = 0; j < 16; j++)
                {
                    buttons[i, j] = new Button();

                    Button newButton = new Button
                    {
                        Size = new Size(50, 50),
                        Location = new Point(j * 50, i * 50)
                    };

                    switch (map[i,j] / 10)
                    {
                        case 1:
                            Image part = new Bitmap(50, 50);
                            Graphics graphic = Graphics.FromImage(part);
                            graphic.DrawImage(chessSprite, new Rectangle(0, 0, 50, 50), 0 + 150 * (map[i,j]%10 -1) , 0, 150, 150, GraphicsUnit.Pixel);
                            newButton.BackgroundImage = part;
                            break;
                        case 2:
                            Image part1 = new Bitmap(50, 50);
                            Graphics graphic1 = Graphics.FromImage(part1);
                            graphic1.DrawImage(chessSprite, new Rectangle(0, 0, 50, 50), 0 + 150 * (map[i, j] % 10 - 1), 150, 150, 150, GraphicsUnit.Pixel);
                            newButton.BackgroundImage = part1;
                            break;
                        case 3: 
                            if (map[i, j] % 10 == 1)
                            {
                                Image part2 = new Bitmap(50, 50);
                                Graphics graphic2 = Graphics.FromImage(part2);
                                graphic2.DrawImage(chessSprite, new Rectangle(0, 0, 50, 50), 0 + 150 * (8), 0, 150, 150, GraphicsUnit.Pixel);
                                newButton.BackgroundImage = part2;
                                break;
                            }
                            else
                            {
                                Image part3 = new Bitmap(50, 50);
                                Graphics graphic3 = Graphics.FromImage(part3);
                                graphic3.DrawImage(chessSprite, new Rectangle(0, 0, 50, 50), 0 + 150 * (8), 150, 150, 150, GraphicsUnit.Pixel);
                                newButton.BackgroundImage = part3;
                                break;
                            }

                    };
                    newButton.BackColor = Color.White;
                    newButton.Click += new EventHandler(OnFigurePress);
                    this.Controls.Add(newButton);
                    buttons[i, j] = newButton;
                    
                }
            }
        }

        public void OnFigurePress(object sender, EventArgs eventPress)
        {
            if (prevButton != null)
            {
                prevButton.BackColor = Color.White;
            }
            Button pressedButton = sender as Button;

            if (map[pressedButton.Location.Y / 50, pressedButton.Location.X / 50] != 0 && map[pressedButton.Location.Y / 50, pressedButton.Location.X / 50] / 10 == currPlayer)
            {
                CloseSteps();
                pressedButton.BackColor = Color.Red;
                Deactivate();
                pressedButton.Enabled = true;
                ShowSteps(pressedButton.Location.Y / 50, pressedButton.Location.X / 50, map[pressedButton.Location.Y / 50, pressedButton.Location.X / 50]);

                if (isMoving)
                {
                    CloseSteps();
                    pressedButton.BackColor = Color.White;
                    Activate();
                    isMoving = false;
                }
                else
                    isMoving = true;
            }
            else
            {
                if (isMoving)
                {
                    int temp = map[pressedButton.Location.Y / 50, pressedButton.Location.X / 50];
                    map[pressedButton.Location.Y / 50, pressedButton.Location.X / 50] = map[prevButton.Location.Y / 50, prevButton.Location.X / 50];
                    map[prevButton.Location.Y / 50, prevButton.Location.X / 50] = temp;
                    pressedButton.BackgroundImage = prevButton.BackgroundImage;
                    prevButton.BackgroundImage = null;
                    isMoving = false;
                    CloseSteps();
                    Activate();
                    SwitchPlayer();
                }
            }
            prevButton = pressedButton;
            
        }

        public void SwitchPlayer()
        {
            if (currPlayer == 1)
                currPlayer = 2;
            else currPlayer = 1;
            
        }

        public void ShowSteps(int IcurrFigure, int JcurrFigure, int currFigure)
        {
            int dir = currPlayer == 1 ? 1 : -1;
            switch (currFigure % 10)
            {
                case 1:
                    ShowKnightSteps(IcurrFigure, JcurrFigure);
                    break;
                case 2:
                    ShowJesterSteps(IcurrFigure, JcurrFigure);
                    break;
                case 3:
                    ShowVerticalHorizontal(IcurrFigure, JcurrFigure, true);
                    ShowDiagonal(IcurrFigure, JcurrFigure, true);
                    break;
                case 4:
                    ShowVerticalHorizontal(IcurrFigure, JcurrFigure);
                    ShowDiagonal(IcurrFigure, JcurrFigure);
                    break;
                case 5:
                    ShowDiagonal(IcurrFigure, JcurrFigure);
                    break;
                case 6:
                    ShowHorseSteps(IcurrFigure, JcurrFigure);
                    break;
                case 7:
                    ShowVerticalHorizontal(IcurrFigure, JcurrFigure);
                    break;
                case 8:
                    if (InsideBorder(IcurrFigure + 1 * dir, JcurrFigure))
                    {
                        if (map[IcurrFigure + 1 * dir, JcurrFigure] == 0)
                        {
                            buttons[IcurrFigure + 1 * dir, JcurrFigure].BackColor = Color.Yellow;
                            buttons[IcurrFigure + 1 * dir, JcurrFigure].Enabled = true;
                        }
                    }

                    CanEat(IcurrFigure, JcurrFigure, dir, 1);
                    CanEat(IcurrFigure, JcurrFigure, dir, -1);
                    break;
            }
        }

        private void ShowKnightSteps(int IcurrFigure, int JcurrFigure)
        {
            CanJump(IcurrFigure, JcurrFigure, 1, 1);
            CanJump(IcurrFigure, JcurrFigure, -1, -1);
            CanJump(IcurrFigure, JcurrFigure, -1, 1);
            CanJump(IcurrFigure, JcurrFigure, 1, -1);
            CanJump(IcurrFigure, JcurrFigure, 2, 0);
            CanJump(IcurrFigure, JcurrFigure, 0, 2);
            CanJump(IcurrFigure, JcurrFigure, -2, 0);
            CanJump(IcurrFigure, JcurrFigure, 0, -2);
        }

        private void ShowJesterSteps(int IcurrFigure, int JcurrFigure)
        {
            CanJump(IcurrFigure, JcurrFigure, 2, 0);
            CanJump(IcurrFigure, JcurrFigure, -2, 0);
            CanJump(IcurrFigure, JcurrFigure, 0, 2);
            CanJump(IcurrFigure, JcurrFigure, 0, -2);
        }

        private void CanEat(int IcurrFigure, int JcurrFigure, int dir, int dir2)
        {
            if (InsideBorder(IcurrFigure + 1 * dir, JcurrFigure + dir2))
            {
                if (map[IcurrFigure + 1 * dir, JcurrFigure + 1] != 0 && map[IcurrFigure + 1 * dir, JcurrFigure + 1] / 10 != currPlayer && map[IcurrFigure + 1 * dir, JcurrFigure + 1] / 10 != 3)
                {
                    if (map[IcurrFigure + 1 * dir, JcurrFigure + 1] / 10 != currPlayer && map[IcurrFigure + 1 * dir, JcurrFigure + 1] % 10 == 3)
                        Restart();
                    else
                    {
                        buttons[IcurrFigure + 1 * dir, JcurrFigure + dir2].BackColor = Color.Yellow;
                        buttons[IcurrFigure + 1 * dir, JcurrFigure + dir2].Enabled = true;
                    }
                }
            }
        }

        public void ShowHorseSteps(int IcurrFigure, int JcurrFigure)
        {
            CanJump(IcurrFigure, JcurrFigure, -2, 1);
            CanJump(IcurrFigure, JcurrFigure, -2, -1);
            CanJump(IcurrFigure, JcurrFigure, 2, 1);
            CanJump(IcurrFigure, JcurrFigure, 2, -1);
            CanJump(IcurrFigure, JcurrFigure, -1, 2);
            CanJump(IcurrFigure, JcurrFigure, 1, 2);
            CanJump(IcurrFigure, JcurrFigure, -1, 2);
            CanJump(IcurrFigure, JcurrFigure, -1, -2);
            CanJump(IcurrFigure, JcurrFigure, -1, 2);
            CanJump(IcurrFigure, JcurrFigure, 1, -2);
        }

        private void CanJump(int IcurrFigure, int JcurrFigure, int one, int two)
        {
            if (InsideBorder(IcurrFigure + one, JcurrFigure + two))
            {
                DeterminePath(IcurrFigure + one, JcurrFigure + two);
            }
        }

        public bool InsideBorder(int ti, int tj)
        {
            if (ti >= 16 || tj >= 16 || ti < 0 || tj < 0)
                return false;
            return true;
        }

        public void CloseSteps()
        {
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    buttons[i, j].BackColor = Color.White;
                }
            }
        }

        public void ShowDiagonal(int IcurrFigure, int JcurrFigure, bool isOneStep = false)
        {
            int j = JcurrFigure + 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (InsideBorder(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j < 15)
                    j++;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure - 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (InsideBorder(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j > 0)
                    j--;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure - 1;
            for (int i = IcurrFigure + 1; i < 16; i++)
            {
                if (InsideBorder(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j > 0)
                    j--;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure + 1;
            for (int i = IcurrFigure + 1; i < 16; i++)
            {
                if (InsideBorder(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j < 15)
                    j++;
                else break;

                if (isOneStep)
                    break;
            }
        }

        public void ShowVerticalHorizontal(int IcurrFigure, int JcurrFigure, bool isOneStep = false)
        {
            for (int i = IcurrFigure + 1; i < 16; i++)
            {
                if (InsideBorder(i, JcurrFigure))
                {
                    if (!DeterminePath(i, JcurrFigure))
                        break;
                }
                if (isOneStep)
                    break;
            }
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (InsideBorder(i, JcurrFigure))
                {
                    if (!DeterminePath(i, JcurrFigure))
                        break;
                }
                if (isOneStep)
                    break;
            }
            for (int j = JcurrFigure + 1; j < 16; j++)
            {
                if (InsideBorder(IcurrFigure, j))
                {
                    if (!DeterminePath(IcurrFigure, j))
                        break;
                }
                if (isOneStep)
                    break;
            }
            for (int j = JcurrFigure - 1; j >= 0; j--)
            {
                if (InsideBorder(IcurrFigure, j))
                {
                    if (!DeterminePath(IcurrFigure, j))
                        break;
                }
                if (isOneStep)
                    break;
            }
        }

        public bool DeterminePath(int IcurrFigure, int j)
        {
            if (map[IcurrFigure, j] == 0)
            {
                buttons[IcurrFigure, j].BackColor = Color.Yellow;
                buttons[IcurrFigure, j].Enabled = true;
            }
            else
            {
                if (map[IcurrFigure, j] / 10 != currPlayer && map[IcurrFigure, j] / 10 != 3)
                {
                    if (map[IcurrFigure, j] % 10 == 3)
                        Restart();
                    else
                    {
                        buttons[IcurrFigure, j].BackColor = Color.Yellow;
                        buttons[IcurrFigure, j].Enabled = true;
                    }
                }
                return false;
            }
            return true;
        }

        public new void Activate()
        {

            for (int i = 0; i < 16; i++)
                for (int j = 0; j < 16; j++)
                {
                    buttons[i, j].Enabled = true;
                }
        }

        public new void Deactivate()
        {
            for (int i = 0; i < 16; i++)
                for (int j = 0; j < 16; j++)
                {
                    buttons[i, j].Enabled = false;
                }
        }

        private void Restart()
        {
            this.Controls.Clear();
            this.Controls.Add(button1);
            Initalization();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            this.Controls.Add(button1);
            Initalization();
        }
    }
}

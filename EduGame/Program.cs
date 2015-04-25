using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace EduGame
{
    sealed class Game : Form
    {
        private const int cellWidth = 70;
        private const int cellHeight = 70;

        private readonly int[,] _valueMatr;
        private readonly IList<Action> actions;
        private readonly int[,] valueMatr;
        private readonly int PauseInterval;
        private readonly int cellsY;
        private readonly int cellsX;

        private int points = 10;
        private int steps;
        private int X, Y;

        private PictureBox[,] valueImages;
        private PictureBox heroBox;

        private Panel panel;
        private Label pointsLavel;
        private Label stepsLabel;
        private Image heroImg;

        public Game(int[,] valueMatr, IList<Action> actions, int pauseInterval)
        {
            _valueMatr = valueMatr;
            this.actions = actions;

            cellsY = valueMatr.GetLength(0);
            cellsX = valueMatr.GetLength(1);

            X = Y = 0;
            PauseInterval = pauseInterval;

            AutoSize = true;

            panel = new Panel
            {
                Parent = this,
                Size = new Size(cellsX * cellWidth, cellsY * cellHeight),
                Dock = DockStyle.Fill,
                BackColor = Color.DarkOliveGreen
            };

            InitStatsPanel();
            InitValues(valueMatr);
            InitHero();
            RefreshFields();
        }

        private void InitStatsPanel()
        {
            var font = new Font("Arial", 9, FontStyle.Bold);
            var statsPanel = new FlowLayoutPanel
            {
                Parent = this,
                Dock = DockStyle.Top,
                Height = 20,
                BackColor = Color.LightSlateGray,
                ForeColor = Color.White
            };

            new Label
            {
                Text = "Набрано очков:",
                Parent = statsPanel,
                AutoSize = true,
                Font = font
            };

            pointsLavel = new Label
            {
                Text = points.ToString(),
                Parent = statsPanel,
                AutoSize = true,
                Font = font
            };

            new Label
            {
                Text = "Сделано шагов:",
                Parent = statsPanel,
                AutoSize = true,
                Font = font
            };

            stepsLabel = new Label
            {
                Text = steps.ToString(),
                Parent = statsPanel,
                AutoSize = true,
                Font = font
            };
        }

        private void InitValues(int[,] valueMatr)
        {
            var valueImg = Image.FromFile("value.png");

            valueImages = new PictureBox[cellsY, cellsX];

            for (var i = 0; i < cellsY; i++)
            {
                for (var j = 0; j < cellsX; j++)
                {
                    if (valueMatr[i, j] != 1) continue;
                    var pictureBox = new PictureBox
                    {
                        Image = valueImg,
                        Location = new Point(j * cellWidth, i * cellHeight),
                        Width = cellWidth,
                        Height = cellHeight,
                        SizeMode = PictureBoxSizeMode.CenterImage,
                        Parent = panel
                    };
                    valueImages[i, j] = pictureBox;
                }
            }
        }

        private void InitHero()
        {
            heroImg = Image.FromFile("hero.png");
            heroBox = new PictureBox
            {
                Image = heroImg,
                Location = new Point(X * cellWidth, Y * cellHeight),
                Width = cellWidth,
                Height = cellHeight,
                SizeMode = PictureBoxSizeMode.Zoom,
                Parent = panel
            };
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var i = 0;
            var timer = new Timer {Interval = PauseInterval};
            timer.Tick += (sender, args) =>
            {
                if (i < actions.Count)
                {
                    actions[i]();
                    i++;
                }
                else
                {
                    timer.Stop();
                }
            };
            timer.Start();
        }

        private void RefreshFields()
        {
            if (valueImages[Y, X] != null && _valueMatr[Y, X] == 1)
            {
                valueImages[Y, X].Parent = null;
                valueImages[Y, X] = null;
                points++;
            }
            else
            {
                points--;
            }
            steps++;

            var dx = (X*cellWidth - heroBox.Location.X);
            var dy = (Y*cellHeight - heroBox.Location.Y);
            heroBox.Location = new Point(heroBox.Location.X + dx, heroBox.Location.Y + dy);
            pointsLavel.Text = points.ToString();
            stepsLabel.Text = steps.ToString();
        }

        private void SetRotateHero(int angle)
        {
            var img = (Image)heroImg.Clone();
            img.RotateFlip(angle.FromInt());
            heroBox.Image = img;
        }

        public void Top()
        {
            if (Y == 0) return;
            Y--;
            SetRotateHero(180);
            RefreshFields();
        }
        public void Down()
        {
            if (Y == cellsY - 1) return;
            Y++;
            SetRotateHero(0);
            RefreshFields();
        }
        public void Left()
        {
            if (X == 0) return;
            X--;
            SetRotateHero(90);
            RefreshFields();
        }
        public void Right()
        {
            if (X == cellsX - 1) return;
            X++;
            SetRotateHero(270);
            RefreshFields();
        }
    }

    static class Program
    {
        
        [STAThread]
        static void Main()
        {
            Игра.УстановитьВеличинуПаузы(200);
            
        }
    }

    public static class Игра
    {
        private static Game game;
        private static IList<Action> actions = new List<Action>();
        private static int pauseInterval = 1000;

        public static dynamic Запустить()
        {
            Application.Run(game = new Game(new[,]
            {
                {0, 0, 0, 0, 1, 1, 1, 0, 1, 0, 0},
                {0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0},
                {0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0},
                {0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0},
                {0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0},
                {0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0},
                {0, 0, 1, 0, 1, 1, 1, 0, 0, 0, 0}
            }, actions, pauseInterval));
            return null;
        }

        public static dynamic УстановитьВеличинуПаузы(dynamic интервал)
        {
            pauseInterval = Convert.ToInt32(Convert.ToDecimal(интервал));
            return null;
        }

        private static dynamic withGUIThread(Action action)
        {
            actions.Add(action);
            return null;
        }

        public static dynamic Вправо()
        {
            return withGUIThread(() => game.Right());
        }
        public static dynamic Влево()
        {
            return withGUIThread(() => game.Left());
        }
        public static dynamic Вниз()
        {
            return withGUIThread(() => game.Down());
        }
        public static dynamic Вверх()
        {
            return withGUIThread(() => game.Top());
        }
    }

    public static class RotateFlipTypeExtension
    {
        public static RotateFlipType FromInt(this int angle)
        {
            angle = angle < 0 ? 360 - angle : angle;
            angle = angle%360;
            switch (angle)
            {
                case 0:
                    return RotateFlipType.RotateNoneFlipNone;
                case 90:
                    return RotateFlipType.Rotate90FlipNone;
                case 180:
                    return RotateFlipType.Rotate180FlipNone;
                case 270:
                    return RotateFlipType.Rotate270FlipNone;
            }
            
            throw new Exception("Unsupported angle: " + angle);
        }
    }
}

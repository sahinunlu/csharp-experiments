using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace snakegame
{
    public partial class FrmMain : Form
    {
        private Snake Snake = new Snake();
        private Timer Timer = new Timer();
        private Food Food = new Food();

        public FrmMain()
        {
            InitializeComponent();
            InitializeTimer();
            InitializeScene();
        }

        private void InitializeScene()
        {
            Food.Location = new Point(400, 400);
            scene.Width = Constants.Size;
            scene.Height = Constants.Size;
            Snake.Collision += new EventHandler(OnCollision);
        }

        private void InitializeTimer()
        {
            Timer.Interval = Constants.TimerInterval;
            Timer.Tick += T_Tick;
            Timer.Start();
        }

        private void OnCollision(object sender, EventArgs e)
        {
            Timer.Enabled = false;
        }

        private void T_Tick(object sender, EventArgs e)
        {
            if (CheckFood())
            {
                Snake.EatFood();
                LocateFood();
            }

            Snake.Move(Snake.CurrentDirection);
            Draw();
        }

        private void LocateFood()
        {
            bool onSnake = true;

            while (onSnake)
            {
                Random rX = new Random();
                Food.Location.X = rX.Next(Constants.Size / Constants.NodeSize) * Constants.NodeSize;

                Random rY = new Random();
                Food.Location.Y = rY.Next(Constants.Size / Constants.NodeSize) * Constants.NodeSize;

                if (Food.Location.X <= 0)
                {
                    Food.Location.X += Constants.Size + 20;
                }

                if (Food.Location.Y <= 0)
                {
                    Food.Location.Y += Constants.Size + 20;
                }

                if (Food.Location.Y >= Constants.Size)
                {
                    Food.Location.Y -= Constants.Size-20;
                }

                if (Food.Location.X >= Constants.Size)
                {
                    Food.Location.X -= Constants.Size-20;
                }

                onSnake = Snake.Nodes.Any(p => p.NewLocation.X == Food.Location.X && p.NewLocation.Y == Food.Location.Y);
            }

        }

        private bool CheckFood()
        {
            if (Snake.Head.Value.OldLocation == Food.Location || Snake.Head.Value.NewLocation == Food.Location)
            {
                return true;
            }

            return false;
        }

        private void Draw()
        {
            if (scene.Image == null)
            {
                scene.Image = new Bitmap(scene.Width, scene.Height);
            }

            using (Graphics g = Graphics.FromImage(scene.Image))
            {
                g.Clear(Color.Black);
                SolidBrush myFoodBrush = new SolidBrush(Color.Red);
                Rectangle myFoodRectangle = new Rectangle(Food.Location.X, Food.Location.Y, Constants.NodeSize, Constants.NodeSize);
                g.FillRectangle(myFoodBrush, myFoodRectangle);

                foreach (var item in Snake.Nodes)
                {
                    Rectangle myNodeRectangle = new Rectangle(item.NewLocation.X, item.NewLocation.Y, Constants.NodeSize, Constants.NodeSize);
                    if (Snake.Nodes.Find(item).Previous == null)
                    {
                        SolidBrush myBrush = new SolidBrush(Color.White);
                        g.FillRectangle(myBrush, myNodeRectangle);
                    }
                    else
                    {
                        SolidBrush myBrush = new SolidBrush(Color.Azure);
                        g.FillRectangle(myBrush, myNodeRectangle);
                    }
                }
            }

            scene.Invalidate();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Down)
            {
                Snake.Move(Direction.Down);
            }

            if (e.KeyData == Keys.Up)
            {
                Snake.Move(Direction.Up);
            }

            if (e.KeyData == Keys.Right)
            {
                Snake.Move(Direction.Rigth);
            }

            if (e.KeyData == Keys.Left)
            {
                Snake.Move(Direction.Left);
            }

            if (e.KeyData == Keys.Space)
            {
                Timer.Enabled = !Timer.Enabled;
            }
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Timer.Stop();
            Timer.Tick -= T_Tick;
            Timer.Dispose();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoxField
{
    public partial class GameScreen : UserControl
    {
        //player1 button control keys - DO NOT CHANGE
        Boolean leftArrowDown, rightArrowDown;

        //used to draw boxes and character on screen
        SolidBrush drawBrush = new SolidBrush(Color.White);

        //Hero object and values
        Box hero;
        int heroSpeed = 4;
        int heroSize = 30;

        Random rand = new Random();

        //Box values        
        List<Box> boxesLeft = new List<Box>();
        List<Box> boxesRight = new List<Box>();

        int leftStartX = 300;
        int gap = 300;

        int newBoxCounter = 5;
        int boxSpeed = 7;
        int boxSize = 30;
        bool patternLeft = true;
        int patternLength = 8;
        int xChange = 7;

        public GameScreen()
        {
            InitializeComponent();
            OnStart();           
        }

        /// <summary>
        /// Set initial game values here
        /// </summary>
        public void OnStart()
        {
            hero = new Box(500, 400, heroSize, Color.White);

            Box b = new Box(leftStartX, 0, boxSize, Color.Red);
            boxesLeft.Add(b);

            b = new Box(leftStartX + gap, 0, boxSize, Color.Red);
            boxesRight.Add(b);
        }

        private void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //player 1 button presses
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;
            }
        }

        private void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            //player 1 button releases
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
            }
        }

        private void gameLoop_Tick(object sender, EventArgs e)
        {
            #region create new box if it is time           
            newBoxCounter--;

            if (newBoxCounter == 0)
            {
                if (patternLength == 0)
                {
                    patternLeft = !patternLeft;  //change direction
                    patternLength = rand.Next(5, 15);  
                    xChange = rand.Next(10, 25);

                    if (patternLeft) { xChange *= -1; } // xChange value made negative to move left                
                }

                // create left and right cubes
                int randValue = rand.Next(1, 4);
                Color c = Color.White;

                if (randValue == 1) { c = Color.Red; }
                else if (randValue == 2) { c = Color.Yellow; }
                else if (randValue == 3) { c = Color.Orange; }

                leftStartX = boxesLeft[boxesLeft.Count() - 1].rec.X + xChange; // set left position

                Box b1 = new Box(leftStartX, 0, boxSize, c);
                Box b2 = new Box(leftStartX + gap, 0, boxSize, c);
                boxesLeft.Add(b1);
                boxesRight.Add(b2);

                newBoxCounter = 5;
                patternLength--;
            }

            #endregion

            #region update position of each box

            foreach (Box b in boxesLeft)
            {
                b.Move(boxSpeed);
            }

            foreach (Box b in boxesRight)
            {
                b.Move(boxSpeed);
            }

            #endregion

            #region Remove boxes from list that have gone off the screen

            if (boxesLeft[0].rec.Y > this.Height)
            {
                boxesLeft.RemoveAt(0);
                boxesRight.RemoveAt(0);
            }

            #endregion

            #region move hero

            if (leftArrowDown)
            {
                hero.Move(heroSpeed, "left");
            }
            else if (rightArrowDown)
            {
                hero.Move(heroSpeed, "right");
            }

            #endregion

            #region check for a collision

            foreach (Box b in boxesLeft.Union(boxesRight))
            {
                if (hero.Collision(b))
                {
                    gameLoop.Stop();
                }
            }
            #endregion

            Refresh();
        }

        //private void GameScreen_KeyPress(object sender, KeyPressEventArgs e)
        //{

        //}

        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            drawBrush.Color = Color.White;
            e.Graphics.FillRectangle(drawBrush, hero.rec);

            foreach (Box b in boxesLeft)
            {
                drawBrush.Color = b.color;
                e.Graphics.FillRectangle(drawBrush, b.rec);
            }

            foreach (Box b in boxesRight)
            {
                drawBrush.Color = b.color;
                e.Graphics.FillRectangle(drawBrush, b.rec);
            }
        }
    }
}

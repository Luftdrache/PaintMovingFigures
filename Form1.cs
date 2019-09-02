using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThirdLab
{
    public partial class Form1 : Form
    {
        List<Point> points = new List<Point>();//List of dots
        List<Point> offsetPoints = new List<Point>();
        List<bool> xOut = new List<bool>();
        List<bool> yOut = new List<bool>();


        bool bCanMakeNewPoint; 
        bool isClosedCurve; 
        bool isBrokenLine; 
        bool isFillCurve; 
        bool isBezier; 
        Timer timer = new Timer();
        bool isMoving;
        public int moveCounter = 1; 
        Point speed  = new Point(3, 3); 
        int startXposition;
        bool bDrag; 
        int iPointToDrag;
        Random rand = new Random();
        bool isXOutOfScreen;
        bool isYOutOfScreen;
       

        public Color dotColor = Color.DarkOrange;
        public Color lineColor = Color.DarkOrange;
        public int dotSize = 12;
        public int lineWidth = 5;
  
        public Form1()
        {
            InitializeComponent();

            #region Window's settings 
            Text = "Paint It!";
            BackColor = Color.Lavender;
            Height = Screen.PrimaryScreen.Bounds.Height / 2 +300;
            Width = Screen.PrimaryScreen.Bounds.Width / 2 +200;
            MinimumSize = new Size(300, 300);
            MaximumSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            CenterToScreen();
            DoubleBuffered = true;
            #endregion

            #region Buttons

            FlowLayoutPanel pane = new FlowLayoutPanel()
            {
                BackColor = Color.YellowGreen,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.TopDown,
                Dock = DockStyle.Left,
                BorderStyle = BorderStyle.Fixed3D,
            };

            Button buttonPoints = new Button()
            {
                Text = "ТОЧКИ",
                Size = new Size(100, 50),
                Font = new Font("Times New Roman", 16, FontStyle.Bold),
                BackColor = Color.YellowGreen,
                AutoSize = true,
                Margin = new Padding(10, 10, 10, 0)
            };


            Button buttonClosedCurve = new Button()
            {
                Text = "КРИВАЯ",
                Size = new Size(100, 50),
                Font = new Font("Times New Roman", 16, FontStyle.Bold),
                BackColor = Color.YellowGreen,
                AutoSize = true,
                Margin = new Padding(10, 10, 10, 0)
            };

            Button buttonFillCurve = new Button()
            {
                Text = "ЗАПОЛНЕННАЯ",
                Size = new Size(100, 50),
                Font = new Font("Times New Roman", 16, FontStyle.Bold),
                BackColor = Color.YellowGreen,
                AutoSize = true,
                Margin = new Padding(10, 10, 10, 0)
            };

            Button buttonBrokenLine = new Button()
            {
                Text = "ЛОМАННАЯ",
                Size = new Size(100, 50),
                Font = new Font("Times New Roman", 16, FontStyle.Bold),
                BackColor = Color.YellowGreen,
                AutoSize = true,
                Margin = new Padding(10, 10, 10, 0)
            };

            Button buttonBezier = new Button()
            {
                Text = "БЕЗЬЕ",
                Size = new Size(100, 50),
                Font = new Font("Times New Roman", 16, FontStyle.Bold),
                BackColor = Color.YellowGreen,
                AutoSize = true,
                Margin = new Padding(10, 10, 10, 0)
            };

            Button buttonMoving = new Button()
            {
                Text = "ДВИЖЕНИЕ",
                Size = new Size(100, 50),
                Font = new Font("Times New Roman", 16, FontStyle.Bold),
                BackColor = Color.CadetBlue,
                AutoSize = true,
                Margin = new Padding(10, 30, 10, 0)
            };

            Button buttonClear = new Button()
            {
                Text = "ОЧИСТИТЬ ЭКРАН",
                Size = new Size(100, 50),
                Font = new Font("Times New Roman", 16, FontStyle.Bold),
                BackColor = Color.CadetBlue,
                AutoSize = true,
                Margin = new Padding(10, 10, 10, 0),
            };

            Button buttonParam = new Button()
            {
                Text = "ПАРАМЕТРЫ",
                Size = new Size(100, 50),
                Font = new Font("Times New Roman", 14, FontStyle.Regular),
                AutoSize = true,
                Margin = new Padding(40, 50, 10, 10),
                FlatStyle = FlatStyle.System,
                BackColor = Color.PapayaWhip
            };

            pane.Controls.Add(buttonPoints);
            pane.Controls.Add(buttonClosedCurve);
            pane.Controls.Add(buttonBrokenLine);
            pane.Controls.Add(buttonFillCurve);
            pane.Controls.Add(buttonBezier);
            pane.Controls.Add(buttonMoving);
            pane.Controls.Add(buttonClear);
            pane.Controls.Add(buttonParam);
            Controls.Add(pane);
            startXposition = pane.Width;

            //обработчики нажатий на кнопки
            buttonPoints.Click += ButtonPoints_Click;
            buttonClear.Click += ButtonClear_Click;
            buttonClosedCurve.Click += ButtonClosedCurve_Click;
            buttonBrokenLine.Click += ButtonBrokenLine_Click;
            buttonFillCurve.Click += ButtonFillCurve_Click;
            buttonBezier.Click += ButtonBezier_Click;
            buttonMoving.Click += ButtonMoving_Click;
            buttonParam.Click += ButtonParam_Click;

            #endregion

            Paint += Form1_Paint;

            timer.Interval = 30;
            timer.Tick += Timer_Tick;

            KeyPreview = true;
            KeyDown += Form1_KeyDown;
  
            MouseClick += Form1_MouseClick;
            MouseDown += Form1_MouseDown;
            MouseMove += Form1_MouseMove;
            MouseUp += Form1_MouseUp;  
        }


        #region Mouse

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (bCanMakeNewPoint) //Draw a point, if allowed
            {
                if (e.Button == MouseButtons.Left)
                {
                    Point p = e.Location;
                    points.Add(p);
                    Point ofst = new Point(rand.Next(5), rand.Next(5));
                    offsetPoints.Add(ofst);
                    xOut.Add(false);
                    yOut.Add(false);
                    Refresh();
                }
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            bDrag = false;
        }


        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i <points.Count(); i++)
            {
                Point p = points[i];
                if ((e.Location.X <= p.X + 10 && e.Location.X >= p.X - 10) && (e.Location.Y <= p.Y + 10 && e.Location.Y >= p.Y - 10))
                {
                    bDrag = true;
                    iPointToDrag = i;
                }
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if(bDrag)
            {
                points[iPointToDrag] = e.Location;
                Refresh();
            }
        }


        #endregion

        #region Clicks

        //Choice of parameters
        private void ButtonParam_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Owner = this;
            form2.Show();
        }

        //Bezier
        private void ButtonBezier_Click(object sender, EventArgs e)
        {
            isClosedCurve = false;
            isBrokenLine = false;
            isFillCurve = false;
            bCanMakeNewPoint = false;
            isBezier = !isBezier;
            Refresh();
        }
        //Filled curve
        private void ButtonFillCurve_Click(object sender, EventArgs e)
        {
            bCanMakeNewPoint = false;
            isClosedCurve = false;
            isBrokenLine = false;
            isBezier = false;
            isFillCurve = !isFillCurve;
            Refresh();
        }

        //Broken line
        private void ButtonBrokenLine_Click(object sender, EventArgs e)
        {
            bCanMakeNewPoint = false;
            isClosedCurve = false;
            isFillCurve = false;
            isBezier = false;
            isBrokenLine = !isBrokenLine;
            Refresh();
        }

        //Closed curve
        private void ButtonClosedCurve_Click(object sender, EventArgs e)
        {
            isBrokenLine = false;
            bCanMakeNewPoint = false;
            isFillCurve = false;
            isBezier = false;
            isClosedCurve = !isClosedCurve;
            Refresh();
        }

        //Reaction to a click
        private void ButtonPoints_Click(object sender, EventArgs e)
        {
            bCanMakeNewPoint = true;
            isBrokenLine = false;
            isFillCurve = false;
            isBezier = false;
            isClosedCurve = false;
            isMoving = false ;
            timer.Stop();

            Refresh();
        }

        //Clear screen
        private void ButtonClear_Click(object sender, EventArgs e)
        {
            points.Clear();
            isMoving = false;
            speed.X = speed.Y = 3; 
            Refresh();
        }

        //Movement
        private void ButtonMoving_Click(object sender, EventArgs e)
        {
            bCanMakeNewPoint = false;
            isMoving = !isMoving;
            if (isMoving)
            {
                timer.Start();
            }
            else
                timer.Stop();
        }
        #endregion

        //Timer
        private void Timer_Tick(object sender, EventArgs e)
        {
            int x, y = 0;

            switch (moveCounter)
            {
                //Normal movement
                case 1:
                    for (int i = 0; i < points.Count; i++)
                    {
                        if (points[i].X >= Width - 10 || points[i].X < startXposition + 10) isXOutOfScreen = true;
                        if (points[i].Y >= ClientRectangle.Height - 10 || points[i].Y <= 10) isYOutOfScreen = true;
                    }

                    for (int i = 0; i < points.Count; i++)
                    {
                        if (isXOutOfScreen == false)
                            x = points[i].X + speed.X;
                        else
                        {
                            speed = new Point(-speed.X, speed.Y);
                            x = points[i].X + speed.X;
                            isXOutOfScreen = false;
                        }
                        if (isYOutOfScreen == false)
                            y = points[i].Y + speed.Y;
                        else
                        {
                            speed = new Point(speed.X, -speed.Y);
                            y = points[i].Y + speed.Y;
                            isYOutOfScreen = false;
                        }

                        points[i] = new Point(x, y);
                    }
                    break;

                //Chaotic movement
                case 2:
                    for (int i = 0; i < points.Count; i++)
                    {
                        if (points[i].X >= Width - 10 || points[i].X < startXposition + 10) xOut[i] = true;
                        if (points[i].Y >= ClientRectangle.Height - 10 || points[i].Y <= 10) yOut[i] = true;
                    }

                    for (int i = 0; i < points.Count; i++)
                    {
                        if (xOut[i] == false)
                        {
                            x = points[i].X + offsetPoints[i].X;
                        }
                        else    
                        {
                            offsetPoints[i] = new Point(-offsetPoints[i].X, offsetPoints[i].Y);
                            x = points[i].X + offsetPoints[i].X;
                            if (x > Width - 10) x = Width - 10;
                            if (x < startXposition + 10) x = startXposition + 10;
                            xOut[i] = false;
                        }


                        if (yOut[i] == false)
                        {
                            y = points[i].Y + offsetPoints[i].Y;
                        }
                        else  
                        {
                            offsetPoints[i] = new Point(offsetPoints[i].X, -offsetPoints[i].Y);
                            y = points[i].Y + offsetPoints[i].Y;
                            if (y >= ClientRectangle.Height - 10) y = ClientRectangle.Height - 10;
                            if (y < 10) y = 10;
                            yOut[i] = false;
                        }
                        
                        points[i] = new Point(x, y);
                    }
                    break;
            }

            Refresh();
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            switch (e.KeyCode)
            {
                case (Keys.Escape):
                    points.Clear();
                    isMoving = !isMoving;
                    speed.X = speed.Y = 3;
                    Refresh();
                    break;
                case (Keys.Oemplus):

                    if (moveCounter == 1)
                    {
                        if (speed.X < 25)
                        {
                            if (speed.X > 0) speed.X++;
                            else speed.X--;
                            if (speed.Y > 0) speed.Y++;
                            else speed.Y--;
                        }
                    }
                    else if(moveCounter==2)
                    {

                            for (int i = 0; i < offsetPoints.Count; i++)
                            {

                            if (offsetPoints[i].X > 0 || offsetPoints[i].Y > 0)
                            {
                                offsetPoints[i] = new Point(offsetPoints[i].X + speed.X, offsetPoints[i].Y + speed.Y);
                            }
                            else if (offsetPoints[i].X < 0 || offsetPoints[i].Y < 0)
                            {
                                offsetPoints[i] = new Point(offsetPoints[i].X - speed.X, offsetPoints[i].Y - speed.Y);
                            }
                            else if (offsetPoints[i].X < 0 || offsetPoints[i].Y > 0)
                            {
                                offsetPoints[i] = new Point(offsetPoints[i].X - speed.X, offsetPoints[i].Y + speed.Y);
                            }
                            else if (offsetPoints[i].X > 0 || offsetPoints[i].Y < 0)
                            {
                                offsetPoints[i] = new Point(offsetPoints[i].X + speed.X, offsetPoints[i].Y - speed.Y);
                            }

                        }
                    }

                    break;

                case (Keys.OemMinus):
                    if (moveCounter == 1)
                    {    
                        if (speed.X > 0) speed.X--;
                        else speed.X++;
                        if (speed.Y > 0) speed.Y--;
                        else speed.Y++;
                    }
                    else if(moveCounter ==2)
                    {
                        if (speed.X > 1)
                        {
                            for (int i = 0; i < offsetPoints.Count; i++)
                            {
                                if (offsetPoints[i].X > 0 || offsetPoints[i].Y>0)
                                {
                                    offsetPoints[i] = new Point(offsetPoints[i].X - speed.X*2, offsetPoints[i].Y - speed.Y*2);
                                }
                                else if(offsetPoints[i].X < 0 || offsetPoints[i].Y < 0)
                                {
                                    offsetPoints[i] = new Point(offsetPoints[i].X + speed.X*2, offsetPoints[i].Y + speed.Y*2);
                                }
                                else if (offsetPoints[i].X < 0 || offsetPoints[i].Y > 0)
                                {
                                    offsetPoints[i] = new Point(offsetPoints[i].X + speed.X*2, offsetPoints[i].Y - speed.Y*2);
                                }
                                else if (offsetPoints[i].X > 0 || offsetPoints[i].Y < 0)
                                {
                                    offsetPoints[i] = new Point(offsetPoints[i].X - speed.X*2, offsetPoints[i].Y + speed.Y*2);
                                }
                            }
                        }
                    }

                    break;
                case (Keys.Space):
                    bCanMakeNewPoint = false;

                    isMoving = !isMoving;
                    if (isMoving)
                    {
                        timer.Start();
                    }
                    else
                        timer.Stop();
                    e.Handled = true;
                    break;
            }
            e.Handled = true;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
           
            bool isRight = true;
            bool isLeft = true;
            bool isUp = true;
            bool isDown = true;
            for(int i = 0; i < points.Count; i++)
            {
                if (points[i].X >= Width) isRight = false;
                if (points[i].X < startXposition) isLeft = false;
                if (points[i].Y >= this.ClientRectangle.Height-10) isDown = false;
                if (points[i].Y <= 0) isUp = false;
            }

            if (!isMoving)
            {
                switch (keyData)
                {

                    case Keys.Up:
                        if (isUp)
                        {
                            for (int i = 0; i < points.Count; i++)
                            {
                                points[i] = new Point(points[i].X, points[i].Y - 10);
                            }
                            Refresh();
                        }
                        return true; 
                    case Keys.Down:
                        if (isDown)
                        {
                            for (int i = 0; i < points.Count; i++)
                            {
                                points[i] = new Point(points[i].X, points[i].Y + 10);
                            }
                            Refresh();
                        }
                        return true;
 
                    case Keys.Right:
                        if (isRight)
                        {
                            for (int i = 0; i < points.Count; i++)
                            {
                                points[i] = new Point(points[i].X + 10, points[i].Y);
                            }
                            Refresh();
                        }
                        return true;
                    case Keys.Left:
                        if (isLeft)
                        {
                            for (int i = 0; i < points.Count; i++)
                            {
                                points[i] = new Point(points[i].X - 10, points[i].Y);
                            }
                            Refresh();
                        }
                        return true;
                }
                return base.ProcessCmdKey(ref msg, keyData); 
            }
            else
            {
                switch(keyData) {
                    case Keys.Up:
                        if (moveCounter == 1)
                        {
                            if (speed.Y > 0) speed.Y = -(speed.Y += 5);
                            else if (speed.Y < 0) speed.Y = speed.Y -= 5;
                        }

                        else if (moveCounter == 2)
                        {
                            for (int i = 0; i < offsetPoints.Count; i++)
                            {
                                if(offsetPoints[i].Y >0) 
                                offsetPoints[i] = new Point(offsetPoints[i].X, -offsetPoints[i].Y-10);
                                else offsetPoints[i] = new Point(offsetPoints[i].X, offsetPoints[i].Y - 10);
                            }
                           
                        }
                        return true;
                    case Keys.Down:
                        if (moveCounter == 1)
                        {
                            if (speed.Y < 0) speed.Y = -(speed.Y -= 5);
                            else if (speed.Y > 0) speed.Y = (speed.Y += 5);
                        }

                        else if (moveCounter == 2)
                        {
                            for (int i = 0; i < offsetPoints.Count; i++)
                            {
                                if (offsetPoints[i].Y > 0)
                                    offsetPoints[i] = new Point(-offsetPoints[i].X, offsetPoints[i].Y + 10);
                                else if(offsetPoints[i].Y < 0)
                                    offsetPoints[i] = new Point(offsetPoints[i].X, -offsetPoints[i].Y + 10);
                            }
                        }
                        return true;
                    case Keys.Right:
                        if (moveCounter == 1)
                        {
                            if (speed.X < 0) speed.X = -(speed.X -= 5);
                            else if (speed.X > 0) speed.X = speed.X += 5;
                        }

                        else if (moveCounter == 2)
                        {
                            for (int i = 0; i < offsetPoints.Count; i++)
                            {
                                if (offsetPoints[i].Y > 0)
                                    offsetPoints[i] = new Point(-offsetPoints[i].X+ 10, -offsetPoints[i].Y);
                                else offsetPoints[i] = new Point(offsetPoints[i].X + 10, -offsetPoints[i].Y);
                            }
                        }
                        return true;
                    case Keys.Left:
                        if (moveCounter == 1)
                        {
                            if (speed.X > 0) speed.X = -(speed.X += 5);
                            else if (speed.X < 0) speed.X = speed.X -= 5;
                        }

                        else if (moveCounter == 2)
                        {
                            for (int i = 0; i < offsetPoints.Count; i++)
                            {
                                if (offsetPoints[i].Y > 0)
                                    offsetPoints[i] = new Point(-offsetPoints[i].X - 10, -offsetPoints[i].Y);
                                else offsetPoints[i] = new Point(offsetPoints[i].X - 10, offsetPoints[i].Y);
                            }
                        }
                        return true;
                }
                return base.ProcessCmdKey(ref msg, keyData); 
            }
        }


        private void Form1_Paint(object sender, PaintEventArgs e) 
        {
            Graphics g = e.Graphics;
           g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Brush brush = new SolidBrush(dotColor); 
            Pen pen = new Pen(lineColor, lineWidth);

            foreach (var p in points)
            {
                g.FillEllipse(brush,  p.X, p.Y, dotSize, dotSize);
            }

            Point[] ptArr = points.ToArray();
            for(int i = 0; i < ptArr.Length; i++)
            {
                ptArr[i].X = ptArr[i].X + dotSize/2;
                ptArr[i].Y = ptArr[i].Y + dotSize/2;
            }

            if (points.Count >= 2) {
                if (isBrokenLine) {
                   g.DrawPolygon(pen, ptArr);
  
                }
                else if (isClosedCurve)
                {
                    try
                    {
                        g.DrawClosedCurve(pen, ptArr);
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
                else if (isFillCurve)
                {
                    g.FillClosedCurve(brush, ptArr);

                }
                else if (isBezier)
                {
                    try { 
                        
                   g.DrawBeziers(pen, ptArr);

                 } catch (Exception)
                    {
                        return;
                    }
                    
                }
            }
           
        }
    }
}
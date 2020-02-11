using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnalogClockWinForms
{
    public partial class Form1 : Form
    {
        Timer mTimer = new Timer();
        bool start = false;
        bool reset = false;
        int secondIntS = 0;
        bool HelperSW = false; 
        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            mTimer.Interval = 1000;
            mTimer.Tick += new EventHandler(OnTimer);
            mTimer.Start();
            Text = "Analog clock";
            SetStyle(ControlStyles.ResizeRedraw, true);
            
        }

        private void DrawHand(Graphics g, SolidBrush solidBrush, int length, bool seen)
        {
            Point[] points = new Point[4];
            points[0].X = 0;
            points[0].Y = -length;
            points[1].X = (seen) ? -2 : -10;
            points[1].Y = 0;
            points[2].X = 0;
            points[2].Y = (seen) ? 2 : 10;
            points[3].X = (seen) ? 2 : 10;
            points[3].Y = 0;
            g.FillPolygon(solidBrush, points);
        }

        private void InitializeTransform(Graphics g, int a, int b)
        {
            g.ResetTransform();
            g.TranslateTransform(ClientSize.Width / a, ClientSize.Height / b);
            float scale = System.Math.Min(ClientSize.Width, ClientSize.Height) / 200.0f;
            g.ScaleTransform(scale, scale);
        }

        //private void InitializeTransformStopWatch(Graphics g)
        //{
        //    g.ResetTransform();
        //    g.TranslateTransform(ClientSize.Width / 4, ClientSize.Height / 4);
        //    float scale = System.Math.Min(ClientSize.Width, ClientSize.Height) / 200.0f;
        //    g.ScaleTransform(scale, scale);
        //}

        private void OnTimer(object sender, EventArgs e)
        {
            HelperSW = true;
            Invalidate();
        }


        

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (HelperSW)
            {
                if (start)
                {
                    secondIntS++;
                }
                if (reset)
                {
                    secondIntS = 0;
                    reset = false;
                }
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                SolidBrush red = new SolidBrush(Color.Red);
                SolidBrush green = new SolidBrush(Color.Green);
                SolidBrush blue = new SolidBrush(Color.Blue);
                SolidBrush white = new SolidBrush(Color.White);
                SolidBrush black = new SolidBrush(Color.Black);
                InitializeTransform(g, 2, 2);
                //draw border
                for (int i = 0; i < 120; i++)
                {
                    g.RotateTransform(5.0f);
                    g.FillRectangle(black, 90, -5, 10, 10);
                }
                //draw hour mark
                for (int i = 0; i < 12; i++)
                {
                    g.RotateTransform(30.0f);
                    g.FillRectangle(white, 85, -5, 10, 10);
                }
                //draw minute mark
                for (int i = 0; i < 60; i++)
                {
                    g.RotateTransform(6.0f);
                    g.FillRectangle(black, 85, -10, 5, 1);
                }
                //get current time
                DateTime nowDateTime = DateTime.Now;
                int secondInt = nowDateTime.Second;
                int minuteInt = nowDateTime.Minute;
                int hourInt = nowDateTime.Hour % 12;
                InitializeTransform(g, 2, 2);
                //hour hand draw
                g.RotateTransform((hourInt * 30) + (minuteInt / 2));
                DrawHand(g, blue, 70, false);
                InitializeTransform(g, 2, 2);
                //minute hand draw
                g.RotateTransform((minuteInt * 6f) + secondInt / 10f);
                DrawHand(g, red, 100, false);
                InitializeTransform(g, 2, 2);
                //second hand draw
                g.RotateTransform(secondInt * 6);
                DrawHand(g, green, 100, true);

                //STOPWATCH

                Graphics gs = e.Graphics;
                gs.SmoothingMode = SmoothingMode.AntiAlias;
                InitializeTransform(gs, 9, 5);

                //draw border
                for (int i = 0; i < 120; i++)
                {
                    gs.RotateTransform(5.0f);
                    gs.FillRectangle(black, 30, -5, 3, 10);
                }
                //draw minute mark
                for (int i = 0; i < 60; i++)
                {
                    gs.RotateTransform(6.0f);
                    gs.FillRectangle(black, 25, -10, 5, 1);
                }



                InitializeTransform(gs, 9, 5);
                //second hand draw
                gs.RotateTransform(secondIntS * 6);
                DrawHand(gs, green, 30, true);

                SecondsLabel.Text = $"Seconds: {secondIntS}";
                SecondsLabel.Show();

                red.Dispose();
                green.Dispose();
                blue.Dispose();
                white.Dispose();
                black.Dispose();
                HelperSW = false;
            }
        }

        private void StartPauseButton_Click(object sender, EventArgs e)
        {
            if (start)
            {
                start = false;
            }
            else start = true; 
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            reset = true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Алена_Дебил
{
    public partial class Form1 : Form
    {
        class Point
        {
            public double x;
            public double y;
            public double z;
            public double H;
           static public Point Parse(string str)
            {
                Point point = new Point();
                string[] st = str.Split(' ');
                point.x = double.Parse(st[0]);
                point.y = double.Parse(st[1]);
                point.z = double.Parse(st[2]);
                point.H = double.Parse(st[3]);
                return point;
            }
        }
        class Line
        {
            public int begin;
            public int end;
            static public Line Parse(string str)
            {
                Line line = new Line();
                string[] a = str.Split(' ');
                line.begin = int.Parse(a[0]);
                line.end = int.Parse(a[1]);
                return line;
            }
        }
        class Ploskost
        {
            public int a;
            public int b;
            public int c;
            static public Ploskost Parse(string str)
            {
                Ploskost line = new Ploskost();
                string[] a = str.Split(' ');
                line.a = int.Parse(a[0]);
                line.b = int.Parse(a[1]);
                line.c = int.Parse(a[2]);
                return line;
            }
        }
        List<Point> t = new List<Point>();
        List<Line> o = new List<Line>();
        List<Ploskost> p = new List<Ploskost>();
        Graphics g;
        void Chtenie()
        {
            StreamReader a = new StreamReader("C:\\Users\\DIMA\\tando.txt");
            var str = a.ReadLine();
            while (true)
            {
                if (a.EndOfStream) break;
                if (str == "stop") break;
                t.Add(Point.Parse(str));
                str = a.ReadLine();
            }
            str = a.ReadLine();
            while (true)
            {
                if (a.EndOfStream) break;
                if (str == "stop") break;
                o.Add(Line.Parse(str));
                str = a.ReadLine();
            }
            str = a.ReadLine();
            while (true)
            {
             
                if (a.EndOfStream) break;
                p.Add(Ploskost.Parse(str));
                str = a.ReadLine();
            }
        }
        void Vmeshenie()
        {
            double maximux = t[0].x;
            double mininux = t[0].x;
            double maximuy = t[0].y;
            double mininuy = t[0].y;
            for (int i = 0; i < t.Count; i++)
            {
                if (t[i].x > maximux) maximux = t[i].x;
                if (t[i].y > maximuy) maximuy = t[i].y;

                if (t[i].x < mininux) mininux = t[i].x;
                if (t[i].y < mininuy) mininuy = t[i].y;
            }
            double k;
            if (pictureBox1.Height / (maximuy - mininuy) > pictureBox1.Width / (maximux - mininux))
                k = pictureBox1.Width / (maximux - mininux);
            else k = pictureBox1.Height / (maximuy - mininuy);
            for (int i = 0; i < t.Count; i++)
            {
                t[i].x -= mininux;
                t[i].y -= mininuy;

                t[i].x *= k;
                t[i].y *= k;
                t[i].z *= k;
            }
        }
        void Kalakanie()
        {
            g.Clear(Color.Black);
            if (checkBox1.Checked)
            {
                for (int i = 0; i < p.Count; i++)
                {
                    double Ax = t[p[i].b - 1].x - t[p[i].a - 1].x;
                    double Ay = t[p[i].b - 1].y - t[p[i].a - 1].y;
                    double Az = t[p[i].b - 1].z - t[p[i].a - 1].z;
                    double Bx = t[p[i].c - 1].x - t[p[i].a - 1].x;
                    double By = t[p[i].c - 1].y - t[p[i].a - 1].y;
                    double Bz = t[p[i].c - 1].z - t[p[i].a - 1].z;
                    double Normalx = Ay * Bz - By * Az;
                    double Normaly = -(Ax * Bz - Bx * Az);
                    double Normalz = Ax * By - Bx * Ay;
                    double Umnojenie = 0; 
                    for (int j = 0; j < t.Count; j++)
                    {
                        double tx = t[j].x - t[p[i].a - 1].x;
                        double ty = t[j].y - t[p[i].a - 1].y;
                        double tz = t[j].z - t[p[i].a - 1].z;
                        Umnojenie = Normalx * tx + Normaly * ty + Normalz * tz;
                        if (Umnojenie > 1 || Umnojenie < -1) break;
                    }
                    if (Umnojenie > 0)
                    { 
                        Normalx *= -1;
                        Normaly = -Normaly;
                        Normalz = -Normalz;
                    }
                    if (Normalz > 0)
                    {
                        for (int k = 0; k < o.Count; k++)
                        {
                            if ((p[i].a == o[k].begin & p[i].b == o[k].end) ||
                                (p[i].a == o[k].end & p[i].b == o[k].begin))
                            {
                                g.DrawLine(new Pen(Color.Red, 2),
                                      (float)t[o[k].begin - 1].x, (float)t[o[k].begin - 1].y,
                                      (float)t[o[k].end - 1].x, (float)t[o[k].end - 1].y);
                            }
                            if ((p[i].a == o[k].begin & p[i].c == o[k].end) ||
                                (p[i].a == o[k].end & p[i].c == o[k].begin))
                            {
                                g.DrawLine(new Pen(Color.Orange, 2),
                                      (float)t[o[k].begin - 1].x, (float)t[o[k].begin - 1].y,
                                      (float)t[o[k].end - 1].x, (float)t[o[k].end - 1].y);
                            }
                            if ((p[i].c == o[k].begin & p[i].b == o[k].end) ||
                                (p[i].c == o[k].end & p[i].b == o[k].begin))
                            {
                                g.DrawLine(new Pen(Color.Blue, 2),
                                      (float)t[o[k].begin - 1].x, (float)t[o[k].begin - 1].y,
                                      (float)t[o[k].end - 1].x, (float)t[o[k].end - 1].y);
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < o.Count; i++)
                {
                    g.DrawLine(new Pen(Color.Orange, 2),
                        (float)t[o[i].begin - 1].x, (float)t[o[i].begin - 1].y,
                        (float)t[o[i].end - 1].x, (float)t[o[i].end - 1].y);
                }
            }
            pictureBox1.Invalidate();
        }
        void Smeshenie(double X, double Y, double Z)
        {
            for (int i = 0; i < t.Count; i++)
            {
                t[i].x = t[i].x + t[i].H * X;
                t[i].y = t[i].y + t[i].H * Y;
                t[i].z = t[i].z + t[i].H * Z;
            }
        }
        void Mashtabirovanie(double X1, double Y1, double Z1)
        {
            for (int i = 0; i < t.Count; i++)
            {
                t[i].x = t[i].x * X1;
                t[i].y = t[i].y * Y1;
                t[i].z = t[i].z * Z1;
            }
        }
        void Povorachivanie(double alpha, double betta, double gamma)
        {
            double newX, newY, newZ;
            for (int i = 0; i < t.Count; i++)
            {
                newY = t[i].y * Math.Cos(alpha * 180 / Math.PI) - t[i].z * Math.Sin(alpha * 180 / Math.PI);
                newZ = t[i].y * Math.Sin(alpha * 180 / Math.PI) + t[i].z * Math.Cos(alpha * 180 / Math.PI);
                t[i].y = newY;
                t[i].z = newZ;
            }
            for (int i = 0; i < t.Count; i++)
            {
                newX = t[i].x * Math.Cos(betta * 180 / Math.PI) + t[i].z * Math.Sin(betta * 180 / Math.PI);
                newZ = -t[i].x * Math.Sin(betta * 180 / Math.PI) + t[i].z * Math.Cos(betta * 180 / Math.PI);
                t[i].x = newX;
                t[i].z = newZ;
            }
            for (int i = 0; i < t.Count; i++)
            {
                newY = t[i].x * Math.Sin(gamma * 180 / Math.PI) + t[i].y * Math.Cos(gamma * 180 / Math.PI);
                newX = t[i].x * Math.Cos(gamma * 180 / Math.PI) - t[i].y * Math.Sin(gamma * 180 / Math.PI);
                t[i].y = newY;
                t[i].x = newX;
            }
        }
        void Dvizhenie(double xy, double xz, double yz, double yx, double zx, double zy)
        {
            for (int i = 0; i < t.Count; i++)
            {
                t[i].x = t[i].x + t[i].y * xy;
            }
            for (int i = 0; i < t.Count; i++)
            {
                t[i].x = t[i].x + t[i].z * xz;
            }
            for (int i = 0; i < t.Count; i++)
            {
                t[i].y = t[i].y + t[i].x * yx;
            }
            for (int i = 0; i < t.Count; i++)
            {
                t[i].y = t[i].y + t[i].z * yz;
            }
            for (int i = 0; i < t.Count; i++)
            {
                t[i].z = t[i].z + t[i].x * zx;
            }
            for (int i = 0; i < t.Count; i++)
            {
                t[i].x = t[i].z + t[i].y * zy;
            }
        }
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            Chtenie();
            Vmeshenie();
            Kalakanie();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Mashtabirovanie(Double.Parse(textBoxmashx.Text),
                 Double.Parse(textBoxmashy.Text),
                 Double.Parse(textBoxmashz.Text));
            Kalakanie();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Povorachivanie(Double.Parse(textBoxalpha.Text),
                   Double.Parse(textBoxbetta.Text),
                   Double.Parse(textBoxgamma.Text));
            Kalakanie();
        }
      private void button4_Click(object sender, EventArgs e)
        {
            Dvizhenie(Double.Parse(textBoxsxy.Text),
                  Double.Parse(textBoxsxz.Text),
                  Double.Parse(textBoxsyx.Text),
                  Double.Parse(textBoxsyz.Text),
                  Double.Parse(textBoxszx.Text),
                  Double.Parse(textBoxszy.Text));
            Kalakanie();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Smeshenie(Double.Parse(textBoxx.Text),
                Double.Parse(textBoxy.Text),
                Double.Parse(textBoxz.Text));
            Kalakanie();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Vmeshenie();
            Kalakanie();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}



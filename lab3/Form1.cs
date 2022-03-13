using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab3
{
    public partial class Form1 : Form
    {
        private const int  MatrixSize = 30;
        private const int BoxSize = 15;
        private PictureBox[,] _viewMatrix = new PictureBox[MatrixSize, MatrixSize];
        private char[,] _valueMatrix = new char[MatrixSize, MatrixSize];
        private Dictionary<string, char> _dict = new Dictionary<string, char>();
        private string[] keys = { "000", "001", "010", "011", "100", "101", "110", "111" };
        private int _count = 0;
        public Form1()
        {
            InitializeComponent();

            for (var i = 0; i < MatrixSize; i++)
            {
                for (var j = 0; j < MatrixSize; j++)
                {
                    _valueMatrix[i, j] = '0';
                    _viewMatrix[i, j] = CreateBox(100 + (BoxSize - 1) * i,
                        12 + (BoxSize - 1) * j);                    
                    if(j == 0)
                    {
                        var x = i; var y = j;
                        _viewMatrix[i, j].MouseClick += (object sender, MouseEventArgs e) =>
                        {
                            UpdateValue(x, y, !(_valueMatrix[x, y] == '1'));
                        };
                    }              
                }
            }

            for (int i = 0; i < keys.Length; i++)
            {
                _dict.Add(keys[i], '0');
            }           
        }   

        private void button1_Click(object sender, EventArgs e)
        {
            if(!timer1.Enabled)
            {
                var num = Convert.ToInt32(numericUpDown1.Value);
                var binary = Convert.ToString(num, 2);
                for (int i = 0; i < keys.Length; i++)
                {
                    var index = 1 + i;
                    if (index > binary.Length)
                    {
                        _dict[keys[i]] = '0';
                    }
                    else
                    {
                        _dict[keys[i]] = binary[binary.Length - index];
                    }
                }
                timer1.Start();
            }
            else
            {
                timer1.Stop();
            }                   
        }    

        private void UpdateValue(int x, int y, bool value)
        {
            if(x < MatrixSize && y < MatrixSize)
            {
                _valueMatrix[x, y] = value? '1':'0';
                SetBoxActive(ref _viewMatrix[x, y], value);
            }
            else
            {
                _valueMatrix[MatrixSize-1, MatrixSize-1] = value ? '1' : '0';
                SetBoxActive(ref _viewMatrix[MatrixSize-1, MatrixSize-1], value);
            }
        }

        /// <summary>
        /// Создать элемент
        /// </summary>
        /// <param name="x">Координата х</param>
        /// <param name="y">Координата y</param>
        private PictureBox CreateBox(int x, int y)
        {
            PictureBox pictureBox1 = new PictureBox();
            pictureBox1.Size = new Size(BoxSize, BoxSize);
            this.Controls.Add(pictureBox1);

            Bitmap flag = new Bitmap(BoxSize, BoxSize);

            Graphics flagGraphics = Graphics.FromImage(flag);

            flagGraphics.FillRectangle(Brushes.White, 0, 0, BoxSize, BoxSize);
            flagGraphics.FillRectangle(Brushes.Black, 0, 0, BoxSize, 1);
            flagGraphics.FillRectangle(Brushes.Black, 0, 0, 1, BoxSize);
            flagGraphics.FillRectangle(Brushes.Black, 0, BoxSize-1, BoxSize, 1);
            flagGraphics.FillRectangle(Brushes.Black, BoxSize-1, 0, 1, BoxSize);

            pictureBox1.Image = flag;
            pictureBox1.Location = new Point(x, y);
            return pictureBox1;
        }

        private void SetBoxActive(ref PictureBox pictureBox, bool value)
        {
            Bitmap flag = new Bitmap(BoxSize, BoxSize);

            Graphics flagGraphics = Graphics.FromImage(flag);

            if(value)
            {
                flagGraphics.FillRectangle(Brushes.SpringGreen, 0, 0, BoxSize, BoxSize);
            }
            else
            {
                flagGraphics.FillRectangle(Brushes.White, 0, 0, BoxSize, BoxSize);
            }
            flagGraphics.FillRectangle(Brushes.Black, 0, 0, BoxSize, 1);
            flagGraphics.FillRectangle(Brushes.Black, 0, 0, 1, BoxSize);
            flagGraphics.FillRectangle(Brushes.Black, 0, BoxSize-1, BoxSize, 1);
            flagGraphics.FillRectangle(Brushes.Black, BoxSize-1, 0, 1, BoxSize);

            pictureBox.Image = flag;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_count < MatrixSize - 1)
            {
                for (int i = 0; i < MatrixSize; i++)
                {
                    var res = new StringBuilder("000");
                    res[1] = _valueMatrix[i, _count];
                    if (i != MatrixSize - 1)
                    {
                        res[2] = _valueMatrix[i + 1, _count];
                    }
                    if (i != 0)
                    {
                        res[0] = _valueMatrix[i - 1, _count];
                    }
                    UpdateValue(i, _count + 1, _dict[res.ToString()] == '1');
                }
                _count++;
            }
            else
            {
                timer1.Stop();
            }
        }
    }
}

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

namespace RedBlackTreeProject
{
    public partial class Form1 : Form
    {
        RedBlackTree _tree;

        const int _DistanceBetweenTiers = 50;
        const int _NodeDiametr = 50;

        bool _isMouseDownLabel = false;

        public Form1()
        {
            InitializeComponent();

            _tree = new RedBlackTree();

            _tree.AddData(10);
            _tree.AddData(11);
            _tree.AddData(12);
            _tree.AddData(1);
            _tree.AddData(2);

            DisplayedTree();
        }

        private void DisplayedTree()
        {
            List<List<double?[]>> treeDataArray = _tree.GetTreeDataArray();

            int maxHeight = (2 * Convert.ToInt32(Math.Pow(2, treeDataArray.Count - 1)) - 1) * _NodeDiametr;

            for (int i = 0; i < treeDataArray.Count; i++)
            {
                int y = _DistanceBetweenTiers * i;

                for (int j = 0; j < treeDataArray[i].Count; j++)
                {
                    int x = (maxHeight / treeDataArray[i].Count) * (j + 1);

                    Label label = new Label
                    {
                        AutoSize = true,
                        Location = new Point(x, y),
                        Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point),
                        
                    };

                    label.MouseDown += new MouseEventHandler(this.circles_MouseDown);
                    label.MouseUp += new MouseEventHandler(this.circles_MouseUp);
                    label.MouseMove += new MouseEventHandler(this.circles_MouseMove);

                    if (treeDataArray[i][j][0].HasValue)
                    {
                        label.Text = Convert.ToString(treeDataArray[i][j][0]);
                    }
                    else
                    {
                        label.Text = "null";
                    }

                    if (treeDataArray[i][j][1] == 0)
                    {
                        label.ForeColor = Color.Red;
                        label.BackColor = Color.Black;
                    }
                    else
                    {
                        label.ForeColor = Color.Black;
                        label.BackColor = Color.Red;
                    }

                    panel.Controls.Add(label);

                    GraphicsPath path = new GraphicsPath();
                    path.AddEllipse(0, 0, label.Width, label.Height);

                    label.Region = new Region(path);
                }
            }
        }

        private void circles_MouseDown(object sender, MouseEventArgs e)
        {
            _isMouseDownLabel = true;
        }

        private void circles_MouseUp(object sender, MouseEventArgs e)
        {
            _isMouseDownLabel = false;
        }

        private void circles_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMouseDownLabel)
            {
                Label movedCircle = (Label)sender;
                
                Point move = PointToClient(Control.MousePosition);
                move.X -= movedCircle.Width / 2;
                move.Y -= movedCircle.Height / 2;

                movedCircle.Location = move;

                /*if (move.X > 0 && move.Y > -5 && move.X < (pictureBox.Size.Width - 30) &&
                    move.Y < (pictureBox.Size.Height - 35))
                {

                    List<TextBox> movedLine = new List<TextBox>();

                    foreach (TextBox item in line.Keys)
                    {
                        if (line[item].Contains(movedCircle))
                        {
                            movedLine.Add(item);
                        }
                    }

                    if (movedLine.Count >= 0)
                    {
                        Graphics gr = pictureBox.CreateGraphics();

                        foreach (TextBox item in movedLine)
                        {
                            int x1 = line[item][0].Location.X;
                            int y1 = line[item][0].Location.Y;
                            int x2 = line[item][1].Location.X;
                            int y2 = line[item][1].Location.Y;

                            gr.DrawLine(new Pen(Color.White, 2), new Point(x1 + 20, y1 + 20), new Point(x2 + 20, y2 + 20));
                        }

                        movedCircle.Location = move;

                        foreach (TextBox item in movedLine)
                        {
                            int x1 = line[item][0].Location.X;
                            int y1 = line[item][0].Location.Y;
                            int x2 = line[item][1].Location.X;
                            int y2 = line[item][1].Location.Y;

                            gr.DrawLine(new Pen(Color.Black, 2), new Point(x1 + 20, y1 + 20), new Point(x2 + 20, y2 + 20));

                            item.Location = new Point((x1 + x2) / 2 + 17, (y1 + y2) / 2 + 17);
                        }

                    }
                    else
                    {
                        movedCircle.Location = move;
                    }

                }*/

            }

        }
    }
}

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
        AboutForm _aboutDialog;

        Graphics _graphics;
        PictureBox _pictureBox;

        RedBlackTree _tree;

        const int _DistanceBetweenTiers = 70;
        const int _NodeDiametr = 50;

        List<List<Label>> _associatedNodes;

        bool _isMouseDownToNode = false;

        public Form1()
        {
            InitializeComponent();

            _associatedNodes = new List<List<Label>>();

            _tree = new RedBlackTree();

            _pictureBox = new PictureBox();
            _pictureBox.Dock = DockStyle.Fill;
            _pictureBox.BorderStyle = BorderStyle.FixedSingle;
            _pictureBox.BackColor = Color.White;
            _pictureBox.Resize += new EventHandler(pictureBox_Resize);
            panel.Controls.Add(_pictureBox);

            _graphics = _pictureBox.CreateGraphics();

            _aboutDialog = new AboutForm();

            if (Properties.Settings.Default.isShowAboutMenu)
            {
                _aboutDialog.Show();
            }
        }

        private void DisplayedTree()
        {
            List<List<double?[]>> treeDataArray = _tree.GetTreeDataArray();

            int maxWidth = (2 * Convert.ToInt32(Math.Pow(2, treeDataArray.Count - 1)) - 1) * _NodeDiametr;

            List<Label> previosTier = new List<Label>();
            previosTier.Add(new Label { Location = new Point(maxWidth, 0) });
            
            for (int i = 0; i < treeDataArray.Count; i++)
            {
                IEnumerator<Label> iterator = previosTier.GetEnumerator();
                iterator.MoveNext();

                List<Label> newTier = new List<Label>();

                int biasX = Convert.ToInt32(maxWidth / Math.Pow(2, i + 1) );

                for (int j = 0; j < treeDataArray[i].Count; j++)
                {
                    Label label = new Label();
                    newTier.Add(label);

                    label.AutoSize = true;
                    label.Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point);
                    label.MouseDown += new MouseEventHandler(this.Node_MouseDown);
                    label.MouseUp += new MouseEventHandler(this.Node_MouseUp);
                    label.MouseMove += new MouseEventHandler(this.Node_MouseMove);

                    while (iterator.Current.Text == "null")
                    {
                        iterator.MoveNext();
                    }

                    if (i != 0 || j != 0)
                    {
                        _associatedNodes.Add(new List<Label>());
                        _associatedNodes[_associatedNodes.Count - 1].Add(iterator.Current);
                        _associatedNodes[_associatedNodes.Count - 1].Add(label);
                    }

                    int y = iterator.Current.Location.Y + _DistanceBetweenTiers;
                    int x = 0;

                    if (j % 2 == 0)
                    {
                        x = iterator.Current.Location.X - biasX;
                    }
                    else
                    {
                        x = iterator.Current.Location.X + biasX;

                        iterator.MoveNext();
                    }

                    label.Location = new Point(x, y);

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
                    label.BringToFront();
                }

                previosTier = newTier;
            }
        }

        private void DisplayedTreeAssociated()
        {
            foreach (List<Label> node in _associatedNodes)
            {
                DrawLines(node[0], node[1], Color.Black);
            }
        }

        private void Node_MouseDown(object sender, MouseEventArgs e)
        {
            _isMouseDownToNode = true;
        }

        private void Node_MouseUp(object sender, MouseEventArgs e)
        {
            _isMouseDownToNode = false;
        }

        private void Node_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMouseDownToNode)
            {
                Label movedNode = (Label)sender;
                
                Point moveTo = PointToClient(Control.MousePosition);
                moveTo.X -= movedNode.Width / 2;
                moveTo.Y -= movedNode.Height / 2;

                foreach (List<Label> node in _associatedNodes)
                {
                    if (node.Contains(movedNode))
                    {
                        DrawLines(node[0], node[1], Color.White);
                    }
                }

                movedNode.Location = moveTo;

                foreach (List<Label> node in _associatedNodes)
                {
                    if (node.Contains(movedNode))
                    {
                        DrawLines(node[0], node[1], Color.Black);
                    }
                }                
            }
        }

        private void DrawLines(Label node1, Label node2, Color color)
        {
            int x1 = node1.Location.X;
            int y1 = node1.Location.Y;
            int x2 = node2.Location.X;
            int y2 = node2.Location.Y;

            _graphics.DrawLine(new Pen(color, 2), new Point(x1 + node1.Width / 2, y1 + node1.Height / 2),
                                                        new Point(x2 + node2.Width / 2, y2 + node2.Height / 2));
        }

        private void pictureBox_Resize(object sender, EventArgs e)
        {
            _graphics = _pictureBox.CreateGraphics();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            _tree.AddData(Convert.ToDouble(addNumericUpDown.Value));

            ClearGraphics();

            DisplayedTree();
            DisplayedTreeAssociated();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            _tree.DeleteData(Convert.ToDouble(deleteNumericUpDown.Value));

            _graphics.Clear(_pictureBox.BackColor);

            int countControls = panel.Controls.Count;

            for (int i = 0; i < countControls; i++)
            {
                if (panel.Controls[i] is Label)
                {
                    panel.Controls.RemoveAt(i);
                    i--;
                    countControls--;
                }
            }

            _associatedNodes.Clear();

            DisplayedTree();
            DisplayedTreeAssociated();
        }

        private void aboutProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _aboutDialog.ShowDialog();
        }

        private void ClearGraphics()
        {
            _graphics.Clear(_pictureBox.BackColor);

            int countControls = panel.Controls.Count;

            for (int i = 0; i < countControls; i++)
            {
                if (panel.Controls[i] is Label)
                {
                    panel.Controls.RemoveAt(i);
                    i--;
                    countControls--;
                }
            }

            _associatedNodes.Clear();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                try
                {
                    using System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog.OpenFile());

                    string inputValue;

                    while ((inputValue = sr.ReadLine()) != null)
                    {
                        double value = Convert.ToDouble(inputValue);

                        _tree.AddData(value);
                    }
                }
                catch (System.IO.IOException)
                {
                    MessageBox.Show("Файл не может быть прочитан.", "Ошибка!");
                }

                ClearGraphics();

                DisplayedTree();
                DisplayedTreeAssociated();
            }
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DialogResult result = saveFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                try
                {
                    using System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFileDialog.OpenFile());

                    List<double> dataArray = _tree.GetDataArray();

                    foreach(double data in dataArray)
                    {
                        sw.WriteLine(data);
                    }
                }
                catch (System.IO.IOException)
                {
                    MessageBox.Show("Не удалось сохранить данные.", "Ошибка!");
                }
            }
        }

        private void randAddButton_Click(object sender, EventArgs e)
        {
            Random rand = new Random();

            for (int i = 0; i < countRandNumericUpDown.Value; i++)
            {
                _tree.AddData(rand.Next(-1000, 1000));
            }

            ClearGraphics();

            DisplayedTree();
            DisplayedTreeAssociated();
        }
    }
}

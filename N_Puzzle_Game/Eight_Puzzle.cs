using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace N_Puzzle_Game
{
    public partial class Eight_Puzzle : Form
    {
        string path = "";
        Timer t;
        UserControl_Puzzle_Numbers usdg;
        UserControl_Puzzle_Pictures uspc;
        public bool b_bfs = false, b_gbfs = false, b_a_star = false, b_bi = false;

        public Eight_Puzzle()
        {
            InitializeComponent();
            lbl_time.Text = "";
            lbl_time.ForeColor = Color.OrangeRed;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lbl_time.Text = "";
            if (radioButton1.Checked)
            {
                panel1.Controls.Clear();
                usdg = new UserControl_Puzzle_Numbers(270, 3, 90);
                panel1.Controls.Add(usdg);
            }
            else if (radioButton2.Checked && path != "")
            {
                panel1.Controls.Clear();
                uspc = new UserControl_Puzzle_Pictures(path, 270, 3, 90);
                panel1.Controls.Add(uspc);
            }
        }

        private void Eight_Puzzle_Load(object sender, EventArgs e)
        {
            this.Width = 310;
            t = new Timer();
            t.Interval = 5;
            t.Tick += new EventHandler(t_click);
            radioButton1.Checked = true;
            radioButton2.Checked = false;
        }

        private void t_click(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                int x = this.Width + 1;
                if (x <= 624)
                    this.Width = x;
                else t.Stop();
            }
            else if (radioButton1.Checked)
            {
                int x = this.Width - 1;
                if (x >= 310)
                    this.Width = x;
                else t.Stop();
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            t.Start();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            t.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int[,] state;
            string s = "";
            int start = DateTime.Now.Minute * 60 * 1000 +
                DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
            if (usdg != null && radioButton1.Checked)
            {
                state = get_state(usdg.state);
                if (b_bi)
                {
                    usdg.obj_bi = new bidirectional(state, 3);
                    usdg.start();
                    s = "Bidirectional";
                }
                else if (b_bfs && !usdg.is_goal())
                {
                    usdg.obj_bfs = new bfs(usdg.state);
                    usdg.start();
                    s = "BFS";
                }
                else if (b_gbfs && !usdg.is_goal())
                {
                    usdg.obj_gbfs = new Greedy_Best_First_Search(3);
                    usdg.obj_gbfs.set_goal(usdg.obj_gbfs.get_destination());
                    usdg.obj_gbfs.solve(state); usdg.start();
                    s = "GBFS";
                }
                else if (b_a_star && !usdg.is_goal())
                {
                    usdg.obj_astar = new a_star(3);
                    usdg.obj_astar.set_goal(usdg.obj_astar.get_destination());
                    usdg.obj_astar.solve(state); usdg.start();
                    s = "A*";
                }
            }
            else if (uspc != null && uspc.state.Length == 9 && radioButton2.Checked)
            {
                state = get_state(uspc.state);
                if (b_bi)
                {
                    uspc.obj_bi = new bidirectional(state, 3);
                    uspc.start();
                    s = "Bidirectional";
                }
                else if (b_bfs && !uspc.is_goal())
                {
                    uspc.obj_bfs = new bfs(uspc.state);
                    uspc.start();
                    s = "BFS";
                }
                else if (b_gbfs && !uspc.is_goal())
                {
                    uspc.obj_gbfs = new Greedy_Best_First_Search(3);
                    uspc.obj_gbfs.set_goal(uspc.obj_gbfs.get_destination());
                    uspc.obj_gbfs.solve(state);  uspc.start();
                    s = "GBFS";
                }
                else if (b_a_star && !uspc.is_goal())
                {
                    uspc.obj_astar = new a_star(3);
                    uspc.obj_astar.set_goal(uspc.obj_astar.get_destination());
                    uspc.obj_astar.solve(state);  uspc.start();
                    s = "A*";
                }

            }
            int end = DateTime.Now.Minute * 60 * 1000 +
                DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
            lbl_time.Text = "time required is : " + (end - start).ToString()
                + " ms, " + s;
        }

        private int[,] get_state(string state)
        {
            int N = (int)Math.Sqrt(state.Length);
            int[,] rs = new int[N, N];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    int idx = i * 3 + j;
                    rs[i, j] = state[idx] - 48;
                }
            return rs;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog choose = new OpenFileDialog();
            choose.Filter = 
                "Image Files (JPEG, GIF, PNG , JPG) |*.jpeg; *.gif; *.png; *.jpg";

            if (choose.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(choose.FileName);
                path = choose.FileName;
            }
        }

        private void Eight_Puzzle_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Start_Window.__main__ != null)
            {
                Start_Window.__main__.Show();
            }
        }

    }
}

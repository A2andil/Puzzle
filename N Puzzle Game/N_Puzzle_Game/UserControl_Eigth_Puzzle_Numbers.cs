using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace N_Puzzle_Game
{
    public partial class UserControl_Eigth_Puzzle_Numbers : UserControl_Need
    {
        public bfs obj_bfs;
        public a_star obj_astar;
        public Greedy_Best_First_Search obj_gbfs;

        public UserControl_Eigth_Puzzle_Numbers(int sz)
        {
            InitializeComponent();
            bfs.solution.Clear();
            a_star.solution.Clear();
            Greedy_Best_First_Search.solution.Clear();
            Size = new Size(sz, sz);
            t = new Timer();
            t.Interval = 230;
            t.Tick += new EventHandler(start_solution);
        }

        private void start_solution(object sender, EventArgs e)
        {
            if (obj_bfs != null && bfs.solution.Count > 0)
            {
                state = bfs.solution.Pop();
                Draw(state);
            }
            else if (obj_astar != null && a_star.solution.Count > 0)
            {
                state = a_star.solution.Pop();
                Draw(state);
            }
            else if (obj_gbfs != null && Greedy_Best_First_Search.solution.Count > 0)
            {
                state = Greedy_Best_First_Search.solution.Pop();
                Draw(state);
            }
            else t.Stop();
        }

        private void Draw(string state)
        {
            foreach (Button btn in Controls)
            {
                int idx = btn.Location.X / 90 + btn.Location.Y / 90 * 3;
                if (state[idx] != int.Parse(btn.Text) + 48)
                {
                    int tmp_x = btn.Location.X, tmp_y = btn.Location.Y;
                    btn.Location = new Point(x, y);
                    x = tmp_x;
                    y = tmp_y;
                }
            }
        }

        Button create_button(int x, int y, int w)
        {
            Button btn = new Button();
            btn.Size = new Size(w, w);
            btn.Location = new Point(x, y);
            return btn;
        }

        private void UserControl_Eigth_Puzzle_Numbers_Load(object sender, EventArgs e) 
        {
            while (!is_solvable(state = get_state(9)))
                state = suffle(state);

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    if (state[i * 3 + j] == 48)
                    {
                        x = j * 90;
                        y = i * 90;
                        continue;
                    }
                    Button btn = create_button(j * 90, i * 90, 90);
                    btn.Font = new System.Drawing.Font("Mistral", 48, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    btn.Text = (state[i * 3 + j] - 48).ToString();
                    btn.Click += new System.EventHandler(this.click);
                    btn.BackColor = Color.Orange;
                    btn.ForeColor = Color.White;
                    this.Controls.Add(btn);
                }
        }

        private void click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int btn_x = btn.Location.X, btn_y = btn.Location.Y;
            if ((x == btn_x && (y + 90 == btn_y || y - 90 == btn_y))
                || (y == btn_y && (x + 90 == btn_x || x - 90 == btn_x)))
            {
                // swap
                char[] list = state.ToCharArray();
                list[y / 90 * 3 + x / 90] = (char)(int.Parse(btn.Text) + 48);
                list[btn_y / 90 * 3 + btn_x / 90] = '0';
                state = new string(list);
                Point p = new Point(x, y);
                x = btn_x;
                y = btn_y;
                btn.Location = p;
                if (x == 180 && y == 180) is_goal();
            }
        }

        public bool is_goal()
        {
            string ans = "0";
            for (char i = (char)(56); i > '0'; i--)
                ans = i + ans;
            foreach (Button btn in Controls)
            {
                int idx = (btn.Location.X / 90) + (btn.Location.Y / 90) * 3;
                if (ans[idx] != int.Parse(btn.Text) + 48) return false;
            }
            MessageBox.Show("Good job");
            return true;
        }
    }
}

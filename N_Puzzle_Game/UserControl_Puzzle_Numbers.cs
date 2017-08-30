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
    public partial class UserControl_Puzzle_Numbers : UserControl_Need
    {
        public bfs obj_bfs;
        public a_star obj_astar;
        public Greedy_Best_First_Search obj_gbfs;
        public bidirectional obj_bi;

        public UserControl_Puzzle_Numbers(int s, int n, int l)
        {
            InitializeComponent();
            set(s, n, l);
            if (N == 3)
                while (!is_solvable(state = suffle(get_state(N * N)))) ;
            else if (N == 4)
            {
                load();
                state = random_state();
            }
            t = new Timer();
            t.Interval = 200;
            t.Tick += new EventHandler(start_solution);
        }

        private void set(int s, int n, int l)
        {
            N = n;
            btn_length = l;
            w_length = s;
            Size = new Size(w_length, w_length);
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
            else if (obj_bi != null && bidirectional.solution.Count > 0)
            {
                state = bidirectional.solution.Pop();
                Draw(state);
            }
            else t.Stop();
        }

        private void Draw(string state)
        {
            foreach (Button btn in Controls)
            {
                int idx = btn.Location.X / btn_length + btn.Location.Y / btn_length * N;
                if (state[idx] != int.Parse(btn.Text) + 48)
                {
                    int tmp_x = btn.Location.X, tmp_y = btn.Location.Y;
                    btn.Location = new Point(x, y);
                    x = tmp_x; y = tmp_y;
                }
            }
        }

        Button create_button(int x, int y, int w)
        {
            Button btn = new Button();
            btn.Size = new Size(w, w);
            btn.Location = new Point(x, y);
            btn.BackColor = Color.Orange;
            btn.ForeColor = Color.White;
            return btn;
        }

        private void UserControl_Eigth_Puzzle_Numbers_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                {
                    if (state[i * N + j] == 48)
                    {
                        x = j * btn_length; y = i * btn_length;
                        continue;
                    }
                    Button btn = create_button(j * btn_length, i * btn_length, btn_length);
                    int font_size = N == 3 ? 48 : 32;
                    btn.Font = new System.Drawing.Font("Mistral", font_size, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    btn.Text = (state[i * N + j] - 48).ToString();
                    btn.Click += new System.EventHandler(this.click);
                    this.Controls.Add(btn);
                }
        }

        private void click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int btn_x = btn.Location.X, btn_y = btn.Location.Y;
            if ((x == btn_x && (y + btn_length == btn_y || y - btn_length == btn_y))
                || (y == btn_y && (x + btn_length == btn_x || x - btn_length == btn_x)))
            {
                char[] list = state.ToCharArray();
                list[y / btn_length * N + x / btn_length] = (char)(int.Parse(btn.Text) + 48);
                list[btn_y / btn_length * N + btn_x / btn_length] = '0';
                state = new string(list);
                Point p = new Point(x, y);
                x = btn_x; y = btn_y;
                btn.Location = p;
                if (x == (N - 1) * btn_length && y == (N - 1) * btn_length) is_goal();
            }
        }

        public bool is_goal()
        {
            string ans = "0";
            for (char i = (char)(N * N + 47); i > '0'; i--)
                ans = i + ans;
            foreach (Button btn in Controls)
            {
                int idx = (btn.Location.X / btn_length) + (btn.Location.Y / btn_length) * N;
                if (ans[idx] != int.Parse(btn.Text) + 48) return false;
            }
            MessageBox.Show("Good job");
            return true;
        }
    }
}

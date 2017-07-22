using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace N_Puzzle_Game
{
    public partial class UserControl_Puzzle_Pictures : UserControl_Need
    {
        Bitmap btmp;
        List<Bitmap> list = new List<Bitmap>();
        Dictionary<Image, int> map = new Dictionary<Image, int>();
        public bfs obj_bfs;
        public a_star obj_astar;
        public Greedy_Best_First_Search obj_gbfs;
        public bidirectional obj_bi;

        public UserControl_Puzzle_Pictures(string path, int s, int n, int l)
        {
            InitializeComponent();
            set(s, n, l);
            if (N == 3)
            {
                while (!is_solvable(state = suffle(get_state(N * N))))
                    state = suffle(state);
            }
            else if (N == 4)
            {
                load();
                state = random_state();
            }
            resize_image(path);
            this.Controls.Clear();
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                {
                    if (state[i * N + j] == '0')
                    {
                        x = j * btn_length;
                        y = i * btn_length;
                        continue;
                    }
                    Button btn = create_button(j * btn_length, i * btn_length, btn_length);
                    btn.Image = list[state[i * N + j] - 49];
                    btn.Click += new System.EventHandler(click);
                    this.Controls.Add(btn);
                }
        }

        private void set(int s, int n, int l)
        {
            N = n;
            btn_length = l;
            w_length = s;
            btmp = new Bitmap(w_length, w_length);
            this.Size = new Size(w_length, w_length);
        }

        Button create_button(int x, int y, int w)
        {
            Button btn = new Button();
            Point p = new Point(x, y);
            btn.Location = p;
            btn.Size = new Size(w, w);
            return btn;
        }

        private void click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int btn_x = btn.Location.X, btn_y = btn.Location.Y;
            if ((x == btn_x && (y + btn_length == btn_y || y - btn_length == btn_y)) ||
                (y == btn_y && (x + btn_length == btn_x || x - btn_length == btn_x)))
            {
                char[] lst = state.ToCharArray();
                lst[y / btn_length * N + x / btn_length] = (char)(map[btn.Image] + 49);
                lst[btn_y / btn_length * N + btn_x / btn_length] = (char)48;
                state = new string(lst);
                Point p = new Point(x, y);
                x = btn_x; y = btn_y;
                btn.Location = p;
                if (x == (N - 1) * btn_length && y == (N - 1) * btn_length) is_goal();
            }
        }

        public bool is_goal()
        {
            foreach (Button btn in Controls)
            {
                if ((btn.Location.X / btn_length) + (btn.Location.Y / btn_length) * N
                    != map[btn.Image]) return false;
            }
            MessageBox.Show("Good Play");
            return true;
        }

        private void resize_image(string path)
        {
            Graphics graphic = Graphics.FromImage(btmp);

            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.DrawImage(Image.FromFile(path), 0, 0, w_length, w_length);

            graphic.Dispose();

            int rw = 0, cl = 0;
            for (int x = 0; x < N * N - 1; x++)
            {
                Bitmap mp = new Bitmap(btn_length, btn_length);
                for (int i = 0; i < btn_length; i++)
                    for (int j = 0; j < btn_length; j++)
                    {
                        mp.SetPixel(j, i,
                            btmp.GetPixel(cl * btn_length + j, rw * btn_length + i));
                    }
                list.Add(mp);
                map.Add(mp, list.Count - 1);
                cl += 1;
                if (cl == N)
                {
                    rw += 1;
                    cl = 0;
                }
            }
        }

        private void UserControl_Eigth_Puzzle_Pictures_Load(object sender, EventArgs e)
        {
            t = new Timer();
            t.Interval = 200;
            t.Tick += new EventHandler(start_solution);
        }

        private void Draw(string state)
        {
            foreach (Button btn in Controls)
            {
                int idx = btn.Location.X / btn_length + btn.Location.Y / btn_length * N;
                if (state[idx] == (char)48)
                {
                    int tmp_x = btn.Location.X, tmp_y = btn.Location.Y;
                    btn.Location = new Point(x, y);
                    x = tmp_x; y = tmp_y;
                }
            }
        }

        void start_solution(object sender, EventArgs e)
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
    }
}

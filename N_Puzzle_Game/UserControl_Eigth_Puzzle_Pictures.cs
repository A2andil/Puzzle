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
    public partial class UserControl_Eigth_Puzzle_Pictures : UserControl_Need
    {
        Bitmap btmp = new Bitmap(280, 280);
        List<Bitmap> list = new List<Bitmap>();
        Dictionary<Image, int> map = new Dictionary<Image, int>();
        public bfs obj_bfs;
        public a_star obj_astar;
        public Greedy_Best_First_Search obj_gbfs;

        public UserControl_Eigth_Puzzle_Pictures(string path, int N)
        {
            InitializeComponent();
            this.Size = new Size(N, N);
            bfs.solution.Clear();
            while (!is_solvable(state = suffle(get_state(9))))
                state = suffle(state);
            resize_image(path);
            this.Controls.Clear();
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    if (state[i * 3 + j] == '0')
                    {
                        x = j * 90;
                        y = i * 90;
                        continue;
                    }
                    Button btn = create_button(j * 90, i * 90, 90);
                    btn.Image = list[state[i * 3 + j] - 49];
                    btn.Click += new System.EventHandler(click);
                    this.Controls.Add(btn);
                }
        }

        Button create_button(int x, int y, int w)
        {
            Button btn = new Button();
            Point p = new Point(x, y);
            btn.Location = p;
            btn.Size = new Size(90, 90);
            return btn;
        }

        private void click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int btn_x = btn.Location.X, btn_y = btn.Location.Y;
            if ((x == btn_x && (y + 90 == btn_y || y - 90 == btn_y)) ||
                (y == btn_y && (x + 90 == btn_x || x - 90 == btn_x)))
            {
                char[] lst = state.ToCharArray();
                lst[y / 90 * 3 + x / 90] = (char)(map[btn.Image] + 49);
                lst[btn_y / 90 * 3 + btn_x / 90] = '0';
                state = new string(lst);
                Point p = new Point(x, y);
                x = btn_x;
                y = btn_y;
                btn.Location = p;
                if (x == 180 && y == 180) is_goal();
            }
        }

        public bool is_goal()
        {
            foreach (Button btn in Controls)
            {
                if ((btn.Location.X / 90) + (btn.Location.Y / 90) * 3 
                    != map[btn.Image]) return false;
            }
            MessageBox.Show("Good Play");
            return true;
        }

        private void resize_image(string path)
        {
            Graphics graphic = Graphics.FromImage(btmp);

            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.DrawImage(Image.FromFile(path), 0, 0, 280, 280);

            graphic.Dispose();

            int rw = 0, cl = 0;
            for (int x = 0; x < 8; x++)
            {
                Bitmap mp = new Bitmap(90, 90);
                for (int i = 0; i < 90; i++)
                    for (int j = 0; j < 90; j++)
                    {
                        mp.SetPixel(j, i,
                            btmp.GetPixel(cl * 90 + j, rw * 90 + i));
                    }
                list.Add(mp);
                map.Add(mp, list.Count - 1);
                cl += 1;
                if (cl == 3)
                {
                    rw += 1;
                    cl = 0;
                }
            }
        }

        private void UserControl_Eigth_Puzzle_Pictures_Load(object sender, EventArgs e)
        {
             t = new Timer();
            t.Interval = 230;
            t.Tick += new EventHandler(start_solution);
        }

        private void Draw(string state)
        {
            foreach (Button btn in Controls)
            {
                int idx = btn.Location.X / 90 + btn.Location.Y / 90 * 3;
                if (state[idx] == '0')
                {
                    int tmp_x = btn.Location.X, tmp_y = btn.Location.Y;
                    btn.Location = new Point(x, y);
                    x = tmp_x;
                    y = tmp_y;
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
            else t.Stop();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace N_Puzzle_Game
{
    public class UserControl_Need : UserControl
    {
        public Timer t;
        public int btn_length { set; get; }
        public int x { set; get; }
        public int y { set; get; }
        public string state { set; get; }

        public string suffle(string p) 
        {
            Random rand = new Random();
            char[] state = p.ToCharArray();
            for (int i = 0; i < p.Length - 1; i++)
            {
                int x = rand.Next(i + 1, p.Length);
                char c = state[i];
                state[i] = state[x];
                state[x] = c;
            }
            return new string(state);
        }

        public bool is_solvable(string state) 
        {
            int ans = 0;
            for (int i = 0; i < state.Length - 1; i++)
                for (int j = i + 1; j < state.Length; j++)
                    if (state[j] > state[i] && state[i] != 48)
                        ans += 1;
            return (ans % 2 == 0);
        }

        public string get_state(int size) 
        {
            string ans = "";
            for (char i = '0'; i < '0' + size; i++)
                ans += i;
            return suffle(ans);
        }

        public void start() 
        {
            t.Start();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace N_Puzzle_Game
{
    public class bidirectional
    {
        public static a_star front, back;

        public static Dictionary<string, node> back_vis = new Dictionary<string, node>();
        public static Dictionary<string, node> front_vis = new Dictionary<string, node>();
        public static Stack<string> solution = new Stack<string>();

        private void clear()
        {
            back_vis.Clear(); front_vis.Clear(); solution.Clear();
        }

        public bidirectional(int[,] state, int N)
        {
            clear(); initial(state, N);
            Thread th_front = new Thread(new ThreadStart(front.solve_in_thread));
            Thread th_back = new Thread(new ThreadStart(back.solve_in_thread));
            th_front.Start(); th_back.Start();
            while (!a_star.is_goal_state) ;
        }

        private void initial(int[,] state, int N)
        {
            front = new a_star(N);  back = new a_star(N);
            front.front = true; back.back = true;
            front.set_goal(front.get_destination());
            front.set_state(state); back.set_goal(state);
            back.set_state(back.get_destination());
        }

        public static void Path(node ch_nd)
        {
            Stack<string> bk = new Stack<string>();
            node tmp = ch_nd;
            while (tmp != null)
            {
                string state = front.to_string(tmp.state);
                bk.Push(state);
                tmp = back_vis[state].parent;
            }
            while (bk.Count > 0) solution.Push(bk.Pop());
            tmp = front_vis[front.to_string(ch_nd.state)].parent;
            while (tmp != null)
            {
                string state = front.to_string(tmp.state);
                solution.Push(state);
                tmp = front_vis[state].parent;
            }
        }

    }
}

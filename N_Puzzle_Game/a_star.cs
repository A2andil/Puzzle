using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Puzzle_Game
{
    public class a_star : Need
    {
        public static bool is_goal_state = false;
        public bool is_in_tread = false, front = false, back = false; 
        string str_state { set; get; }
        public static Stack<string> solution = new Stack<string>();

        priority_queue states_q = new priority_queue();
        HashSet<string> visited = new HashSet<string>();

        public a_star(int tmp) 
        {
            N = tmp;
            is_goal_state = false;
            solution.Clear();
        }

        public void solve_in_thread() 
        {
            is_in_tread = true;
            solve(initial_state);
        }

        node get_node(int[,] state, node parent, int x, int y) 
        {
            node ch_nd = new node();
            ch_nd.state = state;
            ch_nd.level = parent.level + 1;
            ch_nd.x = x; ch_nd.y = y;
            ch_nd.cost = get_cost(state) + (int)Math.Sqrt(ch_nd.level);
            ch_nd.parent = parent;
            return ch_nd;
        }

        public void solve(int[,] c_state) 
        {
            node root = create_root(c_state);
            str_state = to_string(c_state);
            states_q.push(root); visited.Add(str_state);
            if (is_in_tread && front) bidirectional.front_vis.Add(str_state, root);
            else if (is_in_tread && back) bidirectional.back_vis.Add(str_state, root);
            while (!states_q.empty() && !is_goal_state)
            {
                node tmp = states_q.pop();
                get_child(tmp);
            }
        }

        private node create_root(int[,] c_state) 
        {
            node root = new node();
            root.state = c_state; root.parent = null;
            root.cost = get_cost(c_state);
            set_x_y(root, c_state);
            return root;
        }

        private void set_x_y(node root, int[,] c_state) 
        {
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    if (c_state[i, j] == 0)
                    {
                        root.x = j;
                        root.y = i;
                    }
        }

        private void get_child(node tmp) 
        {
            for (int i = 0; i < 4 && !is_goal_state; i++)
            {
                if (is_safe(tmp.y + dy[i], tmp.x + dx[i]))
                {
                    int[,] c_state = new int[N, N];
                    copy(c_state, tmp.state);
                    c_state[tmp.y, tmp.x] = c_state[tmp.y + dy[i], tmp.x + dx[i]];
                    c_state[tmp.y + dy[i], tmp.x + dx[i]] = 0;
                    str_state = to_string(c_state);
                    if (is_in_tread && front)
                    {
                        if (!bidirectional.front_vis.ContainsKey(str_state))
                        {
                            node ch_nd = get_node(c_state, tmp,
                            tmp.x + dx[i], tmp.y + dy[i]);
                            bidirectional.front_vis.Add(str_state, ch_nd);
                            states_q.push(ch_nd);
                            if (bidirectional.back_vis.ContainsKey(str_state))
                            {
                                is_goal_state = true;
                                bidirectional.Path(ch_nd);
                            }
                        }
                    }
                    else if (is_in_tread && back)
                    {
                        if (!bidirectional.back_vis.ContainsKey(str_state))
                        {
                            node ch_nd = get_node(c_state, tmp, tmp.x + dx[i], tmp.y + dy[i]);
                            bidirectional.back_vis.Add(str_state, ch_nd);
                            states_q.push(ch_nd);
                            if (bidirectional.front_vis.ContainsKey(str_state))
                            {
                                is_goal_state = true;
                                bidirectional.Path(ch_nd);
                            }
                        }
                    }
                    else if (!visited.Contains(str_state))
                    {
                        node ch_nd = get_node(c_state, tmp, tmp.x + dx[i], tmp.y + dy[i]);
                        visited.Add(str_state); states_q.push(ch_nd);
                        if (is_goal(c_state))
                        {
                            node tmp_nd = ch_nd; is_goal_state = true;
                            while (tmp_nd != null)
                            {
                                solution.Push(to_string(tmp_nd.state));
                                tmp_nd = tmp_nd.parent;
                            }
                        }
                    }
                }
            }
        }

    }
}

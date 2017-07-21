using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Puzzle_Game
{
    public class Greedy_Best_First_Search : Need
    {
        bool is_goal_state = false;
        string str_state { set; get; }
        public static Stack<string> solution = new Stack<string>();

        priority_queue states_q = new priority_queue();
        public static HashSet<string> visited = new HashSet<string>();

        public Greedy_Best_First_Search(int tmp)
        {
            N = tmp;
            goal = get_destination();
            visited.Clear();
            solution.Clear();
        }

        node get_node(int[,] state, node parent, int x, int y)
        {
            node ch_nd = new node();
            ch_nd.x = x;
            ch_nd.y = y;
            ch_nd.state = state;
            ch_nd.level = parent.level + 1;
            ch_nd.cost = get_cost(state) + (int)Math.Sqrt(ch_nd.level);
            ch_nd.parent = parent;
            return ch_nd;
        }

        public void solve(int[,] c_state)
        {
            node root = create_root(c_state);
            str_state = to_string(c_state);
            states_q.push(root);
            visited.Add(str_state);

            while (!states_q.empty() && !is_goal_state)
            {
                node tmp = states_q.pop();

                get_child(tmp.state, tmp);
            }
        }

        private node create_root(int[,] c_state)
        {
            node root = new node();
            root.level = 0;
            root.state = c_state;
            root.parent = null;
            root.cost = get_cost(c_state);
            set_x_y(root, c_state);
            return root;
        }

        private void set_x_y(node root, int[,] c_state)
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (c_state[i, j] == 0)
                    {
                        root.x = j;
                        root.y = i;
                    }
                }
            }
        }

        private void get_child(int[,] p, node tmp)
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
                    if (!visited.Contains(str_state))
                    {
                        node ch_nd = get_node(c_state, tmp,
                            tmp.x + dx[i], tmp.y + dy[i]);
                        visited.Add(str_state);
                        states_q.push(ch_nd);
                        if (is_goal(c_state))
                        {
                            is_goal_state = true;
                            node tmp_nd = ch_nd;
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
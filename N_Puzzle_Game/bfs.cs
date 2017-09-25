using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Puzzle_Game
{
    public class bfs
    {
        class bfs_node 
        {
            public string state;
            public bfs_node parent;
        }

        bool is_goal_done = false;
        string goal { set; get; }

        private trie visited;
        public Stack<string> solution = new Stack<string>();
        Queue<bfs_node> q = new Queue<bfs_node>();

        public bfs(string state)
        {
            solution.Clear();
            goal = get_goal(state.Length);
            visited = new trie(state.Length);
            bfs_node root = create_node(state, null);
            solve(root);
        }

        private bfs_node create_node(string state, bfs_node parent)
        {
            bfs_node nd = new bfs_node();
            nd.state = state;
            nd.parent = parent;
            visited.add(state); q.Enqueue(nd);
            return nd;
        }

        private string get_goal(int p)
        {
            string ans = "0";
            for (char i = (char)(48 + p - 1); i > '0'; i--)
                ans = i + ans;
            return ans;
        }

        bool is_goal(string state)
        {
            for (int i = 0; i < state.Length; i++)
                if (state[i] != goal[i])
                    return false;
            return true;
        }

        void solve(bfs_node nd)
        {
            while (q.Count >= 1 && !is_goal_done)
            {
                nd = q.Dequeue();
                get_child(nd);
            }
        }

        void path(bfs_node nd)
        {
            bfs_node tmp = nd;
            while (tmp != null)
            {
                solution.Push(tmp.state);
                tmp = tmp.parent;
            }
        }

        void get_child(bfs_node nd)
        {
            if (is_goal_done) return;
            int idx = get_idx(nd.state), x = (int)Math.Sqrt(nd.state.Length);
            if (idx >= x) // move top
            {
                string c_state = generate_state(nd.state, idx - x);
                if (!visited.is_visited(c_state)) add(nd, c_state);
            }
            if (idx % x < x - 1 && !is_goal_done) // move right
            {
                string c_state = generate_state(nd.state, idx + 1);
                if (!visited.is_visited(c_state)) add(nd, c_state);
            }
            if (idx < (x - 1) * x && !is_goal_done) // move down
            {
                string c_state = generate_state(nd.state, idx + x);
                if (!visited.is_visited(c_state)) add(nd, c_state);
            }
            if (idx % x > 0 && !is_goal_done) // move left
            {
                string c_state = generate_state(nd.state, idx - 1);
                if (!visited.is_visited(c_state)) add(nd, c_state);
            }
        }

        private int get_idx(string state)
        {
            for (int i = 0; i < state.Length; i++)
            {
                if (state[i] == 48)
                    return i;
            }
            return -1;
        }

        private void add(bfs_node parent, string c_state)
        {
            bfs_node ch_nd = create_node(c_state, parent);
            is_goal_done = is_goal(c_state) ? true : false;
            if (is_goal_done) path(ch_nd);
        }

        private string generate_state(string state, int idx)
        {
            string ans = state;
            ans = ans.Replace((char)48, (char)1000);
            ans = ans.Replace(ans[idx], (char)48);
            ans = ans.Replace((char)1000, state[idx]);
            return ans;
        }

    }
}

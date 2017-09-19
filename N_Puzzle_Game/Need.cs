using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Puzzle_Game
{
    public class Need
    {
        public int[] dy = { 1, 0, -1, 0 };
        public int[] dx = { 0, -1, 0, 1 };

        public int N { set; get; }
        public int[,] goal, initial_state;

        public int[,] get_destination()
        {
            int[,] c_state = new int[N, N];
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    c_state[i, j] = (i * N + j + 1) % (N * N);
            return c_state;
        }

        public void copy(int[,] c_state, int[,] state)
        {
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    c_state[i, j] = state[i, j];
        }

        public bool is_goal(int[,] state)
        {
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    if (state[i, j] != goal[i, j])
                        return false;
            return true;
        }

        public string to_string(int[,] state)
        {
            string ans = "";
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    ans += (char)(48 + state[i, j]);
            return ans;
        }

        public int get_cost(int[,] state)
        {
            int cost = 0;
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    if (state[i, j] != goal[i, j])
                        cost++;
            return cost;
        }

        public bool is_safe(int x, int y)
        {
            return x >= 0 && x < N && y >= 0 && y < N;
        }

        public void set_goal(int[,] c_state)
        {
            goal = c_state;
        }

        public void set_state(int[,] c_state)
        {
            initial_state = c_state;
        }

    }
}

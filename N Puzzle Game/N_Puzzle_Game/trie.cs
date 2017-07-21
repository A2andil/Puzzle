using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Puzzle_Game
{
    class trie
    {
        class Node 
        {
            public Node[] child;
            public int count;
        }

        Node root = new Node();
        int size = 0;

        public trie(int mx) 
        {
            size = mx;
            root.count = 0;
            root.child = new Node[size];
        }

        public void add(string state)  
        {
            Node tmp = root;
            int idx = 0, x;
            while (idx < state.Length)
            {
                x = state[idx] - 48;
                if (tmp.child[x] == null)
                {
                    tmp.child[x] = new Node();
                    tmp.child[x].child = new Node[size];
                    tmp.child[x].count = 0;
                }
                tmp.child[x].count += 1;
                idx += 1;
                tmp = tmp.child[x];
            }
        }

        public bool is_visited(string state) 
        {
            Node tmp = root;
            for (int i = 0; i < state.Length; i++)
            {
                int x = state[i] - 48;
                if (tmp.child[x] != null)
                    tmp = tmp.child[x];
                else return false;
            }
            return tmp.count > 0;
        }

    }
}

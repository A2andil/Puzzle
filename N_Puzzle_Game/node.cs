using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Puzzle_Game
{
    public class node
    {
        public int level = 0, cost = 0, x = 0, y = 0;
        public int[,] state;
        public node parent = null;
    }
}

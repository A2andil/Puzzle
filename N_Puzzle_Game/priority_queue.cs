using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Puzzle_Game
{
    class priority_queue
    {
        List<node> lst = new List<node>();

        void swap(node a, node b, int idx, int small_idx)
        {
            lst[idx] = b; lst[small_idx] = a;
        }

        private void minheapify(int idx)
        {
            int left = 2 * idx + 1, right = 2 * idx + 2, small_idx = idx;
            small_idx = left < lst.Count
                && lst[left].cost < lst[idx].cost ? left : idx;
            small_idx = right < lst.Count
                && lst[right].cost < lst[small_idx].cost ? right : small_idx;
            if (small_idx != idx)
            {
                swap(lst[idx], lst[small_idx], idx, small_idx);
                minheapify(small_idx);
            }
        }

        public node pop()
        {
            if (lst.Count > 0)
            {
                node nd = lst[0];
                lst[0] = lst[lst.Count - 1]; 
                lst.RemoveAt(lst.Count - 1);
                minheapify(0);
                return nd;
            }
            return null;
        }

        public bool empty()
        {
            return lst.Count == 0;
        }

        public void push(node nd)
        {
            lst.Add(nd);
            update(lst.Count - 1);
        }

        private void update(int idx)
        {
            while (idx != 0 && lst[(idx - 1) / 2].cost > lst[idx].cost)
            {
                swap(lst[idx], lst[(idx - 1) / 2], idx, (idx - 1) / 2);
                idx = (idx - 1) / 2;
            }
        }

    }
}

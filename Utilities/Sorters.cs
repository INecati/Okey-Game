using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OkeyGame.Entities;
using OkeyGame.Managers;

namespace OkeyGame.Utilities
{
    public class StoneSorterByColorAndNumber : IComparer<Stone>
    {
        public int Compare(Stone c1, Stone c2)
        {
            return (c1.Number + (int)c1.Color * GameManager.StoneNumberLenth).CompareTo(c2.Number + (int)c2.Color * GameManager.StoneNumberLenth);
        }
    }
    public class StoneSorterByNumber : IComparer<Stone>
    {
        public int Compare(Stone c1, Stone c2)
        {
            return c1.Number.CompareTo(c2.Number);
        }
    }
}

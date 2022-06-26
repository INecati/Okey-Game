using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OkeyGame.Exceptions;

namespace OkeyGame.Entities
{
    public enum StoneColor
    {
        None = 0,
        Yellow = 1,
        BLue = 2,
        Black = 3,
        Red = 4,
    }
    public class Stone
    {
        public bool IsIndicatorStone;
        public bool IsOkey;
        public bool IsFakeOkey { get; private set; }
        public int Number { get; private set; }
        public StoneColor Color { get; private set; }
        public Stone( StoneColor color, int number, bool isFakeOkey=false)
        {
            this.Color = color;
            this.Number = number;
            this.IsFakeOkey = isFakeOkey;
        }
        public void SetFakeOkeyValue(StoneColor color, int number)
        {
            if (!IsFakeOkey)
                throw new GameLogicException("The stone must be a FakeOkey to change its value");
            this.Color = color;
            this.Number = number;
        }
        public void SetOkeyNumber(StoneColor color, int number)
        {
            if (!IsOkey)
                throw new GameLogicException("The stone must be an Okey to change its value");
            this.Color = color;
            this.Number = number;
        }
    }
}

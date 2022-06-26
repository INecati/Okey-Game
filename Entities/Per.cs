using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OkeyGame.Managers;

namespace OkeyGame.Entities
{
    public abstract class Per
    {
        public bool IsValidPer { get; protected set; }
        public int Count { get; protected set; }
        public abstract Stone[] GetPerStones();
    }
    public class NumberPer : Per
    {
        public StoneColor Color { get; private set; } = StoneColor.None;
        private Stone[] perStones = new Stone[13];
        private void SetIsValidPer()
        {
            if (Count < GameManager.ValidPerMinStoneCount)
            {
                IsValidPer = false;
                return;
            }

            bool isValid = true;
            bool startingPosFound = false;
            bool endPosFound = false;
            for(int i = 0; i < perStones.Length; i++)
            {
                if (perStones[i] != null)
                {
                    
                    startingPosFound = true;
                    if (endPosFound)
                    {
                        isValid = false;
                        break;
                    }
                }
                else
                {
                    if (startingPosFound)
                    {
                        endPosFound = true;
                    }
                }
            }
            IsValidPer = isValid;
        }
        /// <summary>
        /// Tries to add a stone to the number per
        /// </summary>
        /// <returns>Return true if successful</returns>
        public bool TryAdd(Stone stone, bool after13=false)
        {
            bool added = false;
            if(!(after13 && stone.Number != 1))
            {
                int stoneIndex = after13 ? GameManager.StoneNumberLenth : stone.Number - 1;
                if (perStones[stoneIndex] == null && (stone.Color==Color || Color==StoneColor.None))
                {
                    perStones[stoneIndex] = stone;
                    Color = stone.Color;
                    Count++;
                    added = true;
                    SetIsValidPer();
                }
            }
            return added;
        }

        public override Stone[] GetPerStones()
        {
            return perStones.Where(p=>p!=null).ToArray();
        }
    }
    public class ColorPer : Per
    {
        public int NumberRestriction { get; private set; }
        private bool[] colorSlots = new bool[4];
        public List<Stone> perStoneList = new List<Stone>();
        /// <summary>
        /// Tries to add a stone to the color per
        /// </summary>
        /// <returns>Return true if successful</returns>
        public bool TryAdd(Stone stone)
        {
            bool added = false;
            if (NumberRestriction==0)
            {
                NumberRestriction = stone.Number;
                perStoneList.Add(stone);
                if(!stone.IsOkey)
                    colorSlots[(int)stone.Color - 1] = true;
                added = true;
            }
            else
            {
                if((stone.Number!=NumberRestriction && !stone.IsOkey) || colorSlots[(int)stone.Color - 1] || perStoneList.Count==4)
                {
                    added = false;
                }
                else
                {
                    perStoneList.Add(stone);
                    if (!stone.IsOkey)
                        colorSlots[(int)stone.Color - 1] = true;
                    added = true;
                }
            }
            if (added)
            {
                IsValidPer = perStoneList.Count >= GameManager.ValidPerMinStoneCount;
                Count++;
            }
            return added;
            
        }

        public override Stone[] GetPerStones()
        {
            return perStoneList.ToArray();
        }
    }
}

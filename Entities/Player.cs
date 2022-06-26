using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OkeyGame.Utilities;
using OkeyGame.Managers;

namespace OkeyGame.Entities
{
    public class Player
    {
        public string Name { get; private set; }
        
        public bool isStartingPlayer;
        public List<Stone> stones = new List<Stone>();
        private List<Stone> notPerStoneList;
        private List<Per> perList = new List<Per>();
        public int NotPerStoneCount { get; private set; }
        public Player(string name)
        {
            this.Name = name;
        }
        /// <summary>
        /// Creates Pers for the player
        /// </summary>
        public void AutoPerStones()
        {
            NotPerStoneCount = stones.Count;

            //number per priority calculation
            Stone[] stoneArray = new Stone[stones.Count];
            stones.CopyTo(stoneArray);
            notPerStoneList = new List<Stone>(stoneArray);
            PerByNumber(notPerStoneList);
            PerByColor(notPerStoneList);
            int result = CalcNotPerStoneCount(notPerStoneList);
            if (result < NotPerStoneCount)
                NotPerStoneCount = result;

            //color per priority calculation
            stoneArray = new Stone[stones.Count];
            stones.CopyTo(stoneArray);
            notPerStoneList = new List<Stone>(stoneArray);
            PerByColor(notPerStoneList);
            PerByNumber(notPerStoneList);
            result = CalcNotPerStoneCount(notPerStoneList);
            if (result < NotPerStoneCount)
                NotPerStoneCount = result;
        }
        /// <summary>
        /// Calculates the number of not per stones in the players hand and returns it
        /// </summary>
        private int CalcNotPerStoneCount(List<Stone> notPerStoneList)
        {
            int notPerStoneCount = notPerStoneList.Count;
            int okeyStoneCount = notPerStoneList.Where(p => p.IsOkey).Count();
            if (okeyStoneCount == 0)
                return notPerStoneCount;
            int twoStonePerCount = perList.Where(p => p.Count == GameManager.ValidPerMinStoneCount-1).Count();
            int newValidPerCount = Math.Min(okeyStoneCount, twoStonePerCount);

            okeyStoneCount -= newValidPerCount;
            notPerStoneCount -= newValidPerCount * GameManager.ValidPerMinStoneCount;

            if (okeyStoneCount > 0)
            {
                if (okeyStoneCount == 2 && perList.Where(p => p.Count == GameManager.ValidPerMinStoneCount - 2).Count() > 0)
                {
                    notPerStoneCount -= GameManager.ValidPerMinStoneCount;
                }
                else
                    notPerStoneCount--;
            }
            return notPerStoneCount;
        }
        /// <summary>
        /// Creates color pers from given stone list
        /// </summary>
        private void PerByColor(List<Stone> stoneList, bool isSecondTime = false)
        {
            stoneList.Sort(new StoneSorterByNumber());
            

            ColorPer currentPer = new ColorPer();
            int lastAddedStoneNumber = 0;
            StoneColor lastAddedStoneColor = StoneColor.None;
            List<Stone> skippedStones = new List<Stone>();
            foreach (var stone in stoneList)
            {
                if(!stone.IsOkey)
                {
                    if (!(lastAddedStoneNumber == stone.Number && lastAddedStoneColor == stone.Color))
                    {
                        if (lastAddedStoneNumber == stone.Number && lastAddedStoneColor != stone.Color && currentPer.TryAdd(stone))
                        {
                            lastAddedStoneNumber = stone.Number;
                            lastAddedStoneColor = stone.Color;
                        }
                        else
                        {
                            if (currentPer.Count > 1)
                                perList.Add(currentPer);
                            currentPer = new ColorPer();
                            currentPer.TryAdd(stone);
                            lastAddedStoneNumber = stone.Number;
                            lastAddedStoneColor = stone.Color;
                        }
                    }
                    else
                    {
                        skippedStones.Add(stone);
                    }
                }
            }
            if (currentPer.Count > 1)
                perList.Add(currentPer);
            //Runs the method a second time for the skippedStones
            if (!isSecondTime)
                PerByColor(skippedStones, true);
            //Removes the per stones from the not per stone list
            else
                foreach (var per in perList.Where(p => p.GetType() == typeof(ColorPer) && p.Count >= GameManager.ValidPerMinStoneCount).ToList())
                    foreach (var perStone in per.GetPerStones())
                        notPerStoneList.Remove(perStone);
        }
        /// <summary>
        /// Creates number pers from given stone list
        /// </summary>
        private void PerByNumber(List<Stone> stoneList, bool isSecondTime=false)
        {
            stoneList.Sort(new StoneSorterByColorAndNumber());
            NumberPer currentPer = new NumberPer();
            int lastAddedStoneNumber = 0;
            StoneColor lastAddedStoneColor = StoneColor.None;
            List<Stone> skippedStones = new List<Stone>();
            foreach(var stone in stoneList)
            {
                if (!stone.IsOkey)
                {
                    if (!(lastAddedStoneNumber == stone.Number && lastAddedStoneColor == stone.Color))
                    {
                        if (lastAddedStoneNumber + 1 == stone.Number && lastAddedStoneColor == stone.Color && currentPer.TryAdd(stone))
                        {
                            lastAddedStoneNumber = stone.Number;
                            lastAddedStoneColor = stone.Color;
                        }
                        else
                        {
                            if (currentPer.Count > 1)
                                perList.Add(currentPer);
                            currentPer = new NumberPer();
                            currentPer.TryAdd(stone);
                            lastAddedStoneNumber = stone.Number;
                            lastAddedStoneColor = stone.Color;
                        }
                    }
                    else
                    {
                        skippedStones.Add(stone);
                    }
                    
                }
            }
            if (currentPer.Count > 1)
                perList.Add(currentPer);
            //Runs the method a second time for the skippedStones
            if(!isSecondTime)
                PerByNumber(skippedStones, true);
            //Removes the per stones from the not per stone list
            else
                foreach (var per in perList.Where(p => p.GetType() == typeof(NumberPer) && p.Count >= GameManager.ValidPerMinStoneCount).ToList())
                    foreach (var perStone in per.GetPerStones())
                        notPerStoneList.Remove(perStone);
        }
    }
}

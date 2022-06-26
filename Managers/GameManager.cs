using OkeyGame.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OkeyGame.Entities;

namespace OkeyGame.Managers
{

    public class GameManager
    {
        public const int StoneNumberLenth = 13;
        public const int PlayerMaxStoneCount = 14;
        public const int ValidPerMinStoneCount = 3;
        public const int AllStonesCount = 106;
        private Player[] players;
        private List<Stone> stoneList = new List<Stone>();
        private Stone[] allStones;
        private Stone[] shuffledStones;
        public void StartGame()
        {

            SetupGame();

            DistributeStonesToPlayers(shuffledStones, players);

            AutoPerPlayerStones(players);

            
        }


        private void AutoPerPlayerStones(Player[] players)
        {
            int minStoneCount = int.MaxValue;
            foreach(var player in players)
            {
                player.AutoPerStones();
                Console.WriteLine("{0} has {1} stones left to finish the game", player.Name, player.NotPerStoneCount);
                if (minStoneCount > player.NotPerStoneCount)
                    minStoneCount = player.NotPerStoneCount;
            }
            string[] winningPlayerNames = players.Where(p => p.NotPerStoneCount == minStoneCount).Select(p=>p.Name).ToArray();
            Console.WriteLine("{0} {1} winning the game", string.Join(", ", winningPlayerNames), winningPlayerNames.Length > 1 ? "are" : "is");
        }

        private void SetupGame()
        {
            players = CreatePlayer();
            Random rnd = new Random();
            players[rnd.Next(players.Length)].isStartingPlayer = true;

            allStones = CreateStones();

            shuffledStones = new Stone[allStones.Length];
            allStones.CopyTo(shuffledStones, 0);
            Utils.Shuffle(shuffledStones);

            Stone indicatorStone = PickIndicatorStone(shuffledStones);
            shuffledStones = shuffledStones.Where(p => p != indicatorStone).ToArray();
            SetOkeys(indicatorStone);
        }
        private Player[] CreatePlayer()
        {
            Player[] players = new Player[4];
            for (int i = 0; i < 4; i++)
                players[i] = new Player("Player " + (i + 1));
            return players;
        }
        private void DistributeStonesToPlayers(Stone[] stones, Player[] players)
        {
            int stoneIndex = 0;
            foreach(var player in players)
            {
                for (int k = 0; k < PlayerMaxStoneCount+Convert.ToInt32(player.isStartingPlayer); k++)
                {
                    if(!stones[stoneIndex].IsIndicatorStone)
                        player.stones.Add(stones[stoneIndex]);
                    else
                        k--;
                    stoneIndex++;
                }
            }
        }
        
        private Stone[] CreateStones()
        {
            Stone[] stones = new Stone[106];
            
            int index = 0;
            for (int i = 0; i < 2; i++)
            {
                StoneColor stoneColor = StoneColor.None;
                for (int k = 0; k < 4; k++)
                {
                    stoneColor = (StoneColor)(int)++stoneColor;
                    for (int stoneValue = 1; stoneValue <= 13; stoneValue++)
                    {
                        stones[index++] = new Stone(stoneColor, stoneValue);
                    }
                }
                stones[index++] = new Stone(StoneColor.None, 0, true);
            }
            return stones;
        }
        private Stone PickIndicatorStone(Stone[] stones)
        {
            Random rnd = new Random();
            Stone selectedStone = stones[rnd.Next(stones.Length)];
            if (!selectedStone.IsFakeOkey)
                return selectedStone;
            else
                return PickIndicatorStone(stones);
        }
        private void SetOkeys(Stone indicatorStone)
        {
            int okeyNumber = indicatorStone.Number+1;
            if (okeyNumber == StoneNumberLenth + 1)
                okeyNumber = 1;
            allStones[okeyNumber - 1 + ((int)indicatorStone.Color - 1) * StoneNumberLenth].IsOkey = true;
            allStones[okeyNumber - 1 + ((int)indicatorStone.Color - 1) * StoneNumberLenth + allStones.Length / 2].IsOkey = true;

            allStones[allStones.Length/2-1].SetFakeOkeyValue(indicatorStone.Color, okeyNumber);
            allStones[allStones.Length-1].SetFakeOkeyValue(indicatorStone.Color, okeyNumber);
        }
    }
}

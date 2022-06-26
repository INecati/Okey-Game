using System;
using OkeyGame.Managers;

namespace OkeyGame
{
    class Program
    {
        static void Main(string[] args)
        {
            GameManager gameManager = new GameManager();
            gameManager.StartGame();

            Console.ReadLine();
        }
    }
}

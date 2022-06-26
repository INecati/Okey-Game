using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using OkeyGame.Entities;

namespace OkeyGame.Utilities
{
    public static class Utils
    {
        private static Random rng = new Random();
        /// <summary>
        /// Shuffle an array with Fisher–Yates algorithm
        /// </summary>
        public static void QuickShuffle<T>(T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
        /// <summary>
        /// Shuffle an array with Fisher–Yates algorithm 
        /// using System.Security.Cryptography for better randomness
        /// </summary>
        public static void Shuffle<T>(this IList<T> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}

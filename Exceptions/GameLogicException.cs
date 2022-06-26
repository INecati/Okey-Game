using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkeyGame.Exceptions
{
    public class GameLogicException : Exception
    {
        public GameLogicException()
            : base()
        { }

        public GameLogicException(String message)
            : base(message)

        { }

        public GameLogicException(String message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

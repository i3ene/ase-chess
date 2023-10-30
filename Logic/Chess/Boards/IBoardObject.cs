using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ase_chess.Logic.Chess.Boards
{
    public interface IBoardObject
    {
        public Position position { get; set; }
        public char icon { get; }
    }
}

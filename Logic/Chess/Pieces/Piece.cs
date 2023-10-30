using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ase_chess.Logic.Chess.Pieces
{
    public class Piece
    {
        public const int UNICODE_OFFSET = 0x2654;

        public PieceType type;
        public Color color;

        public Piece(PieceType type, Color color)
        {
            this.type = type;
            this.color = color;
        }
    }
}

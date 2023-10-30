using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ase_chess.Logic.Chess.Boards;
using ase_chess.Logic.Chess.Pieces;

namespace ase_chess.Logic.Chess.Pieces.Instances
{
    public class BoardPiece : Piece, IBoardObject
    {
        public Position position { get; set; }
        public char icon => getIcon(type, color);

        public BoardPiece(PieceType type, Color color) : base(type, color)
        {
            position = new Position();
        }

        public BoardPiece(PieceType type, Color color, Position position) : base(type, color)
        {
            this.position = position;
        }

        public static char getIcon(PieceType type, Color color)
        {
            int pieces = Enum.GetNames(typeof(PieceType)).Length;
            int code = UNICODE_OFFSET + (int)type + (int)color * pieces;
            return (char)code;
        }
    }
}

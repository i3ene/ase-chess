using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ase_chess.Logic.Chess
{
    public class Player
    {
        public interface Object
        {
            public Player player { get; set; }
        }
        public Color color;
        public List<BoardPiece> pieces;

        public Player(Color color)
        {
            this.color = color;
            this.pieces = new List<BoardPiece>();
        }

        public void addPiece(BoardPiece piece)
        {
            this.pieces.Add(new PlayerPiece(piece, this));
        }

        public void addPieces(BoardPiece[] pieces)
        {
            foreach (BoardPiece piece in pieces)
            {
                addPiece(piece);
            }
        }
    }
}

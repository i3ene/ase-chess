using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ase_chess.Logic.Chess.Pieces.Instances;

namespace ase_chess.Logic.Chess.Players
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
            pieces = new List<BoardPiece>();
        }

        public void addPiece(BoardPiece piece)
        {
            pieces.Add(new PlayerPiece(piece, this));
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

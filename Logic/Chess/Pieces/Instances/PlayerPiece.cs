using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ase_chess.Logic.Chess.Boards;
using ase_chess.Logic.Chess.Pieces;
using ase_chess.Logic.Chess.Players;

namespace ase_chess.Logic.Chess.Pieces.Instances
{
    public class PlayerPiece : BoardPiece, IPlayerObject
    {
        public Player player { get; set; }

        public PlayerPiece(BoardPiece piece, Player player) : base(piece.type, piece.color)
        {
            this.player = player;
            position = piece.position;
        }

        public PlayerPiece(PieceType type, Color color, Player player) : base(type, color)
        {
            this.player = player;
        }

        public PlayerPiece(PieceType type, Color color, Player player, Position position) : base(type, color)
        {
            this.player = player;
            this.position = position;
        }

        public void removeFromPlayer()
        {
            player.pieces.Remove(this);
        }
    }
}

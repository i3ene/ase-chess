using ase_chess.Logic.Chess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ase_chess.Logic
{
    public class Game
    {
        public Color active;
        public Board board;
        public Player[] players = new Player[Enum.GetValues(typeof(Color)).Length];
        public Player currentPlayer => players[(int)active];

        public Game()
        {
            active = Color.White;
            board = new Board();
            foreach (Color color in Enum.GetValues(typeof(Color)))
            {
                players[(int)color] = new Player(color);
                players[(int)color].addPieces(getDefaultBoardPieces(color));
                board.objects.AddRange(players[(int)color].pieces);
            }
        }

        public static BoardPiece[] getDefaultBoardPieces(Color color)
        {
            List<BoardPiece> pieces = new List<BoardPiece>();

            int xMax = Position.MAX - 1;
            int yDirection = color == Color.White ? 1 : -1;
            int yMiddle = Position.MAX / 2 - Math.Max(0, yDirection);
            Func<int, int> yFieldIndex = n => yMiddle - yDirection * n;

            int y = yFieldIndex(2);

            for (int i = 0; i <= xMax; i++)
            {
                Position position = new Position(i, y);
                pieces.Add(new BoardPiece(PieceType.Pawn, color, position));
            }

            y = yFieldIndex(3);

            pieces.Add(new BoardPiece(PieceType.Rook, color, new Position(0, y)));
            pieces.Add(new BoardPiece(PieceType.Rook, color, new Position(xMax, y)));

            pieces.Add(new BoardPiece(PieceType.Rook, color, new Position(1, y)));
            pieces.Add(new BoardPiece(PieceType.Rook, color, new Position(xMax - 1, y)));

            pieces.Add(new BoardPiece(PieceType.Bishop, color, new Position(2, y)));
            pieces.Add(new BoardPiece(PieceType.Bishop, color, new Position(xMax - 2, y)));

            pieces.Add(new BoardPiece(PieceType.King, color, new Position(3, y)));
            pieces.Add(new BoardPiece(PieceType.Queen, color, new Position(xMax - 3, y)));

            return pieces.ToArray();
        }
    }
}

using ase_chess.Logic.Chess.Boards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ase_chess.Client.Rendering.Windows.Instances
{
    internal class BoardWindow : Window
    {
        public const char gridCross = '\u253c';
        public const char gridVertical = '\u2502';
        public const char gridHorizontal = '\u2500';

        public Board board;

        public BoardWindow(Board board) : base(48, 21)
        {
            this.board = board;
            border = BorderStyle.Round;
            horizontalAlign = HorizontalAlignment.Center;
            verticalAlign = VerticalAlignment.Middle;
        }

        public override int getLineCount()
        {
            return (Position.MAX - 1) * 2;
        }

        public override string getLine(int y)
        {
            if (y < 0 || y > (Position.MAX - 1) * 2) return "";
            if (y % 2 == 0)
            {
                y /= 2;
                string line = "";
                for (int x = 0; x < Position.MAX; x++)
                {
                    var obj = board.getObject(x, y);
                    char icon = obj is null ? ' ' : obj.icon;
                    if (x != 0) line += gridVertical.ToString();
                    string field = $" {icon} ";
                    if (x % 2 == 0 && y % 2 == 0 || x % 2 == 1 && y % 2 == 1)
                        field = field.Col(Format.Color.BROWN, true);
                    line += field;
                }

                return line;
            }
            else
            {
                return rowDivider();
            }
        }

        public string rowDivider()
        {
            string line = $"{gridHorizontal}{gridHorizontal}{gridHorizontal}";
            for (int x = 0; x < Position.MAX - 1; x++)
            {
                line += $"{gridCross}{gridHorizontal}{gridHorizontal}{gridHorizontal}";
            }

            return line;
        }
    }
}

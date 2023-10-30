using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ase_chess.Logic.Chess
{
    public struct Position : IEquatable<Position>
    {
        private const int CHAR_OFFSET = 41;
        public const int MAX = 8;
        private int _x;
        public int x
        {
            get { return _x; }
            set { _x = value % MAX; }
        }
        public char cx
        {
            get { return toChar(x); }
            set { x = toInt(value); }
        }
        private int _y;
        public int y
        {
            get { return _y; }
            set { _y = value % MAX; }
        }

        public Position()
        {
            x = 0;
            y = 0;
        }

        public Position(char x, int y)
        {
            cx = x;
            this.y = y;
        }

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public string getNotation()
        {
            return $"{cx}{y}";
        }

        public static char toChar(int n)
        {
            return (char)(CHAR_OFFSET + n);
        }

        public static int toInt(char n)
        {
            return char.ToUpper(n) - CHAR_OFFSET;
        }

        public bool Equals(Position p)
        {
            if (ReferenceEquals(this, p))
            {
                return true;
            }

            if (GetType() != p.GetType())
            {
                return false;
            }

            return x == p.x && y == p.y;
        }

        public static bool operator ==(Position lhs, Position rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Position lhs, Position rhs) => !(lhs == rhs);

        public override bool Equals(object obj)
        {
            return obj is Position && Equals((Position)obj);
        }

        public override int GetHashCode() => (x, y).GetHashCode();
    }
}

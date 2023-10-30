using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ase_chess.Logic.Chess.Boards
{
    public class Board
    {
        public List<IBoardObject> objects;

        public Board(List<IBoardObject> objects)
        {
            this.objects = objects;
        }

        public Board()
        {
            objects = new List<IBoardObject>();
        }

        public IBoardObject? getObject(int x, int y)
        {
            return objects.Find(o => o.position == new Position(x, y));
        }
    }
}

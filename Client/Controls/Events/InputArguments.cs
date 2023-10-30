using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ase_chess.Client.Controls;

namespace ase_chess.Client.Controls.Events
{
    public class InputArguments : ControlArguments
    {
        public char key;

        public InputArguments(Control control, char key) : base()
        {
            this.key = key;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ase_chess.Client.Controls;

namespace ase_chess.Client.Controls.Events
{
    public class CommandArguments : ControlArguments
    {
        public string[] arguments;

        public CommandArguments(Control control, string[] arguments) : base()
        {
            this.arguments = arguments;
        }

        public CommandArguments(Control control, string command) : base()
        {
            arguments = command.Split(' ');
        }
    }
}

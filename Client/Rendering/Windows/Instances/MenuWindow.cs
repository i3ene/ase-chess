using ase_chess.Client.Controls.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ase_chess.Client.Controls;

namespace ase_chess.Client.Rendering.Windows.Instances
{
    public class MenuWindow : Window, IControllable
    {
        public MenuWindow() : base(64, 32)
        {

        }

        public void OnInput(Control sender, InputArguments args)
        {
            switch (args.key)
            {
                case '\u2191':
                    // TODO: Navigate up
                    args.handled = true;
                    break;
                case '\u2193':
                    // TODO: Navigate down
                    args.handled = true;
                    break;
            }
        }
    }
}

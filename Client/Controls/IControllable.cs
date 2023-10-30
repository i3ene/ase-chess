using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ase_chess.Client.Controls.Events;

namespace ase_chess.Client.Controls
{
    public interface IControllable
    {
        public void OnInput(Control sender, InputArguments args);
    }
}

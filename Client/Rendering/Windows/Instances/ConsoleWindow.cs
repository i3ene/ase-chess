using ase_chess.Client.Controls.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ase_chess.Client.Controls;

namespace ase_chess.Client.Rendering.Windows.Instances
{
    public class ConsoleWindow : Window, IControllable, ICommandable
    {

        public ConsoleWindow(int width, int height) : base(width, height)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Update += (window) => render();
        }

        public new void render()
        {
            Console.Clear();
            for (int i = 0; i < height; i++)
            {
                Console.WriteLine(renderLine(i));
            }
        }

        public void OnInput(Control sender, InputArguments input)
        {
            if (input.key == '\u241b')
            {
                sender.StopListening();
                input.handled = true;
            }
        }

        public void OnCommand(Control sender, CommandArguments command)
        {
            if (command.arguments[0].ToLower() == "exit")
            {
                sender.StopListening();
                command.handled = true;
            }
        }
    }
}

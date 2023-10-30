using ase_chess.Client.Controls.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ase_chess.Client.Controls;

namespace ase_chess.Client.Rendering.Windows.Instances
{
    public class ChatWindow : Window, ICommandable
    {
        public ChatWindow() : base(64, 8)
        {
            addLine("--This could be a chat--".Col(Format.Color.GREEN));
            addLine("Message1");
            addLine("Message2");
        }

        public override void addLine(string line)
        {
            int offset = border == BorderStyle.None ? 1 : 3;
            if (lines.Count > height - offset)
            {
                lines.RemoveAt(1);
            }
            base.addLine(line);
            NeedsUpdate();
        }

        public void OnCommand(Control sender, CommandArguments command)
        {
            if (command.arguments[0].ToLower() == "say")
            {
                string line = "";
                for (int i = 1; i < command.arguments.Length; i++)
                {
                    line += command.arguments[i] + " ";
                }
                addLine(line.Trim());
            }
        }
    }
}

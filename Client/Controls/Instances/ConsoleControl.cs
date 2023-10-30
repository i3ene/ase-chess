using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ase_chess.Client.Controls.Events;

namespace ase_chess.Client.Controls.Instances
{
    public class ConsoleControl : Control
    {
        public readonly string allowedSpecialCharactes = " !\"§$%&/()=?²³{[]}\\'#+-*~.:;,<>|`´^°@µ";

        public override void Handler()
        {
            var key = Console.ReadKey(true);
            var input = new InputArguments(this, parseKey(key));
            InputEvent(input);
            if (input.handled) return;
            switch (key.Key)
            {
                case ConsoleKey.Backspace:
                    if (buffer.Length > 0)
                    {
                        Console.Write("\b \b");
                        buffer = buffer.Substring(0, buffer.Length - 1);
                    }
                    break;
                case ConsoleKey.Enter:
                    var command = new CommandArguments(this, buffer);
                    foreach (char c in buffer)
                    {
                        Console.Write("\b \b");
                    }
                    buffer = "";
                    CommandEvent(command);
                    break;
                default:
                    if (char.IsLetterOrDigit(key.KeyChar) || allowedSpecialCharactes.Contains(key.KeyChar))
                    {
                        Console.Write(key.KeyChar);
                        buffer += key.KeyChar;
                    }
                    break;
            }
        }

        public char parseKey(ConsoleKeyInfo info)
        {
            switch (info.Key)
            {
                case ConsoleKey.UpArrow: return '\u2191';
                case ConsoleKey.DownArrow: return '\u2193';
                case ConsoleKey.LeftArrow: return '\u2190';
                case ConsoleKey.RightArrow: return '\u2192';
                case ConsoleKey.Tab: return '\u21b9';
                case ConsoleKey.Enter: return '\u21b2';
                case ConsoleKey.Backspace: return '\u21a4';
                case ConsoleKey.Escape: return '\u241b';
            }

            return info.KeyChar;
        }
    }
}

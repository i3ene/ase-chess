using ase_chess.Client.Controls.Instances;
using ase_chess.Client.Rendering.Windows.Instances;
using ase_chess.Client.Rendering.Windows;
using ase_chess.Client.Rendering;
using ase_chess.Logic;

namespace ase_chess
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            ConsoleControl control = new ConsoleControl();

            var game = new Game();

            var console = new ConsoleWindow(64, 32);
            Window main = new Window(64, 24, HorizontalAlignment.Center, VerticalAlignment.Middle);
            Window history = new Window(16, 24, HorizontalAlignment.Center, VerticalAlignment.Top, BorderStyle.Round);
            BoardWindow board = new BoardWindow(game.board);
            ChatWindow chat = new ChatWindow() {
                border = BorderStyle.Round
            };

            history.addLine("--History--".Col(Format.Color.RED));
            history.addLine("Move1");
            history.addLine("Move2");
            history.addLine("Move3");

            main.addWindow(history, 0, 0);
            main.addWindow(board, 1, 0);

            console.addWindow(main);
            console.addWindow(chat, 0, 1);
            console.render();

            control.Register(console);
            control.Register(chat);

            await control.Listen();
        }
    }
}
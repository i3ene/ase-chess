using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ase_chess.Client.Controls.Events;

namespace ase_chess.Client.Controls
{
    public abstract class Control
    {
        public delegate void InputHandler(Control sender, InputArguments args);
        public event InputHandler Input;

        public delegate void CommandHandler(Control sender, CommandArguments args);
        public event CommandHandler Command;

        public string buffer = "";

        private bool listening = false;

        public async Task Listen()
        {
            listening = true;
            await Task.Factory.StartNew(() =>
            {
                while (listening)
                {
                    Handler();
                }
            });
        }

        public void StopListening()
        {
            listening = false;
        }

        public abstract void Handler();

        public void InputEvent(InputArguments args)
        {
            Input?.Invoke(this, args);
        }

        public void CommandEvent(CommandArguments args)
        {
            Command?.Invoke(this,args);
        }

        public void Register(object[] listeners)
        {
            foreach (var listener in listeners)
            {
                Register(listener);
            }
        }

        public void Register(object listener)
        {
            if (listener is IControllable controllable)
            {
                Input += controllable.OnInput;
            }

            if (listener is ICommandable commandable)
            {
                Command += commandable.OnCommand;
            }
        }

        public void Unregister(object[] listeners)
        {
            foreach (var listener in listeners)
            {
                Unregister(listener);
            }
        }

        public void Unregister(object listener)
        {
            if (listener is IControllable controllable)
            {
                Input -= controllable.OnInput;
            }

            if (listener is ICommandable commandable)
            {
                Command -= commandable.OnCommand;
            }
        }
    }
}

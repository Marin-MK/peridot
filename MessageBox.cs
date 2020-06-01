using System;
using System.Collections.Generic;
using System.Text;
using static SDL2.SDL;

namespace Peridot
{
    public class MessageBox
    {
        public ODL.Window Parent;
        public string Title;
        public string Message;
        public int IconType;
        public List<string> Buttons;

        public MessageBox(ODL.Window Parent, string Title, string Message, int IconType, List<string> Buttons)
        {
            this.Parent = Parent;
            this.Title = Title;
            this.Message = Message;
            this.IconType = IconType;
            this.Buttons = Buttons;
        }

        public int Show()
        {
            SDL_MessageBoxData boxdata = new SDL_MessageBoxData();
            if (this.IconType == 1) boxdata.flags = SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION;
            else if (this.IconType == 2) boxdata.flags = SDL_MessageBoxFlags.SDL_MESSAGEBOX_WARNING;
            else if (this.IconType == 3) boxdata.flags = SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR;
            boxdata.buttons = new SDL_MessageBoxButtonData[Buttons.Count];
            for (int i = 0; i < Buttons.Count; i++)
            {
                boxdata.buttons[i] = new SDL_MessageBoxButtonData();
                boxdata.buttons[i].text = Buttons[i];
                boxdata.buttons[i].buttonid = 0;
                if (i == 0) boxdata.buttons[i].flags = SDL_MessageBoxButtonFlags.SDL_MESSAGEBOX_BUTTON_RETURNKEY_DEFAULT;
                else if (i == Buttons.Count - 1) boxdata.buttons[i].flags = SDL_MessageBoxButtonFlags.SDL_MESSAGEBOX_BUTTON_ESCAPEKEY_DEFAULT;
            }
            boxdata.message = this.Message;
            boxdata.numbuttons = Buttons.Count;
            boxdata.title = this.Title;
            boxdata.window = this.Parent == null ? IntPtr.Zero : this.Parent.SDL_Window;
            int result = -1;
            SDL_ShowMessageBox(ref boxdata, out result);
            return result;
        }
    }

    public class StandardBox : MessageBox
    {
        public StandardBox(ODL.Window Parent, string Message)
            : base(Parent, Parent == null ? "peridot" : Parent.Text, Message, 0, new List<string>() { "OK" })
        {

        }
    }

    public class InfoBox : MessageBox
    {
        public InfoBox(ODL.Window Parent, string Message)
            : base(Parent, "Info", Message, 1, new List<string>() { "OK" })
        {

        }
    }

    public class WarningBox : MessageBox
    {
        public WarningBox(ODL.Window Parent, string Message)
            : base(Parent, "Warning", Message, 2, new List<string>() { "OK" })
        {

        }
    }

    public class ErrorBox : MessageBox
    {
        public ErrorBox(ODL.Window Parent, string Message)
            : base(Parent, "Error", Message, 3, new List<string>() { "OK" })
        {

        }
    }
}

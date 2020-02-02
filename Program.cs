using System;
using ODL;
using RubyDotNET;
using static SDL2.SDL;

namespace odlgss
{
    public class Program
    {
        public static Window MainWindow;
        public static bool Looping = true;

        static void Main(string[] args)
        {
            MainWindow = new Window();
            MainWindow.SetResizable(false);
            MainWindow.SetSize(512, 384);
            MainWindow.Show();

            Internal.Initialize();

            Graphics.CreateClass();
            Sprite.CreateClass();
            Rect.CreateClass();
            Viewport.CreateClass();
            Table.CreateClass();
            Bitmap.CreateClass();
            Color.CreateClass();
            Font.CreateClass();

            Graphics.Width = MainWindow.Width;
            Graphics.Height = MainWindow.Height;

            while (Looping && ODL.Graphics.CanUpdate()) Loop();
        }

        public static void Debug()
        {
            Viewport v = new Viewport(0, 0, 512, 384);
            ODL.Sprite s = new ODL.Sprite(v.ViewportObject, new SolidBitmap(999, 999, new ODL.Color(255, 0, 0)));
            Internal.SetGlobalVariable("$v", v.Pointer);
        }

        public static void Loop()
        {
            ODL.Graphics.Update();
            if (Input.Trigger(SDL_Keycode.SDLK_RETURN))
            {
                Debug();
            }
        }
    }
}

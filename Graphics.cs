using System;
using rubydotnet;

namespace peridot
{
    public static class Graphics
    {
        public static IntPtr Module;

        public static IntPtr MainViewport;
        public static IntPtr OverlayViewport;
        public static IntPtr OverlaySprite;
        public static IntPtr OverlayBitmap;

        public static void Create()
        {
            Module = Ruby.Module.Define("Graphics");
            Ruby.Module.DefineClassMethod(Module, "frame_rate", frame_rateget);
            Ruby.Module.DefineClassMethod(Module, "frame_rate=", frame_rateset);
            Ruby.Module.DefineClassMethod(Module, "width", widthget);
            Ruby.Module.DefineClassMethod(Module, "width=", widthset);
            Ruby.Module.DefineClassMethod(Module, "height", heightget);
            Ruby.Module.DefineClassMethod(Module, "height=", heightset);
            Ruby.Module.DefineClassMethod(Module, "brightness", brightnessget);
            Ruby.Module.DefineClassMethod(Module, "brightness=", brightnessset);
            Ruby.Module.DefineClassMethod(Module, "frame_count", frame_countget);
            Ruby.Module.DefineClassMethod(Module, "frame_count=", frame_countset);
            Ruby.Module.DefineClassMethod(Module, "screenshot", screenshot);
            Ruby.Module.DefineClassMethod(Module, "snap_to_bitmap", screenshot);
            Ruby.Module.DefineClassMethod(Module, "update", update);
            Ruby.Module.DefineClassMethod(Module, "wait", wait);
        }

        public static void Start()
        {
            odl.Viewport.DefaultWindow = Program.MainWindow;
            MainViewport = Ruby.Funcall(Viewport.Class, "new", Ruby.Integer.ToPtr(0), Ruby.Integer.ToPtr(0), Ruby.GetIVar(Module, "@width"), Ruby.GetIVar(Module, "@height"));
            Ruby.Funcall(MainViewport, "z=", Ruby.Integer.ToPtr(999999998));

            Ruby.SetIVar(Font.Class, "@default_name", Ruby.String.ToPtr("arial"));
            Ruby.SetIVar(Font.Class, "@default_size", Ruby.Integer.ToPtr(32));
            Ruby.SetIVar(Font.Class, "@default_color", Color.CreateColor(odl.Color.WHITE));
            Ruby.SetIVar(Font.Class, "@default_outline", Ruby.False);
            Ruby.SetIVar(Font.Class, "@default_outline_color", Color.CreateColor(odl.Color.BLACK));

            OverlayViewport = Ruby.Funcall(Viewport.Class, "new", Ruby.Integer.ToPtr(0), Ruby.Integer.ToPtr(0), Ruby.GetIVar(Module, "@width"), Ruby.GetIVar(Module, "@height"));
            Ruby.Funcall(OverlayViewport, "z=", Ruby.Integer.ToPtr(999999999));

            Ruby.SetGlobal("$__overlay_viewport__", OverlayViewport);

            OverlaySprite = Ruby.Funcall(Sprite.Class, "new", OverlayViewport);

            Ruby.SetGlobal("$__overlay_sprite__", OverlaySprite);

            OverlayBitmap = Ruby.Funcall(Bitmap.Class, "new", Ruby.GetIVar(Module, "@width"), Ruby.GetIVar(Module, "@height"));
            Ruby.SetGlobal("$__overlay_bitmap__", OverlayBitmap);

            Ruby.Funcall(OverlaySprite, "bitmap=", OverlayBitmap);
            Ruby.Funcall(OverlaySprite, "opacity=", Ruby.Integer.ToPtr(0));
            Ruby.Funcall(OverlaySprite, "z=", Ruby.Integer.ToPtr(999999999));
            Ruby.Funcall(OverlayBitmap, "fill_rect", Ruby.Integer.ToPtr(0), Ruby.Integer.ToPtr(0), Ruby.GetIVar(Module, "@width"), Ruby.GetIVar(Module, "@height"), Color.CreateColor(odl.Color.BLACK));

            Ruby.SetGlobal("$__mainvp__", MainViewport);
            Ruby.SetGlobal("$peridot", Ruby.True);

            Ruby.SetIVar(Module, "@brightness", Ruby.Integer.ToPtr(255));
            Ruby.SetIVar(Module, "@frame_count", Ruby.Integer.ToPtr(0));

            int fps = Config.FrameRate;
            if (Config.VSync)
            {
                SDL2.SDL.SDL_DisplayMode mode = new SDL2.SDL.SDL_DisplayMode();
                SDL2.SDL.SDL_GetWindowDisplayMode(Program.MainWindow.SDL_Window, out mode);
                fps = mode.refresh_rate;
            }
            Ruby.SetIVar(Module, "@frame_rate", Ruby.Integer.ToPtr(fps));
        }

        static IntPtr frame_rateget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@frame_rate");
        }

        static IntPtr frame_rateset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "Integer");
            return Ruby.SetIVar(Self, "@frame_rate", Ruby.Array.Get(Args, 0));
        }

        static IntPtr widthget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@width");
        }

        static IntPtr widthset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "Integer");
            return Ruby.SetIVar(Self, "@width", Ruby.Array.Get(Args, 0));
        }

        static IntPtr heightget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@height");
        }

        static IntPtr heightset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "Integer");
            return Ruby.SetIVar(Self, "@height", Ruby.Array.Get(Args, 0));
        }

        static IntPtr brightnessget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@brightness");
        }

        static IntPtr brightnessset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            IntPtr brightness = IntPtr.Zero;
            if (Ruby.Array.Is(Args, 0, "Float"))
            {
                double v = Ruby.Float.FromPtr(Ruby.Array.Get(Args, 0));
                if (v < 0) brightness = Ruby.Float.ToPtr(0);
                else if (v > 255) brightness = Ruby.Float.ToPtr(255);
                else brightness = Ruby.Array.Get(Args, 0);
                Ruby.Funcall(OverlaySprite, "opacity=", Ruby.Integer.ToPtr((int) Math.Round(255 - v)));
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                int v = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
                if (v < 0) brightness = Ruby.Integer.ToPtr(0);
                else if (v > 255) brightness = Ruby.Integer.ToPtr(255);
                else brightness = Ruby.Array.Get(Args, 0);
                Ruby.Funcall(OverlaySprite, "opacity=", Ruby.Integer.ToPtr(255 - v));
            }
            return Ruby.SetIVar(Self, "@brightness", brightness);
        }

        static IntPtr frame_countget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@frame_count");
        }

        static IntPtr frame_countset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "Integer");
            return Ruby.SetIVar(Self, "@frame_count", Ruby.Array.Get(Args, 0));
        }

        static IntPtr screenshot(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Bitmap.CreateBitmap(odl.Graphics.Windows[0].Screenshot());
        }

        static IntPtr update(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            if (!odl.Graphics.Initialized) Ruby.Raise(Ruby.ErrorType.SystemExit, "game stopped");

            odl.Graphics.UpdateInput();
            odl.Graphics.UpdateWindows();
            try
            {
                odl.Graphics.UpdateGraphics(true);
            }
            catch (odl.BitmapLockedException)
            {
                Ruby.Raise(Ruby.ErrorType.RuntimeError, "attempted to render a still unlocked bitmap");
            }
            return Ruby.True;
        }

        static IntPtr wait(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "Integer");
            SDL2.SDL.SDL_Delay((uint) (1000d / Ruby.Integer.FromPtr(Ruby.GetIVar(Self, "@frame_rate")) * Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0))));
            return Ruby.True;
        }
    }
}

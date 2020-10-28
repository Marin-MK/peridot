using System;
using System.Diagnostics;
using odl.SDL2;
using rubydotnet;

namespace peridot
{
    public static class System
    {
        public static IntPtr Module;

        public static IntPtr MainViewport;
        static IntPtr OverlayViewport;
        static IntPtr OverlaySprite;
        static bool ManualSync = false;
        static int MillisecondsPerFrame = -1;
        static Stopwatch FPSTimer = new Stopwatch();

        public static void Create()
        {
            Module = Ruby.Module.Define("System");
            Ruby.Module.DefineClassMethod(Module, "frame_rate", frame_rateget);
            Ruby.Module.DefineClassMethod(Module, "frame_rate=", frame_rateset);
            Ruby.Module.DefineClassMethod(Module, "render_speed", render_speedget);
            Ruby.Module.DefineClassMethod(Module, "render_speed=", render_speedset);
            Ruby.Module.DefineClassMethod(Module, "width", widthget);
            Ruby.Module.DefineClassMethod(Module, "width=", widthset);
            Ruby.Module.DefineClassMethod(Module, "height", heightget);
            Ruby.Module.DefineClassMethod(Module, "height=", heightset);
            Ruby.Module.DefineClassMethod(Module, "frame_count", frame_countget);
            Ruby.Module.DefineClassMethod(Module, "frame_count=", frame_countset);
            Ruby.Module.DefineClassMethod(Module, "vsync", vsyncget);
            Ruby.Module.DefineClassMethod(Module, "vsync=", vsyncset);
            Ruby.Module.DefineClassMethod(Module, "show_overlay", show_overlay);
            Ruby.Module.DefineClassMethod(Module, "hide_overlay", hide_overlay);
            Ruby.Module.DefineClassMethod(Module, "overlay_shown?", overlay_shown);
            Ruby.Module.DefineClassMethod(Module, "screenshot", screenshot);
            Ruby.Module.DefineClassMethod(Module, "update", update);
            Ruby.Module.DefineClassMethod(Module, "wait", wait);
            Ruby.Module.DefineClassMethod(Module, "windows?", windows);
            Ruby.Module.DefineClassMethod(Module, "linux?", linux);
            Ruby.Module.DefineClassMethod(Module, "mac?", mac);
        }

        public static void Start()
        {
            odl.Viewport.DefaultWindow = Program.MainWindow;
            MainViewport = Ruby.Funcall(Viewport.Class, "new", Ruby.Integer.ToPtr(0), Ruby.Integer.ToPtr(0), Ruby.GetIVar(Module, "@width"), Ruby.GetIVar(Module, "@height"));
            Ruby.Pin(MainViewport);
            Ruby.Funcall(MainViewport, "z=", Ruby.Integer.ToPtr(999999998));

            Ruby.SetIVar(Font.Class, "@default_name", Ruby.String.ToPtr("arial"));
            Ruby.SetIVar(Font.Class, "@default_size", Ruby.Integer.ToPtr(32));
            Ruby.SetIVar(Font.Class, "@default_color", Color.CreateColor(odl.Color.WHITE));
            Ruby.SetIVar(Font.Class, "@default_outline", Ruby.False);
            Ruby.SetIVar(Font.Class, "@default_outline_color", Color.CreateColor(odl.Color.BLACK));

            OverlayViewport = Ruby.Funcall(Viewport.Class, "new", Ruby.Integer.ToPtr(0), Ruby.Integer.ToPtr(0), Ruby.GetIVar(Module, "@width"), Ruby.GetIVar(Module, "@height"));
            Ruby.Pin(OverlayViewport);
            Ruby.Funcall(OverlayViewport, "z=", Ruby.Integer.ToPtr(999999999));

            OverlaySprite = Ruby.Funcall(Sprite.Class, "new", OverlayViewport);
            Ruby.Pin(OverlaySprite);

            IntPtr OverlayBitmap = Ruby.Funcall(Bitmap.Class, "new", Ruby.GetIVar(Module, "@width"), Ruby.GetIVar(Module, "@height"));
            Ruby.Pin(OverlayBitmap);

            Ruby.Funcall(OverlaySprite, "bitmap=", OverlayBitmap);
            Ruby.Funcall(OverlaySprite, "opacity=", Ruby.Integer.ToPtr(0));
            Ruby.Funcall(OverlaySprite, "z=", Ruby.Integer.ToPtr(999999999));
            Ruby.Funcall(OverlayBitmap, "fill_rect", Ruby.Integer.ToPtr(0), Ruby.Integer.ToPtr(0), Ruby.GetIVar(Module, "@width"), Ruby.GetIVar(Module, "@height"), Color.CreateColor(odl.Color.BLACK));

            Ruby.SetGlobal("$PERIDOT", Ruby.True);

            Ruby.SetIVar(Module, "@brightness", Ruby.Integer.ToPtr(255));
            Ruby.SetIVar(Module, "@frame_count", Ruby.Integer.ToPtr(0));

            int fps = Config.FrameRate;
            if (Config.VSync)
            {
                odl.SDL2.SDL.SDL_DisplayMode mode = new odl.SDL2.SDL.SDL_DisplayMode();
                odl.SDL2.SDL.SDL_GetWindowDisplayMode(Program.MainWindow.SDL_Window, out mode);
                fps = mode.refresh_rate;
            }
            else
            {
                ManualSync = true;
                MillisecondsPerFrame = (int) Math.Ceiling(1d / fps);
                FPSTimer.Start();
            }
            Ruby.SetIVar(Module, "@frame_rate", Ruby.Integer.ToPtr(fps));
            Ruby.SetIVar(Module, "@render_speed", Ruby.Float.ToPtr(1));
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

        static IntPtr render_speedget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@render_speed");
        }

        static IntPtr render_speedset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "Float");
            double speed = Ruby.Float.FromPtr(Ruby.Array.Get(Args, 0));
            if (speed == 1 && !Program.MainWindow.GetVSync())
            {
                Program.MainWindow.SetVSync(true);
                ManualSync = false;
                MillisecondsPerFrame = -1;
                FPSTimer.Stop();
            }
            else if (speed != 1)
            {
                if (Program.MainWindow.GetVSync())
                {
                    Program.MainWindow.SetVSync(false);
                    ManualSync = true;
                    FPSTimer.Start();
                }
                MillisecondsPerFrame = (int) Math.Ceiling(1000d / speed / Config.FrameRate);
            }
            return Ruby.SetIVar(Self, "@render_speed", Ruby.Array.Get(Args, 0));
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

        static IntPtr vsyncget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Program.MainWindow.GetVSync() ? Ruby.True : Ruby.False;
        }

        static IntPtr vsyncset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "TrueClass", "FalseClass");
            bool vsync = Ruby.Array.Get(Args, 0) == Ruby.True;
            Program.MainWindow.SetVSync(vsync);
            if (vsync)
            {
                ManualSync = false;
                MillisecondsPerFrame = -1;
            }
            else
            {
                ManualSync = true;
                double speed = Ruby.Float.FromPtr(Ruby.GetIVar(Self, "@render_speed"));
                MillisecondsPerFrame = (int) Math.Ceiling(1000d / speed / Config.FrameRate);
            }
            return Ruby.Array.Get(Args, 0);
        }

        static IntPtr show_overlay(IntPtr Self, IntPtr Args)
        {
            int frames = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Self, "@frame_rate")) / 4;
            if (Ruby.Array.Length(Args) != 0)
            {
                Ruby.Array.Expect(Args, 1);
                Ruby.Array.Expect(Args, 0, "Integer");
                frames = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
            }
            if (Ruby.Integer.FromPtr(Ruby.GetIVar(OverlaySprite, "@opacity")) == 255) return Ruby.False;
            if (frames == 0)
            {
                Ruby.Funcall(OverlaySprite, "opacity=", Ruby.Integer.ToPtr(255));
                return Ruby.True;
            }
            bool hasblock = Ruby.HasBlock();
            for (int i = 1; i <= frames; i++)
            {
                Ruby.Funcall(OverlaySprite, "opacity=", Ruby.Float.ToPtr(255d / frames * i));
                if (hasblock) Ruby.Yield(Ruby.Integer.ToPtr(i - 1));
                update(Self, Ruby.Array.Create());
            }
            return Ruby.True;
        }

        static IntPtr hide_overlay(IntPtr Self, IntPtr Args)
        {
            int frames = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Self, "@frame_rate")) / 4;
            if (Ruby.Array.Length(Args) != 0)
            {
                Ruby.Array.Expect(Args, 1);
                Ruby.Array.Expect(Args, 0, "Integer");
                frames = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
            }
            if (Ruby.Integer.FromPtr(Ruby.GetIVar(OverlaySprite, "@opacity")) == 0) return Ruby.False;
            if (frames == 0)
            {
                Ruby.Funcall(OverlaySprite, "opacity=", Ruby.Integer.ToPtr(0));
                return Ruby.True;
            }
            bool hasblock = Ruby.HasBlock();
            for (int i = 1; i <= frames; i++)
            {
                Ruby.Funcall(OverlaySprite, "opacity=", Ruby.Float.ToPtr(255 - 255d / frames * i));
                if (hasblock) Ruby.Yield(Ruby.Integer.ToPtr(i - 1));
                update(Self, Ruby.Array.Create());
            }
            return Ruby.True;
        }

        static IntPtr overlay_shown(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.Integer.FromPtr(Ruby.GetIVar(OverlaySprite, "@opacity")) == 255 ? Ruby.True : Ruby.False;
        }

        static IntPtr screenshot(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Bitmap.CreateBitmap(Program.MainWindow.Screenshot());
        }

        static IntPtr update(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            if (!odl.Graphics.Initialized) Ruby.Raise(Ruby.ErrorType.SystemExit, "system stopped");

            long start = FPSTimer.ElapsedMilliseconds;
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
            if (ManualSync)
            {
                long end = FPSTimer.ElapsedMilliseconds;
                long timediff = end - start;
                long delay = MillisecondsPerFrame - timediff;
                if (delay > 0) SDL.SDL_Delay((uint) delay);
            }
            return Ruby.True;
        }

        static IntPtr wait(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "Integer");
            odl.SDL2.SDL.SDL_Delay((uint) (1000d / Ruby.Integer.FromPtr(Ruby.GetIVar(Self, "@frame_rate")) * Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0))));
            return Ruby.True;
        }
        
        static IntPtr windows(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return odl.Graphics.Platform == odl.Platform.Windows ? Ruby.True : Ruby.False;
        }

        static IntPtr linux(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return odl.Graphics.Platform == odl.Platform.Linux ? Ruby.True : Ruby.False;
        }

        static IntPtr mac(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return odl.Graphics.Platform == odl.Platform.MacOS ? Ruby.True : Ruby.False;
        }
    }
}

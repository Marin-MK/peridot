using System;
using RubyDotNET;

namespace Peridot
{
    public class Graphics : RubyObject
    {
        public static IntPtr Module;
        public static IntPtr MainViewport;
        public static IntPtr OverlayViewport;
        public static IntPtr OverlaySprite;
        public static IntPtr OverlayBitmap;

        public static Module CreateModule()
        {
            Module m = new Module("Graphics");
            Module = m.Pointer;
            m.DefineClassMethod("frame_rate", frame_rateget);
            m.DefineClassMethod("frame_rate=", frame_rateset);
            m.DefineClassMethod("width", widthget);
            m.DefineClassMethod("width=", widthset);
            m.DefineClassMethod("height", heightget);
            m.DefineClassMethod("height=", heightset);
            m.DefineClassMethod("brightness", brightnessget);
            m.DefineClassMethod("brightness=", brightnessset);
            m.DefineClassMethod("frame_count", frame_countget);
            m.DefineClassMethod("frame_count=", frame_countset);
            m.DefineClassMethod("transition", transition);
            m.DefineClassMethod("screenshot", screenshot);
            m.DefineClassMethod("snap_to_bitmap", screenshot);
            m.DefineClassMethod("update", update);
            m.DefineClassMethod("wait", wait);
            return m;
        }

        public static void Start()
        {
            odl.Viewport.DefaultWindow = Program.MainWindow;
            MainViewport = Internal.rb_funcallv(Viewport.Class, Internal.rb_intern("new"), 4, new IntPtr[4]
            {
                Internal.LONG2NUM(0),
                Internal.LONG2NUM(0),
                Internal.GetIVar(Module, "@width"),
                Internal.GetIVar(Module, "@height")
            });
            Internal.rb_funcallv(MainViewport, Internal.rb_intern("z="), 1, new IntPtr[] { Internal.LONG2NUM(999999998) });
            Internal.SetIVar(Font.Class, "@default_name", Internal.rb_str_new_cstr("arial"));
            Internal.SetIVar(Font.Class, "@default_size", Internal.LONG2NUM(32));
            Internal.SetIVar(Font.Class, "@default_color", Color.CreateColor(odl.Color.WHITE));
            Internal.SetIVar(Font.Class, "@default_outline", Internal.QFalse);
            Internal.SetIVar(Font.Class, "@default_outline_color", Color.CreateColor(odl.Color.BLACK));
            OverlayViewport = Internal.rb_funcallv(Viewport.Class, Internal.rb_intern("new"), 4, new IntPtr[4]
            {
                Internal.LONG2NUM(0),
                Internal.LONG2NUM(0),
                Internal.GetIVar(Module, "@width"),
                Internal.GetIVar(Module, "@height")
            });
            Internal.SetGlobalVariable("$__overlay_viewport__", OverlayViewport);
            Internal.rb_funcallv(OverlayViewport, Internal.rb_intern("z="), 1, new IntPtr[1] { Internal.LONG2NUM(999999999) });
            OverlaySprite = Internal.rb_funcallv(Sprite.Class, Internal.rb_intern("new"), 1, new IntPtr[1] { OverlayViewport });
            Internal.SetGlobalVariable("$__overlay_sprite__", OverlaySprite);
            OverlayBitmap = Internal.rb_funcallv(Bitmap.Class, Internal.rb_intern("new"), 2, new IntPtr[2]
            {
                Internal.GetIVar(Module, "@width"),
                Internal.GetIVar(Module, "@height")
            });
            Internal.SetGlobalVariable("$__overlay_bitmap__", OverlayBitmap);
            Internal.rb_funcallv(OverlaySprite, Internal.rb_intern("bitmap="), 1, new IntPtr[1] { OverlayBitmap });
            Internal.rb_funcallv(OverlaySprite, Internal.rb_intern("opacity="), 1, new IntPtr[1] { Internal.LONG2NUM(0) });
            Internal.rb_funcallv(OverlaySprite, Internal.rb_intern("z="), 1, new IntPtr[1] { Internal.LONG2NUM(999999999) });
            Internal.rb_funcallv(OverlayBitmap, Internal.rb_intern("fill_rect"), 5, new IntPtr[5]
            {
                Internal.LONG2NUM(0),
                Internal.LONG2NUM(0),
                Internal.GetIVar(Module, "@width"),
                Internal.GetIVar(Module, "@height"),
                Color.CreateColor(odl.Color.BLACK)
            });
            Internal.SetGlobalVariable("$__mainvp__", MainViewport);
            Internal.SetGlobalVariable("$Peridot", Internal.QTrue);
            Internal.SetIVar(Module, "@brightness", Internal.LONG2NUM(255));
            Internal.SetIVar(Module, "@frame_count", Internal.LONG2NUM(0));
            int fps = Config.FrameRate;
            if (Config.VSync)
            {
                SDL2.SDL.SDL_DisplayMode mode = new SDL2.SDL.SDL_DisplayMode();
                SDL2.SDL.SDL_GetWindowDisplayMode(Program.MainWindow.SDL_Window, out mode);
                fps = mode.refresh_rate;
            }
            Internal.SetIVar(Module, "@frame_rate", Internal.LONG2NUM(fps));
        }

        protected static IntPtr frame_rateget(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@frame_rate");
        }

        protected static IntPtr frame_rateset(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.SetIVar(self, "@frame_rate", Args[0].Pointer);
        }

        protected static IntPtr widthget(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@width");
        }

        protected static IntPtr widthset(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.SetIVar(self, "@width", Args[0].Pointer);
        }

        protected static IntPtr heightget(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@height");
        }

        protected static IntPtr heightset(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.SetIVar(self, "@height", Args[0].Pointer);
        }

        protected static IntPtr brightnessget(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@brightness");
        }

        protected static IntPtr brightnessset(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            Internal.rb_funcallv(OverlaySprite, Internal.rb_intern("opacity="), 1, new IntPtr[1] { Internal.LONG2NUM(255 - Internal.NUM2LONG(Args[0].Pointer)) });
            return Internal.SetIVar(self, "@brightness", Args[0].Pointer);
        }

        protected static IntPtr frame_countget(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@frame_count");
        }

        protected static IntPtr frame_countset(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.SetIVar(self, "@frame_count", Args[0].Pointer);
        }

        protected static IntPtr transition(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            int Duration = 0;
            string Filename = null;
            if (Args.Length == 2)
            {
                Duration = (int) Internal.NUM2LONG(Args[0].Pointer);
                Filename = new RubyString(Args[1].Pointer).ToString();
            }
            else if (Args.Length == 1)
            {
                Duration = (int) Internal.NUM2LONG(Args[0].Pointer);
            }
            else ScanArgs(1, Args);

            odl.Viewport vp = new odl.Viewport(0, 0, (int) Internal.NUM2LONG(Internal.GetIVar(self, "@width")), (int) Internal.NUM2LONG(Internal.GetIVar(self, "@height")));
            vp.Z = 999999999;
            odl.Sprite sp = new odl.Sprite(vp);
            sp.Z = 999999999;
            if (string.IsNullOrEmpty(Filename))
            {
                sp.Bitmap = new odl.Bitmap(vp.Width, vp.Height);
                sp.Bitmap.Unlock();
                sp.Bitmap.FillRect(0, 0, vp.Width, vp.Height, odl.Color.BLACK);
                sp.Bitmap.Lock();
            }
            else sp.Bitmap = new odl.Bitmap(Filename);
            sp.Opacity = 0;
            for (int i = 1; i <= Duration; i++)
            {
                Internal.rb_funcallv(self, Internal.rb_intern("update"), 0);
                sp.Opacity = (byte) Math.Round((double) i / Duration * 255d);
            }
            sp.Dispose();
            vp.Dispose();
            return Internal.QTrue;
        }

        protected static IntPtr screenshot(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Bitmap.CreateBitmap(odl.Graphics.Windows[0].Screenshot());
        }

        protected static IntPtr update(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            if (!odl.Graphics.Initialized) Internal.rb_raise(Internal.rb_eSystemExit.Pointer, "game stopped");

            odl.Graphics.UpdateInput();
            odl.Graphics.UpdateWindows();
            try
            {
                odl.Graphics.UpdateGraphics(true);
            }
            catch (odl.BitmapLockedException)
            {
                Internal.rb_raise(Internal.rb_eRuntimeError.Pointer, "attempted to render a still unlocked bitmap");
            }
            return Internal.QTrue;
        }

        protected static IntPtr wait(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            SDL2.SDL.SDL_Delay((uint) (1000d / Internal.NUM2LONG(Internal.GetIVar(self, "@frame_rate")) * Internal.NUM2LONG(Args[0].Pointer)));
            return Internal.QNil;
        }
    }
}

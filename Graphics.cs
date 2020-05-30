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

        public static int FrameRate
        {
            get
            {
                return (int) Internal.NUM2LONG(Internal.rb_funcallv(Module, Internal.rb_intern("frame_rate"), 0));
            }
            set
            {
                Internal.rb_funcallv(Module, Internal.rb_intern("frame_rate="), 1, Internal.LONG2NUM(value));
            }
        }
        public static int TicksPerFrame
        {
            get
            {
                return (int) Math.Round(1000d / FrameRate);
            }
        }

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
            m.DefineClassMethod("update", update);
            m.DefineClassMethod("wait", wait);
            return m;
        }

        static FPSTimer FPSTimer = new FPSTimer();
        static FPSTimer FrameTimer = new FPSTimer();

        public static void Start()
        {
            ODL.Viewport.DefaultRenderer = ODL.Graphics.Windows[0].Renderer;
            MainViewport = Internal.rb_funcallv(Viewport.Class, Internal.rb_intern("new"), 4, new IntPtr[4]
            {
                Internal.LONG2NUM(0),
                Internal.LONG2NUM(0),
                Internal.GetIVar(Module, "@width"),
                Internal.GetIVar(Module, "@height")
            });
            Internal.SetIVar(Font.Class, "@default_name", Internal.rb_str_new_cstr("arial"));
            Internal.SetIVar(Font.Class, "@default_size", Internal.LONG2NUM(32));
            Internal.SetIVar(Font.Class, "@default_color", Color.CreateColor(ODL.Color.WHITE));
            Internal.SetIVar(Font.Class, "@default_outline", Internal.QFalse);
            Internal.SetIVar(Font.Class, "@default_outline_color", Color.CreateColor(ODL.Color.BLACK));
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
                Color.CreateColor(ODL.Color.BLACK)
            });
            Internal.SetGlobalVariable("$__mainvp__", MainViewport);
            Internal.SetGlobalVariable("$Peridot", Internal.QTrue);
            Internal.SetIVar(Module, "@brightness", Internal.LONG2NUM(255));
            Internal.SetIVar(Module, "@frame_count", Internal.LONG2NUM(0));
            FrameRate = Config.FrameRate;
            FPSTimer.Start();
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

        protected static IntPtr update(IntPtr _self, IntPtr _args)
        {
            if (!ODL.Graphics.Initialized) Internal.rb_raise(Internal.rb_eSystemExit.Pointer, "game stopped");

            FrameTimer.Start();

            ODL.Graphics.UpdateInput();
            ODL.Graphics.UpdateWindows();
            try
            {
                ODL.Graphics.UpdateGraphics(true);
            }
            catch (ODL.BitmapLockedException)
            {
                Internal.rb_raise(Internal.rb_eRuntimeError.Pointer, "attempted to render a still unlocked bitmap");
            }

            uint frameTicks = FrameTimer.Ticks;
            if (frameTicks < TicksPerFrame)
            {
                SDL2.SDL.SDL_Delay((uint) (TicksPerFrame - frameTicks));
            }
            return _self;
        }

        protected static IntPtr wait(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            SDL2.SDL.SDL_Delay((uint) (TicksPerFrame * Internal.NUM2LONG(Args[0].Pointer)));
            return Internal.QNil;
        }
    }

    public class FPSTimer
    {
        uint StartTicks = 0;
        uint PauseTicks = 0;
        public bool Running = false;

        public void Start()
        {
            this.Running = true;
            StartTicks = SDL2.SDL.SDL_GetTicks() - PauseTicks;
        }

        public void Stop()
        {
            this.Running = false;
            PauseTicks = SDL2.SDL.SDL_GetTicks() - StartTicks;
        }

        public uint Ticks
        {
            get
            {
                if (Running)
                {
                    return SDL2.SDL.SDL_GetTicks() - StartTicks;
                }
                else
                {
                    return PauseTicks;
                }
            }
        }
    }
}

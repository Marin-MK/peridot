using System;
using RubyDotNET;

namespace odlgss
{
    public class Graphics : RubyObject
    {
        public static IntPtr ModulePointer;
        public static Viewport MainViewport;

        public static int FrameRate
        {
            get
            {
                return (int) Internal.NUM2LONG(Internal.rb_funcallv(ModulePointer, Internal.rb_intern("frame_rate"), 0));
            }
            set
            {
                Internal.rb_funcallv(ModulePointer, Internal.rb_intern("frame_rate="), 1, Internal.LONG2NUM(value));
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
            ModulePointer = m.Pointer;
            m.DefineClassMethod("update", update);
            m.DefineClassMethod("frame_rate", frame_rateget);
            m.DefineClassMethod("frame_rate=", frame_rateset);
            m.DefineClassMethod("width", widthget);
            m.DefineClassMethod("width=", widthset);
            m.DefineClassMethod("height", heightget);
            m.DefineClassMethod("height=", heightset);
            return m;
        }

        static FPSTimer FPSTimer = new FPSTimer();
        static FPSTimer FrameTimer = new FPSTimer();

        public static void Start()
        {
            MainViewport = Viewport.New(0, 0, Width, Height);
            Font.DefaultName = "arial";
            Font.DefaultSize = 22;
            Font.DefaultBold = false;
            Font.DefaultItalic = false;
            Font.DefaultOutline = false;
            Font.DefaultColor = Color.New(255, 255, 255);
            Graphics.FrameRate = 60;
            FPSTimer.Start();
        }

        public static int CountedFrames = 0;

        public static void Update()
        {
            Internal.rb_funcallv(ModulePointer, Internal.rb_intern("update"), 0);
        }
        public static int Width
        {
            get
            {
                return (int) Internal.NUM2LONG(Internal.rb_funcallv(ModulePointer, Internal.rb_intern("width"), 0));
            }
            set
            {
                Internal.rb_funcallv(ModulePointer, Internal.rb_intern("width="), 1, Internal.LONG2NUM(value));
            }
        }
        public static int Height
        {
            get
            {
                return (int) Internal.NUM2LONG(Internal.rb_funcallv(ModulePointer, Internal.rb_intern("height"), 0));
            }
            set
            {
                Internal.rb_funcallv(ModulePointer, Internal.rb_intern("height="), 1, Internal.LONG2NUM(value));
            }
        }

        static IntPtr update(IntPtr _self, IntPtr _args)
        {
            if (!ODL.Graphics.Initialized) Internal.rb_raise(Internal.rb_eSystemExit.Pointer, "game stopped");

            FrameTimer.Start();

            SDL2.SDL.SDL_Event e;
            while (SDL2.SDL.SDL_PollEvent(out e) > 0)
            {
                ODL.Graphics.EvaluateEvent(e);
            }

            double avgFPS = CountedFrames / (FPSTimer.Ticks / 1000d);

            ODL.Graphics.UpdateGraphics(true);
            CountedFrames++;

            uint frameTicks = FrameTimer.Ticks;
            if (frameTicks < TicksPerFrame)
            {
                SDL2.SDL.SDL_Delay((uint) (TicksPerFrame - frameTicks));
            }
            return _self;
        }

        static IntPtr frame_rateget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@frame_rate"));
        }
        static IntPtr frame_rateset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@frame_rate"), Args[0].Pointer);
        }

        static IntPtr widthget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@width"));
        }
        static IntPtr widthset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@width"), Args[0].Pointer);
        }

        static IntPtr heightget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@height"));
        }
        static IntPtr heightset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@height"), Args[0].Pointer);
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

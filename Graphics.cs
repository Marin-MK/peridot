using System;
using rubydotnet;

namespace peridot
{
    public class Graphics : Ruby.Object
    {
        public new static string KlassName = "Graphics";
        public static Ruby.Module Module;

        public static Viewport MainViewport;
        public static Viewport OverlayViewport;
        public static Sprite OverlaySprite;
        public static Bitmap OverlayBitmap;

        public Graphics(IntPtr Pointer) : base(Pointer) { }

        public static void Create()
        {
            Ruby.Module m = Ruby.Module.DefineModule<Graphics>(KlassName);
            Module = m;
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
            m.DefineClassMethod("screenshot", screenshot);
            m.DefineClassMethod("snap_to_bitmap", screenshot);
            m.DefineClassMethod("update", update);
            m.DefineClassMethod("wait", wait);
        }

        public static void Start()
        {
            odl.Viewport.DefaultWindow = Program.MainWindow;
            MainViewport = Viewport.Class.AutoFuncall<Viewport>("new",
                (Ruby.Integer) 0,
                (Ruby.Integer) 0,
                Module.GetIVar("@width"),
                Module.GetIVar("@height")
            );
            MainViewport.Funcall("z=", (Ruby.Integer) 999999998);

            Font.Class.SetIVar("@default_name", (Ruby.String) "arial");
            Font.Class.SetIVar("@default_size", (Ruby.Integer) 32);
            Font.Class.SetIVar("@default_color", Color.CreateColor(odl.Color.WHITE));
            Font.Class.SetIVar("@default_outline", Ruby.False);
            Font.Class.SetIVar("@default_outline_color", Color.CreateColor(odl.Color.BLACK));

            OverlayViewport = Viewport.Class.AutoFuncall<Viewport>("new",
                (Ruby.Integer) 0,
                (Ruby.Integer) 0,
                Module.GetIVar("@width"),
                Module.GetIVar("@height")
            );
            OverlayViewport.Funcall("z=", (Ruby.Integer) 999999999);

            //Internal.SetGlobalVariable("$__overlay_viewport__", OverlayViewport);

            OverlaySprite = Sprite.Class.AutoFuncall<Sprite>("new", OverlayViewport);

            //Internal.SetGlobalVariable("$__overlay_sprite__", OverlaySprite);

            OverlayBitmap = Bitmap.Class.AutoFuncall<Bitmap>("new",
                Module.GetIVar("@width"),
                Module.GetIVar("@height")
            );
            //Internal.SetGlobalVariable("$__overlay_bitmap__", OverlayBitmap);

            OverlaySprite.Funcall("bitmap=", OverlayBitmap);
            OverlaySprite.Funcall("opacity=", (Ruby.Integer) 0);
            OverlaySprite.Funcall("z=", (Ruby.Integer) 999999999);
            OverlayBitmap.Funcall("fill_rect",
                (Ruby.Integer) 0,
                (Ruby.Integer) 0,
                Module.GetIVar("@width"),
                Module.GetIVar("@height"),
                Color.CreateColor(odl.Color.BLACK)
            );

            //Internal.SetGlobalVariable("$__mainvp__", MainViewport);
            //Internal.SetGlobalVariable("$peridot", Internal.QTrue);

            Module.SetIVar("@brightness", (Ruby.Integer) 255);
            Module.SetIVar("@frame_count", (Ruby.Integer) 0);

            int fps = Config.FrameRate;
            if (Config.VSync)
            {
                SDL2.SDL.SDL_DisplayMode mode = new SDL2.SDL.SDL_DisplayMode();
                SDL2.SDL.SDL_GetWindowDisplayMode(Program.MainWindow.SDL_Window, out mode);
                fps = mode.refresh_rate;
            }
            Module.SetIVar("@frame_rate", (Ruby.Integer) fps);
        }

        protected static Ruby.Object frame_rateget(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(0);
            return Self.GetIVar("@frame_rate");
        }

        protected static Ruby.Object frame_rateset(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(1);
            Args[0].Expect(Ruby.Integer.Class);
            return Self.SetIVar("@frame_rate", Args[0]);
        }

        protected static Ruby.Object widthget(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(0);
            return Self.GetIVar("@width");
        }

        protected static Ruby.Object widthset(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(1);
            Args[0].Expect(Ruby.Integer.Class);
            return Self.SetIVar("@width", Args[0]);
        }

        protected static Ruby.Object heightget(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(0);
            return Self.GetIVar("@height");
        }

        protected static Ruby.Object heightset(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(1);
            Args[0].Expect(Ruby.Integer.Class);
            return Self.SetIVar("@height", Args[0]);
        }

        protected static Ruby.Object brightnessget(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(0);
            return Self.GetIVar("@brightness");
        }

        protected static Ruby.Object brightnessset(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(1);
            Ruby.Object brightness = null;
            if (Args[0].Is(Ruby.Float.Class))
            {
                if (Args.Get<Ruby.Float>(0) < 0) brightness = (Ruby.Float) 0;
                else if (Args.Get<Ruby.Float>(0) > 255) brightness = (Ruby.Float) 255;
                else brightness = Args.Get<Ruby.Float>(0);
                OverlaySprite.Funcall("opacity=", (Ruby.Integer) Math.Round((Ruby.Integer) 255 - (Ruby.Float) brightness));
            }
            else
            {
                Args[0].Expect(Ruby.Integer.Class);
                if (Args.Get<Ruby.Integer>(0) < 0) brightness = (Ruby.Integer) 0;
                else if (Args.Get<Ruby.Integer>(0) > 255) brightness = (Ruby.Integer) 255;
                else brightness = Args.Get<Ruby.Integer>(0);
                OverlaySprite.Funcall("opacity=", (Ruby.Integer) 255 - (Ruby.Integer) brightness);
            }
            return Self.SetIVar("@brightness", brightness);
        }

        protected static Ruby.Object frame_countget(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(0);
            return Self.GetIVar("@frame_count");
        }

        protected static Ruby.Object frame_countset(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(1);
            Args[0].Expect(Ruby.Integer.Class);
            return Self.SetIVar("@frame_count", Args[0]);
        }

        protected static Ruby.Object screenshot(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(0);
            return Bitmap.CreateBitmap(odl.Graphics.Windows[0].Screenshot());
        }

        protected static Ruby.Object update(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(0);
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

        protected static Ruby.Object wait(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(1);
            Args[0].Expect(Ruby.Integer.Class);
            SDL2.SDL.SDL_Delay((uint) (1000d / Self.AutoGetIVar<Ruby.Integer>("@frame_rate") * Args.Get<Ruby.Integer>(0)));
            return Ruby.True;
        }
    }
}

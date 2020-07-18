using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using rubydotnet;

namespace peridot
{
    public class Sprite : Ruby.Object
    {
        public new static string KlassName = "Sprite";
        public new static Ruby.Class Class;
        public static Dictionary<IntPtr, odl.Sprite> SpriteDictionary = new Dictionary<IntPtr, odl.Sprite>();

        public Sprite(IntPtr Pointer) : base(Pointer) {  }

        public static void Create()
        {
            Ruby.Class c = Ruby.Class.DefineClass<Sprite>("Sprite");
            Class = c;
            c.DefineMethod("initialize", initialize);
            c.DefineMethod("viewport", viewportget);
            c.DefineMethod("viewport=", viewportset);
            c.DefineMethod("bitmap", bitmapget);
            c.DefineMethod("bitmap=", bitmapset);
            c.DefineMethod("x", xget);
            c.DefineMethod("x=", xset);
            c.DefineMethod("y", yget);
            c.DefineMethod("y=", yset);
            c.DefineMethod("z", zget);
            c.DefineMethod("z=", zset);
            c.DefineMethod("ox", oxget);
            c.DefineMethod("ox=", oxset);
            c.DefineMethod("oy", oyget);
            c.DefineMethod("oy=", oyset);
            c.DefineMethod("zoom_x", zoom_xget);
            c.DefineMethod("zoom_x=", zoom_xset);
            c.DefineMethod("zoom_y", zoom_yget);
            c.DefineMethod("zoom_y=", zoom_yset);
            c.DefineMethod("opacity", opacityget);
            c.DefineMethod("opacity=", opacityset);
            c.DefineMethod("angle", angleget);
            c.DefineMethod("angle=", angleset);
            c.DefineMethod("src_rect", src_rectget);
            c.DefineMethod("src_rect=", src_rectset);
            c.DefineMethod("visible", visibleget);
            c.DefineMethod("visible=", visibleset);
            c.DefineMethod("mirror", mirror_xget);
            c.DefineMethod("mirror=", mirror_xset);
            c.DefineMethod("mirror_x", mirror_xget);
            c.DefineMethod("mirror_x=", mirror_xset);
            c.DefineMethod("mirror_y", mirror_yget);
            c.DefineMethod("mirror_y=", mirror_yset);
            c.DefineMethod("color", colorget);
            c.DefineMethod("color=", colorset);
            c.DefineMethod("tone", toneget);
            c.DefineMethod("tone=", toneset);
            c.DefineMethod("update", update);
            c.DefineMethod("dispose", dispose);
            c.DefineMethod("disposed?", disposed);
        }

        protected static Ruby.Object initialize(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(0, 1);
            Viewport viewport = null;
            if (Args.Length == 0 || Args.Length == 1 && Args[0] == Ruby.Nil)
            {
                viewport = Graphics.MainViewport;
                Self.SetIVar("@viewport", Ruby.Nil);
            }
            else if (Args.Length == 1)
            {
                Args[0].Expect(Viewport.Class);
                viewport = Args.Get<Viewport>(0);
                Self.SetIVar("@viewport", viewport);
            }
            Self.SetIVar("@x", (Ruby.Integer) 0);
            Self.SetIVar("@y", (Ruby.Integer) 0);
            Self.SetIVar("@z", (Ruby.Integer) 0);
            Self.SetIVar("@ox", (Ruby.Integer) 0);
            Self.SetIVar("@oy", (Ruby.Integer) 0);
            Self.SetIVar("@opacity", (Ruby.Integer) 255);
            Self.SetIVar("@angle", (Ruby.Integer) 0);
            Self.SetIVar("@zoom_x", (Ruby.Float) 1);
            Self.SetIVar("@zoom_y", (Ruby.Float) 1);
            Rect src_rect = Rect.CreateRect(new odl.Rect(0, 0, 0, 0));
            src_rect.SetIVar("@__sprite__", Self);
            Self.SetIVar("@src_rect", src_rect);
            Self.SetIVar("@visible", Ruby.True);
            Self.SetIVar("@mirror_x", Ruby.False);
            Self.SetIVar("@mirror_y", Ruby.False);
            Color color = Color.CreateColor(odl.Color.ALPHA);
            color.SetIVar("@__sprite__", Self);
            Self.SetIVar("@color", color);
            Tone tone = Tone.CreateTone(new odl.Tone());
            tone.SetIVar("@__sprite__", Self);
            Self.SetIVar("@tone", tone);
            Self.SetIVar("@disposed", Ruby.False);

            if (!Viewport.ViewportDictionary.ContainsKey(viewport.Pointer)) Ruby.Raise(Ruby.ErrorType.RuntimeError, "invalid viewport");
            odl.Sprite sprite = new odl.Sprite(Viewport.ViewportDictionary[viewport.Pointer]);
            if (SpriteDictionary.ContainsKey(Self.Pointer))
            {
                SpriteDictionary[Self.Pointer].Dispose();
                SpriteDictionary.Remove(Self.Pointer);
            }
            SpriteDictionary.Add(Self.Pointer, sprite);
            return Self;
        }

        protected static Ruby.Object viewportget(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            return Self.GetIVar("@viewport");
        }

        protected static Ruby.Object viewportset(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(1);
            Ruby.Object obj = Ruby.Nil;
            if (Args[0].IsNil())
            {
                SpriteDictionary[Self.Pointer].Viewport = Viewport.ViewportDictionary[Graphics.MainViewport.Pointer];
            }
            else
            {
                Args[0].Expect(Viewport.Class);
                SpriteDictionary[Self.Pointer].Viewport = Viewport.ViewportDictionary[Args[0].Pointer];
                obj = Args[0];
            }
            return Self.SetIVar("@viewport", obj);
        }

        protected static Ruby.Object bitmapget(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            return Self.GetIVar("@bitmap");
        }

        protected static Ruby.Object bitmapset(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(1);
            if (Args[0].IsNil())
            {
                SpriteDictionary[Self.Pointer].Bitmap = null;
            }
            else
            {
                Args[0].Expect(Bitmap.Class);
                SpriteDictionary[Self.Pointer].Bitmap = Bitmap.BitmapDictionary[Args[0].Pointer];
            }
            int x = 0,
                y = 0,
                w = SpriteDictionary[Self.Pointer].Bitmap == null ? 0 : SpriteDictionary[Self.Pointer].Bitmap.Width,
                h = SpriteDictionary[Self.Pointer].Bitmap == null ? 0 : SpriteDictionary[Self.Pointer].Bitmap.Height;
            SpriteDictionary[Self.Pointer].SrcRect.X = 0;
            SpriteDictionary[Self.Pointer].SrcRect.Y = 0;
            SpriteDictionary[Self.Pointer].SrcRect.Width = w;
            SpriteDictionary[Self.Pointer].SrcRect.Height = h;
            Self.GetIVar("@src_rect").SetIVar("@x", (Ruby.Integer) x);
            Self.GetIVar("@src_rect").SetIVar("@y", (Ruby.Integer) y);
            Self.GetIVar("@src_rect").SetIVar("@width", (Ruby.Integer) w);
            Self.GetIVar("@src_rect").SetIVar("@height", (Ruby.Integer) h);
            return Self.SetIVar("@bitmap", Args[0]);
        }

        protected static Ruby.Object xget(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            return Self.GetIVar("@x");
        }

        protected static Ruby.Object xset(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(1);
            if (Args[0].Is(Ruby.Float.Class))
            {
                SpriteDictionary[Self.Pointer].X = (int) Math.Round(Args.Get<Ruby.Float>(0));
            }
            else
            {
                Args[0].Expect(Ruby.Integer.Class);
                SpriteDictionary[Self.Pointer].X = Args.Get<Ruby.Integer>(0);
            }
            return Self.SetIVar("@x", Args[0]);
        }

        protected static Ruby.Object yget(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            return Self.GetIVar("@y");
        }

        protected static Ruby.Object yset(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(1);
            if (Args[0].Is(Ruby.Float.Class))
            {
                SpriteDictionary[Self.Pointer].Y = (int) Math.Round(Args.Get<Ruby.Float>(0));
            }
            else
            {
                Args[0].Expect(Ruby.Integer.Class);
                SpriteDictionary[Self.Pointer].Y = Args.Get<Ruby.Integer>(0);
            }
            return Self.SetIVar("@y", Args[0]);
        }

        protected static Ruby.Object zget(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            return Self.GetIVar("@z");
        }

        protected static Ruby.Object zset(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(1);
            if (Args[0].Is(Ruby.Float.Class))
            {
                SpriteDictionary[Self.Pointer].Z = (int) Math.Round(Args.Get<Ruby.Float>(0));
            }
            else
            {
                Args[0].Expect(Ruby.Integer.Class);
                SpriteDictionary[Self.Pointer].Z = Args.Get<Ruby.Integer>(0);
            }
            return Self.SetIVar("@z", Args[0]);
        }

        protected static Ruby.Object oxget(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            return Self.GetIVar("@ox");
        }

        protected static Ruby.Object oxset(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(1);
            if (Args[0].Is(Ruby.Float.Class))
            {
                SpriteDictionary[Self.Pointer].OX = (int) Math.Round(Args.Get<Ruby.Float>(0));
            }
            else
            {
                Args[0].Expect(Ruby.Integer.Class);
                SpriteDictionary[Self.Pointer].OX = Args.Get<Ruby.Integer>(0);
            }
            return Self.SetIVar("@ox", Args[0]);
        }

        protected static Ruby.Object oyget(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            return Self.GetIVar("@oy");
        }

        protected static Ruby.Object oyset(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(1);
            if (Args[0].Is(Ruby.Float.Class))
            {
                SpriteDictionary[Self.Pointer].OY = (int) Math.Round(Args.Get<Ruby.Float>(0));
            }
            else
            {
                Args[0].Expect(Ruby.Integer.Class);
                SpriteDictionary[Self.Pointer].OY = Args.Get<Ruby.Integer>(0);
            }
            return Self.SetIVar("@oy", Args[0]);
        }

        protected static Ruby.Object zoom_xget(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            return Self.GetIVar("@zoom_x");
        }

        protected static Ruby.Object zoom_xset(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(1);
            Ruby.Object zoomx = null;
            if (Args[0].Is(Ruby.Integer.Class))
            {
                zoomx = (Ruby.Float) ((int) Args.Get<Ruby.Integer>(0));
            }
            else
            {
                Args[0].Expect(Ruby.Float.Class);
                zoomx = Args.Get<Ruby.Float>(0);
            }
            SpriteDictionary[Self.Pointer].ZoomX = (Ruby.Float) zoomx;
            return Self.SetIVar("@zoom_x", zoomx);
        }

        protected static Ruby.Object zoom_yget(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            return Self.GetIVar("@zoom_y");
        }

        protected static Ruby.Object zoom_yset(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(1);
            Ruby.Object zoomy = null;
            if (Args[0].Is(Ruby.Integer.Class))
            {
                zoomy = (Ruby.Float) ((int) Args.Get<Ruby.Integer>(0));
            }
            else
            {
                Args[0].Expect(Ruby.Float.Class);
                zoomy = Args.Get<Ruby.Float>(0);
            }
            SpriteDictionary[Self.Pointer].ZoomY = (Ruby.Float) zoomy;
            return Self.SetIVar("@zoom_y", zoomy);
        }

        protected static Ruby.Object opacityget(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            return Self.GetIVar("@opacity");
        }

        protected static Ruby.Object opacityset(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(1);
            Ruby.Object opacity = null;
            byte realopacity = 0;
            if (Args[0].Is(Ruby.Float.Class))
            {
                if (Args.Get<Ruby.Float>(0) < 0) opacity = (Ruby.Float) 0;
                else if (Args.Get<Ruby.Float>(0) > 255) opacity = (Ruby.Float) 255;
                else opacity = Args.Get<Ruby.Float>(0);
                realopacity = (byte) Math.Round((Ruby.Float) opacity);
            }
            else
            {
                Args[0].Expect(Ruby.Integer.Class);
                if (Args.Get<Ruby.Integer>(0) < 0) opacity = (Ruby.Integer) 0;
                else if (Args.Get<Ruby.Integer>(0) > 255) opacity = (Ruby.Integer) 255;
                else opacity = Args.Get<Ruby.Integer>(0);
                realopacity = (byte) (Ruby.Integer) opacity;
            }
            SpriteDictionary[Self.Pointer].Opacity = realopacity;
            return Self.SetIVar("@opacity", opacity);
        }

        protected static Ruby.Object angleget(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            return Self.GetIVar("@angle");
        }

        protected static Ruby.Object angleset(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(1);
            Ruby.Object angle = null;
            short realangle = 0;
            if (Args[0].Is(Ruby.Float.Class))
            {
                angle = Args.Get<Ruby.Float>(0);
                realangle = (short) ((Ruby.Float) angle % (Ruby.Integer) 360);
            }
            else
            {
                Args[0].Expect(Ruby.Integer.Class);
                angle = Args.Get<Ruby.Integer>(0);
                realangle = (short) ((Ruby.Integer) angle % 360);
            }
            SpriteDictionary[Self.Pointer].Angle = realangle;
            return Self.SetIVar("@angle", angle);
        }

        protected static Ruby.Object src_rectget(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            return Self.GetIVar("@src_rect");
        }

        protected static Ruby.Object src_rectset(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(1);
            Args[0].Expect(Rect.Class);
            Self.GetIVar("@src_rect").SetIVar("@__sprite__", Ruby.Nil);
            SpriteDictionary[Self.Pointer].SrcRect = Rect.CreateRect(Args[0]);
            Args[0].SetIVar("@__sprite__", Self);
            return Self.SetIVar("@src_rect", Args[0]);
        }

        protected static Ruby.Object visibleget(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            return Self.GetIVar("@visible");
        }

        protected static Ruby.Object visibleset(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(1);
            Args[0].Expect(Ruby.TrueClass.Class, Ruby.FalseClass.Class, Ruby.NilClass.Class);
            return Self.SetIVar("@visible", Args[0] == Ruby.True ? (Ruby.Object) Ruby.True : Ruby.False);
        }

        protected static Ruby.Object mirror_xget(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            return Self.GetIVar("@mirror_x");
        }

        protected static Ruby.Object mirror_xset(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(1);
            Args[0].Expect(Ruby.TrueClass.Class, Ruby.FalseClass.Class, Ruby.NilClass.Class);
            return Self.SetIVar("@mirror_x", Args[0] == Ruby.True ? (Ruby.Object) Ruby.True : Ruby.False);
        }

        protected static Ruby.Object mirror_yget(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            return Self.GetIVar("@mirror_y");
        }

        protected static Ruby.Object mirror_yset(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(1);
            Args[0].Expect(Ruby.TrueClass.Class, Ruby.FalseClass.Class, Ruby.NilClass.Class);
            return Self.SetIVar("@mirror_y", Args[0] == Ruby.True ? (Ruby.Object) Ruby.True : Ruby.False);
        }

        protected static Ruby.Object colorget(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            return Self.GetIVar("@color");
        }

        protected static Ruby.Object colorset(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(1);
            Args[0].Expect(Color.Class);
            Self.GetIVar("@color").SetIVar("@__sprite__", Ruby.Nil);
            SpriteDictionary[Self.Pointer].Color = Color.CreateColor(Args[0]);
            Args[0].SetIVar("@__sprite__", Self);
            return Self.SetIVar("@color", Args[0]);
        }

        protected static Ruby.Object toneget(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            return Self.GetIVar("@tone");
        }

        protected static Ruby.Object toneset(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(1);
            Args[0].Expect(Tone.Class);
            Self.GetIVar("@tone").SetIVar("@__sprite__", Ruby.Nil);
            SpriteDictionary[Self.Pointer].Tone = Tone.CreateTone(Args[0]);
            Args[0].SetIVar("@__sprite__", Self);
            return Self.SetIVar("@tone", Args[0]);
        }

        protected static Ruby.Object update(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            return Ruby.Nil;
        }

        protected static Ruby.Object dispose(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            SpriteDictionary[Self.Pointer].Dispose();
            Self.GetIVar("@src_rect").SetIVar("@__sprite__", Ruby.Nil);
            Self.GetIVar("@color").SetIVar("@__sprite__", Ruby.Nil);
            Self.GetIVar("@tone").SetIVar("@__sprite__", Ruby.Nil);
            foreach (KeyValuePair<IntPtr, odl.Bitmap> kvp in Bitmap.BitmapDictionary)
            {
                if (kvp.Value == SpriteDictionary[Self.Pointer].Bitmap)
                {
                    Bitmap.BitmapDictionary.Remove(kvp.Key);
                    break;
                }
            }
            SpriteDictionary.Remove(Self.Pointer);
            Self.SetIVar("@disposed", Ruby.True);
            return Ruby.True;
        }

        protected static Ruby.Object disposed(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(0);
            return Self.GetIVar("@disposed") == Ruby.True ? (Ruby.Object) Ruby.True : Ruby.False;
        }

        public static void GuardDisposed(Ruby.Object Self)
        {
            if (Self.GetIVar("@disposed") == Ruby.True)
            {
                Ruby.Raise(Ruby.ErrorType.RuntimeError, "sprite already disposed");
            }
        }
    }
}

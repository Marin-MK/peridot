using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using rubydotnet;

namespace peridot
{
    public static class Sprite
    {
        public static IntPtr Class;
        public static Dictionary<IntPtr, odl.Sprite> SpriteDictionary = new Dictionary<IntPtr, odl.Sprite>();

        public static void Create()
        {
            Class = Ruby.Class.Define("Sprite");
            Ruby.Class.DefineMethod(Class, "initialize", initialize);
            Ruby.Class.DefineMethod(Class, "viewport", viewportget);
            Ruby.Class.DefineMethod(Class, "viewport=", viewportset);
            Ruby.Class.DefineMethod(Class, "bitmap", bitmapget);
            Ruby.Class.DefineMethod(Class, "bitmap=", bitmapset);
            Ruby.Class.DefineMethod(Class, "x", xget);
            Ruby.Class.DefineMethod(Class, "x=", xset);
            Ruby.Class.DefineMethod(Class, "y", yget);
            Ruby.Class.DefineMethod(Class, "y=", yset);
            Ruby.Class.DefineMethod(Class, "z", zget);
            Ruby.Class.DefineMethod(Class, "z=", zset);
            Ruby.Class.DefineMethod(Class, "ox", oxget);
            Ruby.Class.DefineMethod(Class, "ox=", oxset);
            Ruby.Class.DefineMethod(Class, "oy", oyget);
            Ruby.Class.DefineMethod(Class, "oy=", oyset);
            Ruby.Class.DefineMethod(Class, "zoom_x", zoom_xget);
            Ruby.Class.DefineMethod(Class, "zoom_x=", zoom_xset);
            Ruby.Class.DefineMethod(Class, "zoom_y", zoom_yget);
            Ruby.Class.DefineMethod(Class, "zoom_y=", zoom_yset);
            Ruby.Class.DefineMethod(Class, "opacity", opacityget);
            Ruby.Class.DefineMethod(Class, "opacity=", opacityset);
            Ruby.Class.DefineMethod(Class, "angle", angleget);
            Ruby.Class.DefineMethod(Class, "angle=", angleset);
            Ruby.Class.DefineMethod(Class, "src_rect", src_rectget);
            Ruby.Class.DefineMethod(Class, "src_rect=", src_rectset);
            Ruby.Class.DefineMethod(Class, "visible", visibleget);
            Ruby.Class.DefineMethod(Class, "visible=", visibleset);
            Ruby.Class.DefineMethod(Class, "mirror", mirror_xget);
            Ruby.Class.DefineMethod(Class, "mirror=", mirror_xset);
            Ruby.Class.DefineMethod(Class, "mirror_x", mirror_xget);
            Ruby.Class.DefineMethod(Class, "mirror_x=", mirror_xset);
            Ruby.Class.DefineMethod(Class, "mirror_y", mirror_yget);
            Ruby.Class.DefineMethod(Class, "mirror_y=", mirror_yset);
            Ruby.Class.DefineMethod(Class, "color", colorget);
            Ruby.Class.DefineMethod(Class, "color=", colorset);
            Ruby.Class.DefineMethod(Class, "tone", toneget);
            Ruby.Class.DefineMethod(Class, "tone=", toneset);
            Ruby.Class.DefineMethod(Class, "update", update);
            Ruby.Class.DefineMethod(Class, "dispose", dispose);
            Ruby.Class.DefineMethod(Class, "disposed?", disposed);
        }

        static IntPtr initialize(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0, 1);
            IntPtr vp = IntPtr.Zero;
            long len = Ruby.Array.Length(Args);
            if (len == 0 || len == 1 && Ruby.Array.Get(Args, 0) == Ruby.Nil)
            {
                vp = System.MainViewport;
                Ruby.SetIVar(Self, "@viewport", Ruby.Nil);
            }
            else if (len == 1)
            {
                Ruby.Array.Expect(Args, 0, "Viewport");
                vp = Ruby.Array.Get(Args, 0);
                Ruby.SetIVar(Self, "@viewport", vp);
            }
            Ruby.SetIVar(Self, "@x", Ruby.Integer.ToPtr(0));
            Ruby.SetIVar(Self, "@y", Ruby.Integer.ToPtr(0));
            Ruby.SetIVar(Self, "@z", Ruby.Integer.ToPtr(0));
            Ruby.SetIVar(Self, "@ox", Ruby.Integer.ToPtr(0));
            Ruby.SetIVar(Self, "@oy", Ruby.Integer.ToPtr(0));
            Ruby.SetIVar(Self, "@opacity", Ruby.Integer.ToPtr(255));
            Ruby.SetIVar(Self, "@angle", Ruby.Integer.ToPtr(0));
            Ruby.SetIVar(Self, "@zoom_x", Ruby.Float.ToPtr(1));
            Ruby.SetIVar(Self, "@zoom_y", Ruby.Float.ToPtr(1));
            IntPtr src_rect = Rect.CreateRect(new odl.Rect(0, 0, 0, 0));
            Ruby.SetIVar(src_rect, "@__sprite__", Self);
            Ruby.SetIVar(Self, "@src_rect", src_rect);
            Ruby.SetIVar(Self, "@visible", Ruby.True);
            Ruby.SetIVar(Self, "@mirror_x", Ruby.False);
            Ruby.SetIVar(Self, "@mirror_y", Ruby.False);
            IntPtr color = Color.CreateColor(odl.Color.ALPHA);
            Ruby.SetIVar(color, "@__sprite__", Self);
            Ruby.SetIVar(Self, "@color", color);
            IntPtr tone = Tone.CreateTone(new odl.Tone());
            Ruby.SetIVar(tone, "@__sprite__", Self);
            Ruby.SetIVar(Self, "@tone", tone);
            Ruby.SetIVar(Self, "@disposed", Ruby.False);

            if (!Viewport.ViewportDictionary.ContainsKey(vp)) Ruby.Raise(Ruby.ErrorType.RuntimeError, "invalid viewport");
            odl.Sprite sprite = new odl.Sprite(Viewport.ViewportDictionary[vp]);
            if (SpriteDictionary.ContainsKey(Self))
            {
                SpriteDictionary[Self].Dispose();
                SpriteDictionary.Remove(Self);
            }
            SpriteDictionary.Add(Self, sprite);
            return Self;
        }

        static IntPtr viewportget(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@viewport");
        }

        static IntPtr viewportset(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 1);
            IntPtr obj = Ruby.Nil;
            if (Ruby.Array.Get(Args, 0) == Ruby.Nil)
            {
                SpriteDictionary[Self].Viewport = Viewport.ViewportDictionary[System.MainViewport];
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Viewport");
                SpriteDictionary[Self].Viewport = Viewport.ViewportDictionary[Ruby.Array.Get(Args, 0)];
                obj = Ruby.Array.Get(Args, 0);
            }
            return Ruby.SetIVar(Self, "@viewport", obj);
        }

        static IntPtr bitmapget(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@bitmap");
        }

        static IntPtr bitmapset(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 1);
            if (Ruby.Array.Get(Args, 0) == Ruby.Nil)
            {
                SpriteDictionary[Self].Bitmap = null;
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Bitmap");
                SpriteDictionary[Self].Bitmap = Bitmap.BitmapDictionary[Ruby.Array.Get(Args, 0)];
            }
            int x = 0,
                y = 0,
                w = SpriteDictionary[Self].Bitmap == null ? 0 : SpriteDictionary[Self].Bitmap.Width,
                h = SpriteDictionary[Self].Bitmap == null ? 0 : SpriteDictionary[Self].Bitmap.Height;
            SpriteDictionary[Self].SrcRect.X = 0;
            SpriteDictionary[Self].SrcRect.Y = 0;
            SpriteDictionary[Self].SrcRect.Width = w;
            SpriteDictionary[Self].SrcRect.Height = h;
            IntPtr src_rect = Ruby.GetIVar(Self, "@src_rect");
            Ruby.SetIVar(src_rect, "@x", Ruby.Integer.ToPtr(x));
            Ruby.SetIVar(src_rect, "@y", Ruby.Integer.ToPtr(y));
            Ruby.SetIVar(src_rect, "@width", Ruby.Integer.ToPtr(w));
            Ruby.SetIVar(src_rect, "@height", Ruby.Integer.ToPtr(h));
            return Ruby.SetIVar(Self, "@bitmap", Ruby.Array.Get(Args, 0));
        }

        static IntPtr xget(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@x");
        }

        static IntPtr xset(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 1);
            if (Ruby.Array.Is(Args, 0, "Float"))
            {
                SpriteDictionary[Self].X = Ruby.Float.RoundFromPtr(Ruby.Array.Get(Args, 0));
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                SpriteDictionary[Self].X = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
            }
            return Ruby.SetIVar(Self, "@x", Ruby.Array.Get(Args, 0));
        }

        static IntPtr yget(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@y");
        }

        static IntPtr yset(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 1);
            if (Ruby.Array.Is(Args, 0, "Float"))
            {
                SpriteDictionary[Self].Y = Ruby.Float.RoundFromPtr(Ruby.Array.Get(Args, 0));
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                SpriteDictionary[Self].Y = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
            }
            return Ruby.SetIVar(Self, "@y", Ruby.Array.Get(Args, 0));
        }

        static IntPtr zget(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@z");
        }

        static IntPtr zset(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 1);
            if (Ruby.Array.Is(Args, 0, "Float"))
            {
                SpriteDictionary[Self].Z = Ruby.Float.RoundFromPtr(Ruby.Array.Get(Args, 0));
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                SpriteDictionary[Self].Z = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
            }
            return Ruby.SetIVar(Self, "@z", Ruby.Array.Get(Args, 0));
        }

        static IntPtr oxget(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@ox");
        }

        static IntPtr oxset(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 1);
            if (Ruby.Array.Is(Args, 0, "Float"))
            {
                SpriteDictionary[Self].OX = Ruby.Float.RoundFromPtr(Ruby.Array.Get(Args, 0));
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                SpriteDictionary[Self].OX = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
            }
            return Ruby.SetIVar(Self, "@ox", Ruby.Array.Get(Args, 0));
        }

        static IntPtr oyget(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@oy");
        }

        static IntPtr oyset(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 1);
            if (Ruby.Array.Is(Args, 0, "Float"))
            {
                SpriteDictionary[Self].OY = Ruby.Float.RoundFromPtr(Ruby.Array.Get(Args, 0));
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                SpriteDictionary[Self].OY = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
            }
            return Ruby.SetIVar(Self, "@oy", Ruby.Array.Get(Args, 0));
        }

        static IntPtr zoom_xget(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@zoom_x");
        }

        static IntPtr zoom_xset(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 1);
            IntPtr zoomx = IntPtr.Zero;
            if (Ruby.Array.Is(Args, 0, "Integer"))
            {
                zoomx = Ruby.Float.ToPtr(Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0)));
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Float");
                zoomx = Ruby.Array.Get(Args, 0);
            }
            SpriteDictionary[Self].ZoomX = Ruby.Float.FromPtr(zoomx);
            return Ruby.SetIVar(Self, "@zoom_x", zoomx);
        }

        static IntPtr zoom_yget(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@zoom_y");
        }

        static IntPtr zoom_yset(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 1);
            IntPtr zoomy = IntPtr.Zero;
            if (Ruby.Array.Is(Args, 0, "Integer"))
            {
                zoomy = Ruby.Float.ToPtr(Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0)));
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Float");
                zoomy = Ruby.Array.Get(Args, 0);
            }
            SpriteDictionary[Self].ZoomY = Ruby.Float.FromPtr(zoomy);
            return Ruby.SetIVar(Self, "@zoom_y", zoomy);
        }

        static IntPtr opacityget(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@opacity");
        }

        static IntPtr opacityset(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 1);
            IntPtr opacity = IntPtr.Zero;
            byte realopacity = 0;
            if (Ruby.Array.Is(Args, 0, "Float"))
            {
                double v = Ruby.Float.FromPtr(Ruby.Array.Get(Args, 0));
                if (v < 0) opacity = Ruby.Float.ToPtr(0);
                else if (v > 255) opacity = Ruby.Float.ToPtr(255);
                else opacity = Ruby.Array.Get(Args, 0);
                realopacity = (byte) Math.Round(v);
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                int v = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
                if (v < 0) opacity = Ruby.Integer.ToPtr(0);
                else if (v > 255) opacity = Ruby.Integer.ToPtr(255);
                else opacity = Ruby.Array.Get(Args, 0);
                realopacity = (byte) v;
            }
            SpriteDictionary[Self].Opacity = realopacity;
            return Ruby.SetIVar(Self, "@opacity", opacity);
        }

        static IntPtr angleget(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@angle");
        }

        static IntPtr angleset(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 1);
            IntPtr angle = IntPtr.Zero;
            short realangle = 0;
            if (Ruby.Array.Is(Args, 0, "Float"))
            {
                angle = Ruby.Array.Get(Args, 0);
                realangle = (short) (Ruby.Float.RoundFromPtr(angle) % 360);
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                angle = Ruby.Array.Get(Args, 0);
                realangle = (short) (Ruby.Integer.FromPtr(angle) % 360);
            }
            SpriteDictionary[Self].Angle = realangle;
            return Ruby.SetIVar(Self, "@angle", angle);
        }

        static IntPtr src_rectget(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@src_rect");
        }

        static IntPtr src_rectset(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "Rect");
            Ruby.SetIVar(Ruby.GetIVar(Self, "@src_rect"), "@__sprite__", Ruby.Nil);
            SpriteDictionary[Self].SrcRect = Rect.CreateRect(Ruby.Array.Get(Args, 0));
            Ruby.SetIVar(Ruby.Array.Get(Args, 0), "@__sprite__", Self);
            return Ruby.SetIVar(Self, "@src_rect", Ruby.Array.Get(Args, 0));
        }

        static IntPtr visibleget(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@visible");
        }

        static IntPtr visibleset(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "TrueClass", "FalseClass", "NilClass");
            SpriteDictionary[Self].Visible = Ruby.Array.Get(Args, 0) == Ruby.True;
            return Ruby.SetIVar(Self, "@visible", Ruby.Array.Get(Args, 0) == Ruby.True ? Ruby.True : Ruby.False);
        }

        static IntPtr mirror_xget(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@mirror_x");
        }

        static IntPtr mirror_xset(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "TrueClass", "FalseClass", "NilClass");
            SpriteDictionary[Self].MirrorX = Ruby.Array.Get(Args, 0) == Ruby.True;
            return Ruby.SetIVar(Self, "@mirror_x", Ruby.Array.Get(Args, 0) == Ruby.True ? Ruby.True : Ruby.False);
        }

        static IntPtr mirror_yget(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@mirror_y");
        }

        static IntPtr mirror_yset(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "TrueClass", "FalseClass", "NilClass");
            SpriteDictionary[Self].MirrorY = Ruby.Array.Get(Args, 0) == Ruby.True;
            return Ruby.SetIVar(Self, "@mirror_y", Ruby.Array.Get(Args, 0) == Ruby.True ? Ruby.True : Ruby.False);
        }

        static IntPtr colorget(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@color");
        }

        static IntPtr colorset(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "Color");
            Ruby.SetIVar(Ruby.GetIVar(Self, "@color"), "@__sprite__", Ruby.Nil);
            SpriteDictionary[Self].Color = Color.CreateColor(Ruby.Array.Get(Args, 0));
            Ruby.SetIVar(Ruby.Array.Get(Args, 0), "@__sprite__", Self);
            return Ruby.SetIVar(Self, "@color", Ruby.Array.Get(Args, 0));
        }

        static IntPtr toneget(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@tone");
        }

        static IntPtr toneset(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "Tone");
            Ruby.SetIVar(Ruby.GetIVar(Self, "@tone"), "@__sprite__", Ruby.Nil);
            SpriteDictionary[Self].Tone = Tone.CreateTone(Ruby.Array.Get(Args, 0));
            Ruby.SetIVar(Ruby.Array.Get(Args, 0), "@__sprite__", Self);
            return Ruby.SetIVar(Self, "@tone", Ruby.Array.Get(Args, 0));
        }

        static IntPtr update(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            return Ruby.Nil;
        }

        static IntPtr dispose(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            SpriteDictionary[Self].Dispose();
            Ruby.SetIVar(Ruby.GetIVar(Self, "@src_rect"), "@__sprite__", Ruby.Nil);
            Ruby.SetIVar(Ruby.GetIVar(Self, "@color"), "@__sprite__", Ruby.Nil);
            Ruby.SetIVar(Ruby.GetIVar(Self, "@tone"), "@__sprite__", Ruby.Nil);
            foreach (KeyValuePair<IntPtr, odl.Bitmap> kvp in Bitmap.BitmapDictionary)
            {
                if (kvp.Value == SpriteDictionary[Self].Bitmap)
                {
                    Bitmap.BitmapDictionary.Remove(kvp.Key);
                    break;
                }
            }
            SpriteDictionary.Remove(Self);
            Ruby.SetIVar(Self, "@disposed", Ruby.True);
            return Ruby.True;
        }

        static IntPtr disposed(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@disposed") == Ruby.True ? Ruby.True : Ruby.False;
        }

        public static void GuardDisposed(IntPtr Self)
        {
            if (Ruby.GetIVar(Self, "@disposed") == Ruby.True)
            {
                Ruby.Raise(Ruby.ErrorType.RuntimeError, "sprite already disposed");
            }
        }
    }
}

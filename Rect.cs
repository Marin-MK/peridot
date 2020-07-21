using System;
using rubydotnet;

namespace peridot
{
    public static class Rect
    {
        public static IntPtr Class;

        public static void Create()
        {
            Class = Ruby.Class.Define("Rect");
            Ruby.Class.DefineMethod(Class, "initialize", initialize);
            Ruby.Class.DefineMethod(Class, "x", xget);
            Ruby.Class.DefineMethod(Class, "x=", xset);
            Ruby.Class.DefineMethod(Class, "y", yget);
            Ruby.Class.DefineMethod(Class, "y=", yset);
            Ruby.Class.DefineMethod(Class, "width", widthget);
            Ruby.Class.DefineMethod(Class, "width=", widthset);
            Ruby.Class.DefineMethod(Class, "height", heightget);
            Ruby.Class.DefineMethod(Class, "height=", heightset);
            Ruby.Class.DefineMethod(Class, "set", set);
        }

        public static odl.Rect CreateRect(IntPtr Self)
        {
            int x = 0,
                y = 0,
                w = 0,
                h = 0;
            if (Ruby.IVarIs(Self, "@x", "Float")) x = (byte) Ruby.Float.RoundFromPtr(Ruby.GetIVar(Self, "@x"));
            else x = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Self, "@x"));
            if (Ruby.IVarIs(Self, "@y", "Float")) y = (byte) Ruby.Float.RoundFromPtr(Ruby.GetIVar(Self, "@y"));
            else y = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Self, "@y"));
            if (Ruby.IVarIs(Self, "@width", "Float")) w = (byte) Ruby.Float.RoundFromPtr(Ruby.GetIVar(Self, "@width"));
            else w = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Self, "@width"));
            if (Ruby.IVarIs(Self, "@height", "Float")) h = (byte) Ruby.Float.RoundFromPtr(Ruby.GetIVar(Self, "@height"));
            else h = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Self, "@height"));
            return new odl.Rect(x, y, w, h);
        }

        public static IntPtr CreateRect(odl.Rect Rect)
        {
            return Ruby.Funcall(Class, "new", Ruby.Integer.ToPtr(Rect.X), Ruby.Integer.ToPtr(Rect.Y), Ruby.Integer.ToPtr(Rect.Width), Ruby.Integer.ToPtr(Rect.Height));
        }

        static IntPtr initialize(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 4);
            Ruby.Array.Expect(Args, 0, "Integer");
            Ruby.Array.Expect(Args, 1, "Integer");
            Ruby.Array.Expect(Args, 2, "Integer");
            Ruby.Array.Expect(Args, 3, "Integer");
            Ruby.SetIVar(Self, "@x", Ruby.Array.Get(Args, 0));
            Ruby.SetIVar(Self, "@y", Ruby.Array.Get(Args, 1));
            Ruby.SetIVar(Self, "@width", Ruby.Array.Get(Args, 2));
            Ruby.SetIVar(Self, "@height", Ruby.Array.Get(Args, 3));
            return Self;
        }

        static IntPtr xget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@x");
        }

        static IntPtr xset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            int x = 0;
            if (Ruby.Array.Is(Args, 0, "Float"))
            {
                x = Ruby.Float.RoundFromPtr(Ruby.Array.Get(Args, 0));
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                x = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
            }
            if (Ruby.GetIVar(Self, "@__sprite__") != Ruby.Nil)
            {
                Sprite.SpriteDictionary[Ruby.GetIVar(Self, "@__sprite__")].SrcRect.X = x;
            }
            if (Ruby.GetIVar(Self, "@__viewport__") != Ruby.Nil)
            {
                Viewport.ViewportDictionary[Ruby.GetIVar(Self, "@__viewport__")].X = x;
            }
            return Ruby.SetIVar(Self, "@x", Ruby.Array.Get(Args, 0));
        }

        static IntPtr yget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@y");
        }

        static IntPtr yset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            int y = 0;
            if (Ruby.Array.Is(Args, 0, "Float"))
            {
                y = Ruby.Float.RoundFromPtr(Ruby.Array.Get(Args, 0));
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                y = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
            }
            if (Ruby.GetIVar(Self, "@__sprite__") != Ruby.Nil)
            {
                Sprite.SpriteDictionary[Ruby.GetIVar(Self, "@__sprite__")].SrcRect.Y = y;
            }
            if (Ruby.GetIVar(Self, "@__viewport__") != Ruby.Nil)
            {
                Viewport.ViewportDictionary[Ruby.GetIVar(Self, "@__viewport__")].Y = y;
            }
            return Ruby.SetIVar(Self, "@y", Ruby.Array.Get(Args, 0));
        }

        static IntPtr widthget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@width");
        }

        static IntPtr widthset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            int w = 0;
            if (Ruby.Array.Is(Args, 0, "Float"))
            {
                w = Ruby.Float.RoundFromPtr(Ruby.Array.Get(Args, 0));
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                w = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
            }
            if (Ruby.GetIVar(Self, "@__sprite__") != Ruby.Nil)
            {
                Sprite.SpriteDictionary[Ruby.GetIVar(Self, "@__sprite__")].SrcRect.Width = w;
            }
            if (Ruby.GetIVar(Self, "@__viewport__") != Ruby.Nil)
            {
                Viewport.ViewportDictionary[Ruby.GetIVar(Self, "@__viewport__")].Width = w;
            }
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
            int h = 0;
            if (Ruby.Array.Is(Args, 0, "Float"))
            {
                h = Ruby.Float.RoundFromPtr(Ruby.Array.Get(Args, 0));
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                h = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
            }
            if (Ruby.GetIVar(Self, "@__sprite__") != Ruby.Nil)
            {
                Sprite.SpriteDictionary[Ruby.GetIVar(Self, "@__sprite__")].SrcRect.Height = h;
            }
            if (Ruby.GetIVar(Self, "@__viewport__") != Ruby.Nil)
            {
                Viewport.ViewportDictionary[Ruby.GetIVar(Self, "@__viewport__")].Height = h;
            }
            return Ruby.SetIVar(Self, "@height", Ruby.Array.Get(Args, 0));
        }

        static IntPtr set(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 4);
            int x = 0,
                y = 0,
                w = 0,
                h = 0;
            if (Ruby.Array.Is(Args, 0, "Float"))
            {
                x = Ruby.Float.RoundFromPtr(Ruby.Array.Get(Args, 0));
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                x = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
            }
            if (Ruby.Array.Is(Args, 1, "Float"))
            {
                y = Ruby.Float.RoundFromPtr(Ruby.Array.Get(Args, 1));
            }
            else
            {
                Ruby.Array.Expect(Args, 1, "Integer");
                y = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 1));
            }
            if (Ruby.Array.Is(Args, 2, "Float"))
            {
                w = Ruby.Float.RoundFromPtr(Ruby.Array.Get(Args, 2));
            }
            else
            {
                Ruby.Array.Expect(Args, 2, "Integer");
                w = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 2));
            }
            if (Ruby.Array.Is(Args, 3, "Float"))
            {
                h = Ruby.Float.RoundFromPtr(Ruby.Array.Get(Args, 3));
            }
            else
            {
                Ruby.Array.Expect(Args, 3, "Integer");
                h = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 3));
            }
            Ruby.SetIVar(Self, "@x", Ruby.Array.Get(Args, 0));
            Ruby.SetIVar(Self, "@y", Ruby.Array.Get(Args, 1));
            Ruby.SetIVar(Self, "@width", Ruby.Array.Get(Args, 2));
            Ruby.SetIVar(Self, "@height", Ruby.Array.Get(Args, 3));
            if (Ruby.GetIVar(Self, "@__sprite__") != Ruby.Nil)
            {
                Sprite.SpriteDictionary[Ruby.GetIVar(Self, "@__sprite__")].SrcRect.X = x;
                Sprite.SpriteDictionary[Ruby.GetIVar(Self, "@__sprite__")].SrcRect.Y = y;
                Sprite.SpriteDictionary[Ruby.GetIVar(Self, "@__sprite__")].SrcRect.Width = w;
                Sprite.SpriteDictionary[Ruby.GetIVar(Self, "@__sprite__")].SrcRect.Height = h;
            }
            if (Ruby.GetIVar(Self, "@__viewport__") != Ruby.Nil)
            {
                Viewport.ViewportDictionary[Ruby.GetIVar(Self, "@__viewport__")].X = x;
                Viewport.ViewportDictionary[Ruby.GetIVar(Self, "@__viewport__")].Y = y;
                Viewport.ViewportDictionary[Ruby.GetIVar(Self, "@__viewport__")].Width = w;
                Viewport.ViewportDictionary[Ruby.GetIVar(Self, "@__viewport__")].Height = h;
            }
            return Self;
        }
    }
}

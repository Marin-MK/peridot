using System;
using rubydotnet;

namespace peridot
{
    public class Rect : Ruby.Object
    {
        public new static string KlassName = "Rect";
        public new static Ruby.Class Class;

        public Rect(IntPtr Pointer) : base(Pointer) { }

        public static void Create()
        {
            Ruby.Class c = Ruby.Class.DefineClass<Rect>(KlassName);
            Class = c;
            c.DefineMethod("initialize", initialize);
            c.DefineMethod("x", xget);
            c.DefineMethod("x=", xset);
            c.DefineMethod("y", yget);
            c.DefineMethod("y=", yset);
            c.DefineMethod("width", widthget);
            c.DefineMethod("width=", widthset);
            c.DefineMethod("height", heightget);
            c.DefineMethod("height=", heightset);
            c.DefineMethod("set", set);
        }

        public static odl.Rect CreateRect(Ruby.Object Self)
        {
            int x = 0,
                y = 0,
                w = 0,
                h = 0;
            if (Self.GetIVar("@x").Is(Ruby.Float.Class))
            {
                x = (int) Math.Round(Self.AutoGetIVar<Ruby.Float>("@x"));
            }
            else
            {
                Self.GetIVar("@x").Expect(Ruby.Integer.Class);
                x = Self.AutoGetIVar<Ruby.Integer>("@x");
            }
            if (Self.GetIVar("@y").Is(Ruby.Float.Class))
            {
                y = (int) Math.Round(Self.AutoGetIVar<Ruby.Float>("@y"));
            }
            else
            {
                Self.GetIVar("@y").Expect(Ruby.Integer.Class);
                y = Self.AutoGetIVar<Ruby.Integer>("@y");
            }
            if (Self.GetIVar("@width").Is(Ruby.Float.Class))
            {
                w = (int) Math.Round(Self.AutoGetIVar<Ruby.Float>("@width"));
            }
            else
            {
                Self.GetIVar("@width").Expect(Ruby.Integer.Class);
                w = Self.AutoGetIVar<Ruby.Integer>("@width");
            }
            if (Self.GetIVar("@height").Is(Ruby.Float.Class))
            {
                h = (int) Math.Round(Self.AutoGetIVar<Ruby.Float>("@height"));
            }
            else
            {
                Self.GetIVar("@height").Expect(Ruby.Integer.Class);
                h = Self.AutoGetIVar<Ruby.Integer>("@height");
            }
            return new odl.Rect(x, y, w, h);
        }

        public static Rect CreateRect(odl.Rect Rect)
        {
            return Class.AutoFuncall<Rect>("new", (Ruby.Integer) Rect.X, (Ruby.Integer) Rect.Y, (Ruby.Integer) Rect.Width, (Ruby.Integer) Rect.Height);
        }

        protected static Ruby.Object initialize(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(4);
            Args[0].Expect(Ruby.Integer.Class);
            Args[1].Expect(Ruby.Integer.Class);
            Args[2].Expect(Ruby.Integer.Class);
            Args[3].Expect(Ruby.Integer.Class);
            Self.SetIVar("@x", Args[0]);
            Self.SetIVar("@y", Args[1]);
            Self.SetIVar("@width", Args[2]);
            Self.SetIVar("@height", Args[3]);
            return Self;
        }

        protected static Ruby.Object xget(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(0);
            return Self.GetIVar("@x");
        }

        protected static Ruby.Object xset(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(1);
            int x = 0;
            if (Args[0].Is(Ruby.Float.Class))
            {
                x = (int) Math.Round(Args.Get<Ruby.Float>(0));
            }
            else
            {
                Args[0].Expect(Ruby.Integer.Class);
                x = Args.Get<Ruby.Integer>(0);
            }
            if (Self.GetIVar("@__sprite__") != Ruby.Nil)
            {
                Sprite.SpriteDictionary[Self.RawGetIVar("@__sprite__")].SrcRect.X = x;
            }
            if (Self.GetIVar("@__viewport__") != Ruby.Nil)
            {
                Viewport.ViewportDictionary[Self.RawGetIVar("@__viewport__")].X = x;
            }
            return Self.SetIVar("@x", Args[0]);
        }

        protected static Ruby.Object yget(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(0);
            return Self.GetIVar("@y");
        }

        protected static Ruby.Object yset(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(1);
            int y = 0;
            if (Args[0].Is(Ruby.Float.Class))
            {
                y = (int) Math.Round(Args.Get<Ruby.Float>(0));
            }
            else
            {
                Args[0].Expect(Ruby.Integer.Class);
                y = Args.Get<Ruby.Integer>(0);
            }
            if (Self.GetIVar("@__sprite__") != Ruby.Nil)
            {
                Sprite.SpriteDictionary[Self.RawGetIVar("@__sprite__")].SrcRect.Y = y;
            }
            if (Self.GetIVar("@__viewport__") != Ruby.Nil)
            {
                Viewport.ViewportDictionary[Self.RawGetIVar("@__viewport__")].Y = y;
            }
            return Self.SetIVar("@y", Args[0]);
        }


        protected static Ruby.Object widthget(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(0);
            return Self.GetIVar("@width");
        }

        protected static Ruby.Object widthset(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(1);
            int w = 0;
            if (Args[0].Is(Ruby.Float.Class))
            {
                w = (int) Math.Round(Args.Get<Ruby.Float>(0));
            }
            else
            {
                Args[0].Expect(Ruby.Integer.Class);
                w = Args.Get<Ruby.Integer>(0);
            }
            if (Self.GetIVar("@__sprite__") != Ruby.Nil)
            {
                Sprite.SpriteDictionary[Self.RawGetIVar("@__sprite__")].SrcRect.Width = w;
            }
            if (Self.GetIVar("@__viewport__") != Ruby.Nil)
            {
                Viewport.ViewportDictionary[Self.RawGetIVar("@__viewport__")].Width = w;
            }
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
            int h = 0;
            if (Args[0].Is(Ruby.Float.Class))
            {
                h = (int) Math.Round(Args.Get<Ruby.Float>(0));
            }
            else
            {
                Args[0].Expect(Ruby.Integer.Class);
                h = Args.Get<Ruby.Integer>(0);
            }
            if (Self.GetIVar("@__sprite__") != Ruby.Nil)
            {
                Sprite.SpriteDictionary[Self.RawGetIVar("@__sprite__")].SrcRect.Height = h;
            }
            if (Self.GetIVar("@__viewport__") != Ruby.Nil)
            {
                Viewport.ViewportDictionary[Self.RawGetIVar("@__viewport__")].Height = h;
            }
            return Self.SetIVar("@height", Args[0]);
        }


        protected static Ruby.Object set(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(4);
            int x = 0,
                y = 0,
                w = 0,
                h = 0;
            if (Args[0].Is(Ruby.Float.Class))
            {
                x = (int) Math.Round(Args.Get<Ruby.Float>(0));
            }
            else
            {
                Args[0].Expect(Ruby.Integer.Class);
                x = Args.Get<Ruby.Integer>(0);
            }
            if (Args[1].Is(Ruby.Float.Class))
            {
                y = (int) Math.Round(Args.Get<Ruby.Float>(1));
            }
            else
            {
                Args[1].Expect(Ruby.Integer.Class);
                y = Args.Get<Ruby.Integer>(1);
            }
            if (Args[2].Is(Ruby.Float.Class))
            {
                w = (int) Math.Round(Args.Get<Ruby.Float>(2));
            }
            else
            {
                Args[2].Expect(Ruby.Integer.Class);
                w = Args.Get<Ruby.Integer>(2);
            }
            if (Args[3].Is(Ruby.Float.Class))
            {
                h = (int) Math.Round(Args.Get<Ruby.Float>(3));
            }
            else
            {
                Args[3].Expect(Ruby.Integer.Class);
                h = Args.Get<Ruby.Integer>(3);
            }
            Self.SetIVar("@x", Args[0]);
            Self.SetIVar("@y", Args[1]);
            Self.SetIVar("@width", Args[2]);
            Self.SetIVar("@height", Args[3]);
            if (Self.GetIVar("@__sprite__") != Ruby.Nil)
            {
                Sprite.SpriteDictionary[Self.RawGetIVar("@__sprite__")].SrcRect.X = x;
                Sprite.SpriteDictionary[Self.RawGetIVar("@__sprite__")].SrcRect.Y = y;
                Sprite.SpriteDictionary[Self.RawGetIVar("@__sprite__")].SrcRect.Width = w;
                Sprite.SpriteDictionary[Self.RawGetIVar("@__sprite__")].SrcRect.Height = h;
            }
            if (Self.GetIVar("@__viewport__") != Ruby.Nil)
            {
                Viewport.ViewportDictionary[Self.RawGetIVar("@__viewport__")].X = x;
                Viewport.ViewportDictionary[Self.RawGetIVar("@__viewport__")].Y = y;
                Viewport.ViewportDictionary[Self.RawGetIVar("@__viewport__")].Width = w;
                Viewport.ViewportDictionary[Self.RawGetIVar("@__viewport__")].Height = h;
            }
            return Self;
        }
    }
}

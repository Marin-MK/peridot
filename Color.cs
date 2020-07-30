using System;
using System.Threading;
using rubydotnet;

namespace peridot
{
    public static class Color
    {
        public static IntPtr Class;

        public static void Create()
        {
            Class = Ruby.Class.Define("Color");
            Ruby.Class.DefineClassMethod(Class, "_load", _load);
            Ruby.Class.DefineMethod(Class, "initialize", initialize);
            Ruby.Class.DefineMethod(Class, "red", redget);
            Ruby.Class.DefineMethod(Class, "red=", redset);
            Ruby.Class.DefineMethod(Class, "green", greenget);
            Ruby.Class.DefineMethod(Class, "green=", greenset);
            Ruby.Class.DefineMethod(Class, "blue", blueget);
            Ruby.Class.DefineMethod(Class, "blue=", blueset);
            Ruby.Class.DefineMethod(Class, "alpha", alphaget);
            Ruby.Class.DefineMethod(Class, "alpha=", alphaset);
        }

        public static odl.Color CreateColor(IntPtr Self)
        {
            byte R = 0,
                 G = 0,
                 B = 0,
                 A = 0;
            if (Ruby.IVarIs(Self, "@red", "Float")) R = (byte) Ruby.Float.RoundFromPtr(Ruby.GetIVar(Self, "@red"));
            else R = (byte) Ruby.Integer.FromPtr(Ruby.GetIVar(Self, "@red"));
            if (Ruby.IVarIs(Self, "@green", "Float")) G = (byte) Ruby.Float.RoundFromPtr(Ruby.GetIVar(Self, "@green"));
            else G = (byte) Ruby.Integer.FromPtr(Ruby.GetIVar(Self, "@green"));
            if (Ruby.IVarIs(Self, "@blue", "Float")) B = (byte) Ruby.Float.RoundFromPtr(Ruby.GetIVar(Self, "@blue"));
            else B = (byte) Ruby.Integer.FromPtr(Ruby.GetIVar(Self, "@blue"));
            if (Ruby.IVarIs(Self, "@alpha", "Float")) A = (byte) Ruby.Float.RoundFromPtr(Ruby.GetIVar(Self, "@alpha"));
            else A = (byte) Ruby.Integer.FromPtr(Ruby.GetIVar(Self, "@alpha"));
            return new odl.Color(R, G, B, A);
        }

        public static IntPtr CreateColor(odl.Color Color)
        {
            return Ruby.Funcall(Class, "new", Ruby.Integer.ToPtr(Color.Red), Ruby.Integer.ToPtr(Color.Green), Ruby.Integer.ToPtr(Color.Blue), Ruby.Integer.ToPtr(Color.Alpha));
        }

        static IntPtr initialize(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 3, 4);
            IntPtr R = Ruby.Integer.ToPtr(0),
                   G = R,
                   B = R,
                   A = R;
            if (Ruby.Array.Is(Args, 0, "Float"))
            {
                double v = Ruby.Float.FromPtr(Ruby.Array.Get(Args, 0));
                if (v < 0) R = Ruby.Float.ToPtr(0);
                else if (v > 255) R = Ruby.Float.ToPtr(255);
                else R = Ruby.Array.Get(Args, 0);
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                int v = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
                if (v < 0) R = Ruby.Integer.ToPtr(0);
                else if (v > 255) R = Ruby.Integer.ToPtr(255);
                else R = Ruby.Array.Get(Args, 0);
            }
            if (Ruby.Array.Is(Args, 1, "Float"))
            {
                double v = Ruby.Float.FromPtr(Ruby.Array.Get(Args, 1));
                if (v < 0) G = Ruby.Float.ToPtr(0);
                else if (v > 255) G = Ruby.Float.ToPtr(255);
                else G = Ruby.Array.Get(Args, 1);
            }
            else
            {
                Ruby.Array.Expect(Args, 1, "Integer");
                int v = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 1));
                if (v < 0) G = Ruby.Integer.ToPtr(0);
                else if (v > 255) G = Ruby.Integer.ToPtr(255);
                else G = Ruby.Array.Get(Args, 1);
            }
            if (Ruby.Array.Is(Args, 2, "Float"))
            {
                double v = Ruby.Float.FromPtr(Ruby.Array.Get(Args, 2));
                if (v < 0) B = Ruby.Float.ToPtr(0);
                else if (v > 255) B = Ruby.Float.ToPtr(255);
                else B = Ruby.Array.Get(Args, 2);
            }
            else
            {
                Ruby.Array.Expect(Args, 2, "Integer");
                int v = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 2));
                if (v < 0) B = Ruby.Integer.ToPtr(0);
                else if (v > 255) B = Ruby.Integer.ToPtr(255);
                else B = Ruby.Array.Get(Args, 2);
            }
            if (Ruby.Array.Length(Args) == 4)
            {
                if (Ruby.Array.Is(Args, 3, "Float"))
                {
                    double v = Ruby.Float.FromPtr(Ruby.Array.Get(Args, 3));
                    if (v < 0) A = Ruby.Float.ToPtr(0);
                    else if (v > 255) A = Ruby.Float.ToPtr(255);
                    else A = Ruby.Array.Get(Args, 3);
                }
                else
                {
                    Ruby.Array.Expect(Args, 3, "Integer");
                    int v = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 3));
                    if (v < 0) A = Ruby.Integer.ToPtr(0);
                    else if (v > 255) A = Ruby.Integer.ToPtr(255);
                    else A = Ruby.Array.Get(Args, 3);
                }
            }
            else A = Ruby.Integer.ToPtr(255);
            Ruby.SetIVar(Self, "@red", R);
            Ruby.SetIVar(Self, "@green", G);
            Ruby.SetIVar(Self, "@blue", B);
            Ruby.SetIVar(Self, "@alpha", A);
            return Self;
        }

        static IntPtr _load(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            
            IntPtr ary = Ruby.Funcall(Ruby.Array.Get(Args, 0), "unpack", Ruby.String.ToPtr("D*"));
            Ruby.Array.Expect(ary, 4);

            return Ruby.Funcall(Class, "new", Ruby.Array.Get(ary, 0), Ruby.Array.Get(ary, 2), Ruby.Array.Get(ary, 3), Ruby.Array.Get(ary, 4));
        }

        static IntPtr redget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@red");
        }

        static IntPtr redset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            IntPtr R;
            byte realr = 0;
            if (Ruby.Array.Is(Args, 0, "Float"))
            {
                double v = Ruby.Float.FromPtr(Ruby.Array.Get(Args, 0));
                if (v < 0) { v = 0; R = Ruby.Float.ToPtr(0); }
                else if (v > 255) { v = 255; R = Ruby.Float.ToPtr(255); }
                else R = Ruby.Array.Get(Args, 0);
                realr = (byte) Math.Round(v);
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                int v = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
                if (v < 0) { v = 0; R = Ruby.Integer.ToPtr(0); }
                else if (v > 255) { v = 255; R = Ruby.Integer.ToPtr(255); }
                else R = Ruby.Array.Get(Args, 0);
                realr = (byte) v;
            }
            if (Ruby.GetIVar(Self, "@__viewport__") != Ruby.Nil)
            {
                Viewport.ViewportDictionary[Ruby.GetIVar(Self, "@__viewport__")].Color.Red = realr;
            }
            if (Ruby.GetIVar(Self, "@__sprite__") != Ruby.Nil)
            {
                Sprite.SpriteDictionary[Ruby.GetIVar(Self, "@__sprite__")].Color.Red = realr;
            }
            return Ruby.SetIVar(Self, "@red", R);
        }

        static IntPtr greenget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@green");
        }

        static IntPtr greenset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            IntPtr G;
            byte realg = 0;
            if (Ruby.Array.Is(Args, 0, "Float"))
            {
                double v = Ruby.Float.FromPtr(Ruby.Array.Get(Args, 0));
                if (v < 0) { v = 0; G = Ruby.Float.ToPtr(0); }
                else if (v > 255) { v = 255; G = Ruby.Float.ToPtr(255); }
                else G = Ruby.Array.Get(Args, 0);
                realg = (byte) Math.Round(v);
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                int v = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
                if (v < 0) { v = 0; G = Ruby.Integer.ToPtr(0); }
                else if (v > 255) { v = 255; G = Ruby.Integer.ToPtr(255); }
                else G = Ruby.Array.Get(Args, 0);
                realg = (byte) v;
            }
            if (Ruby.GetIVar(Self, "@__viewport__") != Ruby.Nil)
            {
                Viewport.ViewportDictionary[Ruby.GetIVar(Self, "@__viewport__")].Color.Green = realg;
            }
            if (Ruby.GetIVar(Self, "@__sprite__") != Ruby.Nil)
            {
                Sprite.SpriteDictionary[Ruby.GetIVar(Self, "@__sprite__")].Color.Green = realg;
            }
            return Ruby.SetIVar(Self, "@green", G);
        }

        static IntPtr blueget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@blue");
        }

        static IntPtr blueset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            IntPtr B;
            byte realb = 0;
            if (Ruby.Array.Is(Args, 0, "Float"))
            {
                double v = Ruby.Float.FromPtr(Ruby.Array.Get(Args, 0));
                if (v < 0) { v = 0; B = Ruby.Float.ToPtr(0); }
                else if (v > 255) { v = 255; B = Ruby.Float.ToPtr(255); }
                else B = Ruby.Array.Get(Args, 0);
                realb = (byte) Math.Round(v);
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                int v = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
                if (v < 0) { v = 0; B = Ruby.Integer.ToPtr(0); }
                else if (v > 255) { v = 255; B = Ruby.Integer.ToPtr(255); }
                else B = Ruby.Array.Get(Args, 0);
                realb = (byte) v;
            }
            if (Ruby.GetIVar(Self, "@__viewport__") != Ruby.Nil)
            {
                Viewport.ViewportDictionary[Ruby.GetIVar(Self, "@__viewport__")].Color.Blue = realb;
            }
            if (Ruby.GetIVar(Self, "@__sprite__") != Ruby.Nil)
            {
                Sprite.SpriteDictionary[Ruby.GetIVar(Self, "@__sprite__")].Color.Blue = realb;
            }
            return Ruby.SetIVar(Self, "@blue", B);
        }

        static IntPtr alphaget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@alpha");
        }

        static IntPtr alphaset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            IntPtr A;
            byte reala = 0;
            if (Ruby.Array.Is(Args, 0, "Float"))
            {
                double v = Ruby.Float.FromPtr(Ruby.Array.Get(Args, 0));
                if (v < 0) { v = 0; A = Ruby.Float.ToPtr(0); }
                else if (v > 255) { v = 255; A = Ruby.Float.ToPtr(255); }
                else A = Ruby.Array.Get(Args, 0);
                reala = (byte) Math.Round(v);
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                int v = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
                if (v < 0) A = Ruby.Integer.ToPtr(0);
                else if (v > 255) A = Ruby.Integer.ToPtr(255);
                else A = Ruby.Array.Get(Args, 0);
                reala = (byte) v;
            }
            if (Ruby.GetIVar(Self, "@__viewport__") != Ruby.Nil)
            {
                Viewport.ViewportDictionary[Ruby.GetIVar(Self, "@__viewport__")].Color.Alpha = reala;
            }
            if (Ruby.GetIVar(Self, "@__sprite__") != Ruby.Nil)
            {
                Sprite.SpriteDictionary[Ruby.GetIVar(Self, "@__sprite__")].Color.Alpha = reala;
            }
            return Ruby.SetIVar(Self, "@alpha", A);
        }
    }
}

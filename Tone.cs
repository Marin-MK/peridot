using System;
using rubydotnet;

namespace peridot
{
    public static class Tone
    {
        public static IntPtr Class;

        public static void Create()
        {
            Class = Ruby.Class.Define("Tone");
            Ruby.Class.DefineMethod(Class, "initialize", initialize);
            Ruby.Class.DefineMethod(Class, "red", redget);
            Ruby.Class.DefineMethod(Class, "red=", redset);
            Ruby.Class.DefineMethod(Class, "green", greenget);
            Ruby.Class.DefineMethod(Class, "green=", greenset);
            Ruby.Class.DefineMethod(Class, "blue", blueget);
            Ruby.Class.DefineMethod(Class, "blue=", blueset);
            Ruby.Class.DefineMethod(Class, "grey", greyget);
            Ruby.Class.DefineMethod(Class, "grey=", greyset);
        }

        public static odl.Tone CreateTone(IntPtr Self)
        {
            short R = 0,
                  G = 0,
                  B = 0;
            byte Grey = 0;
            if (Ruby.IVarIs(Self, "@red", "Float")) R = (short) Ruby.Float.RoundFromPtr(Ruby.GetIVar(Self, "@red"));
            else R = (short) Ruby.Integer.FromPtr(Ruby.GetIVar(Self, "@red"));
            if (Ruby.IVarIs(Self, "@green", "Float")) G = (short) Ruby.Float.RoundFromPtr(Ruby.GetIVar(Self, "@green"));
            else G = (short) Ruby.Integer.FromPtr(Ruby.GetIVar(Self, "@green"));
            if (Ruby.IVarIs(Self, "@blue", "Float")) B = (short) Ruby.Float.RoundFromPtr(Ruby.GetIVar(Self, "@blue"));
            else B = (short) Ruby.Integer.FromPtr(Ruby.GetIVar(Self, "@blue"));
            if (Ruby.IVarIs(Self, "@grey", "Float")) Grey = (byte) Ruby.Float.RoundFromPtr(Ruby.GetIVar(Self, "@grey"));
            else Grey = (byte) Ruby.Integer.FromPtr(Ruby.GetIVar(Self, "@grey"));
            return new odl.Tone(R, G, B, Grey);
        }

        public static IntPtr CreateTone(odl.Tone Tone)
        {
            return Ruby.Funcall(Class, "new", Ruby.Integer.ToPtr(Tone.Red), Ruby.Integer.ToPtr(Tone.Green), Ruby.Integer.ToPtr(Tone.Blue), Ruby.Integer.ToPtr(Tone.Gray));
        }

        static IntPtr initialize(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 3, 4);
            IntPtr R = Ruby.Integer.ToPtr(0),
                   G = R,
                   B = R,
                   Grey = R;
            if (Ruby.Array.Is(Args, 0, "Float"))
            {
                double v = Ruby.Float.FromPtr(Ruby.Array.Get(Args, 0));
                if (v < -255) R = Ruby.Float.ToPtr(-255);
                else if (v > 255) R = Ruby.Float.ToPtr(255);
                else R = Ruby.Array.Get(Args, 0);
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                int v = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
                if (v < -255) R = Ruby.Integer.ToPtr(-255);
                else if (v > 255) R = Ruby.Integer.ToPtr(255);
                else R = Ruby.Array.Get(Args, 0);
            }
            if (Ruby.Array.Is(Args, 1, "Float"))
            {
                double v = Ruby.Float.FromPtr(Ruby.Array.Get(Args, 1));
                if (v < -255) G = Ruby.Float.ToPtr(-255);
                else if (v > 255) G = Ruby.Float.ToPtr(255);
                else G = Ruby.Array.Get(Args, 1);
            }
            else
            {
                Ruby.Array.Expect(Args, 1, "Integer");
                int v = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 1));
                if (v < -255) G = Ruby.Integer.ToPtr(-255);
                else if (v > 255) G = Ruby.Integer.ToPtr(255);
                else G = Ruby.Array.Get(Args, 1);
            }
            if (Ruby.Array.Is(Args, 2, "Float"))
            {
                double v = Ruby.Float.FromPtr(Ruby.Array.Get(Args, 2));
                if (v < -255) B = Ruby.Float.ToPtr(-255);
                else if (v > 255) B = Ruby.Float.ToPtr(255);
                else B = Ruby.Array.Get(Args, 2);
            }
            else
            {
                Ruby.Array.Expect(Args, 2, "Integer");
                int v = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 2));
                if (v < -255) B = Ruby.Integer.ToPtr(-255);
                else if (v > 255) B = Ruby.Integer.ToPtr(255);
                else B = Ruby.Array.Get(Args, 2);
            }
            if (Ruby.Array.Length(Args) == 4)
            {
                if (Ruby.Array.Is(Args, 3, "Float"))
                {
                    double v = Ruby.Float.FromPtr(Ruby.Array.Get(Args, 3));
                    if (v < -255) Grey = Ruby.Float.ToPtr(-255);
                    else if (v > 255) Grey = Ruby.Float.ToPtr(255);
                    else Grey = Ruby.Array.Get(Args, 3);
                }
                else
                {
                    Ruby.Array.Expect(Args, 3, "Integer");
                    int v = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 3));
                    if (v < 0) Grey = Ruby.Integer.ToPtr(0);
                    else if (v > 255) Grey = Ruby.Integer.ToPtr(255);
                    else Grey = Ruby.Array.Get(Args, 3);
                }
            }
            Ruby.SetIVar(Self, "@red", R);
            Ruby.SetIVar(Self, "@green", G);
            Ruby.SetIVar(Self, "@blue", B);
            Ruby.SetIVar(Self, "@grey", Grey);
            return Self;
        }

        static IntPtr redget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@red");
        }

        static IntPtr redset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            IntPtr R = Ruby.Integer.ToPtr(0);
            short realr = 0;
            if (Ruby.Array.Is(Args, 0, "Float"))
            {
                double v = Ruby.Float.FromPtr(Ruby.Array.Get(Args, 0));
                if (v < -255) { v = -255; R = Ruby.Float.ToPtr(-255); }
                else if (v > 255) { v = 255; R = Ruby.Float.ToPtr(255); }
                else R = Ruby.Array.Get(Args, 0);
                realr = (short) Math.Round(v);
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                int v = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
                if (v < -255) { v = -255; R = Ruby.Integer.ToPtr(-255); }
                else if (v > 255) { v = 255; R = Ruby.Integer.ToPtr(255); }
                else R = Ruby.Array.Get(Args, 0);
                realr = (short) v;
            }
            if (Ruby.GetIVar(Self, "@__sprite__") != Ruby.Nil)
            {
                Sprite.SpriteDictionary[Ruby.GetIVar(Self, "@__sprite__")].Tone.Red = realr;
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
            IntPtr G = Ruby.Integer.ToPtr(0);
            short realg = 0;
            if (Ruby.Array.Is(Args, 0, "Float"))
            {
                double v = Ruby.Float.FromPtr(Ruby.Array.Get(Args, 0));
                if (v < -255) { v = -255; G = Ruby.Float.ToPtr(-255); }
                else if (v > 255) { v = 255; G = Ruby.Float.ToPtr(255); }
                else G = Ruby.Array.Get(Args, 0);
                realg = (short) Math.Round(v);
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                int v = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
                if (v < -255) { v = -255; G = Ruby.Integer.ToPtr(-255); }
                else if (v > 255) { v = 255; G = Ruby.Integer.ToPtr(255); }
                else G = Ruby.Array.Get(Args, 0);
                realg = (short) v;
            }
            if (Ruby.GetIVar(Self, "@__sprite__") != Ruby.Nil)
            {
                Sprite.SpriteDictionary[Ruby.GetIVar(Self, "@__sprite__")].Tone.Green = realg;
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
            IntPtr B = Ruby.Integer.ToPtr(0);
            short realb = 0;
            if (Ruby.Array.Is(Args, 0, "Float"))
            {
                double v = Ruby.Float.FromPtr(Ruby.Array.Get(Args, 0));
                if (v < -255) { v = -255; B = Ruby.Float.ToPtr(-255); }
                else if (v > 255) { v = 255; B = Ruby.Float.ToPtr(255); }
                else B = Ruby.Array.Get(Args, 0);
                realb = (short) Math.Round(v);
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                int v = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
                if (v < -255) { v = -255; B = Ruby.Integer.ToPtr(-255); }
                else if (v > 255) { v = 255; B = Ruby.Integer.ToPtr(255); }
                else B = Ruby.Array.Get(Args, 0);
                realb = (short) v;
            }
            if (Ruby.GetIVar(Self, "@__sprite__") != Ruby.Nil)
            {
                Sprite.SpriteDictionary[Ruby.GetIVar(Self, "@__sprite__")].Tone.Blue = realb;
            }
            return Ruby.SetIVar(Self, "@blue", B);
        }

        static IntPtr greyget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@grey");
        }

        static IntPtr greyset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            IntPtr Grey = Ruby.Integer.ToPtr(0);
            byte realgrey = 0;
            if (Ruby.Array.Is(Args, 0, "Float"))
            {
                double v = Ruby.Float.FromPtr(Ruby.Array.Get(Args, 0));
                if (v < 0) { v = 0; Grey = Ruby.Float.ToPtr(0); }
                else if (v > 255) { v = 255; Grey = Ruby.Float.ToPtr(255); }
                else Grey = Ruby.Array.Get(Args, 0);
                realgrey = (byte) Math.Round(v);
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                int v = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
                if (v < 0) Grey = Ruby.Integer.ToPtr(0);
                else if (v > 255) Grey = Ruby.Integer.ToPtr(255);
                else Grey = Ruby.Array.Get(Args, 0);
                realgrey = (byte) v;
            }
            if (Ruby.GetIVar(Self, "@__sprite__") != Ruby.Nil)
            {
                Sprite.SpriteDictionary[Ruby.GetIVar(Self, "@__sprite__")].Tone.Gray = realgrey;
            }
            return Ruby.SetIVar(Self, "@grey", Grey);
        }
    }
}

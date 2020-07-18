using System;
using rubydotnet;

namespace peridot
{
    public class Tone : Ruby.Object
    {
        public new static string KlassName = "Tone";
        public new static Ruby.Class Class;

        public Tone(IntPtr Pointer) : base(Pointer) { }

        public static void Create()
        {
            Ruby.Class c = Ruby.Class.DefineClass<Tone>(KlassName);
            Class = c;
            c.DefineMethod("initialize", initialize);
            c.DefineMethod("red", redget);
            c.DefineMethod("red=", redset);
            c.DefineMethod("green", greenget);
            c.DefineMethod("green=", greenset);
            c.DefineMethod("blue", blueget);
            c.DefineMethod("blue=", blueset);
            c.DefineMethod("grey", greyget);
            c.DefineMethod("grey=", greyset);
        }

        public static odl.Tone CreateTone(Ruby.Object Self)
        {
            short R = 0,
                  G = 0,
                  B = 0;
            byte Grey = 0;
            if (Self.Is(Ruby.Float.Class))
            {
                R = (short) Math.Round(Self.AutoGetIVar<Ruby.Float>("@red"));
            }
            else
            {
                R = (short) Self.AutoGetIVar<Ruby.Integer>("@red");
            }
            if (Self.Is(Ruby.Float.Class))
            {
                G = (short) Math.Round(Self.AutoGetIVar<Ruby.Float>("@green"));
            }
            else
            {
                G = (short) Self.AutoGetIVar<Ruby.Integer>("@green");
            }
            if (Self.Is(Ruby.Float.Class))
            {
                B = (short) Math.Round(Self.AutoGetIVar<Ruby.Float>("@blue"));
            }
            else
            {
                B = (short) Self.AutoGetIVar<Ruby.Integer>("@blue");
            }
            if (Self.Is(Ruby.Float.Class))
            {
                Grey = (byte) Math.Round(Self.AutoGetIVar<Ruby.Float>("@grey"));
            }
            else
            {
                Grey = (byte) Self.AutoGetIVar<Ruby.Integer>("@grey");
            }
            return new odl.Tone(R, G, B, Grey);
        }

        public static Tone CreateTone(odl.Tone Tone)
        {
            return Class.AutoFuncall<Tone>("new",
                (Ruby.Integer) Tone.Red,
                (Ruby.Integer) Tone.Green,
                (Ruby.Integer) Tone.Blue,
                (Ruby.Integer) Tone.Gray
            );
        }

        protected static Ruby.Object initialize(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(3, 4);
            Ruby.Object R = (Ruby.Integer) 0,
                        G = (Ruby.Integer) 0,
                        B = (Ruby.Integer) 0,
                        Grey = (Ruby.Integer) 0;
            if (Args.Length == 3 || Args.Length == 4)
            {
                if (Args[0].Is(Ruby.Float.Class))
                {
                    if (Args.Get<Ruby.Float>(0) < -255) R = (Ruby.Float) (-255);
                    else if (Args.Get<Ruby.Float>(0) > 255) R = (Ruby.Float) 255;
                    else R = Args[0];
                }
                else
                {
                    Args[0].Expect(Ruby.Integer.Class);
                    if (Args.Get<Ruby.Integer>(0) < -255) R = (Ruby.Integer) (-255);
                    else if (Args.Get<Ruby.Integer>(0) > 255) R = (Ruby.Integer) 255;
                    else R = Args[0];
                }
                if (Args[1].Is(Ruby.Float.Class))
                {
                    if (Args.Get<Ruby.Float>(1) < -255) G = (Ruby.Float) (-255);
                    else if (Args.Get<Ruby.Float>(1) > 255) G = (Ruby.Float) 255;
                    else G = Args[1];
                }
                else
                {
                    Args[1].Expect(Ruby.Integer.Class);
                    if (Args.Get<Ruby.Integer>(1) < -255) G = (Ruby.Integer) (-255);
                    else if (Args.Get<Ruby.Integer>(1) > 255) G = (Ruby.Integer) 255;
                    else G = Args[1];
                }
                if (Args[2].Is(Ruby.Float.Class))
                {
                    if (Args.Get<Ruby.Float>(2) < -255) B = (Ruby.Float) (-255);
                    else if (Args.Get<Ruby.Float>(2) > 255) B = (Ruby.Float) 255;
                    else B = Args[2];
                }
                else
                {
                    Args[2].Expect(Ruby.Integer.Class);
                    if (Args.Get<Ruby.Integer>(2) < -255) B = (Ruby.Integer) (-255);
                    else if (Args.Get<Ruby.Integer>(2) > 255) B = (Ruby.Integer) 255;
                    else B = Args[2];
                }
                if (Args.Length == 4)
                {
                    if (Args[3].Is(Ruby.Float.Class))
                    {
                        if (Args.Get<Ruby.Float>(3) < -255) R = (Ruby.Float) (-255);
                        else if (Args.Get<Ruby.Float>(3) > 255) R = (Ruby.Float) 255;
                        else Grey = Args[3];
                    }
                    else
                    {
                        Args[3].Expect(Ruby.Integer.Class);
                        if (Args.Get<Ruby.Integer>(3) < -255) Grey = (Ruby.Integer) (-255);
                        else if (Args.Get<Ruby.Integer>(3) > 255) Grey = (Ruby.Integer) 255;
                        else Grey = Args[3];
                    }
                }
                else Grey = (Ruby.Integer) 0;
            }
            Self.SetIVar("@red", R);
            Self.SetIVar("@green", G);
            Self.SetIVar("@blue", B);
            Self.SetIVar("@grey", Grey);
            return Self;
        }

        protected static Ruby.Object redget(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(0);
            return Self.GetIVar("@red");
        }

        protected static Ruby.Object redset(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(1);
            Ruby.Object R = (Ruby.Integer) 0;
            short realr = 0;
            if (Args[0].Is(Ruby.Float.Class))
            {
                if (Args.Get<Ruby.Float>(0) < -255) R = (Ruby.Float) (-255);
                else if (Args.Get<Ruby.Float>(0) > 255) R = (Ruby.Float) 255;
                else R = Args.Get<Ruby.Float>(0);
                realr = (short) Math.Round((Ruby.Float) R);
            }
            else
            {
                Args[0].Expect(Ruby.Integer.Class);
                if (Args.Get<Ruby.Integer>(0) < -255) R = (Ruby.Integer) (-255);
                else if (Args.Get<Ruby.Integer>(0) > 255) R = (Ruby.Integer) 255;
                else R = Args.Get<Ruby.Integer>(0);
                realr = (short) (Ruby.Integer) R;
            }
            if (Self.GetIVar("@__sprite__") != Ruby.Nil)
            {
                Sprite.SpriteDictionary[Self.RawGetIVar("@__sprite__")].Tone.Red = realr;
            }
            return Self.SetIVar("@red", R);
        }

        protected static Ruby.Object greenget(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(0);
            return Self.GetIVar("@green");
        }

        protected static Ruby.Object greenset(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(1);
            Ruby.Object G = (Ruby.Integer) 0;
            short realg = 0;
            if (Args[0].Is(Ruby.Float.Class))
            {
                if (Args.Get<Ruby.Float>(0) < -255) G = (Ruby.Float) (-255);
                else if (Args.Get<Ruby.Float>(0) > 255) G = (Ruby.Float) 255;
                else G = Args.Get<Ruby.Float>(0);
                realg = (short) Math.Round((Ruby.Float) G);
            }
            else
            {
                Args[0].Expect(Ruby.Integer.Class);
                if (Args.Get<Ruby.Integer>(0) < -255) G = (Ruby.Integer) (-255);
                else if (Args.Get<Ruby.Integer>(0) > 255) G = (Ruby.Integer) 255;
                else G = Args.Get<Ruby.Integer>(0);
                realg = (short) (Ruby.Integer) G;
            }
            if (Self.GetIVar("@__sprite__") != Ruby.Nil)
            {
                Sprite.SpriteDictionary[Self.RawGetIVar("@__sprite__")].Tone.Green = realg;
            }
            return Self.SetIVar("@green", G);
        }

        protected static Ruby.Object blueget(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(0);
            return Self.GetIVar("@blue");
        }

        protected static Ruby.Object blueset(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(1);
            Ruby.Object B = (Ruby.Integer) 0;
            short realb = 0;
            if (Args[0].Is(Ruby.Float.Class))
            {
                if (Args.Get<Ruby.Float>(0) < -255) B = (Ruby.Float) (-255);
                else if (Args.Get<Ruby.Float>(0) > 255) B = (Ruby.Float) 255;
                else B = Args.Get<Ruby.Float>(0);
                realb = (short) Math.Round((Ruby.Float) B);
            }
            else
            {
                Args[0].Expect(Ruby.Integer.Class);
                if (Args.Get<Ruby.Integer>(0) < -255) B = (Ruby.Integer) (-255);
                else if (Args.Get<Ruby.Integer>(0) > 255) B = (Ruby.Integer) 255;
                else B = Args.Get<Ruby.Integer>(0);
                realb = (short) (Ruby.Integer) B;
            }
            if (Self.GetIVar("@__sprite__") != Ruby.Nil)
            {
                Sprite.SpriteDictionary[Self.RawGetIVar("@__sprite__")].Tone.Blue = realb;
            }
            return Self.SetIVar("@blue", B);
        }

        protected static Ruby.Object greyget(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(0);
            return Self.GetIVar("@grey");
        }

        protected static Ruby.Object greyset(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(1);
            Ruby.Object Grey = (Ruby.Integer) 0;
            byte realgrey = 0;
            if (Args[0].Is(Ruby.Float.Class))
            {
                if (Args.Get<Ruby.Float>(0) < -255) Grey = (Ruby.Float) (-255);
                else if (Args.Get<Ruby.Float>(0) > 255) Grey = (Ruby.Float) 255;
                else Grey = Args.Get<Ruby.Float>(0);
                realgrey = (byte) Math.Round((Ruby.Float) Grey);
            }
            else
            {
                Args[0].Expect(Ruby.Integer.Class);
                if (Args.Get<Ruby.Integer>(0) < -255) Grey = (Ruby.Integer) (-255);
                else if (Args.Get<Ruby.Integer>(0) > 255) Grey = (Ruby.Integer) 255;
                else Grey = Args.Get<Ruby.Integer>(0);
                realgrey = (byte) (Ruby.Integer) Grey;
            }
            if (Self.GetIVar("@__sprite__") != Ruby.Nil)
            {
                Sprite.SpriteDictionary[Self.RawGetIVar("@__sprite__")].Tone.Gray = realgrey;
            }
            return Self.SetIVar("@grey", Grey);
        }
    }
}

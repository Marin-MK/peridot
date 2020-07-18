using System;
using System.Threading;
using rubydotnet;

namespace peridot
{
    public class Color : Ruby.Object
    {
        public new static string KlassName = "Color";
        public new static Ruby.Class Class;

        public Color(IntPtr Pointer) : base(Pointer) { }

        public static void Create()
        {
            Ruby.Class c = Ruby.Class.DefineClass<Color>(KlassName);
            Class = c;
            c.DefineClassMethod("_load", _load);
            c.DefineMethod("initialize", initialize);
            c.DefineMethod("red", redget);
            c.DefineMethod("red=", redset);
            c.DefineMethod("green", greenget);
            c.DefineMethod("green=", greenset);
            c.DefineMethod("blue", blueget);
            c.DefineMethod("blue=", blueset);
            c.DefineMethod("alpha", alphaget);
            c.DefineMethod("alpha=", alphaset);
        }

        public static odl.Color CreateColor(Ruby.Object Self)
        {
            byte R = 0,
                 G = 0,
                 B = 0,
                 A = 0;
            if (Self.GetIVar("@red").Is(Ruby.Float.Class))
            {
                R = (byte) Math.Round(Self.AutoGetIVar<Ruby.Float>("@red"));
            }
            else
            {
                R = (byte) Self.AutoGetIVar<Ruby.Integer>("@red");
            }
            if (Self.GetIVar("@green").Is(Ruby.Float.Class))
            {
                G = (byte) Math.Round(Self.AutoGetIVar<Ruby.Float>("@green"));
            }
            else
            {
                G = (byte) Self.AutoGetIVar<Ruby.Integer>("@green");
            }
            if (Self.GetIVar("@blue").Is(Ruby.Float.Class))
            {
                B = (byte) Math.Round(Self.AutoGetIVar<Ruby.Float>("@blue"));
            }
            else
            {
                B = (byte) Self.AutoGetIVar<Ruby.Integer>("@blue");
            }
            if (Self.GetIVar("@alpha").Is(Ruby.Float.Class))
            {
                A = (byte) Math.Round(Self.AutoGetIVar<Ruby.Float>("@alpha"));
            }
            else
            {
                A = (byte) Self.AutoGetIVar<Ruby.Integer>("@alpha");
            }
            return new odl.Color(R, G, B, A);
        }

        public static Color CreateColor(odl.Color Color)
        {
            return Class.AutoFuncall<Color>("new", 
                (Ruby.Integer) Color.Red,
                (Ruby.Integer) Color.Green,
                (Ruby.Integer) Color.Blue,
                (Ruby.Integer) Color.Alpha
            );
        }

        protected static Ruby.Object initialize(Ruby.Object Self, Ruby.Array Args)
        {
            //Args.Expect(3, 4);
            Ruby.Object R = (Ruby.Integer) 0,
                        G = (Ruby.Integer) 0,
                        B = (Ruby.Integer) 0,
                        A = (Ruby.Integer) 0;
            if (Args.Length == 3 || Args.Length == 4)
            {
                //if (Args[0].Is(Ruby.Float.Class))
                //{
                    /*if (Args.Get<Ruby.Float>(0) < 0) R = (Ruby.Float) 0;
                    else if (Args.Get<Ruby.Float>(0) > 255) R = (Ruby.Float) 255;
                    else R = Args[0];*/
                //}
                //else
                //{
                    /*Args[0].Expect(Ruby.Integer.Class);
                    if (Args.Get<Ruby.Integer>(0) < 0) R = (Ruby.Integer) 0;
                    else if (Args.Get<Ruby.Integer>(0) > 255) R = (Ruby.Integer) 255;
                    else*/ R = Args[0];
                //}
                //if (Args[1].Is(Ruby.Float.Class))
                //{
                    /*if (Args.Get<Ruby.Float>(1) < 0) G = (Ruby.Float) 0;
                    else if (Args.Get<Ruby.Float>(1) > 255) G = (Ruby.Float) 255;
                    else G = Args[1];*/
                //}
                //else
                //{
                    /*Args[1].Expect(Ruby.Integer.Class);
                    if (Args.Get<Ruby.Integer>(1) < 0) G = (Ruby.Integer) 0;
                    else if (Args.Get<Ruby.Integer>(1) > 255) G = (Ruby.Integer) 255;
                    else*/ G = Args[1];
                //}
                //if (Args[2].Is(Ruby.Float.Class))
                //{
                    /*if (Args.Get<Ruby.Float>(2) < 0) B = (Ruby.Float) 0;
                    else if (Args.Get<Ruby.Float>(2) > 255) B = (Ruby.Float) 255;
                    else B = Args[2];*/
                //}
                //else
                //{
                    /*Args[2].Expect(Ruby.Integer.Class);
                    if (Args.Get<Ruby.Integer>(2) < 0) B = (Ruby.Integer) 0;
                    else if (Args.Get<Ruby.Integer>(2) > 255) B = (Ruby.Integer) 255;
                    else*/ B = Args[2];
                //}
                if (Args.Length == 4)
                {
                    //if (Args[3].Is(Ruby.Float.Class))
                    //{
                        /*if (Args.Get<Ruby.Float>(3) < 0) A = (Ruby.Float) 0;
                        else if (Args.Get<Ruby.Float>(3) > 255) A = (Ruby.Float) 255;
                        else A = Args[3];*/
                    //}
                    //else
                    //{
                        /*Args[3].Expect(Ruby.Integer.Class);
                        if (Args.Get<Ruby.Integer>(3) < 0) A = (Ruby.Integer) 0;
                        else if (Args.Get<Ruby.Integer>(3) > 255) A = (Ruby.Integer) 255;
                        else*/ A = Args[3];
                    //}
                }
                else A = (Ruby.Integer) 255;
            }
            Self.SetIVar("@red", R);
            Self.SetIVar("@green", G);
            Self.SetIVar("@blue", B);
            Self.SetIVar("@alpha", A);
            return Self;
        }

        protected static Ruby.Object _load(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(1);
            
            Ruby.Array ary = Args[0].AutoFuncall<Ruby.Array>("unpack", (Ruby.String) "D*");
            ary.Expect(4);

            return Class.Funcall("new", ary[0], ary[2], ary[3], ary[4]);
        }

        protected static Ruby.Object redget(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(0);
            return Self.GetIVar("@red");
        }

        protected static Ruby.Object redset(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(1);
            Ruby.Object R = null;
            byte realr = 0;
            if (Args[0].Is(Ruby.Float.Class))
            {
                if (Args.Get<Ruby.Float>(0) < 0) R = (Ruby.Float) 0;
                else if (Args.Get<Ruby.Float>(0) > 255) R = (Ruby.Float) 255;
                else R = Args.Get<Ruby.Float>(0);
                realr = (byte) Math.Round((Ruby.Float) R);
            }
            else
            {
                Args[0].Expect(Ruby.Integer.Class);
                if (Args.Get<Ruby.Integer>(0) < 0) R = (Ruby.Integer) 0;
                else if (Args.Get<Ruby.Integer>(0) > 255) R = (Ruby.Integer) 255;
                else R = Args.Get<Ruby.Integer>(0);
                realr = (byte) (Ruby.Integer) R;
            }
            if (Self.GetIVar("@__viewport__") != Ruby.Nil)
            {
                Viewport.ViewportDictionary[Self.RawGetIVar("@__viewport__")].Color.Red = realr;
            }
            if (Self.GetIVar("@__sprite__") != Ruby.Nil)
            {
                Sprite.SpriteDictionary[Self.RawGetIVar("@__sprite__")].Color.Red = realr;
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
            Ruby.Object G = null;
            byte realg = 0;
            if (Args[0].Is(Ruby.Float.Class))
            {
                if (Args.Get<Ruby.Float>(0) < 0) G = (Ruby.Float) 0;
                else if (Args.Get<Ruby.Float>(0) > 255) G = (Ruby.Float) 255;
                else G = Args.Get<Ruby.Float>(0);
                realg = (byte) Math.Round((Ruby.Float) G);
            }
            else
            {
                Args[0].Expect(Ruby.Integer.Class);
                if (Args.Get<Ruby.Integer>(0) < 0) G = (Ruby.Integer) 0;
                else if (Args.Get<Ruby.Integer>(0) > 255) G = (Ruby.Integer) 255;
                else G = Args.Get<Ruby.Integer>(0);
                realg = (byte) (Ruby.Integer) G;
            }
            if (Self.GetIVar("@__viewport__") != Ruby.Nil)
            {
                Viewport.ViewportDictionary[Self.RawGetIVar("@__viewport__")].Color.Green = realg;
            }
            if (Self.GetIVar("@__sprite__") != Ruby.Nil)
            {
                Sprite.SpriteDictionary[Self.RawGetIVar("@__sprite__")].Color.Green = realg;
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
            Ruby.Object B = null;
            byte realb = 0;
            if (Args[0].Is(Ruby.Float.Class))
            {
                if (Args.Get<Ruby.Float>(0) < 0) B = (Ruby.Float) 0;
                else if (Args.Get<Ruby.Float>(0) > 255) B = (Ruby.Float) 255;
                else B = Args.Get<Ruby.Float>(0);
                realb = (byte) Math.Round((Ruby.Float) B);
            }
            else
            {
                Args[0].Expect(Ruby.Integer.Class);
                if (Args.Get<Ruby.Integer>(0) < 0) B = (Ruby.Integer) 0;
                else if (Args.Get<Ruby.Integer>(0) > 255) B = (Ruby.Integer) 255;
                else B = Args.Get<Ruby.Integer>(0);
                realb = (byte) (Ruby.Integer) B;
            }
            if (Self.GetIVar("@__viewport__") != Ruby.Nil)
            {
                Viewport.ViewportDictionary[Self.RawGetIVar("@__viewport__")].Color.Blue = realb;
            }
            if (Self.GetIVar("@__sprite__") != Ruby.Nil)
            {
                Sprite.SpriteDictionary[Self.RawGetIVar("@__sprite__")].Color.Blue = realb;
            }
            return Self.SetIVar("@blue", B);
        }

        protected static Ruby.Object alphaget(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(0);
            return Self.GetIVar("@alpha");
        }

        protected static Ruby.Object alphaset(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(1);
            Ruby.Object A = null;
            byte reala = 0;
            if (Args[0].Is(Ruby.Float.Class))
            {
                if (Args.Get<Ruby.Float>(0) < 0) A = (Ruby.Float) 0;
                else if (Args.Get<Ruby.Float>(0) > 255) A = (Ruby.Float) 255;
                else A = Args.Get<Ruby.Float>(0);
                reala = (byte) Math.Round((Ruby.Float) A);
            }
            else
            {
                Args[0].Expect(Ruby.Integer.Class);
                if (Args.Get<Ruby.Integer>(0) < 0) A = (Ruby.Integer) 0;
                else if (Args.Get<Ruby.Integer>(0) > 255) A = (Ruby.Integer) 255;
                else A = Args.Get<Ruby.Integer>(0);
                reala = (byte) (Ruby.Integer) A;
            }
            if (Self.GetIVar("@__viewport__") != Ruby.Nil)
            {
                Viewport.ViewportDictionary[Self.RawGetIVar("@__viewport__")].Color.Alpha = reala;
            }
            if (Self.GetIVar("@__sprite__") != Ruby.Nil)
            {
                Sprite.SpriteDictionary[Self.RawGetIVar("@__sprite__")].Color.Alpha = reala;
            }
            return Self.SetIVar("@alpha", A);
        }
    }
}

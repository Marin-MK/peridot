using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using rubydotnet;

namespace peridot
{
    public class Viewport : Ruby.Object
    {
        public new static string KlassName = "Viewport";
        public new static Ruby.Class Class;

        public Viewport(IntPtr Pointer) : base(Pointer) { }

        public static Dictionary<IntPtr, odl.Viewport> ViewportDictionary = new Dictionary<IntPtr, odl.Viewport>();

        public static void Create()
        {
            Ruby.Class c = Ruby.Class.DefineClass<Viewport>(KlassName);
            Class = c;
            c.DefineMethod("initialize", initialize);
            c.DefineMethod("rect", rectget);
            c.DefineMethod("rect=", rectset);
            c.DefineMethod("x", xget);
            c.DefineMethod("x=", xset);
            c.DefineMethod("y", yget);
            c.DefineMethod("y=", yset);
            c.DefineMethod("height", heightget);
            c.DefineMethod("height=", heightset);
            c.DefineMethod("width", widthget);
            c.DefineMethod("width=", widthset);
            c.DefineMethod("z", zget);
            c.DefineMethod("z=", zset);
            c.DefineMethod("color", colorget);
            c.DefineMethod("color=", colorset);
            c.DefineMethod("ox", oxget);
            c.DefineMethod("ox=", oxset);
            c.DefineMethod("oy", oyget);
            c.DefineMethod("oy=", oyset);
            c.DefineMethod("dispose", dispose);
            c.DefineMethod("disposed?", disposed);
        }

        protected static Ruby.Object initialize(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(1, 4);
            Rect rect = null;
            int x = 0,
                y = 0,
                w = 0,
                h = 0;
            if (Args.Length == 1)
            {
                Args[0].Expect(Rect.Class);
                if (Args[0].GetIVar("@x").Is(Ruby.Float.Class)) x = (int) Math.Round(Args[0].AutoGetIVar<Ruby.Float>("@x"));
                else x = Args[0].AutoGetIVar<Ruby.Integer>("@x");
                if (Args[0].GetIVar("@y").Is(Ruby.Float.Class)) y = (int) Math.Round(Args[0].AutoGetIVar<Ruby.Float>("@y"));
                else y = Args[0].AutoGetIVar<Ruby.Integer>("@y");
                if (Args[0].GetIVar("@width").Is(Ruby.Float.Class)) w = (int) Math.Round(Args[0].AutoGetIVar<Ruby.Float>("@width"));
                else w = Args[0].AutoGetIVar<Ruby.Integer>("@width");
                if (Args[0].GetIVar("@height").Is(Ruby.Float.Class)) h = (int) Math.Round(Args[0].AutoGetIVar<Ruby.Float>("@height"));
                else h = Args[0].AutoGetIVar<Ruby.Integer>("@height");
                rect = Args.Get<Rect>(0);
                Self.SetIVar("@rect", rect);
            }
            else if (Args.Length == 4)
            {
                if (Args[0].Is(Ruby.Float.Class)) x = (int) Math.Round(Args.Get<Ruby.Float>(0));
                else
                {
                    Args[0].Expect(Ruby.Integer.Class);
                    x = Args.Get<Ruby.Integer>(0);
                }
                if (Args[1].Is(Ruby.Float.Class)) y = (int) Math.Round(Args.Get<Ruby.Float>(1));
                else
                {
                    Args[1].Expect(Ruby.Integer.Class);
                    y = Args.Get<Ruby.Integer>(1);
                }
                if (Args[2].Is(Ruby.Float.Class)) w = (int) Math.Round(Args.Get<Ruby.Float>(2));
                else
                {
                    Args[2].Expect(Ruby.Integer.Class);
                    w = Args.Get<Ruby.Integer>(2);
                }
                if (Args[3].Is(Ruby.Float.Class)) h = (int) Math.Round(Args.Get<Ruby.Float>(3));
                else
                {
                    Args[3].Expect(Ruby.Integer.Class);
                    h = Args.Get<Ruby.Integer>(3);
                }
                rect = Rect.CreateRect(new odl.Rect(0, 0, 0, 0));
                Self.SetIVar("@rect", rect);
                Self.GetIVar("@rect").SetIVar("@x", Args[0]);
                Self.GetIVar("@rect").SetIVar("@y", Args[1]);
                Self.GetIVar("@rect").SetIVar("@width", Args[2]);
                Self.GetIVar("@rect").SetIVar("@height", Args[3]);
            }
            rect.SetIVar("@__viewport__", Self);
            Self.SetIVar("@ox", (Ruby.Integer) 0);
            Self.SetIVar("@oy", (Ruby.Integer) 0);
            Self.SetIVar("@z", (Ruby.Integer) 0);
            Color color = Color.CreateColor(odl.Color.ALPHA);
            color.SetIVar("@__viewport__", Self);
            Self.SetIVar("@color", color);
            Self.SetIVar("@disposed", Ruby.False);

            odl.Viewport vp = new odl.Viewport(x, y, w, h);

            if (ViewportDictionary.ContainsKey(Self.Pointer))
            {
                ViewportDictionary[Self.Pointer].Dispose();
                ViewportDictionary.Remove(Self.Pointer);
            }
            ViewportDictionary.Add(Self.Pointer, vp);
            return Self;
        }

        protected static Ruby.Object rectget(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            return Self.GetIVar("@rect");
        }

        protected static Ruby.Object rectset(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(1);
            Args[0].Expect(Rect.Class);
            Self.GetIVar("@rect").SetIVar("@__viewport__", Ruby.Nil);
            int x = 0,
                y = 0,
                w = 0,
                h = 0;
            if (Args[0].GetIVar("@x").Is(Ruby.Float.Class)) x = (int) Math.Round(Args[0].AutoGetIVar<Ruby.Float>("@x"));
            else x = Args[0].AutoGetIVar<Ruby.Integer>("@x");
            if (Args[0].GetIVar("@y").Is(Ruby.Float.Class)) y = (int) Math.Round(Args[0].AutoGetIVar<Ruby.Float>("@y"));
            else y = Args[0].AutoGetIVar<Ruby.Integer>("@y");
            if (Args[0].GetIVar("@width").Is(Ruby.Float.Class)) w = (int) Math.Round(Args[0].AutoGetIVar<Ruby.Float>("@width"));
            else w = Args[0].AutoGetIVar<Ruby.Integer>("@width");
            if (Args[0].GetIVar("@height").Is(Ruby.Float.Class)) h = (int) Math.Round(Args[0].AutoGetIVar<Ruby.Float>("@height"));
            else h = Args[0].AutoGetIVar<Ruby.Integer>("@height");
            ViewportDictionary[Self.Pointer].X = x;
            ViewportDictionary[Self.Pointer].Y = y;
            ViewportDictionary[Self.Pointer].Width = w;
            ViewportDictionary[Self.Pointer].Height = h;
            Args[0].SetIVar("@__viewport__", Self);
            return Self.SetIVar("@rect", Args[0]);
        }

        protected static Ruby.Object xget(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            return Self.GetIVar("@rect").GetIVar("@x");
        }

        protected static Ruby.Object xset(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(1);
            if (Args[0].Is(Ruby.Float.Class))
            {
                ViewportDictionary[Self.Pointer].X = (int) Math.Round(Args.Get<Ruby.Float>(0));
            }
            else
            {
                Args[0].Expect(Ruby.Integer.Class);
                ViewportDictionary[Self.Pointer].X = Args.Get<Ruby.Integer>(0);
            }
            return Self.GetIVar("@rect").SetIVar("@x", Args[0]);
        }

        protected static Ruby.Object yget(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            return Self.GetIVar("@rect").GetIVar("@y");
        }

        protected static Ruby.Object yset(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(1);
            if (Args[0].Is(Ruby.Float.Class))
            {
                ViewportDictionary[Self.Pointer].Y = (int) Math.Round(Args.Get<Ruby.Float>(0));
            }
            else
            {
                Args[0].Expect(Ruby.Integer.Class);
                ViewportDictionary[Self.Pointer].Y = Args.Get<Ruby.Integer>(0);
            }
            return Self.GetIVar("@rect").SetIVar("@y", Args[0]);
        }

        protected static Ruby.Object widthget(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            return Self.GetIVar("@rect").GetIVar("@width");
        }

        protected static Ruby.Object widthset(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(1);
            if (Args[0].Is(Ruby.Float.Class))
            {
                ViewportDictionary[Self.Pointer].Width = (int) Math.Round(Args.Get<Ruby.Float>(0));
            }
            else
            {
                Args[0].Expect(Ruby.Integer.Class);
                ViewportDictionary[Self.Pointer].Width = Args.Get<Ruby.Integer>(0);
            }
            return Self.GetIVar("@rect").SetIVar("@width", Args[0]);
        }

        protected static Ruby.Object heightget(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            return Self.GetIVar("@rect").GetIVar("@height");
        }

        protected static Ruby.Object heightset(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(1);
            if (Args[0].Is(Ruby.Float.Class))
            {
                ViewportDictionary[Self.Pointer].Height = (int) Math.Round(Args.Get<Ruby.Float>(0));
            }
            else
            {
                Args[0].Expect(Ruby.Integer.Class);
                ViewportDictionary[Self.Pointer].Height = Args.Get<Ruby.Integer>(0);
            }
            return Self.GetIVar("@rect").SetIVar("@height", Args[0]);
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
                ViewportDictionary[Self.Pointer].Z = (int) Math.Round(Args.Get<Ruby.Float>(0));
            }
            else
            {
                Args[0].Expect(Ruby.Integer.Class);
                ViewportDictionary[Self.Pointer].Z = Args.Get<Ruby.Integer>(0);
            }
            return Self.SetIVar("@z", Args[0]);
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
            Self.GetIVar("@color").SetIVar("@__viewport__", Ruby.Nil);
            ViewportDictionary[Self.Pointer].Color = Color.CreateColor(Args[0]);
            Args[0].SetIVar("@__viewport__", Self);
            return Self.SetIVar("@color", Args[0]);
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
                ViewportDictionary[Self.Pointer].OX = (int) Math.Round(Args.Get<Ruby.Float>(0));
            }
            else
            {
                Args[0].Expect(Ruby.Integer.Class);
                ViewportDictionary[Self.Pointer].OX = Args.Get<Ruby.Integer>(0);
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
                ViewportDictionary[Self.Pointer].OY = (int) Math.Round(Args.Get<Ruby.Float>(0));
            }
            else
            {
                Args[0].Expect(Ruby.Integer.Class);
                ViewportDictionary[Self.Pointer].OY = Args.Get<Ruby.Integer>(0);
            }
            return Self.SetIVar("@oy", Args[0]);
        }

        protected static Ruby.Object dispose(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            ViewportDictionary[Self.Pointer].Dispose();
            ViewportDictionary.Remove(Self.Pointer);
            Self.GetIVar("@rect").SetIVar("@__viewport__", Ruby.Nil);
            Self.GetIVar("@color").SetIVar("@__viewport__", Ruby.Nil);
            return Self.SetIVar("@disposed", Ruby.True);
        }

        protected static Ruby.Object disposed(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(0);
            return Self.GetIVar("@disposed");
        }

        protected static void GuardDisposed(Ruby.Object Self)
        {
            if (Self.GetIVar("@disposed") == Ruby.True)
            {
                Ruby.Raise(Ruby.ErrorType.RuntimeError, "viewport already disposed");
            }
        }
    }
}

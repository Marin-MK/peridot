using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using rubydotnet;

namespace peridot
{
    public static class Viewport
    {
        public static IntPtr Class;

        public static Dictionary<IntPtr, odl.Viewport> ViewportDictionary = new Dictionary<IntPtr, odl.Viewport>();

        public static void Create()
        {
            Class = Ruby.Class.Define("Viewport");
            Ruby.Class.DefineMethod(Class, "initialize", initialize);
            Ruby.Class.DefineMethod(Class, "rect", rectget);
            Ruby.Class.DefineMethod(Class, "rect=", rectset);
            Ruby.Class.DefineMethod(Class, "x", xget);
            Ruby.Class.DefineMethod(Class, "x=", xset);
            Ruby.Class.DefineMethod(Class, "y", yget);
            Ruby.Class.DefineMethod(Class, "y=", yset);
            Ruby.Class.DefineMethod(Class, "height", heightget);
            Ruby.Class.DefineMethod(Class, "height=", heightset);
            Ruby.Class.DefineMethod(Class, "width", widthget);
            Ruby.Class.DefineMethod(Class, "width=", widthset);
            Ruby.Class.DefineMethod(Class, "z", zget);
            Ruby.Class.DefineMethod(Class, "z=", zset);
            Ruby.Class.DefineMethod(Class, "color", colorget);
            Ruby.Class.DefineMethod(Class, "color=", colorset);
            Ruby.Class.DefineMethod(Class, "ox", oxget);
            Ruby.Class.DefineMethod(Class, "ox=", oxset);
            Ruby.Class.DefineMethod(Class, "oy", oyget);
            Ruby.Class.DefineMethod(Class, "oy=", oyset);
            Ruby.Class.DefineMethod(Class, "dispose", dispose);
            Ruby.Class.DefineMethod(Class, "disposed?", disposed);
        }

        static IntPtr initialize(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1, 4);
            IntPtr rect = IntPtr.Zero;
            int x = 0,
                y = 0,
                w = 0,
                h = 0;
            long len = Ruby.Array.Length(Args);
            if (len == 1)
            {
                Ruby.Array.Expect(Args, 0, "Rect");
                if (Ruby.IVarIs(Ruby.Array.Get(Args, 0), "@x", "Float")) x = Ruby.Float.RoundFromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@x"));
                else x = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@x"));
                if (Ruby.IVarIs(Ruby.Array.Get(Args, 0), "@y", "Float")) y = Ruby.Float.RoundFromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@y"));
                else y = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@y"));
                if (Ruby.IVarIs(Ruby.Array.Get(Args, 0), "@width", "Float")) w = Ruby.Float.RoundFromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@width"));
                else w = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@width"));
                if (Ruby.IVarIs(Ruby.Array.Get(Args, 0), "@height", "Float")) h = Ruby.Float.RoundFromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@height"));
                else h = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@height"));
                rect = Ruby.Array.Get(Args, 0);
                Ruby.SetIVar(Self, "@rect", rect);
            }
            else if (len == 4)
            {
                if (Ruby.Array.Is(Args, 0, "Float")) x = Ruby.Float.RoundFromPtr(Ruby.Array.Get(Args, 0));
                else
                {
                    Ruby.Array.Expect(Args, 0, "Integer");
                    x = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
                }
                if (Ruby.Array.Is(Args, 1, "Float")) y = Ruby.Float.RoundFromPtr(Ruby.Array.Get(Args, 1));
                else
                {
                    Ruby.Array.Expect(Args, 1, "Integer");
                    y = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 1));
                }
                if (Ruby.Array.Is(Args, 2, "Float")) w = Ruby.Float.RoundFromPtr(Ruby.Array.Get(Args, 2));
                else
                {
                    Ruby.Array.Expect(Args, 2, "Integer");
                    w = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 2));
                }
                if (Ruby.Array.Is(Args, 3, "Float")) h = Ruby.Float.RoundFromPtr(Ruby.Array.Get(Args, 3));
                else
                {
                    Ruby.Array.Expect(Args, 3, "Integer");
                    h = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 3));
                }
                rect = Rect.CreateRect(new odl.Rect(0, 0, 0, 0));
                Ruby.SetIVar(Self, "@rect", rect);
                Ruby.SetIVar(Ruby.GetIVar(Self, "@rect"), "@x", Ruby.Array.Get(Args, 0));
                Ruby.SetIVar(Ruby.GetIVar(Self, "@rect"), "@y", Ruby.Array.Get(Args, 1));
                Ruby.SetIVar(Ruby.GetIVar(Self, "@rect"), "@width", Ruby.Array.Get(Args, 2));
                Ruby.SetIVar(Ruby.GetIVar(Self, "@rect"), "@height", Ruby.Array.Get(Args, 3));
            }
            Ruby.SetIVar(rect, "@__viewport__", Self);
            Ruby.SetIVar(Self, "@ox", Ruby.Integer.ToPtr(0));
            Ruby.SetIVar(Self, "@oy", Ruby.Integer.ToPtr(0));
            Ruby.SetIVar(Self, "@z", Ruby.Integer.ToPtr(0));
            IntPtr color = Color.CreateColor(odl.Color.ALPHA);
            Ruby.SetIVar(color, "@__viewport__", Self);
            Ruby.SetIVar(Self, "@color", color);
            Ruby.SetIVar(Self, "@disposed", Ruby.False);

            odl.Viewport vp = new odl.Viewport(x, y, w, h);

            if (ViewportDictionary.ContainsKey(Self))
            {
                ViewportDictionary[Self].Dispose();
                ViewportDictionary.Remove(Self);
            }
            ViewportDictionary.Add(Self, vp);
            return Self;
        }

        static IntPtr rectget(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@rect");
        }

        static IntPtr rectset(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "Rect");
            Ruby.SetIVar(Ruby.GetIVar(Self,"@rect"), "@__viewport__", Ruby.Nil);
            int x = 0,
                y = 0,
                w = 0,
                h = 0;
            if (Ruby.IVarIs(Ruby.Array.Get(Args, 0), "@x", "Float")) x = Ruby.Float.RoundFromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@x"));
            else x = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@x"));
            if (Ruby.IVarIs(Ruby.Array.Get(Args, 0), "@y", "Float")) y = Ruby.Float.RoundFromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@y"));
            else y = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@x"));
            if (Ruby.IVarIs(Ruby.Array.Get(Args, 0), "@width", "Float")) w = Ruby.Float.RoundFromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@width"));
            else w = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@x"));
            if (Ruby.IVarIs(Ruby.Array.Get(Args, 0), "@height", "Float")) h = Ruby.Float.RoundFromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@height"));
            else h = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@x"));
            ViewportDictionary[Self].X = x;
            ViewportDictionary[Self].Y = y;
            ViewportDictionary[Self].Width = w;
            ViewportDictionary[Self].Height = h;
            Ruby.SetIVar(Ruby.Array.Get(Args, 0), "@__viewport__", Self);
            return Ruby.SetIVar(Self, "@rect", Ruby.Array.Get(Args, 0));
        }

        static IntPtr xget(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Ruby.GetIVar(Self, "@rect"), "@x");
        }

        static IntPtr xset(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 1);
            if (Ruby.Array.Is(Args, 0, "Float"))
            {
                ViewportDictionary[Self].X = Ruby.Float.RoundFromPtr(Ruby.Array.Get(Args, 0));
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                ViewportDictionary[Self].X = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
            }
            return Ruby.SetIVar(Ruby.GetIVar(Self, "@rect"), "@x", Ruby.Array.Get(Args, 0));
        }

        static IntPtr yget(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Ruby.GetIVar(Self, "@rect"), "@y");
        }

        static IntPtr yset(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 1);
            if (Ruby.Array.Is(Args, 0, "Float"))
            {
                ViewportDictionary[Self].Y = Ruby.Float.RoundFromPtr(Ruby.Array.Get(Args, 0));
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                ViewportDictionary[Self].Y = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
            }
            return Ruby.SetIVar(Ruby.GetIVar(Self, "@rect"), "@y", Ruby.Array.Get(Args, 0));
        }

        static IntPtr widthget(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Ruby.GetIVar(Self, "@rect"), "@width");
        }

        static IntPtr widthset(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 1);
            if (Ruby.Array.Is(Args, 0, "Float"))
            {
                ViewportDictionary[Self].Width = Ruby.Float.RoundFromPtr(Ruby.Array.Get(Args, 0));
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                ViewportDictionary[Self].Width = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
            }
            return Ruby.SetIVar(Ruby.GetIVar(Self, "@rect"), "@width", Ruby.Array.Get(Args, 0));
        }

        static IntPtr heightget(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Ruby.GetIVar(Self, "@rect"), "@height");
        }

        static IntPtr heightset(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 1);
            if (Ruby.Array.Is(Args, 0, "Float"))
            {
                ViewportDictionary[Self].Height = Ruby.Float.RoundFromPtr(Ruby.Array.Get(Args, 0));
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                ViewportDictionary[Self].Height = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
            }
            return Ruby.SetIVar(Ruby.GetIVar(Self, "@rect"), "@height", Ruby.Array.Get(Args, 0));
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
                ViewportDictionary[Self].Z = Ruby.Float.RoundFromPtr(Ruby.Array.Get(Args, 0));
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                ViewportDictionary[Self].Z = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
            }
            return Ruby.SetIVar(Self, "@z", Ruby.Array.Get(Args, 0));
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
            Ruby.SetIVar(Ruby.GetIVar(Self, "@color"), "@__viewport__", Ruby.Nil);
            ViewportDictionary[Self].Color = Color.CreateColor(Ruby.Array.Get(Args, 0));
            Ruby.SetIVar(Ruby.Array.Get(Args, 0), "@__viewport__", Self);
            return Ruby.SetIVar(Self, "@color", Ruby.Array.Get(Args, 0));
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
                ViewportDictionary[Self].OX = Ruby.Float.RoundFromPtr(Ruby.Array.Get(Args, 0));
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                ViewportDictionary[Self].OX = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
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
                ViewportDictionary[Self].OY = Ruby.Float.RoundFromPtr(Ruby.Array.Get(Args, 0));
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                ViewportDictionary[Self].OY = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
            }
            return Ruby.SetIVar(Self, "@oy", Ruby.Array.Get(Args, 0));
        }

        static IntPtr dispose(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            ViewportDictionary[Self].Dispose();
            ViewportDictionary.Remove(Self);
            Ruby.SetIVar(Ruby.GetIVar(Self, "@rect"), "@__viewport__", Ruby.Nil);
            Ruby.SetIVar(Ruby.GetIVar(Self, "@color"), "@__viewport__", Ruby.Nil);
            return Ruby.SetIVar(Self, "@disposed", Ruby.True);
        }

        static IntPtr disposed(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@disposed");
        }

        static void GuardDisposed(IntPtr Self)
        {
            if (Ruby.GetIVar(Self, "@disposed") == Ruby.True)
            {
                Ruby.Raise(Ruby.ErrorType.RuntimeError, "viewport already disposed");
            }
        }
    }
}

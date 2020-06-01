using System;
using System.Collections.Generic;
using System.Text;
using RubyDotNET;

namespace Peridot
{
    public class Viewport : RubyObject
    {
        public static IntPtr Class;
        public static Dictionary<IntPtr, ODL.Viewport> ViewportDictionary = new Dictionary<IntPtr, ODL.Viewport>();

        public static Class CreateClass()
        {
            Class c = new Class("Viewport");
            Class = c.Pointer;
            c.DefineClassMethod("new", _new);
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
            return c;
        }

        protected static IntPtr allocate(IntPtr Class)
        {
            return Internal.rb_funcallv(Class, Internal.rb_intern("allocate"), 0);
        }

        protected static IntPtr _new(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            IntPtr obj = allocate(self);
            Internal.rb_funcallv(obj, Internal.rb_intern("initialize"), Args.Length, Args.Rubify());
            return obj;
        }

        protected static IntPtr initialize(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            IntPtr rect = IntPtr.Zero;
            int x = 0,
                y = 0,
                w = 0,
                h = 0;
            if (Args.Length == 1)
            {
                rect = Args[0].Pointer;
                x = (int) Internal.NUM2LONG(Internal.GetIVar(Args[0].Pointer, "@x"));
                y = (int) Internal.NUM2LONG(Internal.GetIVar(Args[0].Pointer, "@y"));
                w = (int) Internal.NUM2LONG(Internal.GetIVar(Args[0].Pointer, "@width"));
                h = (int) Internal.NUM2LONG(Internal.GetIVar(Args[0].Pointer, "@height"));
                Internal.SetIVar(self, "@rect", rect);
            }
            else if (Args.Length == 4)
            {
                x = (int) Internal.NUM2LONG(Args[0].Pointer);
                y = (int) Internal.NUM2LONG(Args[1].Pointer);
                w = (int) Internal.NUM2LONG(Args[2].Pointer);
                h = (int) Internal.NUM2LONG(Args[3].Pointer);
                Internal.SetIVar(self, "@rect", Rect.CreateRect(new ODL.Rect(x, y, w, h)));
            }
            else ScanArgs(4, Args);
            Internal.SetIVar(self, "@ox", Internal.LONG2NUM(0));
            Internal.SetIVar(self, "@oy", Internal.LONG2NUM(0));
            Internal.SetIVar(self, "@z", Internal.LONG2NUM(0));
            IntPtr color = Color.CreateColor(ODL.Color.ALPHA);
            Internal.SetIVar(self, "@color", color);
            Internal.SetIVar(color, "@__viewport__", self);

            ODL.Viewport vp = new ODL.Viewport(x, y, w, h);

            if (ViewportDictionary.ContainsKey(self)) dispose(self, IntPtr.Zero);
            ViewportDictionary.Add(self, vp);
            return self;
        }

        protected static IntPtr rectget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@rect");
        }

        protected static IntPtr rectset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            ViewportDictionary[self].X = (int) Internal.NUM2LONG(Internal.GetIVar(Args[0].Pointer, "@x"));
            ViewportDictionary[self].Y = (int) Internal.NUM2LONG(Internal.GetIVar(Args[0].Pointer, "@y"));
            ViewportDictionary[self].Width = (int) Internal.NUM2LONG(Internal.GetIVar(Args[0].Pointer, "@width"));
            ViewportDictionary[self].Height = (int) Internal.NUM2LONG(Internal.GetIVar(Args[0].Pointer, "@height"));
            return Internal.SetIVar(self, "@rect", Args[0].Pointer);
        }

        protected static IntPtr xget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(Internal.GetIVar(self, "@rect"), "@x");
        }

        protected static IntPtr xset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            ViewportDictionary[self].X = (int) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.SetIVar(Internal.GetIVar(self, "@rect"), "@x", Args[0].Pointer);
        }

        protected static IntPtr yget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(Internal.GetIVar(self, "@rect"), "@y");
        }

        protected static IntPtr yset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            ViewportDictionary[self].Y = (int) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.SetIVar(Internal.GetIVar(self, "@rect"), "@y", Args[0].Pointer);
        }

        protected static IntPtr widthget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(Internal.GetIVar(self, "@rect"), "@width");
        }

        protected static IntPtr widthset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            ViewportDictionary[self].Width = (int) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.SetIVar(Internal.GetIVar(self, "@rect"), "@width", Args[0].Pointer);
        }

        protected static IntPtr heightget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(Internal.GetIVar(self, "@rect"), "@height");
        }

        protected static IntPtr heightset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            ViewportDictionary[self].Height = (int) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.SetIVar(Internal.GetIVar(self, "@rect"), "@height", Args[0].Pointer);
        }

        protected static IntPtr zget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@z");
        }

        protected static IntPtr zset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            ViewportDictionary[self].Z = (int) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.SetIVar(self, "@z", Args[0].Pointer);
        }

        protected static IntPtr colorget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@color");
        }

        protected static IntPtr colorset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            ViewportDictionary[self].Color = Color.CreateColor(Args[0].Pointer);
            Internal.SetIVar(Args[0].Pointer, "@__viewport__", self);
            return Internal.SetIVar(self, "@color", Args[0].Pointer);
        }

        protected static IntPtr oxget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@ox");
        }

        protected static IntPtr oxset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            ViewportDictionary[self].OX = (int) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.SetIVar(self, "@ox", Args[0].Pointer);
        }

        protected static IntPtr oyget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@oy");
        }

        protected static IntPtr oyset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            ViewportDictionary[self].OY = (int) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.SetIVar(self, "@oy", Args[0].Pointer);
        }

        protected static IntPtr dispose(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            if (_args != IntPtr.Zero)
            {
                RubyArray Args = new RubyArray(_args);
                ScanArgs(0, Args);
            }
            ViewportDictionary[self].Dispose();
            ViewportDictionary.Remove(self);
            Internal.SetIVar(self, "@disposed", Internal.QTrue);
            return Internal.QTrue;
        }

        protected static IntPtr disposed(IntPtr self, IntPtr _args)
        {
            if (_args != IntPtr.Zero)
            {
                RubyArray Args = new RubyArray(_args);
                ScanArgs(0, Args);
            }
            return Internal.GetIVar(self, "@disposed") == Internal.QTrue ? Internal.QTrue : Internal.QFalse;
        }

        protected static void GuardDisposed(IntPtr self)
        {
            if (disposed(self, IntPtr.Zero) == Internal.QTrue)
            {
                Internal.rb_raise(Internal.rb_eRuntimeError.Pointer, "viewport already disposed");
            }
        }
    }
}

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
            ScanArgs(4, Args);
            Internal.SetIVar(self, "@x", Args[0].Pointer);
            Internal.SetIVar(self, "@y", Args[1].Pointer);
            Internal.SetIVar(self, "@width", Args[2].Pointer);
            Internal.SetIVar(self, "@height", Args[3].Pointer);
            Internal.SetIVar(self, "@z", Internal.LONG2NUM(0));

            ODL.Viewport vp = new ODL.Viewport(
                (int) Internal.NUM2LONG(Args[0].Pointer),
                (int) Internal.NUM2LONG(Args[1].Pointer),
                (int) Internal.NUM2LONG(Args[2].Pointer),
                (int) Internal.NUM2LONG(Args[3].Pointer)
            );

            if (ViewportDictionary.ContainsKey(self)) dispose(self, IntPtr.Zero);
            ViewportDictionary.Add(self, vp);
            return self;
        }

        protected static IntPtr xget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@x");
        }

        protected static IntPtr xset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            ViewportDictionary[self].X = (int) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.SetIVar(self, "@x", Args[0].Pointer);
        }

        protected static IntPtr yget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.rb_ivar_get(self, Internal.rb_intern("@y"));
        }

        protected static IntPtr yset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            ViewportDictionary[self].Y = (int) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.SetIVar(self, "@y", Args[0].Pointer);
        }

        protected static IntPtr widthget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@width");
        }

        protected static IntPtr widthset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            ViewportDictionary[self].Width = (int) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.SetIVar(self, "@width", Args[0].Pointer);
        }

        protected static IntPtr heightget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@height");
        }

        protected static IntPtr heightset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            ViewportDictionary[self].Height = (int) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.SetIVar(self, "@height", Args[0].Pointer);
        }

        protected static IntPtr zget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.rb_ivar_get(self, Internal.rb_intern("@z"));
        }

        protected static IntPtr zset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            ViewportDictionary[self].Z = (int) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.SetIVar(self, "@z", Args[0].Pointer);
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

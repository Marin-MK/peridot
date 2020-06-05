using System;
using System.Collections.Generic;
using System.Text;
using RubyDotNET;

namespace Peridot
{
    public class Color : RubyObject
    {
        public static IntPtr Class;

        public static Class CreateClass()
        {
            Class c = new Class("Color");
            Class = c.Pointer;
            c.DefineClassMethod("new", _new);
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
            return c;
        }

        public static odl.Color CreateColor(IntPtr self)
        {
            return new odl.Color(
                (byte) Internal.NUM2LONG(Internal.GetIVar(self, "@red")),
                (byte) Internal.NUM2LONG(Internal.GetIVar(self, "@green")),
                (byte) Internal.NUM2LONG(Internal.GetIVar(self, "@blue")),
                (byte) Internal.NUM2LONG(Internal.GetIVar(self, "@alpha"))
            );
        }

        public static IntPtr CreateColor(odl.Color Color)
        {
            return Internal.rb_funcallv(Class, Internal.rb_intern("new"), 4, new IntPtr[4]
            {
                Internal.LONG2NUM(Color.Red),
                Internal.LONG2NUM(Color.Green),
                Internal.LONG2NUM(Color.Blue),
                Internal.LONG2NUM(Color.Alpha)
            });
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
            IntPtr R = IntPtr.Zero,
                   G = IntPtr.Zero,
                   B = IntPtr.Zero,
                   A = IntPtr.Zero;
            if (Args.Length == 3 || Args.Length == 4)
            {
                R = Args[0].Pointer;
                G = Args[1].Pointer;
                B = Args[2].Pointer;
                if (Args.Length == 4) A = Args[3].Pointer;
                else A = Internal.LONG2NUM(255);
            }
            else ScanArgs(3, Args);
            Internal.SetIVar(self, "@red", R);
            Internal.SetIVar(self, "@green", G);
            Internal.SetIVar(self, "@blue", B);
            Internal.SetIVar(self, "@alpha", A);
            return self;
        }

        protected static IntPtr _load(IntPtr _self, IntPtr _args)//int argc, IntPtr[] argv, IntPtr self)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            
            RubyArray ary = new RubyArray(Internal.rb_funcallv(Args[0].Pointer, Internal.rb_intern("unpack"), 1, Internal.rb_str_new_cstr("D*")));

            return Internal.rb_funcallv(Class, Internal.rb_intern("new"), 4, new IntPtr[4]
            {
                ary[0].Pointer, 
                ary[1].Pointer,
                ary[2].Pointer,
                ary[3].Pointer
            });
        }

        protected static IntPtr redget(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@red");
        }

        protected static IntPtr redset(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            if (Internal.GetIVar(self, "@__viewport__") != Internal.QNil)
            {
                Viewport.ViewportDictionary[Internal.GetIVar(self, "@__viewport__")].Color.Red = (byte) Internal.NUM2LONG(Args[0].Pointer);
            }
            if (Internal.GetIVar(self, "@__sprite__") != Internal.QNil)
            {
                Sprite.SpriteDictionary[Internal.GetIVar(self, "@__sprite__")].Color.Red = (byte) Internal.NUM2LONG(Args[0].Pointer);
            }
            return Internal.SetIVar(self, "@red", Args[0].Pointer);
        }

        protected static IntPtr greenget(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@green");
        }

        protected static IntPtr greenset(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            if (Internal.GetIVar(self, "@__viewport__") != Internal.QNil)
            {
                Viewport.ViewportDictionary[Internal.GetIVar(self, "@__viewport__")].Color.Green = (byte) Internal.NUM2LONG(Args[0].Pointer);
            }
            if (Internal.GetIVar(self, "@__sprite__") != Internal.QNil)
            {
                Sprite.SpriteDictionary[Internal.GetIVar(self, "@__sprite__")].Color.Green = (byte) Internal.NUM2LONG(Args[0].Pointer);
            }
            return Internal.SetIVar(self, "@green", Args[0].Pointer);
        }

        protected static IntPtr blueget(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@blue");
        }

        protected static IntPtr blueset(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            if (Internal.GetIVar(self, "@__viewport__") != Internal.QNil)
            {
                Viewport.ViewportDictionary[Internal.GetIVar(self, "@__viewport__")].Color.Blue = (byte) Internal.NUM2LONG(Args[0].Pointer);
            }
            if (Internal.GetIVar(self, "@__sprite__") != Internal.QNil)
            {
                Sprite.SpriteDictionary[Internal.GetIVar(self, "@__sprite__")].Color.Blue = (byte) Internal.NUM2LONG(Args[0].Pointer);
            }
            return Internal.SetIVar(self, "@blue", Args[0].Pointer);
        }

        protected static IntPtr alphaget(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@alpha");
        }

        protected static IntPtr alphaset(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            if (Internal.GetIVar(self, "@__viewport__") != Internal.QNil)
            {
                Viewport.ViewportDictionary[Internal.GetIVar(self, "@__viewport__")].Color.Alpha = (byte) Internal.NUM2LONG(Args[0].Pointer);
            }
            if (Internal.GetIVar(self, "@__sprite__") != Internal.QNil)
            {
                Sprite.SpriteDictionary[Internal.GetIVar(self, "@__sprite__")].Color.Alpha = (byte) Internal.NUM2LONG(Args[0].Pointer);
            }
            return Internal.SetIVar(self, "@alpha", Args[0].Pointer);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using RubyDotNET;

namespace peridot
{
    public class Tone : RubyObject
    {
        public static IntPtr Class;

        public static Class CreateClass()
        {
            Class c = new Class("Tone");
            Class = c.Pointer;
            c.DefineClassMethod("new", _new);
            c.DefineMethod("initialize", initialize);
            c.DefineMethod("red", redget);
            c.DefineMethod("red=", redset);
            c.DefineMethod("green", greenget);
            c.DefineMethod("green=", greenset);
            c.DefineMethod("blue", blueget);
            c.DefineMethod("blue=", blueset);
            c.DefineMethod("grey", greyget);
            c.DefineMethod("grey=", greyset);
            return c;
        }

        public static odl.Tone CreateTone(IntPtr self)
        {
            return new odl.Tone(
                (short) Internal.NUM2LONG(Internal.GetIVar(self, "@red")),
                (short) Internal.NUM2LONG(Internal.GetIVar(self, "@green")),
                (short) Internal.NUM2LONG(Internal.GetIVar(self, "@blue")),
                (byte) Internal.NUM2LONG(Internal.GetIVar(self, "@grey"))
            );
        }

        public static IntPtr CreateTone(odl.Tone Tone)
        {
            return Internal.rb_funcallv(Class, Internal.rb_intern("new"), 4, new IntPtr[4]
            {
                Internal.LONG2NUM(Tone.Red),
                Internal.LONG2NUM(Tone.Green),
                Internal.LONG2NUM(Tone.Blue),
                Internal.LONG2NUM(Tone.Gray)
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
                   Grey = IntPtr.Zero;
            if (Args.Length == 3 || Args.Length == 4)
            {
                Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
                Internal.EnsureType(Args[1].Pointer, RubyClass.Integer);
                Internal.EnsureType(Args[2].Pointer, RubyClass.Integer);
                R = Args[0].Pointer;
                if (Internal.NUM2LONG(R) < -255) R = Internal.LONG2NUM(-255);
                else if (Internal.NUM2LONG(R) > 255) R = Internal.LONG2NUM(255);
                G = Args[1].Pointer;
                if (Internal.NUM2LONG(G) < -255) G = Internal.LONG2NUM(-255);
                else if (Internal.NUM2LONG(G) > 255) G = Internal.LONG2NUM(255);
                B = Args[2].Pointer;
                if (Internal.NUM2LONG(B) < -255) B = Internal.LONG2NUM(-255);
                else if (Internal.NUM2LONG(B) > 255) B = Internal.LONG2NUM(255);
                if (Args.Length == 4)
                {
                    Internal.EnsureType(Args[3].Pointer, RubyClass.Integer);
                    Grey = Args[3].Pointer;
                    if (Internal.NUM2LONG(Grey) < 0) Grey = Internal.LONG2NUM(0);
                    else if (Internal.NUM2LONG(Grey) > 255) Grey = Internal.LONG2NUM(255);
                }
                else Grey = Internal.LONG2NUM(0);
            }
            else ScanArgs(3, Args);
            Internal.SetIVar(self, "@red", R);
            Internal.SetIVar(self, "@green", G);
            Internal.SetIVar(self, "@blue", B);
            Internal.SetIVar(self, "@grey", Grey);
            return self;
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
            Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
            if (Internal.NUM2LONG(Args[0].Pointer) < -255) Args[0].Pointer = Internal.LONG2NUM(-255);
            else if (Internal.NUM2LONG(Args[0].Pointer) > 255) Args[0].Pointer = Internal.LONG2NUM(255);
            if (Internal.GetIVar(self, "@__sprite__") != Internal.QNil)
            {
                Sprite.SpriteDictionary[Internal.GetIVar(self, "@__sprite__")].Tone.Red = (short) Internal.NUM2LONG(Args[0].Pointer);
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
            Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
            if (Internal.NUM2LONG(Args[0].Pointer) < -255) Args[0].Pointer = Internal.LONG2NUM(-255);
            else if (Internal.NUM2LONG(Args[0].Pointer) > 255) Args[0].Pointer = Internal.LONG2NUM(255);
            if (Internal.GetIVar(self, "@__sprite__") != Internal.QNil)
            {
                Sprite.SpriteDictionary[Internal.GetIVar(self, "@__sprite__")].Tone.Green = (short) Internal.NUM2LONG(Args[0].Pointer);
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
            Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
            if (Internal.NUM2LONG(Args[0].Pointer) < -255) Args[0].Pointer = Internal.LONG2NUM(-255);
            else if (Internal.NUM2LONG(Args[0].Pointer) > 255) Args[0].Pointer = Internal.LONG2NUM(255);
            if (Internal.GetIVar(self, "@__sprite__") != Internal.QNil)
            {
                Sprite.SpriteDictionary[Internal.GetIVar(self, "@__sprite__")].Tone.Blue = (short) Internal.NUM2LONG(Args[0].Pointer);
            }
            return Internal.SetIVar(self, "@blue", Args[0].Pointer);
        }

        protected static IntPtr greyget(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@grey");
        }

        protected static IntPtr greyset(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
            if (Internal.NUM2LONG(Args[0].Pointer) < 0) Args[0].Pointer = Internal.LONG2NUM(0);
            else if (Internal.NUM2LONG(Args[0].Pointer) > 255) Args[0].Pointer = Internal.LONG2NUM(255);
            if (Internal.GetIVar(self, "@__sprite__") != Internal.QNil)
            {
                Sprite.SpriteDictionary[Internal.GetIVar(self, "@__sprite__")].Tone.Gray = (byte) Internal.NUM2LONG(Args[0].Pointer);
            }
            return Internal.SetIVar(self, "@grey", Args[0].Pointer);
        }
    }
}

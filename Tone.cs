using System;
using System.Collections.Generic;
using RubyDotNET;

namespace odlgss
{
    public class Tone : RubyObject
    {
        public ODL.Tone ToneObject;
        public static IntPtr ClassPointer;
        public static Dictionary<IntPtr, Tone> Tones = new Dictionary<IntPtr, Tone>();

        public static Class CreateClass()
        {
            Class c = new Class("Tone");
            ClassPointer = c.Pointer;
            c.DefineClassMethod("new", New);
            c.DefineMethod("initialize", initialize);
            c.DefineMethod("red", redget);
            c.DefineMethod("red=", redset);
            c.DefineMethod("green", greenget);
            c.DefineMethod("green=", greenset);
            c.DefineMethod("blue", blueget);
            c.DefineMethod("blue=", blueset);
            c.DefineMethod("gray", grayget);
            c.DefineMethod("gray=", grayset);
            return c;
        }

        private Tone()
        {
            this.Pointer = Internal.rb_funcallv(ClassPointer, Internal.rb_intern("allocate"), 0);
            Tones[Pointer] = this;
        }

        public static Tone New(sbyte Red, sbyte Green, sbyte Blue, byte Gray = 0)
        {
            IntPtr ptr = Internal.rb_funcallv(ClassPointer, Internal.rb_intern("new"), 4,
                Internal.LONG2NUM(Red),
                Internal.LONG2NUM(Green),
                Internal.LONG2NUM(Blue),
                Internal.LONG2NUM(Gray)
            );
            return Tones[ptr];
        }
        public void Initialize(sbyte Red, sbyte Green, sbyte Blue, byte Gray = 0)
        {
            ToneObject = new ODL.Tone(Red, Green, Blue, Gray);
            this.Red = Red;
            this.Green = Green;
            this.Blue = Blue;
            this.Gray = Gray;
        }

        public sbyte Red
        {
            get
            {
                return (sbyte) Internal.NUM2LONG(Internal.rb_funcallv(Pointer, Internal.rb_intern("red"), 0));
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("red="), 1, Internal.LONG2NUM(value));
            }
        }
        public sbyte Green
        {
            get
            {
                return (sbyte) Internal.NUM2LONG(Internal.rb_funcallv(Pointer, Internal.rb_intern("green"), 0));
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("green="), 1, Internal.LONG2NUM(value));
            }
        }
        public sbyte Blue
        {
            get
            {
                return (sbyte) Internal.NUM2LONG(Internal.rb_funcallv(Pointer, Internal.rb_intern("blue"), 0));
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("blue="), 1, Internal.LONG2NUM(value));
            }
        }
        public byte Gray
        {
            get
            {
                return (byte) Internal.NUM2LONG(Internal.rb_funcallv(Pointer, Internal.rb_intern("gray"), 0));
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("gray="), 1, Internal.LONG2NUM(value));
            }
        }

        static IntPtr New(IntPtr _self, IntPtr _args)
        {
            Tone t = new Tone();
            RubyArray Args = new RubyArray(_args);
            IntPtr[] newargs = new IntPtr[Args.Length];
            for (int i = 0; i < Args.Length; i++) newargs[i] = Args[i].Pointer;
            Internal.rb_funcallv(t.Pointer, Internal.rb_intern("initialize"), Args.Length, newargs);
            return t.Pointer;
        }
        static IntPtr initialize(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);

            Tone t = Tone.Tones[_self];
            if (Args.Length == 3 || Args.Length == 4)
            {
                int r = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[0].Pointer, Internal.rb_intern("to_i"), 0));
                int g = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[1].Pointer, Internal.rb_intern("to_i"), 0));
                int b = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[2].Pointer, Internal.rb_intern("to_i"), 0));
                int gr = 0;
                if (Args.Length == 4) gr = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[3].Pointer, Internal.rb_intern("to_i"), 0));
                t.Initialize((sbyte) r, (sbyte) g, (sbyte) b, (byte) gr);
            }
            else
            {
                ScanArgs(3, Args);
            }
            return _self;
        }

        static IntPtr redget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@red"));
        }
        static IntPtr redset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            Tones[_self].ToneObject.Red = (sbyte) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@red"), Args[0].Pointer);
        }

        static IntPtr greenget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@green"));
        }
        static IntPtr greenset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            Tones[_self].ToneObject.Green = (sbyte) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@green"), Args[0].Pointer);
        }

        static IntPtr blueget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@blue"));
        }
        static IntPtr blueset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            Tones[_self].ToneObject.Blue = (sbyte) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@blue"), Args[0].Pointer);
        }

        static IntPtr grayget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@gray"));
        }
        static IntPtr grayset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            Tones[_self].ToneObject.Gray = (byte) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@gray"), Args[0].Pointer);
        }
    }
}

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
            short R = 0,
                  G = 0,
                  B = 0;
            byte Grey = 0;
            if (Internal.IsType(Internal.GetIVar(self, "@red"), RubyClass.Float))
            {
                R = (short) Math.Round(Internal.rb_num2dbl(Internal.GetIVar(self, "@red")));
            }
            else
            {
                Internal.EnsureType(Internal.GetIVar(self, "@red"), RubyClass.Integer);
                R = (short) Internal.NUM2LONG(Internal.GetIVar(self, "@red"));
            }
            if (Internal.IsType(Internal.GetIVar(self, "@green"), RubyClass.Float))
            {
                G = (short) Math.Round(Internal.rb_num2dbl(Internal.GetIVar(self, "@green")));
            }
            else
            {
                Internal.EnsureType(Internal.GetIVar(self, "@green"), RubyClass.Integer);
                G = (short) Internal.NUM2LONG(Internal.GetIVar(self, "@green"));
            }
            if (Internal.IsType(Internal.GetIVar(self, "@blue"), RubyClass.Float))
            {
                B = (short) Math.Round(Internal.rb_num2dbl(Internal.GetIVar(self, "@blue")));
            }
            else
            {
                Internal.EnsureType(Internal.GetIVar(self, "@blue"), RubyClass.Integer);
                B = (short) Internal.NUM2LONG(Internal.GetIVar(self, "@blue"));
            }
            if (Internal.IsType(Internal.GetIVar(self, "@alpha"), RubyClass.Float))
            {
                Grey = (byte) Math.Round(Internal.rb_num2dbl(Internal.GetIVar(self, "@alpha")));
            }
            else
            {
                Internal.EnsureType(Internal.GetIVar(self, "@alpha"), RubyClass.Integer);
                Grey = (byte) Internal.NUM2LONG(Internal.GetIVar(self, "@alpha"));
            }
            return new odl.Tone(R, G, B, Grey);
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
                if (Internal.IsType(Args[0].Pointer, RubyClass.Float))
                {
                    if (Internal.rb_num2dbl(Args[0].Pointer) < -255) R = Internal.rb_float_new(-255);
                    else if (Internal.rb_num2dbl(Args[0].Pointer) > 255) R = Internal.rb_float_new(255);
                    else R = Args[0].Pointer;
                }
                else
                {
                    Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
                    if (Internal.NUM2LONG(Args[0].Pointer) < -255) R = Internal.LONG2NUM(-255);
                    else if (Internal.NUM2LONG(Args[0].Pointer) > 255) R = Internal.LONG2NUM(255);
                    else R = Args[0].Pointer;
                }
                if (Internal.IsType(Args[1].Pointer, RubyClass.Float))
                {
                    if (Internal.rb_num2dbl(Args[1].Pointer) < -255) G = Internal.rb_float_new(-255);
                    else if (Internal.rb_num2dbl(Args[1].Pointer) > 255) G = Internal.rb_float_new(255);
                    else G = Args[1].Pointer;
                }
                else
                {
                    Internal.EnsureType(Args[1].Pointer, RubyClass.Integer);
                    if (Internal.NUM2LONG(Args[1].Pointer) < -255) G = Internal.LONG2NUM(-255);
                    else if (Internal.NUM2LONG(Args[1].Pointer) > 255) G = Internal.LONG2NUM(255);
                    else G = Args[1].Pointer;
                }
                if (Internal.IsType(Args[2].Pointer, RubyClass.Float))
                {
                    if (Internal.rb_num2dbl(Args[2].Pointer) < -255) B = Internal.rb_float_new(-255);
                    else if (Internal.rb_num2dbl(Args[2].Pointer) > 255) B = Internal.rb_float_new(255);
                    else B = Args[2].Pointer;
                }
                else
                {
                    Internal.EnsureType(Args[2].Pointer, RubyClass.Integer);
                    if (Internal.NUM2LONG(Args[2].Pointer) < -255) B = Internal.LONG2NUM(-255);
                    else if (Internal.NUM2LONG(Args[2].Pointer) > 255) B = Internal.LONG2NUM(255);
                    else B = Args[2].Pointer;
                }
                if (Args.Length == 4)
                {
                    if (Internal.IsType(Args[3].Pointer, RubyClass.Float))
                    {
                        if (Internal.rb_num2dbl(Args[3].Pointer) < -255) Grey = Internal.rb_float_new(-255);
                        else if (Internal.rb_num2dbl(Args[3].Pointer) > 255) Grey = Internal.rb_float_new(255);
                        else Grey = Args[3].Pointer;
                    }
                    else
                    {
                        Internal.EnsureType(Args[3].Pointer, RubyClass.Integer);
                        if (Internal.NUM2LONG(Args[3].Pointer) < -255) Grey = Internal.LONG2NUM(-255);
                        else if (Internal.NUM2LONG(Args[3].Pointer) > 255) Grey = Internal.LONG2NUM(255);
                        else Grey = Args[3].Pointer;
                    }
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
            short R = 0;
            if (Internal.IsType(Args[0].Pointer, RubyClass.Float))
            {
                if (Internal.rb_num2dbl(Args[0].Pointer) < -255) R = -255;
                else if (Internal.rb_num2dbl(Args[0].Pointer) > 255) R = 255;
                else R = (short) Internal.NUM2LONG(Args[0].Pointer);
                Args[0].Pointer = Internal.rb_float_new(R);
            }
            else
            {
                Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
                if (Internal.NUM2LONG(Args[0].Pointer) < -255) R = -255;
                else if (Internal.NUM2LONG(Args[0].Pointer) > 255) R = 255;
                else R = (short) Internal.NUM2LONG(Args[0].Pointer);
                Args[0].Pointer = Internal.LONG2NUM(R);
            }
            if (Internal.GetIVar(self, "@__sprite__") != Internal.QNil)
            {
                Sprite.SpriteDictionary[Internal.GetIVar(self, "@__sprite__")].Tone.Red = R;
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
            short G = 0;
            if (Internal.IsType(Args[0].Pointer, RubyClass.Float))
            {
                if (Internal.rb_num2dbl(Args[0].Pointer) < -255) G = -255;
                else if (Internal.rb_num2dbl(Args[0].Pointer) > 255) G = 255;
                else G = (short) Internal.NUM2LONG(Args[0].Pointer);
                Args[0].Pointer = Internal.rb_float_new(G);
            }
            else
            {
                Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
                if (Internal.NUM2LONG(Args[0].Pointer) < -255) G = -255;
                else if (Internal.NUM2LONG(Args[0].Pointer) > 255) G = 255;
                else G = (short) Internal.NUM2LONG(Args[0].Pointer);
                Args[0].Pointer = Internal.LONG2NUM(G);
            }
            if (Internal.GetIVar(self, "@__sprite__") != Internal.QNil)
            {
                Sprite.SpriteDictionary[Internal.GetIVar(self, "@__sprite__")].Tone.Green = G;
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
            short B = 0;
            if (Internal.IsType(Args[0].Pointer, RubyClass.Float))
            {
                if (Internal.rb_num2dbl(Args[0].Pointer) < -255) B = -255;
                else if (Internal.rb_num2dbl(Args[0].Pointer) > 255) B = 255;
                else B = (short) Internal.NUM2LONG(Args[0].Pointer);
                Args[0].Pointer = Internal.rb_float_new(B);
            }
            else
            {
                Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
                if (Internal.NUM2LONG(Args[0].Pointer) < -255) B = -255;
                else if (Internal.NUM2LONG(Args[0].Pointer) > 255) B = 255;
                else B = (short) Internal.NUM2LONG(Args[0].Pointer);
                Args[0].Pointer = Internal.LONG2NUM(B);
            }
            if (Internal.GetIVar(self, "@__sprite__") != Internal.QNil)
            {
                Sprite.SpriteDictionary[Internal.GetIVar(self, "@__sprite__")].Tone.Blue = B;
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
            byte Grey = 0;
            if (Internal.IsType(Args[0].Pointer, RubyClass.Float))
            {
                if (Internal.rb_num2dbl(Args[0].Pointer) < 0) Grey = 0;
                else if (Internal.rb_num2dbl(Args[0].Pointer) > 255) Grey = 255;
                else Grey = (byte) Internal.NUM2LONG(Args[0].Pointer);
                Args[0].Pointer = Internal.rb_float_new(Grey);
            }
            else
            {
                Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
                if (Internal.NUM2LONG(Args[0].Pointer) < 0) Grey = 0;
                else if (Internal.NUM2LONG(Args[0].Pointer) > 255) Grey = 255;
                else Grey = (byte) Internal.NUM2LONG(Args[0].Pointer);
                Args[0].Pointer = Internal.LONG2NUM(Grey);
            }
            if (Internal.GetIVar(self, "@__sprite__") != Internal.QNil)
            {
                Sprite.SpriteDictionary[Internal.GetIVar(self, "@__sprite__")].Tone.Gray = Grey;
            }
            return Internal.SetIVar(self, "@grey", Args[0].Pointer);
        }
    }
}

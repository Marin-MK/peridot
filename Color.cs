using System;
using System.Collections.Generic;
using System.Text;
using RubyDotNET;

namespace peridot
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
            byte R = 0,
                 G = 0,
                 B = 0,
                 A = 0;
            if (Internal.IsType(Internal.GetIVar(self, "@red"), RubyClass.Float))
            {
                R = (byte) Math.Round(Internal.rb_num2dbl(Internal.GetIVar(self, "@red")));
            }
            else
            {
                Internal.EnsureType(Internal.GetIVar(self, "@red"), RubyClass.Integer);
                R = (byte) Internal.NUM2LONG(Internal.GetIVar(self, "@red"));
            }
            if (Internal.IsType(Internal.GetIVar(self, "@green"), RubyClass.Float))
            {
                G = (byte) Math.Round(Internal.rb_num2dbl(Internal.GetIVar(self, "@green")));
            }
            else
            {
                Internal.EnsureType(Internal.GetIVar(self, "@green"), RubyClass.Integer);
                G = (byte) Internal.NUM2LONG(Internal.GetIVar(self, "@green"));
            }
            if (Internal.IsType(Internal.GetIVar(self, "@blue"), RubyClass.Float))
            {
                B = (byte) Math.Round(Internal.rb_num2dbl(Internal.GetIVar(self, "@blue")));
            }
            else
            {
                Internal.EnsureType(Internal.GetIVar(self, "@blue"), RubyClass.Integer);
                B = (byte) Internal.NUM2LONG(Internal.GetIVar(self, "@blue"));
            }
            if (Internal.IsType(Internal.GetIVar(self, "@alpha"), RubyClass.Float))
            {
                A = (byte) Math.Round(Internal.rb_num2dbl(Internal.GetIVar(self, "@alpha")));
            }
            else
            {
                Internal.EnsureType(Internal.GetIVar(self, "@alpha"), RubyClass.Integer);
                A = (byte) Internal.NUM2LONG(Internal.GetIVar(self, "@alpha"));
            }
            return new odl.Color(R, G, B, A);
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
                if (Internal.IsType(Args[0].Pointer, RubyClass.Float))
                {
                    if (Internal.rb_num2dbl(Args[0].Pointer) < 0) R = Internal.rb_float_new(0);
                    else if (Internal.rb_num2dbl(Args[0].Pointer) > 255) R = Internal.rb_float_new(255);
                    else R = Args[0].Pointer;
                }
                else
                {
                    Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
                    if (Internal.NUM2LONG(Args[0].Pointer) < 0) R = Internal.LONG2NUM(0);
                    else if (Internal.NUM2LONG(Args[0].Pointer) > 255) R = Internal.LONG2NUM(255);
                    else R = Args[0].Pointer;
                }
                if (Internal.IsType(Args[1].Pointer, RubyClass.Float))
                {
                    if (Internal.rb_num2dbl(Args[1].Pointer) < 0) G = Internal.rb_float_new(0);
                    else if (Internal.rb_num2dbl(Args[1].Pointer) > 255) G = Internal.rb_float_new(255);
                    else G = Args[1].Pointer;
                }
                else
                {
                    Internal.EnsureType(Args[1].Pointer, RubyClass.Integer);
                    if (Internal.NUM2LONG(Args[1].Pointer) < 0) G = Internal.LONG2NUM(0);
                    else if (Internal.NUM2LONG(Args[1].Pointer) > 255) G = Internal.LONG2NUM(255);
                    else G = Args[1].Pointer;
                }
                if (Internal.IsType(Args[2].Pointer, RubyClass.Float))
                {
                    if (Internal.rb_num2dbl(Args[2].Pointer) < 0) B = Internal.rb_float_new(0);
                    else if (Internal.rb_num2dbl(Args[2].Pointer) > 255) B = Internal.rb_float_new(255);
                    else B = Args[2].Pointer;
                }
                else
                {
                    Internal.EnsureType(Args[2].Pointer, RubyClass.Integer);
                    if (Internal.NUM2LONG(Args[2].Pointer) < 0) B = Internal.LONG2NUM(0);
                    else if (Internal.NUM2LONG(Args[2].Pointer) > 255) B = Internal.LONG2NUM(255);
                    else B = Args[2].Pointer;
                }
                if (Args.Length == 4)
                {
                    if (Internal.IsType(Args[3].Pointer, RubyClass.Float))
                    {
                        if (Internal.rb_num2dbl(Args[3].Pointer) < 0) A = Internal.rb_float_new(0);
                        else if (Internal.rb_num2dbl(Args[3].Pointer) > 255) A = Internal.rb_float_new(255);
                        else A = Args[3].Pointer;
                    }
                    else
                    {
                        Internal.EnsureType(Args[3].Pointer, RubyClass.Integer);
                        if (Internal.NUM2LONG(Args[3].Pointer) < 0) A = Internal.LONG2NUM(0);
                        else if (Internal.NUM2LONG(Args[3].Pointer) > 255) A = Internal.LONG2NUM(255);
                        else A = Args[3].Pointer;
                    }
                }
                else A = Internal.LONG2NUM(255);
            }
            else ScanArgs(3, Args);
            Internal.SetIVar(self, "@red", R);
            Internal.SetIVar(self, "@green", G);
            Internal.SetIVar(self, "@blue", B);
            Internal.SetIVar(self, "@alpha", A);
            return self;
        }

        protected static IntPtr _load(IntPtr _self, IntPtr _args)
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
            byte R = 0;
            if (Internal.IsType(Args[0].Pointer, RubyClass.Float))
            {
                if (Internal.rb_num2dbl(Args[0].Pointer) < 0) R = 0;
                else if (Internal.rb_num2dbl(Args[0].Pointer) > 255) R = 255;
                else R = (byte) Internal.NUM2LONG(Args[0].Pointer);
                Args[0].Pointer = Internal.rb_float_new(R);
            }
            else
            {
                Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
                if (Internal.NUM2LONG(Args[0].Pointer) < 0) R = 0;
                else if (Internal.NUM2LONG(Args[0].Pointer) > 255) R = 255;
                else R = (byte) Internal.NUM2LONG(Args[0].Pointer);
                Args[0].Pointer = Internal.LONG2NUM(R);
            }
            if (Internal.GetIVar(self, "@__viewport__") != Internal.QNil)
            {
                Viewport.ViewportDictionary[Internal.GetIVar(self, "@__viewport__")].Color.Red = R;
            }
            if (Internal.GetIVar(self, "@__sprite__") != Internal.QNil)
            {
                Sprite.SpriteDictionary[Internal.GetIVar(self, "@__sprite__")].Color.Red = R;
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
            byte G = 0;
            if (Internal.IsType(Args[0].Pointer, RubyClass.Float))
            {
                if (Internal.rb_num2dbl(Args[0].Pointer) < 0) G = 0;
                else if (Internal.rb_num2dbl(Args[0].Pointer) > 255) G = 255;
                else G = (byte) Internal.NUM2LONG(Args[0].Pointer);
                Args[0].Pointer = Internal.rb_float_new(G);
            }
            else
            {
                Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
                if (Internal.NUM2LONG(Args[0].Pointer) < 0) G = 0;
                else if (Internal.NUM2LONG(Args[0].Pointer) > 255) G = 255;
                else G = (byte) Internal.NUM2LONG(Args[0].Pointer);
                Args[0].Pointer = Internal.LONG2NUM(G);
            }
            if (Internal.GetIVar(self, "@__viewport__") != Internal.QNil)
            {
                Viewport.ViewportDictionary[Internal.GetIVar(self, "@__viewport__")].Color.Green = G;
            }
            if (Internal.GetIVar(self, "@__sprite__") != Internal.QNil)
            {
                Sprite.SpriteDictionary[Internal.GetIVar(self, "@__sprite__")].Color.Green = G;
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
            byte B = 0;
            if (Internal.IsType(Args[0].Pointer, RubyClass.Float))
            {
                if (Internal.rb_num2dbl(Args[0].Pointer) < 0) B = 0;
                else if (Internal.rb_num2dbl(Args[0].Pointer) > 255) B = 255;
                else B = (byte) Internal.NUM2LONG(Args[0].Pointer);
                Args[0].Pointer = Internal.rb_float_new(B);
            }
            else
            {
                Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
                if (Internal.NUM2LONG(Args[0].Pointer) < 0) B = 0;
                else if (Internal.NUM2LONG(Args[0].Pointer) > 255) B = 255;
                else B = (byte) Internal.NUM2LONG(Args[0].Pointer);
                Args[0].Pointer = Internal.LONG2NUM(B);
            }
            if (Internal.GetIVar(self, "@__viewport__") != Internal.QNil)
            {
                Viewport.ViewportDictionary[Internal.GetIVar(self, "@__viewport__")].Color.Blue = B;
            }
            if (Internal.GetIVar(self, "@__sprite__") != Internal.QNil)
            {
                Sprite.SpriteDictionary[Internal.GetIVar(self, "@__sprite__")].Color.Blue = B;
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
            byte A = 0;
            if (Internal.IsType(Args[0].Pointer, RubyClass.Float))
            {
                if (Internal.rb_num2dbl(Args[0].Pointer) < 0) A = 0;
                else if (Internal.rb_num2dbl(Args[0].Pointer) > 255) A = 255;
                else A = (byte) Internal.NUM2LONG(Args[0].Pointer);
                Args[0].Pointer = Internal.rb_float_new(A);
            }
            else
            {
                Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
                if (Internal.NUM2LONG(Args[0].Pointer) < 0) A = 0;
                else if (Internal.NUM2LONG(Args[0].Pointer) > 255) A = 255;
                else A = (byte) Internal.NUM2LONG(Args[0].Pointer);
                Args[0].Pointer = Internal.LONG2NUM(A);
            }
            if (Internal.GetIVar(self, "@__viewport__") != Internal.QNil)
            {
                Viewport.ViewportDictionary[Internal.GetIVar(self, "@__viewport__")].Color.Alpha = A;
            }
            if (Internal.GetIVar(self, "@__sprite__") != Internal.QNil)
            {
                Sprite.SpriteDictionary[Internal.GetIVar(self, "@__sprite__")].Color.Alpha = A;
            }
            return Internal.SetIVar(self, "@alpha", Args[0].Pointer);
        }
    }
}

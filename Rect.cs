using System;
using System.Collections.Generic;
using System.Text;
using RubyDotNET;

namespace peridot
{
    public class Rect : RubyObject
    {
        public static IntPtr Class;

        public static Class CreateClass()
        {
            Class c = new Class("Rect");
            Class = c.Pointer;
            c.DefineClassMethod("new", _new);
            c.DefineMethod("initialize", initialize);
            c.DefineMethod("x", xget);
            c.DefineMethod("x=", xset);
            c.DefineMethod("y", yget);
            c.DefineMethod("y=", yset);
            c.DefineMethod("width", widthget);
            c.DefineMethod("width=", widthset);
            c.DefineMethod("height", heightget);
            c.DefineMethod("height=", heightset);
            c.DefineMethod("set", set);
            return c;
        }

        public static odl.Rect CreateRect(IntPtr self)
        {
            int x = 0,
                y = 0,
                w = 0,
                h = 0;
            if (Internal.IsType(Internal.GetIVar(self, "@x"), RubyClass.Float))
            {
                x = (int) Math.Round(Internal.rb_num2dbl(Internal.GetIVar(self, "@x")));
            }
            else
            {
                Internal.EnsureType(Internal.GetIVar(self, "@x"), RubyClass.Integer);
                x = (int) Internal.NUM2LONG(Internal.GetIVar(self, "@x"));
            }
            if (Internal.IsType(Internal.GetIVar(self, "@y"), RubyClass.Float))
            {
                y = (int) Math.Round(Internal.rb_num2dbl(Internal.GetIVar(self, "@y")));
            }
            else
            {
                Internal.EnsureType(Internal.GetIVar(self, "@y"), RubyClass.Integer);
                y = (int) Internal.NUM2LONG(Internal.GetIVar(self, "@y"));
            }
            if (Internal.IsType(Internal.GetIVar(self, "@width"), RubyClass.Float))
            {
                w = (int) Math.Round(Internal.rb_num2dbl(Internal.GetIVar(self, "@width")));
            }
            else
            {
                Internal.EnsureType(Internal.GetIVar(self, "@width"), RubyClass.Integer);
                w = (int) Internal.NUM2LONG(Internal.GetIVar(self, "@width"));
            }
            if (Internal.IsType(Internal.GetIVar(self, "@height"), RubyClass.Float))
            {
                h = (int) Math.Round(Internal.rb_num2dbl(Internal.GetIVar(self, "@height")));
            }
            else
            {
                Internal.EnsureType(Internal.GetIVar(self, "@height"), RubyClass.Integer);
                h = (int) Internal.NUM2LONG(Internal.GetIVar(self, "@height"));
            }
            return new odl.Rect(x, y, w, h);
        }

        public static IntPtr CreateRect(odl.Rect Rect)
        {
            return Internal.rb_funcallv(Class, Internal.rb_intern("new"), 4, new IntPtr[4]
            {
                Internal.LONG2NUM(Rect.X),
                Internal.LONG2NUM(Rect.Y),
                Internal.LONG2NUM(Rect.Width),
                Internal.LONG2NUM(Rect.Height)
            });
        }

        public static IntPtr CreateRect(odl.Size Size)
        {
            return Internal.rb_funcallv(Class, Internal.rb_intern("new"), 4, new IntPtr[4]
            {
                Internal.LONG2NUM(0),
                Internal.LONG2NUM(0),
                Internal.LONG2NUM(Size.Width),
                Internal.LONG2NUM(Size.Height)
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
            ScanArgs(4, Args);
            Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
            Internal.EnsureType(Args[1].Pointer, RubyClass.Integer);
            Internal.EnsureType(Args[2].Pointer, RubyClass.Integer);
            Internal.EnsureType(Args[3].Pointer, RubyClass.Integer);
            IntPtr X = Args[0].Pointer;
            IntPtr Y = Args[1].Pointer;
            IntPtr Width = Args[2].Pointer;
            IntPtr Height = Args[3].Pointer;
            Internal.SetIVar(self, "@x", X);
            Internal.SetIVar(self, "@y", Y);
            Internal.SetIVar(self, "@width", Width);
            Internal.SetIVar(self, "@height", Height);
            return self;
        }

        protected static IntPtr xget(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@x");
        }

        protected static IntPtr xset(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            int x = 0;
            if (Internal.IsType(Args[0].Pointer, RubyClass.Float))
            {
                x = (int) Math.Round(Internal.rb_num2dbl(Args[0].Pointer));
            }
            else
            {
                Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
                x = (int) Internal.NUM2LONG(Args[0].Pointer);
            }
            if (Internal.GetIVar(self, "@__sprite__") != Internal.QNil)
            {
                Sprite.SpriteDictionary[Internal.GetIVar(self, "@__sprite__")].SrcRect.X = x;
            }
            if (Internal.GetIVar(self, "@__viewport__") != Internal.QNil)
            {
                Viewport.ViewportDictionary[Internal.GetIVar(self, "@__viewport__")].X = x;
            }
            return Internal.SetIVar(self, "@x", Args[0].Pointer);
        }

        protected static IntPtr yget(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@y");
        }

        protected static IntPtr yset(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            int y = 0;
            if (Internal.IsType(Args[0].Pointer, RubyClass.Float))
            {
                y = (int) Math.Round(Internal.rb_num2dbl(Args[0].Pointer));
            }
            else
            {
                Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
                y = (int) Internal.NUM2LONG(Args[0].Pointer);
            }
            if (Internal.GetIVar(self, "@__sprite__") != Internal.QNil)
            {
                Sprite.SpriteDictionary[Internal.GetIVar(self, "@__sprite__")].SrcRect.Y = y;
            }
            if (Internal.GetIVar(self, "@__viewport__") != Internal.QNil)
            {
                Viewport.ViewportDictionary[Internal.GetIVar(self, "@__viewport__")].Y = y;
            }
            return Internal.SetIVar(self, "@y", Args[0].Pointer);
        }

        protected static IntPtr widthget(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@width");
        }

        protected static IntPtr widthset(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            int w = 0;
            if (Internal.IsType(Args[0].Pointer, RubyClass.Float))
            {
                w = (int) Math.Round(Internal.rb_num2dbl(Args[0].Pointer));
            }
            else
            {
                Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
                w = (int) Internal.NUM2LONG(Args[0].Pointer);
            }
            if (Internal.GetIVar(self, "@__sprite__") != Internal.QNil)
            {
                Sprite.SpriteDictionary[Internal.GetIVar(self, "@__sprite__")].SrcRect.Width = w;
            }
            if (Internal.GetIVar(self, "@__viewport__") != Internal.QNil)
            {
                Viewport.ViewportDictionary[Internal.GetIVar(self, "@__viewport__")].Width = w;
            }
            return Internal.SetIVar(self, "@width", Args[0].Pointer);
        }

        protected static IntPtr heightget(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@height");
        }

        protected static IntPtr heightset(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            int h = 0;
            if (Internal.IsType(Args[0].Pointer, RubyClass.Float))
            {
                h = (int) Math.Round(Internal.rb_num2dbl(Args[0].Pointer));
            }
            else
            {
                Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
                h = (int) Internal.NUM2LONG(Args[0].Pointer);
            }
            if (Internal.GetIVar(self, "@__sprite__") != Internal.QNil)
            {
                Sprite.SpriteDictionary[Internal.GetIVar(self, "@__sprite__")].SrcRect.Height = h;
            }
            if (Internal.GetIVar(self, "@__viewport__") != Internal.QNil)
            {
                Viewport.ViewportDictionary[Internal.GetIVar(self, "@__viewport__")].Height = h;
            }
            return Internal.SetIVar(self, "@height", Args[0].Pointer);
        }

        protected static IntPtr set(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(4, Args);
            int x = 0,
                y = 0,
                w = 0,
                h = 0;
            if (Internal.IsType(Args[0].Pointer, RubyClass.Float))
            {
                x = (int) Math.Round(Internal.rb_num2dbl(Args[0].Pointer));
            }
            else
            {
                Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
                x = (int) Internal.NUM2LONG(Args[0].Pointer);
            }
            if (Internal.IsType(Args[1].Pointer, RubyClass.Float))
            {
                y = (int) Math.Round(Internal.rb_num2dbl(Args[1].Pointer));
            }
            else
            {
                Internal.EnsureType(Args[1].Pointer, RubyClass.Integer);
                y = (int) Internal.NUM2LONG(Args[1].Pointer);
            }
            if (Internal.IsType(Args[2].Pointer, RubyClass.Float))
            {
                w = (int) Math.Round(Internal.rb_num2dbl(Args[2].Pointer));
            }
            else
            {
                Internal.EnsureType(Args[2].Pointer, RubyClass.Integer);
                w = (int) Internal.NUM2LONG(Args[2].Pointer);
            }
            if (Internal.IsType(Args[3].Pointer, RubyClass.Float))
            {
                h = (int) Math.Round(Internal.rb_num2dbl(Args[3].Pointer));
            }
            else
            {
                Internal.EnsureType(Args[3].Pointer, RubyClass.Integer);
                h = (int) Internal.NUM2LONG(Args[3].Pointer);
            }
            Internal.SetIVar(self, "@x", Args[0].Pointer);
            Internal.SetIVar(self, "@y", Args[1].Pointer);
            Internal.SetIVar(self, "@width", Args[2].Pointer);
            Internal.SetIVar(self, "@height", Args[3].Pointer);
            if (Internal.GetIVar(self, "@__sprite__") != Internal.QNil)
            {
                Sprite.SpriteDictionary[Internal.GetIVar(self, "@__sprite__")].SrcRect.X = x;
                Sprite.SpriteDictionary[Internal.GetIVar(self, "@__sprite__")].SrcRect.Y = y;
                Sprite.SpriteDictionary[Internal.GetIVar(self, "@__sprite__")].SrcRect.Width = w;
                Sprite.SpriteDictionary[Internal.GetIVar(self, "@__sprite__")].SrcRect.Height = h;
            }
            if (Internal.GetIVar(self, "@__viewport__") != Internal.QNil)
            {
                Viewport.ViewportDictionary[Internal.GetIVar(self, "@__viewport__")].X = x;
                Viewport.ViewportDictionary[Internal.GetIVar(self, "@__viewport__")].Y = y;
                Viewport.ViewportDictionary[Internal.GetIVar(self, "@__viewport__")].Width = w;
                Viewport.ViewportDictionary[Internal.GetIVar(self, "@__viewport__")].Height = h;
            }
            return self;
        }
    }
}

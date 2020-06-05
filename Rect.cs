using System;
using System.Collections.Generic;
using System.Text;
using RubyDotNET;

namespace Peridot
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
            return new odl.Rect(
                (int) Internal.NUM2LONG(Internal.GetIVar(self, "@x")),
                (int) Internal.NUM2LONG(Internal.GetIVar(self, "@y")),
                (int) Internal.NUM2LONG(Internal.GetIVar(self, "@width")),
                (int) Internal.NUM2LONG(Internal.GetIVar(self, "@height"))
            );
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
            if (Internal.GetIVar(self, "@__sprite__") != Internal.QNil)
            {
                Sprite.SpriteDictionary[Internal.GetIVar(self, "@__sprite__")].SrcRect.X = (int) Internal.NUM2LONG(Args[0].Pointer);
            }
            if (Internal.GetIVar(self, "@__viewport__") != Internal.QNil)
            {
                Viewport.ViewportDictionary[Internal.GetIVar(self, "@__viewport__")].X = (int) Internal.NUM2LONG(Args[0].Pointer);
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
            if (Internal.GetIVar(self, "@__sprite__") != Internal.QNil)
            {
                Sprite.SpriteDictionary[Internal.GetIVar(self, "@__sprite__")].SrcRect.Y = (int) Internal.NUM2LONG(Args[0].Pointer);
            }
            if (Internal.GetIVar(self, "@__viewport__") != Internal.QNil)
            {
                Viewport.ViewportDictionary[Internal.GetIVar(self, "@__viewport__")].Y = (int) Internal.NUM2LONG(Args[0].Pointer);
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
            if (Internal.GetIVar(self, "@__sprite__") != Internal.QNil)
            {
                Sprite.SpriteDictionary[Internal.GetIVar(self, "@__sprite__")].SrcRect.Width = (int) Internal.NUM2LONG(Args[0].Pointer);
            }
            if (Internal.GetIVar(self, "@__viewport__") != Internal.QNil)
            {
                Viewport.ViewportDictionary[Internal.GetIVar(self, "@__viewport__")].Width = (int) Internal.NUM2LONG(Args[0].Pointer);
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
            if (Internal.GetIVar(self, "@__sprite__") != Internal.QNil)
            {
                Sprite.SpriteDictionary[Internal.GetIVar(self, "@__sprite__")].SrcRect.Height = (int) Internal.NUM2LONG(Args[0].Pointer);
            }
            if (Internal.GetIVar(self, "@__viewport__") != Internal.QNil)
            {
                Viewport.ViewportDictionary[Internal.GetIVar(self, "@__viewport__")].Height = (int) Internal.NUM2LONG(Args[0].Pointer);
            }
            return Internal.SetIVar(self, "@height", Args[0].Pointer);
        }

        protected static IntPtr set(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(4, Args);
            int x = (int) Internal.NUM2LONG(Args[0].Pointer);
            int y = (int) Internal.NUM2LONG(Args[1].Pointer);
            int w = (int) Internal.NUM2LONG(Args[2].Pointer);
            int h = (int) Internal.NUM2LONG(Args[3].Pointer);
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

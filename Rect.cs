using System;
using System.Collections.Generic;
using RubyDotNET;

namespace odlgss
{
    public class Rect : RubyObject
    {
        public ODL.Rect RectObject;
        public IntPtr ViewportPointer;
        public static IntPtr ClassPointer { get; set; }
        public static Dictionary<IntPtr, Rect> Rects = new Dictionary<IntPtr, Rect>();

        public static Class CreateClass()
        {
            Class c = new Class("Rect");
            ClassPointer = c.Pointer;
            c.DefineClassMethod("new", New);
            c.DefineMethod("x", xget);
            c.DefineMethod("x=", xset);
            c.DefineMethod("y", yget);
            c.DefineMethod("y=", yset);
            c.DefineMethod("width", widthget);
            c.DefineMethod("width=", widthset);
            c.DefineMethod("height", heightget);
            c.DefineMethod("height=", heightset);
            return c;
        }

        public Rect(IntPtr ViewportPointer, int X, int Y, int Width, int Height)
        {
            this.Pointer = Internal.rb_funcallv(ClassPointer, Internal.rb_intern("allocate"), 0);
            RectObject = new ODL.Rect(X, Y, Width, Height);
            Rects[Pointer] = this;
            this.ViewportPointer = ViewportPointer;
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
        }

        public Rect(int X, int Y, int Width, int Height)
            : this(IntPtr.Zero, X, Y, Width, Height) { }

        public int X
        {
            get
            {
                return (int) Internal.NUM2LONG(Internal.rb_funcallv(Pointer, Internal.rb_intern("x"), 0, IntPtr.Zero));
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("x="), 1, Internal.LONG2NUM(value));
            }
        }
        public int Y
        {
            get
            {
                return (int)Internal.NUM2LONG(Internal.rb_funcallv(Pointer, Internal.rb_intern("y"), 0, IntPtr.Zero));
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("y="), 1, Internal.LONG2NUM(value));
            }
        }
        public int Width
        {
            get
            {
                return (int)Internal.NUM2LONG(Internal.rb_funcallv(Pointer, Internal.rb_intern("width"), 0, IntPtr.Zero));
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("width="), 1, Internal.LONG2NUM(value));
            }
        }
        public int Height
        {
            get
            {
                return (int)Internal.NUM2LONG(Internal.rb_funcallv(Pointer, Internal.rb_intern("height"), 0, IntPtr.Zero));
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("height="), 1, Internal.LONG2NUM(value));
            }
        }

        static IntPtr New(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(4, Args);

            int x = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[0].Pointer, Internal.rb_intern("to_i"), 0, IntPtr.Zero));
            int y = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[1].Pointer, Internal.rb_intern("to_i"), 0, IntPtr.Zero));
            int w = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[2].Pointer, Internal.rb_intern("to_i"), 0, IntPtr.Zero));
            int h = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[3].Pointer, Internal.rb_intern("to_i"), 0, IntPtr.Zero));

            Rect v = new Rect(x, y, w, h);
            return v.Pointer;
        }

        static IntPtr xget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@x"));
        }

        static IntPtr xset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            Rects[_self].RectObject.X = (int) Internal.NUM2LONG(Args[0].Pointer);
            if (Rects[_self].ViewportPointer != IntPtr.Zero)
                Viewport.Viewports[Rects[_self].ViewportPointer].ViewportObject.X = (int) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@x"), Args[0].Pointer);
        }

        static IntPtr yget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@y"));
        }

        static IntPtr yset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            Rects[_self].RectObject.Y = (int) Internal.NUM2LONG(Args[0].Pointer);
            if (Rects[_self].ViewportPointer != IntPtr.Zero)
                Viewport.Viewports[Rects[_self].ViewportPointer].ViewportObject.Y = (int) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@y"), Args[0].Pointer);
        }

        static IntPtr widthget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@width"));
        }

        static IntPtr widthset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            Rects[_self].RectObject.Width = (int) Internal.NUM2LONG(Args[0].Pointer);
            if (Rects[_self].ViewportPointer != IntPtr.Zero)
                Viewport.Viewports[Rects[_self].ViewportPointer].ViewportObject.Width = (int) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@width"), Args[0].Pointer);
        }

        static IntPtr heightget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@height"));
        }

        static IntPtr heightset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            Rects[_self].RectObject.Height = (int) Internal.NUM2LONG(Args[0].Pointer);
            if (Rects[_self].ViewportPointer != IntPtr.Zero)
                Viewport.Viewports[Rects[_self].ViewportPointer].ViewportObject.Height = (int) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@height"), Args[0].Pointer);
        }
    }
}

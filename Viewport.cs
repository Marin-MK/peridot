using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using RubyDotNET;

namespace odlgss
{
    public class Viewport : RubyObject
    {
        public ODL.Viewport ViewportObject;
        public static IntPtr ClassPointer { get; set; }
        public static Dictionary<IntPtr, Viewport> Viewports = new Dictionary<IntPtr, Viewport>();

        public static Class CreateClass()
        {
            Class c = new Class("Viewport");
            ClassPointer = c.Pointer;
            c.DefineClassMethod("new", New);
            c.DefineMethod("rect", rectget);
            c.DefineMethod("rect=", rectset);
            c.DefineMethod("z", zget);
            c.DefineMethod("z=", zset);
            c.DefineMethod("zoom_x", zoom_xget);
            c.DefineMethod("zoom_x=", zoom_xset);
            c.DefineMethod("zoom_y", zoom_yget);
            c.DefineMethod("zoom_y=", zoom_yset);
            return c;
        }

        public Viewport(int X, int Y, int Width, int Height)
        {
            this.Pointer = Internal.rb_funcallv(ClassPointer, Internal.rb_intern("allocate"), 0);
            ViewportObject = new ODL.Viewport(Program.MainWindow.Renderer, X, Y, Width, Height);
            Viewports[Pointer] = this;
            this.Z = 0;
            this.ZoomX = 1.0;
            this.ZoomY = 1.0;
            this.Rect = new Rect(Pointer, X, Y, Width, Height);
        }

        public Viewport(Rect Rect)
        {
            this.Pointer = Internal.rb_funcallv(ClassPointer, Internal.rb_intern("allocate"), 0);
            ViewportObject = new ODL.Viewport(Program.MainWindow.Renderer, Rect.X, Rect.Y, Rect.Width, Rect.Height);
            Viewports[Pointer] = this;
            this.Z = 0;
            this.ZoomX = 1.0;
            this.ZoomY = 1.0;
            this.Rect = Rect;
        }

        public Viewport()
        {
            this.Pointer = Internal.rb_funcallv(ClassPointer, Internal.rb_intern("allocate"), 0);
            ViewportObject = new ODL.Viewport(Program.MainWindow.Renderer, 0, 0, Graphics.Width, Graphics.Height);
            Viewports[Pointer] = this;
            this.Z = 0;
            this.ZoomX = 1.0;
            this.ZoomY = 1.0;
            this.Rect = new Rect(0, 0, Graphics.Width, Graphics.Height);
        }

        public Rect Rect
        {
            get
            {
                IntPtr ptr = Internal.rb_funcallv(Pointer, Internal.rb_intern("rect"), 0, IntPtr.Zero);
                if (ptr == Internal.QNil) return null;
                return Rect.Rects[ptr];
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("rect="), 1, value.Pointer);
            }
        }
        public int Z
        {
            get
            {
                return (int) Internal.NUM2LONG(Internal.rb_funcallv(Pointer, Internal.rb_intern("z"), 0, IntPtr.Zero));
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("z="), 1, Internal.LONG2NUM(value));
            }
        }
        public double ZoomX
        {
            get
            {
                return Internal.rb_num2dbl(Internal.rb_funcallv(Pointer, Internal.rb_intern("zoom_x"), 0, IntPtr.Zero));
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("zoom_x="), 1, Internal.rb_float_new(value));
            }
        }
        public double ZoomY
        {
            get
            {
                return Internal.rb_num2dbl(Internal.rb_funcallv(Pointer, Internal.rb_intern("zoom_y"), 0, IntPtr.Zero));
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("zoom_y="), 1, Internal.rb_float_new(value));
            }
        }

        static IntPtr New(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);

            Viewport v = null;
            if (Args.Length == 4)
            {
                int x = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[0].Pointer, Internal.rb_intern("to_i"), 0, IntPtr.Zero));
                int y = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[1].Pointer, Internal.rb_intern("to_i"), 0, IntPtr.Zero));
                int w = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[2].Pointer, Internal.rb_intern("to_i"), 0, IntPtr.Zero));
                int h = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[3].Pointer, Internal.rb_intern("to_i"), 0, IntPtr.Zero));
                v = new Viewport(x, y, w, h);
            }
            else if (Args.Length == 1)
            {
                v = new Viewport(Rect.Rects[Args[0].Pointer]);
            }
            else if (Args.Length == 0)
            {
                v = new Viewport(0, 0, Graphics.Width, Graphics.Height);
            }
            else
            {
                ScanArgs(4, Args);
            }

            return v.Pointer;
        }

        static IntPtr rectget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@rect"));
        }

        static IntPtr rectset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            if (Viewports[_self].Rect != null)
                Viewports[_self].Rect.ViewportPointer = IntPtr.Zero;
            IntPtr Value = Internal.rb_ivar_set(_self, Internal.rb_intern("@rect"), Args[0].Pointer);
            Rect.Rects[Args[0].Pointer].ViewportPointer = _self;
            Rect.Rects[Args[0].Pointer].X = Viewports[_self].Rect.X;
            Rect.Rects[Args[0].Pointer].Y = Viewports[_self].Rect.Y;
            Rect.Rects[Args[0].Pointer].Width = Viewports[_self].Rect.Width;
            Rect.Rects[Args[0].Pointer].Height = Viewports[_self].Rect.Height;
            return Value;
        }

        static IntPtr zget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@z"));
        }

        static IntPtr zset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            Viewports[_self].ViewportObject.Z = (int) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@z"), Args[0].Pointer);
        }

        static IntPtr zoom_xget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@zoom_x"));
        }

        static IntPtr zoom_xset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            Viewports[_self].ViewportObject.ZoomX = Internal.rb_num2dbl(Args[0].Pointer);
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@zoom_x"), Args[0].Pointer);
        }

        static IntPtr zoom_yget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@zoom_y"));
        }

        static IntPtr zoom_yset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            Viewports[_self].ViewportObject.ZoomY = Internal.rb_num2dbl(Args[0].Pointer);
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@zoom_y"), Args[0].Pointer);
        }
    }
}

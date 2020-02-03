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
            c.DefineMethod("initialize", initialize);
            c.DefineMethod("rect", rectget);
            c.DefineMethod("rect=", rectset);
            c.DefineMethod("color", colorget);
            c.DefineMethod("color=", colorset);
            c.DefineMethod("z", zget);
            c.DefineMethod("z=", zset);
            c.DefineMethod("zoom_x", zoom_xget);
            c.DefineMethod("zoom_x=", zoom_xset);
            c.DefineMethod("zoom_y", zoom_yget);
            c.DefineMethod("zoom_y=", zoom_yset);
            c.DefineMethod("visible", visibleget);
            c.DefineMethod("visible=", visibleset);
            c.DefineMethod("ox", oxget);
            c.DefineMethod("ox=", oxset);
            c.DefineMethod("oy", oyget);
            c.DefineMethod("oy=", oyset);
            c.DefineMethod("dispose", dispose);
            c.DefineMethod("disposed?", disposed);
            return c;
        }

        private Viewport()
        {
            this.Pointer = Internal.rb_funcallv(ClassPointer, Internal.rb_intern("allocate"), 0);
            Viewports[Pointer] = this;
        }

        public static Viewport New(int X, int Y, int Width, int Height)
        {
            IntPtr ptr = Internal.rb_funcallv(ClassPointer, Internal.rb_intern("new"), 4,
                Internal.LONG2NUM(X),
                Internal.LONG2NUM(Y),
                Internal.LONG2NUM(Width),
                Internal.LONG2NUM(Height)
            );
            return Viewports[ptr];
        }
        public void Initialize(int X, int Y, int Width, int Height)
        {
            ViewportObject = new ODL.Viewport(Program.MainWindow.Renderer, X, Y, Width, Height);
            this.Z = 0;
            this.ZoomX = 1.0;
            this.ZoomY = 1.0;
            this.Rect = Rect.New(X, Y, Width, Height);
            this.Rect.ViewportPointer = Pointer;
            this.Color = Color.New(255, 255, 255);
            ViewportObject.Color = Color.ColorObject;
            this.Visible = true;
            this.OX = 0;
            this.OY = 0;
        }

        public static Viewport New(Rect r)
        {
            IntPtr ptr = Internal.rb_funcallv(ClassPointer, Internal.rb_intern("new"), 1, r.Pointer);
            return Viewports[ptr];
        }
        public void Initialize(Rect Rect)
        {
            ViewportObject = new ODL.Viewport(Program.MainWindow.Renderer, Rect.X, Rect.Y, Rect.Width, Rect.Height);
            this.Z = 0;
            this.ZoomX = 1.0;
            this.ZoomY = 1.0;
            this.Rect = Rect;
            this.Color = Color.New(255, 255, 255);
            ViewportObject.Color = Color.ColorObject;
            this.Visible = true;
            this.OX = 0;
            this.OY = 0;
        }

        public static Viewport New()
        {
            IntPtr ptr = Internal.rb_funcallv(ClassPointer, Internal.rb_intern("new"), 0);
            return Viewports[ptr];
        }
        public void Initialize()
        {
            ViewportObject = new ODL.Viewport(Program.MainWindow.Renderer, 0, 0, Graphics.Width, Graphics.Height);
            this.Z = 0;
            this.ZoomX = 1.0;
            this.ZoomY = 1.0;
            this.Rect = Rect.New(0, 0, Graphics.Width, Graphics.Height);
            this.Color = Color.New(255, 255, 255);
            ViewportObject.Color = Color.ColorObject;
            this.Visible = true;
            this.OX = 0;
            this.OY = 0;
        }

        public Rect Rect
        {
            get
            {
                IntPtr ptr = Internal.rb_funcallv(Pointer, Internal.rb_intern("rect"), 0);
                if (ptr == Internal.QNil) return null;
                return Rect.Rects[ptr];
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("rect="), 1, value.Pointer);
            }
        }
        public Color Color
        {
            get
            {
                IntPtr ptr = Internal.rb_funcallv(Pointer, Internal.rb_intern("color"), 0);
                if (ptr == Internal.QNil) return null;
                return Color.Colors[ptr];
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("color="), 1, value.Pointer);
            }
        }
        public int Z
        {
            get
            {
                return (int) Internal.NUM2LONG(Internal.rb_funcallv(Pointer, Internal.rb_intern("z"), 0));
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
                return Internal.rb_num2dbl(Internal.rb_funcallv(Pointer, Internal.rb_intern("zoom_x"), 0));
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
                return Internal.rb_num2dbl(Internal.rb_funcallv(Pointer, Internal.rb_intern("zoom_y"), 0));
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("zoom_y="), 1, Internal.rb_float_new(value));
            }
        }
        public bool Visible
        {
            get
            {
                return Internal.rb_funcallv(Pointer, Internal.rb_intern("visible"), 0) == Internal.QTrue;
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("visible="), 1, value ? Internal.QTrue : Internal.QFalse);
            }
        }
        public int OX
        {
            get
            {
                return (int) Internal.NUM2LONG(Internal.rb_funcallv(Pointer, Internal.rb_intern("ox"), 0));
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("ox="), 1, Internal.LONG2NUM(value));
            }
        }
        public int OY
        {
            get
            {
                return (int) Internal.NUM2LONG(Internal.rb_funcallv(Pointer, Internal.rb_intern("oy"), 0));
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("oy="), 1, Internal.LONG2NUM(value));
            }
        }
        public void Dispose()
        {
            Internal.rb_funcallv(Pointer, Internal.rb_intern("dispose"), 0);
        }
        public bool Disposed
        {
            get
            {
                return Internal.rb_funcallv(Pointer, Internal.rb_intern("disposed?"), 0) == Internal.QTrue;
            }
        }

        static IntPtr New(IntPtr _self, IntPtr _args)
        {
            Viewport v = new Viewport();
            RubyArray Args = new RubyArray(_args);
            IntPtr[] newargs = new IntPtr[Args.Length];
            for (int i = 0; i < Args.Length; i++) newargs[i] = Args[i].Pointer;
            Internal.rb_funcallv(v.Pointer, Internal.rb_intern("initialize"), Args.Length, newargs);
            return v.Pointer;
        }
        static IntPtr initialize(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);

            Viewport v = Viewport.Viewports[_self];
            if (Args.Length == 4)
            {
                int x = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[0].Pointer, Internal.rb_intern("to_i"), 0, IntPtr.Zero));
                int y = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[1].Pointer, Internal.rb_intern("to_i"), 0, IntPtr.Zero));
                int w = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[2].Pointer, Internal.rb_intern("to_i"), 0, IntPtr.Zero));
                int h = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[3].Pointer, Internal.rb_intern("to_i"), 0, IntPtr.Zero));
                v.Initialize(x, y, w, h);
            }
            else if (Args.Length == 1)
            {
                v.Initialize(Rect.Rects[Args[0].Pointer]);
            }
            else if (Args.Length == 0)
            {
                v.Initialize(0, 0, Graphics.Width, Graphics.Height);
            }
            else
            {
                ScanArgs(4, Args);
            }
            return _self;
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

        static IntPtr colorget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@color"));
        }
        static IntPtr colorset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            IntPtr Value = Internal.rb_ivar_set(_self, Internal.rb_intern("@color"), Args[0].Pointer);
            Viewports[_self].ViewportObject.Color = Color.Colors[Args[0].Pointer].ColorObject;
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

        static IntPtr visibleget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@visible"));
        }
        static IntPtr visibleset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            Viewports[_self].ViewportObject.Visible = Args[0].Pointer == Internal.QTrue;
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@visible"), Args[0].Pointer);
        }

        static IntPtr oxget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@ox"));
        }
        static IntPtr oxset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            Viewports[_self].ViewportObject.OX = (int) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@ox"), Args[0].Pointer);
        }

        static IntPtr oyget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@oy"));
        }
        static IntPtr oyset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            Viewports[_self].ViewportObject.OY = (int) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@oy"), Args[0].Pointer);
        }

        static IntPtr dispose(IntPtr _self, IntPtr _args)
        {
            Viewport.Viewports[_self].ViewportObject.Dispose();
            return _self;
        }

        static IntPtr disposed(IntPtr _self, IntPtr _args)
        {
            return Viewport.Viewports[_self].ViewportObject.Disposed ? Internal.QTrue : Internal.QFalse;
        }
    }
}

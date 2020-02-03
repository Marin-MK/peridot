using System;
using System.Collections.Generic;
using RubyDotNET;

namespace odlgss
{
    public class Sprite : RubyObject
    {
        public ODL.Sprite SpriteObject;
        public static IntPtr ClassPointer;
        public static Dictionary<IntPtr, Sprite> Sprites = new Dictionary<IntPtr, Sprite>();

        public static Class CreateClass()
        {
            Class c = new Class("Sprite");
            ClassPointer = c.Pointer;
            c.DefineClassMethod("new", New);
            c.DefineMethod("initialize", initialize);
            c.DefineMethod("viewport", viewportget);
            c.DefineMethod("viewport=", viewportset);
            c.DefineMethod("bitmap", bitmapget);
            c.DefineMethod("bitmap=", bitmapset);
            c.DefineMethod("src_rect", src_rectget);
            c.DefineMethod("src_rect=", src_rectset);
            c.DefineMethod("x", xget);
            c.DefineMethod("x=", xset);
            c.DefineMethod("y", yget);
            c.DefineMethod("y=", yset);
            c.DefineMethod("z", zget);
            c.DefineMethod("z=", zset);
            c.DefineMethod("ox", oxget);
            c.DefineMethod("ox=", oxset);
            c.DefineMethod("oy", oyget);
            c.DefineMethod("oy=", oyset);
            c.DefineMethod("zoom_x", zoom_xget);
            c.DefineMethod("zoom_x=", zoom_xset);
            c.DefineMethod("zoom_y", zoom_yget);
            c.DefineMethod("zoom_y=", zoom_yset);
            c.DefineMethod("opacity", opacityget);
            c.DefineMethod("opacity=", opacityset);
            c.DefineMethod("angle", angleget);
            c.DefineMethod("angle=", angleset);
            c.DefineMethod("mirror", mirrorget);
            c.DefineMethod("mirror=", mirrorset);
            c.DefineMethod("color", colorget);
            c.DefineMethod("color=", colorset);
            c.DefineMethod("tone", toneget);
            c.DefineMethod("tone=", toneset);
            c.DefineMethod("visible", visibleget);
            c.DefineMethod("visible=", visibleset);
            c.DefineMethod("update", update);
            c.DefineMethod("dispose", dispose);
            c.DefineMethod("disposed?", disposed);
            return c;
        }

        private Sprite()
        {
            this.Pointer = Internal.rb_funcallv(ClassPointer, Internal.rb_intern("allocate"), 0);
            Sprites[Pointer] = this;
        }

        public static Sprite New(Viewport Viewport)
        {
            IntPtr ptr = Internal.rb_funcallv(ClassPointer, Internal.rb_intern("new"), 1, Viewport.Pointer);
            return Sprites[ptr];
        }
        public void Initialize(Viewport Viewport)
        {
            SpriteObject = new ODL.Sprite(Viewport.ViewportObject);
            this.Viewport = Viewport;
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
            this.OX = 0;
            this.OY = 0;
            this.ZoomX = 1.0;
            this.ZoomY = 1.0;
            this.Opacity = 255;
            this.Angle = 0;
            this.Mirror = false;
            this.Color = Color.New(255, 255, 255);
            this.Tone = Tone.New(0, 0, 0, 0);
            this.Visible = true;
        }

        public static Sprite New()
        {
            return Sprite.New(Graphics.MainViewport);
        }
        public void Initialize()
        {
            this.Initialize(Graphics.MainViewport);
        }

        public Viewport Viewport
        {
            get
            {
                IntPtr ptr = Internal.rb_funcallv(Pointer, Internal.rb_intern("viewport"), 0);
                if (ptr == Internal.QNil) return null;
                return Viewport.Viewports[ptr];
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("viewport="), 1, value == null ? Internal.QNil : value.Pointer);
            }
        }
        public Bitmap Bitmap
        {
            get
            {
                IntPtr ptr = Internal.rb_funcallv(Pointer, Internal.rb_intern("bitmap"), 0);
                if (ptr == Internal.QNil) return null;
                return Bitmap.Bitmaps[ptr];
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("bitmap="), 1, value == null ? Internal.QNil : value.Pointer);
            }
        }
        public Rect SrcRect
        {
            get
            {
                IntPtr ptr = Internal.rb_funcallv(Pointer, Internal.rb_intern("src_rect"), 0);
                if (ptr == Internal.QNil) return null;
                return Rect.Rects[ptr];
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("src_rect="), 1, value.Pointer);
            }
        }
        public int X
        {
            get
            {
                return (int) Internal.NUM2LONG(Internal.rb_funcallv(Pointer, Internal.rb_intern("x"), 0));
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
                return (int) Internal.NUM2LONG(Internal.rb_funcallv(Pointer, Internal.rb_intern("y"), 0));
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("y="), 1, Internal.LONG2NUM(value));
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
        public byte Opacity
        {
            get
            {
                return (byte) Internal.NUM2LONG(Internal.rb_funcallv(Pointer, Internal.rb_intern("opacity"), 0));
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("opacity="), 1, Internal.LONG2NUM(value));
            }
        }
        public int Angle
        {
            get
            {
                return (int) Internal.NUM2LONG(Internal.rb_funcallv(Pointer, Internal.rb_intern("angle"), 0));
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("angle="), 1, Internal.LONG2NUM(value));
            }
        }
        public bool Mirror
        {
            get
            {
                return Internal.rb_funcallv(Pointer, Internal.rb_intern("mirror"), 0) == Internal.QTrue;
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("mirror="), 1, value ? Internal.QTrue : Internal.QFalse);
            }
        }
        public Color Color
        {
            get
            {
                IntPtr ptr = Internal.rb_funcallv(Pointer, Internal.rb_intern("color"), 0);
                return Color.Colors[ptr];
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("color="), 1, value.Pointer);
            }
        }
        public Tone Tone
        {
            get
            {
                IntPtr ptr = Internal.rb_funcallv(Pointer, Internal.rb_intern("tone"), 0);
                return Tone.Tones[ptr];
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("tone="), 1, value.Pointer);
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
        public void Update()
        {
            Internal.rb_funcallv(Pointer, Internal.rb_intern("update"), 0);
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
            Sprite s = new Sprite();
            RubyArray Args = new RubyArray(_args);
            IntPtr[] newargs = new IntPtr[Args.Length];
            for (int i = 0; i < Args.Length; i++) newargs[i] = Args[i].Pointer;
            Internal.rb_funcallv(s.Pointer, Internal.rb_intern("initialize"), Args.Length, newargs);
            return s.Pointer;
        }
        static IntPtr initialize(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);

            Sprite s = Sprites[_self];
            if (Args.Length == 0) // Main viewport
            {
                s.Initialize();
            }
            else if (Args.Length == 1)
            {
                s.Initialize(Viewport.Viewports[Args[0].Pointer]);
            }
            else
            {
                ScanArgs(1, Args);
            }

            return s.Pointer;
        }

        static IntPtr viewportget(IntPtr _self, IntPtr _args)
        {
            IntPtr ptr = Internal.rb_ivar_get(_self, Internal.rb_intern("@viewport"));
            if (ptr == Graphics.MainViewport.Pointer) return Internal.QNil;
            return ptr;
        }
        static IntPtr viewportset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            IntPtr vp = Args[0].Pointer;
            if (vp == Internal.QNil)
                vp = Graphics.MainViewport.Pointer;
            IntPtr Value = Internal.rb_ivar_set(_self, Internal.rb_intern("@viewport"), vp);
            return Value;
        }

        static IntPtr bitmapget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@bitmap"));
        }
        static IntPtr bitmapset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            IntPtr Value = Internal.rb_ivar_set(_self, Internal.rb_intern("@bitmap"), Args[0].Pointer);
            if (Args[0].Pointer != Internal.QNil)
            {
                Sprite.Sprites[_self].SpriteObject.Bitmap = Bitmap.Bitmaps[Args[0].Pointer].BitmapObject;
                Sprite.Sprites[_self].SrcRect = Rect.New(0, 0, Bitmap.Bitmaps[Args[0].Pointer].Width, Bitmap.Bitmaps[Args[0].Pointer].Height);
            }
            return Value;
        }

        static IntPtr src_rectget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@src_rect"));
        }
        static IntPtr src_rectset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            IntPtr Value = Internal.rb_ivar_set(_self, Internal.rb_intern("@src_rect"), Args[0].Pointer);
            Sprite.Sprites[_self].SpriteObject.SrcRect = Rect.Rects[Args[0].Pointer].RectObject;
            return Value;
        }

        static IntPtr xget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@x"));
        }
        static IntPtr xset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            Sprite.Sprites[_self].SpriteObject.X = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[0].Pointer, Internal.rb_intern("to_i"), 0));
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
            Sprite.Sprites[_self].SpriteObject.Y = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[0].Pointer, Internal.rb_intern("to_i"), 0));
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@y"), Args[0].Pointer);
        }

        static IntPtr zget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@z"));
        }
        static IntPtr zset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            Sprite.Sprites[_self].SpriteObject.Z = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[0].Pointer, Internal.rb_intern("to_i"), 0));
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@z"), Args[0].Pointer);
        }

        static IntPtr oxget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@ox"));
        }
        static IntPtr oxset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            Sprite.Sprites[_self].SpriteObject.OX = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[0].Pointer, Internal.rb_intern("to_i"), 0));
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
            Sprite.Sprites[_self].SpriteObject.OY = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[0].Pointer, Internal.rb_intern("to_i"), 0));
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@oy"), Args[0].Pointer);
        }

        static IntPtr zoom_xget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@zoom_x"));
        }
        static IntPtr zoom_xset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            Sprite.Sprites[_self].SpriteObject.ZoomX = Internal.rb_num2dbl(Internal.rb_funcallv(Args[0].Pointer, Internal.rb_intern("to_f"), 0));
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
            Sprite.Sprites[_self].SpriteObject.ZoomY = Internal.rb_num2dbl(Internal.rb_funcallv(Args[0].Pointer, Internal.rb_intern("to_f"), 0));
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@zoom_y"), Args[0].Pointer);
        }

        static IntPtr opacityget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@opacity"));
        }
        static IntPtr opacityset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            Sprite.Sprites[_self].SpriteObject.Opacity = (byte) Internal.NUM2LONG(Internal.rb_funcallv(Args[0].Pointer, Internal.rb_intern("to_i"), 0));
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@opacity"), Args[0].Pointer);
        }

        static IntPtr angleget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@angle"));
        }
        static IntPtr angleset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            Sprite.Sprites[_self].SpriteObject.Angle = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[0].Pointer, Internal.rb_intern("to_i"), 0));
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@angle"), Args[0].Pointer);
        }

        static IntPtr mirrorget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@mirror"));
        }
        static IntPtr mirrorset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            Sprite.Sprites[_self].SpriteObject.MirrorX = Args[0].Pointer == Internal.QTrue;
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@mirror"), Args[0].Pointer);
        }

        static IntPtr colorget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@color"));
        }
        static IntPtr colorset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            Sprite.Sprites[_self].SpriteObject.Color = Color.Colors[Args[0].Pointer].ColorObject;
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@color"), Args[0].Pointer);
        }

        static IntPtr toneget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@tone"));
        }
        static IntPtr toneset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            Sprite.Sprites[_self].SpriteObject.Tone = Tone.Tones[Args[0].Pointer].ToneObject;
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@tone"), Args[0].Pointer);
        }

        static IntPtr visibleget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@visible"));
        }
        static IntPtr visibleset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            Sprite.Sprites[_self].SpriteObject.Visible = Args[0].Pointer == Internal.QTrue;
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@visible"), Args[0].Pointer);
        }

        static IntPtr update(IntPtr _self, IntPtr _args)
        {
            return _self;
        }

        static IntPtr dispose(IntPtr _self, IntPtr _args)
        {
            ODL.Bitmap bmp = null;
            if (Sprite.Sprites[_self].SpriteObject.Bitmap != null)
                bmp = Sprite.Sprites[_self].SpriteObject.Bitmap;
            Sprite.Sprites[_self].SpriteObject.Bitmap = null;
            // Bitmap should only be separated from the sprite; not disposed
            Sprite.Sprites[_self].SpriteObject.Dispose();
            Sprite.Sprites[_self].SpriteObject.Bitmap = bmp;
            return _self;
        }

        static IntPtr disposed(IntPtr _self, IntPtr _args)
        {
            return Sprite.Sprites[_self].SpriteObject.Disposed ? Internal.QTrue : Internal.QFalse;
        }
    }
}

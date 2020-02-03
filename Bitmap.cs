using System;
using System.Collections.Generic;
using RubyDotNET;

namespace odlgss
{
    public class Bitmap : RubyObject
    {
        public ODL.Bitmap BitmapObject;
        public static IntPtr ClassPointer;
        public static Dictionary<IntPtr, Bitmap> Bitmaps = new Dictionary<IntPtr, Bitmap>();

        public static Class CreateClass()
        {
            Class c = new Class("Bitmap");
            ClassPointer = c.Pointer;
            c.DefineClassMethod("new", New);
            c.DefineMethod("initialize", initialize);
            c.DefineMethod("width", width);
            c.DefineMethod("height", height);
            c.DefineMethod("rect", rect);
            c.DefineMethod("font", fontget);
            c.DefineMethod("font=", fontset);
            c.DefineMethod("get_pixel", get_pixel);
            c.DefineMethod("set_pixel", set_pixel);
            c.DefineMethod("fill_rect", fill_rect);
            c.DefineMethod("draw_text", draw_text);
            c.DefineMethod("text_size", text_size);
            c.DefineMethod("clear", clear);
            c.DefineMethod("dispose", dispose);
            c.DefineMethod("disposed?", disposed);
            return c;
        }

        private Bitmap()
        {
            this.Pointer = Internal.rb_funcallv(ClassPointer, Internal.rb_intern("allocate"), 0);
            Bitmaps[Pointer] = this;
        }

        public static Bitmap New(string File)
        {
            IntPtr ptr = Internal.rb_funcallv(ClassPointer, Internal.rb_intern("new"), 1, Internal.rb_str_new_cstr(File));
            return Bitmaps[ptr];
        }
        public void Initialize(string File)
        {
            BitmapObject = new ODL.Bitmap(File);
            Internal.rb_ivar_set(Pointer, Internal.rb_intern("@width"), Internal.LONG2NUM(BitmapObject.Width));
            Internal.rb_ivar_set(Pointer, Internal.rb_intern("@height"), Internal.LONG2NUM(BitmapObject.Height));
            this.Font = Font.New();
            BitmapObject.Font = this.Font.FontObject;
        }

        public static Bitmap New(int Width, int Height)
        {
            IntPtr ptr = Internal.rb_funcallv(ClassPointer, Internal.rb_intern("new"), 2,
                Internal.LONG2NUM(Width),
                Internal.LONG2NUM(Height)
            );
            return Bitmaps[ptr];
        }
        public void Initialize(int Width, int Height)
        {
            BitmapObject = new ODL.Bitmap(Width, Height);
            Internal.rb_ivar_set(Pointer, Internal.rb_intern("@width"), Internal.LONG2NUM(Width));
            Internal.rb_ivar_set(Pointer, Internal.rb_intern("@height"), Internal.LONG2NUM(Height));
            this.Font = Font.New();
            BitmapObject.Font = this.Font.FontObject;
        }

        public int Width
        {
            get
            {
                return (int) Internal.NUM2LONG(Internal.rb_funcallv(Pointer, Internal.rb_intern("width"), 0));
            }
        }
        public int Height
        {
            get
            {
                return (int) Internal.NUM2LONG(Internal.rb_funcallv(Pointer, Internal.rb_intern("height"), 0));
            }
        }
        public Font Font
        { 
            get
            {
                IntPtr ptr = Internal.rb_funcallv(Pointer, Internal.rb_intern("font"), 0);
                if (ptr == Internal.QNil) return null;
                return Font.Fonts[ptr];
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("font="), 1, value.Pointer);
            }
        }
        public Color GetPixel(int X, int Y)
        {
            IntPtr ptr = Internal.rb_funcallv(Pointer, Internal.rb_intern("get_pixel"), 2, Internal.LONG2NUM(X), Internal.LONG2NUM(Y));
            return Color.Colors[ptr];
        }
        public void SetPixel(int X, int Y, Color c)
        {
            Internal.rb_funcallv(Pointer, Internal.rb_intern("set_pixel"), 3, Internal.LONG2NUM(X), Internal.LONG2NUM(Y), c.Pointer);
        }
        public void FillRect(Rect r, Color c)
        {
            Internal.rb_funcallv(Pointer, Internal.rb_intern("fill_rect"), 2, r.Pointer, c.Pointer);
        }
        public void FillRect(int X, int Y, int Width, int Height, Color c)
        {
            Internal.rb_funcallv(Pointer, Internal.rb_intern("fill_rect"), 5, 
                Internal.LONG2NUM(X), Internal.LONG2NUM(Y), Internal.LONG2NUM(Width), Internal.LONG2NUM(Height), c.Pointer);
        }
        public void DrawText(Rect r, string Text, int Align = 0)
        {
            Internal.rb_funcallv(Pointer, Internal.rb_intern("draw_text"), 6,
                r.Pointer,
                Internal.rb_str_new_cstr(Text),
                Internal.LONG2NUM(Align)
            );
        }
        public void DrawText(int X, int Y, int Width, int Height, string Text, int Align = 0)
        {
            Internal.rb_funcallv(Pointer, Internal.rb_intern("draw_text"), 6,
                Internal.LONG2NUM(X),
                Internal.LONG2NUM(Y),
                Internal.LONG2NUM(Width),
                Internal.LONG2NUM(Height),
                Internal.rb_str_new_cstr(Text),
                Internal.LONG2NUM(Align)
            );
        }
        public Rect TextSize(string Text)
        {
            IntPtr ptr = Internal.rb_funcallv(Pointer, Internal.rb_intern("text_size"), 1, Internal.rb_str_new_cstr(Text));
            return Rect.Rects[ptr];
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
            Bitmap b = new Bitmap();
            RubyArray Args = new RubyArray(_args);
            IntPtr[] newargs = new IntPtr[Args.Length];
            for (int i = 0; i < Args.Length; i++) newargs[i] = Args[i].Pointer;
            Internal.rb_funcallv(b.Pointer, Internal.rb_intern("initialize"), Args.Length, newargs);
            return b.Pointer;
        }
        static IntPtr initialize(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);

            Bitmap b = Bitmaps[_self];
            if (Args.Length == 1) // File
            {
                string file = new RubyString(Args[0].Pointer).ToString();
                b.Initialize(file);
            }
            else if (Args.Length == 2)
            {
                int w = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[0].Pointer, Internal.rb_intern("to_i"), 0, IntPtr.Zero));
                int h = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[1].Pointer, Internal.rb_intern("to_i"), 0, IntPtr.Zero));
                b.Initialize(w, h);
            }
            else
            {
                ScanArgs(1, Args);
            }

            return _self;
        }

        static IntPtr width(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@width"));
        }

        static IntPtr height(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@height"));
        }

        static IntPtr rect(IntPtr _self, IntPtr _args)
        {
            return Rect.New(0, 0, Bitmap.Bitmaps[_self].Width, Bitmap.Bitmaps[_self].Height).Pointer;
        }

        static IntPtr fontget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@font"));
        }
        static IntPtr fontset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            IntPtr Value = Internal.rb_ivar_set(_self, Internal.rb_intern("@font"), Args[0].Pointer);
            Bitmap.Bitmaps[_self].BitmapObject.Font = Font.Fonts[Args[0].Pointer].FontObject;
            return Value;
        }

        static IntPtr get_pixel(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(2, Args);
            int x = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[0].Pointer, Internal.rb_intern("to_i"), 0));
            int y = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[1].Pointer, Internal.rb_intern("to_i"), 0));
            ODL.Color c = Bitmap.Bitmaps[_self].BitmapObject.GetPixel(x, y);
            return Color.New(c.Red, c.Green, c.Blue, c.Alpha).Pointer;
        }

        static IntPtr set_pixel(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(3, Args);
            int x = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[0].Pointer, Internal.rb_intern("to_i"), 0));
            int y = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[1].Pointer, Internal.rb_intern("to_i"), 0));
            Color c = Color.Colors[Args[2].Pointer];
            if (Config.AutoLock) Bitmap.Bitmaps[_self].BitmapObject.Unlock();
            Bitmap.Bitmaps[_self].BitmapObject.SetPixel(x, y, c.Red, c.Green, c.Blue, c.Alpha);
            if (Config.AutoLock) Bitmap.Bitmaps[_self].BitmapObject.Lock();
            return _self;
        }

        static IntPtr fill_rect(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            int x = 0,
                y = 0,
                w = 0,
                h = 0;
            Color c = null;
            if (Args.Length == 2)
            {
                Rect r = Rect.Rects[Args[0].Pointer];
                x = r.X;
                y = r.Y;
                w = r.Width;
                h = r.Height;
                c = Color.Colors[Args[1].Pointer];
            }
            else if (Args.Length == 5)
            {
                x = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[0].Pointer, Internal.rb_intern("to_i"), 0));
                y = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[1].Pointer, Internal.rb_intern("to_i"), 0));
                w = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[2].Pointer, Internal.rb_intern("to_i"), 0));
                h = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[3].Pointer, Internal.rb_intern("to_i"), 0));
                c = Color.Colors[Args[4].Pointer];
            }
            else
            {
                ScanArgs(5, Args);
            }
            if (Config.AutoLock) Bitmap.Bitmaps[_self].BitmapObject.Unlock();
            Bitmap.Bitmaps[_self].BitmapObject.FillRect(x, y, w, h, c.Red, c.Green, c.Blue, c.Alpha);
            if (Config.AutoLock) Bitmap.Bitmaps[_self].BitmapObject.Lock();
            return _self;
        }

        static IntPtr draw_text(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            int x = 0,
                y = 0,
                w = 0,
                h = 0;
            string txt = null;
            int align = 0;
            if (Args.Length == 2 || Args.Length == 3)
            {
                Rect r = Rect.Rects[Args[0].Pointer];
                x = r.X;
                y = r.Y;
                w = r.Width;
                h = r.Height;
                txt = new RubyString(Args[1].Pointer).ToString();
                if (Args.Length == 3) align = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[2].Pointer, Internal.rb_intern("to_i"), 0));
            }
            else if (Args.Length == 5 || Args.Length == 6)
            {
                x = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[0].Pointer, Internal.rb_intern("to_i"), 0));
                y = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[1].Pointer, Internal.rb_intern("to_i"), 0));
                w = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[2].Pointer, Internal.rb_intern("to_i"), 0));
                h = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[3].Pointer, Internal.rb_intern("to_i"), 0));
                txt = new RubyString(Args[4].Pointer).ToString();
                if (Args.Length == 6) align = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[5].Pointer, Internal.rb_intern("to_i"), 0));
            }
            else
            {
                ScanArgs(5, Args);
            }

            ODL.DrawOptions Options = ODL.DrawOptions.LeftAlign;
            if (align == 1) Options = ODL.DrawOptions.CenterAlign;
            else if (align == 2) Options = ODL.DrawOptions.RightAlign;
            if (Bitmap.Bitmaps[_self].Font.Bold) Options |= ODL.DrawOptions.Bold;
            if (Bitmap.Bitmaps[_self].Font.Italic) Options |= ODL.DrawOptions.Italic;

            if (Config.AutoLock) Bitmap.Bitmaps[_self].BitmapObject.Unlock();
            Bitmap.Bitmaps[_self].BitmapObject.DrawText(txt, x, y, w, h, Bitmap.Bitmaps[_self].Font.Color.ColorObject, Options);
            if (Config.AutoLock) Bitmap.Bitmaps[_self].BitmapObject.Lock();

            return _self;
        }

        static IntPtr text_size(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);

            ODL.DrawOptions Options = 0;
            if (Bitmap.Bitmaps[_self].Font.Bold) Options |= ODL.DrawOptions.Bold;
            if (Bitmap.Bitmaps[_self].Font.Italic) Options |= ODL.DrawOptions.Italic;
            ODL.Size s = Bitmap.Bitmaps[_self].BitmapObject.TextSize(new RubyString(Args[0].Pointer).ToString(), Options);
            return Rect.New(0, 0, s.Width, s.Height).Pointer;
        }

        static IntPtr clear(IntPtr _self, IntPtr _args)
        {
            if (Config.AutoLock) Bitmap.Bitmaps[_self].BitmapObject.Unlock();
            Bitmap.Bitmaps[_self].BitmapObject.Clear();
            if (Config.AutoLock) Bitmap.Bitmaps[_self].BitmapObject.Lock();
            return _self;
        }

        static IntPtr dispose(IntPtr _self, IntPtr _args)
        {
            if (Config.AutoLock) Bitmap.Bitmaps[_self].BitmapObject.Unlock();
            Bitmap.Bitmaps[_self].BitmapObject.Dispose();
            if (Config.AutoLock) Bitmap.Bitmaps[_self].BitmapObject.Lock();
            return _self;
        }

        static IntPtr disposed(IntPtr _self, IntPtr _args)
        {
            return Bitmap.Bitmaps[_self].BitmapObject.Disposed ? Internal.QTrue : Internal.QFalse;
        }
    }
}

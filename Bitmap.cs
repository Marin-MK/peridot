using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using RubyDotNET;

namespace Peridot
{
    public class Bitmap : RubyObject
    {
        public static IntPtr Class;
        public static Dictionary<IntPtr, ODL.Bitmap> BitmapDictionary = new Dictionary<IntPtr, ODL.Bitmap>();

        public static Class CreateClass()
        {
            Class c = new Class("Bitmap");
            Class = c.Pointer;
            c.DefineClassMethod("new", _new);
            c.DefineMethod("initialize", initialize);
            c.DefineMethod("width", widthget);
            c.DefineMethod("height", heightget);
            c.DefineMethod("font", fontget);
            c.DefineMethod("font=", fontset);
            c.DefineMethod("autolock", autolockget);
            c.DefineMethod("autolock=", autolockset);
            c.DefineMethod("unlock", unlockbmp);
            c.DefineMethod("lock", lockbmp);
            c.DefineMethod("get_pixel", get_pixel);
            c.DefineMethod("set_pixel", set_pixel);
            c.DefineMethod("draw_line", draw_line);
            c.DefineMethod("draw_rect", draw_rect);
            c.DefineMethod("fill_rect", fill_rect);
            c.DefineMethod("blt", blt);
            c.DefineMethod("text_size", text_size);
            c.DefineMethod("draw_text", draw_text);
            c.DefineMethod("clear", clear);
            c.DefineMethod("dispose", dispose);
            c.DefineMethod("disposed?", disposed);
            return c;
        }

        protected static IntPtr allocate(IntPtr Class)
        {
            return Internal.rb_funcallv(Class, Internal.rb_intern("allocate"), 0);
        }

        protected static IntPtr _new(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            IntPtr obj = allocate(self);
            IntPtr[] newargs = Args.Rubify();
            Internal.rb_funcallv(obj, Internal.rb_intern("initialize"), Args.Length, Args.Rubify());
            return obj;
        }

        protected static IntPtr initialize(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);

            ODL.Bitmap bmp = null;
            if (Args.Length == 1)
            {
                if (Internal.rb_funcallv(Args[0].Pointer, Internal.rb_intern("is_a?"), 1, new IntPtr[1] { Class }) == Internal.QTrue)
                {
                    bmp = Bitmap.BitmapDictionary[Args[0].Pointer];
                }
                else
                {
                    bmp = new ODL.Bitmap(new RubyString(Args[0].Pointer).ToString());
                }
            }
            else if (Args.Length == 2)
            {
                bmp = new ODL.Bitmap(
                    (int) Internal.NUM2LONG(Args[0].Pointer),
                    (int) Internal.NUM2LONG(Args[1].Pointer)
                );
            }
            else ScanArgs(1, Args);
            Internal.SetIVar(self, "@width", Internal.LONG2NUM(bmp.Width));
            Internal.SetIVar(self, "@height", Internal.LONG2NUM(bmp.Height));
            Internal.SetIVar(self, "@autolock", Internal.QTrue);
            Internal.SetIVar(self, "@font", Font.CreateFont());

            if (BitmapDictionary.ContainsKey(self)) dispose(self, IntPtr.Zero);
            BitmapDictionary.Add(self, bmp);
            return self;
        }

        protected static IntPtr AutoUnlock(IntPtr self)
        {
            if (autolockget(self, IntPtr.Zero) == Internal.QTrue)
            {
                BitmapDictionary[self].Unlock();
                return Internal.QTrue;
            }
            else if (BitmapDictionary[self].Locked)
            {
                Internal.rb_raise(Internal.rb_eRuntimeError.Pointer, "bitmap locked for writing");
            }
            return Internal.QFalse;
        }

        protected static IntPtr AutoLock(IntPtr self)
        {
            if (autolockget(self, IntPtr.Zero) == Internal.QTrue)
            {
                BitmapDictionary[self].Lock();
                return Internal.QTrue;
            }
            return Internal.QFalse;
        }

        protected static IntPtr widthget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@width");
        }

        protected static IntPtr heightget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@height");
        }

        protected static IntPtr fontget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@font");
        }

        protected static IntPtr fontset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.SetIVar(self, "@font", Args[0].Pointer);
        }

        protected static IntPtr autolockget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            if (_args != IntPtr.Zero)
            {
                RubyArray Args = new RubyArray(_args);
                ScanArgs(0, Args);
            }
            return Internal.GetIVar(self, "@autolock");
        }

        protected static IntPtr autolockset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.SetIVar(self, "@autolock", Args[0].Pointer);
        }

        protected static IntPtr unlockbmp(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            if (Internal.GetIVar(self, "@autolock") == Internal.QTrue) Internal.rb_raise(Internal.rb_eRuntimeError.Pointer, "manual unlocking disallowed with autolock enabled");
            else if (!BitmapDictionary[self].Locked) Internal.rb_raise(Internal.rb_eRuntimeError.Pointer, "bitmap already unlocked");
            BitmapDictionary[self].Unlock();
            return Internal.QTrue;
        }

        protected static IntPtr lockbmp(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            if (Internal.GetIVar(self, "@autolock") == Internal.QTrue) Internal.rb_raise(Internal.rb_eRuntimeError.Pointer, "manual locking disallowed with autolock enabled");
            else if (BitmapDictionary[self].Locked) Internal.rb_raise(Internal.rb_eRuntimeError.Pointer, "bitmap already locked");
            BitmapDictionary[self].Lock();
            return Internal.QTrue;
        }

        protected static IntPtr get_pixel(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(2, Args);
            int X = (int) Internal.NUM2LONG(Args[0].Pointer),
                Y = (int) Internal.NUM2LONG(Args[1].Pointer);
            return Color.CreateColor(BitmapDictionary[self].GetPixel(X, Y));
        }

        protected static IntPtr set_pixel(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(3, Args);
            int X = (int) Internal.NUM2LONG(Args[0].Pointer),
                Y = (int) Internal.NUM2LONG(Args[1].Pointer);
            AutoUnlock(self);
            BitmapDictionary[self].SetPixel(X, Y, Color.CreateColor(Args[2].Pointer));
            AutoLock(self);
            return Args[2].Pointer;
        }

        protected static IntPtr draw_line(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(5, Args);
            int x1 = (int) Internal.NUM2LONG(Args[0].Pointer),
                y1 = (int) Internal.NUM2LONG(Args[1].Pointer),
                x2 = (int) Internal.NUM2LONG(Args[2].Pointer),
                y2 = (int) Internal.NUM2LONG(Args[3].Pointer);
            AutoUnlock(self);
            ODL.Color c = Color.CreateColor(Args[4].Pointer);
            BitmapDictionary[self].DrawLine(x1, y1, x2, y2, c);
            AutoLock(self);
            return Internal.QTrue;
        }

        protected static IntPtr draw_rect(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(5, Args);
            int x = (int) Internal.NUM2LONG(Args[0].Pointer),
                y = (int) Internal.NUM2LONG(Args[1].Pointer),
                w = (int) Internal.NUM2LONG(Args[2].Pointer),
                h = (int) Internal.NUM2LONG(Args[3].Pointer);
            AutoUnlock(self);
            ODL.Color c = Color.CreateColor(Args[4].Pointer);
            BitmapDictionary[self].DrawRect(x, y, w, h, c);
            AutoLock(self);
            return Internal.QTrue;
        }

        protected static IntPtr fill_rect(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(5, Args);
            int x = (int) Internal.NUM2LONG(Args[0].Pointer),
                y = (int) Internal.NUM2LONG(Args[1].Pointer),
                w = (int) Internal.NUM2LONG(Args[2].Pointer),
                h = (int) Internal.NUM2LONG(Args[3].Pointer);
            AutoUnlock(self);
            ODL.Color c = Color.CreateColor(Args[4].Pointer);
            BitmapDictionary[self].FillRect(x, y, w, h, c);
            AutoLock(self);
            return Internal.QTrue;
        }

        protected static IntPtr blt(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(4, Args);
            int x = (int) Internal.NUM2LONG(Args[0].Pointer),
                y = (int) Internal.NUM2LONG(Args[1].Pointer);
            AutoUnlock(self);
            ODL.Bitmap srcbmp = BitmapDictionary[Args[2].Pointer];
            ODL.Rect srcrect = Rect.CreateRect(Args[3].Pointer);
            BitmapDictionary[self].Build(x, y, srcbmp, srcrect);
            AutoLock(self);
            return Internal.QTrue;
        }

        protected static IntPtr text_size(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            if (Args[0].Pointer == Internal.QNil) Internal.rb_raise(Internal.rb_eRuntimeError.Pointer, "nil in text_size");
            string text = new RubyString(Args[0].Pointer).ToString();
            BitmapDictionary[self].Font = Font.CreateFont(Internal.GetIVar(self, "@font"));
            return Rect.CreateRect(BitmapDictionary[self].TextSize(text));
        }

        protected static IntPtr draw_text(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            if (Args.Length != 5 && Args.Length != 6) ScanArgs(6, Args);
            int x = (int) Internal.NUM2LONG(Args[0].Pointer),
                y = (int) Internal.NUM2LONG(Args[1].Pointer),
                w = (int) Internal.NUM2LONG(Args[2].Pointer),
                h = (int) Internal.NUM2LONG(Args[3].Pointer);
            string text = new RubyString(Args[4].Pointer).ToString();
            int align = 0;
            if (Args.Length == 6) align = (int) Internal.NUM2LONG(Args[5].Pointer);
            AutoUnlock(self);
            BitmapDictionary[self].Font = Font.CreateFont(Internal.GetIVar(self, "@font"));
            ODL.Color color = Color.CreateColor(Internal.GetIVar(Internal.GetIVar(self, "@font"), "@color"));
            ODL.DrawOptions options = 0;
            bool outline = false;
            if (align == 0) options |= ODL.DrawOptions.LeftAlign;
            else if (align == 1) options |= ODL.DrawOptions.CenterAlign;
            else if (align == 2) options |= ODL.DrawOptions.RightAlign;
            if (Internal.GetIVar(Internal.GetIVar(self, "@font"), "@outline") == Internal.QTrue) outline = true;
            if (outline)
            {
                ODL.Color outline_color = Color.CreateColor(Internal.GetIVar(Internal.GetIVar(self, "@font"), "@outline_color"));
                BitmapDictionary[self].DrawText(text, new ODL.Rect(x - 1, y - 1, w, h), outline_color, options);
                BitmapDictionary[self].DrawText(text, new ODL.Rect(x, y - 1, w, h), outline_color, options);
                BitmapDictionary[self].DrawText(text, new ODL.Rect(x + 1, y - 1, w, h), outline_color, options);
                BitmapDictionary[self].DrawText(text, new ODL.Rect(x - 1, y, w, h), outline_color, options);
                BitmapDictionary[self].DrawText(text, new ODL.Rect(x + 1, y, w, h), outline_color, options);
                BitmapDictionary[self].DrawText(text, new ODL.Rect(x - 1, y + 1, w, h), outline_color, options);
                BitmapDictionary[self].DrawText(text, new ODL.Rect(x, y + 1, w, h), outline_color, options);
                BitmapDictionary[self].DrawText(text, new ODL.Rect(x + 1, y + 1, w, h), outline_color, options);
            }
            BitmapDictionary[self].DrawText(text, new ODL.Rect(x, y, w, h), color, options);
            SDL2.SDL.SDL_SetTextureBlendMode(BitmapDictionary[self].Texture, SDL2.SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
            AutoLock(self);
            return Internal.QTrue;
        }

        protected static IntPtr clear(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            AutoUnlock(self);
            BitmapDictionary[self].Clear();
            AutoLock(self);
            return Internal.QTrue;
        }

        protected static IntPtr dispose(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            if (_args != IntPtr.Zero)
            {
                RubyArray Args = new RubyArray(_args);
                ScanArgs(0, Args);
            }
            BitmapDictionary[self].Dispose();
            BitmapDictionary.Remove(self);
            Internal.SetIVar(self, "@disposed", Internal.QTrue);
            return Internal.QTrue;
        }

        protected static IntPtr disposed(IntPtr self, IntPtr _args)
        {
            if (_args != IntPtr.Zero)
            {
                RubyArray Args = new RubyArray(_args);
                ScanArgs(0, Args);
            }
            return Internal.GetIVar(self, "@disposed") == Internal.QTrue ? Internal.QTrue : Internal.QFalse;
        }

        protected static void GuardDisposed(IntPtr self)
        {
            if (disposed(self, IntPtr.Zero) == Internal.QTrue)
            {
                Internal.rb_raise(Internal.rb_eRuntimeError.Pointer, "bitmap already disposed");
            }
        }
    }
}

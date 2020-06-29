﻿using System;
using System.Collections.Generic;
using RubyDotNET;

namespace peridot
{
    public class Bitmap : RubyObject
    {
        public static IntPtr Class;
        public static Dictionary<IntPtr, odl.Bitmap> BitmapDictionary = new Dictionary<IntPtr, odl.Bitmap>();

        public static Class CreateClass()
        {
            Class c = new Class("Bitmap");
            Class = c.Pointer;
            c.DefineClassMethod("new", _new);
            c.DefineMethod("initialize", initialize);
            c.DefineClassMethod("mask", mask);
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

        public static IntPtr CreateBitmap(odl.Bitmap Bitmap)
        {
            IntPtr obj = allocate(Class);
            Internal.SetIVar(obj, "@width", Internal.LONG2NUM(Bitmap.Width));
            Internal.SetIVar(obj, "@height", Internal.LONG2NUM(Bitmap.Height));
            Internal.SetIVar(obj, "@autolock", Internal.QTrue);
            Internal.SetIVar(obj, "@font", Font.CreateFont());
            if (BitmapDictionary.ContainsKey(obj))
            {
                BitmapDictionary[obj].Dispose();
                BitmapDictionary.Remove(obj);
            }
            BitmapDictionary.Add(obj, Bitmap);
            return obj;
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

            odl.Bitmap bmp = null;
            if (Args.Length == 1)
            {
                if (Internal.IsType(Args[0].Pointer, Bitmap.Class))
                {
                    bmp = BitmapDictionary[Args[0].Pointer].Clone();
                }
                else
                {
                    Internal.EnsureType(Args[0].Pointer, RubyClass.String);
                    bmp = new odl.Bitmap(new RubyString(Args[0].Pointer).ToString());
                }
            }
            else if (Args.Length == 2)
            {
                Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
                Internal.EnsureType(Args[1].Pointer, RubyClass.Integer);
                bmp = new odl.Bitmap(
                    (int) Internal.NUM2LONG(Args[0].Pointer),
                    (int) Internal.NUM2LONG(Args[1].Pointer)
                );
            }
            else if (Args.Length == 3)
            {
                Internal.EnsureType(Args[0].Pointer, RubyClass.Array);
                Internal.EnsureType(Args[1].Pointer, RubyClass.Integer);
                Internal.EnsureType(Args[2].Pointer, RubyClass.Integer);
                RubyArray pixelarray = new RubyArray(Args[0].Pointer);
                byte[] bytearray = new byte[pixelarray.Length];
                for (int i = 0; i < pixelarray.Length; i++)
                {
                    bytearray[i] = (byte) Internal.NUM2LONG(pixelarray[i].Pointer);
                }
                int width = (int) Internal.NUM2LONG(Args[1].Pointer);
                int height = (int) Internal.NUM2LONG(Args[2].Pointer);
                bmp = new odl.Bitmap(bytearray, width, height);
            }
            else ScanArgs(1, Args);
            Internal.SetIVar(self, "@width", Internal.LONG2NUM(bmp.Width));
            Internal.SetIVar(self, "@height", Internal.LONG2NUM(bmp.Height));
            Internal.SetIVar(self, "@autolock", Internal.QTrue);
            Internal.SetIVar(self, "@font", Font.CreateFont());

            if (BitmapDictionary.ContainsKey(self))
            {
                BitmapDictionary[self].Dispose();
                BitmapDictionary.Remove(self);
            }
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

        protected static IntPtr mask(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            odl.Bitmap result = null;
            if (Args.Length == 2)
            {
                Internal.EnsureType(Args[0].Pointer, Bitmap.Class, "Bitmap");
                Internal.EnsureType(Args[1].Pointer, Bitmap.Class, "Bitmap");
                odl.Bitmap maskbmp = BitmapDictionary[Args[0].Pointer];
                odl.Bitmap srcbmp = BitmapDictionary[Args[1].Pointer];
                result = odl.Bitmap.Mask(maskbmp, srcbmp);
            }
            else if (Args.Length == 3)
            {
                Internal.EnsureType(Args[0].Pointer, Bitmap.Class, "Bitmap");
                Internal.EnsureType(Args[1].Pointer, Bitmap.Class, "Bitmap");
                Internal.EnsureType(Args[2].Pointer, Rect.Class, "Rect");
                odl.Bitmap maskbmp = BitmapDictionary[Args[0].Pointer];
                odl.Bitmap srcbmp = BitmapDictionary[Args[1].Pointer];
                odl.Rect srcrect = Rect.CreateRect(Args[2].Pointer);
                result = odl.Bitmap.Mask(maskbmp, srcbmp, srcrect);
            }
            else if (Args.Length == 4)
            {
                Internal.EnsureType(Args[0].Pointer, Bitmap.Class, "Bitmap");
                Internal.EnsureType(Args[1].Pointer, Bitmap.Class, "Bitmap");
                Internal.EnsureType(Args[2].Pointer, RubyClass.Integer);
                Internal.EnsureType(Args[3].Pointer, RubyClass.Integer);
                odl.Bitmap maskbmp = BitmapDictionary[Args[0].Pointer];
                odl.Bitmap srcbmp = BitmapDictionary[Args[1].Pointer];
                int offsetx = (int) Internal.NUM2LONG(Args[2].Pointer);
                int offsety = (int) Internal.NUM2LONG(Args[3].Pointer);
                result = odl.Bitmap.Mask(maskbmp, srcbmp, offsetx, offsety);
            }
            else if (Args.Length == 5)
            {
                Internal.EnsureType(Args[0].Pointer, Bitmap.Class, "Bitmap");
                Internal.EnsureType(Args[1].Pointer, Bitmap.Class, "Bitmap");
                Internal.EnsureType(Args[2].Pointer, Rect.Class, "Rect");
                Internal.EnsureType(Args[3].Pointer, RubyClass.Integer);
                Internal.EnsureType(Args[4].Pointer, RubyClass.Integer);
                odl.Bitmap maskbmp = BitmapDictionary[Args[0].Pointer];
                odl.Bitmap srcbmp = BitmapDictionary[Args[1].Pointer];
                odl.Rect srcrect = Rect.CreateRect(Args[2].Pointer);
                int offsetx = (int) Internal.NUM2LONG(Args[3].Pointer);
                int offsety = (int) Internal.NUM2LONG(Args[4].Pointer);
                result = odl.Bitmap.Mask(maskbmp, srcbmp, srcrect, offsetx, offsety);
            }
            return Bitmap.CreateBitmap(result);
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
            Internal.EnsureType(Args[0].Pointer, Font.Class, "Font");
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
            if (!Internal.IsType(Args[0].Pointer, RubyClass.Nil))
            {
                Internal.EnsureType(Args[0].Pointer, RubyClass.Bool);
            }
            return Internal.SetIVar(self, "@autolock", Args[0].Pointer == Internal.QTrue ? Internal.QTrue : Internal.QFalse);
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
            Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
            Internal.EnsureType(Args[1].Pointer, RubyClass.Integer);
            int X = (int) Internal.NUM2LONG(Args[0].Pointer),
                Y = (int) Internal.NUM2LONG(Args[1].Pointer);
            return Color.CreateColor(BitmapDictionary[self].GetPixel(X, Y));
        }

        protected static IntPtr set_pixel(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(3, Args);
            Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
            Internal.EnsureType(Args[1].Pointer, RubyClass.Integer);
            Internal.EnsureType(Args[2].Pointer, Color.Class, "Color");
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
            Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
            Internal.EnsureType(Args[1].Pointer, RubyClass.Integer);
            Internal.EnsureType(Args[2].Pointer, RubyClass.Integer);
            Internal.EnsureType(Args[3].Pointer, RubyClass.Integer);
            Internal.EnsureType(Args[4].Pointer, Color.Class, "Color");
            int x1 = (int) Internal.NUM2LONG(Args[0].Pointer),
                y1 = (int) Internal.NUM2LONG(Args[1].Pointer),
                x2 = (int) Internal.NUM2LONG(Args[2].Pointer),
                y2 = (int) Internal.NUM2LONG(Args[3].Pointer);
            AutoUnlock(self);
            odl.Color c = Color.CreateColor(Args[4].Pointer);
            BitmapDictionary[self].DrawLine(x1, y1, x2, y2, c);
            AutoLock(self);
            return Internal.QTrue;
        }

        protected static IntPtr draw_rect(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            int x = 0,
                y = 0,
                w = 0,
                h = 0;
            odl.Color c = null;
            if (Args.Length == 2)
            {
                Internal.EnsureType(Args[0].Pointer, Rect.Class, "Rect");
                Internal.EnsureType(Args[1].Pointer, Color.Class, "Color");
                x = (int) Internal.NUM2LONG(Internal.GetIVar(Args[0].Pointer, "@x"));
                y = (int) Internal.NUM2LONG(Internal.GetIVar(Args[1].Pointer, "@y"));
                w = (int) Internal.NUM2LONG(Internal.GetIVar(Args[2].Pointer, "@width"));
                h = (int) Internal.NUM2LONG(Internal.GetIVar(Args[3].Pointer, "@height"));
                c = Color.CreateColor(Args[1].Pointer);
            }
            else if (Args.Length == 5)
            {
                Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
                Internal.EnsureType(Args[1].Pointer, RubyClass.Integer);
                Internal.EnsureType(Args[2].Pointer, RubyClass.Integer);
                Internal.EnsureType(Args[3].Pointer, RubyClass.Integer);
                Internal.EnsureType(Args[4].Pointer, Color.Class, "Color");
                x = (int) Internal.NUM2LONG(Args[0].Pointer);
                y = (int) Internal.NUM2LONG(Args[1].Pointer);
                w = (int) Internal.NUM2LONG(Args[2].Pointer);
                h = (int) Internal.NUM2LONG(Args[3].Pointer);
                c = Color.CreateColor(Args[4].Pointer);
            }
            else ScanArgs(5, Args);
            AutoUnlock(self);
            BitmapDictionary[self].DrawRect(x, y, w, h, c);
            AutoLock(self);
            return Internal.QTrue;
        }

        protected static IntPtr fill_rect(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            int x = 0,
                y = 0,
                w = 0,
                h = 0;
            odl.Color c = null;
            if (Args.Length == 2)
            {
                Internal.EnsureType(Args[0].Pointer, Rect.Class, "Rect");
                Internal.EnsureType(Args[1].Pointer, Color.Class, "Color");
                x = (int) Internal.NUM2LONG(Internal.GetIVar(Args[0].Pointer, "@x"));
                y = (int) Internal.NUM2LONG(Internal.GetIVar(Args[1].Pointer, "@y"));
                w = (int) Internal.NUM2LONG(Internal.GetIVar(Args[2].Pointer, "@width"));
                h = (int) Internal.NUM2LONG(Internal.GetIVar(Args[3].Pointer, "@height"));
                c = Color.CreateColor(Args[1].Pointer);
            }
            else if (Args.Length == 5)
            {
                Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
                Internal.EnsureType(Args[1].Pointer, RubyClass.Integer);
                Internal.EnsureType(Args[2].Pointer, RubyClass.Integer);
                Internal.EnsureType(Args[3].Pointer, RubyClass.Integer);
                Internal.EnsureType(Args[4].Pointer, Color.Class, "Color");
                x = (int) Internal.NUM2LONG(Args[0].Pointer);
                y = (int) Internal.NUM2LONG(Args[1].Pointer);
                w = (int) Internal.NUM2LONG(Args[2].Pointer);
                h = (int) Internal.NUM2LONG(Args[3].Pointer);
                c = Color.CreateColor(Args[4].Pointer);
            }
            else ScanArgs(5, Args);
            AutoUnlock(self);
            BitmapDictionary[self].FillRect(x, y, w, h, c);
            AutoLock(self);
            return Internal.QTrue;
        }

        protected static IntPtr blt(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            int dx = 0,
                dy = 0,
                dw = 0,
                dh = 0;
            odl.Bitmap srcbmp = null;
            int sx = 0,
                sy = 0,
                sw = 0,
                sh = 0;
            if (Args.Length == 4) // x, y, bmp, srcrect
            {
                Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
                Internal.EnsureType(Args[1].Pointer, RubyClass.Integer);
                Internal.EnsureType(Args[2].Pointer, Bitmap.Class, "Bitmap");
                Internal.EnsureType(Args[3].Pointer, Rect.Class, "Rect");
                dx = (int) Internal.NUM2LONG(Args[0].Pointer);
                dy = (int) Internal.NUM2LONG(Args[1].Pointer);
                srcbmp = BitmapDictionary[Args[2].Pointer];
                sx = (int) Internal.NUM2LONG(Internal.GetIVar(Args[3].Pointer, "@x"));
                sy = (int) Internal.NUM2LONG(Internal.GetIVar(Args[3].Pointer, "@y"));
                sw = (int) Internal.NUM2LONG(Internal.GetIVar(Args[3].Pointer, "@width"));
                sh = (int) Internal.NUM2LONG(Internal.GetIVar(Args[3].Pointer, "@height"));
                dw = sw;
                dh = sh;
            }
            else if (Args.Length == 6) // x, y, w, h, bmp, srcrect
            {
                Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
                Internal.EnsureType(Args[1].Pointer, RubyClass.Integer);
                Internal.EnsureType(Args[2].Pointer, RubyClass.Integer);
                Internal.EnsureType(Args[3].Pointer, RubyClass.Integer);
                Internal.EnsureType(Args[4].Pointer, Bitmap.Class, "Bitmap");
                Internal.EnsureType(Args[5].Pointer, Rect.Class, "Rect");
                dx = (int) Internal.NUM2LONG(Args[0].Pointer);
                dy = (int) Internal.NUM2LONG(Args[1].Pointer);
                dw = (int) Internal.NUM2LONG(Args[2].Pointer);
                dh = (int) Internal.NUM2LONG(Args[3].Pointer);
                srcbmp = BitmapDictionary[Args[4].Pointer];
                sx = (int) Internal.NUM2LONG(Internal.GetIVar(Args[5].Pointer, "@x"));
                sy = (int) Internal.NUM2LONG(Internal.GetIVar(Args[5].Pointer, "@y"));
                sw = (int) Internal.NUM2LONG(Internal.GetIVar(Args[5].Pointer, "@width"));
                sh = (int) Internal.NUM2LONG(Internal.GetIVar(Args[5].Pointer, "@height"));
            }
            else if (Args.Length == 9) // dx, dy, dw, dh, bmp, sx, sy, sw, sh
            {
                Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
                Internal.EnsureType(Args[1].Pointer, RubyClass.Integer);
                Internal.EnsureType(Args[2].Pointer, RubyClass.Integer);
                Internal.EnsureType(Args[3].Pointer, RubyClass.Integer);
                Internal.EnsureType(Args[4].Pointer, Bitmap.Class, "Bitmap");
                Internal.EnsureType(Args[5].Pointer, RubyClass.Integer);
                Internal.EnsureType(Args[6].Pointer, RubyClass.Integer);
                Internal.EnsureType(Args[7].Pointer, RubyClass.Integer);
                Internal.EnsureType(Args[8].Pointer, RubyClass.Integer);
                dx = (int) Internal.NUM2LONG(Args[0].Pointer);
                dy = (int) Internal.NUM2LONG(Args[1].Pointer);
                dw = (int) Internal.NUM2LONG(Args[2].Pointer);
                dh = (int) Internal.NUM2LONG(Args[3].Pointer);
                srcbmp = BitmapDictionary[Args[4].Pointer];
                sx = (int) Internal.NUM2LONG(Args[5].Pointer);
                sy = (int) Internal.NUM2LONG(Args[6].Pointer);
                sw = (int) Internal.NUM2LONG(Args[7].Pointer);
                sh = (int) Internal.NUM2LONG(Args[8].Pointer);
            }
            else if (Args.Length == 3) // destrect, bmp, srcrect
            {
                Internal.EnsureType(Args[0].Pointer, Rect.Class, "Rect");
                Internal.EnsureType(Args[1].Pointer, Bitmap.Class, "Bitmap");
                Internal.EnsureType(Args[2].Pointer, Rect.Class, "Rect");
                dx = (int) Internal.NUM2LONG(Internal.GetIVar(Args[0].Pointer, "@x"));
                dy = (int) Internal.NUM2LONG(Internal.GetIVar(Args[0].Pointer, "@y"));
                dw = (int) Internal.NUM2LONG(Internal.GetIVar(Args[0].Pointer, "@width"));
                dh = (int) Internal.NUM2LONG(Internal.GetIVar(Args[0].Pointer, "@height"));
                srcbmp = BitmapDictionary[Args[1].Pointer];
                sx = (int) Internal.NUM2LONG(Internal.GetIVar(Args[2].Pointer, "@x"));
                sy = (int) Internal.NUM2LONG(Internal.GetIVar(Args[2].Pointer, "@y"));
                sw = (int) Internal.NUM2LONG(Internal.GetIVar(Args[2].Pointer, "@width"));
                sh = (int) Internal.NUM2LONG(Internal.GetIVar(Args[2].Pointer, "@height"));
            }
            else if (Args.Length == 1) // bmp
            {
                Internal.EnsureType(Args[0].Pointer, Bitmap.Class, "Bitmap");
                srcbmp = BitmapDictionary[Args[0].Pointer];
                dw = srcbmp.Width;
                dh = srcbmp.Height;
                sw = srcbmp.Width;
                sh = srcbmp.Height;
            }
            else ScanArgs(4, Args);
            AutoUnlock(self);
            BitmapDictionary[self].Build(dx, dy, dw, dh, srcbmp, sx, sy, sw, sh);
            AutoLock(self);
            return Internal.QTrue;
        }

        protected static IntPtr text_size(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            Internal.EnsureType(Args[0].Pointer, RubyClass.String);
            string text = new RubyString(Args[0].Pointer).ToString();
            BitmapDictionary[self].Font = Font.CreateFont(Internal.GetIVar(self, "@font"));
            return Rect.CreateRect(BitmapDictionary[self].TextSize(text));
        }

        protected static IntPtr draw_text(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            if (Args.Length != 5 && Args.Length != 6) ScanArgs(6, Args);
            Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
            Internal.EnsureType(Args[1].Pointer, RubyClass.Integer);
            Internal.EnsureType(Args[2].Pointer, RubyClass.Integer);
            Internal.EnsureType(Args[3].Pointer, RubyClass.Integer);
            Internal.EnsureType(Args[4].Pointer, RubyClass.String);
            int x = (int) Internal.NUM2LONG(Args[0].Pointer),
                y = (int) Internal.NUM2LONG(Args[1].Pointer),
                w = (int) Internal.NUM2LONG(Args[2].Pointer),
                h = (int) Internal.NUM2LONG(Args[3].Pointer);
            string text = new RubyString(Args[4].Pointer).ToString();
            int align = 0;
            if (Args.Length == 6)
            {
                Internal.EnsureType(Args[5].Pointer, RubyClass.Integer);
                align = (int) Internal.NUM2LONG(Args[5].Pointer);
            }
            AutoUnlock(self);
            BitmapDictionary[self].Font = Font.CreateFont(Internal.GetIVar(self, "@font"));
            odl.Color color = Color.CreateColor(Internal.GetIVar(Internal.GetIVar(self, "@font"), "@color"));
            odl.DrawOptions options = 0;
            bool outline = false;
            if (align == 0) options |= odl.DrawOptions.LeftAlign;
            else if (align == 1) options |= odl.DrawOptions.CenterAlign;
            else if (align == 2) options |= odl.DrawOptions.RightAlign;
            if (Internal.GetIVar(Internal.GetIVar(self, "@font"), "@outline") == Internal.QTrue) outline = true;
            if (outline)
            {
                odl.Color outline_color = Color.CreateColor(Internal.GetIVar(Internal.GetIVar(self, "@font"), "@outline_color"));
                BitmapDictionary[self].DrawText(text, new odl.Rect(x - 1, y - 1, w, h), outline_color, options);
                BitmapDictionary[self].DrawText(text, new odl.Rect(x, y - 1, w, h), outline_color, options);
                BitmapDictionary[self].DrawText(text, new odl.Rect(x + 1, y - 1, w, h), outline_color, options);
                BitmapDictionary[self].DrawText(text, new odl.Rect(x - 1, y, w, h), outline_color, options);
                BitmapDictionary[self].DrawText(text, new odl.Rect(x + 1, y, w, h), outline_color, options);
                BitmapDictionary[self].DrawText(text, new odl.Rect(x - 1, y + 1, w, h), outline_color, options);
                BitmapDictionary[self].DrawText(text, new odl.Rect(x, y + 1, w, h), outline_color, options);
                BitmapDictionary[self].DrawText(text, new odl.Rect(x + 1, y + 1, w, h), outline_color, options);
            }
            BitmapDictionary[self].DrawText(text, new odl.Rect(x, y, w, h), color, options);
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

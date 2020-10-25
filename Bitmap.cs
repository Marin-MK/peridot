using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using Microsoft.VisualBasic.CompilerServices;
using rubydotnet;

namespace peridot
{
    public static class Bitmap
    {
        public static IntPtr Class;
        public static Dictionary<IntPtr, odl.Bitmap> BitmapDictionary = new Dictionary<IntPtr, odl.Bitmap>();

        public static void Create()
        {
            Class = Ruby.Class.Define("Bitmap");
            Ruby.Class.DefineClassMethod(Class, "mask", mask);
            Ruby.Class.DefineMethod(Class, "initialize", initialize);
            Ruby.Class.DefineMethod(Class, "width", widthget);
            Ruby.Class.DefineMethod(Class, "height", heightget);
            Ruby.Class.DefineMethod(Class, "font", fontget);
            Ruby.Class.DefineMethod(Class, "font=", fontset);
            Ruby.Class.DefineMethod(Class, "autolock", autolockget);
            Ruby.Class.DefineMethod(Class, "autolock=", autolockset);
            Ruby.Class.DefineMethod(Class, "unlock", unlockbmp);
            Ruby.Class.DefineMethod(Class, "lock", lockbmp);
            Ruby.Class.DefineMethod(Class, "locked?", locked);
            Ruby.Class.DefineMethod(Class, "get_pixel", get_pixel);
            Ruby.Class.DefineMethod(Class, "set_pixel", set_pixel);
            Ruby.Class.DefineMethod(Class, "draw_line", draw_line);
            Ruby.Class.DefineMethod(Class, "draw_rect", draw_rect);
            Ruby.Class.DefineMethod(Class, "fill_rect", fill_rect);
            Ruby.Class.DefineMethod(Class, "blt", blt);
            Ruby.Class.DefineMethod(Class, "text_size", text_size);
            Ruby.Class.DefineMethod(Class, "draw_text", draw_text);
            Ruby.Class.DefineMethod(Class, "clear", clear);
            Ruby.Class.DefineMethod(Class, "dispose", dispose);
            Ruby.Class.DefineMethod(Class, "disposed?", disposed);
        }

        public static IntPtr CreateBitmap(odl.Bitmap Bitmap)
        {
            IntPtr bmp = Ruby.Class.Allocate(Class);
            Ruby.SetIVar(bmp, "@width", Ruby.Integer.ToPtr(Bitmap.Width));
            Ruby.SetIVar(bmp, "@height", Ruby.Integer.ToPtr(Bitmap.Height));
            Ruby.SetIVar(bmp, "@autolock", Ruby.True);
            Ruby.SetIVar(bmp, "@font", Font.CreateFont());
            if (BitmapDictionary.ContainsKey(bmp))
            {
                BitmapDictionary[bmp].Dispose();
                BitmapDictionary.Remove(bmp);
            }
            BitmapDictionary.Add(bmp, Bitmap);
            return bmp;
        }

        static IntPtr initialize(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1, 2, 3, 4);
            odl.Bitmap bmp = null;
            long len = Ruby.Array.Length(Args);
            if (len == 1)
            {
                if (Ruby.Array.Is(Args, 0, "Bitmap"))
                {
                    bmp = BitmapDictionary[Ruby.Array.Get(Args, 0)];
                }
                else
                {
                    Ruby.Array.Expect(Args, 0, "String");
                    bmp = new odl.Bitmap(Ruby.String.FromPtr(Ruby.Array.Get(Args, 0)));
                }
            }
            else if (len == 2)
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                Ruby.Array.Expect(Args, 1, "Integer");
                bmp = new odl.Bitmap((int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0)), (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 1)));
            }
            else if (len == 3 || len == 4)
            {
                Ruby.Array.Expect(Args, 0, "Array");
                Ruby.Array.Expect(Args, 1, "Integer");
                Ruby.Array.Expect(Args, 2, "Integer");
                if (len == 4) Ruby.Array.Expect(Args, 3, "TrueClass", "FalseClass");
                IntPtr pixelarray = Ruby.Array.Get(Args, 0);
                long pixellen = Ruby.Array.Length(pixelarray);
                byte[] bytearray = new byte[pixellen];
                bool validate = len == 3 || Ruby.Array.Get(Args, 3) == Ruby.True;
                for (int i = 0; i < pixellen; i++)
                {
                    if (validate) Ruby.Array.Expect(pixelarray, i, "Integer");
                    bytearray[i] = (byte) Ruby.Integer.FromPtr(Ruby.Array.Get(pixelarray, i));
                }
                int width = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 1));
                int height = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 2));
                bmp = new odl.Bitmap(bytearray, width, height);
            }
            Ruby.SetIVar(Self, "@width", Ruby.Integer.ToPtr(bmp.Width));
            Ruby.SetIVar(Self, "@height", Ruby.Integer.ToPtr(bmp.Height));
            Ruby.SetIVar(Self, "@autolock", Ruby.True);
            Ruby.SetIVar(Self, "@font", Font.CreateFont());

            if (BitmapDictionary.ContainsKey(Self))
            {
                BitmapDictionary[Self].Dispose();
                BitmapDictionary.Remove(Self);
            }
            BitmapDictionary.Add(Self, bmp);
            return Self;
        }

        static IntPtr AutoUnlock(IntPtr Self)
        {
            if (Ruby.GetIVar(Self, "@autolock") == Ruby.True)
            {
                BitmapDictionary[Self].Unlock();
                return Ruby.True;
            }
            else if (BitmapDictionary[Self].Locked)
            {
                Ruby.Raise(Ruby.ErrorType.RuntimeError, "bitmap locked for writing");
            }
            return Ruby.True;
        }

        static IntPtr AutoLock(IntPtr Self)
        {
            if (Ruby.GetIVar(Self, "@autolock") == Ruby.True)
            {
                BitmapDictionary[Self].Lock();
                return Ruby.True;
            }
            return Ruby.True;
        }

        static IntPtr mask(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 2, 3, 4, 5);
            odl.Bitmap result = null;
            long len = Ruby.Array.Length(Args);
            if (len == 2)
            {
                Ruby.Array.Expect(Args, 0, "Bitmap");
                Ruby.Array.Expect(Args, 1, "Bitmap");
                GuardDisposed(Ruby.Array.Get(Args, 0));
                GuardDisposed(Ruby.Array.Get(Args, 1));
                odl.Bitmap maskbmp = BitmapDictionary[Ruby.Array.Get(Args, 0)];
                odl.Bitmap srcbmp = BitmapDictionary[Ruby.Array.Get(Args, 1)];
                result = odl.Bitmap.Mask(maskbmp, srcbmp);
            }
            else if (len == 3)
            {
                Ruby.Array.Expect(Args, 0, "Bitmap");
                Ruby.Array.Expect(Args, 1, "Bitmap");
                Ruby.Array.Expect(Args, 2, "Rect");
                GuardDisposed(Ruby.Array.Get(Args, 0));
                GuardDisposed(Ruby.Array.Get(Args, 1));
                odl.Bitmap maskbmp = BitmapDictionary[Ruby.Array.Get(Args, 0)];
                odl.Bitmap srcbmp = BitmapDictionary[Ruby.Array.Get(Args, 1)];
                odl.Rect srcrect = Rect.CreateRect(Ruby.Array.Get(Args, 2));
                result = odl.Bitmap.Mask(maskbmp, srcbmp, srcrect);
            }
            else if (len == 4)
            {
                Ruby.Array.Expect(Args, 0, "Bitmap");
                Ruby.Array.Expect(Args, 1, "Bitmap");
                Ruby.Array.Expect(Args, 2, "Integer");
                Ruby.Array.Expect(Args, 3, "Integer");
                GuardDisposed(Ruby.Array.Get(Args, 0));
                GuardDisposed(Ruby.Array.Get(Args, 1));
                odl.Bitmap maskbmp = BitmapDictionary[Ruby.Array.Get(Args, 0)];
                odl.Bitmap srcbmp = BitmapDictionary[Ruby.Array.Get(Args, 1)];
                int offsetx = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 2));
                int offsety = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 3));
                result = odl.Bitmap.Mask(maskbmp, srcbmp, offsetx, offsety);
            }
            else if (len == 5)
            {
                Ruby.Array.Expect(Args, 0, "Bitmap");
                Ruby.Array.Expect(Args, 1, "Bitmap");
                Ruby.Array.Expect(Args, 2, "Rect");
                Ruby.Array.Expect(Args, 3, "Integer");
                Ruby.Array.Expect(Args, 4, "Integer");
                GuardDisposed(Ruby.Array.Get(Args, 0));
                GuardDisposed(Ruby.Array.Get(Args, 1));
                odl.Bitmap maskbmp = BitmapDictionary[Ruby.Array.Get(Args, 0)];
                odl.Bitmap srcbmp = BitmapDictionary[Ruby.Array.Get(Args, 1)];
                odl.Rect srcrect = Rect.CreateRect(Ruby.Array.Get(Args, 2));
                int offsetx = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 3));
                int offsety = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 4));
                result = odl.Bitmap.Mask(maskbmp, srcbmp, srcrect, offsetx, offsety);
            }
            return Bitmap.CreateBitmap(result);
        }

        static IntPtr widthget(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@width");
        }

        static IntPtr heightget(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@height");
        }

        static IntPtr fontget(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@font");
        }

        static IntPtr fontset(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "Font");
            return Ruby.SetIVar(Self, "@font", Ruby.Array.Get(Args, 0));
        }

        static IntPtr autolockget(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@autolock");
        }

        static IntPtr autolockset(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "TrueClass", "FalseClass", "NilClass");
            return Ruby.SetIVar(Self, "@autolock", Ruby.Array.Get(Args, 0) == Ruby.True ? Ruby.True : Ruby.False);
        }

        static IntPtr unlockbmp(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            if (Ruby.GetIVar(Self, "@autolock") == Ruby.True) Ruby.Raise(Ruby.ErrorType.RuntimeError, "manual unlocking disallowed while autolock is enabled");
            else if (!BitmapDictionary[Self].Locked) Ruby.Raise(Ruby.ErrorType.RuntimeError, "bitmap already unlocked");
            BitmapDictionary[Self].Unlock();
            return Ruby.True;
        }

        static IntPtr lockbmp(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            if (Ruby.GetIVar(Self, "@autolock") == Ruby.True) Ruby.Raise(Ruby.ErrorType.RuntimeError, "manual locking disallowed with autolock enabled");
            else if (BitmapDictionary[Self].Locked) Ruby.Raise(Ruby.ErrorType.RuntimeError, "bitmap already locked");
            BitmapDictionary[Self].Lock();
            return Ruby.True;
        }

        static IntPtr locked(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            return BitmapDictionary[Self].Locked ? Ruby.True : Ruby.False;
        }

        static IntPtr get_pixel(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 2);
            Ruby.Array.Expect(Args, 0, "Integer");
            Ruby.Array.Expect(Args, 1, "Integer");
            int X = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
            int Y = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 1));
            return Color.CreateColor(BitmapDictionary[Self].GetPixel(X, Y));
        }

        static IntPtr set_pixel(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 3);
            Ruby.Array.Expect(Args, 0, "Integer");
            Ruby.Array.Expect(Args, 1, "Integer");
            Ruby.Array.Expect(Args, 2, "Color");
            int X = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
            int Y = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 1));
            AutoUnlock(Self);
            BitmapDictionary[Self].SetPixel(X, Y, Color.CreateColor(Ruby.Array.Get(Args, 2)));
            AutoLock(Self);
            return Ruby.True;
        }

        static IntPtr draw_line(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 5);
            Ruby.Array.Expect(Args, 0, "Integer");
            Ruby.Array.Expect(Args, 1, "Integer");
            Ruby.Array.Expect(Args, 2, "Integer");
            Ruby.Array.Expect(Args, 3, "Integer");
            Ruby.Array.Expect(Args, 4, "Color");
            int x1 = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
            int y1 = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 1));
            int x2 = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 2));
            int y2 = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 3));
            AutoUnlock(Self);
            odl.Color c = Color.CreateColor(Ruby.Array.Get(Args, 4));
            BitmapDictionary[Self].DrawLine(x1, y1, x2, y2, c);
            AutoLock(Self);
            return Ruby.True;
        }

        static IntPtr draw_rect(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 2, 5);
            int x = 0,
                y = 0,
                w = 0,
                h = 0;
            odl.Color c = null;
            long len = Ruby.Array.Length(Args);
            if (len == 2)
            {
                Ruby.Array.Expect(Args, 0, "Rect");
                Ruby.Array.Expect(Args, 1, "Color");
                if (Ruby.IVarIs(Ruby.Array.Get(Args, 0), "@x", "Float")) x = Ruby.Float.RoundFromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@x"));
                else x = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@x"));
                if (Ruby.IVarIs(Ruby.Array.Get(Args, 0), "@y", "Float")) y = Ruby.Float.RoundFromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@y"));
                else y = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@y"));
                if (Ruby.IVarIs(Ruby.Array.Get(Args, 0), "@width", "Float")) w = Ruby.Float.RoundFromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@width"));
                else w = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@width"));
                if (Ruby.IVarIs(Ruby.Array.Get(Args, 0), "@height", "Float")) h = Ruby.Float.RoundFromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@height"));
                else h = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@height"));
                c = Color.CreateColor(Ruby.Array.Get(Args, 1));
            }
            else if (len == 5)
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                Ruby.Array.Expect(Args, 1, "Integer");
                Ruby.Array.Expect(Args, 2, "Integer");
                Ruby.Array.Expect(Args, 3, "Integer");
                Ruby.Array.Expect(Args, 4, "Color");
                x = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
                y = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 1));
                w = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 2));
                h = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 3));
                c = Color.CreateColor(Ruby.Array.Get(Args, 4));
            }
            AutoUnlock(Self);
            BitmapDictionary[Self].DrawRect(x, y, w, h, c);
            AutoLock(Self);
            return Ruby.True;
        }

        static IntPtr fill_rect(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 2, 5);
            int x = 0,
                y = 0,
                w = 0,
                h = 0;
            odl.Color c = null;
            long len = Ruby.Array.Length(Args);
            if (len == 2)
            {
                Ruby.Array.Expect(Args, 0, "Rect");
                Ruby.Array.Expect(Args, 1, "Color");
                if (Ruby.IVarIs(Ruby.Array.Get(Args, 0), "@x", "Float")) x = Ruby.Float.RoundFromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@x"));
                else x = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@x"));
                if (Ruby.IVarIs(Ruby.Array.Get(Args, 0), "@y", "Float")) y = Ruby.Float.RoundFromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@y"));
                else y = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@y"));
                if (Ruby.IVarIs(Ruby.Array.Get(Args, 0), "@width", "Float")) w = Ruby.Float.RoundFromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@width"));
                else w = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@width"));
                if (Ruby.IVarIs(Ruby.Array.Get(Args, 0), "@height", "Float")) h = Ruby.Float.RoundFromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@height"));
                else h = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@height"));
                c = Color.CreateColor(Ruby.Array.Get(Args, 1));
            }
            else if (len == 5)
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                Ruby.Array.Expect(Args, 1, "Integer");
                Ruby.Array.Expect(Args, 2, "Integer");
                Ruby.Array.Expect(Args, 3, "Integer");
                Ruby.Array.Expect(Args, 4, "Color");
                x = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
                y = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 1));
                w = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 2));
                h = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 3));
                c = Color.CreateColor(Ruby.Array.Get(Args, 4));
            }
            AutoUnlock(Self);
            BitmapDictionary[Self].FillRect(x, y, w, h, c);
            AutoLock(Self);
            return Ruby.True;
        }

        static IntPtr blt(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 1, 3, 4, 6, 9);
            int dx = 0,
                dy = 0,
                dw = 0,
                dh = 0;
            odl.Bitmap srcbmp = null;
            int sx = 0,
                sy = 0,
                sw = 0,
                sh = 0;
            long len = Ruby.Array.Length(Args);
            if (len == 1) // bmp
            {
                Ruby.Array.Expect(Args, 0, "Bitmap");
                srcbmp = BitmapDictionary[Ruby.Array.Get(Args, 0)];
                dw = srcbmp.Width;
                dh = srcbmp.Height;
                sw = srcbmp.Width;
                sh = srcbmp.Height;
            }
            else if (len == 3) // destrect, bmp, srcrect
            {
                Ruby.Array.Expect(Args, 0, "Rect");
                Ruby.Array.Expect(Args, 1, "Bitmap");
                Ruby.Array.Expect(Args, 2, "Rect");

                if (Ruby.IVarIs(Ruby.Array.Get(Args, 0), "@x", "Float")) dx = Ruby.Float.RoundFromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@x"));
                else dx = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@x"));
                if (Ruby.IVarIs(Ruby.Array.Get(Args, 0), "@y", "Float")) dy = Ruby.Float.RoundFromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@y"));
                else dy = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@y"));
                if (Ruby.IVarIs(Ruby.Array.Get(Args, 0), "@width", "Float")) dw = Ruby.Float.RoundFromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@width"));
                else dw = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@width"));
                if (Ruby.IVarIs(Ruby.Array.Get(Args, 0), "@height", "Float")) dh = Ruby.Float.RoundFromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@height"));
                else dh = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 0), "@height"));
                srcbmp = BitmapDictionary[Ruby.Array.Get(Args, 1)];
                if (Ruby.IVarIs(Ruby.Array.Get(Args, 2), "@x", "Float")) sx = Ruby.Float.RoundFromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 2), "@x"));
                else sx = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 2), "@x"));
                if (Ruby.IVarIs(Ruby.Array.Get(Args, 2), "@y", "Float")) sy = Ruby.Float.RoundFromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 2), "@y"));
                else sy = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 2), "@y"));
                if (Ruby.IVarIs(Ruby.Array.Get(Args, 2), "@width", "Float")) sw = Ruby.Float.RoundFromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 2), "@width"));
                else sw = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 2), "@width"));
                if (Ruby.IVarIs(Ruby.Array.Get(Args, 2), "@height", "Float")) sh = Ruby.Float.RoundFromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 2), "@height"));
                else sh = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 2), "@height"));
            }
            else if (len == 4) // dx, dy, bmp, srcrect
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                Ruby.Array.Expect(Args, 1, "Integer");
                Ruby.Array.Expect(Args, 2, "Bitmap");
                Ruby.Array.Expect(Args, 3, "Rect");
                dx = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
                dy = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 1));
                srcbmp = BitmapDictionary[Ruby.Array.Get(Args, 2)];
                if (Ruby.IVarIs(Ruby.Array.Get(Args, 3), "@x", "Float")) sx = Ruby.Float.RoundFromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 3), "@x"));
                else sx = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 3), "@x"));
                if (Ruby.IVarIs(Ruby.Array.Get(Args, 3), "@y", "Float")) sy = Ruby.Float.RoundFromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 3), "@y"));
                else sy = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 3), "@y"));
                if (Ruby.IVarIs(Ruby.Array.Get(Args, 3), "@width", "Float")) sw = Ruby.Float.RoundFromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 3), "@width"));
                else sw = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 3), "@width"));
                if (Ruby.IVarIs(Ruby.Array.Get(Args, 3), "@height", "Float")) sh = Ruby.Float.RoundFromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 3), "@height"));
                else sh = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 3), "@height"));
                dw = sw;
                dh = sh;
            }
            else if (len == 6) // dx, dy, dw, dh, bmp, srcrect
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                Ruby.Array.Expect(Args, 1, "Integer");
                Ruby.Array.Expect(Args, 2, "Integer");
                Ruby.Array.Expect(Args, 3, "Integer");
                Ruby.Array.Expect(Args, 4, "Bitmap");
                Ruby.Array.Expect(Args, 5, "Rect");
                dx = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
                dy = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 1));
                dw = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 2));
                dh = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 3));
                srcbmp = BitmapDictionary[Ruby.Array.Get(Args, 4)];
                if (Ruby.IVarIs(Ruby.Array.Get(Args, 5), "@x", "Float")) sx = Ruby.Float.RoundFromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 5), "@x"));
                else sx = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 5), "@x"));
                if (Ruby.IVarIs(Ruby.Array.Get(Args, 5), "@y", "Float")) sy = Ruby.Float.RoundFromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 5), "@y"));
                else sy = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 5), "@y"));
                if (Ruby.IVarIs(Ruby.Array.Get(Args, 5), "@width", "Float")) sw = Ruby.Float.RoundFromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 5), "@width"));
                else sw = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 5), "@width"));
                if (Ruby.IVarIs(Ruby.Array.Get(Args, 5), "@height", "Float")) sh = Ruby.Float.RoundFromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 5), "@height"));
                else sh = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Ruby.Array.Get(Args, 5), "@height"));
            }
            else if (len == 9) // dx, dy, dw, dh, bmp, sx, sy, sw, sh
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                Ruby.Array.Expect(Args, 1, "Integer");
                Ruby.Array.Expect(Args, 2, "Integer");
                Ruby.Array.Expect(Args, 3, "Integer");
                Ruby.Array.Expect(Args, 4, "Bitmap");
                Ruby.Array.Expect(Args, 5, "Integer");
                Ruby.Array.Expect(Args, 6, "Integer");
                Ruby.Array.Expect(Args, 7, "Integer");
                Ruby.Array.Expect(Args, 8, "Integer");
                dx = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
                dy = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 1));
                dw = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 2));
                dh = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 3));
                srcbmp = BitmapDictionary[Ruby.Array.Get(Args, 4)];
                sx = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 5));
                sy = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 6));
                sw = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 7));
                sh = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 8));
            }
            AutoUnlock(Self);
            BitmapDictionary[Self].Build(dx, dy, dw, dh, srcbmp, sx, sy, sw, sh);
            AutoLock(Self);
            return Ruby.True;
        }

        static IntPtr text_size(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "String");
            string text = Ruby.String.FromPtr(Ruby.Array.Get(Args, 0));
            BitmapDictionary[Self].Font = Font.CreateFont(Ruby.GetIVar(Self, "@font"));
            return Rect.CreateRect(new odl.Rect(BitmapDictionary[Self].TextSize(text)));
        }

        static IntPtr draw_text(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 5, 6);
            Ruby.Array.Expect(Args, 0, "Integer");
            Ruby.Array.Expect(Args, 1, "Integer");
            Ruby.Array.Expect(Args, 2, "Integer");
            Ruby.Array.Expect(Args, 3, "Integer");
            Ruby.Array.Expect(Args, 4, "String");
            int x = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
            int y = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 1));
            int w = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 2));
            int h = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 3));
            string text = Ruby.String.FromPtr(Ruby.Array.Get(Args, 4));
            int align = 0;
            if (Ruby.Array.Length(Args) == 6)
            {
                if (Ruby.Is(Ruby.Array.Get(Args, 5), "Symbol"))
                {
                    string sym = Ruby.Symbol.FromPtr(Ruby.Array.Get(Args, 5));
                    if (sym == ":left") align = 0;
                    else if (sym == ":center" || sym == ":middle") align = 1;
                    else if (sym == ":right") align = 2;
                    else Ruby.Raise(Ruby.ErrorType.ArgumentError, "invalid alignment argument");
                }
                else
                {
                    Ruby.Array.Expect(Args, 5, "Integer");
                    align = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 5));
                    if (align < 0 || align > 2) Ruby.Raise(Ruby.ErrorType.ArgumentError, "invalid alignment argument");
                }
            }
            AutoUnlock(Self);
            BitmapDictionary[Self].Font = Font.CreateFont(Ruby.GetIVar(Self, "@font"));
            odl.Color color = Color.CreateColor(Ruby.GetIVar(Ruby.GetIVar(Self, "@font"), "@color"));
            odl.DrawOptions options = 0;
            bool outline = false;
            if (align == 0) options |= odl.DrawOptions.LeftAlign;
            else if (align == 1) options |= odl.DrawOptions.CenterAlign;
            else if (align == 2) options |= odl.DrawOptions.RightAlign;
            if (Ruby.GetIVar(Ruby.GetIVar(Self, "@font"), "@outline") == Ruby.True) outline = true;
            if (outline)
            {
                odl.Color outline_color = Color.CreateColor(Ruby.GetIVar(Ruby.GetIVar(Self, "@font"), "@outline_color"));
                BitmapDictionary[Self].DrawText(text, new odl.Rect(x - 1, y - 1, w, h), outline_color, options);
                BitmapDictionary[Self].DrawText(text, new odl.Rect(x, y - 1, w, h), outline_color, options);
                BitmapDictionary[Self].DrawText(text, new odl.Rect(x + 1, y - 1, w, h), outline_color, options);
                BitmapDictionary[Self].DrawText(text, new odl.Rect(x - 1, y, w, h), outline_color, options);
                BitmapDictionary[Self].DrawText(text, new odl.Rect(x + 1, y, w, h), outline_color, options);
                BitmapDictionary[Self].DrawText(text, new odl.Rect(x - 1, y + 1, w, h), outline_color, options);
                BitmapDictionary[Self].DrawText(text, new odl.Rect(x, y + 1, w, h), outline_color, options);
                BitmapDictionary[Self].DrawText(text, new odl.Rect(x + 1, y + 1, w, h), outline_color, options);
            }
            BitmapDictionary[Self].DrawText(text, new odl.Rect(x, y, w, h), color, options);
            odl.SDL2.SDL.SDL_SetTextureBlendMode(BitmapDictionary[Self].Texture, odl.SDL2.SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
            AutoLock(Self);
            return Ruby.True;
        }

        static IntPtr clear(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            AutoUnlock(Self);
            BitmapDictionary[Self].Clear();
            AutoLock(Self);
            return Ruby.True;
        }

        static IntPtr dispose(IntPtr Self, IntPtr Args)
        {
            GuardDisposed(Self);
            Ruby.Array.Expect(Args, 0);
            if (BitmapDictionary.ContainsKey(Self))
            {
                BitmapDictionary[Self].Dispose();
                foreach (KeyValuePair<IntPtr, odl.Bitmap> kvp in BitmapDictionary)
                {
                    if (kvp.Value == BitmapDictionary[Self] && kvp.Key != Self)
                    {
                        Ruby.SetIVar(kvp.Key, "@disposed", Ruby.True);
                    }
                }
            }
            BitmapDictionary.Remove(Self);
            Ruby.SetIVar(Self, "@disposed", Ruby.True);
            return Ruby.True;
        }

        static IntPtr disposed(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@disposed") == Ruby.True ? Ruby.True : Ruby.False;
        }

        static void GuardDisposed(IntPtr Self)
        {
            if (Ruby.GetIVar(Self, "@disposed") == Ruby.True)
            {
                Ruby.Raise(Ruby.ErrorType.RuntimeError, "bitmap already disposed");
            }
        }
    }
}

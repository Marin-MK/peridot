using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.VisualBasic.CompilerServices;
using rubydotnet;

namespace peridot
{
    public class Bitmap : Ruby.Object
    {
        public new static string KlassName = "Bitmap";
        public new static Ruby.Class Class;
        public static Dictionary<IntPtr, odl.Bitmap> BitmapDictionary = new Dictionary<IntPtr, odl.Bitmap>();

        public Bitmap(IntPtr Pointer) : base(Pointer) { }

        public static void Create()
        {
            Ruby.Class c = Ruby.Class.DefineClass<Bitmap>(KlassName);
            Class = c;
            c.DefineClassMethod("mask", mask);
            c.DefineMethod("initialize", initialize);
            c.DefineMethod("width", widthget);
            c.DefineMethod("height", heightget);
            c.DefineMethod("font", fontget);
            c.DefineMethod("font=", fontset);
            c.DefineMethod("autolock", autolockget);
            c.DefineMethod("autolock=", autolockset);
            c.DefineMethod("unlock", unlockbmp);
            c.DefineMethod("lock", lockbmp);
            c.DefineMethod("locked?", locked);
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
        }

        public static Bitmap CreateBitmap(odl.Bitmap Bitmap)
        {
            Bitmap bmp = Class.Allocate().Convert<Bitmap>();
            bmp.SetIVar("@width", (Ruby.Integer) Bitmap.Width);
            bmp.SetIVar("@height", (Ruby.Integer) Bitmap.Height);
            bmp.SetIVar("@autolock", Ruby.True);
            bmp.SetIVar("@font", Font.CreateFont());
            if (BitmapDictionary.ContainsKey(bmp.Pointer))
            {
                BitmapDictionary[bmp.Pointer].Dispose();
                BitmapDictionary.Remove(bmp.Pointer);
            }
            BitmapDictionary.Add(bmp.Pointer, Bitmap);
            return bmp;
        }

        protected static Ruby.Object initialize(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect((0, 5));
            odl.Bitmap bmp = null;
            if (Args.Length == 1)
            {
                if (Args[0].Is(Bitmap.Class))
                {
                    bmp = BitmapDictionary[Args[0].Pointer];
                }
                else
                {
                    Args[0].Expect(Ruby.String.Class);
                    bmp = new odl.Bitmap(Args.Get<Ruby.String>(0));
                }
            }
            else if (Args.Length == 2)
            {
                Args[0].Expect(Ruby.Integer.Class);
                Args[1].Expect(Ruby.Integer.Class);
                bmp = new odl.Bitmap(Args.Get<Ruby.Integer>(0), Args.Get<Ruby.Integer>(1));
            }
            else if (Args.Length == 3 || Args.Length == 4)
            {
                Args[0].Expect(Ruby.Array.Class);
                Args[1].Expect(Ruby.Integer.Class);
                Args[2].Expect(Ruby.Integer.Class);
                if (Args.Length == 4) Args[3].Expect(Ruby.TrueClass.Class, Ruby.FalseClass.Class);
                Ruby.Array pixelarray = Args.Get<Ruby.Array>(0);
                byte[] bytearray = new byte[pixelarray.Length];
                bool validate = Args.Length == 3 || Args[3] == Ruby.True;
                for (int i = 0; i < pixelarray.Length; i++)
                {
                    if (validate) pixelarray[i].Expect(Ruby.Integer.Class);
                    bytearray[i] = (byte) pixelarray[i].Convert<Ruby.Integer>();
                }
                int width = Args.Get<Ruby.Integer>(1);
                int height = Args.Get<Ruby.Integer>(2);
                bmp = new odl.Bitmap(bytearray, width, height);
            }
            Self.SetIVar("@width", (Ruby.Integer) bmp.Width);
            Self.SetIVar("@height", (Ruby.Integer) bmp.Height);
            Self.SetIVar("@autolock", Ruby.True);
            Self.SetIVar("@font", Font.CreateFont());

            if (BitmapDictionary.ContainsKey(Self.Pointer))
            {
                BitmapDictionary[Self.Pointer].Dispose();
                BitmapDictionary.Remove(Self.Pointer);
            }
            BitmapDictionary.Add(Self.Pointer, bmp);
            return Self;
        }

        protected static Ruby.Object AutoUnlock(Ruby.Object Self)
        {
            if (Self.GetIVar("@autolock") == Ruby.True)
            {
                BitmapDictionary[Self.Pointer].Unlock();
                return Ruby.True;
            }
            else if (BitmapDictionary[Self.Pointer].Locked)
            {
                Ruby.Raise(Ruby.ErrorType.RuntimeError, "bitmap locked for writing");
            }
            return Ruby.True;
        }

        protected static Ruby.Object AutoLock(Ruby.Object Self)
        {
            if (Self.GetIVar("@autolock") == Ruby.True)
            {
                BitmapDictionary[Self.Pointer].Lock();
                return Ruby.True;
            }
            return Ruby.True;
        }

        protected static Ruby.Object mask(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(2, 3, 4, 5);
            odl.Bitmap result = null;
            if (Args.Length == 2)
            {
                Args[0].Expect(Bitmap.Class);
                Args[1].Expect(Bitmap.Class);
                odl.Bitmap maskbmp = BitmapDictionary[Args[0].Pointer];
                odl.Bitmap srcbmp = BitmapDictionary[Args[1].Pointer];
                result = odl.Bitmap.Mask(maskbmp, srcbmp);
            }
            else if (Args.Length == 3)
            {
                Args[0].Expect(Bitmap.Class);
                Args[1].Expect(Bitmap.Class);
                Args[2].Expect(Rect.Class);
                odl.Bitmap maskbmp = BitmapDictionary[Args[0].Pointer];
                odl.Bitmap srcbmp = BitmapDictionary[Args[1].Pointer];
                odl.Rect srcrect = Rect.CreateRect(Args[2]);
                result = odl.Bitmap.Mask(maskbmp, srcbmp, srcrect);
            }
            else if (Args.Length == 4)
            {
                Args[0].Expect(Bitmap.Class);
                Args[1].Expect(Bitmap.Class);
                Args[2].Expect(Ruby.Integer.Class);
                Args[3].Expect(Ruby.Integer.Class);
                odl.Bitmap maskbmp = BitmapDictionary[Args[0].Pointer];
                odl.Bitmap srcbmp = BitmapDictionary[Args[1].Pointer];
                int offsetx = Args.Get<Ruby.Integer>(2);
                int offsety = Args.Get<Ruby.Integer>(3);
                result = odl.Bitmap.Mask(maskbmp, srcbmp, offsetx, offsety);
            }
            else if (Args.Length == 5)
            {
                Args[0].Expect(Bitmap.Class);
                Args[1].Expect(Bitmap.Class);
                Args[2].Expect(Rect.Class);
                Args[3].Expect(Ruby.Integer.Class);
                Args[4].Expect(Ruby.Integer.Class);
                odl.Bitmap maskbmp = BitmapDictionary[Args[0].Pointer];
                odl.Bitmap srcbmp = BitmapDictionary[Args[1].Pointer];
                odl.Rect srcrect = Rect.CreateRect(Args[2]);
                int offsetx = Args.Get<Ruby.Integer>(3);
                int offsety = Args.Get<Ruby.Integer>(4);
                result = odl.Bitmap.Mask(maskbmp, srcbmp, srcrect, offsetx, offsety);
            }
            return Bitmap.CreateBitmap(result);
        }

        protected static Ruby.Object widthget(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            return Self.GetIVar("@width");
        }

        protected static Ruby.Object heightget(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            return Self.GetIVar("@height");
        }

        protected static Ruby.Object fontget(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            return Self.GetIVar("@font");
        }

        protected static Ruby.Object fontset(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(1);
            Args[0].Expect(Font.Class);
            return Self.SetIVar("@font", Args[0]);
        }

        protected static Ruby.Object autolockget(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            return Self.GetIVar("@autolock");
        }

        protected static Ruby.Object autolockset(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(1);
            Args[0].Expect(Ruby.TrueClass.Class, Ruby.FalseClass.Class, Ruby.NilClass.Class);
            return Self.SetIVar("@autolock", Args[0] == Ruby.True ? (Ruby.Object) Ruby.True : Ruby.False);
        }

        protected static Ruby.Object unlockbmp(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            if (Self.GetIVar("@autolock") == Ruby.True) Ruby.Raise(Ruby.ErrorType.RuntimeError, "manual unlocking disallowed while autolock is enabled");
            else if (!BitmapDictionary[Self.Pointer].Locked) Ruby.Raise(Ruby.ErrorType.RuntimeError, "bitmap already unlocked");
            BitmapDictionary[Self.Pointer].Unlock();
            return Ruby.True;
        }

        protected static Ruby.Object lockbmp(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            if (Self.GetIVar("@autolock") == Ruby.True) Ruby.Raise(Ruby.ErrorType.RuntimeError, "manual locking disallowed with autolock enabled");
            else if (BitmapDictionary[Self.Pointer].Locked) Ruby.Raise(Ruby.ErrorType.RuntimeError, "bitmap already locked");
            BitmapDictionary[Self.Pointer].Lock();
            return Ruby.True;
        }

        protected static Ruby.Object locked(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            return BitmapDictionary[Self.Pointer].Locked ? (Ruby.Object) Ruby.True : Ruby.False;
        }

        protected static Ruby.Object get_pixel(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(2);
            Args[0].Expect(Ruby.Integer.Class);
            Args[1].Expect(Ruby.Integer.Class);
            int X = Args.Get<Ruby.Integer>(0);
            int Y = Args.Get<Ruby.Integer>(1);
            return Color.CreateColor(BitmapDictionary[Self.Pointer].GetPixel(X, Y));
        }

        protected static Ruby.Object set_pixel(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(3);
            Args[0].Expect(Ruby.Integer.Class);
            Args[1].Expect(Ruby.Integer.Class);
            Args[2].Expect(Color.Class);
            int X = Args.Get<Ruby.Integer>(0);
            int Y = Args.Get<Ruby.Integer>(1);
            AutoUnlock(Self);
            BitmapDictionary[Self.Pointer].SetPixel(X, Y, Color.CreateColor(Args[2]));
            AutoLock(Self);
            return Ruby.True;
        }

        protected static Ruby.Object draw_line(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(5);
            Args[0].Expect(Ruby.Integer.Class);
            Args[1].Expect(Ruby.Integer.Class);
            Args[2].Expect(Ruby.Integer.Class);
            Args[3].Expect(Ruby.Integer.Class);
            Args[4].Expect(Color.Class);
            int x1 = Args.Get<Ruby.Integer>(0);
            int y1 = Args.Get<Ruby.Integer>(1);
            int x2 = Args.Get<Ruby.Integer>(2);
            int y2 = Args.Get<Ruby.Integer>(3);
            AutoUnlock(Self);
            odl.Color c = Color.CreateColor(Args[4]);
            BitmapDictionary[Self.Pointer].DrawLine(x1, y1, x2, y2, c);
            AutoLock(Self);
            return Ruby.True;
        }

        protected static Ruby.Object draw_rect(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(2, 5);
            int x = 0,
                y = 0,
                w = 0,
                h = 0;
            odl.Color c = null;
            if (Args.Length == 2)
            {
                Args[0].Expect(Rect.Class);
                Args[1].Expect(Color.Class);
                if (Args[0].GetIVar("@x").Is(Ruby.Float.Class)) x = (int) Math.Round(Args[0].AutoGetIVar<Ruby.Float>("@x"));
                else x = Args[0].AutoGetIVar<Ruby.Integer>("@x");
                if (Args[0].GetIVar("@y").Is(Ruby.Float.Class)) y = (int) Math.Round(Args[0].AutoGetIVar<Ruby.Float>("@y"));
                else y = Args[0].AutoGetIVar<Ruby.Integer>("@y");
                if (Args[0].GetIVar("@width").Is(Ruby.Float.Class)) w = (int) Math.Round(Args[0].AutoGetIVar<Ruby.Float>("@width"));
                else w = Args[0].AutoGetIVar<Ruby.Integer>("@width");
                if (Args[0].GetIVar("@height").Is(Ruby.Float.Class)) h = (int) Math.Round(Args[0].AutoGetIVar<Ruby.Float>("@height"));
                else h = Args[0].AutoGetIVar<Ruby.Integer>("@height");
                c = Color.CreateColor(Args[1]);
            }
            else if (Args.Length == 5)
            {
                Args[0].Expect(Ruby.Integer.Class);
                Args[1].Expect(Ruby.Integer.Class);
                Args[2].Expect(Ruby.Integer.Class);
                Args[3].Expect(Ruby.Integer.Class);
                Args[4].Expect(Color.Class);
                x = Args.Get<Ruby.Integer>(0);
                y = Args.Get<Ruby.Integer>(1);
                w = Args.Get<Ruby.Integer>(2);
                h = Args.Get<Ruby.Integer>(3);
                c = Color.CreateColor(Args[4]);
            }
            AutoUnlock(Self);
            BitmapDictionary[Self.Pointer].DrawRect(x, y, w, h, c);
            AutoLock(Self);
            return Ruby.True;
        }

        protected static Ruby.Object fill_rect(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(2, 5);
            int x = 0,
                y = 0,
                w = 0,
                h = 0;
            odl.Color c = null;
            if (Args.Length == 2)
            {
                Args[0].Expect(Rect.Class);
                Args[1].Expect(Color.Class);
                if (Args[0].GetIVar("@x").Is(Ruby.Float.Class)) x = (int) Math.Round(Args[0].AutoGetIVar<Ruby.Float>("@x"));
                else x = Args[0].AutoGetIVar<Ruby.Integer>("@x");
                if (Args[0].GetIVar("@y").Is(Ruby.Float.Class)) y = (int) Math.Round(Args[0].AutoGetIVar<Ruby.Float>("@y"));
                else y = Args[0].AutoGetIVar<Ruby.Integer>("@y");
                if (Args[0].GetIVar("@width").Is(Ruby.Float.Class)) w = (int) Math.Round(Args[0].AutoGetIVar<Ruby.Float>("@width"));
                else w = Args[0].AutoGetIVar<Ruby.Integer>("@width");
                if (Args[0].GetIVar("@height").Is(Ruby.Float.Class)) h = (int) Math.Round(Args[0].AutoGetIVar<Ruby.Float>("@height"));
                else h = Args[0].AutoGetIVar<Ruby.Integer>("@height");
                c = Color.CreateColor(Args[1]);
            }
            else if (Args.Length == 5)
            {
                Args[0].Expect(Ruby.Integer.Class);
                Args[1].Expect(Ruby.Integer.Class);
                Args[2].Expect(Ruby.Integer.Class);
                Args[3].Expect(Ruby.Integer.Class);
                Args[4].Expect(Color.Class);
                x = Args.Get<Ruby.Integer>(0);
                y = Args.Get<Ruby.Integer>(1);
                w = Args.Get<Ruby.Integer>(2);
                h = Args.Get<Ruby.Integer>(3);
                c = Color.CreateColor(Args[4]);
            }
            AutoUnlock(Self);
            BitmapDictionary[Self.Pointer].FillRect(x, y, w, h, c);
            AutoLock(Self);
            return Ruby.True;
        }

        protected static Ruby.Object blt(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(1, 3, 4, 6, 9);
            int dx = 0,
                dy = 0,
                dw = 0,
                dh = 0;
            odl.Bitmap srcbmp = null;
            int sx = 0,
                sy = 0,
                sw = 0,
                sh = 0;
            if (Args.Length == 1) // bmp
            {
                Args[0].Expect(Bitmap.Class);
                srcbmp = BitmapDictionary[Args[0].Pointer];
                dw = srcbmp.Width;
                dh = srcbmp.Height;
                sw = srcbmp.Width;
                sh = srcbmp.Height;
            }else if (Args.Length == 3) // destrect, bmp, srcrect
            {
                Args[0].Expect(Rect.Class);
                Args[1].Expect(Bitmap.Class);
                Args[2].Expect(Rect.Class);
                if (Args[0].GetIVar("@x").Is(Ruby.Float.Class)) dx = (int) Math.Round(Args[0].AutoGetIVar<Ruby.Float>("@x"));
                else dx = Args[0].AutoGetIVar<Ruby.Integer>("@x");
                if (Args[0].GetIVar("@y").Is(Ruby.Float.Class)) dy = (int) Math.Round(Args[0].AutoGetIVar<Ruby.Float>("@y"));
                else dy = Args[0].AutoGetIVar<Ruby.Integer>("@y");
                if (Args[0].GetIVar("@width").Is(Ruby.Float.Class)) dw = (int) Math.Round(Args[0].AutoGetIVar<Ruby.Float>("@width"));
                else dw = Args[0].AutoGetIVar<Ruby.Integer>("@width");
                if (Args[0].GetIVar("@height").Is(Ruby.Float.Class)) dh = (int) Math.Round(Args[0].AutoGetIVar<Ruby.Float>("@height"));
                else dh = Args[0].AutoGetIVar<Ruby.Integer>("@height");
                srcbmp = BitmapDictionary[Args[1].Pointer];
                if (Args[2].GetIVar("@x").Is(Ruby.Float.Class)) sx = (int) Math.Round(Args[2].AutoGetIVar<Ruby.Float>("@x"));
                else sx = Args[2].AutoGetIVar<Ruby.Integer>("@x");
                if (Args[2].GetIVar("@y").Is(Ruby.Float.Class)) sy = (int) Math.Round(Args[2].AutoGetIVar<Ruby.Float>("@y"));
                else sy = Args[2].AutoGetIVar<Ruby.Integer>("@y");
                if (Args[2].GetIVar("@width").Is(Ruby.Float.Class)) sw = (int) Math.Round(Args[2].AutoGetIVar<Ruby.Float>("@width"));
                else sw = Args[2].AutoGetIVar<Ruby.Integer>("@width");
                if (Args[2].GetIVar("@height").Is(Ruby.Float.Class)) sh = (int) Math.Round(Args[2].AutoGetIVar<Ruby.Float>("@height"));
                else sh = Args[2].AutoGetIVar<Ruby.Integer>("@height");
            }
            else if (Args.Length == 4) // dx, dy, bmp, srcrect
            {
                Args[0].Expect(Ruby.Integer.Class);
                Args[1].Expect(Ruby.Integer.Class);
                Args[2].Expect(Bitmap.Class);
                Args[3].Expect(Rect.Class);
                dx = Args.Get<Ruby.Integer>(0);
                dy = Args.Get<Ruby.Integer>(1);
                srcbmp = BitmapDictionary[Args[2].Pointer];
                if (Args[3].GetIVar("@x").Is(Ruby.Float.Class)) sx = (int) Math.Round(Args[3].AutoGetIVar<Ruby.Float>("@x"));
                else sx = Args[3].AutoGetIVar<Ruby.Integer>("@x");
                if (Args[3].GetIVar("@y").Is(Ruby.Float.Class)) sy = (int) Math.Round(Args[3].AutoGetIVar<Ruby.Float>("@y"));
                else sy = Args[3].AutoGetIVar<Ruby.Integer>("@y");
                if (Args[3].GetIVar("@width").Is(Ruby.Float.Class)) sw = (int) Math.Round(Args[3].AutoGetIVar<Ruby.Float>("@width"));
                else sw = Args[3].AutoGetIVar<Ruby.Integer>("@width");
                if (Args[3].GetIVar("@height").Is(Ruby.Float.Class)) sh = (int) Math.Round(Args[3].AutoGetIVar<Ruby.Float>("@height"));
                else sh = Args[3].AutoGetIVar<Ruby.Integer>("@height");
                dw = sw;
                dh = sh;
            }
            else if (Args.Length == 6) // dx, dy, dw, dh, bmp, srcrect
            {
                Args[0].Expect(Ruby.Integer.Class);
                Args[1].Expect(Ruby.Integer.Class);
                Args[2].Expect(Ruby.Integer.Class);
                Args[3].Expect(Ruby.Integer.Class);
                Args[4].Expect(Bitmap.Class);
                Args[5].Expect(Rect.Class);
                dx = Args.Get<Ruby.Integer>(0);
                dy = Args.Get<Ruby.Integer>(1);
                dw = Args.Get<Ruby.Integer>(2);
                dh = Args.Get<Ruby.Integer>(3);
                srcbmp = BitmapDictionary[Args[4].Pointer];
                if (Args[5].GetIVar("@x").Is(Ruby.Float.Class)) sx = (int) Math.Round(Args[5].AutoGetIVar<Ruby.Float>("@x"));
                else sx = Args[5].AutoGetIVar<Ruby.Integer>("@x");
                if (Args[5].GetIVar("@y").Is(Ruby.Float.Class)) sy = (int) Math.Round(Args[5].AutoGetIVar<Ruby.Float>("@y"));
                else sy = Args[5].AutoGetIVar<Ruby.Integer>("@y");
                if (Args[5].GetIVar("@width").Is(Ruby.Float.Class)) sw = (int) Math.Round(Args[5].AutoGetIVar<Ruby.Float>("@width"));
                else sw = Args[5].AutoGetIVar<Ruby.Integer>("@width");
                if (Args[5].GetIVar("@height").Is(Ruby.Float.Class)) sh = (int) Math.Round(Args[5].AutoGetIVar<Ruby.Float>("@height"));
                else sh = Args[5].AutoGetIVar<Ruby.Integer>("@height");
            }
            else if (Args.Length == 9) // dx, dy, dw, dh, bmp, sx, sy, sw, sh
            {
                Args[0].Expect(Ruby.Integer.Class);
                Args[1].Expect(Ruby.Integer.Class);
                Args[2].Expect(Ruby.Integer.Class);
                Args[3].Expect(Ruby.Integer.Class);
                Args[4].Expect(Bitmap.Class);
                Args[5].Expect(Ruby.Integer.Class);
                Args[6].Expect(Ruby.Integer.Class);
                Args[7].Expect(Ruby.Integer.Class);
                Args[8].Expect(Ruby.Integer.Class);
                dx = Args.Get<Ruby.Integer>(0);
                dy = Args.Get<Ruby.Integer>(1);
                dw = Args.Get<Ruby.Integer>(2);
                dh = Args.Get<Ruby.Integer>(3);
                srcbmp = BitmapDictionary[Args[4].Pointer];
                sx = Args.Get<Ruby.Integer>(5);
                sy = Args.Get<Ruby.Integer>(6);
                sw = Args.Get<Ruby.Integer>(7);
                sh = Args.Get<Ruby.Integer>(8);
            }
            AutoUnlock(Self);
            BitmapDictionary[Self.Pointer].Build(dx, dy, dw, dh, srcbmp, sx, sy, sw, sh);
            AutoLock(Self);
            return Ruby.True;
        }

        protected static Ruby.Object text_size(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(1);
            Args[0].Expect(Ruby.String.Class);
            string text = Args.Get<Ruby.String>(0);
            BitmapDictionary[Self.Pointer].Font = Font.CreateFont(Self.GetIVar("@font"));
            return Rect.CreateRect(new odl.Rect(BitmapDictionary[Self.Pointer].TextSize(text)));
        }

        protected static Ruby.Object draw_text(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(5, 6);
            Args[0].Expect(Ruby.Integer.Class);
            Args[1].Expect(Ruby.Integer.Class);
            Args[2].Expect(Ruby.Integer.Class);
            Args[3].Expect(Ruby.Integer.Class);
            Args[4].Expect(Ruby.String.Class);
            int x = Args.Get<Ruby.Integer>(0);
            int y = Args.Get<Ruby.Integer>(1);
            int w = Args.Get<Ruby.Integer>(2);
            int h = Args.Get<Ruby.Integer>(3);
            string text = Args.Get<Ruby.String>(4);
            int align = 0;
            if (Args.Length == 6)
            {
                if (Args[5].Is(Ruby.Symbol.Class))
                {
                    if (Args.Get<Ruby.Symbol>(5) == ":left") align = 0;
                    else if (Args.Get<Ruby.Symbol>(5) == ":center" || Args.Get<Ruby.Symbol>(5) == ":middle") align = 1;
                    else if (Args.Get<Ruby.Symbol>(5) == ":right") align = 2;
                    else Ruby.Raise(Ruby.ErrorType.ArgumentError, "invalid alignment argument");
                }
                else
                {
                    Args[5].Expect(Ruby.Integer.Class);
                    align = Args.Get<Ruby.Integer>(5);
                    if (align < 0 || align > 2) Ruby.Raise(Ruby.ErrorType.ArgumentError, "invalid alignment argument");
                }
            }
            AutoUnlock(Self);
            BitmapDictionary[Self.Pointer].Font = Font.CreateFont(Self.GetIVar("@font"));
            odl.Color color = Color.CreateColor(Self.GetIVar("@font").GetIVar("@color"));
            odl.DrawOptions options = 0;
            bool outline = false;
            if (align == 0) options |= odl.DrawOptions.LeftAlign;
            else if (align == 1) options |= odl.DrawOptions.CenterAlign;
            else if (align == 2) options |= odl.DrawOptions.RightAlign;
            if (Self.GetIVar("@font").GetIVar("@outline") == Ruby.True) outline = true;
            if (outline)
            {
                odl.Color outline_color = Color.CreateColor(Self.GetIVar("@font").GetIVar("@outline_color"));
                BitmapDictionary[Self.Pointer].DrawText(text, new odl.Rect(x - 1, y - 1, w, h), outline_color, options);
                BitmapDictionary[Self.Pointer].DrawText(text, new odl.Rect(x, y - 1, w, h), outline_color, options);
                BitmapDictionary[Self.Pointer].DrawText(text, new odl.Rect(x + 1, y - 1, w, h), outline_color, options);
                BitmapDictionary[Self.Pointer].DrawText(text, new odl.Rect(x - 1, y, w, h), outline_color, options);
                BitmapDictionary[Self.Pointer].DrawText(text, new odl.Rect(x + 1, y, w, h), outline_color, options);
                BitmapDictionary[Self.Pointer].DrawText(text, new odl.Rect(x - 1, y + 1, w, h), outline_color, options);
                BitmapDictionary[Self.Pointer].DrawText(text, new odl.Rect(x, y + 1, w, h), outline_color, options);
                BitmapDictionary[Self.Pointer].DrawText(text, new odl.Rect(x + 1, y + 1, w, h), outline_color, options);
            }
            BitmapDictionary[Self.Pointer].DrawText(text, new odl.Rect(x, y, w, h), color, options);
            SDL2.SDL.SDL_SetTextureBlendMode(BitmapDictionary[Self.Pointer].Texture, SDL2.SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
            AutoLock(Self);
            return Ruby.True;
        }

        protected static Ruby.Object clear(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            AutoUnlock(Self);
            BitmapDictionary[Self.Pointer].Clear();
            AutoLock(Self);
            return Ruby.True;
        }

        protected static Ruby.Object dispose(Ruby.Object Self, Ruby.Array Args)
        {
            GuardDisposed(Self);
            Args.Expect(0);
            BitmapDictionary[Self.Pointer].Dispose();
            foreach (KeyValuePair<IntPtr, odl.Bitmap> kvp in BitmapDictionary)
            {
                if (kvp.Value == BitmapDictionary[Self.Pointer] && kvp.Key != Self.Pointer)
                {
                    Ruby.Object otherbmp = new Ruby.Object(kvp.Key);
                    otherbmp.SetIVar("@disposed", Ruby.True);
                }
            }
            BitmapDictionary.Remove(Self.Pointer);
            Self.SetIVar("@disposed", Ruby.True);
            return Ruby.True;
        }

        protected static Ruby.Object disposed(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(0);
            return Self.GetIVar("@disposed") == Ruby.True ? (Ruby.Object) Ruby.True : Ruby.False;
        }

        protected static void GuardDisposed(Ruby.Object Self)
        {
            if (Self.GetIVar("@disposed") == Ruby.True)
            {
                Ruby.Raise(Ruby.ErrorType.RuntimeError, "bitmap already disposed");
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Text;
using RubyDotNET;

namespace Peridot
{
    public class Sprite : RubyObject
    {
        public static IntPtr Class;
        public static Dictionary<IntPtr, ODL.Sprite> SpriteDictionary = new Dictionary<IntPtr, ODL.Sprite>();

        public static Class CreateClass()
        {
            Class c = new Class("Sprite");
            Class = c.Pointer;
            c.DefineClassMethod("new", _new);
            c.DefineMethod("initialize", initialize);
            c.DefineMethod("viewport", viewportget);
            c.DefineMethod("viewport=", viewportset);
            c.DefineMethod("bitmap", bitmapget);
            c.DefineMethod("bitmap=", bitmapset);
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
            c.DefineMethod("src_rect", src_rectget);
            c.DefineMethod("src_rect=", src_rectset);
            c.DefineMethod("visible", visibleget);
            c.DefineMethod("visible=", visibleset);
            c.DefineMethod("mirror_x", mirror_xget);
            c.DefineMethod("mirror_x=", mirror_xset);
            c.DefineMethod("mirror_y", mirror_yget);
            c.DefineMethod("mirror_y=", mirror_yset);
            c.DefineMethod("update", update);
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
            Internal.rb_funcallv(obj, Internal.rb_intern("initialize"), Args.Length, Args.Rubify());
            return obj;
        }

        protected static IntPtr initialize(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            IntPtr viewport = IntPtr.Zero;
            if (Args.Length == 0 || Args.Length == 1 && Args[0].Pointer == Internal.QNil)
                viewport = Internal.GetGlobalVariable("$__mainvp__");
            else if (Args.Length == 1)
                viewport = Args[0].Pointer;
            else ScanArgs(1, Args);
            Internal.SetIVar(self, "@viewport", viewport);
            Internal.SetIVar(self, "@x", Internal.LONG2NUM(0));
            Internal.SetIVar(self, "@y", Internal.LONG2NUM(0));
            Internal.SetIVar(self, "@z", Internal.LONG2NUM(0));
            Internal.SetIVar(self, "@opacity", Internal.LONG2NUM(255));
            Internal.SetIVar(self, "@angle", Internal.LONG2NUM(0));
            Internal.SetIVar(self, "@zoom_x", Internal.rb_float_new(1d));
            Internal.SetIVar(self, "@zoom_y", Internal.rb_float_new(1d));
            IntPtr src_rect = Rect.CreateRect(new ODL.Rect(0, 0, 0, 0));
            Internal.SetIVar(self, "@src_rect", src_rect);
            Internal.SetIVar(src_rect, "@__sprite__", self);
            Internal.SetIVar(self, "@visible", Internal.QTrue);
            Internal.SetIVar(self, "@mirror_x", Internal.QFalse);
            Internal.SetIVar(self, "@mirror_y", Internal.QFalse);

            if (!Viewport.ViewportDictionary.ContainsKey(viewport)) Internal.rb_raise(Internal.rb_eRuntimeError.Pointer, "invalid viewport");
            ODL.Sprite sprite = new ODL.Sprite(Viewport.ViewportDictionary[viewport]);
            if (SpriteDictionary.ContainsKey(self)) dispose(self, IntPtr.Zero);
            SpriteDictionary.Add(self, sprite);
            return self;
        }

        protected static IntPtr viewportget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@viewport");
        }

        protected static IntPtr viewportset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.SetIVar(self, "@viewport", Args[0].Pointer);
        }

        protected static IntPtr bitmapget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@bitmap");
        }

        protected static IntPtr bitmapset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            if (Args[0].Pointer == Internal.QNil) SpriteDictionary[self].Bitmap = null;
            else SpriteDictionary[self].Bitmap = Bitmap.BitmapDictionary[Args[0].Pointer];
            int x = 0,
                y = 0,
                w = SpriteDictionary[self].Bitmap == null ? 0 : SpriteDictionary[self].Bitmap.Width,
                h = SpriteDictionary[self].Bitmap == null ? 0 : SpriteDictionary[self].Bitmap.Height;
            SpriteDictionary[self].SrcRect.X = 0;
            SpriteDictionary[self].SrcRect.Y = 0;
            SpriteDictionary[self].SrcRect.Width = w;
            SpriteDictionary[self].SrcRect.Height = h;
            Internal.SetIVar(Internal.GetIVar(self, "@src_rect"), "@x", Internal.LONG2NUM(x));
            Internal.SetIVar(Internal.GetIVar(self, "@src_rect"), "@y", Internal.LONG2NUM(y));
            Internal.SetIVar(Internal.GetIVar(self, "@src_rect"), "@width", Internal.LONG2NUM(w));
            Internal.SetIVar(Internal.GetIVar(self, "@src_rect"), "@height", Internal.LONG2NUM(h));
            return Internal.SetIVar(self, "@bitmap", Args[0].Pointer);
        }

        protected static IntPtr xget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@x");
        }

        protected static IntPtr xset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            SpriteDictionary[self].X = (int) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.SetIVar(self, "@x", Args[0].Pointer);
        }

        protected static IntPtr yget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@y");
        }

        protected static IntPtr yset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            SpriteDictionary[self].Y = (int) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.SetIVar(self, "@y", Args[0].Pointer);
        }

        protected static IntPtr zget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@z");
        }

        protected static IntPtr zset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            SpriteDictionary[self].Z = (int) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.SetIVar(self, "@z", Args[0].Pointer);
        }

        protected static IntPtr oxget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@ox");
        }

        protected static IntPtr oxset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            SpriteDictionary[self].OX = (int) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.SetIVar(self, "@ox", Args[0].Pointer);
        }

        protected static IntPtr oyget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@oy");
        }

        protected static IntPtr oyset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            SpriteDictionary[self].OY = (int)Internal.NUM2LONG(Args[0].Pointer);
            return Internal.SetIVar(self, "@oy", Args[0].Pointer);
        }

        protected static IntPtr zoom_xget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@zoom_x");
        }

        protected static IntPtr zoom_xset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            SpriteDictionary[self].ZoomX = Internal.rb_num2dbl(Args[0].Pointer);
            return Internal.SetIVar(self, "@zoom_x", Args[0].Pointer);
        }

        protected static IntPtr zoom_yget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@zoom_y");
        }

        protected static IntPtr zoom_yset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            SpriteDictionary[self].ZoomY = Internal.rb_num2dbl(Args[0].Pointer);
            return Internal.SetIVar(self, "@zoom_y", Args[0].Pointer);
        }

        protected static IntPtr opacityget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@opacity");
        }

        protected static IntPtr opacityset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            SpriteDictionary[self].Opacity = (byte) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.SetIVar(self, "@opacity", Args[0].Pointer);
        }

        protected static IntPtr angleget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@angle");
        }

        protected static IntPtr angleset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            int integer = (int) Internal.NUM2LONG(Args[0].Pointer) % 360;
            IntPtr value = Internal.LONG2NUM(integer);
            SpriteDictionary[self].Angle = integer;
            return Internal.SetIVar(self, "@angle", value);
        }

        protected static IntPtr src_rectget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@src_rect");
        }

        protected static IntPtr src_rectset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            Internal.SetIVar(Internal.GetIVar(self, "@src_rect"), "@__sprite__", Internal.QNil);
            SpriteDictionary[self].SrcRect = Rect.CreateRect(Args[0].Pointer);
            Internal.SetIVar(Args[0].Pointer, "@__sprite__", self);
            return Internal.SetIVar(self, "@src_rect", Args[0].Pointer);
        }

        protected static IntPtr visibleget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@visible");
        }

        protected static IntPtr visibleset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            SpriteDictionary[self].Visible = Args[0].Pointer == Internal.QTrue;
            return Internal.SetIVar(self, "@visible", Args[0].Pointer);
        }

        protected static IntPtr mirror_xget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@mirror_x");
        }

        protected static IntPtr mirror_xset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            SpriteDictionary[self].MirrorX = Args[0].Pointer == Internal.QTrue;
            return Internal.SetIVar(self, "@mirror_x", Args[0].Pointer);
        }

        protected static IntPtr mirror_yget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@mirror_y");
        }

        protected static IntPtr mirror_yset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            SpriteDictionary[self].MirrorY = Args[0].Pointer == Internal.QTrue;
            return Internal.SetIVar(self, "@mirror_y", Args[0].Pointer);
        }

        protected static IntPtr update(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.QNil;
        }

        protected static IntPtr dispose(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            if (_args != IntPtr.Zero)
            {
                RubyArray Args = new RubyArray(_args);
                ScanArgs(0, Args);
            }
            SpriteDictionary[self].Dispose();
            foreach (KeyValuePair<IntPtr, ODL.Bitmap> kvp in Bitmap.BitmapDictionary)
            {
                if (kvp.Value == SpriteDictionary[self].Bitmap)
                {
                    Bitmap.BitmapDictionary.Remove(kvp.Key);
                    break;
                }
            }
            SpriteDictionary.Remove(self);
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
                Internal.rb_raise(Internal.rb_eRuntimeError.Pointer, "sprite already disposed");
            }
        }
    }
}

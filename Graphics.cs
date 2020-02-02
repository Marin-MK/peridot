using System;
using RubyDotNET;

namespace odlgss
{
    public class Graphics : RubyObject
    {
        public static IntPtr ClassPointer;

        public static Class CreateClass()
        {
            Class c = new Class("Graphics");
            ClassPointer = c.Pointer;
            c.DefineClassMethod("width", widthget);
            c.DefineClassMethod("width=", widthset);
            c.DefineClassMethod("height", heightget);
            c.DefineClassMethod("height=", heightset);
            return c;
        }

        public static int Width
        {
            get
            {
                return (int) Internal.NUM2LONG(Internal.rb_funcallv(Internal.Klasses[ClassPointer].Pointer, Internal.rb_intern("width"), 0, IntPtr.Zero));
            }
            set
            {
                Internal.rb_funcallv(Internal.Klasses[ClassPointer].Pointer, Internal.rb_intern("width="), 1, Internal.LONG2NUM(value));
            }
        }

        public static int Height
        {
            get
            {
                return (int) Internal.NUM2LONG(Internal.rb_funcallv(Internal.Klasses[ClassPointer].Pointer, Internal.rb_intern("height"), 0, IntPtr.Zero));
            }
            set
            {
                Internal.rb_funcallv(Internal.Klasses[ClassPointer].Pointer, Internal.rb_intern("height="), 1, Internal.LONG2NUM(value));
            }
        }

        static IntPtr widthget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@width"));
        }

        static IntPtr widthset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
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
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@height"), Args[0].Pointer);
        }
    }
}

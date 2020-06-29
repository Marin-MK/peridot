using System;
using RubyDotNET;

namespace peridot
{
    public class Table : RubyObject
    {
        public static IntPtr Class;

        public static Class CreateClass()
        {
            Class c = new Class("Table");
            Class = c.Pointer;
            c.DefineClassMethod("_load", _load, -2);
            c.DefineMethod("size", sizeget, -2);
            c.DefineMethod("size=", sizeset, -2);
            return c;
        }

        public static IntPtr sizeget(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@size");
        }

        public static IntPtr sizeset(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.SetIVar(self, "@size", Args[0].Pointer);
        }

        public static IntPtr _load(IntPtr self, IntPtr _args)//int argc, IntPtr[] argv, IntPtr self)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);

            RubyArray ary = new RubyArray(Internal.rb_funcallv(Args[0].Pointer, Internal.rb_intern("unpack"), 1, Internal.rb_str_new_cstr("LLLLLS*")));

            IntPtr obj = Internal.rb_funcallv(Class, Internal.rb_intern("allocate"), 0);

            Internal.SetIVar(obj, "@size", ary[0].Pointer);
            Internal.SetIVar(obj, "@xsize", ary[1].Pointer);
            Internal.SetIVar(obj, "@ysize", ary[2].Pointer);
            Internal.SetIVar(obj, "@zsize", ary[3].Pointer);
            // Fifth argument unused; data size (equal to @size)
            // Set @data to elements 5 to -1 and drop @size, @xsize, @ysize, @zsize and data_size.
            Internal.SetIVar(obj, "@data", Internal.rb_funcallv(ary.Pointer, Internal.rb_intern("drop"), 1, Internal.LONG2NUM(5)));

            return obj;
        }
    }
}

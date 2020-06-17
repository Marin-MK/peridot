using System;
using System.Collections.Generic;
using System.Text;
using RubyDotNET;

namespace peridot
{
    public class Font : RubyObject
    {
        public static IntPtr Class;

        public static Class CreateClass()
        {
            Class c = new Class("Font");
            Class = c.Pointer;
            c.DefineClassMethod("default_folder", default_folderget);
            c.DefineClassMethod("default_folder=", default_folderset);
            c.DefineClassMethod("default_name", default_nameget);
            c.DefineClassMethod("default_name=", default_nameset);
            c.DefineClassMethod("default_size", default_sizeget);
            c.DefineClassMethod("default_size=", default_sizeset);
            c.DefineClassMethod("default_color", default_colorget);
            c.DefineClassMethod("default_color=", default_colorset);
            c.DefineClassMethod("default_outline", default_outlineget);
            c.DefineClassMethod("default_outline=", default_outlineset);
            c.DefineClassMethod("default_outline_color", default_outline_colorget);
            c.DefineClassMethod("default_outline_color=", default_outline_colorset);
            c.DefineClassMethod("new", _new);
            c.DefineMethod("initialize", initialize);
            c.DefineMethod("name", nameget);
            c.DefineMethod("name=", nameset);
            c.DefineMethod("size", sizeget);
            c.DefineMethod("size=", sizeset);
            c.DefineMethod("color", colorget);
            c.DefineMethod("color=", colorset);
            c.DefineMethod("outline", outlineget);
            c.DefineMethod("outline=", outlineset);
            c.DefineMethod("outline_color", outline_colorget);
            c.DefineMethod("outline_color=", outline_colorset);
            return c;
        }

        public static IntPtr CreateFont()
        {
            if (Internal.GetIVar(Class, "@default_name") == Internal.QNil) Internal.rb_raise(Internal.rb_eRuntimeError.Pointer, "no default font name defined");
            if (Internal.GetIVar(Class, "@default_size") == Internal.QNil) Internal.rb_raise(Internal.rb_eRuntimeError.Pointer, "no default font size defined");
            return Internal.rb_funcallv(Class, Internal.rb_intern("new"), 2, new IntPtr[2]
            {
                Internal.GetIVar(Class, "@default_name"),
                Internal.GetIVar(Class, "@default_size")
            });
        }

        public static odl.Font CreateFont(IntPtr self)
        {
            string folder = null;
            if (Internal.GetIVar(Class, "@default_folder") != Internal.QNil) folder = new RubyString(Internal.GetIVar(Class, "@default_folder")).ToString();
            string name = new RubyString(Internal.GetIVar(self, "@name")).ToString();
            int size = (int) Internal.NUM2LONG(Internal.GetIVar(self, "@size"));
            if (!string.IsNullOrEmpty(folder) && odl.Font.Exists(folder + "/" + name))
                return odl.Font.Get(folder + "/" + name, size);
            else return odl.Font.Get(name, size);
        }

        public static IntPtr default_folderget(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(Class, "@default_folder");
        }

        public static IntPtr default_folderset(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.SetIVar(Class, "@default_folder", Args[0].Pointer);
        }

        public static IntPtr default_nameget(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(Class, "@default_name");
        }

        public static IntPtr default_nameset(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.SetIVar(Class, "@default_name", Args[0].Pointer);
        }

        public static IntPtr default_sizeget(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(Class, "@default_size");
        }

        public static IntPtr default_sizeset(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.SetIVar(Class, "@default_size", Args[0].Pointer);
        }

        public static IntPtr default_colorget(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(Class, "@default_color");
        }

        public static IntPtr default_colorset(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.SetIVar(Class, "@default_color", Args[0].Pointer);
        }

        public static IntPtr default_outlineget(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(Class, "@default_outline");
        }

        public static IntPtr default_outlineset(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.SetIVar(Class, "@default_outline", Args[0].Pointer);
        }

        public static IntPtr default_outline_colorget(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(Class, "@default_outline_color");
        }

        public static IntPtr default_outline_colorset(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.SetIVar(Class, "@default_outline_color", Args[0].Pointer);
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
            IntPtr Name = IntPtr.Zero,
                   Size = IntPtr.Zero;
            if (Args.Length == 2)
            {
                Name = Args[0].Pointer;
                Size = Args[1].Pointer;
            }
            else if (Args.Length == 1)
            {
                Name = Args[0].Pointer;
                if (Internal.GetIVar(Class, "@default_size") == Internal.QNil) Internal.rb_raise(Internal.rb_eRuntimeError.Pointer, "no default font size defined");
                Size = Internal.GetIVar(Class, "@default_size");
            }
            else if (Args.Length == 0)
            {
                if (Internal.GetIVar(Class, "@default_name") == Internal.QNil) Internal.rb_raise(Internal.rb_eRuntimeError.Pointer, "no default font name defined");
                if (Internal.GetIVar(Class, "@default_size") == Internal.QNil) Internal.rb_raise(Internal.rb_eRuntimeError.Pointer, "no default font size defined");
                Name = Internal.GetIVar(Class, "@default_name");
                Size = Internal.GetIVar(Class, "@default_size");
            }
            else ScanArgs(2, Args);

            Internal.SetIVar(self, "@name", Name);
            Internal.SetIVar(self, "@size", Size);
            Internal.SetIVar(self, "@color", Internal.GetIVar(Class, "@default_color"));
            Internal.SetIVar(self, "@outline", Internal.GetIVar(Class, "@default_outline"));
            Internal.SetIVar(self, "@outline_color", Internal.GetIVar(Class, "@default_outline_color"));

            return self;
        }

        protected static IntPtr nameget(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@name");
        }

        protected static IntPtr nameset(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.SetIVar(self, "@name", Args[0].Pointer);
        }

        protected static IntPtr sizeget(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@size");
        }

        protected static IntPtr sizeset(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.SetIVar(self, "@size", Args[0].Pointer);
        }

        protected static IntPtr colorget(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@color");
        }

        protected static IntPtr colorset(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.SetIVar(self, "@color", Args[0].Pointer);
        }

        protected static IntPtr outlineget(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@outline");
        }

        protected static IntPtr outlineset(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.SetIVar(self, "@outline", Args[0].Pointer);
        }

        protected static IntPtr outline_colorget(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@outline_color");
        }

        protected static IntPtr outline_colorset(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.SetIVar(self, "@outline_color", Args[0].Pointer);
        }
    }
}

using System;
using rubydotnet;

namespace peridot
{
    public static class Font
    {
        public static IntPtr Class;

        public static void Create()
        {
            Class = Ruby.Class.Define("Font");
            Ruby.Class.DefineClassMethod(Class, "default_folder", default_folderget);
            Ruby.Class.DefineClassMethod(Class, "default_folder=", default_folderset);
            Ruby.Class.DefineClassMethod(Class, "default_name", default_nameget);
            Ruby.Class.DefineClassMethod(Class, "default_name=", default_nameset);
            Ruby.Class.DefineClassMethod(Class, "default_size", default_sizeget);
            Ruby.Class.DefineClassMethod(Class, "default_size=", default_sizeset);
            Ruby.Class.DefineClassMethod(Class, "default_color", default_colorget);
            Ruby.Class.DefineClassMethod(Class, "default_color=", default_colorset);
            Ruby.Class.DefineClassMethod(Class, "default_outline", default_outlineget);
            Ruby.Class.DefineClassMethod(Class, "default_outline=", default_outlineset);
            Ruby.Class.DefineClassMethod(Class, "default_outline_color", default_outline_colorget);
            Ruby.Class.DefineClassMethod(Class, "default_outline_color=", default_outline_colorset);
            Ruby.Class.DefineMethod(Class, "initialize", initialize);
            Ruby.Class.DefineMethod(Class, "name", nameget);
            Ruby.Class.DefineMethod(Class, "name=", nameset);
            Ruby.Class.DefineMethod(Class, "size", sizeget);
            Ruby.Class.DefineMethod(Class, "size=", sizeset);
            Ruby.Class.DefineMethod(Class, "color", colorget);
            Ruby.Class.DefineMethod(Class, "color=", colorset);
            Ruby.Class.DefineMethod(Class, "outline", outlineget);
            Ruby.Class.DefineMethod(Class, "outline=", outlineset);
            Ruby.Class.DefineMethod(Class, "outline_color", outline_colorget);
            Ruby.Class.DefineMethod(Class, "outline_color=", outline_colorset);
        }

        public static IntPtr CreateFont()
        {
            if (Ruby.GetIVar(Class, "@default_name") == Ruby.Nil) Ruby.Raise(Ruby.ErrorType.RuntimeError, "no default font name defined");
            if (Ruby.GetIVar(Class, "@default_size") == Ruby.Nil) Ruby.Raise(Ruby.ErrorType.RuntimeError, "no default font size defined");
            return Ruby.Funcall(Class, "new", Ruby.GetIVar(Class, "@default_name"), Ruby.GetIVar(Class, "@default_size"));
        }

        public static odl.Font CreateFont(IntPtr Self)
        {
            string folder = null;
            if (Ruby.GetIVar(Class, "@default_folder") != Ruby.Nil) folder = Ruby.String.FromPtr(Ruby.GetIVar(Class, "@default_folder"));
            string name = Ruby.String.FromPtr(Ruby.GetIVar(Self, "@name"));
            int size = (int) Ruby.Integer.FromPtr(Ruby.GetIVar(Self, "@size"));
            if (!string.IsNullOrEmpty(folder) && odl.Font.Exists(folder + "/" + name))
                return odl.Font.Get(folder + "/" + name, size);
            else return odl.Font.Get(name, size);
        }

        static IntPtr default_folderget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@default_folder");
        }

        static IntPtr default_folderset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "String", "NilClass");
            return Ruby.SetIVar(Self, "@default_folder", Ruby.Array.Get(Args, 0));
        }

        static IntPtr default_nameget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@default_name");
        }

        static IntPtr default_nameset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "String", "NilClass");
            return Ruby.SetIVar(Self, "@default_name", Ruby.Array.Get(Args, 0));
        }

        static IntPtr default_sizeget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@default_size");
        }

        static IntPtr default_sizeset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "Integer", "NilClass");
            return Ruby.SetIVar(Self, "@default_size", Ruby.Array.Get(Args, 0));
        }

        static IntPtr default_colorget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@default_color");
        }

        public static IntPtr default_colorset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "Color", "NilClass");
            return Ruby.SetIVar(Self, "@default_color", Ruby.Array.Get(Args, 0));
        }

        static IntPtr default_outlineget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@default_outline");
        }

        static IntPtr default_outlineset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "TrueClass", "FalseClass", "NilClass");
            return Ruby.SetIVar(Self, "@default_outline", Ruby.Array.Get(Args, 0) == Ruby.True ? Ruby.True : Ruby.False);
        }

        static IntPtr default_outline_colorget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@default_outline_color");
        }

        static IntPtr default_outline_colorset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "Color", "NilClass");
            return Ruby.SetIVar(Self, "@default_outline_color", Ruby.Array.Get(Args, 0));
        }

        static IntPtr initialize(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0, 1, 2);
            IntPtr Name = IntPtr.Zero,
                   Size = IntPtr.Zero;
            long len = Ruby.Array.Length(Args);
            if (len == 0)
            {
                if (Ruby.GetIVar(Class, "@default_name") == Ruby.Nil) Ruby.Raise(Ruby.ErrorType.RuntimeError, "no default font name defined");
                if (Ruby.GetIVar(Class, "@default_size") == Ruby.Nil) Ruby.Raise(Ruby.ErrorType.RuntimeError, "no default font size defined");
                Name = Ruby.GetIVar(Class, "@default_name");
                Size = Ruby.GetIVar(Class, "@default_size");
            }
            else if (len == 1)
            {
                Ruby.Array.Expect(Args, 0, "String");
                Name = Ruby.Array.Get(Args, 0);
                if (Ruby.GetIVar(Class, "@default_size") == Ruby.Nil) Ruby.Raise(Ruby.ErrorType.RuntimeError, "no default font size defined");
                Size = Ruby.GetIVar(Class, "@default_size");
            }
            else if (len == 2)
            {
                Ruby.Array.Expect(Args, 0, "String");
                Ruby.Array.Expect(Args, 1, "Integer");
                Name = Ruby.Array.Get(Args, 0);
                Size = Ruby.Array.Get(Args, 1);
            }
            Ruby.SetIVar(Self, "@name", Name);
            Ruby.SetIVar(Self, "@size", Size);
            Ruby.SetIVar(Self, "@color", Ruby.GetIVar(Class, "@default_color"));
            Ruby.SetIVar(Self, "@outline", Ruby.GetIVar(Class, "@default_outline"));
            Ruby.SetIVar(Self, "@outline_color", Ruby.GetIVar(Class, "@default_outline_color"));
            return Self;
        }

        static IntPtr nameget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@name");
        }

        static IntPtr nameset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "String");
            return Ruby.SetIVar(Self, "@name", Ruby.Array.Get(Args, 0));
        }

        static IntPtr sizeget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@size");
        }

        static IntPtr sizeset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "Integer");
            return Ruby.SetIVar(Self, "@size", Ruby.Array.Get(Args, 0));
        }

        static IntPtr colorget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@color");
        }

        static IntPtr colorset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "Color");
            return Ruby.SetIVar(Self, "@color", Ruby.Array.Get(Args, 0));
        }

        static IntPtr outlineget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@outline");
        }

        static IntPtr outlineset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "TrueClass", "FalseClass", "NilClass");
            return Ruby.SetIVar(Self, "@outline", Ruby.Array.Get(Args, 0) == Ruby.True ? Ruby.True : Ruby.False);
        }

        static IntPtr outline_colorget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@outline_color");
        }

        static IntPtr outline_colorset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "Color");
            return Ruby.SetIVar(Self, "@outline_color", Ruby.Array.Get(Args, 0));
        }
    }
}

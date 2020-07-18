using System;
using rubydotnet;

namespace peridot
{
    public class Font : Ruby.Object
    {
        public new static string KlassName = "Font";
        public new static Ruby.Class Class;

        public Font(IntPtr Pointer) : base(Pointer) { }

        public static void Create()
        {
            Ruby.Class c = Ruby.Class.DefineClass<Font>(KlassName);
            Class = c;
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
        }

        public static Font CreateFont()
        {
            if (Class.GetIVar("@default_name") == Ruby.Nil) Ruby.Raise(Ruby.ErrorType.RuntimeError, "no default font name defined");
            if (Class.GetIVar("@default_size") == Ruby.Nil) Ruby.Raise(Ruby.ErrorType.RuntimeError, "no default font size defined");
            return Class.AutoFuncall<Font>("new",
                Class.GetIVar("@default_name"),
                Class.GetIVar("@default_size")
            );
        }

        public static odl.Font CreateFont(Ruby.Object Self)
        {
            string folder = null;
            if (Class.GetIVar("@default_folder") != Ruby.Nil) folder = Class.AutoGetIVar<Ruby.String>("@default_folder");
            string name = Self.AutoGetIVar<Ruby.String>("@name");
            int size = Self.AutoGetIVar<Ruby.Integer>("@size");
            if (!string.IsNullOrEmpty(folder) && odl.Font.Exists(folder + "/" + name))
                return odl.Font.Get(folder + "/" + name, size);
            else return odl.Font.Get(name, size);
        }

        public static Ruby.Object default_folderget(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(0);
            return Self.GetIVar("@default_folder");
        }

        public static Ruby.Object default_folderset(Ruby.Object self, Ruby.Array Args)
        {
            Args.Expect(1);
            Args[0].Expect(Ruby.String.Class, Ruby.NilClass.Class);
            return Class.SetIVar("@default_folder", Args[0]);
        }

        public static Ruby.Object default_nameget(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(0);
            return Class.GetIVar("@default_name");
        }

        public static Ruby.Object default_nameset(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(1);
            Args[0].Expect(Ruby.String.Class, Ruby.NilClass.Class);
            return Class.SetIVar("@default_name", Args[0]);
        }

        public static Ruby.Object default_sizeget(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(0);
            return Class.GetIVar("@default_size");
        }

        public static Ruby.Object default_sizeset(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(1);
            Args[0].Expect(Ruby.Integer.Class, Ruby.NilClass.Class);
            return Class.SetIVar("@default_size", Args[0]);
        }

        public static Ruby.Object default_colorget(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(0);
            return Class.GetIVar("@default_color");
        }

        public static Ruby.Object default_colorset(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(1);
            Args[0].Expect(Color.Class, Ruby.NilClass.Class);
            return Class.SetIVar("@default_color", Args[0]);
        }

        public static Ruby.Object default_outlineget(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(0);
            return Class.GetIVar("@default_outline");
        }

        public static Ruby.Object default_outlineset(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(1);
            Args[0].Expect(Ruby.TrueClass.Class, Ruby.FalseClass.Class, Ruby.NilClass.Class);
            return Class.SetIVar("@default_outline", Args[0] == Ruby.True ? (Ruby.Object) Ruby.True : Ruby.False);
        }

        public static Ruby.Object default_outline_colorget(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(0);
            return Class.GetIVar("@default_outline_color");
        }

        public static Ruby.Object default_outline_colorset(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(1);
            Args[0].Expect(Color.Class, Ruby.NilClass.Class);
            return Self.SetIVar("@default_outline_color", Args[0]);
        }

        protected static Ruby.Object initialize(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(0, 1, 2);
            Ruby.Object Name = null,
                        Size = null;
            if (Args.Length == 2)
            {
                Args[0].Expect(Ruby.String.Class);
                Args[1].Expect(Ruby.Integer.Class);
                Name = Args[0];
                Size = Args[1];
            }
            else if (Args.Length == 1)
            {
                Args[0].Expect(Ruby.String.Class);
                Name = Args[0];
                if (Class.GetIVar("@default_size") == Ruby.Nil) Ruby.Raise(Ruby.ErrorType.RuntimeError, "no default font size defined");
                Size = Class.GetIVar("@default_size");
            }
            else if (Args.Length == 0)
            {
                if (Class.GetIVar("@default_name") == Ruby.Nil) Ruby.Raise(Ruby.ErrorType.RuntimeError, "no default font name defined");
                if (Class.GetIVar("@default_size") == Ruby.Nil) Ruby.Raise(Ruby.ErrorType.RuntimeError, "no default font size defined");
                Name = Class.GetIVar("@default_name");
                Size = Class.GetIVar("@default_size");
            }
            Self.SetIVar("@name", Name);
            Self.SetIVar("@size", Size);
            Self.SetIVar("@color", Class.GetIVar("@default_color"));
            Self.SetIVar("@outline", Class.GetIVar("@default_outline"));
            Self.SetIVar("@outline_color", Class.GetIVar("@default_outline_color"));
            return Self;
        }

        protected static Ruby.Object nameget(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(0);
            return Self.GetIVar("@name");
        }

        protected static Ruby.Object nameset(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(1);
            Args[0].Expect(Ruby.String.Class);
            return Self.SetIVar("@name", Args[0]);
        }

        protected static Ruby.Object sizeget(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(0);
            return Self.GetIVar("@size");
        }

        protected static Ruby.Object sizeset(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(1);
            Args[0].Expect(Ruby.Integer.Class);
            return Self.SetIVar("@size", Args[0]);
        }

        protected static Ruby.Object colorget(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(0);
            return Self.GetIVar("@color");
        }

        protected static Ruby.Object colorset(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(1);
            Args[0].Expect(Color.Class);
            return Self.SetIVar("@color", Args[0]);
        }

        protected static Ruby.Object outlineget(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(0);
            return Self.GetIVar("@outline");
        }

        protected static Ruby.Object outlineset(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(1);
            Args[0].Expect(Ruby.TrueClass.Class, Ruby.FalseClass.Class, Ruby.NilClass.Class);
            return Self.SetIVar("@outline", Args[0] == Ruby.True ? (Ruby.Object) Ruby.True : Ruby.False);
        }

        protected static Ruby.Object outline_colorget(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(0);
            return Self.GetIVar("@outline_color");
        }

        protected static Ruby.Object outline_colorset(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(1);
            Args[0].Expect(Color.Class);
            return Self.SetIVar("@outline_color", Args[0]);
        }
    }
}

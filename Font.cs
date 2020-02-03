using System;
using System.Collections.Generic;
using RubyDotNET;

namespace odlgss
{
    public class Font : RubyObject
    {
        public ODL.Font FontObject;
        public static IntPtr ClassPointer;
        public static Dictionary<IntPtr, Font> Fonts = new Dictionary<IntPtr, Font>();

        public static string DefaultName
        {
            get
            {
                return new RubyString(Internal.rb_funcallv(ClassPointer, Internal.rb_intern("default_name"), 0)).ToString();
            }
            set
            {
                Internal.rb_funcallv(ClassPointer, Internal.rb_intern("default_name="), 1, Internal.rb_str_new_cstr(value));
            }
        }
        public static int DefaultSize
        {
            get
            {
                return (int) Internal.NUM2LONG(Internal.rb_funcallv(ClassPointer, Internal.rb_intern("default_size"), 0));
            }
            set
            {
                Internal.rb_funcallv(ClassPointer, Internal.rb_intern("default_size="), 1, Internal.LONG2NUM(value));
            }
        }
        public static bool DefaultBold
        {
            get
            {
                return Internal.rb_funcallv(ClassPointer, Internal.rb_intern("default_bold"), 0) == Internal.QTrue;
            }
            set
            {
                Internal.rb_funcallv(ClassPointer, Internal.rb_intern("default_bold="), 1, value ? Internal.QTrue : Internal.QFalse);
            }
        }
        public static bool DefaultItalic
        {
            get
            {
                return Internal.rb_funcallv(ClassPointer, Internal.rb_intern("default_italic"), 0) == Internal.QTrue;
            }
            set
            {
                Internal.rb_funcallv(ClassPointer, Internal.rb_intern("default_italic="), 1, value ? Internal.QTrue : Internal.QFalse);
            }
        }
        public static bool DefaultOutline
        {
            get
            {
                return Internal.rb_funcallv(ClassPointer, Internal.rb_intern("default_outline"), 0) == Internal.QTrue;
            }
            set
            {
                Internal.rb_funcallv(ClassPointer, Internal.rb_intern("default_outline="), 1, value ? Internal.QTrue : Internal.QFalse);
            }
        }
        public static Color DefaultColor
        {
            get
            {
                IntPtr ptr = Internal.rb_funcallv(ClassPointer, Internal.rb_intern("default_color"), 0);
                if (ptr == Internal.QNil) return null;
                return Color.Colors[ptr];
            }
            set
            {
                Internal.rb_funcallv(ClassPointer, Internal.rb_intern("default_color="), 1, value.Pointer);
            }
        }

        public static Class CreateClass()
        {
            Class c = new Class("Font");
            ClassPointer = c.Pointer;
            c.DefineClassMethod("new", New);
            c.DefineMethod("initialize", initialize);
            c.DefineClassMethod("default_name", default_nameget);
            c.DefineClassMethod("default_name=", default_nameset);
            c.DefineClassMethod("default_size", default_sizeget);
            c.DefineClassMethod("default_size=", default_sizeset);
            c.DefineClassMethod("default_bold", default_boldget);
            c.DefineClassMethod("default_bold=", default_boldset);
            c.DefineClassMethod("default_italic", default_italicget);
            c.DefineClassMethod("default_italic=", default_italicset);
            c.DefineClassMethod("default_outline", default_outlineget);
            c.DefineClassMethod("default_outline=", default_outlineset);
            c.DefineClassMethod("default_color", default_colorget);
            c.DefineClassMethod("default_color=", default_colorset);
            c.DefineMethod("name", nameget);
            c.DefineMethod("name=", nameset);
            c.DefineMethod("size", sizeget);
            c.DefineMethod("size=", sizeset);
            c.DefineMethod("bold", boldget);
            c.DefineMethod("bold=", boldset);
            c.DefineMethod("italic", italicget);
            c.DefineMethod("italic=", italicset);
            c.DefineMethod("color", colorget);
            c.DefineMethod("color=", colorset);
            return c;
        }

        static IntPtr default_nameget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@default_name"));
        }
        static IntPtr default_nameset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@default_name"), Args[0].Pointer);
        }

        static IntPtr default_sizeget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@default_size"));
        }
        static IntPtr default_sizeset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@default_size"), Args[0].Pointer);
        }

        static IntPtr default_boldget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@default_bold"));
        }
        static IntPtr default_boldset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@default_bold"), Args[0].Pointer);
        }

        static IntPtr default_italicget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@default_italic"));
        }
        static IntPtr default_italicset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@default_italic"), Args[0].Pointer);
        }

        static IntPtr default_outlineget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@default_outline"));
        }
        static IntPtr default_outlineset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@default_outline"), Args[0].Pointer);
        }

        static IntPtr default_colorget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@default_color"));
        }
        static IntPtr default_colorset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@default_color"), Args[0].Pointer);
        }

        private Font()
        {
            this.Pointer = Internal.rb_funcallv(ClassPointer, Internal.rb_intern("allocate"), 0);
            Fonts[Pointer] = this;
        }

        public static Font New(string Name, int Size)
        {
            IntPtr ptr = Internal.rb_funcallv(ClassPointer, Internal.rb_intern("new"), 2,
                Internal.rb_str_new_cstr(Name),
                Internal.LONG2NUM(Size)
            );
            return Fonts[ptr];
        }
        public void Initialize(string Name, int Size)
        {
            FontObject = new ODL.Font(Name, Size);
            this.Name = Name;
            this.Size = Size;
            this.Bold = Font.DefaultBold;
            this.Italic = Font.DefaultItalic;
            this.Color = Font.DefaultColor;
        }

        public static Font New(string Name)
        {
            return Font.New(Name, DefaultSize);
        }
        public void Initialize(string Name)
        {
            this.Initialize(Name, DefaultSize);
        }

        public static Font New()
        {
            return Font.New(DefaultName, DefaultSize);
        }
        public void Initialize()
        {
            this.Initialize(DefaultName, DefaultSize);
        }

        public string Name
        {
            get
            {
                return new RubyString(Internal.rb_funcallv(Pointer, Internal.rb_intern("name"), 0)).ToString();
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("name="), 1, Internal.rb_str_new_cstr(value));
            }
        }
        public int Size
        {
            get
            {
                return (int) Internal.NUM2LONG(Internal.rb_funcallv(Pointer, Internal.rb_intern("size"), 0));
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("size="), 1, Internal.LONG2NUM(value));
            }
        }
        public bool Bold
        {
            get
            {
                return Internal.rb_funcallv(Pointer, Internal.rb_intern("bold"), 0) == Internal.QTrue;
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("bold="), 1, value ? Internal.QTrue : Internal.QFalse);
            }
        }
        public bool Italic
        {
            get
            {
                return Internal.rb_funcallv(Pointer, Internal.rb_intern("italic"), 0) == Internal.QTrue;
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("italic="), 1, value ? Internal.QTrue : Internal.QFalse);
            }
        }
        public Color Color
        {
            get
            {
                IntPtr ptr = Internal.rb_funcallv(Pointer, Internal.rb_intern("color"), 0);
                if (ptr == Internal.QNil) return null;
                return Color.Colors[ptr];
            }
            set
            {
                Internal.rb_funcallv(Pointer, Internal.rb_intern("color="), 1, value.Pointer);
            }
        }

        static IntPtr New(IntPtr _self, IntPtr _args)
        {
            Font f = new Font();
            RubyArray Args = new RubyArray(_args);
            IntPtr[] newargs = new IntPtr[Args.Length];
            for (int i = 0; i < Args.Length; i++) newargs[i] = Args[i].Pointer;
            Internal.rb_funcallv(f.Pointer, Internal.rb_intern("initialize"), Args.Length, newargs);
            return f.Pointer;
        }
        static IntPtr initialize(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);

            Font f = Fonts[_self];
            if (Args.Length == 2)
            {
                string Name = new RubyString(Args[0].Pointer).ToString();
                int Size = (int) Internal.NUM2LONG(Internal.rb_funcallv(Args[1].Pointer, Internal.rb_intern("to_i"), 0));
                f.Initialize(Name, Size);
            }
            else if (Args.Length == 1)
            {
                string Name = new RubyString(Args[0].Pointer).ToString();
                int Size = Font.DefaultSize;
                f.Initialize(Name, Size);
            }
            else
            {
                string Name = Font.DefaultName;
                int Size = Font.DefaultSize;
                f.Initialize(Name, Size);
            }

            return _self;
        }

        static IntPtr nameget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@name"));
        }
        static IntPtr nameset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            Font.Fonts[_self].FontObject.SetName(new RubyString(Args[0].Pointer).ToString());
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@name"), Args[0].Pointer);
        }

        static IntPtr sizeget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@size"));
        }
        static IntPtr sizeset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            Font.Fonts[_self].FontObject.SetSize((int) Internal.NUM2LONG(Internal.rb_funcallv(Args[0].Pointer, Internal.rb_intern("to_i"), 0)));
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@size"), Args[0].Pointer);
        }

        static IntPtr boldget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@bold"));
        }
        static IntPtr boldset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@bold"), Args[0].Pointer);
        }

        static IntPtr italicget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@italic"));
        }
        static IntPtr italicset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.rb_ivar_set(_self, Internal.rb_intern("@italic"), Args[0].Pointer);
        }

        static IntPtr colorget(IntPtr _self, IntPtr _args)
        {
            return Internal.rb_ivar_get(_self, Internal.rb_intern("@color"));
        }
        static IntPtr colorset(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            IntPtr Value = Internal.rb_ivar_set(_self, Internal.rb_intern("@color"), Args[0].Pointer);
            return Value;
        }
    }
}

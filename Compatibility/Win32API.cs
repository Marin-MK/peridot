using System;
using RubyDotNET;

namespace peridot
{
    public class Win32API : RubyObject
    {
        public static IntPtr Class;

        public static Class CreateClass()
        {
            Class c = new Class("Win32API");
            Class = c.Pointer;
            c.DefineClassMethod("new", _new);
            c.DefineMethod("initialize", initialize);
            c.DefineMethod("call", call);
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
            ScanArgs(4, Args);
            Internal.SetIVar(self, "@library", Args[0].Pointer);
            Internal.SetIVar(self, "@function", Args[1].Pointer);
            Internal.SetIVar(self, "@in_args", Args[2].Pointer);
            Internal.SetIVar(self, "@out_arg", Args[3].Pointer);
            return self;
        }

        protected static IntPtr call(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            string library = new RubyString(Internal.GetIVar(self, "@library")).ToString();
            string function = new RubyString(Internal.GetIVar(self, "@function")).ToString();
            IntPtr nil = Internal.QNil;
            if (library == "user32" || library == "user32.dll")
            {
                if (function == "RegisterHotKey") return nil;
                else if (function == "GetCursorPos")
                {
                    RubyArray Array = new RubyArray(2);
                    Array[0] = new RubyInt(odl.Graphics.LastMouseEvent.X);
                    Array[1] = new RubyInt(odl.Graphics.LastMouseEvent.Y);
                    return Array.Pointer;
                }
                else if (function == "GetWindowRect")
                {
                    RubyArray Array = new RubyArray(4);
                    Array[0] = new RubyInt(Program.MainWindow.X);
                    Array[1] = new RubyInt(Program.MainWindow.Y);
                    Array[2] = new RubyInt(Program.MainWindow.Width);
                    Array[3] = new RubyInt(Program.MainWindow.Height);
                    return Array.Pointer;
                }
                else if (function == "SetWindowPos")
                {
                    return Internal.QTrue;
                }
                else if (function == "FindWindowEx")
                {
                    if (Args.Length >= 3 && new RubyString(Args[2].Pointer).ToString() == "RGSS Player") return Internal.LONG2NUM(1337);
                    else return Internal.LONG2NUM(0);
                }
                else if (function == "GetWindowThreadProcessId")
                {
                    if (Args.Length >= 2 && (int)Internal.NUM2LONG(Args[0].Pointer) == 1337)
                        return Internal.LONG2NUM(System.Threading.Thread.CurrentThread.ManagedThreadId);
                    else return Internal.LONG2NUM(0);
                }
                else if (function == "GetAsyncKeyState") return Internal.LONG2NUM(0);
                else if (function == "GetForegroundWindow") return Internal.LONG2NUM(Program.MainWindow.SDL_Window.ToInt64());
                else
                {
                    Internal.rb_raise(Internal.rb_eRuntimeError.Pointer, $"fake win32api has no implementation for '{function}' in '{library}'");
                }
            }
            else if (library == "Kernel32" || library == "Kernel32.dll" || library == "kernel32" || library == "kernel32.dll")
            {
                if (function == "RegisterHotKey") return nil;
                else if (function == "GetCurrentThreadId") return Internal.LONG2NUM(System.Threading.Thread.CurrentThread.ManagedThreadId);
                else if (function == "GetCurrentProcess") return nil;
                else if (function == "SetPriorityClass") return nil;
                else
                {
                    Internal.rb_raise(Internal.rb_eRuntimeError.Pointer, $"fake win32api has no implementation for '{function}' in '{library}'");
                }
            }
            else if (library == "shell32" || library == "Shell32" || library == "shell32.dll" || library == "Shell32.dll")
            {
                if (function == "SHGetKnownFolderPath")
                {
                    //return new RubyString("C:\\Users\\Eigenaar\\Saved Games").Pointer;
                    return new RubyString(System.IO.Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "Saved Games")).Pointer;
                }
                else
                {
                    Internal.rb_raise(Internal.rb_eRuntimeError.Pointer, $"fake win32api has no implementation for '{function}' in '{library}'");
                }
            }
            else if (library == "ole32" || library == "ole32.dll" || library == "Ole32" || library == "Ole32.dll")
            {
                if (function == "CoTaskMemFree")
                {
                    return Internal.QNil;
                }
                else
                {
                    Internal.rb_raise(Internal.rb_eRuntimeError.Pointer, $"fake win32api has no implementation for '{function}' in '{library}'");
                }
            }
            else
            {
                Internal.rb_raise(Internal.rb_eRuntimeError.Pointer, $"fake win32api has no implementations for library '{library}'");
            }
            return nil;
        }

        public static IntPtr load_data(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            IntPtr file = Internal.rb_funcallv(Internal.rb_cFile.Pointer, Internal.rb_intern("open"), 2, new IntPtr[] { Args[0].Pointer, Internal.rb_str_new_cstr("rb") });
            IntPtr data = Internal.rb_funcallv(file, Internal.rb_intern("read"), 0);
            Internal.rb_funcallv(file, Internal.rb_intern("close"), 0);
            return Internal.rb_marshal_load(data);
        }

        public static IntPtr save_data(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(2, Args);
            IntPtr file = Internal.rb_funcallv(Internal.rb_cFile.Pointer, Internal.rb_intern("new"), 2, new IntPtr[] { Args[0].Pointer, Internal.rb_str_new_cstr("wb") });
            Internal.rb_marshal_dump(file, Args[1].Pointer);
            Internal.rb_funcallv(file, Internal.rb_intern("close"), 0);
            return Internal.QTrue;
        }

        public static IntPtr criticalget(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.QFalse;
        }

        public static IntPtr criticalset(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Args[0].Pointer;
        }

        public static IntPtr freeze(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.QNil;
        }

        public static IntPtr frame_reset(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.QNil;
        }

        public static IntPtr bush_depthget(IntPtr self, IntPtr _args)
        {
            Sprite.GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@bush_depth");
        }

        public static IntPtr bush_depthset(IntPtr self, IntPtr _args)
        {
            Sprite.GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.SetIVar(self, "@bush_depth", Args[0].Pointer);
        }

        public static IntPtr blend_typeget(IntPtr self, IntPtr _args)
        {
            Sprite.GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@blend_type");
        }

        public static IntPtr blend_typeset(IntPtr self, IntPtr _args)
        {
            Sprite.GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.SetIVar(self, "@blend_type", Args[0].Pointer);
        }
    }
}

using System;
using RubyDotNET;

namespace odlgss
{
    public class Input : RubyObject
    {
        public static IntPtr ModulePointer;

        public static Module CreateModule()
        {
            Module m = new Module("Input");
            ModulePointer = m.Pointer;
            m.DefineClassMethod("update", update);
            m.DefineClassMethod("trigger?", trigger);
            m.DefineClassMethod("press?", press);
            return m;
        }

        public static void Update()
        {
            Internal.rb_funcallv(ModulePointer, Internal.rb_intern("update"), 0);
        }
        public static bool Trigger(long Hex)
        {
            return Internal.rb_funcallv(ModulePointer, Internal.rb_intern("trigger?"), 1, Internal.LONG2NUM(Hex)) == Internal.QTrue;
        }
        public static bool Press(long Hex)
        {
            return Internal.rb_funcallv(ModulePointer, Internal.rb_intern("press?"), 1, Internal.LONG2NUM(Hex)) == Internal.QTrue;
        }

        static IntPtr update(IntPtr _self, IntPtr _args)
        {
            ODL.Graphics.UpdateInput();
            return _self;
        }

        static IntPtr trigger(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            SDL2.SDL.SDL_Keycode key = (SDL2.SDL.SDL_Keycode) Internal.NUM2LONG(Args[0].Pointer);
            return ODL.Input.Trigger(key) ? Internal.QTrue : Internal.QFalse;
        }

        static IntPtr press(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return ODL.Input.Press((SDL2.SDL.SDL_Keycode) Internal.NUM2LONG(Args[0].Pointer)) ? Internal.QTrue : Internal.QFalse;
        }
    }
}

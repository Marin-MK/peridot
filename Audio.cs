using System;
using System.Collections.Generic;
using RubyDotNET;

namespace odlgss
{
    public class Audio : RubyObject
    {
        public static IntPtr ModulePointer;

        public static Module CreateModule()
        {
            Module m = new Module("Audio");
            ModulePointer = m.Pointer;
            m.DefineClassMethod("se_play", se_play);
            return m;
        }

        static IntPtr se_play(IntPtr _self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return _self;
        }
    }
}

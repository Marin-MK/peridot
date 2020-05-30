using System;
using System.Collections.Generic;
using System.Text;
using RubyDotNET;

namespace Peridot
{
    public class Audio : RubyObject
    {
        public static IntPtr Module;

        public static Module CreateModule()
        {
            Module m = new Module("Audio");
            Module = m.Pointer;
            m.DefineClassMethod("se_play", se_play);
            return m;
        }

        protected static IntPtr se_play(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            return Internal.QTrue;
        }
    }
}

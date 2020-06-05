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
            m.DefineClassMethod("bgm_play", bgm_play);
            return m;
        }

        protected static IntPtr se_play(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            if (Internal.rb_funcallv(Args[0].Pointer, Internal.rb_intern("is_a?"), 1, new IntPtr[] { Internal.rb_cString.Pointer }) == Internal.QTrue)
            {
                string filename = new RubyString(Args[0].Pointer).ToString();
                odl.Audio.Play(filename);
            }
            else
            {
                odl.Sound sound = Sound.SoundDictionary[Args[0].Pointer];
                odl.Audio.Play(sound);
            }
            return Internal.QTrue;
        }

        protected static IntPtr bgm_play(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            string filename = new RubyString(Args[0].Pointer).ToString();
            odl.Audio.Play(filename);
            return Internal.QTrue;
        }
    }
}

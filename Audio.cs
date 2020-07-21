using System;
using rubydotnet;

namespace peridot
{
    public static class Audio
    {
        public static IntPtr Module;

        public static void Create()
        {
            Module = Ruby.Module.Define("Audio");
            Ruby.Module.DefineClassMethod(Module, "se_play", se_play);
            Ruby.Module.DefineClassMethod(Module, "bgm_play", bgm_play);
        }

        static IntPtr se_play(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "String");
            odl.Audio.Play(Ruby.String.FromPtr(Ruby.Array.Get(Args, 0)));
            return Ruby.True;
        }

        static IntPtr bgm_play(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "String");
            odl.Audio.Play(Ruby.String.FromPtr(Ruby.Array.Get(Args, 0)));
            return Ruby.True;
        }
    }
}

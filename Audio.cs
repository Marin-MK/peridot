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
            Ruby.Module.DefineClassMethod(Module, "bgm_play", bgm_play);
            Ruby.Module.DefineClassMethod(Module, "se_play", se_play);
            Ruby.Module.DefineClassMethod(Module, "me_play", me_play);
        }

        static IntPtr bgm_play(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1, 2, 3);
            if (Ruby.Array.Is(Args, 0, "Sound"))
            {
                odl.Sound sound = Sound.SoundDictionary[Ruby.Array.Get(Args, 0)];
                odl.Audio.BGMPlay(sound);
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "String");
                string Filename = Ruby.String.FromPtr(Ruby.Array.Get(Args, 0));
                int Volume = 100;
                int Pitch = 0;
                long len = Ruby.Array.Length(Args);
                if (len >= 2)
                {
                    Ruby.Array.Expect(Args, 1, "Integer");
                    Volume = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 1));
                }
                if (len == 3)
                {
                    Ruby.Array.Expect(Args, 2, "Integer");
                    Pitch = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 2));
                }
                odl.Audio.BGMPlay(Filename, Volume, Pitch);
            }
            return Ruby.True;
        }

        static IntPtr se_play(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1, 2, 3);
            if (Ruby.Array.Is(Args, 0, "Sound"))
            {
                odl.Sound sound = Sound.SoundDictionary[Ruby.Array.Get(Args, 0)];
                odl.Audio.SEPlay(sound);
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "String");
                string Filename = Ruby.String.FromPtr(Ruby.Array.Get(Args, 0));
                int Volume = 100;
                int Pitch = 0;
                long len = Ruby.Array.Length(Args);
                if (len >= 2)
                {
                    Ruby.Array.Expect(Args, 1, "Integer");
                    Volume = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 1));
                }
                if (len == 3)
                {
                    Ruby.Array.Expect(Args, 2, "Integer");
                    Pitch = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 2));
                }
                odl.Audio.SEPlay(Filename, Volume, Pitch);
            }
            return Ruby.True;
        }

        static IntPtr me_play(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1, 2, 3);
            if (Ruby.Array.Is(Args, 0, "Sound"))
            {
                odl.Sound sound = Sound.SoundDictionary[Ruby.Array.Get(Args, 0)];
                odl.Audio.MEPlay(sound);
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "String");
                string Filename = Ruby.String.FromPtr(Ruby.Array.Get(Args, 0));
                int Volume = 100;
                int Pitch = 0;
                long len = Ruby.Array.Length(Args);
                if (len >= 2)
                {
                    Ruby.Array.Expect(Args, 1, "Integer");
                    Volume = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 1));
                }
                if (len == 3)
                {
                    Ruby.Array.Expect(Args, 2, "Integer");
                    Pitch = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 2));
                }
                odl.Audio.MEPlay(Filename, Volume, Pitch);
            }
            return Ruby.True;
        }
    }
}

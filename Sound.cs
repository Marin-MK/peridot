using System;
using System.Collections.Generic;
using rubydotnet;

namespace peridot
{
    public static class Sound
    {
        public static IntPtr Class;
        public static Dictionary<IntPtr, odl.Sound> SoundDictionary = new Dictionary<IntPtr, odl.Sound>();

        public static void Create()
        {
            Class = Ruby.Class.Define("Sound");
            Ruby.Class.DefineMethod(Class, "initialize", initialize);
            Ruby.Class.DefineMethod(Class, "filename", filenameget);
            Ruby.Class.DefineMethod(Class, "volume", volumeget);
            Ruby.Class.DefineMethod(Class, "volume=", volumeset);
            Ruby.Class.DefineMethod(Class, "pitch", pitchget);
            Ruby.Class.DefineMethod(Class, "pitch=", pitchset);
            Ruby.Class.DefineMethod(Class, "sample_rate", sample_rateget);
            Ruby.Class.DefineMethod(Class, "sample_rate=", sample_rateset);
            Ruby.Class.DefineMethod(Class, "pan", panget);
            Ruby.Class.DefineMethod(Class, "pan=", panset);
            Ruby.Class.DefineMethod(Class, "position", positionget);
            Ruby.Class.DefineMethod(Class, "position=", positionset);
            Ruby.Class.DefineMethod(Class, "looping", loopingget);
            Ruby.Class.DefineMethod(Class, "looping=", loopingset);
            Ruby.Class.DefineMethod(Class, "loop_start", loop_startget);
            Ruby.Class.DefineMethod(Class, "loop_start=", loop_startset);
            Ruby.Class.DefineMethod(Class, "loop_end", loop_endget);
            Ruby.Class.DefineMethod(Class, "loop_end=", loop_endset);
            Ruby.Class.DefineMethod(Class, "fade_in_length", fade_in_lengthget);
            Ruby.Class.DefineMethod(Class, "fade_in_length=", fade_in_lengthset);
            Ruby.Class.DefineMethod(Class, "fade_out_length", fade_out_lengthget);
            Ruby.Class.DefineMethod(Class, "fade_out_length=", fade_out_lengthset);
            Ruby.Class.DefineMethod(Class, "length", length);
            Ruby.Class.DefineMethod(Class, "playing?", playing);
        }

        static IntPtr initialize(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1, 2, 3);
            long len = Ruby.Array.Length(Args);
            Ruby.Array.Expect(Args, 0, "String");
            Ruby.SetIVar(Self, "@filename", Ruby.Array.Get(Args, 0));
            string Filename = Ruby.String.FromPtr(Ruby.Array.Get(Args, 0));
            int Volume = 100;
            int Pitch = 0;
            if (len >= 2)
            {
                Ruby.Array.Expect(Args, 1, "Integer");
                Ruby.SetIVar(Self, "@volume", Ruby.Array.Get(Args, 1));
                Volume = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 1));
            }
            else
            {
                Ruby.SetIVar(Self, "@volume", Ruby.Integer.ToPtr(100));
            }
            if (len >= 3)
            {
                Ruby.Array.Expect(Args, 2, "Integer");
                Ruby.SetIVar(Self, "@pitch", Ruby.Array.Get(Args, 2));
                Pitch = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 2));
            }
            else Ruby.SetIVar(Self, "@pitch", Ruby.Integer.ToPtr(0));
            
            if (SoundDictionary.ContainsKey(Self))
            {
                SoundDictionary[Self].Stop();
                SoundDictionary.Remove(Self);
            }

            odl.Sound sound = new odl.Sound(Filename, Volume, Pitch);
            Ruby.SetIVar(Self, "@length", Ruby.Integer.ToPtr(sound.Length));

            SoundDictionary.Add(Self, sound);
            return Self;
        }

        static IntPtr filenameget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@filename");
        }

        static IntPtr volumeget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@volume");
        }

        static IntPtr volumeset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "Integer");
            int Volume = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
            SoundDictionary[Self].Volume = Volume;
            return Ruby.SetIVar(Self, "@volume", Ruby.Array.Get(Args, 0));
        }

        static IntPtr pitchget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@pitch");
        }

        static IntPtr pitchset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "Integer");
            int Pitch = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
            SoundDictionary[Self].Pitch = Pitch;
            return Ruby.SetIVar(Self, "@pitch", Ruby.Array.Get(Args, 0));
        }

        static IntPtr sample_rateget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@sample_rate");
        }

        static IntPtr sample_rateset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "Integer");
            int SampleRate = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
            SoundDictionary[Self].SampleRate = SampleRate;
            return Ruby.SetIVar(Self, "@sample_rate", Ruby.Array.Get(Args, 0));
        }

        static IntPtr panget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@pan");
        }

        static IntPtr panset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "Integer");
            int Pan = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
            SoundDictionary[Self].Pan = Pan;
            return Ruby.SetIVar(Self, "@pan", Ruby.Array.Get(Args, 0));
        }

        static IntPtr positionget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@position");
        }

        static IntPtr positionset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "Integer");
            int Position = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
            SoundDictionary[Self].Position = Position;
            return Ruby.SetIVar(Self, "@position", Ruby.Array.Get(Args, 0));
        }

        static IntPtr loopingget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@looping");
        }

        static IntPtr loopingset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "TrueClass", "FalseClass", "NilClass");
            bool Looping = Ruby.Array.Get(Args, 0) == Ruby.True;
            SoundDictionary[Self].Looping = Looping;
            return Ruby.SetIVar(Self, "@position", Ruby.Array.Get(Args, 0) == Ruby.True ? Ruby.True : Ruby.False);
        }

        static IntPtr loop_startget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@loop_start");
        }

        static IntPtr loop_startset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "Integer");
            int LoopStart = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
            SoundDictionary[Self].LoopStart = LoopStart;
            return Ruby.SetIVar(Self, "@loop_start", Ruby.Array.Get(Args, 0));
        }

        static IntPtr loop_endget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@loop_end");
        }

        static IntPtr loop_endset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "Integer");
            int LoopEnd = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
            SoundDictionary[Self].LoopEnd = LoopEnd;
            return Ruby.SetIVar(Self, "@loop_end", Ruby.Array.Get(Args, 0));
        }

        static IntPtr fade_in_lengthget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@fade_in_length");
        }

        static IntPtr fade_in_lengthset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "Integer");
            int FadeInLength = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
            SoundDictionary[Self].FadeInLength = FadeInLength;
            return Ruby.SetIVar(Self, "@fade_in_length", Ruby.Array.Get(Args, 0));
        }

        static IntPtr fade_out_lengthget(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@fade_out_length");
        }

        static IntPtr fade_out_lengthset(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            Ruby.Array.Expect(Args, 0, "Integer");
            int FadeOutLength = (int) Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0));
            SoundDictionary[Self].FadeOutLength = FadeOutLength;
            return Ruby.SetIVar(Self, "@fade_out_length", Ruby.Array.Get(Args, 0));
        }

        static IntPtr length(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return Ruby.GetIVar(Self, "@length");
        }

        static IntPtr playing(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 0);
            return SoundDictionary[Self].Playing ? Ruby.True : Ruby.False;
        }
    }
}

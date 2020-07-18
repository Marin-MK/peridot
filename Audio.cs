using System;
using rubydotnet;

namespace peridot
{
    public class Audio : Ruby.Object
    {
        public new static string KlassName = "Audio";
        public static Ruby.Module Module;

        public Audio(IntPtr Pointer) : base(Pointer) { }

        public static void Create()
        {
            Ruby.Module m = Ruby.Module.DefineModule<Audio>(KlassName);
            Module = m;
            m.DefineClassMethod("se_play", se_play);
            m.DefineClassMethod("bgm_play", bgm_play);
        }

        protected static Ruby.Object se_play(Ruby.Object Self, Ruby.Array Args)
        {
            Args.Expect(1);
            Args[0].Expect(Ruby.String.Class); 
            odl.Audio.Play(Args.Get<Ruby.String>(0));
            return Ruby.True;
        }

        protected static Ruby.Object bgm_play(Ruby.Object Self, Ruby.Array Args)
        {
            if (!Config.FakeWin32API) Args.Expect(1);
            Args[0].Expect(Ruby.String.Class);
            odl.Audio.Play(Args.Get<Ruby.String>(0));
            return Ruby.True;
        }
    }
}

using System;
using System.Collections.Generic;
using RubyDotNET;

namespace Peridot
{
    public class Sound : RubyObject
    {
        public static IntPtr Class;
        public static Dictionary<IntPtr, odl.Sound> SoundDictionary = new Dictionary<IntPtr, odl.Sound>();

        public static Class CreateClass()
        {
            Class c = new Class("Sound");
            Class = c.Pointer;
            c.DefineClassMethod("new", _new);
            c.DefineMethod("initialize", initialize);
            c.DefineMethod("play", play);
            c.DefineMethod("pause", pause);
            c.DefineMethod("paused?", paused);
            c.DefineMethod("resume", resume);
            c.DefineMethod("stop", stop);
            c.DefineMethod("dispose", dispose);
            c.DefineMethod("disposed?", disposed);
            c.DefineMethod("autodispose", autodisposeget);
            c.DefineMethod("autodispose=", autodisposeset);
            c.DefineMethod("filename", filenameget);
            c.DefineMethod("volume", volumeget);
            c.DefineMethod("volume=", volumeset);
            c.DefineMethod("position", positionget);
            c.DefineMethod("position=", positionset);
            c.DefineMethod("pan", panget);
            c.DefineMethod("pan=", panset);
            c.DefineMethod("fade_in_length", fade_in_lengthget);
            c.DefineMethod("fade_in_length=", fade_in_lengthset);
            c.DefineMethod("fade_out_length", fade_out_lengthget);
            c.DefineMethod("fade_out_length=", fade_out_lengthset);
            c.DefineMethod("loop", loopget);
            c.DefineMethod("loop=", loopset);
            c.DefineMethod("loop_start_position", loop_start_positionget);
            c.DefineMethod("loop_start_position=", loop_start_positionset);
            c.DefineMethod("loop_end_position", loop_end_positionget);
            c.DefineMethod("loop_end_position=", loop_end_positionset);
            c.DefineMethod("loop_times", loop_timesget);
            c.DefineMethod("loop_times=", loop_timesset);
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
            IntPtr filename = IntPtr.Zero;
            IntPtr volume = Internal.LONG2NUM(100);
            if (Args.Length == 1 || Args.Length == 2)
            {
                filename = Args[0].Pointer;
                if (Args.Length == 2) volume = Args[1].Pointer;
            }
            else ScanArgs(1, Args);

            Internal.SetIVar(self, "@filename", filename);
            Internal.SetIVar(self, "@volume", volume);
            Internal.SetIVar(self, "@position", Internal.LONG2NUM(0));
            Internal.SetIVar(self, "@pan", Internal.LONG2NUM(0));
            Internal.SetIVar(self, "@fade_in_length", Internal.LONG2NUM(0));
            Internal.SetIVar(self, "@fade_out_length", Internal.LONG2NUM(0));
            Internal.SetIVar(self, "@loop", Internal.QFalse);
            Internal.SetIVar(self, "@loop_start_position", Internal.LONG2NUM(0));
            Internal.SetIVar(self, "@loop_end_position", Internal.LONG2NUM(0));
            Internal.SetIVar(self, "@loop_times", Internal.LONG2NUM(-1));
            Internal.SetIVar(self, "@autodispose", Internal.QTrue);
            Internal.SetIVar(self, "@paused", Internal.QFalse);

            odl.Sound sound = new odl.Sound(new RubyString(filename).ToString(), (int) Internal.NUM2LONG(volume));
            sound.OnStopped += delegate (odl.BaseEventArgs e)
            {
                if (Internal.GetIVar(self, "@autodispose") == Internal.QTrue)
                {
                    sound.Dispose();
                }
            };
            if (SoundDictionary.ContainsKey(self))
            {
                SoundDictionary[self].Dispose();
                SoundDictionary.Remove(self);
            }
            SoundDictionary.Add(self, sound);

            return self;
        }

        protected static IntPtr autodisposeget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@autodispose");
        }

        protected static IntPtr autodisposeset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            return Internal.SetIVar(self, "@autodispose", Args[0].Pointer);
        }

        protected static IntPtr play(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            Internal.rb_funcallv(Audio.Module, Internal.rb_intern("play"), 1, new IntPtr[] { self });
            return Internal.QTrue;
        }

        protected static IntPtr stop(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            SoundDictionary[self].Stop();
            return Internal.QTrue;
        }

        protected static IntPtr pause(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            SoundDictionary[self].Pause();
            Internal.SetIVar(self, "@paused", Internal.QTrue);
            return Internal.QTrue;
        }

        protected static IntPtr paused(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@paused");
        }

        protected static IntPtr resume(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            SoundDictionary[self].Resume();
            Internal.SetIVar(self, "@paused", Internal.QFalse);
            return Internal.QTrue;
        }

        protected static IntPtr filenameget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@filename");
        }

        protected static IntPtr volumeget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@volume");
        }

        protected static IntPtr volumeset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            SoundDictionary[self].Volume = (int) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.SetIVar(self, "@volume", Args[0].Pointer);
        }

        protected static IntPtr positionget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@position");
        }

        protected static IntPtr positionset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            SoundDictionary[self].Position = (uint) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.SetIVar(self, "@position", Args[0].Pointer);
        }

        protected static IntPtr panget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@pan");
        }

        protected static IntPtr panset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            SoundDictionary[self].Pan = (int) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.SetIVar(self, "@pan", Args[0].Pointer);
        }

        protected static IntPtr fade_in_lengthget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@fade_in_length");
        }

        protected static IntPtr fade_in_lengthset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            SoundDictionary[self].FadeInLength = (uint) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.SetIVar(self, "@fade_in_length", Args[0].Pointer);
        }

        protected static IntPtr fade_out_lengthget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@fade_out_length");
        }

        protected static IntPtr fade_out_lengthset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            SoundDictionary[self].FadeOutLength = (uint) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.SetIVar(self, "@fade_out_length", Args[0].Pointer);
        }

        protected static IntPtr loopget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@loopget");
        }

        protected static IntPtr loopset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            SoundDictionary[self].Loop = Args[0].Pointer == Internal.QTrue;
            return Internal.SetIVar(self, "@loop", Args[0].Pointer);
        }

        protected static IntPtr loop_start_positionget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@loop_start_position");
        }

        protected static IntPtr loop_start_positionset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            SoundDictionary[self].LoopStartPosition = (uint) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.SetIVar(self, "@loop_start_position", Args[0].Pointer);
        }

        protected static IntPtr loop_end_positionget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@loop_end_position");
        }

        protected static IntPtr loop_end_positionset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            SoundDictionary[self].LoopEndPosition = (uint) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.SetIVar(self, "@loop_end_position", Args[0].Pointer);
        }

        protected static IntPtr loop_timesget(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.GetIVar(self, "@loop_times");
        }

        protected static IntPtr loop_timesset(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            SoundDictionary[self].LoopTimes = (int) Internal.NUM2LONG(Args[0].Pointer);
            return Internal.SetIVar(self, "@loop_times", Args[0].Pointer);
        }

        protected static IntPtr dispose(IntPtr self, IntPtr _args)
        {
            GuardDisposed(self);
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            SoundDictionary[self].Dispose();
            return Internal.QTrue;
        }

        protected static IntPtr disposed(IntPtr self, IntPtr _args)
        {
            if (_args != IntPtr.Zero)
            {
                RubyArray Args = new RubyArray(_args);
                ScanArgs(0, Args);
            }
            return Internal.GetIVar(self, "@disposed") == Internal.QTrue ? Internal.QTrue : Internal.QFalse;
        }

        public static void GuardDisposed(IntPtr self)
        {
            if (disposed(self, IntPtr.Zero) == Internal.QTrue)
            {
                Internal.rb_raise(Internal.rb_eRuntimeError.Pointer, "sound already disposed");
            }
        }
    }
}

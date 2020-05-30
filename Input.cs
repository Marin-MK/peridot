using System;
using System.Collections.Generic;
using System.Text;
using RubyDotNET;
using static SDL2.SDL;

namespace Peridot
{
    public class Input : RubyObject
    {
        public static IntPtr Module;

        public static Module CreateModule()
        {
            Module m = new Module("Input");
            Module = m.Pointer;
            m.DefineClassMethod("trigger?", trigger);
            m.DefineClassMethod("press?", press);
            m.DefineClassMethod("update", update);
            DefineButton(m, "A", SDL_Keycode.SDLK_a);
            DefineButton(m, "B", SDL_Keycode.SDLK_b);
            DefineButton(m, "C", SDL_Keycode.SDLK_c);
            DefineButton(m, "D", SDL_Keycode.SDLK_d);
            DefineButton(m, "E", SDL_Keycode.SDLK_e);
            DefineButton(m, "F", SDL_Keycode.SDLK_f);
            DefineButton(m, "G", SDL_Keycode.SDLK_g);
            DefineButton(m, "H", SDL_Keycode.SDLK_h);
            DefineButton(m, "I", SDL_Keycode.SDLK_i);
            DefineButton(m, "J", SDL_Keycode.SDLK_j);
            DefineButton(m, "K", SDL_Keycode.SDLK_k);
            DefineButton(m, "L", SDL_Keycode.SDLK_l);
            DefineButton(m, "M", SDL_Keycode.SDLK_m);
            DefineButton(m, "N", SDL_Keycode.SDLK_n);
            DefineButton(m, "O", SDL_Keycode.SDLK_o);
            DefineButton(m, "P", SDL_Keycode.SDLK_p);
            DefineButton(m, "Q", SDL_Keycode.SDLK_q);
            DefineButton(m, "R", SDL_Keycode.SDLK_r);
            DefineButton(m, "S", SDL_Keycode.SDLK_s);
            DefineButton(m, "T", SDL_Keycode.SDLK_t);
            DefineButton(m, "U", SDL_Keycode.SDLK_u);
            DefineButton(m, "V", SDL_Keycode.SDLK_v);
            DefineButton(m, "W", SDL_Keycode.SDLK_w);
            DefineButton(m, "X", SDL_Keycode.SDLK_x);
            DefineButton(m, "Y", SDL_Keycode.SDLK_y);
            DefineButton(m, "Z", SDL_Keycode.SDLK_z);
            DefineButton(m, "DOWN", SDL_Keycode.SDLK_DOWN);
            DefineButton(m, "LEFT", SDL_Keycode.SDLK_LEFT);
            DefineButton(m, "RIGHT", SDL_Keycode.SDLK_RIGHT);
            DefineButton(m, "UP", SDL_Keycode.SDLK_UP);
            RubyArray shift = new RubyArray();
            shift.Add(new RubyInt(Internal.LONG2NUM(System.Convert.ToInt64(SDL_Keycode.SDLK_LSHIFT))));
            shift.Add(new RubyInt(Internal.LONG2NUM(System.Convert.ToInt64(SDL_Keycode.SDLK_RSHIFT))));
            m.DefineConstant("SHIFT", shift.Pointer);
            RubyArray ctrl = new RubyArray();
            ctrl.Add(new RubyInt(Internal.LONG2NUM(System.Convert.ToInt64(SDL_Keycode.SDLK_LCTRL))));
            ctrl.Add(new RubyInt(Internal.LONG2NUM(System.Convert.ToInt64(SDL_Keycode.SDLK_RCTRL))));
            m.DefineConstant("CTRL", ctrl.Pointer);
            return m;
        }

        protected static void DefineButton(Module Module, string Name, SDL_Keycode keycode)
        {
            Module.DefineConstant(Name, Internal.LONG2NUM(System.Convert.ToInt64(keycode)));
        }

        protected static IntPtr trigger(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            if (Internal.rb_funcallv(Args[0].Pointer, Internal.rb_intern("is_a?"), 1, new IntPtr[1] { Internal.rb_cArray.Pointer }) == Internal.QTrue)
            {
                RubyArray Keys = new RubyArray(Args[0].Pointer);
                for (int i = 0; i < Keys.Length; i++)
                {
                    if (ODL.Input.Trigger(Internal.NUM2LONG(Keys[i].Pointer))) return Internal.QTrue;
                }
            }
            else if (ODL.Input.Trigger(Internal.NUM2LONG(Args[0].Pointer))) return Internal.QTrue;
            return Internal.QFalse;
        }

        protected static IntPtr press(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            if (Internal.rb_funcallv(Args[0].Pointer, Internal.rb_intern("is_a?"), 1, new IntPtr[1] { Internal.rb_cArray.Pointer }) == Internal.QTrue)
            {
                RubyArray Keys = new RubyArray(Args[0].Pointer);
                for (int i = 0; i < Keys.Length; i++)
                {
                    if (ODL.Input.Press(Internal.NUM2LONG(Keys[i].Pointer))) return Internal.QTrue;
                }
            }
            else if (ODL.Input.Press(Internal.NUM2LONG(Args[0].Pointer))) return Internal.QTrue;
            return Internal.QFalse;
        }

        protected static IntPtr update(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(0, Args);
            return Internal.QNil;
        }
    }
}

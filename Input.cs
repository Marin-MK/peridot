using System;
using RubyDotNET;
using static SDL2.SDL;

namespace peridot
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
            DefineButton(m, "SHIFT", SDL_Keycode.SDLK_LSHIFT, SDL_Keycode.SDLK_RSHIFT);
            DefineButton(m, "CTRL", SDL_Keycode.SDLK_LCTRL, SDL_Keycode.SDLK_RCTRL);
            DefineButton(m, "ENTER", SDL_Keycode.SDLK_RETURN);
            DefineButton(m, "RETURN", SDL_Keycode.SDLK_RETURN);
            DefineButton(m, "ESCAPE", SDL_Keycode.SDLK_ESCAPE);
            DefineButton(m, "F1", SDL_Keycode.SDLK_F1);
            DefineButton(m, "F2", SDL_Keycode.SDLK_F2);
            DefineButton(m, "F3", SDL_Keycode.SDLK_F3);
            DefineButton(m, "F4", SDL_Keycode.SDLK_F4);
            DefineButton(m, "F5", SDL_Keycode.SDLK_F5);
            DefineButton(m, "F6", SDL_Keycode.SDLK_F6);
            DefineButton(m, "F7", SDL_Keycode.SDLK_F7);
            DefineButton(m, "F8", SDL_Keycode.SDLK_F8);
            DefineButton(m, "F9", SDL_Keycode.SDLK_F9);
            DefineButton(m, "F10", SDL_Keycode.SDLK_F10);
            DefineButton(m, "F11", SDL_Keycode.SDLK_F11);
            DefineButton(m, "F12", SDL_Keycode.SDLK_F12);
            DefineButton(m, "F13", SDL_Keycode.SDLK_F13);
            DefineButton(m, "F14", SDL_Keycode.SDLK_F14);
            DefineButton(m, "F15", SDL_Keycode.SDLK_F15);
            DefineButton(m, "F16", SDL_Keycode.SDLK_F16);
            DefineButton(m, "F17", SDL_Keycode.SDLK_F17);
            DefineButton(m, "F18", SDL_Keycode.SDLK_F18);
            DefineButton(m, "F19", SDL_Keycode.SDLK_F19);
            DefineButton(m, "F20", SDL_Keycode.SDLK_F20);
            DefineButton(m, "F21", SDL_Keycode.SDLK_F21);
            DefineButton(m, "F22", SDL_Keycode.SDLK_F22);
            DefineButton(m, "F23", SDL_Keycode.SDLK_F23);
            DefineButton(m, "F24", SDL_Keycode.SDLK_F24);
            DefineButton(m, "HOME", SDL_Keycode.SDLK_HOME);
            DefineButton(m, "END", SDL_Keycode.SDLK_END);
            DefineButton(m, "PAGEUP", SDL_Keycode.SDLK_PAGEUP);
            DefineButton(m, "PAGEDOWN", SDL_Keycode.SDLK_PAGEDOWN);
            DefineButton(m, "INSERT", SDL_Keycode.SDLK_INSERT);
            DefineButton(m, "DELETE", SDL_Keycode.SDLK_DELETE);
            DefineButton(m, "BACKSPACE", SDL_Keycode.SDLK_BACKSPACE);
            DefineButton(m, "SPACE", SDL_Keycode.SDLK_SPACE);
            DefineButton(m, "TAB", SDL_Keycode.SDLK_TAB);
            DefineButton(m, "CAPSLOCK", SDL_Keycode.SDLK_CAPSLOCK);
            return m;
        }

        protected static void DefineButton(Module Module, string Name, params SDL_Keycode[] keycodes)
        {
            if (keycodes.Length == 1)
            {
                Module.DefineConstant(Name, Internal.LONG2NUM(System.Convert.ToInt64(keycodes[0])));
            }
            else
            {
                RubyArray ary = new RubyArray();
                for (int i = 0; i < keycodes.Length; i++)
                {
                    ary.Add(new RubyInt(Internal.LONG2NUM(System.Convert.ToInt64(keycodes[i]))));
                }
                Module.DefineConstant(Name, ary.Pointer);
            }
        }

        protected static IntPtr trigger(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            if (Internal.IsType(Args[0].Pointer, RubyClass.Array))
            {
                RubyArray Keys = new RubyArray(Args[0].Pointer);
                for (int i = 0; i < Keys.Length; i++)
                {
                    if (odl.Input.Trigger(Internal.NUM2LONG(Keys[i].Pointer))) return Internal.QTrue;
                }
            }
            else
            {
                Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
                if (odl.Input.Trigger(Internal.NUM2LONG(Args[0].Pointer))) return Internal.QTrue;
            }
            return Internal.QFalse;
        }

        protected static IntPtr press(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            ScanArgs(1, Args);
            if (Internal.IsType(Args[0].Pointer, RubyClass.Array))
            {
                RubyArray Keys = new RubyArray(Args[0].Pointer);
                for (int i = 0; i < Keys.Length; i++)
                {
                    if (odl.Input.Press(Internal.NUM2LONG(Keys[i].Pointer))) return Internal.QTrue;
                }
            }
            else
            {
                Internal.EnsureType(Args[0].Pointer, RubyClass.Integer);
                if (odl.Input.Press(Internal.NUM2LONG(Args[0].Pointer))) return Internal.QTrue;
            }
            return Internal.QFalse;
        }
    }
}

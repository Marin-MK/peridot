using System;
using rubydotnet;
using static odl.SDL2.SDL;

namespace peridot
{
    public static class Input
    {
        public static IntPtr Module;

        public static void Create()
        {
            Module = Ruby.Module.Define("Input");
            Ruby.Module.DefineClassMethod(Module, "trigger?", trigger);
            Ruby.Module.DefineClassMethod(Module, "press?", press);
            DefineButton(Module, "A", SDL_Keycode.SDLK_a);
            DefineButton(Module, "B", SDL_Keycode.SDLK_b);
            DefineButton(Module, "C", SDL_Keycode.SDLK_c);
            DefineButton(Module, "D", SDL_Keycode.SDLK_d);
            DefineButton(Module, "E", SDL_Keycode.SDLK_e);
            DefineButton(Module, "F", SDL_Keycode.SDLK_f);
            DefineButton(Module, "G", SDL_Keycode.SDLK_g);
            DefineButton(Module, "H", SDL_Keycode.SDLK_h);
            DefineButton(Module, "I", SDL_Keycode.SDLK_i);
            DefineButton(Module, "J", SDL_Keycode.SDLK_j);
            DefineButton(Module, "K", SDL_Keycode.SDLK_k);
            DefineButton(Module, "L", SDL_Keycode.SDLK_l);
            DefineButton(Module, "M", SDL_Keycode.SDLK_m);
            DefineButton(Module, "N", SDL_Keycode.SDLK_n);
            DefineButton(Module, "O", SDL_Keycode.SDLK_o);
            DefineButton(Module, "P", SDL_Keycode.SDLK_p);
            DefineButton(Module, "Q", SDL_Keycode.SDLK_q);
            DefineButton(Module, "R", SDL_Keycode.SDLK_r);
            DefineButton(Module, "S", SDL_Keycode.SDLK_s);
            DefineButton(Module, "T", SDL_Keycode.SDLK_t);
            DefineButton(Module, "U", SDL_Keycode.SDLK_u);
            DefineButton(Module, "V", SDL_Keycode.SDLK_v);
            DefineButton(Module, "W", SDL_Keycode.SDLK_w);
            DefineButton(Module, "X", SDL_Keycode.SDLK_x);
            DefineButton(Module, "Y", SDL_Keycode.SDLK_y);
            DefineButton(Module, "Z", SDL_Keycode.SDLK_z);
            DefineButton(Module, "DOWN", SDL_Keycode.SDLK_DOWN);
            DefineButton(Module, "LEFT", SDL_Keycode.SDLK_LEFT);
            DefineButton(Module, "RIGHT", SDL_Keycode.SDLK_RIGHT);
            DefineButton(Module, "UP", SDL_Keycode.SDLK_UP);
            DefineButton(Module, "SHIFT", SDL_Keycode.SDLK_LSHIFT, SDL_Keycode.SDLK_RSHIFT);
            DefineButton(Module, "CTRL", SDL_Keycode.SDLK_LCTRL, SDL_Keycode.SDLK_RCTRL);
            DefineButton(Module, "ENTER", SDL_Keycode.SDLK_RETURN);
            DefineButton(Module, "RETURN", SDL_Keycode.SDLK_RETURN);
            DefineButton(Module, "ESCAPE", SDL_Keycode.SDLK_ESCAPE);
            DefineButton(Module, "F1", SDL_Keycode.SDLK_F1);
            DefineButton(Module, "F2", SDL_Keycode.SDLK_F2);
            DefineButton(Module, "F3", SDL_Keycode.SDLK_F3);
            DefineButton(Module, "F4", SDL_Keycode.SDLK_F4);
            DefineButton(Module, "F5", SDL_Keycode.SDLK_F5);
            DefineButton(Module, "F6", SDL_Keycode.SDLK_F6);
            DefineButton(Module, "F7", SDL_Keycode.SDLK_F7);
            DefineButton(Module, "F8", SDL_Keycode.SDLK_F8);
            DefineButton(Module, "F9", SDL_Keycode.SDLK_F9);
            DefineButton(Module, "F10", SDL_Keycode.SDLK_F10);
            DefineButton(Module, "F11", SDL_Keycode.SDLK_F11);
            DefineButton(Module, "F12", SDL_Keycode.SDLK_F12);
            DefineButton(Module, "F13", SDL_Keycode.SDLK_F13);
            DefineButton(Module, "F14", SDL_Keycode.SDLK_F14);
            DefineButton(Module, "F15", SDL_Keycode.SDLK_F15);
            DefineButton(Module, "F16", SDL_Keycode.SDLK_F16);
            DefineButton(Module, "F17", SDL_Keycode.SDLK_F17);
            DefineButton(Module, "F18", SDL_Keycode.SDLK_F18);
            DefineButton(Module, "F19", SDL_Keycode.SDLK_F19);
            DefineButton(Module, "F20", SDL_Keycode.SDLK_F20);
            DefineButton(Module, "F21", SDL_Keycode.SDLK_F21);
            DefineButton(Module, "F22", SDL_Keycode.SDLK_F22);
            DefineButton(Module, "F23", SDL_Keycode.SDLK_F23);
            DefineButton(Module, "F24", SDL_Keycode.SDLK_F24);
            DefineButton(Module, "HOME", SDL_Keycode.SDLK_HOME);
            DefineButton(Module, "END", SDL_Keycode.SDLK_END);
            DefineButton(Module, "PAGEUP", SDL_Keycode.SDLK_PAGEUP);
            DefineButton(Module, "PAGEDOWN", SDL_Keycode.SDLK_PAGEDOWN);
            DefineButton(Module, "INSERT", SDL_Keycode.SDLK_INSERT);
            DefineButton(Module, "DELETE", SDL_Keycode.SDLK_DELETE);
            DefineButton(Module, "BACKSPACE", SDL_Keycode.SDLK_BACKSPACE);
            DefineButton(Module, "SPACE", SDL_Keycode.SDLK_SPACE);
            DefineButton(Module, "TAB", SDL_Keycode.SDLK_TAB);
            DefineButton(Module, "CAPSLOCK", SDL_Keycode.SDLK_CAPSLOCK);
        }

        static void DefineButton(IntPtr Module, string Name, params SDL_Keycode[] keycodes)
        {
            if (keycodes.Length == 1)
            {
                Ruby.SetConst(Module, Name, Ruby.Integer.ToPtr(Convert.ToInt64(keycodes[0])));
            }
            else
            {
                IntPtr ary = Ruby.Array.Create();
                for (int i = 0; i < keycodes.Length; i++)
                {
                    Ruby.Funcall(ary, "push", Ruby.Integer.ToPtr(Convert.ToInt64(keycodes[i])));
                }
                Ruby.SetConst(Module, Name, ary);
            }
        }

        static IntPtr trigger(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            if (Ruby.Array.Is(Args, 0, "Array"))
            {
                IntPtr Keys = Ruby.Array.Get(Args, 0);
                long len = Ruby.Array.Length(Keys);
                for (int i = 0; i < len; i++)
                {
                    Ruby.Array.Expect(Keys, i, "Integer");
                    if (odl.Input.Trigger(Ruby.Integer.FromPtr(Ruby.Array.Get(Keys, i)))) return Ruby.True;
                }
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                if (odl.Input.Trigger(Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0)))) return Ruby.True;
            }
            return Ruby.False;
        }

        static IntPtr press(IntPtr Self, IntPtr Args)
        {
            Ruby.Array.Expect(Args, 1);
            if (Ruby.Array.Is(Args, 0, "Array"))
            {
                IntPtr Keys = Ruby.Array.Get(Args, 0);
                long len = Ruby.Array.Length(Keys);
                for (int i = 0; i < len; i++)
                {
                    Ruby.Array.Expect(Keys, i, "Integer");
                    if (odl.Input.Press(Ruby.Integer.FromPtr(Ruby.Array.Get(Keys, i)))) return Ruby.True;
                }
            }
            else
            {
                Ruby.Array.Expect(Args, 0, "Integer");
                if (odl.Input.Press(Ruby.Integer.FromPtr(Ruby.Array.Get(Args, 0)))) return Ruby.True;
            }
            return Ruby.False;
        }
    }
}

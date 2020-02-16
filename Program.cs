using System;
using ODL;
using RubyDotNET;
using static SDL2.SDL;

namespace odlgss
{
    public class Program
    {
        public static Window MainWindow;
        public static bool Looping = true;

        static void Main(string[] args)
        {
            Internal.Initialize();

            Graphics.CreateModule();
            Input.CreateModule();
            Audio.CreateModule();
            Sprite.CreateClass();
            Rect.CreateClass();
            Viewport.CreateClass();
            Bitmap.CreateClass();
            Color.CreateClass();
            Tone.CreateClass();
            Font.CreateClass();

            int Width = 512;
            int Height = 384;
            Graphics.Width = Width;
            Graphics.Height = Height;
            ODL.Font.FontPath = "D:/Desktop/MK/mk/fonts";
            Internal.rb_const_set(Internal.rb_cObject.Pointer, Internal.rb_intern("SCREENWIDTH"), Internal.LONG2NUM(Graphics.Width));
            Internal.rb_const_set(Internal.rb_cObject.Pointer, Internal.rb_intern("SCREENHEIGHT"), Internal.LONG2NUM(Graphics.Height));

            ODL.Graphics.Start();

            MainWindow = new Window();
            MainWindow.SetResizable(false);
            MainWindow.SetSize(Width, Height);
            MainWindow.Show();
            MainWindow.OnClosed += delegate (object sender, ClosedEventArgs e)
            {
                ODL.Graphics.Stop();
            };

            Graphics.Start();

            // Sets extension load paths and working directory
            PrepareLoadPath();

            // Runs the script and returns raises a RuntimeError when window is closed.
            LoadScript("D:/Desktop/MK/mk/ruby/scripts/requires.rb");

            //MainFunc();

            if (!MainWindow.Closed) MainWindow.Close();
        }

        public static void PrepareLoadPath()
        {
            IntPtr var = Internal.rb_gv_get("$LOAD_PATH");
            Internal.rb_ary_push(var, Internal.rb_str_new_cstr("D:\\Desktop\\MK\\mk\\ruby\\extensions\\2.6.0"));
            Internal.rb_ary_push(var, Internal.rb_str_new_cstr("D:\\Desktop\\MK\\mk\\ruby\\extensions\\2.6.0\\i386-mingw32"));
            Internal.Eval("Dir.chdir 'D:/Desktop/MK/mk'");
        }

        public static void MainFunc()
        {
            while (ODL.Graphics.CanUpdate())
            {
                Input.Update();
                Graphics.Update();
                if (Input.Trigger((long) SDL_Keycode.SDLK_RETURN))
                {
                    
                }
            }
        }

        public static void LoadScript(string File)
        {
            Internal.DangerousFunction call = delegate (IntPtr Arg)
            {
                Internal.rb_require(File);
                return IntPtr.Zero;
            };
            IntPtr state = IntPtr.Zero;
            Internal.rb_protect(call, IntPtr.Zero, out state);
            if (state != IntPtr.Zero) // Error
            {
                IntPtr Err = Internal.rb_errinfo();
                Internal.rb_gv_set("$x", Err);
                bool Handled = Internal.Eval("$x.is_a?(SystemExit)") == Internal.QTrue;
                if (Handled) return;
                Internal.Eval("p $x");
                Internal.Eval(@"type = $x.class.to_s
msg = type + ': ' + $x.to_s + ""\n""
for i in 0...$x.backtrace.size
  line = $x.backtrace[i].sub(Dir.pwd,'')
  colons = line.split(':')
  msg << 'Line ' + colons[1] + ' in ' + colons[0] + ': '
  for j in 2...colons.size
    if colons[j].size > 3 && colons[j][0] == 'i' && colons[j][1] == 'n' && colons[j][2] == ' '
      colons[j] = colons[j].sub(/in /,'')
    end
    msg << colons[j]
    msg << ':' if j != colons.size - 1
  end
  msg << ""\n"" if i != $x.backtrace.size - 1
end
print msg"); // Print error
                Internal.rb_gv_set("$x", Internal.QNil);
                Internal.rb_set_errinfo(Internal.QNil);
            }
        }
    }
}

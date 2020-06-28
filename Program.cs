using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using odl;
using RubyDotNET;
using System.IO;

namespace peridot
{
    public class Program
    {
        public static Window MainWindow;
        public int ViewportOffsetX = 0;
        public int ViewportOffsetY = 0;
        
        public static void Start(string Path, bool InitializeEverything)
        {
            string OldWorkingDirectory = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(Path);
            
            InitializeRubyClasses();
            
            LoadConfig();
            
            if (InitializeEverything) InitializeOdl();
            
            ValidateEntryPoint();
            
            if (InitializeEverything) InitializeWindow();
            
            StartGraphics();
            
            RunGame();
            
            if (InitializeEverything) CloseWindow();
            Directory.SetCurrentDirectory(OldWorkingDirectory);
        }

        public static void EmbedGame(Window EditorWindow, string Path)
        {
            MainWindow = EditorWindow;
            Start(Path, false);
        }

        public static void InitializeRubyClasses()
        {
            try
            {
                Internal.Initialize();
            }
            catch (Exception ex)
            {
                Error($"Failed to initialize Ruby.\n\n{ex}");
            }

            try
            {
                if (Graphics.Module == IntPtr.Zero)
                {
                    Graphics.CreateModule();
                    Input.CreateModule();
                    Audio.CreateModule();
                    Sound.CreateClass();
                    Viewport.CreateClass();
                    Sprite.CreateClass();
                    Bitmap.CreateClass();
                    Color.CreateClass();
                    Tone.CreateClass();
                    Rect.CreateClass();
                    Font.CreateClass();
                }
            }
            catch (Exception ex)
            {
                Error($"Failed to create Ruby bindings.\n\n{ex}");
            }
        }

        public static void LoadConfig()
        {
            string AssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            Config.LoadSettings(AssemblyName + ".json", AssemblyName + "-config.json", "peridot.json", "config.json", "game.json", "game-config.json");

            int Width = Config.WindowWidth;
            int Height = Config.WindowHeight;
            Internal.SetIVar(Graphics.Module, "@width", Internal.LONG2NUM(Width));
            Internal.SetIVar(Graphics.Module, "@height", Internal.LONG2NUM(Height));
            Internal.rb_define_global_const("SCREENWIDTH", Internal.LONG2NUM(Width));
            Internal.rb_define_global_const("SCREENHEIGHT", Internal.LONG2NUM(Height));

            Internal.rb_cObject.DefineMethod("p", p);
            Internal.rb_cObject.DefineMethod("puts", puts);

            if (Config.FakeWin32API)
            {
                Win32API.CreateClass();
                Internal.rb_define_class("Plane", Sprite.Class);
                Internal.rb_define_class("Tilemap", Sprite.Class);
                Internal.rb_cObject.DefineMethod("load_data", Win32API.load_data);
                Internal.rb_cObject.DefineMethod("save_data", Win32API.save_data);
                Internal.rb_cThread.DefineClassMethod("critical", Win32API.criticalget);
                Internal.rb_cThread.DefineClassMethod("critical=", Win32API.criticalset);
                Table.CreateClass();
                Internal.GetKlass("Graphics").DefineClassMethod("freeze", Win32API.freeze);
                Internal.GetKlass("Graphics").DefineClassMethod("frame_reset", Win32API.frame_reset);
                Internal.GetKlass("Sprite").DefineMethod("bush_depth", Win32API.bush_depthget);
                Internal.GetKlass("Sprite").DefineMethod("bush_depth=", Win32API.bush_depthset);
                Internal.GetKlass("Sprite").DefineMethod("blend_type", Win32API.blend_typeget);
                Internal.GetKlass("Sprite").DefineMethod("blend_type=", Win32API.blend_typeset);
            }
        }

        public static void InitializeOdl()
        {
            odl.Graphics.Start();
            odl.Audio.Start();
        }

        public static void ValidateEntryPoint()
        {
            if (string.IsNullOrEmpty(Config.Script)) Error($"No starting script found (use the 'script' key in the configuration file).");
            else
            {
                Config.Script = Path.GetFullPath(Config.Script);
                if (!File.Exists(Config.Script)) Error($"Could not find starting script at '{Config.Script}'.");
            }
        }

        public static void InitializeWindow()
        {
            MainWindow = new Window();
            MainWindow.Initialize(true, true);
            MainWindow.SetText(Config.WindowTitle);
            MainWindow.SetResizable(Config.WindowResizable);
            MainWindow.SetSize((int) Math.Round(Config.WindowWidth * Config.WindowScale), (int) Math.Round(Config.WindowHeight * Config.WindowScale));
            MainWindow.SetBackgroundColor(Config.BackgroundColor);
            if (!string.IsNullOrEmpty(Config.WindowIcon))
            {
                if (!File.Exists(Config.WindowIcon) && !File.Exists(Config.WindowIcon + ".png")) Error($"Could not find an image to use as the window icon at '{Config.WindowIcon}'");
                MainWindow.SetIcon(Config.WindowIcon);
            }
            MainWindow.Show();
            odl.Graphics.Update(); // Ensure the renderer updates to show the specified background color while loading the game
            MainWindow.OnClosed += delegate (BaseEventArgs e)
            {
                odl.Graphics.Stop();
                odl.Audio.Stop();
            };
            MainWindow.Renderer.RenderScaleX = (float) Config.WindowScale;
            MainWindow.Renderer.RenderScaleY = (float) Config.WindowScale;
            MainWindow.OnSizeChanged += delegate (BaseEventArgs e)
            {
                float xscale = (float) MainWindow.Width / Config.WindowWidth;
                float yscale = (float) MainWindow.Height / Config.WindowHeight;
                if (Config.MaintainAspectRatio)
                {
                    MainWindow.Renderer.RenderOffsetX = 0;
                    MainWindow.Renderer.RenderOffsetY = 0;
                    if (xscale > yscale) xscale = yscale;
                    else if (xscale < yscale) yscale = xscale;
                }
                MainWindow.Renderer.RenderScaleX = xscale;
                MainWindow.Renderer.RenderScaleY = yscale;
                if (Config.MaintainAspectRatio)
                {
                    int w = (int) Math.Round(xscale * Config.WindowWidth);
                    int h = (int) Math.Round(yscale * Config.WindowHeight);
                    if (MainWindow.Width != w || MainWindow.Height != h) MainWindow.SetSize(w, h);
                }
            };
        }

        public static void StartGraphics()
        {
            Graphics.Start();
        }

        public static void RunGame()
        {
            // Sets extension load paths and working directory
            PrepareLoadPath();

            try
            {
                Internal.Eval("require 'zlib'");
            }
            catch (Exception ex)
            {
                Error($"Failed to link zlib.\n\n{ex}");
            }

            // Runs the script and returns raises a RuntimeError when window is closed.
            LoadScript(Config.Script);
        }

        public static void CloseWindow()
        {
            if (!MainWindow.IsClosed) MainWindow.Close();
        }

        public static void Main(string[] args)
        {
            Start(Directory.GetCurrentDirectory(), true);
        }

        public static void Error(string Message)
        {
            new ErrorBox(MainWindow, Message).Show();
            Environment.Exit(1);
        }

        public static void PrepareLoadPath()
        {
            IntPtr load_path = Internal.rb_gv_get("$LOAD_PATH");
            for (int i = 0; i < Config.RubyLoadPath.Count; i++)
            {
                Internal.rb_ary_push(load_path, Internal.rb_str_new_cstr(Config.RubyLoadPath[i]));
            }
            if (!string.IsNullOrEmpty(Config.MainDirectory))
            {
                Internal.rb_funcallv(Internal.rb_cDir.Pointer, Internal.rb_intern("chdir"), 1, new IntPtr[1] { Internal.rb_str_new_cstr(Config.MainDirectory) });
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
                IntPtr msg = Internal.Eval(@"type = $x.class.to_s
msg = type + ': ' + $x.to_s + ""\n\n""
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
msg"); // Print error
                Internal.rb_gv_set("$x", Internal.QNil);
                Internal.rb_set_errinfo(Internal.QNil);
                string text = new RubyString(msg).ToString();
                new ErrorBox(MainWindow, text).Show();
            }
        }

        protected static IntPtr p(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            StringBuilder msg = new StringBuilder();
            for (int i = 0; i < Args.Length; i++)
            {
                string value = new RubyString(Internal.rb_funcallv(Args[i].Pointer, Internal.rb_intern("inspect"), 0)).ToString();
                for (int j = 0; j < value.Length / 96; j++)
                {
                    value = value.Insert(j + j * 96, "\n");
                }
                msg.Append(value);
                if (i != Args.Length - 1) msg.AppendLine();
            }
            string text = msg.ToString();
            int newlines = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '\n') newlines++;
                if (newlines == 24) text = text.Substring(0, i) + "...";
            }
            new StandardBox(MainWindow, text).Show();
            return _args;
        }

        protected static IntPtr puts(IntPtr self, IntPtr _args)
        {
            RubyArray Args = new RubyArray(_args);
            StringBuilder msg = new StringBuilder();
            for (int i = 0; i < Args.Length; i++)
            {
                string value = new RubyString(Internal.rb_funcallv(Args[i].Pointer, Internal.rb_intern("to_s"), 0)).ToString();
                for (int j = 0; j < value.Length / 96; j++)
                {
                    value = value.Insert(j + j * 96, "\n");
                }
                msg.Append(value);
                if (i != Args.Length - 1) msg.AppendLine();
            }
            string text = msg.ToString();
            int newlines = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '\n') newlines++;
                if (newlines == 24) text = text.Substring(0, i) + "...";
            }
            new StandardBox(MainWindow, text).Show();
            return _args;
        }
    }
}

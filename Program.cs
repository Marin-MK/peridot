using System;
using System.Reflection;
using System.Text;
using odl;
using rubydotnet;
using System.IO;
using System.Runtime;

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
                Ruby.Initialize();
            }
            catch (Exception ex)
            {
                Error($"Failed to initialize Ruby.\n\n{ex}");
            }

            try
            {
                Graphics.Create();
                Input.Create();
                Audio.Create();
                //Sound.Create();
                Viewport.Create();
                Sprite.Create();
                Bitmap.Create();
                Color.Create();
                Tone.Create();
                Rect.Create();
                Font.Create();
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
            Graphics.Module.SetIVar("@width", (Ruby.Integer) Width);
            Graphics.Module.SetIVar("@height", (Ruby.Integer) Height);
            Ruby.Object.Class.SetConst("SCREENWIDTH", (Ruby.Integer) Width);
            Ruby.Object.Class.SetConst("SCREENHEIGHT", (Ruby.Integer) Height);

            Ruby.Object.Class.DefineMethod("p", p);
            Ruby.Object.Class.DefineMethod("puts", puts);
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
                Environment.Exit(1);
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
                Ruby.Eval("require 'zlib'");
            }
            catch (Exception ex)
            {
                Error($"Failed to link zlib.\n\n{ex}");
            }

            // Runs the script and returns raises a RuntimeError when window is closed.
            Ruby.Load(Config.Script);
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
            Ruby.Array load_path = Ruby.GetGlobal("$LOAD_PATH").Convert<Ruby.Array>();
            for (int i = 0; i < Config.RubyLoadPath.Count; i++)
            {
                load_path.Funcall("push", (Ruby.String) Config.RubyLoadPath[i]);
            }
            if (!string.IsNullOrEmpty(Config.MainDirectory))
            {
                Ruby.Dir.Chdir(Config.MainDirectory);
            }
        }

        protected static Ruby.Object p(Ruby.Object Self, Ruby.Array Args)
        {
            if (Args.Length == 0) Ruby.Raise(Ruby.ErrorType.ArgumentError, $"wrong number of arguments (given 0, expected at least 1)");
            StringBuilder msg = new StringBuilder();
            for (int i = 0; i < Args.Length; i++)
            {
                string value = Args[i].AutoFuncall<Ruby.String>("inspect");
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
            return Args.Length > 1 ? Args : Args[0];
        }

        protected static Ruby.Object puts(Ruby.Object Self, Ruby.Array Args)
        {
            if (Args.Length == 0) Ruby.Raise(Ruby.ErrorType.ArgumentError, $"wrong number of arguments (given 0, expected at least 1)");
            StringBuilder msg = new StringBuilder();
            for (int i = 0; i < Args.Length; i++)
            {
                string value = Args[i].AutoFuncall<Ruby.String>("to_s");
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
            return Args.Length > 1 ? Args : Args[0];
        }
    }
}

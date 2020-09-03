using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace peridot
{
    public static class Config
    {
        public static int FrameRate = 60;
        public static bool VSync = true;
        public static int WindowWidth = 480;
        public static int WindowHeight = 320;
        public static string WindowIcon = null;
        public static string WindowTitle = "Game";
        public static bool WindowResizable = true;
        public static double WindowScale = 1d;
        public static bool MaintainAspectRatio = true;
        public static string Script = "scripts/core/entry.rb";
        public static odl.Color BackgroundColor = odl.Color.BLACK;
        public static string MainDirectory = null;
        public static bool FakeWin32API = false;

        public static void LoadSettings(params string[] Filenames)
        {
            foreach (string Filename in Filenames)
            {
                if (File.Exists(Filename))
                {
                    StreamReader sr = new StreamReader(File.OpenRead(Filename));
                    string str = sr.ReadToEnd();
                    sr.Close();
                    Dictionary<string, object> data = null;
                    try
                    {
                        data = ((JObject) JsonConvert.DeserializeObject(str)).ToObject<Dictionary<string, object>>();
                    }
                    catch (Exception ex)
                    {
                        Program.Error($"An error occured while loading the config JSON. It is likely not a valid JSON format.\n\n{ex}");
                    }
                    bool setfps = false;
                    bool setvsync = false;
                    foreach (string key in data.Keys)
                    {
                        object value = data[key];
                        switch (key)
                        {
                            case "frame_rate":
                                EnsureType(typeof(long), value, key);
                                FrameRate = Convert.ToInt32(value);
                                if (FrameRate < 1) Program.Error($"The framerate specified by the config JSON must be at least 1.");
                                setfps = true;
                                break;
                            case "vsync":
                                EnsureType(typeof(bool), value, key);
                                VSync = Convert.ToBoolean(value);
                                setvsync = true;
                                break;
                            case "window_width":
                                EnsureType(typeof(long), value, key);
                                WindowWidth = Convert.ToInt32(value);
                                if (WindowWidth < 1) Program.Error($"The window width specified by the config JSON must be at least 1.");
                                break;
                            case "window_height":
                                EnsureType(typeof(long), value, key);
                                WindowHeight = Convert.ToInt32(value);
                                if (WindowHeight < 1) Program.Error($"The window height specified by the config JSON must be at least 1.");
                                break;
                            case "window_icon":
                                EnsureType(typeof(string), value, key);
                                WindowIcon = (string)value;
                                break;
                            case "window_title":
                                EnsureType(typeof(string), value, key);
                                WindowTitle = (string) value;
                                break;
                            case "window_resizable":
                                EnsureType(typeof(bool), value, key);
                                WindowResizable = Convert.ToBoolean(value);
                                break;
                            case "window_scale":
                                EnsureTypes(value, key, typeof(double), typeof(long));
                                WindowScale = Convert.ToDouble(value);
                                break;
                            case "maintain_aspect_ratio":
                                EnsureType(typeof(bool), value, key);
                                MaintainAspectRatio = Convert.ToBoolean(value);
                                break;
                            case "script":
                                EnsureType(typeof(string), value, key);
                                Script = (string) value;
                                break;
                            case "background_color":
                                EnsureType(typeof(JObject), value, key);
                                BackgroundColor = ParseColor(((JObject) value).ToObject<Dictionary<string, object>>(), key);
                                break;
                            case "main_directory":
                                EnsureType(typeof(string), value, key);
                                MainDirectory = (string) value;
                                break;
                            case "fake_win32api":
                                EnsureType(typeof(bool), value, key);
                                FakeWin32API = Convert.ToBoolean(value);
                                break;
                            default:
                                Program.Error($"Unknown key in the config JSON: '{key}'");
                                break;
                        }
                    }
                    if (setfps && setvsync && VSync)
                    {
                        Program.Error($"Cannot specify a frame rate and also have vsync enabled.");
                    }
                    break;
                }
            }
        }

        public static odl.Color ParseColor(Dictionary<string, object> data, string mainkey)
        {
            byte R = 0,
                 G = 0,
                 B = 0,
                 A = 255;
            foreach (string key in data.Keys)
            {
                object value = data[key];
                switch (key)
                {
                    case "red":
                        EnsureType(typeof(long), value, key);
                        int red = Convert.ToInt32(value);
                        if (red < 0 || red > 255) Program.Error($"Red color component ({red}) must be between 0 and 255 in config JSON, color definition in '{mainkey}'");
                        R = (byte) red;
                        break;
                    case "green":
                        EnsureType(typeof(long), value, key);
                        int green = Convert.ToInt32(value);
                        if (green < 0 || green > 255) Program.Error($"Green color component ({green}) must be between 0 and 255 in config JSON, color definition in '{mainkey}'");
                        G = (byte) green;
                        break;
                    case "blue":
                        EnsureType(typeof(long), value, key);
                        int blue = Convert.ToInt32(value);
                        if (blue < 0 || blue > 255) Program.Error($"Blue color component ({blue}) must be between 0 and 255 in config JSON, color definition in '{mainkey}'");
                        B = (byte) blue;
                        break;
                    case "alpha":
                        EnsureType(typeof(long), value, key);
                        int alpha = Convert.ToInt32(value);
                        if (alpha < 0 || alpha > 255) Program.Error($"Alpha color component ({alpha}) must be between 0 and 255 in config JSON, color definition in '{mainkey}'");
                        A = (byte) alpha;
                        break;
                    default:
                        Program.Error($"Unknown key in the config JSON, color definition in '{mainkey}', key '{key}'");
                        break;
                }
            }
            return new odl.Color(R, G, B, A);
        }

        public static void EnsureType(Type type, object value, string key)
        {
            if (value.GetType() != type)
            {
                Program.Error($"Expected a value of type {type.GetType().Name}, but got a value of type {value.GetType().Name} in JSON config key '{key}'");
            }
        }

        public static void EnsureTypes(object value, string key, params Type[] types)
        {
            string typestr = "";
            for (int i = 0; i < types.Length; i++)
            {
                if (value.GetType() == types[i]) return;
                typestr += types[i].Name;
                if (i < types.Length - 1 && types.Length != 2) typestr += ", ";
                else if (i == types.Length - 2) typestr += " or ";
            }
            Program.Error($"Expected a value of type {typestr}, but got a value of type {value.GetType().Name} in JSON config key '{key}'");
        }
    }
}

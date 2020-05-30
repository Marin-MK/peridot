using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Peridot
{
    public static class Config
    {
        public static int FrameRate = 60;
        public static int WindowWidth = 480;
        public static int WindowHeight = 320;
        public static string WindowIcon = null;
        public static string WindowTitle = "Peridot";
        public static string Script = null;
        public static ODL.Color BackgroundColor = ODL.Color.BLACK;
        public static List<string> RubyLoadPath = new List<string>();
        public static string MainDirectory = null;

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
                        throw new Exception($"An error occured while loading the config JSON. It is likely not a valid JSON format.\n\n{ex}");
                    }
                    foreach (string key in data.Keys)
                    {
                        object value = data[key];
                        switch (key)
                        {
                            case "frame_rate":
                                EnsureType(typeof(long), value, key);
                                FrameRate = Convert.ToInt32(value);
                                if (FrameRate < 1) throw new Exception($"The framerate specified by the config JSON must be at least 1.");
                                break;
                            case "window_width":
                                EnsureType(typeof(long), value, key);
                                WindowWidth = Convert.ToInt32(value);
                                if (WindowWidth < 1) throw new Exception($"The window width specified by the config JSON must be at least 1.");
                                break;
                            case "window_height":
                                EnsureType(typeof(long), value, key);
                                WindowHeight = Convert.ToInt32(value);
                                if (WindowHeight < 1) throw new Exception($"The window height specified by the config JSON must be at least 1.");
                                break;
                            case "window_icon":
                                EnsureType(typeof(string), value, key);
                                WindowIcon = (string)value;
                                break;
                            case "window_title":
                                EnsureType(typeof(string), value, key);
                                WindowTitle = (string) value;
                                break;
                            case "script":
                                EnsureType(typeof(string), value, key);
                                Script = (string) value;
                                break;
                            case "background_color":
                                EnsureType(typeof(JObject), value, key);
                                BackgroundColor = ParseColor(((JObject) value).ToObject<Dictionary<string, object>>(), key);
                                break;
                            case "ruby_load_path":
                                EnsureType(typeof(JArray), value, key);
                                List<object> paths = ((JArray) value).ToObject<List<object>>();
                                foreach (object path in paths)
                                {
                                    if (!(path is string)) throw new Exception($"Expected a value of type String, but got a value of type {path.GetType().Name} in element in key 'ruby_load_path'");
                                    RubyLoadPath.Add((string) path);
                                }
                                break;
                            case "main_directory":
                                EnsureType(typeof(string), value, key);
                                MainDirectory = (string) value;
                                break;
                            default:
                                throw new Exception($"Unknown key in the config JSON: '{key}'");
                        }
                    }
                    break;
                }
            }
        }

        public static ODL.Color ParseColor(Dictionary<string, object> data, string mainkey)
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
                        if (red < 0 || red > 255) throw new Exception($"Red color component ({red}) must be between 0 and 255 in config JSON, color definition in '{mainkey}'");
                        R = (byte) red;
                        break;
                    case "green":
                        EnsureType(typeof(long), value, key);
                        int green = Convert.ToInt32(value);
                        if (green < 0 || green > 255) throw new Exception($"Green color component ({green}) must be between 0 and 255 in config JSON, color definition in '{mainkey}'");
                        G = (byte) green;
                        break;
                    case "blue":
                        EnsureType(typeof(long), value, key);
                        int blue = Convert.ToInt32(value);
                        if (blue < 0 || blue > 255) throw new Exception($"Blue color component ({blue}) must be between 0 and 255 in config JSON, color definition in '{mainkey}'");
                        B = (byte) blue;
                        break;
                    case "alpha":
                        EnsureType(typeof(long), value, key);
                        int alpha = Convert.ToInt32(value);
                        if (alpha < 0 || alpha > 255) throw new Exception($"Alpha color component ({alpha}) must be between 0 and 255 in config JSON, color definition in '{mainkey}'");
                        A = (byte) alpha;
                        break;
                    default:
                        throw new Exception($"Unknown key in the config JSON, color definition in '{mainkey}', key '{key}'");
                }
            }
            return new ODL.Color(R, G, B, A);
        }

        public static void EnsureType(Type type, object value, string key)
        {
            if (value.GetType() != type)
            {
                throw new Exception($"Expected a value of type {type.GetType().Name}, but got a value of type {value.GetType().Name} in JSON config key '{key}'");
            }
        }
    }
}

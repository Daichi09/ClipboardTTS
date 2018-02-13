using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Windows.Forms;
using System.IO;

namespace ClipboardTTS
{
    class Program
    {
        public static string SettingFile = "ClipboardTTS.settings";

        static class Settings
        {
            public static string Voice = null;
            public static int Volume = 100;    // 0...100
            public static int Rate = 0;        // -10...10
            public static int CharLimit = 1000;
            public static Dictionary<string, string> Substitutions = new Dictionary<string, string>();
        }

        enum ParseMode
        {
            Normal,
            Substitutions
        };

        [STAThread]
        static void Main(string[] args)
        {
            string cliptext = Clipboard.GetText();

            if (cliptext.Length == 0)
                return;

            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {
                // Write new settings file if needed
                if (!File.Exists(SettingFile))
                {
                    using (StreamWriter sw = new StreamWriter(SettingFile))
                    {
                        // Write out each voice installed on the system
                        foreach (InstalledVoice voice in synth.GetInstalledVoices())
                        {
                            // comment out all except the first
                            if (voice != synth.GetInstalledVoices().First())
                                sw.Write(";");
                            sw.WriteLine("Voice = \"" + voice.VoiceInfo.Name + "\"");
                        }
                        sw.WriteLine();
                        sw.WriteLine(";Volume 0...100");
                        sw.WriteLine("Volume = 100");
                        sw.WriteLine();
                        sw.WriteLine(";Rate -10...10");
                        sw.WriteLine("Rate = 0");
                        sw.WriteLine();
                        sw.WriteLine(";Max Character Limit (0 = Unlimited)");
                        sw.WriteLine("CharLimit = 1000");
                        sw.WriteLine();
                        sw.WriteLine("[Substitutions]");
                        sw.WriteLine(";OriginalWord => ReplacementWord");
                    }
                }

                // Read settings
                using (StreamReader sr = new StreamReader(SettingFile))
                {
                    string Line;
                    ParseMode parseMode = ParseMode.Normal;
                    while ((Line = sr.ReadLine()) != null)
                    {
                        if (Line.Length == 0 || Line[0] == ';')
                            continue;

                        if (Line.Trim() == "[Substitutions]")
                        {
                            parseMode = ParseMode.Substitutions;
                            continue;
                        }

                        switch (parseMode)
                        {
                            case ParseMode.Normal:
                                var split = Line.Split(new char[] { '=' }, 2).Select(s => s.Trim()).ToArray();

                                switch (split[0])
                                {
                                    case "Voice":
                                        Settings.Voice = split[1].Trim('"');
                                        break;
                                    case "Volume":
                                        Settings.Volume = int.Parse(split[1]);
                                        break;
                                    case "Rate":
                                        Settings.Rate = int.Parse(split[1]);
                                        break;
                                    case "CharLimit":
                                        Settings.CharLimit = int.Parse(split[1]);
                                        break;

                                    default:
                                        break;
                                }
                                break;
                            case ParseMode.Substitutions:
                                var subSplit = Line.Split(new string[] { "=>" }, StringSplitOptions.None).Select(s => s.Trim()).ToArray();
                                Settings.Substitutions.Add(subSplit[0], subSplit[1]);
                                break;
                            default:
                                break;
                        } // end switch
                    } // end while
                } // end using StreamReader

                // Subsitutions
                foreach (var sub in Settings.Substitutions)
                {
                    cliptext = cliptext.Replace(sub.Key, sub.Value);
                }

                // Max Character Limit
                if (Settings.CharLimit != 0 && cliptext.Length > Settings.CharLimit)
                    cliptext = cliptext.Substring(0, Settings.CharLimit);

                // Playback
                if (Settings.Voice != null)
                    synth.SelectVoice(Settings.Voice);
                synth.Volume = Settings.Volume;
                synth.Rate = Settings.Rate;
                synth.Speak(cliptext);
            } //end using SpeechSynthesizer
        }
    }
}

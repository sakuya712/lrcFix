using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;

namespace lrcFix
{
    class Processing
    {
        public static void FixLrcFile(string path,bool IsFaster,TimeSpan offsetTime)
        {
            //open lrc file
            string[] lrcFile = File.ReadAllLines(path);
            foreach (var (lineValue, index) in lrcFile.Select((value, index) => (value, index)))
            {
                //find  format [mm:ss.xx] (mm is minutes, ss is seconds and xx is hundredths of a second. )
                MatchCollection match = Regex.Matches(lineValue, Regex.Escape("[") + "[0-9]{2}:[0-9]{2}.[0-9]{2}" + Regex.Escape("]"));
                foreach (Match time in match)
                {
                    //set TimeSpan
                    int m = int.Parse(time.Value.Substring(1, 2));
                    int s = int.Parse(time.Value.Substring(4, 2));
                    int ms = int.Parse(time.Value.Substring(7, 2) + "0");
                    TimeSpan datatime = new TimeSpan(0, 0, m, s, ms);
                    TimeSpan setTime;
                    // calc time
                    if (IsFaster)
                    {
                        setTime = datatime - offsetTime;
                    }
                    else
                    {
                        setTime = datatime + offsetTime;
                    }
                    //replace time
                    string min = String.Format("{0:00}", Math.Max(setTime.Minutes, 0));
                    string sec = String.Format("{0:00}", Math.Max(setTime.Seconds, 0));
                    string millisec = String.Format("{0:00}", Math.Max(setTime.Milliseconds, 0).ToString().PadRight(2, '0').Substring(0, 2));
                    lrcFile[index] = lrcFile[index].Replace(time.Value, $"[{min}:{sec}.{millisec}]");
                }
            }
            File.WriteAllLines(path, lrcFile);
        }
    }
}

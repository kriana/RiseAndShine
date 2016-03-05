using System;
using System.IO;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace RiseAndShineMod
{
    public class RiseAndShine : Mod
    {
        public override string Name
        {
            get { return "Rise and Shine"; }
        }

        public override string Authour
        {
            get { return "Yoshify"; }
        }

        public override string Version
        {
            get { return "1.0.1"; }
        }

        public override string Description
        {
            get { return "Changes the time you wake up."; }
        }

        public int wakeUpTime = 900; //Default wakeup time is 6AM (600).

        public override void Entry(params object[] objects)
        {          
            DoLoadPlayerConfig();
            TimeEvents.DayOfMonthChanged += SetTime;
            Console.WriteLine("Rise and Shine Mod has loaded!");
        }

        void DoLoadPlayerConfig()
        {
            string configFilePath = "Mods\\RiseAndShine.ini";
            StreamReader configReader;

            try
            {
                configReader = File.OpenText(configFilePath);
                Console.WriteLine("Got Rise and Shine config at " + configFilePath);
            }
            catch
            {
                Console.WriteLine("Rise and Shine config doesn't exist! Creating one for you.");

                StreamWriter configWriter = new StreamWriter(configFilePath);
                configWriter.WriteLine("WakeUpAt=900");
                configWriter.Close();

                configReader = File.OpenText(configFilePath);
            }

            string currLine = configReader.ReadLine();
            bool couldParse = int.TryParse(currLine.Split('=')[1], out wakeUpTime);

            if (!couldParse) Console.WriteLine("Couldn't parse Rise and Shine config! Using default values.");

            configReader.Close();
        }

        void SetTime(object sender, EventArgs e)
        {
            Command.CallCommand("world_settime " + wakeUpTime);
        }
    }
}

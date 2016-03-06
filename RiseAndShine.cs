using System;
using System.IO;
using StardewValley;
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
            get { return "1.1"; }
        }

        public override string Description
        {
            get { return "Changes the time you wake up."; }
        }

        public int wakeUpTime = 900; //Default wakeup time is 6AM (600).
        public int weekendTime = 900; //Wake up at the same time on the weekends.

        public override void Entry(params object[] objects)
        {
            Console.WriteLine("Rise and Shine Mod loading...");
            DoLoadPlayerConfig();
            TimeEvents.DayOfMonthChanged += SetTime;
            TimeEvents.SeasonOfYearChanged += SetTime;
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
                configWriter.WriteLine("WeekendWakeUpAt=900");
                configWriter.Close();

                configReader = File.OpenText(configFilePath);
            }

            string currLine = configReader.ReadLine();
            bool couldParseA = int.TryParse(currLine.Split('=')[1], out wakeUpTime);
            currLine = configReader.ReadLine();
            bool couldParseB = int.TryParse(currLine.Split('=')[1], out weekendTime);

            if (!couldParseA || !couldParseB)
            {
                Console.WriteLine("Couldn't parse Rise and Shine config! Using default values.");
                configReader.Close();
                return;
            }
                
            if(wakeUpTime < 600 || weekendTime < 600)
            {
                wakeUpTime = wakeUpTime < 600 ? 600 : wakeUpTime;
                weekendTime = weekendTime < 600 ? 600 : weekendTime;
                Console.WriteLine("You cannot wake up earlier than 6am. Sorry!");
            }
            else if(wakeUpTime > 2600 || weekendTime > 2600)
            {
                wakeUpTime = wakeUpTime > 2600 ? 2600 : wakeUpTime;
                weekendTime = weekendTime > 2600 ? 2600 : weekendTime;
                Console.WriteLine("You cannot wake up later than 2am the next day (I don't know why you'd want to do this). Sorry!");
            }

            configReader.Close();
        }

        void SetTime(object sender, EventArgs e)
        {
            string currDay = Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth);
            bool shouldUseWeekendTime = (currDay == "Sat" || currDay == "Sun") ? true : false;

            if (shouldUseWeekendTime)
                Game1.timeOfDay = weekendTime;
            else
                Game1.timeOfDay = wakeUpTime;
        }
    }
}

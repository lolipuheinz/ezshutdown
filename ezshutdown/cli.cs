/*
Priorität
TODO: ezshutdown-c# für einen ersten Release fertig bekommen.




TODO: Soll möglich sein mit ezshutdown [args], ezshut [args] und ezsh [args] äquivalent aufzurufen!
TODO: ne flag die statt shutdown hibernate oder energy save macht das wäre next level
TODO: Extra optionen für das automatische ausschalten nach abschliesen eines Downloads
TODO: Ein GUI
TODO: Optimierung der Variabelnamen, Ram-Verbrauch, Übersichtlichkeit des Codes



Total Hours invested here: 18
Aber das ist in ordung weil es mein erstes c# Project ist.




*/





using System;
using System.Globalization;
using System.Text.RegularExpressions;

class Ezshutdown
{   



    static void Main() 
    {

        
        
        while (true)
        {


        DateTime currentTime = DateTime.Now;
            


            

            Console.Write("Zeit bis Shutdown(s/m/sek):");
            string user_input = Console.ReadLine();



            bool EnergySaveState = false;
            int totalTimeinSeconds;
            byte currentMinute = (byte)DateTime.Now.Minute;
            byte currentHour = (byte)DateTime.Now.Hour;
            byte timetoshutdowninH;
            byte timetoshutdowninMin;
            byte targetTimeMin;
            byte targetTimeh;
            byte targetDateDay;
            byte targetDateMonth;
            short targetDateYear;
            string patternTimeVorhanden = @"-time\s{1}([01]?[0-9]|2[0-3]):([0-5][0-9])";
            string patternDateVorhanden = @"-date\s{1}([01]?[0-9]|2[0-3]):([0-5][0-9])\s{1}([0123][0-9]).([10][1-9]).([12][0-9][0-9][0-9])";

            Match TimeVorhanden = Regex.Match(user_input, patternTimeVorhanden);
            Match DateVorhanden = Regex.Match(user_input, patternDateVorhanden);
            Match EnergySavE = Regex.Match(user_input, @"\s+\-E\s+");
            Match EnergySave = Regex.Match(user_input, @"\s+\-e\s+");



            if (EnergySavE.Success || EnergySave.Success)
            {
                EnergySaveState = true;
            }
            

                
            if (TimeVorhanden.Success)
            {
                targetTimeMin = byte.Parse(TimeVorhanden.Groups[2].Value);
                targetTimeh = byte.Parse(TimeVorhanden.Groups[1].Value);
                timetoshutdowninH =  (byte)(targetTimeh - currentHour);
                timetoshutdowninMin= (byte)(targetTimeMin - currentMinute);

                totalTimeinSeconds = timeinHandMintosec(timetoshutdowninH, timetoshutdowninMin);
            }
            else if (DateVorhanden.Success)
            {
                targetTimeMin = byte.Parse(DateVorhanden.Groups[2].Value);
                targetTimeh = byte.Parse(DateVorhanden.Groups[1].Value);
                targetDateDay = byte.Parse(DateVorhanden.Groups[3].Value);
                targetDateMonth = byte.Parse(DateVorhanden.Groups[4].Value);
                targetDateYear = short.Parse(DateVorhanden.Groups[5].Value);

                totalTimeinSeconds = TimeAndDatetoSecond(targetTimeMin, targetTimeh, targetDateDay, targetDateMonth, targetDateYear);

            }
            else
            {
                totalTimeinSeconds = convert_args_to_time(user_input);
            }
                
            
            static int TimeAndDatetoSecond(byte targetTimeMin, byte targetTimeh, byte targetDateDay, byte targetDateMonth, short targetDateYear)
            {
                DateTime target = new DateTime(targetDateYear, targetDateMonth, targetDateDay, targetTimeh, targetTimeMin, 0);
                
                TimeSpan span = target - DateTime.Now;


                Console.WriteLine($"{span.TotalSeconds}");
                int totalTimeinSeconds = (int)span.TotalSeconds;
                return totalTimeinSeconds;




            }
            
            
            
            
            static int timeinHandMintosec(byte timetoshutdowninH, byte timetoshutdowninMin)
            {
                int totalTimeinSeconds = timetoshutdowninH * 60 * 60 + timetoshutdowninMin * 60;
                return totalTimeinSeconds;


            }
            
            
            
            
            
            
            
            
            
            
            
            static int convert_args_to_time(string user_input)
            {   
                int totalTimeinSeconds;

                Match timesearch = Regex.Match(user_input, @"[(\d+)y]\s?[(\d+)mo]\s?[(\d+)w]\s?[(\d+)d]\s?[(\d+)h]\s?[(\d+)m]\s?[(\d+)s]?");

                int yearsTime = (int) int.Parse(timesearch.Groups[1].Value) * 365 * 24 * 60 * 60;
                int monthsTime = int.Parse(timesearch.Groups[2].Value) * 30 * 24 * 60 * 60;
                int weeksTime = int.Parse(timesearch.Groups[3].Value) * 7 * 24 * 60 * 60;
                int daysTime = int.Parse(timesearch.Groups[4].Value) * 24 * 60 * 60;
                int hoursTime = int.Parse(timesearch.Groups[5].Value) * 60 * 60;
                short minutesTime = (short) (int.Parse(timesearch.Groups[6].Value) * 60);
                short secondsTime = short.Parse(timesearch.Groups[7].Value);























                /*
                int totalTimeinSeconds;
                
                int yearsTime = 0;

                Match yearsSearch = Regex.Match(user_input, @"(\d+)y");

                if (yearsSearch.Success)
                {
                    yearsTime = int.Parse(yearsSearch.Groups[1].Value) * 365 * 24 * 60 * 60;
                }

                int monthsTime = 0;
                Match monthsSearch = Regex.Match(user_input, @"(\d+)mo");
                if (monthsSearch.Success)
                {
                    monthsTime = int.Parse(monthsSearch.Groups[1].Value) * 30 * 24 * 60 * 60;
                }
                
                int weeksTime = 0;
                Match weeksSearch = Regex.Match(user_input, @"(\d+)w");
                if (weeksSearch.Success)
                {
                    weeksTime = int.Parse(weeksSearch.Groups[1].Value) * 7 * 24 * 60 * 60;
                }

                int daysTime = 0;
                Match daysSearch = Regex.Match(user_input, @"(\d+)d");
                if (daysSearch.Success)
                {
                    daysTime = int.Parse(daysSearch.Groups[1].Value) * 24 * 60 * 60;
                }

                int hoursTime = 0;
                Match hoursSearch = Regex.Match(user_input, @"(\d+)h");
                if (hoursSearch.Success)
                {
                    hoursTime = int.Parse(hoursSearch.Groups[1].Value) * 60 * 60;
                }

                int minutesTime = 0;
                Match minutesSearch = Regex.Match(user_input, @"(\d+)m");
                if (minutesSearch.Success)
                {
                    minutesTime = int.Parse(minutesSearch.Groups[1].Value) * 60;
                }

                int secondsTime = 0;
                Match secondsSearch = Regex.Match(user_input, @"(\d+)s");
                if (secondsSearch.Success)
                {
                    secondsTime = int.Parse(secondsSearch.Groups[1].Value);
                }
                */


                totalTimeinSeconds = secondsTime + minutesTime + hoursTime + daysTime + weeksTime + monthsTime + yearsTime;
                return totalTimeinSeconds;
                




            }







            

            bool Error = false;

            if (totalTimeinSeconds > 315360000 || totalTimeinSeconds <= 0)
            {
                Error = true;
            }
            
            
            
            
            
            
            
            if (Error)
            {
                totalTimeinSeconds = 0;
                Console.WriteLine("Critical Error (nothing)");
                Console.WriteLine("************************");
            }


            if (!Error)
            {
            Console.WriteLine($"Die komplette Zeit in Sekunden Beträgt {totalTimeinSeconds} sekunden.");
            }



            if (System.Text.RegularExpressions.Regex.IsMatch(user_input, @"(^|\s)-h(\s|$)"))
            {
            Console.WriteLine("So funktioniert das programm:");
            Console.WriteLine("Hier könnte in Zukunft eine Erklärung stehen.");
            }





        }


        

    }

}

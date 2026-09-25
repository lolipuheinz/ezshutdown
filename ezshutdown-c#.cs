/*
Priorität
TODO: ezshutdown-c# für einen ersten Release fertig bekommen.




TODO: Soll möglich sein mit ezshutdown [args], ezshut [args] und ezsh [args] äquivalent aufzurufen!
TODO: ne flag die statt shutdown hibernate oder energy save macht das wäre next level
TODO: Extra optionen für das automatische ausschalten nach abschliesen eines Downloads
TODO: Ein GUI
TODO: Optimierung der Variabelnamen, Ram-Verbrauch, Übersichtlichkeit des Codes



Total Hours invested here: 20
Aber das ist in ordung weil es mein erstes c# Project ist.




*/





using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Diagnostics;

// only for windows
// using System.Windows.Forms;

class Ezshutdown
{   



    static void Main() 
    {

        
        
        


            DateTime currentTime = DateTime.Now;
            
            string user_input;

            while (true)
            {
                

                Console.Write("Zeit bis Shutdown(s/m/sek):");
                user_input = Console.ReadLine();


                if(string.IsNullOrWhiteSpace(user_input) || user_input.Length > 30)
                {
                    Console.WriteLine("Critical Error too long User Input");
                    Console.WriteLine("press any Button to continue");
                    continue;
                }
                else if (System.Text.RegularExpressions.Regex.IsMatch(user_input, @"(^|\s)-{1,2}(h|help)(\s|$)"))
                {
                    Console.WriteLine("Help");
                    Console.WriteLine("No arguments: 3y 4mo 2w 4d 4m 2s");
                    Console.WriteLine("Arguments for Energy save: -es / --es or -EnergySave / -- EnergySave");
                    Console.WriteLine("Arguments for Hybernating: -hbn / --hbn or -Hybernate / --Hybernate");
                    Console.WriteLine("You can also set a -time 18:00 for a scheduled shutdown");
                    Console.WriteLine("If you want to shutdown you system in +1 day use -date");
                    Console.WriteLine("Example: -date 18:00 25.09.2026");
                    continue;
                }
                else
                {
                break;
                }
            
            
                
            }

            bool HybernateState = false;
            bool NoOtherArgument = true;
            bool EnergySaveState = false;
            long totalTimeinSeconds;
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

            // some over complex bs that somehow works

            Match TimeVorhanden = Regex.Match(user_input, patternTimeVorhanden);
            Match DateVorhanden = Regex.Match(user_input, patternDateVorhanden);
            Match EnergySave = Regex.Match(user_input, @"(^|\s)-{1,2}(es|EnergySave)(\s|$)");
            Match Hybernate = Regex.Match(user_input, @"(^|\s)-{1,2}(hbn|Hybernate)(\s|$)");


            if (EnergySave.Success)
            {
                EnergySaveState = true;
                NoOtherArgument = false;
            }
            else if (Hybernate.Success)
            {
                HybernateState = true;
                NoOtherArgument = false;
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
            
            
            
            
            
            
            
            
            
            
            
            static long convert_args_to_time(string user_input)
            {   
                if (string.IsNullOrWhiteSpace(user_input))
                {
                    return 0;
                }

                
                long totalTimeinSeconds = 0;


                MatchCollection matches = Regex.Matches(user_input, @"(\d+)\s*([a-zA-Z]+)");

                foreach (Match match in matches)
                {
                    if (!int.TryParse(match.Groups[1].Value, out int value))
                        continue;

                    string unit = match.Groups[2].Value.ToLower();

                    totalTimeinSeconds += unit switch
                    {
                        "y"  => value * 365L * 24 * 60 * 60,
                        "mo" => value * 30L * 24 * 60 * 60,
                        "w"  => value * 7L * 24 * 60 * 60,
                        "d"  => value * 24L * 60 * 60,
                        "h"  => value * 60L * 60,
                        "m"  => value * 60L,
                        "s"  => value,
                        _    => 0
                    };
                }

                return totalTimeinSeconds;
            }







            // over 10y check

            bool Error = false;

            if (totalTimeinSeconds > 315360000 || totalTimeinSeconds < 0)
            {
                Error = true;
            }
            
            
            
            
            
            
            
            if (Error)
            {
                totalTimeinSeconds = 0;
                Console.WriteLine("Critical Error (totalTimeinSeconds cant be greater than 10y)");
                Console.WriteLine("Critical Error (totalTimeinSeconds cant be smaller than 0)  ");
                Console.WriteLine("************************************************************");
            }



            //End Arguments for Poweroff/Standby/Hybernating in future

            if (!Error && NoOtherArgument)
            {
                Console.WriteLine($"Shutdown in {totalTimeinSeconds} seconds.");

                // Source - https://stackoverflow.com/a/104258
                // Posted by Pop Catalin, modified by community. See post 'Timeline' for change history
                // Retrieved 2026-09-23, License - CC BY-SA 3.0

                var psi = new ProcessStartInfo("shutdown",$"/s /t {totalTimeinSeconds}");
                psi.CreateNoWindow = true;
                psi.UseShellExecute = false;
                Process.Start(psi);
            }
            else if (EnergySaveState && !Error)
            {
                // for Windows
                Console.WriteLine($"Energy Save in {totalTimeinSeconds} seconds.");
                var psi = new ProcessStartInfo("cmd.exe", $"/c timeout /t {totalTimeinSeconds} && rundll32.exe powrprof.dll,SetSuspendState Sleep");
                psi.CreateNoWindow = true;
                psi.UseShellExecute = false;
                Process.Start(psi);

            }
            else if (HybernateState && !Error)
            {
                Console.WriteLine($"Hybernating in {totalTimeinSeconds} seconds.");
                var psi = new ProcessStartInfo("shutdown",$"/h /t {totalTimeinSeconds}");
                psi.CreateNoWindow = true;
                psi.UseShellExecute = false;
                Process.Start(psi);
            }
            else
            {
                Console.WriteLine("Critical Error (in end arguments)");
                Console.WriteLine("*********************************");
            }


            Console.WriteLine("Cancel with shutdown.exe /a or shutdown /a in cmd");
            Console.WriteLine("cant cancel energysave");


        }


        

    }




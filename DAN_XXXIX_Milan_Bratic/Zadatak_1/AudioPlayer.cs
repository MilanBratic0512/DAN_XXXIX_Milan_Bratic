using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zadatak_1
{
    class AudioPlayer
    {
        public string Author { get; set; }
        public string SongTitle { get; set; }
        public TimeSpan Duration { get; set; }
        


        public delegate void callingDelegate(string message);
        public static Dictionary<int, string> Songs = new Dictionary<int, string>();
        static Random rnd = new Random();
        static string path = "../../Music.txt";
        static string path2 = "../../Reklame.txt";
        public override string ToString()
        {
            return Author + ": " + SongTitle + " " + Duration;
        }
        /// <summary>
        /// method for adding new song
        /// </summary>
        internal void AddNewSong()
        {
            Console.WriteLine("Author:");
            string author = Console.ReadLine();
            Console.WriteLine("Song title: ");
            string title = Console.ReadLine();
            Console.WriteLine("Duration (hh:mm:ss): ");
            string input = null;
            TimeSpan duration = TimeSpan.Parse("00:00:00");
            //validation
            do
            {
                input = Console.ReadLine();
            } while (!TimeSpanValidation(input, out duration));
            AudioPlayer player = new AudioPlayer();
            player.Author = author;
            player.SongTitle = title;
            player.Duration = duration;
            //represent order number of song 
            int key = Songs.Count;
            Songs.Add(key, player.ToString());
            File.AppendAllText(path, player.ToString() + "\n");
        }


        internal void ShowAll()
        {
            string[] lines = File.ReadAllLines(path);

            foreach (var item in lines)
            {
                Console.WriteLine(item);
            }
        }
        /// <summary>
        /// method for playing song
        /// </summary>
        internal void PlayTheSong(string song)
        {
            
            lock (Program.locker)
            {

                string[] stringSong = song.Split(' ');
                //pick out song duration from the string
                TimeSpan songDuration = TimeSpan.Parse(stringSong[stringSong.Length - 1]);
                //represent total second 
                int intDuration = songDuration.Seconds + songDuration.Minutes * 60 + songDuration.Hours * 3600;
                Thread t = new Thread(Advertising);
                do
                {
                    Thread.Sleep(1000);
                    intDuration--;
                    Console.WriteLine("Plays a song...");
                    //start thread for advertising
                    if (!t.IsAlive)
                    {
                        t.Start();
                    }
                    //press any key for stop playing song
                    if (Console.KeyAvailable)
                    {
                        ShowMessage(CallDelegate);
                        break;
                    }
                } while (intDuration != 0);

                t.Abort();
                
                Monitor.Pulse(Program.locker);
            }

        }
        internal void SelectSong()
        {

            foreach (var item in Songs)
            {
                Console.WriteLine(item.Key + ". " + item.Value);
            }
            Console.WriteLine("Press 'esc' if you want stop the song.");
            Console.WriteLine("Under witch ordinal number you want to listen song?");
            string input = null;
            int index = 0;
            //validation
            do
            {
                input = Console.ReadLine();
            } while (!IntValidation(input, out index));
            //if find key create and start thread
            if (Songs.ContainsKey(index))
            {
                Thread t = new Thread(() => PlayTheSong(Songs[index]));
                t.Start();
            }
        }
        /// <summary>
        /// method for read advertising from the file
        /// </summary>
        internal void Advertising()
        {
            string[] lines = File.ReadAllLines(path2);

            do
            {
                Thread.Sleep(200);
                Console.WriteLine(lines[rnd.Next(0, 5)]);
            } while (true);
        }
        /// <summary>
        /// method for write message on the console
        /// </summary>
        public static void CallDelegate(string message)
        {
            Console.WriteLine(message);
        }
        public static void ShowMessage(callingDelegate cd)
        {
            string message = "The song is stopped";
            
            cd(message);

        }
        /// <summary>
        /// method for read song from the file and place them to the dictonary
        /// </summary>
        public static void ReadSongsFromTheFile()
        {
            string[] lines = File.ReadAllLines(path);


            for (int i = 0; i < lines.Length; i++)
            {
                Songs.Add(i, lines[i]);
            }
        }

        /// <summary>
        /// time span validation
        /// </summary>
        bool TimeSpanValidation(string input,out TimeSpan time)
        {
            do
            {
                if (TimeSpan.TryParseExact(input, "hh\\:mm\\:ss", CultureInfo.CurrentCulture, out time))
                {
                    return true;
                }
                
            } while (TimeSpan.TryParseExact(input, "hh\\:mm\\:ss", CultureInfo.CurrentCulture, out time));
            Console.WriteLine("Incorect format. Please try again.");
            return false;
        }
        /// <summary>
        /// int validation
        /// </summary>
        bool IntValidation(string input, out int index)
        {
            do
            {
                if (int.TryParse(input, out index) && Songs.ContainsKey(index))
                {
                    return true;
                }
            } while (int.TryParse(input, out index) && Songs.ContainsKey(index));
            Console.WriteLine("Incorect input. Please try again");
            return false;
        }
    }
}

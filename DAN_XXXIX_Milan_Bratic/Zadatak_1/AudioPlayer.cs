using System;
using System.Collections.Generic;
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
        internal void AddNewSong()
        {
            Console.WriteLine("Author:");
            string author = Console.ReadLine();
            Console.WriteLine("Song title: ");
            string title = Console.ReadLine();
            Console.WriteLine("Duration (hh:mm:ss): ");
            TimeSpan duration = TimeSpan.Parse(Console.ReadLine());
            AudioPlayer player = new AudioPlayer();
            player.Author = author;
            player.SongTitle = title;
            player.Duration = duration;
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
        internal void PlayTheSong(string song)
        {
            lock (Program.locker)
            {

                string[] stringSong = song.Split(' ');
                string songName = stringSong[1];
                TimeSpan songDuration = TimeSpan.Parse(stringSong[stringSong.Length - 1]);
                int intDuration = songDuration.Seconds + songDuration.Minutes * 60 + songDuration.Hours * 3600;
                Thread t = new Thread(Advertising);
                do
                {
                    Thread.Sleep(1000);
                    intDuration--;
                    Console.WriteLine("Plays a song");
                    if (!t.IsAlive)
                    {
                        t.Start();
                    }

                    if (Console.ReadKey(true).Key == ConsoleKey.Enter)
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
            Console.WriteLine("Under witch ordinal number you want to listen song?");
            int index = int.Parse(Console.ReadLine());

            if (Songs.ContainsKey(index))
            {
                Thread t = new Thread(() => PlayTheSong(Songs[index]));
                t.Start();
            }
        }
        internal void Advertising()
        {
            string[] lines = File.ReadAllLines(path2);

            do
            {
                Thread.Sleep(200);
                Console.WriteLine(lines[rnd.Next(0, 5)]);
            } while (true);
        }
        public static void CallDelegate(string message)
        {
            Console.WriteLine(message);
        }
        public static void ShowMessage(callingDelegate cd)
        {
            string message = null;

            message = "The song is stopped";
            cd(message);

        }
        public static void ReadSongsFromTheFile()
        {
            string[] lines = File.ReadAllLines(path);


            for (int i = 0; i < lines.Length; i++)
            {
                Songs.Add(i, lines[i]);
            }
        }
    }
}

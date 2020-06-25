using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadatak_1
{
    class AudioPlayer
    {
        public string Author { get; set; }
        public string SongTitle { get; set; }
        public TimeSpan Duration { get; set; }

        public List<AudioPlayer> Songs = new List<AudioPlayer>();
        string path = "../../Music.txt";

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
            Songs.Add(player);
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
    }
}

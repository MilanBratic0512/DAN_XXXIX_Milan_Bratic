using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadatak_1
{
    class Program
    {
        
        static void Main(string[] args)
        {
            AudioPlayer audioPlayer = new AudioPlayer();
            string option = null;
            do
            {
                Console.WriteLine("1. Add new song");
                Console.WriteLine("2. Show all");
                Console.WriteLine("3. Play the song");
                Console.WriteLine("4. Exit");
                option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        audioPlayer.AddNewSong();
                        break;
                    case "2":
                        audioPlayer.ShowAll();
                        break;
                    case "3":

                        break;
                    default:
                        break;
                }
            } while (option != "4");
        }
    }
}

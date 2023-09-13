using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentDiary
{
    internal static class Program
    {
        public static string FilePath = Path.Combine(Environment.CurrentDirectory, "students.txt");
        public static List<string> groups = new List<string> { "Klasa 1A", "Klasa 1B", "Klasa 2A", "Klasa 2B", "Klasa 3A", "Klasa 3B", "Klasa 4A", "Klasa 4B", "Klasa 5A",  };

        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}

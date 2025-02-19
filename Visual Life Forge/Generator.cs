using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visual_Life_Forge
{
    class Generator
    {
        List<string> names;
        public Generator()
        {
            names = new List<string>();
            string input = "Aiden, Akira, Alessandro, Amari, Andrei, Antoine, Arvid, Ashwin, Aymeric, Bao, Benicio" +
                ", Bodhi, Carlos, Chike, Cristiano, Daan, Darian, Dimitri, Diego, Elijah, " +
                "Emil, Ethan, Farid, Finn, Francesco, Gero, Gustavo, Hamza, Haruto, Hassan, " +
                "Hugo, Ibrahim, Igor, Isaac, Ivan, Jaden, Javier, Jian, Joaquín, Johan, José, " +
                "Kai, Kamal, Kareem, Keegan, Khalid, Liam, Luca, Luis, Maksim, Marek, " +
                "Mateo, Matthias, Musa, Nashit, Nikolai, Nuno, Omar, Oren, Oscar, Pablo, " +
                "Parth, Rafael, Rami, Ravi, Rico, Rohan, Sami, Santiago, Sebastian, Shane, " +
                "Shoaib, Simone, Sven, Tariq, Theo, Thiago, Timur, Tobias, Umar, Vlad, " +
                "Wade, Xander, Yasir, Youssef, Zane, Zayd, Zhen, Ziad, Zubair";
            input = input.Replace(',', '\0');
            string[] names1 = input.Split(',');
            foreach (string name in names1)
            {
                names.Add(name);
            }
            // now names should be filled with every name that you need.
        }

        public string CreateName()
        {
            Random rnd = new Random();
            int index = rnd.Next(names.Count);
            int number = rnd.Next(names.Count);
            return $"{names[index]}{number}";
        }
    }
}

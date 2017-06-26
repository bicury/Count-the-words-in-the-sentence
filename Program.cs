using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;

namespace reciURecenici
{
    class Program
    {
        static void Main(string[] args)
        {
            bool displayMenu = true;
            while (displayMenu)
            {
                displayMenu = MainManu();
            }
            
        }

        private static bool MainManu()
        {
            Console.Clear();
            Console.WriteLine("Meni unosa teksta: ");
            Console.WriteLine("1) Console");
            Console.WriteLine("2) Fajl");
            Console.WriteLine("3) SQL Database");
            Console.WriteLine("4) Exit");
            string result = Console.ReadLine();
            if (result == "1")
            {
                Konzola();
                return true;
            }
            else if (result == "2")
            {
                Fajl();
                return true;
            }
            else if (result == "3")
            {
                Sql();
                return true;
            }
            else if (result == "4")
            {
                return true;
            }
            else
            {
                return true;
            }
        }

        public static void Konzola()
        {
            Console.Clear();
            string txt;
            Console.WriteLine("Unesite tekst");
            txt = Console.ReadLine();
            Rezultat(txt);
            
            Console.ReadLine();
             
        }
        private static void Fajl ()
        {
            Console.Clear();
            string text = System.IO.File.ReadAllText(@"d:\Nemanja.txt");
            Console.WriteLine(text);
            Rezultat(text);
           
            Console.ReadLine();

        }

        private static void Sql()
        {
            char[] seperator = new char[] { ' ' };
            SqlConnection conn = new SqlConnection(Properties.Settings.Default.connectinString);
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM TestTabela", conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Rezultat(reader["Tekst"].ToString());
                    }
                }
            }
            Console.ReadLine();
        }

        static void Rezultat(string rez)
        {
            //Broji reci u recenici
            char[] seperator = new char[] { ' ' };
            int numberOfWords = rez.Split(seperator, StringSplitOptions.RemoveEmptyEntries).Length;
            Console.WriteLine("Broj reci u recenici je {0}", numberOfWords);

            //Upisuje rezultate u fajl
            try
            {
                string path = @"d:\test.txt";
                if (!File.Exists(path))
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.Write("Broj reci u recenici je {0}", numberOfWords);
                    }
                }
                else if (File.Exists(path))
                {
                    using (var sw = new StreamWriter(path, true))
                    {
                        sw.WriteLine("Broj reci u recenici je {0}", numberOfWords);
                        sw.Close();
                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine("Doslo je do grekse: {0}", e.ToString());
            }

            //Upis rezultata u bazu

            try
            {

                SqlConnection con = new SqlConnection(Properties.Settings.Default.connectinString);
                using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.resconnectingString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO RezultatTb(Broj) VALUES (" +
                           "@broj)", conn))
                    {
                        cmd.Parameters.AddWithValue("@broj", numberOfWords);
                        int rows = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Doslo je do grekse: {0}", ex.ToString());
            }
        }

    }
}

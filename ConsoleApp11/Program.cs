using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp11
{
    public struct State
    {
        public string name;
        public string capital;
        public int area;
        public double people;

        public State(string name, string capital, int area, double people)
        {
            this.name = name;
            this.capital = capital;
            this.area = area;
            this.people = people;
        }
    }

    public class Sim
    {
        public char cha { get; set; }
        public char chb { get; set; }


    }
    class Program
    {

        static string PasFrom;
        static string PasTo;
        static void Main(string[] args)
        {
            Console.WriteLine("Введите PasFrom");
            PasFrom = @"\\sd\City\SDP-181\Файловая система\TNSShort";// Console.ReadLine();
            Console.WriteLine("Введите PasTo");
            PasTo = @"\\sd\City\SDP-181\Файловая система\TNSShort\001"; //Console.ReadLine();

            DirectoryInfo DirPasFrom = new DirectoryInfo(PasFrom);
            DirectoryInfo DirPasTo = new DirectoryInfo(PasTo);

            if (!DirPasFrom.Exists || !DirPasTo.Exists)
            {
                throw new Exception("Дирректория не существует");
            }

            Dictionary<string, bool> Formats = new Dictionary<string, bool>();
            Dictionary<string, bool> FormatsChoice = new Dictionary<string, bool>();

            foreach (FileInfo file in DirPasFrom.GetFiles())
            {
                if (!Formats.ContainsKey(file.Extension))
                    Formats.Add(file.Extension, false);

            }

            foreach (var item in Formats)
            {
                Console.Write(item.Key + " использовать д/н: ");
                if (Console.ReadLine() == "д")
                    FormatsChoice.Add(item.Key, true);
                Console.WriteLine("");
            }

            Console.Clear();

            foreach (var item in FormatsChoice)
            {
                foreach (FileInfo file in DirPasFrom.GetFiles("*" + item.Key))
                {
                    Console.WriteLine("--> {0}", file.Name);
                }
                Console.WriteLine("-----------------------");
                Console.WriteLine("total: {0} files", DirPasFrom.GetFiles("*" + item.Key).Length);
            }

            List<Sim> s = new List<Sim>();

            Console.WriteLine("Редактировать?! д/н");
            if (Console.ReadLine() == "д")
            {
                foreach (var item in FormatsChoice)
                {
                    foreach (FileInfo file in DirPasFrom.GetFiles("*" + item.Key))
                    {
                        s.AddRange(LCh(file.Name.Substring(0,file.Name.LastIndexOf("."))));                      
                    }
                }

                s = s.GroupBy(w => w.cha).
                    Select(ss => new Sim { cha = ss.Key }).ToList();

                Console.WriteLine("-------------------");
                foreach (Sim item in s)
                {
                    Console.Write(item.cha+"-");
                    string choice = Console.ReadLine();
                    item.chb = string.IsNullOrWhiteSpace(choice)
                                ? new char()
                                : choice[0];
                }

                foreach (var item in FormatsChoice)
                {
                    foreach (FileInfo file in DirPasFrom.GetFiles("*" + item.Key))
                    {
                        string str = file.Name.Substring(0, file.Name.LastIndexOf("."));

                        foreach (Sim i in s)
                        {
                            str = str.Replace(i.cha, i.chb);
                        }

                        string newP = Path.Combine(PasTo, str+""+file.Extension);
                        file.MoveTo(newP);
                    }
                }
            }

        }

        public static List<Sim> LCh(string s)
        {
            List<Sim> result = new List<Sim>();
            foreach (char item in s)
            {
                if ((int)item >= 33 && (int)item <= 47)
                    result.Add(new Sim() { cha = item });
            }
            return result;
        }


        public enum FileType { sales, stored }

        static void Exmpl01(FileType ft)
        {

            FileInfo f = new FileInfo("testFile.xtx");
            if (f.Exists)
            {

            }
            else
            {
                FileStream fs = f.Create();
                fs.Close();

                using (FileStream fs2 = f.Create())
                {

                }
            }

            

            //FileAccess - используется для определения чтения/записи лежащего в основе потока
            //FileShare - указывает, как может быть разделене с другими файловыми потоками

            string path = Path.Combine("Upload", DateTime.Now.ToShortDateString(), ft.ToString(), "file.txt");

            using (FileStream FS = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {

            }
        }

        static void Exmpl02()
        {
            //привязываемся к директории Upload
            DirectoryInfo dir = new DirectoryInfo(@"Upload");
            dir.Create();

            Console.WriteLine("-----------------");
            Console.WriteLine("Полный путь: {0}", dir.FullName);
            Console.WriteLine("Название папки: {0}", dir.Name);
            Console.WriteLine("Родительский каталог: {0}", dir.Parent);
            Console.WriteLine("Время создания: {0}", dir.CreationTime);
            Console.WriteLine("Атрибуты: {0}", dir.Attributes);
            Console.WriteLine("Корневой каталог: {0}", dir.Root);

            dir.CreateSubdirectory(DateTime.Now.ToShortDateString());

            dir = new DirectoryInfo(@"C:\Users\ГерценЕ.LOCAL\source\repos");
            foreach (FileInfo file in dir.GetFiles())
            {
                Console.WriteLine("\n--------------------\n");
                Console.WriteLine("Имя файла: {0}", file.Name);
                Console.WriteLine("Размер файла: {0}", file.Length);
                Console.WriteLine("Время создания файла: {0}", file.CreationTime);
            }

        }

        static void Exmpl03()
        {
            string path = @"C:\Users\ГерценЕ.LOCAL\source\repos\001.txt";

            try
            {
                Console.WriteLine("----------------");

                //считованре по строчно
                using (StreamReader sr = new StreamReader(path, Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
                Console.WriteLine("**********");
                //считование по блочно
                using (StreamReader sr = new StreamReader(path, Encoding.Default))
                {
                    char[] array = new char[4];
                    sr.Read(array, 0, 4);
                    Console.WriteLine(array);

                    Console.WriteLine(sr.ReadToEnd());
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        static void Exmpl04()
        {
            string path = @"C:\Users\ГерценЕ.LOCAL\source\repos\001.txt";

            string text = "";
            try
            {
                //дозапись в файл
                using (StreamWriter sw = new StreamWriter(path, false))
                {
                    sw.WriteLine("****////****");
                    sw.Write("5.5");
                }
            }
            catch (Exception)
            {
            }
        }

        static void Exmpl05()
        {
            State[] states = new State[2];
            states[0] = new State("Казахстан", "Шымкент", 35555, 80.8);
            states[1] = new State("Узбекистан", "Ташкент", 3589555, 90.8);

            string path = @"C:\Users\ГерценЕ.LOCAL\source\repos\states.txt";
            try
            {
                using (BinaryWriter writer =
                    new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
                {
                    foreach (var item in states)
                    {
                        writer.Write(item.name);
                        writer.Write(item.capital);
                        writer.Write(item.area);
                        writer.Write(item.people);
                    }
                }

                using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
                {
                    while (reader.PeekChar() > -1)
                    {
                        string name = reader.ReadString();
                        string capital = reader.ReadString();
                        int area = reader.ReadInt32();
                        double people = reader.ReadDouble();

                        Console.WriteLine("Страна: {0}", name);
                        Console.WriteLine("Столица: {0}", capital);
                        Console.WriteLine("Площадь: {0}", area);
                        Console.WriteLine("Люди: {0}", people);
                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}

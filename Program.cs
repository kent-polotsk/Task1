using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Task1
{
    class Program
    {
        /// <summary>
        /// Заполнение файла тестовыми данными
        /// </summary>
        static void FillingFile(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                Random value = new Random();
                for (int month_count = 0; month_count < 12; month_count++)
                    sw.WriteLine($"{month_count + 1};{value.Next(0, 1000000)};{value.Next(0, 1000000)}");
            }
        }

        /// <summary>
        /// Чтение данных из файла
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>file converted to array</returns>
        static string[,] ReadFromFile(string filename)
        {
            string[,] filedata = new string[12, 3];
            using (StreamReader sr = new StreamReader(filename))
            {
                int row = 0;
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] content = line.Split(';').ToArray();
                    for (int col = 0; col < content.Length; col++)
                        filedata[row, col] = content[col];

                    row++;
                }
            }
            return filedata;
        }

        /// <summary>
        /// Вычисление прибыли и худших месяцев
        /// </summary>
        /// <param name="stringdata"></param>
        /// <returns>decimal data array</returns>
        static decimal[,] Calculate(string[,] stringdata)
        {
            int profit = 0;
            decimal[,] decimdata = new decimal[12, 4];
            for (int row = 0; row < 12; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    decimdata[row, col] = Convert.ToDecimal(stringdata[row, col]);
                }
                if ((decimdata[row, 3] = decimdata[row, 1] - decimdata[row, 2]) > 0)
                    profit++;
            }

            {
                Console.WriteLine("============================================================");
                Console.WriteLine("          Месяц" + "          Доход" + "         Расход" + "        Прибыль");
                for (int i = 0; i < 12; i++)
                {
                    Console.WriteLine($"{decimdata[i, 0],15}{decimdata[i, 1],15}{decimdata[i, 2],15}{decimdata[i, 3],15}");
                }
            }

            Console.WriteLine($"Худшая прибыль в месяцах:");

            decimal[,] buffer_array = new decimal[decimdata.GetLength(0), decimdata.GetLength(1)];
            for (int i = 0; i < decimdata.GetLength(0); i++)
                for (int j = 0; j < decimdata.GetLength(1); j++)
                    buffer_array[i, j] = decimdata[i, j];

            decimal[] buffer_array2 = new decimal[12];
            for (int i = 0; i < 12; i++)
                buffer_array2[i] = decimdata[i, 3];

            decimal min = buffer_array2.Min();
            int counter_min = 1, counter_repeat=0;
            while ((counter_min <= 3)&&(counter_repeat<12))
            {
                for (int i = 0; i < 12; i++)
                    if (buffer_array2[i] == min)
                    {
                        Console.Write(" " + buffer_array[i, 0]);
                        buffer_array2[i] = buffer_array2.Max()+1;
                        counter_repeat++;
                    }

                min = buffer_array2.Min();
                counter_min++;
            }
            Console.WriteLine();
            Console.WriteLine($"Месяцев с положительной прибылью: {profit}");
            return decimdata;
        }

        /// <summary>
        /// Запись в файл
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="decimdata"></param>
        static void FileWrite(string filename, decimal[,] decimdata)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                for (int row = 0; row < 12; row++)
                {
                    sw.WriteLine($"{decimdata[row, 0]};{decimdata[row, 1]};{decimdata[row, 2]};{decimdata[row, 3]}");
                }
            }
        }


        static void Main(string[] args)
        {
            bool continue_program = false;
            do
            {
                string confirm = "y";
                string filename= "1.csv";
               /* do
                {
                    Console.Write("Введите название файла: ");
                    filename = Console.ReadLine();
                    Console.WriteLine("Некорректный ввод, повторите!");
                }
                while (!File.Exists(filename));
                */    

                /*Console.Write($"Заполнить файл {filename} случайными тестовыми данными (y/n)?: ");
                string confirm = Console.ReadLine();
                if (confirm == "y" || confirm == "Y")*/
                  //  FillingFile(filename);

                string[,] str_file = ReadFromFile(filename);
                decimal[,] dec_data = Calculate(str_file);

                FileWrite("Output_" + filename, dec_data);

                Console.Write("Выполнить программу заново (y/n)?: ");
                confirm = Console.ReadLine();
                if (confirm == "y" || confirm == "Y") continue_program = true;
                else continue_program = false;
            }
            while (continue_program);

        }
    }
}

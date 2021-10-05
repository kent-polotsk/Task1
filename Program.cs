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
                    sw.WriteLine($"{month_count + 1}\t{value.Next(0, 1000000)}\t{value.Next(0, 1000000)}");
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
                string line;
                for (int row = 0; row < 12; row++)
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] content = line.Split('\t', '\n').ToArray();
                        for (int col = 0; col < 3; col++)
                            filedata[row, col] = content[col];
                    }
            }

            for (int row = 0; row < filedata.GetLength(0); row++)
            {
                for (int col = 0; col < filedata.GetLength(1); col++)
                {
                    Console.Write(filedata[row, col] + " ");
                }
                Console.WriteLine();
            }
            return filedata;
        }

        /// <summary>
        /// Вычисление прибыли
        /// </summary>
        /// <param name="stringdata"></param>
        /// <returns>decimal data array</returns>
        static decimal[,] Calculate(string[,] stringdata)
        {
            decimal[,] decimdata = new decimal[12, 4];
            for (int row = 0; row < 12; row++)
                for (int col = 0; col < 3; col++)
                {
                    decimdata[row, col] = Convert.ToDecimal(stringdata[row, col]);
                }

            for (int row = 0; row < 12; row++)
            {
                decimdata[row, 3] = decimdata[row, 1] - decimdata[row, 2];
            }

/*
            for (int row = 0; row < decimdata.GetLength(0); row++)
            {
                for (int col = 0; col < decimdata.GetLength(1); col++)
                {
                    Console.Write(decimdata[row, col] + " ");
                }
                Console.WriteLine();
            }*/
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
                    sw.WriteLine($"{decimdata[row, 0]}\t{decimdata[row, 1]}\t{decimdata[row, 2]}\t{decimdata[row, 3]}");
                }
            }
        }

        static void Main(string[] args)
        {
            bool continue_program = false;
            do
            {
                Console.Write("Введите название файла: ");
                string filename = Console.ReadLine();
                Console.Write($"Заполнить файл {filename} случайными тестовыми данными (y/n)?: ");
                string confirm = Console.ReadLine();
                if (confirm == "y" || confirm == "Y")
                    FillingFile(filename);

                string[,] str_file = ReadFromFile(filename);
                decimal[,] dec_data = Calculate(str_file);

                /*for (int row = 0; row < dec_data.GetLength(0); row++)
                {
                    for (int col = 0; col < dec_data.GetLength(1); col++)
                    {
                        Console.Write(dec_data[row, col]+" ");
                    }
                    Console.WriteLine();
                }*/

                FileWrite("1" + filename, dec_data);

                Console.Write("Выполнить программу заново (y/n)?: ");
                confirm = Console.ReadLine();
                if (confirm == "y" || confirm == "Y") continue_program = true;
            }
            while (continue_program);


        }
    }
}

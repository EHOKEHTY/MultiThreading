using System;
using System.Diagnostics;
using System.Threading;

namespace MultiThreading
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            Console.WriteLine("Введите количество потоков");
            var threadCount = int.Parse(Console.ReadLine());

            MultiThreadArray ma = new MultiThreadArray(threadCount, new int[100_000_000]);


            sw.Start();
            ma.MultiArrayGenerator();
            sw.Stop();
            Console.WriteLine($"{threadCount} потоков Заполнили массив за: {sw.Elapsed}");
            sw.Restart();


            sw.Start();
            dynamic temp = ma.MinInArray();
            sw.Stop();
            Console.WriteLine($"{threadCount} потоков Нашли минимум {temp} в массиве за: {sw.Elapsed}");
            sw.Restart();


            sw.Start();
            temp = ma.MaxInArray();
            sw.Stop();
            Console.WriteLine($"{threadCount} потоков Нашли мaксимум {temp} в массиве за: {sw.Elapsed}");
            sw.Restart();


            sw.Start();
            temp = ma.ArraySum();
            sw.Stop();
            Console.WriteLine($"{threadCount} потоков Нашли сумму всех элементов {temp} массива за: {sw.Elapsed}");
            sw.Restart();


            sw.Start();
            temp = ma.ArrayAverage();
            sw.Stop();
            Console.WriteLine($"{threadCount} потоков Нашли среднее всех элементов {temp} массива за: {sw.Elapsed}");
            sw.Restart();


            sw.Start();
            temp = ma.CopyPartOdArray(0, 10);
            sw.Stop();
            Console.WriteLine($"{threadCount} потоков выделили часть массива за: {sw.Elapsed}");

            //foreach (var item in temp)
            //{
            //    Console.WriteLine(item);
            //}

            sw.Restart();


            sw.Start();
            temp = ma.GetCharFrequency("C:\\Users\\jenya\\OneDrive\\Desktop\\Новый текстовый документ.txt");
            sw.Stop();
            Console.WriteLine("Частотный словарь символов в тексте:");

            foreach (var pair in temp)
            {
                Console.WriteLine($"Символ: {pair.Key}, Частота: {pair.Value}");
            }
            Console.WriteLine($"Поиск частоты символов выполнен за: {sw.Elapsed}");

            sw.Restart();


            sw.Start();
            temp = ma.GetWordFrequency("C:\\Users\\jenya\\OneDrive\\Desktop\\Новый текстовый документ.txt");
            sw.Stop();

            //Console.WriteLine("Частотный словарь слов в тексте:");
            //foreach (var pair in temp)
            //{
            //    Console.WriteLine($"Слово: {pair.Key}, Частота: {pair.Value}");
            //}

            Console.WriteLine($"Поиск частоты слов выполнен за: {sw.Elapsed}");
            sw.Restart();
        }
    }
}



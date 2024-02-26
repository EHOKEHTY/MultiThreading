using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading
{
    internal class MultiThreadArray
    {
        private int[] array;
        private Thread[] threads;
        private Random[] randoms;

        public MultiThreadArray(int threadsCount, int[] array)
        {
            this.array = array;
            threads = new Thread[threadsCount];
            randoms = new Random[threadsCount];
            for (int i = 0; i < threadsCount; i++)
            {
                randoms[i] = new Random(i); // Указал сид для проверки работоспособности
            }
        }

        public void MultiArrayGenerator()
        {
            int threadCount = threads.Length;
            int chunkSize = array.Length / threadCount;

            for (int i = 0; i < threadCount; i++)
            {
                int index = i;
                threads[i] = new Thread(() =>
                {
                    for (int j = index * chunkSize; j <= (index + 1) * chunkSize && j < array.Length; j++)
                    {
                        array[j] = randoms[index].Next(120, 2000);
                    }
                });
                threads[i].Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
        }

        public int MinInArray()
        {
            int min = int.MaxValue;
            int threadCount = threads.Length;
            int chunkSize = array.Length / threadCount;

            int[] results = new int[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                int index = i;
                results[i] = int.MaxValue;
                threads[i] = new Thread(() =>
                {
                    for (int j = index * chunkSize; j < (index + 1) * chunkSize && j < array.Length; j++)
                    {
                        if (array[j] < results[index])
                            results[index] = array[j];
                    }
                });
                threads[i].Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            foreach (int num in results)
            {
                if (num < min)
                    min = num;
            }
            return min;
        }

        public int MaxInArray()
        {
            int max = int.MinValue;
            int threadCount = threads.Length;
            int chunkSize = array.Length / threadCount;

            int[] results = new int[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                int index = i;
                threads[i] = new Thread(() =>
                {

                    for (int j = index * chunkSize; j < (index + 1) * chunkSize && j < array.Length; j++)
                    {
                        if (array[j] > results[index])
                            results[index] = array[j];
                    }
                });
                threads[i].Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            foreach (int num in results)
            {
                if (num > max)
                    max = num;
            }

            return max;
        }

        public long ArraySum()
        {
            int threadCount = threads.Length;
            int chunkSize = array.Length / threadCount;

            long sum = 0;
            long[] results = new long[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                int index = i;
                threads[i] = new Thread(() =>
                {
                    for (int j = index * chunkSize; j < (index + 1) * chunkSize && j < array.Length; j++)
                    {
                        results[index] += array[j];
                    }
                });

                threads[i].Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            foreach (var num in results)
            {
                sum += num;
            }

            return sum;
        }

        public float ArrayAverage()
        {
            return (float)(ArraySum() / array.Length);
        }

        public int[] CopyPartOdArray(int startIndex, int length)
        {
            int[] destinationArray = new int[length];
            Thread[] threads = new Thread[length];

            for (int i = 0; i < length; i++)
            {
                int index = i;
                threads[i] = new Thread(() =>
                {
                    destinationArray[index] = array[startIndex + index];
                });
                threads[i].Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            return destinationArray;
        }

        public Dictionary<char, int> GetCharFrequency(string filePath)
        {
            Dictionary<char, int> charFrequency = new Dictionary<char, int>();

            string text = File.ReadAllText(filePath);

            int chunkSize = text.Length / threads.Length;
            for (int i = 0; i < threads.Length; i++)
            {
                int startIndex = i * chunkSize;
                int endIndex = (i == threads.Length - 1) ? text.Length : (i + 1) * chunkSize;

                threads[i] = new Thread(() =>
                {
                    Dictionary<char, int> localCharFrequency = new Dictionary<char, int>();

                    for (int j = startIndex; j < endIndex; j++)
                    {
                        char c = char.ToLower(text[j]);
                        if (char.IsLetter(c))
                        {
                            if (localCharFrequency.ContainsKey(c))
                            {
                                localCharFrequency[c]++;
                            }
                            else
                            {
                                localCharFrequency[c] = 1;
                            }
                        }
                    }

                    lock (this)
                    {
                        foreach (var pair in localCharFrequency)
                        {
                            if (charFrequency.ContainsKey(pair.Key))
                            {
                                charFrequency[pair.Key] += pair.Value;
                            }
                            else
                            {
                                charFrequency[pair.Key] = pair.Value;
                            }
                        }
                    }
                });

                threads[i].Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            return charFrequency;
        }

        public Dictionary<string, int> GetWordFrequency(string filePath)
        {
            Dictionary<string, int> wordFrequency = new Dictionary<string, int>();

            string text = File.ReadAllText(filePath);

            int chunkSize = text.Length / threads.Length;
            for (int i = 0; i < threads.Length; i++)
            {
                int startIndex = i * chunkSize;
                int endIndex = (i == threads.Length - 1) ? text.Length : (i + 1) * chunkSize;

                threads[i] = new Thread(() =>
                {
                    Dictionary<string, int> localWordFrequency = new Dictionary<string, int>();

                    string[] words = text.Substring(startIndex, endIndex - startIndex).Split(new char[] { ' ', '.', ',', ';', ':', '-', '!', '?', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string word in words)
                    {
                        string normalizedWord = word.ToLower();
                        if (localWordFrequency.ContainsKey(normalizedWord))
                        {
                            localWordFrequency[normalizedWord]++;
                        }
                        else
                        {
                            localWordFrequency[normalizedWord] = 1;
                        }
                    }

                    lock (this)
                    {
                        foreach (var pair in localWordFrequency)
                        {
                            if (wordFrequency.ContainsKey(pair.Key))
                            {
                                wordFrequency[pair.Key] += pair.Value;
                            }
                            else
                            {
                                wordFrequency[pair.Key] = pair.Value;
                            }
                        }
                    }
                });

                threads[i].Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            return wordFrequency;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Resources;

namespace Dictionary
{
    class SmartDictionary
    {
        string wordsUri;        
        string[] words;

        public SmartDictionary(string[] words)
        {
            this.words = words;
        }
        public SmartDictionary(string wordsUri)
        {
            this.wordsUri = wordsUri;
            using (StreamReader reader = new StreamReader(new FileStream(wordsUri, FileMode.OpenOrCreate, FileAccess.Read)))
            {
                string[] split_opt = new string[] { "\r\n" };
                words = reader.ReadToEnd().Split(split_opt, StringSplitOptions.None);
            }
        }

        public void AddWordToTheDictionary(string word)
        {
            var write = ("\r\n" + word);
            File.AppendAllText(wordsUri, write);
            Array.Resize(ref words, words.Length + 1);
            words[words.Length - 1] = word;
        }

        public bool Contains(string word)
        {
            return words.Contains(word);
        }

        public List<string> FindClosestWords(string input_word, int count)
        {
            Dictionary<string, int> words_with_distance = new Dictionary<string, int>();
            for (int i = 0; i < words.Length; i++)
            {
                string word_from_dictionary = words[i];
                int distance = LevenshteinDistance(word_from_dictionary, input_word);
                words_with_distance.Add(key: word_from_dictionary, value: distance);
            }

            // sort words_with_distance by length from min to max
            List<string> sorted = words_with_distance.OrderBy(item => item.Value).Select(item => item.Key + " (" + item.Value + ")").ToList();

            if (sorted.Count <= count) return sorted;

            List<string> founded = new List<string>();
            for (int i = 0; i < count; i++)
            {
                founded.Add(sorted[i]);
            }
            return founded;
        }

        static int Minimum(params int[] numbers)
        {
            return numbers.Min();
        }

        static int LevenshteinDistance(string firstWord, string secondWord)
        {
            var n = firstWord.Length + 1;
            var m = secondWord.Length + 1;
            var array = new int[n, m];

            int deletion_cost = 1, insertion_cost = 1, substitution_cost = 1;

            for (var i = 0; i < n; i++)
            {
                array[i, 0] = i;
            }

            for (var j = 0; j < m; j++)
            {
                array[0, j] = j;
            }

            for (var i = 1; i < n; i++)
            {
                for (var j = 1; j < m; j++)
                {
                    var substitution = firstWord[i - 1] == secondWord[j - 1] ? 0 : substitution_cost;

                    array[i, j] = Minimum(array[i - 1, j] + deletion_cost,          // удаление
                                            array[i, j - 1] + insertion_cost,         // вставка
                                            array[i - 1, j - 1] + substitution); // замена
                }
            }

            return array[n - 1, m - 1];
        }

    }
}
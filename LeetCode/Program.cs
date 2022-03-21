using System;
using System.Collections.Generic;
using System.Linq;

namespace LeetCode
{
    class Program
    {
        static void Main(string[] args)
        {
            TestFindSubstring();
        }
        private static void TestFindSubstring()
        {
            string s = "barfoothefoobarman";
            var words = new string[] { "foo", "bar" };

            Console.WriteLine(String.Join(",", FindSubstring(s, words)));
            Console.WriteLine("Expected: 0,9");
        }
        public static List<int> FindSubstring(String s, String[] words)
        {
            var map = new Dictionary<string,int>();
            List<int> res = new List<int>();
            foreach (var str in words)
                if (map.ContainsKey(str))
                    map[str]++;
                else
                    map.Add(str, 1);

            int wordLen = words[0].Length;

            for (int i = 0; i < s.Length - wordLen * words.Length + 1; i++)
            {

                var copy = new Dictionary<string,int>(map);

                for (int j = i; j < i + wordLen * words.Length; j += wordLen)
                {
                    String sub = s.Substring(j, wordLen);
                    if (!copy.ContainsKey(sub) || copy[sub] == 0)
                        break;
                    copy[sub]--;
                    if (copy[sub] == 0)
                    {
                        copy.Remove(sub);
                    }
                    if (copy.Count == 0)
                    {
                        res.Add(i);
                    }
                }
            }
            return res;
        }
        public static IList<int> FindSubstring0(string s, string[] words)
        {
            List<int> result = new List<int>();
            if (s == null || s.Length == 0 || words == null || words.Length == 0) return result;
            int wordLength = words[0].Length;
            int wordsCount = words.Count();

            Dictionary<string, int> dict = new Dictionary<string, int>();
            Dictionary<string, int> copyDict = new Dictionary<string, int>();

            foreach (string word in words)
            {
                if (!dict.Keys.Contains(word))
                {
                    dict.Add(word, 1);
                    copyDict.Add(word, 1);
                }
                else
                {
                    dict[word]++;
                    copyDict[word]++;
                }
            }

            for (int i = 0; i < wordLength; i++)
            {
                int count = 0;
                resetDict(copyDict, dict);
                for (int j = i; j <= s.Length - wordLength; j += wordLength)
                {
                    string word = s.Substring(j, wordLength);
                    if (copyDict.Keys.Contains(word))
                    {
                        copyDict[word]--;
                        if (copyDict[word] >= 0)
                        {
                            count++;
                        }
                    }

                    int popStart = j - wordsCount * wordLength;
                    if (popStart >= 0)
                    {
                        string popWord = s.Substring(popStart, wordLength);
                        if (copyDict.Keys.Contains(popWord))
                        {
                            copyDict[popWord]++;
                            if (copyDict[popWord] > 0)
                            {
                                count--;
                            }
                        }
                    }
                    if (count == wordsCount)
                    {
                        result.Add(popStart + wordLength);
                    }
                }
            }
            return result;
        }
        public static void resetDict(Dictionary<string, int> copyDict, Dictionary<string, int> dict)
        {
            copyDict.Keys.ToList().ForEach(key => copyDict[key] = dict[key]);
        }
    }
}

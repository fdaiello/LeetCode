using System;
using System.Collections.Generic;
using System.Linq;

namespace LeetCode
{
    class Program
    {
        static void Main(string[] args)
        {
            TestMinWindow();
        }
        // Bug
        public static string MinWindow(string s, string t)
        {
            int slen = s.Length;
            int tlen = t.Length;
            string ans = "";

            var tfmap = new Dictionary<char, int>(); // freq map of string t
            foreach (var ch in t)
                if (tfmap.ContainsKey(ch))
                    tfmap[ch]++;
                else
                    tfmap.Add(ch, 1);

            var sfmap = new Dictionary<char, int>(); // freq map of string s (for a window of s)
            int match = 0, exactmatch = tlen;

            int i = -1, j = -1;
            while (true)
            {
                bool f1 = false;
                bool f2 = false;
                // acquire
                while (i < slen - 1 && match < exactmatch)
                {
                    f1 = true;
                    i++;
                    if (sfmap.ContainsKey(s[i]))
                        sfmap[s[i]]++;
                    else
                        sfmap.Add(s[i], 1);
                    if (!tfmap.ContainsKey(s[i]) || sfmap[s[i]] <= tfmap[s[i]])
                        match++;
                }

                // release
                while (j < i && match == exactmatch)
                {
                    f2 = true;
                    string tans = s.Substring(j + 1, i - j);
                    if (ans.Length == 0 || tans.Length < ans.Length)
                    {
                        ans = tans;
                    }
                    j++;
                    sfmap[s[j]]--;
                    if (!tfmap.ContainsKey(s[j]) || sfmap[s[j]] < tfmap[s[j]])
                        match--;
                }
                if (f1 == false && f2 == false)
                {
                    break;
                }
            }
            return ans;
        }

        // Too slowly --- timed out
        public static string MinWindow1(string s, string t)
        {
            string minSub = string.Empty;

            // Build map with all chars in t, with quantity
            var map = new Dictionary<char, int>();
            foreach (var c in t)
            {
                if (map.ContainsKey(c))
                    map[c]++;
                else
                    map.Add(c, 1);
            }

            for (int i = 0; i < s.Length - t.Length + 1; i++)
            {
                var map2 = new Dictionary<char, int>(map);
                var sub = MinWindowRec(s.Substring(i), map2);
                if (!String.IsNullOrEmpty(sub) && (sub.Length < minSub.Length || minSub == string.Empty))
                    minSub = sub;
            }

            return minSub;
        }
        public static string MinWindowRec(string s, Dictionary<char, int> map)
        {
            // Interate s looking for chars in t
            int pStart = -1;
            int pEnd = 0;
            int pThis = 0;

            foreach (char c in s)
            {
                if (map.ContainsKey(c) && map[c] > 0)
                {
                    map[c]--;
                    if (map[c] == 0)
                        map.Remove(c);
                    if (pStart == -1)
                        pStart = pThis;
                    if (map.Count == 0)
                    {
                        pEnd = pThis;
                        break;
                    }
                }
                pThis++;
            }

            if (map.Count > 0)
                return string.Empty;
            else
                return s.Substring(pStart, pEnd - pStart + 1);

        }

        // Bugged - cannot find the minimum substring
        public static string MinWindow0(string s, string t)
        {
            // Build map with all chars in t, with quantity
            var map = new Dictionary<char, int>();
            foreach ( var c in t)
            {
                if (map.ContainsKey(c))
                    map[c]++;
                else
                    map.Add(c, 1);
            }

            // Interate s looking for chars in t
            int pStart = -1;
            int pEnd = 0;
            int pThis = 0;

            foreach(char c in s)
            {
                if (map.ContainsKey(c) && map[c] > 0)
                {
                    map[c]--;
                    if (map[c] == 0)
                        map.Remove(c);
                    if (pStart == -1)
                        pStart = pThis;
                    if (map.Count == 0)
                    {
                        pEnd = pThis;
                        break;
                    }
                }
                pThis++;
            }

            if (map.Count > 0)
                return "";
            else
                return s.Substring(pStart, pEnd - pStart + 1);

        }
        public static void TestMinWindow()
        {
            string s = "ADOBECODEBANC";
            string t = t = "ABC";
            Console.WriteLine(MinWindow(s, t));
            Console.WriteLine("Expected: BANC");

            s = "a";
            t = "a";
            Console.WriteLine(MinWindow(s, t));
            Console.WriteLine("Expected: a");

            s = "a";
            t = "aa";
            Console.WriteLine(MinWindow(s, t));
            Console.WriteLine("Expected: empty string");


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

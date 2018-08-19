using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LocalizationHelper
{
    public static class KnownValues
    {
        private static Dictionary<string, Dictionary<string, Dictionary<string, int>>> _keyValuePairs = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();
        public static Dictionary<(string lang, string key), int> NotKnown = new Dictionary<(string lang, string key), int>();
        public static int Hits;
        public static int Misses;
        private static void AddNotKnown(string value, string lang)
        {
            var dictKey = $"[{lang}]{value}";
            if (!NotKnown.ContainsKey((lang, value)))
            {
                NotKnown.Add((lang, value), 0);
            }
            NotKnown[(lang, value)]++;
        }
        public static void AddValue(string lang, string key, string value)
        {
            if (!_keyValuePairs.ContainsKey(lang))
            {
                _keyValuePairs.Add(lang, new Dictionary<string, Dictionary<string, int>>());
            }
            var langDict = _keyValuePairs[lang];
            if (!langDict.ContainsKey(key))
            {
                langDict.Add(key, new Dictionary<string, int>());
            }
            var langKeyDict = langDict[key];
            if (!langKeyDict.ContainsKey(value))
            {
                langKeyDict.Add(value, 0);
            }
            langKeyDict[value]++;
        }
        public static string GetValue(string lang, string key)
        {
            if (_keyValuePairs.ContainsKey(lang) && _keyValuePairs[lang].ContainsKey(key))
            {
                var values = _keyValuePairs[lang][key];
                var max = values.Max(i => i.Value);
                Hits++;
                return values.First(i => i.Value == max).Key;
            }
            AddNotKnown(key, lang);
            Misses++;
            return key;
        }
        public static int ExtractNewValues(root root, string lang)
        {
            var ValidDataPoints = root.data.Where(i => i.Generated == null || i.Generated != i.value).ToList();
            foreach (var dataPoint in ValidDataPoints)
            {
                AddValue(lang, dataPoint.name, dataPoint.value);
            }
            return ValidDataPoints.Count();
        }
    }
}

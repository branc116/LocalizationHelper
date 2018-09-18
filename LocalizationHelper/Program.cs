using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace LocalizationHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null ||args.Length < 3)
            {
                Console.WriteLine("Use this tool to generate Resx files from you'r c# project\n\nUse it like this:\n     LocalizationHelper csharProjectFolder ResxOutFolder Languages[]\n\nE.G.\n   LocalizationHelper MyAwesomeCSharpProject\\src MyAwesomeCSharpProject\\src\\Resources en hr rus pl");
                Console.WriteLine("\n\n\nAfter you start you can start filling in the missing localization strings!");
                return;
            }
            var inDir = args[0];
            var outDir = args[1];
            var langs = args.Skip(2).ToArray();
            var a = new ProjectCompiler(inDir, outDir, false, langs);
            a.Start();
            
            var UnKnowns = new List<(string lang, string key)>(KnownValues.NotKnown.ToList().OrderByDescending(i => i.Value).Select(i => i.Key));
            foreach(var unKnown in UnKnowns)
            {
                foreach (var vals in KnownValues.NotKnown.ToList().OrderBy(i => i.Value))
                {
                    Console.WriteLine($"#{vals.Value}  {vals.Key}");

                }
                Console.WriteLine($"Hits: {KnownValues.Hits} Misses: {KnownValues.Misses}");
                Console.WriteLine("Can you tell me how to translate this? (Press enter to skip)");
                Console.Write($"{unKnown}=");
                var value = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    KnownValues.AddValue(unKnown.lang, unKnown.key, value);
                    a.Start();
                }
            }
        }
    }
}

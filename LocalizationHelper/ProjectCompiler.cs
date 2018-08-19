using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LocalizationHelper
{
    public class ProjectCompiler
    {
        private readonly string _projectRoot;
        private readonly string _outputRoot;
        private readonly string[] _languages;
        private readonly bool _findFileds;
        private readonly int UpdatedCount;
        private readonly int AddedCount;
        private readonly int AllCount;
        private readonly int LocalizedCount;
        public ProjectCompiler(string projectRoot, string outputRoot, bool findFields = false, params string[] languages)
        {
            _projectRoot = projectRoot;
            _outputRoot = outputRoot;
            _languages = languages;
            _findFileds = findFields;
        }
        /// <summary>
        /// Returns list of words that are not translated or at least it's possible that they aren't translated.
        /// </summary>
        public void Start()
        {
            IterateOverAllFiles(_outputRoot, i =>
            {
                var fileName = i.Split('.');
                var lang = fileName[fileName.Count() - 2];
                var root = i.DeSeriXML<root>();
                KnownValues.ExtractNewValues(root, lang);
            }, i => i.Split('.').Length > 2 && i.IndexOf("resx") != -1);
            Start(_projectRoot, _outputRoot, _languages);
        }
        private static string FileNameToResxName(string filename, string outdit, string language)
        {
            var newExt = Path.ChangeExtension(filename, $"{language}.resx");
            var onlyName = Path.GetFileName(newExt);
            return Path.Combine(outdit, onlyName);
            //var name = filename.Split('\\').Last();
            //var nameNoExt = name.Substring(0, name.LastIndexOf('.'));
            //return  $"{nameNoExt}.{language}.resx"
        }
        private static void IterateOverAllFiles(string dir, Action<string> action, Func<string, bool> predicate = null)
        {
            if (!Directory.Exists(dir))
                return;
            var files = Directory.GetFiles(dir);
            foreach (var file in files.Where(predicate ?? (i => true)))
            {
                action(file);
            }
            foreach (var dr in Directory.GetDirectories(dir))
            {
                IterateOverAllFiles(dr, action , predicate);
            }
        }
        private void Start(string curIn, string curOut, params string[] langs)
        {
            var files = Directory.GetFiles(curIn);
            var seri = new XmlSerializer(typeof(root));
            foreach (var file in files)
            {
                var l = new LocalizeStringScanner(file);
                if (l.HasLocalizer())
                {
                    foreach (var lang in langs) {
                        var outFile = FileNameToResxName(file, curOut, lang);
                        var names = File.Exists(outFile) ? outFile.DeSeriXML<root>() : l.AllLocalization(_findFileds).Distinct().ToList().MapToroot();
                        root newNames = new root();
                        if (File.Exists(outFile)) {
                            newNames = l.AllLocalization(_findFileds)
                                .Distinct()
                                .Where(i => names.data.All(j => j.name != i && j.Generated != i))
                                .MapToroot();
                            names.data.AddRange(newNames.data);
                        }
                        if (names.data.Any())
                        {
                            var updatedCount = 0;
                            foreach(var name in names.data.Where(i => i.value == i.Generated))
                            {
                                var newValue = KnownValues.GetValue(lang, name.name);
                                if (newValue != name.Generated)
                                {
                                    name.value = newValue;
                                    updatedCount++;
                                }
                            }
                            Console.WriteLine($"{Path.GetRelativePath(_projectRoot, file)}: {newNames.data?.Count() ?? names.data.Count()}+ {names.data.Count()}= {updatedCount}^");
                            if ((newNames.data?.Count() ?? names.data.Count()) > 0 || updatedCount != 0)
                            {
                                Directory.CreateDirectory(curOut);
                                Console.WriteLine($"Writing to: {Path.GetRelativePath(_outputRoot, outFile)}");
                                seri.Serialize(File.Create(outFile, 4), names);
                            }
                        }
                        Console.WriteLine($"Done with: {file}");
                        Console.WriteLine(new string('-', Console.WindowWidth));
                    }
                }
            }
            foreach(var dir in Directory.GetDirectories(curIn).Where(i => i.IndexOf("node_modu") == -1))
            {
                var newpartOfPath = dir.Split('\\', StringSplitOptions.RemoveEmptyEntries).Last();
                Start(dir, Path.Combine(curOut, newpartOfPath), langs);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LocalizationHelper
{
    class LocalizeStringScanner
    {
        private string _fileContent;
        public LocalizeStringScanner(string fileName)
        {
            _fileContent = fileName.IndexOf(".cs") != -1 ? System.IO.File.ReadAllText(fileName, Encoding.UTF8) : " ";
        }
        public bool HasLocalizer()
        {
            return _fileContent.IndexOf("IStringLocalizer") != -1 || _fileContent.IndexOf("IViewLocalizer") != -1 || _fileContent.IndexOf("Display") != -1;
        }
        public string LocalizerFieldName()
        {
            var startIndex = _fileContent.IndexOf("IStringLocalizer");
            if (startIndex != -1)
            {
                var filedStartIndex = _fileContent.IndexOf("> ", startIndex) + 2;
                var filedEndIndex = _fileContent.IndexOf(";", filedStartIndex);
                return _fileContent.Substring(filedStartIndex, filedEndIndex - filedStartIndex).TrimEnd().TrimStart();
            }
            startIndex = _fileContent.IndexOf("IViewLocalizer"); 
            if (startIndex != -1)
            {
                var filedStartIndex = _fileContent.IndexOf(' ', startIndex);
                var filedEndIndex = _fileContent.IndexOf("\n", filedStartIndex);
                return _fileContent.Substring(filedStartIndex, filedEndIndex - filedStartIndex).TrimEnd().TrimStart();
            }
            return null;
        }
        public IEnumerable<string> AllLocalization(bool includeFileds = false)
        {
            return (includeFileds ? AllLocalizationStrings().Union(AllLocalizationFields().Union(AllLocalizationDisplayFields())) : AllLocalizationStrings()).Union(AllLocalizationDisplayStrings());
        }
        public IEnumerable<string> AllLocalizationStrings()
        {
            var fieldName = LocalizerFieldName();
            if (fieldName == null)
                yield break;
            var index = 0;
            var searchString = $"{fieldName}[\"";
            while(-1 != (index = _fileContent.IndexOf(searchString, index)))
            {
                var startOfString = _fileContent.IndexOf("\"", index);
                var endOfString = _fileContent.IndexOf("\"", startOfString + 1);
                yield return _fileContent.Substring(startOfString + 1, endOfString - startOfString - 1);
                index = endOfString;
            }
            yield break;
        }
        public IEnumerable<string> AllLocalizationFields()
        {
            var fieldName = LocalizerFieldName();
            if (fieldName == null)
                yield break;
            var index = 0;
            var searchString = $"{fieldName}[";
            while (-1 != (index = _fileContent.IndexOf(searchString, index)))
            {
                var startOfFieldName = _fileContent.IndexOf('[', index) + 1;
                if (_fileContent[startOfFieldName] == '"')
                {
                    index = startOfFieldName + 1;
                    continue;
                }
                var endOfFieldName = _fileContent.IndexOf(']', startOfFieldName);
                yield return _fileContent.Substring(startOfFieldName, endOfFieldName - startOfFieldName);
                index = endOfFieldName;
            }
            yield break;
        }
        public IEnumerable<string> AllLocalizationDisplayStrings()
        {
            var index = 0;
            var searchString = $"[Display(Name";
            while (-1 != (index = _fileContent.IndexOf(searchString, index)))
            {
                var startOfString = _fileContent.IndexOf('"', index) + 1;
                var endOfString = _fileContent.IndexOf('"', startOfString);
                if (startOfString < index || endOfString < startOfString)
                    yield break;
                yield return _fileContent.Substring(startOfString, endOfString - startOfString);
                index = endOfString;
            }
            yield break;
        }
        public IEnumerable<string> AllLocalizationDisplayFields()
        {

            var index = 0;
            var searchString = $"[Display(Name";
            while (-1 != (index = _fileContent.IndexOf(searchString, index)))
            {
                var StartOfFieldName = _fileContent.IndexOf('=', index) + 1;
                var EndOfFieldName = _fileContent.IndexOf(')', StartOfFieldName);
                var potentionalString = _fileContent.Substring(StartOfFieldName, EndOfFieldName - StartOfFieldName).Trim();
                if (!string.IsNullOrWhiteSpace(potentionalString) && potentionalString[0] != '"')
                    yield return potentionalString;
                index = EndOfFieldName;
            }
            yield break;
        }

    }
}

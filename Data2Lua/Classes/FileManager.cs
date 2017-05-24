using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Data2Lua.Classes
{
    class FileManager
    {
        public static List<string> GetLines(string file, Encoding encoding)
        {
            List<string> _List = new List<string>();
            _List.Add("");

            using (StreamReader _Reader = new StreamReader(file, encoding))
            {
                string[] _Lines = _Reader.ReadToEnd().Split('\n');

                for(int i = 0; i < _Lines.Length; i++)
                {
                    string _Line = _Lines[i];

                    if (_Line.StartsWith("//") || _Line.StartsWith("-") || string.IsNullOrEmpty(_Line) || string.IsNullOrWhiteSpace(_Line))
                        continue;

                    _List.Add(_Line);
                }
            }

            return _List;
        }

        public static string GetText(string file, Encoding encoding)
        {
            using (StreamReader _Reader = new StreamReader(file, encoding))
            {
                return '\n' + _Reader.ReadToEnd();
            }
        }
    }
}

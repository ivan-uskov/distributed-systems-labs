using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BadWordsReplacer
{
    class ReplaceMap
    {
        const string MAP_FILE_PATH = @"";

        public ReplaceMap()
        {
            string text = System.IO.File.ReadAllText(@"C:\Users\Public\TestFolder\WriteText.txt");
        }
    }
}

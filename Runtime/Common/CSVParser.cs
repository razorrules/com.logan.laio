using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Laio;
using Laio.Tools;

namespace Laio
{
    public class CSVParser
    {

        private string[,] _data;

        public CSVParser(TextAsset csvData)
        {
            Parse(csvData.text);
        }

        public CSVParser(string csvData)
        {
            Parse(csvData);
        }

        private void Parse(string data)
        {

        }



    }
}
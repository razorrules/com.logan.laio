using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Laio;
using Laio.Tools;
using System.Text;

namespace Laio
{
    public class CSVParser
    {

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Laio/Import Test")]
        public static void Import()
        {
            TextAsset csvFile = Resources.Load<TextAsset>("test");

            CSVParser parser = new CSVParser(csvFile);
        }
#endif

        public int HeaderRow = -1;
        public int HeaderColumn = -1;

        private string[,] _data;

        public CSVParser(TextAsset csvData)
        {
            Parse(csvData.text);
        }

        public CSVParser(string csvData)
        {
            Parse(csvData);
        }

        /// <summary>
        /// Get a column of the csv data
        /// </summary>
        /// <param name="column">Column to get</param>
        /// <returns></returns>
        public string[] GetColumn(int column)
        {
            int offset = Mathf.Clamp(HeaderRow + 1, 0, int.MaxValue);
            string[] values = new string[_data.GetLength(1) - offset];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = _data[column, i + offset];
            }
            return values;
        }

        /// <summary>
        /// Get a row of the csv data
        /// </summary>
        /// <param name="row">Row to get</param>
        /// <returns></returns>
        public string[] GetRow(int row)
        {
            int offset = Mathf.Clamp(HeaderColumn + 1, 0, int.MaxValue);
            string[] values = new string[_data.GetLength(0) - offset];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = _data[i + offset, row];
            }
            return values;
        }

        //========== private

        /// <summary>
        /// Parse CSV text.
        /// </summary>
        /// <param name="data">CSV data to parse</param>
        private void Parse(string data)
        {
            string[] lines = data.Split('\n');
            int y = lines.Length;
            //Parse first line to get x
            int x = ParseLine(lines[0]).Length;
            _data = new string[x, y];

            for (int i = 0; i < y; i++)
            {
                ParseLineNonAlloc(lines[i], ref _data, i);
            }
        }

        /// <summary>
        /// Parses the line without allocating memory.
        /// </summary>
        /// <param name="line">Line of data from csv</param>
        /// <param name="output">where to store the data</param>
        /// <param name="y">What row to put data in?</param>
        private void ParseLineNonAlloc(string line, ref string[,] output, int y)
        {
            int index = 0;
            string cell = "";
            int counter = 0;

            foreach (char c in line)
            {
                if (c.Equals(',') && counter % 2 == 0)
                {
                    output[index, y] = cell;
                    cell = "";
                    counter = 0;
                    index++;
                    continue;
                }

                if (c.Equals('"'))
                {
                    counter++;
                    if (counter % 2 != 0)
                        continue;
                }

                cell += c;
            }
            //There is no comma after last element, so parse after finishing.
            output[index, y] = cell;
        }

        /// <summary>
        /// Uses a list of string to parse a line of unknown length. Returns array
        /// </summary>
        /// <param name="line">Line of CSV you want to parse</param>
        /// <returns>parsed data</returns>
        private string[] ParseLine(string line)
        {
            List<string> values = new List<string>();

            string cell = "";
            int counter = 0;

            void ParseCell()
            {
                values.Add(cell);
                cell = "";
                counter = 0;
            }

            foreach (char c in line)
            {
                if (c.Equals(',') && counter % 2 == 0)
                {
                    ParseCell();
                    continue;
                }

                if (c.Equals('"'))
                {
                    counter++;
                    if (counter % 2 != 0)
                        continue;
                }

                cell += c;
            }
            //There is no comma after last element, so parse after finishing.
            ParseCell();
            return values.ToArray();
        }

        public override string ToString()
        {
            StringBuilder _sb = new StringBuilder("CSVParser: \n");
            for (int y = 0; y < _data.GetLength(1); y++)
            {
                for (int x = 0; x < _data.GetLength(0); x++)
                {
                    _sb.Append($"\'{_data[x, y]}\',\t");
                }
                _sb.Append("\n");
            }
            return _sb.ToString();
        }

    }
}
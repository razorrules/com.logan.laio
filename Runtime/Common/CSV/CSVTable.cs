using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Laio;
using Laio.Tools;

namespace Laio
{
    public class CSVTable
    {

        public List<RowData> Columns;
        public List<RowData> Rows;

        public CSVTable(string[,] data, int HeaderRow, int HeaderColumn)
        {

        }

        [System.Serializable]
        public struct RowData
        {
            public RowData(string name)
            {
                Name = name;
                Entries = new List<string>();
                Indexes = new List<int>();
            }

            public string Name { get; private set; }
            public List<string> Entries;
            public List<int> Indexes;
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Laio
{
    [System.Serializable]
    public class EArray<TEnum, TValue> where TEnum : System.Enum
    {
        public EArray() { _values = new TValue[0]; }

        [SerializeField] private TValue[] _values;

        public TValue this[int index]
        {
            get
            {
                return _values[index];
            }
        }

        public int Length
        {
            get
            {
                return _values.Length;
            }
        }

    }
}
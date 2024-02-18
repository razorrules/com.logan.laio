using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laio
{
    //TODO: Add a way to set a method for getting custom names
    public class EArray<TEnum, TValue> where TEnum : System.Enum
    {

        private TEnum _enumType;

        private TValue[] _values;

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
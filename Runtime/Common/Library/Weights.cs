using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Laio;
using Laio.Tools;

namespace Laio
{
    [System.Serializable]
    public class Weights<T>
    {

        public int WeightCount;

        public T[] values;
        public float[] valuesWeight;

        public Weights()
        {
            values = new T[0];
            valuesWeight = new float[0];
        }

        public T Get()
        {
            throw new System.NotImplementedException();
        }

        public T Get(System.Random random)
        {
            throw new System.NotImplementedException();
        }

    }

}
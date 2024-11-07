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

        [SerializeField] private int WeightCount;

        [SerializeField] private T[] values;
        [SerializeField] private int[] valuesWeight;

        public Weights()
        {
            values = new T[0];
            valuesWeight = new int[0];
        }

        private int TotalWeight()
        {
            int returnVal = 0;
            for (int i = 0; i < valuesWeight.Length; i++)
                returnVal += valuesWeight[i];
            return returnVal;
        }

        public T Get()
        {
            if (WeightCount == 0)
            {
                Debug.LogError("Attempted to get random value from Weights, when there are no values and weights setup.");
                return default(T);
            }
            int rng = UnityEngine.Random.Range(0, TotalWeight() + 1);
            int acc = 0;
            for (int i = 0; i < valuesWeight.Length; i++)
            {
                acc += valuesWeight[i];
                if (rng <= acc)
                {
                    return values[i];
                }
            }
            Debug.LogError("Unable to get a value from weights");
            return values[values.Length - 1];
        }

        public T Get(System.Random random)
        {
            if (WeightCount == 0)
            {
                Debug.LogError("Attempted to get random value from Weights, when there are no values and weights setup.");
                return default(T);
            }
            int rng = random.Next(0, TotalWeight() + 1);
            int acc = 0;
            for (int i = 0; i < valuesWeight.Length; i++)
            {
                acc += valuesWeight[i];
                if (rng <= acc)
                {
                    return values[i];
                }
            }
            Debug.LogError("Unable to get a value from weights");
            return values[values.Length - 1];
        }

    }

}
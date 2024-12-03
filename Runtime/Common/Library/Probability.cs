using UnityEditor;
using UnityEngine;

namespace Laio
{

    /// <summary>
    /// A simple class for managing probabilities. Offering the method Flip() where you can get a bool on whether you were
    /// successful based on the probability. Can do operations on probability with floats, integers and other probabilities.
    /// </summary>
    [System.Serializable]
    public class Probability
    {
        //======= Properties
        public float HitProbability { get => _hitProbability; }
        public bool IsSuccessful { get; private set; }

        //======= Private
        [SerializeField] private float _hitProbability;

        public Probability(float probability)
        {
            this._hitProbability = probability;
        }

        /// <summary>
        /// Was successfully
        /// </summary>
        /// <returns></returns>
        public bool Flip()
        {
            IsSuccessful = UnityEngine.Random.Range(0, 100.0f) <= _hitProbability;
            return IsSuccessful;
        }

        public override string ToString()
        {
            return "Probability: " + _hitProbability.ToString("N3") + "%";
        }

        //====== Floats
        public static Probability operator +(Probability a, float b) => new Probability(a.HitProbability + b);
        public static Probability operator -(Probability a, float b) => new Probability(a.HitProbability + b);
        public static Probability operator *(Probability a, float b) => new Probability(a.HitProbability * b);
        public static Probability operator /(Probability a, float b) => new Probability(a.HitProbability / b);

        //====== Integers
        public static Probability operator +(Probability a, int b) => new Probability(a.HitProbability + b);
        public static Probability operator -(Probability a, int b) => new Probability(a.HitProbability + b);
        public static Probability operator *(Probability a, int b) => new Probability(a.HitProbability * b);
        public static Probability operator /(Probability a, int b) => new Probability(a.HitProbability / b);

        //====== Odds
        public static Probability operator +(Probability a, Probability b) => new Probability(a.HitProbability + b.HitProbability);
        public static Probability operator -(Probability a, Probability b) => new Probability(a.HitProbability + b.HitProbability);
        public static Probability operator *(Probability a, Probability b) => new Probability(a.HitProbability * b.HitProbability);
        public static Probability operator /(Probability a, Probability b) => new Probability(a.HitProbability / b.HitProbability);

    }
}
using UnityEngine;

public delegate void OnStatDrain();
public delegate void OnStatFull();
public delegate void OnStatUpdate(float newValue);

namespace Laio.Library
{
    /// <summary>
    /// Struct that can ecapsulate all of Stat's data, can be used to save
    /// or easily set defaults for a given stat.
    /// </summary>
    [System.Serializable]
    public struct StatData
    {
        public float current;
        public float minimum;
        public float maximum;
        public float regenerateRate;
        public float drainRate;

        public StatData(float current, float minimum, float maximum, float regenerateRate, float drainRate)
        {
            this.current = current;
            this.minimum = minimum;
            this.maximum = maximum;
            this.regenerateRate = regenerateRate;
            this.drainRate = drainRate;
        }
    }

    /// <summary>
    /// Generic stat class that has delegates for state changes, a min and max clamp, 
    /// along with regeneration. Class is easily extensible
    /// </summary>
    public class Stat
    {
        public OnStatDrain onStatEmpty;
        public OnStatFull onStatFull;
        public OnStatUpdate onStatUpdate;

        protected float _current;
        protected float _minimum;
        protected float _maximum;
        protected float _regenerateRate;
        protected float _drainRate;

        public float Current
        {
            get
            {
                return _current;
            }
            protected set
            {
                if (_current < _maximum && value >= _maximum)
                    onStatFull?.Invoke();
                if (_current > _minimum && value <= _minimum)
                    onStatEmpty?.Invoke();

                _current = Mathf.Clamp(value, _minimum, _maximum);
                onStatUpdate?.Invoke(value);
            }
        }

        /// <summary>
        /// Initialize stat data with a StatData struct
        /// </summary>
        /// <param name="statData">Data to set stat to</param>
        public Stat(StatData statData)
        {
            _current = statData.current;
            _maximum = statData.maximum;
            _minimum = statData.minimum;
            _regenerateRate = statData.regenerateRate;
            _drainRate = statData.drainRate;
        }

        /// <summary>
        /// Initialize a stat
        /// </summary>
        /// <param name="current">Current value</param>
        /// <param name="maximum">Maximum</param>
        /// <param name="regenerateRate">Regeneration rate</param>
        /// <param name="drainRate">Draining rate</param>
        /// <param name="min">Minimum value</param>
        public Stat(float current, float maximum, float regenerateRate, float drainRate, float min = 0)
        {
            _current = current;
            _maximum = maximum;
            _minimum = min;
            _regenerateRate = regenerateRate;
            _drainRate = drainRate;
        }

        /// <summary>
        /// Get the stat as a StatData
        /// </summary>
        /// <returns></returns>
        public StatData GetStatData()
        {
            return new StatData(Current, _minimum, _maximum, _regenerateRate, _drainRate);
        }

        /// <summary>
        /// Drain the stat, must be called every frame if to be used. (drainRate * Time.deltaTime)
        /// </summary>
        /// <param name="ammount"></param>
        public virtual void Drain()
        {
            Current -= _drainRate * Time.deltaTime;
        }

        /// <summary>
        /// Adjusts the stat. does NOT apply Time.deltaTime
        /// </summary>
        /// <param name="ammount"></param>
        public virtual void Adjust(float ammount)
        {
            Current += ammount;
        }

        /// <summary>
        /// This must be called every frame if you wish to use regeneration.
        /// (drainRate * Time.deltaTime)
        /// regeneration.
        /// </summary>
        public virtual void Regenerate()
        {
            Current += _regenerateRate * Time.deltaTime;
        }

    }

}
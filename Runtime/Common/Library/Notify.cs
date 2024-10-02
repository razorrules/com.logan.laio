namespace Laio.Library
{
    public class Notify<T> where T : struct
    {
        public delegate void OnValueChange(T newValue);
        public delegate void OnValueChangeWithOld(T oldValue, T newValue);

        public OnValueChange onValueChange;
        public OnValueChangeWithOld onValueChangeWithOld;

        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                T oldValue = _value;
                _value = value;
                onValueChangeWithOld?.Invoke(oldValue, value);
                onValueChange?.Invoke(value);
            }
        }
        public void Set(T newValue) => Value = newValue;
    }
}

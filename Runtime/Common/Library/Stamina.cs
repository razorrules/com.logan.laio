using UnityEngine;

[System.Serializable]
public class Stamina
{
    public float Value { get; private set; }

    [SerializeField] private float _min = 0;
    [SerializeField] private float _max = 100;
    [SerializeField] private float _requiredStaminaForSprint = 20;
    [Space]
    [SerializeField] private float _regenSpeed;
    [SerializeField] private float _consumptionSpeed;

    private bool _isSprinting;

    public Stamina(float regen, float consumption)
    {
        _min = 0;
        _max = 100;
        _requiredStaminaForSprint = 20;
        _regenSpeed = 5;
        _consumptionSpeed = 10;
        _isSprinting = false;
    }

    public Stamina(float min, float max, float requiredStaminaForSprint, float regenSpeed, float consumptionSpeed)
    {
        _min = min;
        _max = max;
        _requiredStaminaForSprint = requiredStaminaForSprint;
        _regenSpeed = regenSpeed;
        _consumptionSpeed = consumptionSpeed;
        _isSprinting = false;
    }

    public void ResetStamina()
    {
        Value = _max;
    }

    public bool TrySpend(float cost)
    {
        if (Value >= cost)
        {
            Value -= cost;
            return true;
        }
        return false;
    }

    public void Update(bool attemptingSprint)
    {
        if (_isSprinting)
        {
            Value -= _consumptionSpeed * Time.deltaTime;
            if (!attemptingSprint)
                _isSprinting = false;
            else if (Value <= 0)
                _isSprinting = false;
        }
        else
        {
            Value += _regenSpeed * Time.deltaTime;
            if (attemptingSprint && Value > _requiredStaminaForSprint)
            {
                _isSprinting = true;
                Value -= _consumptionSpeed * Time.deltaTime;
            }
        }

        Value = Mathf.Clamp(Value, _min, _max);
    }

    public void SetRegeneration(float regeneration)
    {
        _regenSpeed = regeneration;
    }

    public void SetConsumption(float consumption)
    {
        _consumptionSpeed = consumption;
    }

    public bool IsSprinting()
    {
        return _isSprinting;
    }
}
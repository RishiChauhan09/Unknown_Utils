using System;
using System.Collections.Generic;
using UnityEngine;

public interface ICurrencyService {

    /// <summary>
    /// This currency multiplier is for powerups or something like ad and all.
    /// !!!!! Please don't edit this directly !!!!!
    /// </summary>
    public float CurrencyMultiplier { get; set; }
    public List<float> CurrentMultipliersList { get; set; }

    public void Initialize(List<CurrencyInfo> defaultCurrencies);
    public void AddCurrency(CurrencyType currencyType, int amount);
    public bool SpendCurrency(CurrencyType currencyType, int amount);
    public bool CanSpend(CurrencyType type, int amount);
    public string GetCurrencyInFormat(CurrencyType type);
    public int GetCurrency(CurrencyType type);

    // events 
    public static event Action<CurrencyDataEventArgs> OnCurrencyUpdate;
    public static void InvokeCurrencyChangeEvent(CurrencyDataEventArgs currencyDataEventArgs) {
        OnCurrencyUpdate?.Invoke(currencyDataEventArgs);
    }

    /// <summary>
    /// For adding multiplier to currency
    /// </summary>
    public void AddCurrencyMultiplier(float multiplier) {
        CurrencyMultiplier *= multiplier;

        if(CurrentMultipliersList == null) {
            CurrentMultipliersList = new List<float>();
        }

        CurrentMultipliersList.Add(multiplier);
    }

    /// <summary>
    /// For removing multiplier to currency.
    /// </summary>
    public void RemoveCurrencyMultiplier(float multipler) {
        CurrencyMultiplier /= multipler;

        if(CurrentMultipliersList.Contains(multipler)) {
            CurrentMultipliersList.Remove(multipler);
        } else {
            Debug.LogError("There is no multiplier of " + multipler + " in currency multiplier");
        }
    }

}

public class CurrencyDataEventArgs {
    public CurrencyType currencyType;
    public int currentValue;
}

public enum CurrencyType {
    Coins,
}


[Serializable]
public struct CurrencyInfo {
    public CurrencyType currencyType;
    public int amount;
}
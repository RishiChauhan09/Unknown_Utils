using System.Collections.Generic;
using UnityEngine;

public class FakeCurrencyService : ICurrencyService {

    public float CurrencyMultiplier { get; set; }
    public List<float> CurrentMultipliersList { get; set; }

    public void AddCurrency(CurrencyType currencyType, int amount) {
        return;
    }

    public bool CanSpend(CurrencyType type, int amount) {
        return true;
    }

    public int GetCurrency(CurrencyType type) {
        return 1;
    }

    public string GetCurrencyInFormat(CurrencyType type) {
        return "1";
    }

    public void Initialize(List<CurrencyInfo> defaultCurrencies) {
        return;
    }

    public bool SpendCurrency(CurrencyType currencyType, int amount) {
        return true;
    }
}
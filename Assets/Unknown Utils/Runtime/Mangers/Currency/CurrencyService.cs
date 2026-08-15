using System;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyService : ICurrencyService {

    private Dictionary<CurrencyType, int> currentCurrencies;

    public float CurrencyMultiplier { get; set; } = 1;
    public List<float> CurrentMultipliersList { get; set; }

    // -- events -----------------------------

    /// <summary>
    /// this is for initializing the currency manager at start of game
    /// </summary>
    public void Initialize(List<CurrencyInfo> currenciesInfo) {
        currentCurrencies = new Dictionary<CurrencyType, int>();

        foreach(CurrencyInfo dc in currenciesInfo) {
            currentCurrencies[dc.currencyType] = dc.amount;
        }
    }

    /// <summary>
    /// To add currency.
    /// </summary>
    public void AddCurrency(CurrencyType currencyType, int amount) {
        currentCurrencies[currencyType] += (int)(amount * CurrencyMultiplier);
        ICurrencyService.InvokeCurrencyChangeEvent(new CurrencyDataEventArgs() {
            currencyType = currencyType,
            currentValue = currentCurrencies[currencyType]
        });
    }

    /// <summary>
    /// Spending currency returns true when spent.
    /// </summary>
    public bool SpendCurrency(CurrencyType currencyType, int amount) {
        if (currentCurrencies[currencyType] < amount) {
            return false;
        }

        currentCurrencies[currencyType] -= amount;
        ICurrencyService.InvokeCurrencyChangeEvent(new CurrencyDataEventArgs() {
            currencyType = currencyType,
            currentValue = currentCurrencies[currencyType]
        });
        return true;
    }

    /// <summary>
    /// To check weather we have that much currency or not.
    /// </summary>
    public bool CanSpend(CurrencyType type, int amount) {
        if (currentCurrencies[type] >= amount) {
            return true;
        } else {
            return false;
        }
    }

    /// <summary>
    /// Returns the currency in format.
    /// </summary>
    public string GetCurrencyInFormat(CurrencyType type) {
        //return currentCurrencies[type].GetFormat();
        return currentCurrencies[type].ToString();          // make sure you change it with your format method
    }

    /// <summary>
    /// Getting the currency of type.
    /// </summary>
    public int GetCurrency(CurrencyType type) {
        return currentCurrencies[type];
    }
}
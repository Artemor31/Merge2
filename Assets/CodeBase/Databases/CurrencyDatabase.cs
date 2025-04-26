using System;
using System.Collections.Generic;
using Infrastructure;
using UnityEngine;

namespace Databases
{
    public class CurrencyDatabase : Database
    {
        [SerializeField] private List<CurrencyData> _currencyDatas;
        public IReadOnlyList<CurrencyData> CurrencyDatas => _currencyDatas;
    }

    [Serializable]
    public class CurrencyData
    {
        public Currency Type;
        public Sprite Sprite;
        public string NameEn;
        public string NameRu;
        public string NameTr;
    }
}
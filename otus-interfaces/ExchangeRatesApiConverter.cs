using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace otus_interfaces
{
    public class ExchangeRatesApiConverter : ICurrencyConverter
    {
        private HttpClient _httpclient;
        private IMemoryCache _memoryCache;
        private string _apiKey;
        private string _basecurrency;
    
        public ExchangeRatesApiConverter(HttpClient httpClient, IMemoryCache memoryCache, string apiKey, string baseCurrency)
        {   _httpclient = httpClient;
            _memoryCache = memoryCache;
            _apiKey = apiKey;
            _basecurrency = baseCurrency;
          }

        public ICurrencyAmount ConvertCurrency(ICurrencyAmount amount, string currencyCode)
        {
            return ConvertCurrencyAsync(amount, currencyCode).ConfigureAwait(false).GetAwaiter().GetResult();
        }


        public async Task<ICurrencyAmount> ConvertCurrencyAsync(ICurrencyAmount amount, string currencyCode)
        {
            var _rates = await _memoryCache.GetOrCreateAsync<RatesAPI>("rates_api", entry => GetRatesAsync());
            if (_rates.Rates.TryGetValue(currencyCode!=_basecurrency? currencyCode: amount.CurrencyCode, out var rate))
            {
                return CnvAmount(amount, currencyCode, rate);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(currencyCode), $"Не найден курс для валюты: {currencyCode}");
            }
            
                                
        }

        public async Task<RatesAPI> GetRatesAsync()

        {
            HttpResponseMessage response = await _httpclient.GetAsync("http://api.exchangeratesapi.io/v1/latest?access_key=" + _apiKey + "&format=1");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var _rates = RatesAPI.FromJson(responseBody);
            return RateToBase(_rates);

        }

        public RatesAPI RateToBase(RatesAPI _rates)
        { if (_rates.Base == _basecurrency)
                return _rates;
            RatesAPI _ratesinbase = new RatesAPI();
            _ratesinbase.Base = _rates.Base;
            _ratesinbase.Date = _rates.Date;
            _ratesinbase.Success = _rates.Success;
            _ratesinbase.Timestamp = _rates.Timestamp;
            _ratesinbase.Rates = new Dictionary<string, double>();
            double baserate = _rates.Rates[_basecurrency];
            foreach (KeyValuePair<string, double> keyvalue in _rates.Rates)
            {
                _ratesinbase.Rates.TryAdd(keyvalue.Key, keyvalue.Value >= 1 ? keyvalue.Value / baserate : baserate / keyvalue.Value);
            }

            return _ratesinbase;
        }

        public  ICurrencyAmount CnvAmount (ICurrencyAmount amountInfo, string currencyCode, double rate)
        {
            var newAmount = ((amountInfo.CurrencyCode==_basecurrency)? amountInfo.Amount * (decimal)rate: amountInfo.Amount / (decimal)rate);
            var convertedAmountInfo = new CurrencyAmount(currencyCode, newAmount);

            return convertedAmountInfo;
        }

    }
}
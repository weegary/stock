namespace Stock.Data.Indicator
{
    public class EMA
    {
        public static decimal[] CaclulateEMA(Candlestick[] candlesticks, int period = 12)
        {
            decimal[] emaValues = new decimal[candlesticks.Length];
            for (int i = 0; i < candlesticks.Length; i++)
            {
                if (i == 0)
                {
                    emaValues[i] = candlesticks[i].ClosePrice;
                }
                else
                {
                    emaValues[i] = (candlesticks[i].ClosePrice - emaValues[i - 1]) * (2m / (period + 1)) + emaValues[i - 1];
                }
            }
            return emaValues;
        }
    }
}

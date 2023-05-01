namespace Stock.Data.Indicator
{
    public class SMA
    {
        public static decimal CalculateSMA_Last(Candlestick[] candlesticks, int length)
        {
            decimal sum = 0;
            for (int i = candlesticks.Length - 1; i >= candlesticks.Length - length; i--)
            {
                sum += candlesticks[i].ClosePrice;
            }
            decimal sma = sum / length;
            return sma;
        }

        public static decimal[] CalculateSMA(Candlestick[] candlesticks, int length)
        {
            decimal[] smaValues = new decimal[candlesticks.Length];
            for (int i = 0; i < candlesticks.Length; i++)
            {
                if (i < length - 1)
                {
                    smaValues[i] = 0;
                }
                else
                {
                    decimal sum = 0;
                    for (int j = i; j > i-length; j--)
                    {
                        sum += candlesticks[j].ClosePrice;
                    }
                    smaValues[i] = Math.Round(sum / length,2);
                }
            }
            return smaValues;
        }
    }
}

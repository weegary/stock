namespace Stock.Data.Indicator
{
    public class MACD
    {
        public static (decimal[] macd, decimal[] osc, decimal[] dif) CalculateMACD(Candlestick[] candlesticks, int slowPeriod = 26, int fastPeriod = 12, int signalPeriod = 9)
        {
            // Calculate EMA12 and EMA26
            decimal[] ema12Values = new decimal[candlesticks.Length];
            decimal[] ema26Values = new decimal[candlesticks.Length];
            decimal[] difValues = new decimal[candlesticks.Length];
            for (int i = 0; i < candlesticks.Length; i++)
            {
                if (i == 0)
                {
                    ema12Values[i] = candlesticks[i].ClosePrice;
                    ema26Values[i] = candlesticks[i].ClosePrice;
                }
                else
                {
                    ema12Values[i] = (candlesticks[i].ClosePrice - ema12Values[i - 1]) * (2m / (fastPeriod + 1)) + ema12Values[i - 1];
                    ema26Values[i] = (candlesticks[i].ClosePrice - ema26Values[i - 1]) * (2m / (slowPeriod + 1)) + ema26Values[i - 1];
                }
                // Calculate DIF 
                difValues[i] = ema12Values[i] - ema26Values[i];
            }

            // Calculate DEA
            decimal[] deaValues = new decimal[candlesticks.Length];
            for (int i = 0; i < candlesticks.Length; i++)
            {
                if (i != 0)
                    deaValues[i] = (difValues[i] - deaValues[i - 1]) * (2m / (signalPeriod + 1)) + deaValues[i - 1];
                else
                    deaValues[i] = difValues[i];
            }

            // Calculate MACD and OSC
            decimal[] macdValues = new decimal[candlesticks.Length];
            decimal[] oscValues = new decimal[candlesticks.Length];
            for (int i = 0; i < candlesticks.Length; i++)
            {
                macdValues[i] = deaValues[i];
                oscValues[i] = difValues[i] - deaValues[i];

                macdValues[i] = Math.Round(macdValues[i], 2);
                oscValues[i] = Math.Round(oscValues[i], 2);
                difValues[i] = Math.Round(difValues[i], 2);
            }

            return (macdValues, oscValues, difValues);
        }
    }
}

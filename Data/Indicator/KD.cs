namespace Stock.Data.Indicator
{
    public class KD
    {
        /// <summary>
        /// This function calculates the RSV, K and D values. 
        /// For the candlesticks before rsvPeriod, the values will be -1.
        /// </summary>
        /// <param name="candlesticks"></param>
        /// <param name="rsvPeriod"></param>
        /// <param name="kPeriod"></param>
        /// <param name="dPeriod"></param>
        /// <returns></returns>
        public static (decimal[] RSV, decimal[] K, decimal[] D) RSV_KD(Candlestick[] candlesticks, int rsvPeriod = 9, int kPeriod = 3, int dPeriod = 3, decimal pre_k = -1, decimal pre_d = -1)
        {
            decimal[] RSV = new decimal[candlesticks.Length];
            decimal[] K = new decimal[candlesticks.Length];
            decimal[] D = new decimal[candlesticks.Length];

            for (int i = 0; i < candlesticks.Length; i++)
            {
                if (i < rsvPeriod - 1)
                {
                    RSV[i] = -1;
                    K[i] = -1;
                    D[i] = -1;
                    continue;
                }
                decimal min = candlesticks[i].LowPrice;
                decimal max = candlesticks[i].HighPrice;
                for (int j = i - (rsvPeriod - 1); j <= i; j++)
                {
                    if (min > candlesticks[j].LowPrice)
                        min = candlesticks[j].LowPrice;
                    if (max < candlesticks[j].HighPrice)
                        max = candlesticks[j].HighPrice;
                }
                decimal temp1 = (max - min);
                decimal temp2 = (candlesticks[i].ClosePrice - min);
                int index = i; 
                if ((max - min) == 0)
                    RSV[index] = 100;
                else
                    RSV[index] = (decimal)Math.Round((Math.Round((candlesticks[i].ClosePrice - min), 2) / Math.Round((max - min), 2) * 100), 3);
                
                decimal pre_K, pre_D;
                if (index == rsvPeriod - 1)
                {
                    pre_K = 50;
                    pre_D = 50;
                    if (pre_k != -1)
                        pre_K = pre_k;
                    if (pre_d != -1)
                        pre_D = pre_d;
                }
                else
                {
                    pre_K = K[index - 1];
                    pre_D = D[index - 1];
                }
                K[index] = pre_K * 2 / kPeriod + (RSV[index] / kPeriod);
                D[index] = pre_D * 2 / dPeriod + (K[index] / dPeriod);
                K[index] = Math.Round(K[index],2);
                D[index] = Math.Round(D[index],2);
            }
            return (RSV, K, D);
        }
        
        public enum Signal { GoldenCross, DeathCross};

        public bool IsGoldenCross(decimal pre_K, decimal K, decimal pre_D, decimal D)
        {
            if (K < pre_K)
                return false;
            if (pre_K < pre_D && K > D)
                return true;
            return false;
        }

        public bool IsDeathCross(decimal pre_K, decimal K, decimal pre_D, decimal D)
        {
            if (K > pre_K)
                return false;
            if (pre_K > pre_D && K < D)
                return true;
            return false;
        }

        public bool IsOverBuy(decimal K, decimal threshold = 80)
        {
            if (K >= threshold)
                return true;
            return false;
        }

        public bool IsOverSell(decimal K, decimal threshold = 20)
        {
            if (K <= threshold)
                return true;
            return false;
        }
    }
}

namespace Stock.Data.Indicator
{
    public class Trend
    {
        /// <summary>
        /// Find the peaks from candlesticks
        /// </summary>
        /// <param name="candlesticks"></param>
        /// <param name="filter_neighbor">If two peaks are too closed, select the higher</param>
        /// <returns></returns>
        public List<int> FindIndicesOfPeaksFromHighPrice(Candlestick[] candlesticks, int filter_neighbor = 5)
        { 
            List<int> indices = new List<int>();
            if (candlesticks != null && candlesticks.Length != 0)
            {
                for (int i = 1; i < candlesticks.Length - 1; i++)
                {
                    if (candlesticks[i].HighPrice > candlesticks[i-1].HighPrice && candlesticks[i].HighPrice >= candlesticks[i+1].HighPrice)
                    {
                        if (indices.Count > 0 && i - indices.Last() >=0 && i-indices.Last() < filter_neighbor)
                        {
                            if (candlesticks[indices.Last()].HighPrice < candlesticks[i].HighPrice)
                            {
                                indices.Remove(indices.Last());
                            }
                            else
                            {
                                continue;
                            }
                        }
                        indices.Add(i);
                    }
                }
            }
            return indices;
        }

        /// <summary>
        /// Find the troughs from candlesticks
        /// </summary>
        /// <param name="candlesticks"></param>
        /// <param name="filter_neighbor">If two troughs are too closed, select the lower</param>
        /// <returns></returns>
        public List<int> FindIndicesOfTroughsFromLowPrice(Candlestick[] candlesticks, int filter_neighbor = 5)
        {
            List<int> indices = new List<int>();
            if (candlesticks != null && candlesticks.Length != 0)
            {
                for (int i = 1; i < candlesticks.Length - 1; i++)
                {
                    if (candlesticks[i].LowPrice < candlesticks[i - 1].LowPrice && candlesticks[i].LowPrice <= candlesticks[i + 1].LowPrice)
                    {
                        if (indices.Count > 0 && i - indices.Last() >= 0 && i - indices.Last() < filter_neighbor)
                        {
                            if (candlesticks[indices.Last()].LowPrice > candlesticks[i].LowPrice)
                            {
                                indices.Remove(indices.Last());
                            }
                            else
                            {
                                continue;
                            }
                        }
                        indices.Add(i);
                    }
                }
            }
            return indices;
        }

        public enum PeakTrough { Peak,Trough};
        public Dictionary<int, PeakTrough> MergeIndicesOfPeaksAndTroughs(Candlestick[] candlesticks, List<int> indices_of_peaks, List<int> indices_of_troughs)
        {
            Dictionary<int, PeakTrough> indices = new Dictionary<int, PeakTrough>();
            for (int i = 0; i < candlesticks.Length; i++)
            {
                if (indices_of_peaks.Contains(i))
                    indices.Add(i, PeakTrough.Peak);
                if (indices_of_troughs.Contains(i))
                    indices.Add(i, PeakTrough.Trough);
            }

            bool isPeakTurn = false;
            if (indices.Last().Value == PeakTrough.Peak)
                isPeakTurn = true;

            for (int i = indices.Count - 1; i >= 0; i--)
            {
                if (isPeakTurn)
                {
                    if (indices.ElementAt(i).Value == PeakTrough.Trough)
                    {
                        if (candlesticks[indices.ElementAt(i).Key].LowPrice <= candlesticks[indices.ElementAt(i + 1).Key].LowPrice)
                            indices.Remove(indices.ElementAt(i + 1).Key);
                        else
                            indices.Remove(indices.ElementAt(i).Key);
                    }
                    else
                    {
                        isPeakTurn = false;
                    }
                }
                else
                {
                    if (indices.ElementAt(i).Value == PeakTrough.Peak)
                    {
                        if (candlesticks[indices.ElementAt(i).Key].HighPrice >= candlesticks[indices.ElementAt(i + 1).Key].HighPrice)
                            indices.Remove(indices.ElementAt(i + 1).Key);
                        else
                            indices.Remove(indices.ElementAt(i).Key);
                    }
                    else
                    { 
                        isPeakTurn = true;
                    }
                }
            }
            return indices;
        }
        
        public enum Type {Long, Short, Consolidation}; //{多頭,空頭,盤整};
        public Type TrendType(Dictionary<int,PeakTrough> peak_trough, Candlestick[] candlesticks, int candlesticks_index = -1)
        {
            if (peak_trough.Count < 4)
            {
                throw new Exception("Data is not enough to determine the trend.");
            }
            if (candlesticks_index != -1)
            {
                throw new Exception("Unfinished development, user cannot set the value of 'candlesticks_index'.");
            }
            decimal last_high, last_second_high,
                    last_low, last_second_low;
            if (candlesticks_index == -1)
            {
                candlesticks_index = peak_trough.Count - 1;
            }
            var high_low = GetLastHighAndLow(candlesticks, peak_trough, candlesticks_index);
            last_high = high_low.Item1;
            last_second_high = high_low.Item2;
            last_low = high_low.Item3;
            last_second_low = high_low.Item4;

            if (last_high > last_second_high && last_low > last_second_low)
                return Type.Long;
            else if (last_high < last_second_high && last_low < last_second_low)
                return Type.Short;
            else
                return Type.Consolidation;
        }

        public (decimal,decimal,decimal,decimal) GetLastHighAndLow(Candlestick[] candlesticks, Dictionary<int, PeakTrough> peak_trough, int candlestick_index)
        {
            decimal last_high, last_second_high,
                    last_low, last_second_low;
            if (peak_trough.ElementAt(candlestick_index).Value == PeakTrough.Peak)
            {
                last_high = candlesticks[peak_trough.ElementAt(candlestick_index).Key].HighPrice;
                last_second_high = candlesticks[peak_trough.ElementAt(candlestick_index-2).Key].HighPrice;
                last_low = candlesticks[peak_trough.ElementAt(candlestick_index-1).Key].LowPrice;
                last_second_low = candlesticks[peak_trough.ElementAt(candlestick_index-3).Key].LowPrice;
            }
            else
            {
                last_high = candlesticks[peak_trough.ElementAt(candlestick_index-1).Key].HighPrice;
                last_second_high = candlesticks[peak_trough.ElementAt(candlestick_index-3).Key].HighPrice;
                last_low = candlesticks[peak_trough.ElementAt(candlestick_index).Key].LowPrice;
                last_second_low = candlesticks[peak_trough.ElementAt(candlestick_index-2).Key].LowPrice;
            }
            return (last_high, last_second_high, last_low, last_second_low);
        }
    }
}

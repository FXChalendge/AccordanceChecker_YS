using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Engine
{
    /// <summary>
    /// 
    /// </summary>
    public static class ListMathExtentions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double Mean(this List<Delta> values)
        {
            return values.Count == 0 ? 0 : values.Mean(0, values.Count);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static double Mean(this List<Delta> values, int start, int end)
        {
            double s = 0;

            for (int i = start; i < end; i++)
            {
                s += values.ElementAt(i).Value.Value;
            }

            return s / (end - start);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double Variance(this List<Delta> values)
        {
            return values.Variance(values.Mean(), 0, values.Count);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="mean"></param>
        /// <returns></returns>
        public static double Variance(this List<Delta> values, double mean)
        {
            return values.Variance(mean, 0, values.Count);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="mean"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static double Variance(this List<Delta> values, double mean, int start, int end)
        {
            double variance = 0;

            for (int i = start; i < end; i++)
            {
                variance += Math.Pow((values.ElementAt(i).Value.Value - mean), 2);
            }

            int n = end - start;
            if (start > 0)
                n--;

            return variance / (n);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double StandardDeviation(this List<Delta> values)
        {
            return values.Count == 0 ? 0 : values.StandardDeviation(0, values.Count);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static double StandardDeviation(this List<Delta> values, int start, int end)
        {
            double mean = values.Mean(start, end);
            double variance = values.Variance(mean, start, end);

            return Math.Sqrt(variance);
        }
    }
}

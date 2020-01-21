using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine
{
    /// <summary>
    /// 
    /// </summary>
    public class Agent
    {
        #region | Classes |

        #endregion | Classes |

        #region | Fields |
        private readonly double _widthLimit;
        private RecordsNodesCircularList _base = null;
        private RecordsNodesCircularList _second = null;
        #endregion | Fields |

        #region | Properties |
        /// <summary>
        /// 
        /// </summary>
        public bool IsCorrelated { get; private set; }
        #endregion | Properties |

        #region | Constructors |
        /// <summary>
        /// 
        /// </summary>
        /// <param name="widthLimit"></param>
        public Agent(double widthLimit)
        {
            _widthLimit = widthLimit;
        }
        #endregion | Constructors |

        #region | Methods |
        /// <summary>
        /// 
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public void Corellate(RecordsNodesCircularList first, RecordsNodesCircularList second)
        {
            if (first == null || second == null)
            {
                throw new ArgumentNullException(first == null ? "first" : "second");
            }

            if (first.Count != second.Count)
                throw new ArgumentException($"Different members quantity for {nameof(first)}({first.Count}) and {nameof(second)}({second.Count})");

            _base = first;
            _second = second;

            try
            {
                IsCorrelated = _base.Adjust(_second);
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public string Corellate(string first, string second)
        {
            string res = null;

            RecordsNodesCircularList rnclFirst = RecordsNodesCircularList.Deserialize(_widthLimit, first);
            RecordsNodesCircularList rnclSecond = RecordsNodesCircularList.Deserialize(_widthLimit, second);

            Corellate(rnclFirst, rnclSecond);

            if(rnclSecond.IsAdjusted)
            {
                RecordsNodesCircularList.Serialize(first, rnclFirst, rnclSecond);
                res = first;
            }

            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="percentile">desiered percental rank (in range 0...1)</param>
        /// <remarks>Set <cref="percentile"/> with less to <c="0"/> to elliminate selection by percentile rank.</remarks>
        private double Accordance(RecordsNodesCircularList first, RecordsNodesCircularList second, double percentile)
        {
            double res = 0;

            if (!IsCorrelated)
                throw new Exception("Accodance terminated! Launch 'Correlate(...)' method first and then try again. CAUSE: Records collactions are NOT correlated yet.");

            RecordNode itrtrFirst = first.Head;
            RecordNode itrtrSecond = second.Head;

            Delta delta = null;
            List<Delta> deltas = null;
            List<Delta> deltasSorted = null;

            do
            {
                delta = new Delta(itrtrFirst, itrtrSecond);

                if (deltas == null)
                    deltas = new List<Delta>();
                deltas.Add(delta);

                if (deltasSorted == null)
                    deltasSorted = new List<Delta>();
                deltasSorted.Add(delta);

                itrtrFirst = itrtrFirst.Next;
                itrtrSecond = itrtrSecond.Next;
            }
            while (itrtrFirst != first.Head);

            if (PercentRank(deltasSorted))
            {
                int endIndx = deltasSorted.Count;

                if (percentile >= 0)
                {
                    int i = 0;
                    for (; i < endIndx && deltasSorted.ElementAt(i).PercentRank.HasValue && deltasSorted.ElementAt(i).PercentRank.Value < percentile; i++)
                    { }
                    endIndx = i;
                }
                res = deltasSorted.StandardDeviation(0, endIndx);
            }
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="percentile"></param>
        /// <returns></returns>
        public double Accordance(double percentile)
        {
            double res = 0;

            res = Accordance(_base, _second, percentile);

            return res;
        }

        private bool PercentRank(List<Delta> deltasCollection)
        {
            bool res = false;

            if (deltasCollection == null)
                throw new ArgumentNullException(nameof(deltasCollection));

            deltasCollection.Sort();

            for (int i = 0; i < deltasCollection.Count; i++)
            {
                double? dlta = deltasCollection.ElementAt(i).Value;
                double? prcntRnk = deltasCollection.ElementAt(i).PercentRank;
                if (dlta.HasValue && !prcntRnk.HasValue)
                {
                    deltasCollection.ElementAt(i).PercentRank = PercentRank(deltasCollection, dlta.Value);
                }

                res = true;
            }

            return res;
        }

        private double PercentRank(Delta delta, List<Delta> deltasCollection)
        {
            return delta.PercentRank != null ? delta.PercentRank.Value : PercentRank(deltasCollection, delta.Value.Value);
        }

        private double PercentRank(List<Delta> deltasCollection, double value)
        {
            double? res = null;

            if (deltasCollection == null)
                throw new ArgumentNullException(nameof(deltasCollection));

            deltasCollection.Sort();

            for (int i = 0; i < deltasCollection.Count && res == null; i++)
            {
                double? dlta = deltasCollection.ElementAt(i).Value;
                if (dlta.HasValue)
                {
                    if (dlta.Value == value)
                        res = ((double)i) / (deltasCollection.Count - 1);
                    else if (i < deltasCollection.Count - 1)
                    {
                        double? dltaNxt = deltasCollection.ElementAt(i + 1).Value;
                        if (dltaNxt != null && dlta.Value < value && value < dltaNxt.Value)
                        {
                            double valSmaller = dlta.Value;
                            double valBigger = dltaNxt.Value;

                            double prcntRankOfSmaller = PercentRank(deltasCollection.ElementAt(i), deltasCollection);
                            double prcntRankOfBigger = PercentRank(deltasCollection.ElementAt(i + 1), deltasCollection);

                            res = (((valBigger - value) * prcntRankOfSmaller + (value - valSmaller) * prcntRankOfBigger)) / (valBigger - valSmaller);
                        }
                    }
                }
            }

            //// calculate value using linear interpolation
            //double x1, x2, y1, y2;

            //for (int i = 0; i < deltasCollection.Count - 1; i++)
            //{
            //    if (deltasCollection[i] < value && value < deltasCollection[i + 1])
            //    {
            //        x1 = deltasCollection[i];
            //        x2 = deltasCollection[i + 1];
            //        y1 = PercentRank(deltasCollection, x1);
            //        y2 = PercentRank(deltasCollection, x2);

            //        return (((x2 - value) * y1 + (value - x1) * y2)) / (x2 - x1);
            //    }
            //}

            //throw new Exception("Out of bounds");

            return res ?? 0;
        }

        #endregion | Methods |
    }
}

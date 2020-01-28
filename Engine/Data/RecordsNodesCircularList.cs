using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Engine
{
    public class RecordsNodesCircularList<TPosition>
        : IList<Record<TPosition>>
        , INormalizable
        , IAdjustable<RecordsNodesCircularList<TPosition>>
        , IDisposable
    {
        #region | Fields |
        private double _validWidthsSum = 0;
        #endregion | Fields |

        #region | Properties |
        /// <summary>
        /// 
        /// </summary>
        public RecordNode<Record<TPosition>> Head { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public RecordNode<Record<TPosition>> Tail { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public int ValidRecsCount { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// 
        /// </summary>
        public bool IsNormalized { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsAdjusted { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        Record<TPosition> IList<Record<TPosition>>.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        #endregion | Properties |

        #region | Methods |

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static RecordsNodesCircularList<TPosition> Deserialize(double widthLimit, string filePath)
        {
            RecordsNodesCircularList<TPosition> res = null;

            if (!string.IsNullOrEmpty(filePath))
            {
                var lines = File.ReadLines(filePath);
                foreach (var line in lines)
                {
                    Record<TPosition> r = Record<TPosition>.Deserialize(widthLimit, line);
                    if (r != null)
                    {
                        if (res == null)
                            res = new RecordsNodesCircularList<TPosition>();
                        res.Add(r);
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destFilePath"></param>
        /// <param name="rnclFirst"></param>
        /// <param name="rnclSecond"></param>
        /// <returns></returns>
        public static string Serialize(string destFilePath, RecordsNodesCircularList<TPosition> rnclFirst, RecordsNodesCircularList<TPosition> rnclSecond)
        {
            string res = null;

            if (string.IsNullOrEmpty(destFilePath) || rnclFirst == null || rnclSecond == null)
                throw new ArgumentNullException("Serialize(string destFilePath, RecordsNodesCircularList rnclFirst, RecordsNodesCircularList rnclSecond)");

            string[] newLines = Serialize(rnclFirst, rnclSecond);

            if (newLines.Length > 0)
            {
                File.WriteAllLines(destFilePath, newLines);
                res = destFilePath;
            }
            return res;
        }

        private static string[] Serialize(RecordsNodesCircularList<TPosition> rnclFirst, RecordsNodesCircularList<TPosition> rnclSecond)
        {
            string[] res = null;
            if ( rnclFirst == null || rnclSecond == null)
                throw new ArgumentNullException(rnclFirst==null ? nameof(rnclFirst) : nameof(rnclSecond) + " is Empty");

            List<string> lines = new List<string>();

            RecordNode<Record<TPosition>> iteratorFirst = rnclFirst.Head;
            RecordNode<Record<TPosition>> iteratorSecond = rnclSecond.Head;

            lines.Add("Position,FirstRun,SecondRun");

            do
            {
                lines.Add($"{iteratorFirst.Data.Serialize()},{iteratorSecond.Data.Width}");

                iteratorFirst = iteratorFirst.Next;
                iteratorSecond = iteratorSecond.Next;
            }
            while (iteratorFirst != rnclFirst.Head);

            if (lines != null && lines.Count == rnclFirst.Count)
                res = lines.ToArray();

            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Add(Record<TPosition> item)
        {
            if (item != null)
            {
                RecordNode<Record<TPosition>> rn = new RecordNode<Record<TPosition>> { Data = item };
                if (Head == null)
                {
                    Head = rn;
                    Tail = Head;                    
                    Head.Next = Tail;
                    Tail.Next = Head;
                    Tail.Previous = Head;
                }
                else
                {
                    rn.Previous = Tail;
                    rn.Next = Tail.Next;
                    Tail.Next = rn;
                    Tail = rn;
                }
                Head.Previous = Tail;

                Count++;

                if (item.IsValid)
                {
                    _validWidthsSum += item.Width;
                    ValidRecsCount++;
                }

                IsNormalized = false;
                IsAdjusted = false;
            }
        }

        public void Clear()
        {
            while(Head?.Next!=null)
            {
                if (Tail.Next != null)
                {
                    Tail.Next = null;
                    Head.Previous = null;
                }
                RecordNode<Record<TPosition>> rn = Head.Next;
                rn.Previous = null;
                Head.Next = null;
                _validWidthsSum -= Head.Data.Width;
                Head = null;
                Head = rn;
            }
            Tail = null;
            _validWidthsSum = 0;
            IsNormalized = false;
            IsAdjusted = false;
            Count = 0;
            ValidRecsCount = 0;
            Head = null;
        }

        public bool Contains(Record<TPosition> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Record<TPosition>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<RecordNode<Record<TPosition>>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public int IndexOf(Record<TPosition> item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, Record<TPosition> item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(Record<TPosition> item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator<Record<TPosition>> IEnumerable<Record<TPosition>>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public INormalizable Normalize()
        {
            double widthAvg = _validWidthsSum / ValidRecsCount;
            double normalizedWidthSum = 0;
            RecordNode<Record<TPosition>> iterator = Head;

            do
            {
                iterator.Data.Normalize(widthAvg);

                if(iterator.Data.Normalized.HasValue)
                    normalizedWidthSum += iterator.Data.Normalized.Value;

                iterator = iterator.Next;
            }
            while (iterator != Head);

            if (Math.Abs(normalizedWidthSum) > 0.01)
                throw new ArgumentException($"Normalization failed! CAUSE: Normilized Widthes Sum is too big ({normalizedWidthSum} but should be 0)");

            IsNormalized = true;
            IsAdjusted = false;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="forAdjutment"></param>
        /// <returns>True if <cref="forAdjutment"/> adjusted with this list, otherwise - false;</returns>
        public bool Adjust(RecordsNodesCircularList<TPosition> forAdjutment)
        {
            bool res = false;
            IDictionary<RecordNode<Record<TPosition>>, Tuple<double?, int>> productsSums = null;
            RecordNode<Record<TPosition>> headAdj = null;

            Tuple<double?, int> maxProductsSum = null;
            RecordNode<Record<TPosition>> maxAdj = null;

            if (!IsNormalized)
                Normalize();

            if (IsNormalized && forAdjutment != null)
            {
                if (!forAdjutment.IsNormalized)
                    forAdjutment.Normalize();

                if (forAdjutment.IsNormalized)
                {
                    headAdj = forAdjutment.Head;

                    productsSums = new Dictionary<RecordNode<Record<TPosition>>, Tuple<double?, int>>();

                    do
                    {
                        int countWasSum; //count of records were summed, due to presence of the invalid records that disqualify the record in both collections;
                        double? productsSum = GetProductsSum(headAdj, out countWasSum);
                        if (productsSum != null)
                        {
                            productsSums[headAdj] = new Tuple<double?, int>(productsSum, countWasSum);

                            if (maxProductsSum == null || productsSum > maxProductsSum.Item1)
                            {
                                maxProductsSum = productsSums[headAdj];
                                maxAdj = headAdj;
                            }
                        }
                        headAdj = headAdj.Next;
                    }
                    while (headAdj != forAdjutment.Head);
                }

                res = maxProductsSum != null && productsSums.Count == Count;

                if (res)
                    forAdjutment.Shift(maxAdj);               
            }

            return res;
        }

        private RecordsNodesCircularList<TPosition> Shift(RecordNode<Record<TPosition>> maxAdj)
        {
            Head = maxAdj;
            Tail = maxAdj.Previous;

            IsAdjusted = true;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headAdj"></param>
        /// <param name="countWasSum">The count of records were summed, due to presence of the invalid records that disqualify the record in both collections;</param>
        /// <returns></returns>
        private double? GetProductsSum(RecordNode<Record<TPosition>> headAdj, out int countWasSum)
        {
            countWasSum = -1;
            double? res = null;

            if (Head != null && headAdj != null)
            {
                RecordNode<Record<TPosition>> iterator = Head;
                RecordNode<Record<TPosition>> iteratorAdj = headAdj;

                do
                {
                    double? product = iterator.GetProduct(iteratorAdj);

                    if (product.HasValue)
                    {
                        if (res == null)
                            res = product;
                        else
                            res += product;

                        if (countWasSum < 0)
                            countWasSum = 0;
                        countWasSum++;
                    }

                    iterator = iterator.Next;
                    iteratorAdj = iteratorAdj.Next;
                }
                while (iterator != Head);
            }
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Clear();
        }
        #endregion | Methods |
    }
}

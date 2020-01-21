using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Engine
{
    public class RecordsNodesCircularList
        : IList<Record>
        , INormalizable
        , IAdjustable<RecordsNodesCircularList>
        , IDisposable
    {
        #region | Fields |
        private double _widthsSum = 0;
        #endregion | Fields |

        #region | Properties |
        /// <summary>
        /// 
        /// </summary>
        public RecordNode Head { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public RecordNode Tail { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Count { get; private set; }

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
        Record IList<Record>.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        #endregion | Properties |

        #region | Methods |

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static RecordsNodesCircularList Deserialize(string filePath)
        {
            RecordsNodesCircularList res = null;

            if (!string.IsNullOrEmpty(filePath))
            {
                var lines = File.ReadLines(filePath);
                foreach (var line in lines)
                {
                    Record r = Record.Deserialize(line);
                    if (r != null)
                    {
                        if (res == null)
                            res = new RecordsNodesCircularList();
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
        public static string Serialize(string destFilePath, RecordsNodesCircularList rnclFirst, RecordsNodesCircularList rnclSecond)
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

        private static string[] Serialize(RecordsNodesCircularList rnclFirst, RecordsNodesCircularList rnclSecond)
        {
            string[] res = null;
            if ( rnclFirst == null || rnclSecond == null)
                throw new ArgumentNullException(rnclFirst==null ? nameof(rnclFirst) : nameof(rnclSecond) + " is Empty");

            List<string> lines = new List<string>();

            RecordNode iteratorFirst = rnclFirst.Head;
            RecordNode iteratorSecond = rnclSecond.Head;

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
        public void Add(Record item)
        {
            if (item != null)
            {
                RecordNode rn = new RecordNode { Data = item };
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
                _widthsSum += item.Width;
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
                RecordNode rn = Head.Next;
                rn.Previous = null;
                Head.Next = null;
                _widthsSum -= Head.Data.Width;
                Head = null;
                Head = rn;
            }
            Tail = null;
            _widthsSum = 0;
            IsNormalized = false;
            IsAdjusted = false;
        }

        public bool Contains(Record item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Record[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<RecordNode> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public int IndexOf(Record item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, Record item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(Record item)
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

        IEnumerator<Record> IEnumerable<Record>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public INormalizable Normalize()
        {
            double widthAvg = _widthsSum / Count;
            double normalizedWidthSum = 0;
            RecordNode iterator = Head;

            do
            {
                iterator.Data.Normalize(widthAvg);

                normalizedWidthSum += iterator.Data.Normalized;

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
        public bool Adjust(RecordsNodesCircularList forAdjutment)
        {
            bool res = false;
            IDictionary<RecordNode, double?> productsSums = null;
            RecordNode headAdj = null;

            double? maxProductsSum = null;
            RecordNode maxAdj = null;

            if (!IsNormalized)
                Normalize();

            if (IsNormalized && forAdjutment != null)
            {
                if (!forAdjutment.IsNormalized)
                    forAdjutment.Normalize();

                if (forAdjutment.IsNormalized)
                {
                    headAdj = forAdjutment.Head;

                    productsSums = new Dictionary<RecordNode, double?>();

                    do
                    {
                        double? productsSum = GetProductsSum(headAdj);
                        if (productsSum != null)
                        {
                            productsSums[headAdj] = productsSum;

                            if (maxProductsSum == null || productsSum > maxProductsSum)
                            {
                                maxProductsSum = productsSum;
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

        private RecordsNodesCircularList Shift(RecordNode maxAdj)
        {
            Head = maxAdj;
            Tail = maxAdj.Previous;

            IsAdjusted = true;

            return this;
        }

        private double? GetProductsSum(RecordNode headAdj)
        {
            double? res = null;

            if (Head != null && headAdj != null)
            {
                RecordNode iterator = Head;
                RecordNode iteratorAdj = headAdj;

                do
                {
                    double? product = iterator.GetProduct(iteratorAdj);

                    if (res == null)
                        res = product;
                    else
                        res += product;

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

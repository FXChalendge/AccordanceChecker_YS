using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class Delta
        : IComparer<Delta>
        , IComparable<Delta>
    {
        #region | Fields |
        private double? _delta = null;
        #endregion | Fields |

        #region | Properties |
        /// <summary>
        /// 
        /// </summary>
        public RecordNode First
        {
            get;
            private set;
        }
        /// <summary>
        /// 
        /// </summary>
        public RecordNode Second
        {
            get;
            private set;
        }
        /// <summary>
        /// 
        /// </summary>
        public double? Value
        {
            get
            {
                if (_delta == null && First?.Data != null && Second?.Data != null && First.Data.IsValid && Second.Data.IsValid)
                    _delta = First.Data.Width - Second.Data.Width;
                return _delta;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public double? PercentRank { get; set; }
        #endregion | Properties |

        #region | Constructors |
        /// <summary>
        /// 
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        public Delta(RecordNode first, RecordNode second)
        {
            First = first;
            Second = second;
        }
        #endregion | Constructors |

        #region | Methods |
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(Delta x, Delta y)
        {
            return x.CompareTo(y);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Delta other)
        {
            int res = 0;
            if (Value != null && other?.Value != null)
            {
                double diff = Value.Value - other.Value.Value;
                if (diff < 0)
                    res = -1;
                else if (diff > 0)
                    res = 1;
            }
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nameof(First)}={First};{nameof(Second)}={Second};{nameof(Value)}={Value};{nameof(PercentRank)}={PercentRank}";
        }
        #endregion | Methods |
    }
}

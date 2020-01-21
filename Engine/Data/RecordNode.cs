using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class RecordNode
    {
        #region | Properties |
        /// <summary>
        /// 
        /// </summary>
        public Record Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public RecordNode Previous { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public RecordNode Next { get; set; }
        #endregion | Properties |

        #region | Methods |
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nameof(Previous)}:{Previous!=null}<-{nameof(Data)}:{Data}->{nameof(Next)}:{Next!=null}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public double? GetProduct(RecordNode other)
        {
            return !Data.IsValid || other == null || !other.Data.IsValid ? (double?)null : Data.Normalized * other.Data.Normalized;
        }
        #endregion | Methods |
    }
}

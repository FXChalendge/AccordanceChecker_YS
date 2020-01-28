using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class RecordNode<T>
    {
        #region | Properties |
        /// <summary>
        /// 
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public RecordNode<T> Previous { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public RecordNode<T> Next { get; set; }
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
        public double? GetProduct(RecordNode<T> other)
        {
            return (Data as IProduct<T>)?.GetProduct(other.Data);
        }
        #endregion | Methods |
    }
}

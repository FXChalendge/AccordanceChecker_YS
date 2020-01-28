using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public interface IAdjustable<T>// where T : RecordsNodesCircularList<TPosition>
    {
        #region | Properties |
        bool IsAdjusted { get; }
        #endregion | Properties |

        #region | Methods |
        /// <summary>
        /// 
        /// </summary>
        /// <param name="forAdjutment"></param>
        /// <returns>True if <cref="forAdjutment"/> adjusted with this list, otherwise - false;</returns>
        bool Adjust(T forAdjutment);
        #endregion | Methods |
    }
}

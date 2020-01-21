using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface INormalized<T>
        where T : struct
    {
        #region | Properties |
        T? Normalized { get; }
        #endregion | Properties |

        #region | Methods |
        INormalized<T> Normalize(T avg);
        #endregion | Methods |
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public interface INormalizable
    {
        #region | Properties |
        bool IsNormalized { get; }
        #endregion | Properties |

        #region | Methods |
        INormalizable Normalize();
        #endregion | Methods |
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public interface IProduct<T>
    {
        #region | Methods |
        double? GetProduct(T other);
        #endregion | Methods |
    }
}

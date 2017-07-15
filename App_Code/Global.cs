using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Normalization
{
    /// <summary>
    /// Summary description for Global
    /// </summary>
    [Serializable]
    public static class Global
    {
        public static AttrComparer comparer = new AttrComparer();
    }
}
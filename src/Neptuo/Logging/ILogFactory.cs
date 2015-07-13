﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Logging
{
    /// <summary>
    /// Factory for child scopes.
    /// </summary>
    public interface ILogFactory
    {
        /// <summary>
        /// Creates child log with <paramref name="scopeName"/> in it's scope path.
        /// </summary>
        /// <param name="scopeName"></param>
        /// <returns></returns>
        ILog Scope(string scopeName);
    }
}

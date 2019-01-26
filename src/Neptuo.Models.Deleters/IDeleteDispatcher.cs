﻿using Neptuo.Models.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Models.Deleters
{
    /// <summary>
    /// Provides ability to delete object.
    /// </summary>
    public interface IDeleteDispatcher
    {
        /// <summary>
        /// Prepares context for deleting object identifier by <paramref name="key"/>.
        /// </summary>
        /// <param name="key">A key of the object to delete.</param>
        /// <returns>A continuation task contaning a deletion context.</returns>
        Task<IDeleteContext> PrepareContextAsync(IKey key);
    }
}

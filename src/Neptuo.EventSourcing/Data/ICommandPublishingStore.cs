﻿using Neptuo.Models.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Data
{
    /// <summary>
    /// Describes the underlaying store for the persistent command delivery.
    /// </summary>
    public interface ICommandPublishingStore
    {
        /// <summary>
        /// Returns the enumeration of the unpublished commands.
        /// </summary>
        /// <returns>The enumeration of the unpublished commands.</returns>
        Task<IEnumerable<CommandModel>> GetAsync();

        /// <summary>
        /// Saves information about publishing <paramref name="commandKey"/>.
        /// </summary>
        /// <param name="eventKey">The key of the published command.</param>
        /// <returns>The continuation task.</returns>
        Task PublishedAsync(IKey commandKey);

        /// <summary>
        /// Clears the queue of the unpublished commands.
        /// </summary>
        /// <returns>The continuation task.</returns>
        Task ClearAsync();
    }
}

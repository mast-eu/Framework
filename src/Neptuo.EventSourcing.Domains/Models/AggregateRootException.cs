﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Models
{
    /// <summary>
    /// Base exception for aggregate root errors.
    /// </summary>
    [Serializable]
    public class AggregateRootException : Exception
    {
        /// <summary>
        /// Creates new empty instance.
        /// </summary>
        public AggregateRootException() 
        { }

        /// <summary>
        /// Creates new instance with text <paramref name="message"/>.
        /// </summary>
        /// <param name="message">The text description.</param>
        public AggregateRootException(string message) 
            : base(message) 
        { }

        /// <summary>
        /// Create new instance with text <paramref name="message"/> and <paramref name="inner"/> exception.
        /// </summary>
        /// <param name="message">The text description.</param>
        /// <param name="inner">The inner exception.</param>
        public AggregateRootException(string message, Exception inner) 
            : base(message, inner) 
        { }

        /// <summary>
        /// Serialization constructor.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The serialization context.</param>
        protected AggregateRootException(SerializationInfo info, StreamingContext context)
            : base(info, context) 
        { }
    }
}
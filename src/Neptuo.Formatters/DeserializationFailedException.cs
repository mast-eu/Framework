﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Formatters
{
    /// <summary>
    /// Raised when serialization of the input failed.
    /// </summary>
    public class DeserializationFailedException : FormatterException
    {
        /// <summary>
        /// Gets a type of the output.
        /// </summary>
        public Type OutputType { get; private set; }

        /// <summary>
        /// Gets a serialized input.
        /// </summary>
        public string Input { get; private set; }

        /// <summary>
        /// Creates a new instance for failed deserialization of <paramref name="outputType"/>.
        /// </summary>
        /// <param name="outputType">A type of the output.</param>
        /// <param name="input">A serialized input.</param>
        public DeserializationFailedException(Type outputType, string input)
            : base(String.Format("Deserialization of the '{0}' from '{1}' failed.", outputType.AssemblyQualifiedName, input))
        {
            OutputType = outputType;
            Input = input;
        }

        /// <summary>
        /// Creates a new instance for failed deserialization of <paramref name="outputType"/>.
        /// </summary>
        /// <param name="outputType">A type of the output.</param>
        /// <param name="input">A serialized input.</param>
        /// <param name="inner">An exception cause.</param>
        public DeserializationFailedException(Type outputType, string input, Exception inner)
            : base(String.Format("Deserialization of the '{0}' from '{1}' failed.", outputType.AssemblyQualifiedName, input), inner)
        {
            OutputType = outputType;
            Input = input;
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="info">A serialization info.</param>
        /// <param name="context">A streaming context.</param>
        protected DeserializationFailedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}

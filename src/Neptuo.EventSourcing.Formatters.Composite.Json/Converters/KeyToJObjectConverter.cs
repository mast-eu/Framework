﻿using Neptuo.Models.Keys;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Formatters.Converters
{
    /// <summary>
    /// The base class for converting keys to and from <see cref="JObject"/>.
    /// </summary>
    /// <typeparam name="T">The type of the key.</typeparam>
    public class KeyToJObjectConverter<T> : TwoWayConverter<T, JToken>
        where T : IKey
    {
        public override bool TryConvert(T sourceValue, out JToken targetValue)
        {
            if (sourceValue == null)
            {
                targetValue = null;
                return false;
            }

            JObject target;
            if (TryConvert(sourceValue, out target))
            {
                targetValue = target;
                return true;
            }

            targetValue = null;
            return false;
        }

        protected abstract bool TryConvert(T source, out JObject target);

        public override bool TryConvert(JToken sourceValue, out T targetValue)
        {
            JObject source = sourceValue as JObject;
            if (source == null)
            {
                targetValue = default(T);
                return false;
            }

            return TryConvert(source, out targetValue);
        }

        protected abstract bool TryConvert(JObject source, out T target);
    }
}

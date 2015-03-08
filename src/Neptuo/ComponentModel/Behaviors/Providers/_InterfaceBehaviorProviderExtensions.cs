﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.ComponentModel.Behaviors.Providers
{
    public static class _InterfaceBehaviorProviderExtensions
    {
        /// <summary>
        /// Adds mapping with <typeparamref name="TBehaviorContract"/> as contract and <typeparamref name="TBehaviorImplementation"/> as implementation type.
        /// </summary>
        /// <typeparam name="TBehaviorContract">Behavior interface contract.</typeparam>
        /// <typeparam name="TBehaviorImplementation">Behavior contract implementor.</typeparam>
        /// <returns>Self (for fluency).</returns>>
        public static InterfaceBehaviorProvider AddMapping<TBehaviorContract, TBehaviorImplementation>(this InterfaceBehaviorProvider provider)
            where TBehaviorImplementation : IBehavior<TBehaviorContract>
        {
            Guard.NotNull(provider, "provider");
            return provider.AddMapping(typeof(TBehaviorContract), typeof(TBehaviorImplementation));
        }
    }
}
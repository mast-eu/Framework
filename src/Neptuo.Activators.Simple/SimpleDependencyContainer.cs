﻿using Neptuo.Activators.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Activators
{
    /// <summary>
    /// Base reflection (and #kit compliant) implementation of <see cref="IDependencyContainer"/>.
    /// </summary>
    public class SimpleDependencyContainer : DisposableBase, IDependencyContainer, IFactory<IDependencyContainer, string>
    {
        private readonly SimpleDependencyContainer parentContainer;
        private readonly string scopeName;
        private readonly DependencyDefinitionCollection definitions;
        private readonly InstanceResolver resolver;

        /// <summary>
        /// Creates new intance.
        /// </summary>
        public SimpleDependencyContainer()
            : this(DependencyLifetime.RootScopeName, null)
        { }

        private SimpleDependencyContainer(string scopeName, SimpleDependencyContainer parentContainer)
        {
            Ensure.NotNullOrEmpty(scopeName, "scopeName");
            this.scopeName = scopeName;
            this.parentContainer = parentContainer;

            InstanceStorage instances = new InstanceStorage();

            if (parentContainer == null)
                this.definitions = new DependencyDefinitionCollection(scopeName, instances);
            else
                this.definitions = new DependencyDefinitionCollection(scopeName, instances, parentContainer.definitions);

            this.resolver = new InstanceResolver(definitions, instances);

            definitions.Add(typeof(IFactory<IDependencyContainer, string>), DependencyLifetime.NameScope(scopeName), this);
            definitions.Add(typeof(IDependencyContainer), DependencyLifetime.NameScope(scopeName), this);
            definitions.Add(typeof(IDependencyProvider), DependencyLifetime.NameScope(scopeName), this);
        }

        #region IDependencyContainer

        public IDependencyDefinitionCollection Definitions
        {
            get { return definitions; }
        }

        #endregion

        #region IDependencyProvider

        public string ScopeName
        {
            get { return scopeName; }
        }
        
        IDependencyDefinitionReadOnlyCollection IDependencyProvider.Definitions
        {
            get { return definitions; }
        }

        public object Resolve(Type requiredType)
        {
            return resolver.Resolve(requiredType);
        }

        #endregion

        #region IFactory<IDependencyContainer, string>

        public IDependencyContainer Create(string scopeName)
        {
            return new SimpleDependencyContainer(scopeName, this);
        }

        #endregion
    }
}

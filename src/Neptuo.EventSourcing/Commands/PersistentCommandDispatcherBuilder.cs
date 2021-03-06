﻿using Neptuo;
using Neptuo.Data;
using Neptuo.Exceptions;
using Neptuo.Formatters;
using Neptuo.Internals;
using Neptuo.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Commands
{
    /// <summary>
    /// The factory for <see cref="PersistentCommandDispatcher"/> that shares threading and paralelism.
    /// </summary>
    public class PersistentCommandDispatcherBuilder
    {
        private TreeQueue queue;
        private TreeQueueThreadPool threadPool;

        private ICommandDistributor distributor;
        private ICommandPublishingStore store;
        private ISerializer formatter;
        private ISchedulingProvider schedulingProvider;

        private IExceptionHandlerCollection commandExceptionHandlers;
        private IExceptionHandlerCollection dispatcherExceptionHandlers;

        private ILogFactory logFactory;

        private void EnsureInternals()
        {
            if (queue == null)
                queue = new TreeQueue();

            if (threadPool == null)
                threadPool = new TreeQueueThreadPool(queue);

            if (logFactory == null)
                logFactory = new DefaultLogFactory();
        }

        /// <summary>
        /// Sets <paramref name="distributor"/> to be used for distributing commands on all newly created dispatchers.
        /// </summary>
        /// <param name="distributor">The command-to-the-queue distributor.</param>
        /// <returns>Self (for fluency).</returns>
        public PersistentCommandDispatcherBuilder UseCommandDistributor(ICommandDistributor distributor)
        {
            Ensure.NotNull(distributor, "distributor");
            this.distributor = distributor;
            return this;
        }

        /// <summary>
        /// Sets <paramref name="store"/> to be used for persisting commands on all newly created dispatchers.
        /// </summary>
        /// <param name="store">The publishing store for command persistent delivery (can be <c>null</c> for reseting to default).</param>
        /// <returns>Self (for fluency).</returns>
        public PersistentCommandDispatcherBuilder UseStore(ICommandPublishingStore store)
        {
            this.store = store;
            return this;
        }

        /// <summary>
        /// Sets <paramref name="formatter"/> to be used for serializing commands on all newly created dispatchers.
        /// </summary>
        /// <param name="formatter">The formatter for serializing commands.</param>
        /// <returns>Self (for fluency).</returns>
        public PersistentCommandDispatcherBuilder UseFormatter(ISerializer formatter)
        {
            Ensure.NotNull(formatter, "formatter");
            this.formatter = formatter;
            return this;
        }

        /// <summary>
        /// Sets <paramref name="schedulingProvider"/> to be used for delaying commands on all newly created dispatchers.
        /// </summary>
        /// <param name="schedulingProvider">The provider of a delay computation for delayed commands (can be <c>null</c> for reseting to default).</param>
        /// <returns>Self (for fluency).</returns>
        public PersistentCommandDispatcherBuilder UseSchedulingProvider(ISchedulingProvider schedulingProvider)
        {
            this.schedulingProvider = schedulingProvider;
            return this;
        }

        /// <summary>
        /// Sets <paramref name="commandExceptionHandlers"/> to be used for handling command processing exceptions.
        /// </summary>
        /// <param name="commandExceptionHandlers">A collection of handlers for command processing exceptions.</param>
        /// <returns>Self (for fluency).</returns>
        public PersistentCommandDispatcherBuilder UseCommandExceptionHandlers(IExceptionHandlerCollection commandExceptionHandlers)
        {
            this.commandExceptionHandlers = commandExceptionHandlers;
            return this;
        }

        /// <summary>
        /// Sets <paramref name="dispatcherExceptionHandlers"/> to be used for handling infrastructure exceptions.
        /// </summary>
        /// <param name="dispatcherExceptionHandlers">A collection of handlers for infrastructure exceptions.</param>
        /// <returns>Self (for fluency).</returns>
        public PersistentCommandDispatcherBuilder UseDispatcherExceptionHandlers(IExceptionHandlerCollection dispatcherExceptionHandlers)
        {
            this.dispatcherExceptionHandlers = dispatcherExceptionHandlers;
            return this;
        }

        /// <summary>
        /// Sets <paramref name="logFactory"/> to be used for logging.
        /// </summary>
        /// <param name="logFactory">A log factory.</param>
        /// <returns>Self (for fluency).</returns>
        public PersistentCommandDispatcherBuilder UseLogFactory(ILogFactory logFactory)
        {
            this.logFactory = logFactory;
            return this;
        }

        /// <summary>
        /// Creates new instance of <see cref="PersistentCommandDispatcher"/> based on passed components.
        /// If some of required dependencies are missing, the exception is thrown.
        /// </summary>
        /// <returns>New instance of <see cref="PersistentCommandDispatcher"/>.</returns>
        public PersistentCommandDispatcher Create()
        {
            Ensure.NotNull(distributor, "distributor");
            Ensure.NotNull(formatter, "formatter");
            Ensure.NotNull(schedulingProvider, "schedulingProvider");
            EnsureInternals();

            PersistentCommandDispatcher dispatcher = new PersistentCommandDispatcher(
                queue,
                threadPool,
                distributor,
                store,
                formatter,
                schedulingProvider,
                logFactory
            );

            if (commandExceptionHandlers != null)
                dispatcher.CommandExceptionHandlers = commandExceptionHandlers;

            if (dispatcherExceptionHandlers != null)
                dispatcher.DispatcherExceptionHandlers = dispatcherExceptionHandlers;

            return dispatcher;
        }
    }
}

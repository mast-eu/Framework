﻿using Neptuo.Activators;
using Neptuo.Commands;
using Neptuo.Data;
using Neptuo.Events;
using Neptuo.Formatters;
using Neptuo.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Models.Repositories
{
    /// <summary>
    /// The implementation of EventSourcing ProcessRoot repository.
    /// </summary>
    /// <typeparam name="T">The type of the process root.</typeparam>
    public class ProcessRootRepository<T> : AggregateRootRepository<T>
        where T : ProcessRoot
    {
        private readonly ICommandDispatcher commandDispatcher;

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="store">The underlaying event store.</param>
        /// <param name="formatter">The formatter for serializing and deserializing event payloads.</param>
        /// <param name="factory">The process root factory.</param>
        /// <param name="eventDispatcher">The dispatcher for newly created events in the processes.</param>
        /// <param name="commandDispatcher">The dispatcher for newly created commands in the processes.</param>
        public ProcessRootRepository(IEventStore store, IFormatter formatter, IAggregateRootFactory<T> factory, IEventDispatcher eventDispatcher, ICommandDispatcher commandDispatcher)
            : base(store, formatter, factory, eventDispatcher)
        {
            Ensure.NotNull(commandDispatcher, "commandDispatcher");
            this.commandDispatcher = commandDispatcher;
        }

        public override void Save(T model)
        {
            base.Save(model);

            IEnumerable<Envelope<ICommand>> commands = model.Commands;
            if(commands.Any())
            {
                IEnumerable<Task> tasks = commands.Select(c => commandDispatcher.HandleAsync(c));
                Task.WaitAll(tasks.ToArray());
            }
        }
    }
}

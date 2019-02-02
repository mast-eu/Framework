﻿using Neptuo.Events;
using Neptuo.Events.Handlers;
using Neptuo.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Models.Domains
{
    /// <summary>
    /// Collection of event handlers bound from aggregate roots.
    /// </summary>
    internal class AggregateRootHandlerCollection
    {
        private static readonly string eventMethodName = "Handle";
        private static readonly string eventHandlerMethodName = TypeHelper.MethodName<IEventHandler<object>, object, Task>(h => h.HandleAsync);
        private readonly Dictionary<Type, Dictionary<Type, Action<object, object>>> storage = new Dictionary<Type, Dictionary<Type, Action<object, object>>>();

        public AggregateRootHandlerCollection Map(Type type)
        {
            Ensure.NotNull(type, "type");
            MapConventionMethods(type);
            MapInterfaceMethods(type);
            return this;
        }

        private void MapConventionMethods(Type type)
        {
            MethodInfo[] methodInfos = type.GetMethods(BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic);
            foreach (MethodInfo methodInfo in methodInfos)
            {
                if (methodInfo.Name == eventMethodName && methodInfo.ReturnType == typeof(void))
                {
                    ParameterInfo[] parameters = methodInfo.GetParameters();
                    if (parameters.Length == 1)
                    {
                        Dictionary<Type, Action<object, object>> typeHandlers;
                        if (!storage.TryGetValue(type, out typeHandlers))
                            storage[type] = typeHandlers = new Dictionary<Type, Action<object, object>>();

                        Type eventType = parameters[0].ParameterType;
                        typeHandlers[eventType] = (model, payload) => methodInfo.Invoke(model, new object[] { payload });
                    }
                }
            }
        }

        private void MapInterfaceMethods(Type type)
        {
            foreach (Type interfaceType in type.GetInterfaces())
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IEventHandler<>))
                {
                    Type eventType = interfaceType.GetGenericArguments()[0];

                    //MethodInfo eventMethod = type.GetMethod(eventHandlerMethodName, new Type[] { eventType });
                    // EndsWith is here because explicitly implemented methods have very complicated name.
                    MethodInfo eventMethod = type.GetInterfaceMap(interfaceType).TargetMethods
                        .FirstOrDefault(m => m.Name.EndsWith(eventHandlerMethodName));

                    Dictionary<Type, Action<object, object>> typeHandlers;
                    if (!storage.TryGetValue(type, out typeHandlers))
                        storage[type] = typeHandlers = new Dictionary<Type, Action<object, object>>();

                    typeHandlers[eventType] = (model, payload) => eventMethod.Invoke(model, new object[] { payload });
                }
            }
        }

        public bool Has(Type type)
        {
            Ensure.NotNull(type, "type");
            return storage.ContainsKey(type);
        }

        public AggregateRootHandlerCollection Publish(AggregateRoot model, IEvent payload)
        {
            Ensure.NotNull(model, "model");
            Ensure.NotNull(payload, "payload");

            Type type = model.GetType();
            Type eventType = payload.GetType();

            Dictionary<Type, Action<object, object>> typeHandlers;
            if (!storage.TryGetValue(type, out typeHandlers))
                throw Ensure.Exception.InvalidOperation("Type '{0}' is not registered for publishing events.", type.FullName);

            Action<object, object> handler;
            if (!typeHandlers.TryGetValue(eventType, out handler))
                throw Ensure.Exception.InvalidOperation("Type '{0}' doesn't have registered handler for event of type '{1}'.", type.FullName, eventType.FullName);

            handler(model, payload);
            return this;
        }
    }
}

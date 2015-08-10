/* Generated by SharpKit 5 v5.4.4 */
if (typeof($CreateException)=='undefined') 
{
    var $CreateException = function(ex, error) 
    {
        if(error==null)
            error = new Error();
        if(ex==null)
            ex = new System.Exception.ctor();       
        error.message = ex.message;
        for (var p in ex)
           error[p] = ex[p];
        return error;
    }
}

if (typeof ($CreateAnonymousDelegate) == 'undefined') {
    var $CreateAnonymousDelegate = function (target, func) {
        if (target == null || func == null)
            return func;
        var delegate = function () {
            return func.apply(target, arguments);
        };
        delegate.func = func;
        delegate.target = target;
        delegate.isDelegate = true;
        return delegate;
    }
}


if (typeof(JsTypes) == "undefined")
    var JsTypes = [];
var Neptuo$Services$Events$DefaultEventManager = {
    fullname: "Neptuo.Services.Events.DefaultEventManager",
    baseTypeName: "System.Object",
    assemblyName: "Neptuo.Services.Events",
    interfaceNames: ["Neptuo.Services.Events.IEventDispatcher", "Neptuo.Services.Events.IEventHandlerCollection"],
    Kind: "Class",
    definition: {
        ctor: function (){
            this.eventTypeResolver = null;
            this.registry = null;
            System.Object.ctor.call(this);
            this.eventTypeResolver = new Neptuo.Services.Events.Internals.TypeResolver.ctor(Typeof(Neptuo.Services.Events.Handlers.IEventHandlerContext$1.ctor));
            this.registry = new Neptuo.Services.Events.Internals.ThreeBranchStorage.ctor();
        },
        PublishAsync$1: function (TEvent, payload){
            Neptuo.Ensure.NotNull$$Object$$String(payload, "payload");
            var eventType = Typeof(TEvent);
            var eventTypeDescriptor = this.eventTypeResolver.Resolve(eventType);
            if (eventTypeDescriptor.get_IsContext())
                throw $CreateException(Neptuo._EnsureSystemExtensions.NotSupported(Neptuo.Ensure.Exception, "Event manager can publish event context."), new Error());
            if (eventTypeDescriptor.get_IsEnvelope()){
                var contextType = Typeof(Neptuo.Services.Events.Handlers.DefaultEventHandlerContext$1.ctor).MakeGenericType(eventTypeDescriptor.get_DataType());
                var context = System.Activator.CreateInstance$$Type$$Object$Array(contextType, payload, this, this);
                var publishInternalMethod = Typeof(Neptuo.Services.Events.DefaultEventManager.ctor).GetMethod$$String$$BindingFlags("PublishInternalAsyc", 36);
                if (System.Reflection.MethodInfo.op_Equality$$MethodInfo$$MethodInfo(publishInternalMethod, null))
                    throw $CreateException(Neptuo._EnsureSystemExtensions.NotImplemented(Neptuo.Ensure.Exception, "Bug in implementation of DefaultEventManager. Unnable to find publishing method."), new Error());
                return Cast(publishInternalMethod.MakeGenericMethod(eventTypeDescriptor.get_DataType()).Invoke$$Object$$Object$Array(this, [context]), System.Threading.Tasks.Task.ctor);
            }
            return this.PublishInternalAsyc$1(TEvent, new Neptuo.Services.Events.Handlers.DefaultEventHandlerContext$1.ctor$$TEvent$$IEventHandlerCollection$$IEventDispatcher(TEvent, payload, this, this));
        },
        PublishInternalAsyc$1: function (TEvent, context){
            var eventType = Typeof(TEvent);
            var contextHandlers = this.registry.GetContextHandlers(eventType);
            var envelopeHandlers = this.registry.GetEnvelopeHandlers(eventType);
            var directHandlers = this.registry.GetDirectHandlers(eventType);
            var tasks = new Array(contextHandlers.get_Length() + envelopeHandlers.get_Length() + directHandlers.get_Length());
            if (tasks.get_Length() == 0)
                return System.Threading.Tasks.Task.FromResult$1(System.Boolean.ctor, true);
            for (var i = 0; i < contextHandlers.get_Length(); i++)
                tasks[i] = (Cast(contextHandlers[i], Neptuo.Services.Events.Handlers.IEventHandler$1.ctor)).HandleAsync(context);
            for (var i = 0; i < envelopeHandlers.get_Length(); i++)
                tasks[contextHandlers.get_Length() + i] = (Cast(envelopeHandlers[i], Neptuo.Services.Events.Handlers.IEventHandler$1.ctor)).HandleAsync(context.get_Payload());
            for (var i = 0; i < directHandlers.get_Length(); i++)
                tasks[contextHandlers.get_Length() + envelopeHandlers.get_Length() + i] = (Cast(directHandlers[i], Neptuo.Services.Events.Handlers.IEventHandler$1.ctor)).HandleAsync(context.get_Payload().get_Body());
            return System.Threading.Tasks.Task.get_Factory().ContinueWhenAll$1$$Task$Array$$Func$2(System.Threading.Tasks.Task$1.ctor, tasks, $CreateAnonymousDelegate(this, function (items){
                return System.Threading.Tasks.Task.FromResult$1(System.Boolean.ctor, true);
            }));
        },
        Subscribe$1: function (TEvent, handler){
            Neptuo.Ensure.NotNull$$Object$$String(handler, "handler");
            var eventType = Typeof(TEvent);
            var eventTypeDescriptor = this.eventTypeResolver.Resolve(eventType);
            if (eventTypeDescriptor.get_IsContext())
                this.registry.AddContextHandler(eventTypeDescriptor.get_DataType(), handler);
            else if (eventTypeDescriptor.get_IsEnvelope())
                this.registry.AddEnvelopeHandler(eventTypeDescriptor.get_DataType(), handler);
            else
                this.registry.AddDirectHandler(eventTypeDescriptor.get_DataType(), handler);
            return this;
        },
        UnSubscribe$1: function (TEvent, handler){
            Neptuo.Ensure.NotNull$$Object$$String(handler, "handler");
            var eventType = Typeof(TEvent);
            var eventTypeDescriptor = this.eventTypeResolver.Resolve(eventType);
            if (eventTypeDescriptor.get_IsContext())
                this.registry.RemoveContextHandler(eventTypeDescriptor.get_DataType(), handler);
            else if (eventTypeDescriptor.get_IsEnvelope())
                this.registry.RemoveEnvelopeHandler(eventTypeDescriptor.get_DataType(), handler);
            else
                this.registry.RemoveDirectHandler(eventTypeDescriptor.get_DataType(), handler);
            return this;
        }
    },
    ctors: [{
        name: "ctor",
        parameters: []
    }
    ],
    IsAbstract: false
};
JsTypes.push(Neptuo$Services$Events$DefaultEventManager);
var Neptuo$Services$Events$Handlers$ActivatorEventHandler$2 = {
    fullname: "Neptuo.Services.Events.Handlers.ActivatorEventHandler$2",
    baseTypeName: "System.Object",
    assemblyName: "Neptuo.Services.Events",
    interfaceNames: ["Neptuo.Services.Events.Handlers.IEventHandler$1"],
    Kind: "Class",
    definition: {
        ctor: function (TEventHandler, TEvent, innerHandlerFactory){
            this.TEventHandler = TEventHandler;
            this.TEvent = TEvent;
            this.innerHandlerFactory = null;
            System.Object.ctor.call(this);
            Neptuo.Ensure.NotNull$$Object$$String(innerHandlerFactory, "innerHandlerFactory");
            this.innerHandlerFactory = innerHandlerFactory;
        },
        HandleAsync: function (payload){
            var innerHandler = this.innerHandlerFactory.Create();
            return innerHandler.HandleAsync(payload);
        }
    },
    ctors: [{
        name: "ctor",
        parameters: ["Neptuo.Activators.IFactory"]
    }
    ],
    IsAbstract: false
};
JsTypes.push(Neptuo$Services$Events$Handlers$ActivatorEventHandler$2);
var Neptuo$Services$Events$Handlers$DefaultEventHandlerContext$1 = {
    fullname: "Neptuo.Services.Events.Handlers.DefaultEventHandlerContext$1",
    baseTypeName: "System.Object",
    assemblyName: "Neptuo.Services.Events",
    interfaceNames: ["Neptuo.Services.Events.Handlers.IEventHandlerContext$1"],
    Kind: "Class",
    definition: {
        ctor$$TEvent$$IEventHandlerCollection$$IEventDispatcher: function (TEvent, payload, eventHandlers, dispatcher){
            this.TEvent = TEvent;
            this._Payload = null;
            this._EventHandlers = null;
            this._Dispatcher = null;
            Neptuo.Services.Events.Handlers.DefaultEventHandlerContext$1.ctor$$Envelope$1$$IEventHandlerCollection$$IEventDispatcher.call(this, this.TEvent, Neptuo.Components.Envelope.Create$1(this.TEvent, payload), eventHandlers, dispatcher);
        },
        Payload$$: "Neptuo.Components.Envelope`1[[`0]]",
        get_Payload: function (){
            return this._Payload;
        },
        set_Payload: function (value){
            this._Payload = value;
        },
        EventHandlers$$: "Neptuo.Services.Events.IEventHandlerCollection",
        get_EventHandlers: function (){
            return this._EventHandlers;
        },
        set_EventHandlers: function (value){
            this._EventHandlers = value;
        },
        Dispatcher$$: "Neptuo.Services.Events.IEventDispatcher",
        get_Dispatcher: function (){
            return this._Dispatcher;
        },
        set_Dispatcher: function (value){
            this._Dispatcher = value;
        },
        ctor$$Envelope$1$$IEventHandlerCollection$$IEventDispatcher: function (TEvent, payload, eventHandlers, dispatcher){
            this.TEvent = TEvent;
            this._Payload = null;
            this._EventHandlers = null;
            this._Dispatcher = null;
            System.Object.ctor.call(this);
            Neptuo.Ensure.NotNull$$Object$$String(payload, "payload");
            Neptuo.Ensure.NotNull$$Object$$String(eventHandlers, "eventHandlers");
            Neptuo.Ensure.NotNull$$Object$$String(dispatcher, "dispatcher");
            this.set_Payload(payload);
            this.set_EventHandlers(eventHandlers);
            this.set_Dispatcher(dispatcher);
        }
    },
    ctors: [{
        name: "ctor$$TEvent$$IEventHandlerCollection$$IEventDispatcher",
        parameters: ["TEvent", "Neptuo.Services.Events.IEventHandlerCollection", "Neptuo.Services.Events.IEventDispatcher"]
    }, {
        name: "ctor$$Envelope$$IEventHandlerCollection$$IEventDispatcher",
        parameters: ["Neptuo.Components.Envelope", "Neptuo.Services.Events.IEventHandlerCollection", "Neptuo.Services.Events.IEventDispatcher"]
    }
    ],
    IsAbstract: false
};
JsTypes.push(Neptuo$Services$Events$Handlers$DefaultEventHandlerContext$1);
var Neptuo$Services$Events$Handlers$DelegateEventHandler = {
    fullname: "Neptuo.Services.Events.Handlers.DelegateEventHandler",
    baseTypeName: "System.Object",
    staticDefinition: {
        FromAction$1: function (TEvent, action){
            Neptuo.Ensure.NotNull$$Object$$String(action, "action");
            return new Neptuo.Services.Events.Handlers.DelegateEventHandler.EventHandler$1.ctor(TEvent, function (payload){
                action(payload);
                return System.Threading.Tasks.Task.FromResult$1(System.Boolean.ctor, true);
            });
        },
        FromFunc$1: function (TEvent, func){
            Neptuo.Ensure.NotNull$$Object$$String(func, "func");
            return new Neptuo.Services.Events.Handlers.DelegateEventHandler.EventHandler$1.ctor(TEvent, func);
        }
    },
    assemblyName: "Neptuo.Services.Events",
    Kind: "Class",
    definition: {
        ctor: function (){
            System.Object.ctor.call(this);
        }
    },
    ctors: [],
    IsAbstract: true
};
JsTypes.push(Neptuo$Services$Events$Handlers$DelegateEventHandler);
var Neptuo$Services$Events$Handlers$DelegateEventHandler$EventHandler$1 = {
    fullname: "Neptuo.Services.Events.Handlers.DelegateEventHandler.EventHandler$1",
    baseTypeName: "System.Object",
    assemblyName: "Neptuo.Services.Events",
    interfaceNames: ["Neptuo.Services.Events.Handlers.IEventHandler$1"],
    Kind: "Class",
    definition: {
        ctor: function (TEvent, func){
            this.TEvent = TEvent;
            this.func = null;
            System.Object.ctor.call(this);
            this.func = func;
        },
        HandleAsync: function (payload){
            return this.func(payload);
        }
    },
    ctors: [{
        name: "ctor",
        parameters: ["System.Func"]
    }
    ],
    IsAbstract: false
};
JsTypes.push(Neptuo$Services$Events$Handlers$DelegateEventHandler$EventHandler$1);
var Neptuo$Services$Events$Handlers$IEventHandler$1 = {
    fullname: "Neptuo.Services.Events.Handlers.IEventHandler$1",
    baseTypeName: "System.Object",
    assemblyName: "Neptuo.Services.Events",
    Kind: "Interface",
    ctors: [],
    IsAbstract: true
};
JsTypes.push(Neptuo$Services$Events$Handlers$IEventHandler$1);
var Neptuo$Services$Events$Handlers$IEventHandlerContext$1 = {
    fullname: "Neptuo.Services.Events.Handlers.IEventHandlerContext$1",
    baseTypeName: "System.Object",
    assemblyName: "Neptuo.Services.Events",
    Kind: "Interface",
    ctors: [],
    IsAbstract: true
};
JsTypes.push(Neptuo$Services$Events$Handlers$IEventHandlerContext$1);
var Neptuo$Services$Events$Handlers$WeakEventHandler$1 = {
    fullname: "Neptuo.Services.Events.Handlers.WeakEventHandler$1",
    baseTypeName: "System.Object",
    assemblyName: "Neptuo.Services.Events",
    interfaceNames: ["Neptuo.Services.Events.Handlers.IEventHandler$1"],
    Kind: "Class",
    definition: {
        ctor: function (TEvent, innerHandler){
            this.TEvent = TEvent;
            this.innerHandler = null;
            System.Object.ctor.call(this);
            Neptuo.Ensure.NotNull$$Object$$String(innerHandler, "innerHandler");
            this.innerHandler = new System.WeakReference$1.ctor$$T(Neptuo.Services.Events.Handlers.IEventHandler$1.ctor, innerHandler);
        },
        HandleAsync: function (context){
            var target;
            if ((function (){
                var $1 = {
                    Value: target
                };
                var $res = this.innerHandler.TryGetTarget($1);
                target = $1.Value;
                return $res;
            }).call(this))
                return target.HandleAsync(context.get_Payload().get_Body());
            else
                context.get_EventHandlers().UnSubscribe$1(Neptuo.Services.Events.Handlers.IEventHandlerContext$1.ctor, this);
            return System.Threading.Tasks.Task.FromResult$1(System.Boolean.ctor, false);
        }
    },
    ctors: [{
        name: "ctor",
        parameters: ["Neptuo.Services.Events.Handlers.IEventHandler"]
    }
    ],
    IsAbstract: false
};
JsTypes.push(Neptuo$Services$Events$Handlers$WeakEventHandler$1);
var Neptuo$Services$Events$IEventDispatcher = {
    fullname: "Neptuo.Services.Events.IEventDispatcher",
    baseTypeName: "System.Object",
    assemblyName: "Neptuo.Services.Events",
    Kind: "Interface",
    ctors: [],
    IsAbstract: true
};
JsTypes.push(Neptuo$Services$Events$IEventDispatcher);
var Neptuo$Services$Events$IEventHandlerCollection = {
    fullname: "Neptuo.Services.Events.IEventHandlerCollection",
    baseTypeName: "System.Object",
    assemblyName: "Neptuo.Services.Events",
    Kind: "Interface",
    ctors: [],
    IsAbstract: true
};
JsTypes.push(Neptuo$Services$Events$IEventHandlerCollection);
var Neptuo$Services$Events$Internals$ThreeBranchStorage = {
    fullname: "Neptuo.Services.Events.Internals.ThreeBranchStorage",
    baseTypeName: "System.Object",
    assemblyName: "Neptuo.Services.Events",
    Kind: "Class",
    definition: {
        ctor: function (){
            this.directHandlers = null;
            this.envelopeHandlers = null;
            this.contextHandlers = null;
            System.Object.ctor.call(this);
        },
        AddHandlerInternal: function (eventType, storage, handler){
            var handlers;
            if (!(function (){
                var $1 = {
                    Value: handlers
                };
                var $res = storage.TryGetValue(eventType, $1);
                handlers = $1.Value;
                return $res;
            }).call(this))
                storage.set_Item$$TKey(eventType, handlers = new System.Collections.Generic.List$1.ctor(System.Object.ctor));
            handlers.Add(handler);
        },
        AddDirectHandler: function (eventType, handler){
            if (this.directHandlers == null)
                this.directHandlers = new System.Collections.Generic.Dictionary$2.ctor(System.Type.ctor, System.Collections.Generic.List$1.ctor);
            this.AddHandlerInternal(eventType, this.directHandlers, handler);
        },
        AddEnvelopeHandler: function (eventType, handler){
            if (this.envelopeHandlers == null)
                this.envelopeHandlers = new System.Collections.Generic.Dictionary$2.ctor(System.Type.ctor, System.Collections.Generic.List$1.ctor);
            this.AddHandlerInternal(eventType, this.envelopeHandlers, handler);
        },
        AddContextHandler: function (eventType, handler){
            if (this.contextHandlers == null)
                this.contextHandlers = new System.Collections.Generic.Dictionary$2.ctor(System.Type.ctor, System.Collections.Generic.List$1.ctor);
            this.AddHandlerInternal(eventType, this.contextHandlers, handler);
        },
        RemoveHandlerInternal: function (eventType, storage, handler){
            if (storage != null){
                var handlers;
                if ((function (){
                    var $1 = {
                        Value: handlers
                    };
                    var $res = storage.TryGetValue(eventType, $1);
                    handlers = $1.Value;
                    return $res;
                }).call(this))
                    handlers.Remove(handler);
            }
        },
        RemoveDirectHandler: function (eventType, handler){
            this.RemoveHandlerInternal(eventType, this.directHandlers, handler);
        },
        RemoveEnvelopeHandler: function (eventType, handler){
            this.RemoveHandlerInternal(eventType, this.envelopeHandlers, handler);
        },
        RemoveContextHandler: function (eventType, handler){
            this.RemoveHandlerInternal(eventType, this.contextHandlers, handler);
        },
        GetHandlersInternal: function (eventType, storage, includeSubTypes){
            if (storage != null){
                var handlers;
                if ((function (){
                    var $1 = {
                        Value: handlers
                    };
                    var $res = storage.TryGetValue(eventType, $1);
                    handlers = $1.Value;
                    return $res;
                }).call(this))
                    return handlers;
            }
            return System.Linq.Enumerable.Empty$1(System.Object.ctor);
        },
        GetDirectHandlers: function (eventType){
            return System.Linq.Enumerable.ToArray$1(System.Object.ctor, this.GetHandlersInternal(eventType, this.directHandlers, true));
        },
        GetEnvelopeHandlers: function (eventType){
            return System.Linq.Enumerable.ToArray$1(System.Object.ctor, this.GetHandlersInternal(eventType, this.envelopeHandlers, true));
        },
        GetContextHandlers: function (eventType){
            return System.Linq.Enumerable.ToArray$1(System.Object.ctor, this.GetHandlersInternal(eventType, this.contextHandlers, true));
        }
    },
    ctors: [{
        name: "ctor",
        parameters: []
    }
    ],
    IsAbstract: false
};
JsTypes.push(Neptuo$Services$Events$Internals$ThreeBranchStorage);
var Neptuo$Services$Events$Internals$TypeResolver = {
    fullname: "Neptuo.Services.Events.Internals.TypeResolver",
    baseTypeName: "System.Object",
    assemblyName: "Neptuo.Services.Events",
    Kind: "Class",
    definition: {
        ctor: function (contextType){
            this.contextType = null;
            System.Object.ctor.call(this);
            Neptuo.Ensure.NotNull$$Object$$String(contextType, "contextType");
            this.contextType = contextType;
        },
        Resolve: function (targetType){
            Neptuo.Ensure.NotNull$$Object$$String(targetType, "targetType");
            return new Neptuo.Services.Events.Internals.TypeResolverResult.ctor(this.contextType, targetType);
        }
    },
    ctors: [{
        name: "ctor",
        parameters: ["System.Type"]
    }
    ],
    IsAbstract: false
};
JsTypes.push(Neptuo$Services$Events$Internals$TypeResolver);
var Neptuo$Services$Events$Internals$TypeResolverResult = {
    fullname: "Neptuo.Services.Events.Internals.TypeResolverResult",
    baseTypeName: "System.Object",
    assemblyName: "Neptuo.Services.Events",
    Kind: "Class",
    definition: {
        ctor: function (contextType, targetType){
            this.contextType = null;
            this.targetType = null;
            this._IsContext = false;
            this._IsEnvelope = false;
            this._DataType = null;
            System.Object.ctor.call(this);
            Neptuo.Ensure.NotNull$$Object$$String(contextType, "contextType");
            Neptuo.Ensure.NotNull$$Object$$String(targetType, "targetType");
            this.contextType = contextType;
            this.targetType = targetType;
            if (targetType.get_IsGenericType()){
                var genericType = targetType.GetGenericTypeDefinition();
                this.set_IsContext(contextType.IsAssignableFrom(genericType));
                this.set_IsEnvelope(Typeof(Neptuo.Components.Envelope$1.ctor).IsAssignableFrom(genericType));
                this.set_DataType(System.Linq.Enumerable.First$1$$IEnumerable$1(System.Type.ctor, targetType.GetGenericArguments()));
            }
            else {
                this.set_DataType(targetType);
            }
        },
        IsContext$$: "System.Boolean",
        get_IsContext: function (){
            return this._IsContext;
        },
        set_IsContext: function (value){
            this._IsContext = value;
        },
        IsEnvelope$$: "System.Boolean",
        get_IsEnvelope: function (){
            return this._IsEnvelope;
        },
        set_IsEnvelope: function (value){
            this._IsEnvelope = value;
        },
        DataType$$: "System.Type",
        get_DataType: function (){
            return this._DataType;
        },
        set_DataType: function (value){
            this._DataType = value;
        }
    },
    ctors: [{
        name: "ctor",
        parameters: ["System.Type", "System.Type"]
    }
    ],
    IsAbstract: false
};
JsTypes.push(Neptuo$Services$Events$Internals$TypeResolverResult);
var Neptuo$Services$Events$VersionInfo = {
    fullname: "Neptuo.Services.Events.VersionInfo",
    baseTypeName: "System.Object",
    staticDefinition: {
        cctor: function (){
            Neptuo.Services.Events.VersionInfo.Version = "1.0.0";
        },
        GetVersion: function (){
            return new System.Version.ctor$$String("1.0.0");
        }
    },
    assemblyName: "Neptuo.Services.Events",
    Kind: "Class",
    definition: {
        ctor: function (){
            System.Object.ctor.call(this);
        }
    },
    ctors: [],
    IsAbstract: true
};
JsTypes.push(Neptuo$Services$Events$VersionInfo);


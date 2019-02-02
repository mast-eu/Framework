﻿using Neptuo;
using Neptuo.Behaviors;
using Neptuo.Behaviors.Processing.Reflection;
using Neptuo.Behaviors.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole.Behaviors
{
    class TestBehaviors
    {
        public static void Test()
        {
            // Map behaviors.
            InterfaceBehaviorCollection interfaceBehaviors = new InterfaceBehaviorCollection();
            interfaceBehaviors
                .Add<I1, Ti1>()
                .Insert<I2, Ti2>(0)
                .Insert(0, typeof(I3<,>), typeof(Ti3<,>))
                .Add(typeof(I4<>), typeof(Ti4));

            IBehaviorProvider behaviorProvider = new BehaviorProviderCollection()
                .Add(interfaceBehaviors)
                .Add(new AttributeBehaviorCollection().Add(typeof(ReprocessAttribute), typeof(ReprocessBehavior)));

            IEnumerable<Type> behaviors = behaviorProvider.GetBehaviors(typeof(Test));
            Console.WriteLine("Number of behaviors for Test class '{0}'.", behaviors.Count());

            // Create reflection providers.
            IReflectionBehaviorFactory behaviorInstance = new ReflectionBehaviorFactoryCollection()
                .Add(typeof(ReprocessBehavior), new ReflectionReprocessBehaviorInstanceProvider());

            // Invoke pipeline.
            //ReflectionMethodInvokePipeline<HelloService, string> pipeline = new ReflectionMethodInvokePipeline<HelloService, string>(interfaceBehaviors, behaviorInstance, "SayHello");
            //pipeline.ExecuteAsync().ContinueWith(message => Console.WriteLine("Method result: '{0}'.", message.Result));
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ReprocessAttribute : Attribute
    {
        public int Count { get; }
        public int Delay { get; }

        public ReprocessAttribute(int count, int delay)
        {
            Ensure.PositiveOrZero(count, "count");
            Ensure.PositiveOrZero(delay, "delay");
            Count = count;
            Delay = delay;
        }
    }

    public class ReprocessBehavior : IBehavior<object>
    {
        public async Task ExecuteAsync(object handler, IBehaviorContext context)
        {
            int count = 0;

            while (true)
            {
                try
                {
                    await context.NextAsync();
                    break;
                }
                catch (Exception)
                {
                    count++;
                }
            }
        }
    }

    public class ReflectionReprocessBehaviorInstanceProvider : IReflectionBehaviorFactory
    {
        public object TryCreate(IReflectionBehaviorFactoryContext context, Type behaviorType)
        {
            return new ReprocessBehavior();
        }
    }

    public class Test : I1, I2, I3<string, string>, I3<int, int>, I4<string>, I4<int>
    { }

    [Reprocess(2, 5000)]
    public class HelloService
    {
        bool isFirst = true;

        public string SayHello()
        {
            if (isFirst)
            {
                isFirst = false;
                throw new NotSupportedException();
            }
            
            return "Hello!";
        }
    }

    public interface I1
    { }

    public class Ti1 : IBehavior<I1>
    {
        public Task ExecuteAsync(I1 handler, IBehaviorContext context)
        {
            throw new NotImplementedException();
        }
    }

    public interface I2
    { }

    public class Ti2 : IBehavior<I2>
    {
        public Task ExecuteAsync(I2 handler, IBehaviorContext context)
        {
            throw new NotImplementedException();
        }
    }

    public interface I3<T1, T2>
    { }

    public class Ti3<T1, T2> : IBehavior<I3<T1, T2>>
    {
        public Task ExecuteAsync(I3<T1, T2> handler, IBehaviorContext context)
        {
            throw new NotImplementedException();
        }
    }

    public interface I4<T1>
    { }

    public class Ti4 : IBehavior<object>
    {
        public Task ExecuteAsync(object handler, IBehaviorContext context)
        {
            throw new NotImplementedException();
        }
    }
}

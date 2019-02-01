﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neptuo.Activators;
using Neptuo.Behaviors;
using Neptuo.Behaviors.Processing;
using Neptuo.Behaviors.Processing.Compilation;
using Neptuo.Behaviors.Processing.Reflection;
using Neptuo.Behaviors.Providers;
using Neptuo.Compilers;
using Neptuo.Queries.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Neptuo.Queries
{
    public class ProductQuery : IQuery<Product>
    {
        public string Name { get; set; }
    }

    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public class ProductQueryHandler : IQueryHandler<ProductQuery, Product>
    {
        public Task<Product> HandleAsync(ProductQuery query, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Product()
            {
                Name = query.Name,
                Price = 10.1M
            });
        }
    }

    public class TestBehavior : IBehavior<object>
    {
        public static int count;

        public Task ExecuteAsync(object handler, IBehaviorContext context)
        {
            count++;
            return context.NextAsync();
        }
    }

    [TestClass]
    public class T_Services_Queries_Behaviors
    {
        private void TestPipeline(Func<IBehaviorProvider, IPipeline<ProductQueryHandler>> pipelinefactory)
        {
            IBehaviorProvider behaviorProvider = new GlobalBehaviorCollection()
                .Add(typeof(TestBehavior));

            IPipeline<ProductQueryHandler> pipeline = pipelinefactory(behaviorProvider);
            IQueryHandler<ProductQuery, Product> queryHandler = new BehaviorQueryHandler<ProductQueryHandler, ProductQuery, Product>(pipeline, new DefaultFactory<ProductQueryHandler>());

            Task<Product> task = queryHandler.HandleAsync(new ProductQuery() { Name = "Test" }, default);
            if (!task.IsCompleted)
                task.RunSynchronously();

            Assert.AreEqual(10.1M, task.Result.Price);
            Assert.AreEqual(1, TestBehavior.count);
        }

        [TestMethod]
        public void ReflectionPipeline()
        {
            TestBehavior.count = 0;
            TestPipeline(behaviorProvider => new ReflectionPipeline<ProductQueryHandler>(behaviorProvider, new DefaultReflectionBehaviorFactory()));
        }

        [TestMethod]
        public void CodeDomPipeline()
        {
            TestBehavior.count = 0;
            ICompilerConfiguration configuration = new CompilerConfiguration();
            configuration.References().AddDirectory(Environment.CurrentDirectory);

            TestPipeline(behaviorProvider => new CodeDomPipeline<ProductQueryHandler>(new CodeDomPipelineConfiguration(behaviorProvider, configuration)));
        }
    }
}

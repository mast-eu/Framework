﻿using Neptuo.ComponentModel.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Timers.Behaviors
{
    public class ReprocessBehavior : IBehavior<object>
    {
        public int Count { get; set; }

        public Task ExecuteAsync(object handler, IBehaviorContext context)
        {
            return ExecuteAsync(handler, context, Count);
        }

        private Task ExecuteAsync(object handler, IBehaviorContext context, int remaingCount)
        {
            IBehaviorContext contextState = context.Clone();
            try
            {
                return context.NextAsync();
            }
            catch (Exception e)
            {
                remaingCount--;
                if (remaingCount > 0)
                    return ExecuteAsync(handler, contextState, remaingCount);
                else
                    throw e;
            }
        }
    }
}

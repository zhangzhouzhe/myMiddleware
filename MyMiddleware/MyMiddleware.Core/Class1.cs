using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMiddleware.Core
{
    public static class Extension
    {
        public static IPipelineBuilder<TContext> Use<TContext>(
            this IPipelineBuilder<TContext> builder, Action<TContext, Action> action)
            where TContext : IContext
        {
            return builder.Use(next =>
               context =>
               {
                   action(context, () => next(context));
               });

        }
    }

    public interface IContext
    {
        int TenantId { get; set; }
        string App { get; set; }
    }
    public interface IPipelineBuilder<TContext>
        where TContext : IContext
    {
        IPipelineBuilder<TContext> Use(
            Func<Action<TContext>, Action<TContext>> middleware);

        Action<TContext> Build();
    }

    public class Context : IContext
    {
        public int TenantId { get; set; }
        public string App { get; set; }
    }

    public class PipelineBuilder : IPipelineBuilder<Context>
    {
        private Action<Context> _completeFunc;
        private IList<Func<Action<Context>, Action<Context>>> _pipelines
            = new List<Func<Action<Context>, Action<Context>>>();

        public PipelineBuilder(Action<Context> completeFunc)
        {
            _completeFunc = completeFunc;
        }

        public IPipelineBuilder<Context> Use(Func<Action<Context>, Action<Context>> middleware)
        {
            _pipelines.Add(middleware);
            return this;
        }
        public Action<Context> Build()
        {
            var request = _completeFunc;
            foreach (var pipeline in _pipelines.Reverse())
            {
                request = pipeline(request);
            }
            return request;
        }


    }
}

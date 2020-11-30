using MyMiddleware.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMiddleware
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new Context();
            context.TenantId = 100001;
            context.App = "UserFramework";

            var builder = new PipelineBuilder(Finally)
                ;
        }


        static void Finally(Context context)
        {
            Console.WriteLine($"Finally:TenantId{context.TenantId}");
        }
    }
}

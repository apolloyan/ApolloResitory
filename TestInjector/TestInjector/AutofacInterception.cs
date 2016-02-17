using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestInjector
{
    public class AutofacInterception : Castle.DynamicProxy.IInterceptor
    {
        public void Intercept(Castle.DynamicProxy.IInvocation invocation)
        {
            TestLog.SayHello();
            invocation.Proceed();
        }

        //public void Intercept(IInvocation invocation)
        //{
        //    TestLog.SayHello();
        //    invocation.Proceed();
        //    //TestLog.SayBye();
        //}
    }
}

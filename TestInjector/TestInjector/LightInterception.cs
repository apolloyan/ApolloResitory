using LightInject.Interception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestInjector
{
    public class LightInterception : LightInject.Interception.IInterceptor
    {
        public object Invoke(IInvocationInfo invocationInfo)
        {
            TestLog.SayHello();
            return invocationInfo.Proceed();
        }
    }
}

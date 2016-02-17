using ITestInjector.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestInjector.Implementation
{
    public class DoSomeSimple : IDoSomeSimple
    {
        public void DoToString(int i)
        {
            i.ToString();
        }
    }
}

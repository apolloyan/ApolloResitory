using Autofac;
using Autofac.Extras.DynamicProxy2;
using ITestInjector.Interface;
using LightInject;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestInjector.Implementation;
using System.Linq.Expressions;
using System.Reflection;

namespace TestInjector
{
    class Program
    {
        private const int COUNT = 1000000;
        private static IContainer AutofacContainer { get; set; }

        private static IContainer AutofacContainerWithInterceptor { get; set; }

        private static Container simpleContainer { get; set; }

        private static Container simpleContainerWithInterceptor { get; set; }

        private static ServiceContainer lightContainer { get; set; }

        private static ServiceContainer lightContainerWithInterception { get; set; }
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            AutofacReg();
            SimpleInjectorReg();
            LightInjectorReg();
            LightInjectorRegWithInterception();
            AutofacRegWithInterceptor();
            SimpleInjectorRegWithInterceptor();
            stopwatch.Start();
            for (int i = 0; i < COUNT; i++)
            {
                i.ToString();
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            stopwatch.Reset();
            stopwatch.Restart();
            for (int i = 0; i < COUNT; i++)
            {
                IDoSomeSimple dosome = new DoSomeSimple();
                dosome.DoToString(i);
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            stopwatch.Reset();
            stopwatch.Restart();
            for (int i = 0; i < COUNT; i++)
            {
                IDoSomeSimple autofacdo = AutofacContainer.Resolve<IDoSomeSimple>();
                autofacdo.DoToString(i);
            }
            stopwatch.Stop();
            Console.WriteLine("autofac:" + stopwatch.ElapsedMilliseconds);
            stopwatch.Reset();
            stopwatch.Restart();
            for (int i = 0; i < COUNT; i++)
            {
                IDoSomeSimple simple = simpleContainer.GetInstance<IDoSomeSimple>();
                simple.DoToString(i);
            }
            stopwatch.Stop();
            Console.WriteLine("simle:" + stopwatch.ElapsedMilliseconds);
            stopwatch.Reset();
            stopwatch.Restart();
            for (int i = 0; i < COUNT; i++)
            {
                IDoSomeSimple light = lightContainer.GetInstance<IDoSomeSimple>();
                light.DoToString(i);
            }
            stopwatch.Stop();
            Console.WriteLine("light:" + stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            stopwatch.Restart();
            for (int i = 0; i < COUNT; i++)
            {
                IDoSomeSimple light = lightContainerWithInterception.GetInstance<IDoSomeSimple>();
                light.DoToString(i);
            }
            stopwatch.Stop();
            Console.WriteLine("lightWithInterception:" + stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            stopwatch.Restart();
            for (int i = 0; i < COUNT; i++)
            {
                IDoSomeSimple auto = AutofacContainerWithInterceptor.Resolve<IDoSomeSimple>();
                auto.DoToString(i);
            }
            stopwatch.Stop();
            Console.WriteLine("AutofacWithInterceptor:" + stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            stopwatch.Restart();
            for (int i = 0; i < COUNT; i++)
            {
                IDoSomeSimple auto = simpleContainerWithInterceptor.GetInstance<IDoSomeSimple>();
                auto.DoToString(i);
            }
            stopwatch.Stop();
            Console.WriteLine("simpleWithInterceptor:" + stopwatch.ElapsedMilliseconds);

            Console.WriteLine("end");

            Console.Write("end_test");

            Console.WriteLine("end-test-devbranch17：16"); 
            Console.ReadLine();
        }

        private static void AutofacReg()
        {
            var builder = new ContainerBuilder();
            //builder.RegisterType<AutofacInterception>();
            builder.RegisterType<DoSomeSimple>().As<IDoSomeSimple>().SingleInstance();

            AutofacContainer = builder.Build();
        }

        private static void AutofacRegWithInterceptor()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<DoSomeSimple>().As<IDoSomeSimple>().EnableInterfaceInterceptors().InterceptedBy(typeof(AutofacInterception)).SingleInstance();
            builder.Register(a => new AutofacInterception());
            AutofacContainerWithInterceptor = builder.Build();

        }

        private static void SimpleInjectorReg()
        {
            simpleContainer = new Container();
            simpleContainer.Register<IDoSomeSimple, DoSomeSimple>(Lifestyle.Singleton);
            simpleContainer.Verify();
        }

        private static void LightInjectorReg()
        {
            lightContainer = new ServiceContainer();
            //lightContainer.Intercept(sr => sr.ServiceType == typeof(IDoSomeSimple), sf => new LightInterception());
            //lightContainer.BeginScope();
            lightContainer.Register<IDoSomeSimple, DoSomeSimple>(new PerContainerLifetime());
        }

        private static void LightInjectorRegWithInterception()
        {
            lightContainerWithInterception = new ServiceContainer();
            lightContainerWithInterception.Intercept(sr => sr.ServiceType == typeof(IDoSomeSimple), sf => new LightInterception());
            //lightContainer.BeginScope();
            lightContainerWithInterception.Register<IDoSomeSimple, DoSomeSimple>(new PerContainerLifetime());
        }

        private static void SimpleInjectorRegWithInterceptor()
        {
            simpleContainerWithInterceptor = new Container();
            simpleContainerWithInterceptor.Register<IDoSomeSimple, DoSomeSimple>(Lifestyle.Singleton);
            simpleContainerWithInterceptor.InterceptWith<MonitoringInterceptor>(
    serviceType => serviceType.Name.EndsWith("Simple"));
            simpleContainerWithInterceptor.RegisterSingleton<MonitoringInterceptor>();
            simpleContainerWithInterceptor.Verify();
        }


    }
}

using FinPos.DAL.Interfaces;
using FinPos.DAL.Repositories;
//using FinPos.Domain.ServiceContracts;
using FinPos.Service;
using FinPos.WcfHost;
//using FinPos.WcfHost;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.WebHost
{
    class Program
    {
        static void Main(string[] args)
        {
            IKernel _kernel = new StandardKernel();
            _kernel.Load(Assembly.GetExecutingAssembly());  //Load Module
            IFinPosService customerService = _kernel.Get<IFinPosService>();
            ServiceHost serviceHost = new ServiceHost(customerService);
            serviceHost.Open();
            var customers = customerService.GetCompanies();
            Console.WriteLine("Finposservice is up and running");
            Console.ReadLine();
            serviceHost.Close();
        }
    }
}

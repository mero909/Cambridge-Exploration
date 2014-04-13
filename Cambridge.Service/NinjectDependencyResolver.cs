using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Cambridge.Data.Models;
using Ninject;

namespace Cambridge.Service
{
    public class NinjectDependencyResolver  : IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver()
        {
            _kernel = new StandardKernel();
            AddBindings();
        }

        private void AddBindings()
        {
            _kernel.Bind<CambridgeContext>().To<CambridgeContext>();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }
    }
}
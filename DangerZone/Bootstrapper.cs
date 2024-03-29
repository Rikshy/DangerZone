﻿using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System;

using Caliburn.Micro;

using DangerZone.ViewModels;

namespace DangerZone
{
    public class Bootstrapper : BootstrapperBase
    {
        private readonly SimpleContainer container = new();

        public Bootstrapper() => Initialize();

        protected override void Configure()
        {
            container.Instance(container);

            container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>();


            GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(vm => container.RegisterPerRequest(vm, vm.ToString(), vm));
        }

        protected override void OnStartup(object sender, StartupEventArgs e) => DisplayRootViewForAsync<ShellViewModel>();

        protected override object GetInstance(Type service, string key) => container.GetInstance(service, key);

        protected override IEnumerable<object> GetAllInstances(Type service) => container.GetAllInstances(service);

        protected override void BuildUp(object instance) => container.BuildUp(instance);
    }
}

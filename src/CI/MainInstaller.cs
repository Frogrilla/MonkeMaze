using System;
using System.Collections.Generic;
using System.Text;
using ComputerInterface.Interfaces;
using Zenject;

namespace MonkeMaze.CI
{
    internal class MainInstaller : Installer
    {
        public override void InstallBindings()
        {
            base.Container.Bind<IComputerModEntry>().To<MonkeMazeEntry>().AsSingle();
        }
    }
}

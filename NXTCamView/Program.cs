//
//    Copyright 2007 Paul Tingey
//
//    This file is part of NXTCamView.
//
//    NXTCamView is free software; you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation; either version 3 of the License, or
//    (at your option) any later version.
//
//    Foobar is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using System.Windows.Forms;
using Ninject;
using Ninject.Modules;
using NXTCamView.Core;
using NXTCamView.Core.Comms;
using NXTCamView.Forms;
using NXTCamView.VersionUpdater;

namespace NXTCamView
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var kernal = new StandardKernel( new NxtCamViewModule() );
            var mainForm = kernal.Get<MainForm>();
            Application.Run( mainForm );
        }

        class NxtCamViewModule : NinjectModule
        {
            public override void Load()
            {
                Bind<ITracer>().To<Tracer>();
                Bind<IAppState>().To<AppState>();
                Bind<IUpdater>().To<Updater>();
                Bind<ISettings> ().ToConstant<Properties.Settings> (Properties.Settings.Default);

                Bind<ICommsPortFactory>().To<CommsPortFactory>().InSingletonScope();
                Bind<IConfigCommsPort>().To<ConfigCommsPort>().InSingletonScope();
                Bind<ICommsPort>().To<CommsPort>().InTransientScope();

                Bind<ColorForm>().ToSelf();

                //hackish to allow different interfaces to provide the same singleton
                Bind<TrackingForm>().ToSelf();
                Bind<ITrackingForm>().ToMethod( x => x.Kernel.Get<TrackingForm>() );
                Bind<IColorTarget>().ToMethod( x => x.Kernel.Get<TrackingForm>() );

                Bind<MainForm>().ToSelf();
            }
        }
    }
}
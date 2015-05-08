//
//  Program.cs
//
//  Author:
//       David Lechner <david@lechnology.com>
//
//  Copyright (c) 2015 David Lechner
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using Gtk;
using Ninject;
using Ninject.Modules;
using NXTCamView.Core;
using NXTCamView.Core.Comms;
using NXTCamView.Gtk.Windows;

namespace NXTCamView.Gtk
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            Application.Init ("NXTCamView", ref args);
            var kernel = new StandardKernel (new NxtCamViewModule ());
            var win = kernel.Get<MainWindow> ();
            win.Show ();
            Application.Run ();
        }
    }

    class NxtCamViewModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ITracer>().To<Tracer>();
            Bind<IAppState>().To<AppState>();
            //Bind<IUpdater>().To<Updater>();
            Bind<ISettings> ().To<NXTCamView.Gtk.Settings> ().InSingletonScope ();

            Bind<ICommsPortFactory>().To<CommsPortFactory>().InSingletonScope();
            Bind<IConfigCommsPort>().To<ConfigCommsPort>().InSingletonScope();
            Bind<ICommsPort>().To<CommsPort>().InTransientScope();

            //Bind<ColorForm>().ToSelf();

            //hackish to allow different interfaces to provide the same singleton
            //Bind<TrackingForm>().ToSelf();
            //Bind<ITrackingForm>().ToMethod( x => x.Kernel.Get<TrackingForm>() );
            //Bind<IColorTarget>().ToMethod( x => x.Kernel.Get<TrackingForm>() );

            Bind<MainWindow>().ToSelf();
        }
    }
}

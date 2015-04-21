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
using Ninject;
using Ninject.Parameters;

namespace NXTCamView.Core.Comms
{
    public interface ICommsPortFactory
    {
        ICommsPort Create( CommsPortSettings settings );
    }

    public class CommsPortFactory : ICommsPortFactory
    {
        private readonly IKernel _kernel;

        public CommsPortFactory( IKernel kernel )
        {
            _kernel = kernel;
        }

        public ICommsPort Create( CommsPortSettings settings )
        {
            var port = _kernel.Get<ICommsPort>( new ConstructorArgument( "settings", settings ) );
            return port;
        }
    }
}
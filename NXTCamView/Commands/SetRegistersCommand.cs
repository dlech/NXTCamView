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
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace NXTCamView.Commands
{
    public class SetRegistersCommand : Command
    {
        private Dictionary<byte, byte> _registers = new Dictionary<byte, byte>();
        public Dictionary<byte, byte> Register { get { return _registers; } set { _registers = value; } }

        public SetRegistersCommand(SerialPort _serialPort)
            : base("Upload Settings",_serialPort)
        {            
        }

        //Registers
        // name                      reg on off
        // auto white balance        45   7   3
        // auto adjust mode          19   1   0
        // flourescent light filter  18  44  40
        
        private void ensureExists(byte register, byte defVal)
        {
            if (!_registers.ContainsKey(register)) _registers.Add(register, defVal);
        }

        public const byte AutoWhiteBalanceReg = 45;
        public const byte AutoWhiteBalanceON = 7;
        public const byte AutoWhiteBalanceOFF = 3;
        public bool IsAutoWhiteBalance
        {
            get
            {
                ensureExists(AutoWhiteBalanceReg, AutoWhiteBalanceON);
                return _registers[AutoWhiteBalanceReg] == AutoWhiteBalanceON;
            } 
            set
            {
                ensureExists(AutoWhiteBalanceReg, AutoWhiteBalanceON);
                _registers[AutoWhiteBalanceReg] = value ? AutoWhiteBalanceON : AutoWhiteBalanceOFF;
            }
        }

        public const byte AutoAdjustModeReg = 19;
        public const byte AutoAdjustModeON = 1;
        public const byte AutoAdjustModeOFF = 0;
        public bool IsAutoAdjustMode
        {
             get
             {
                 ensureExists(AutoAdjustModeReg, AutoAdjustModeON);
                 return _registers[AutoAdjustModeReg] == AutoAdjustModeON;
                 
             } 
             set
             {
                 ensureExists(AutoAdjustModeReg, AutoAdjustModeON);
                 _registers[AutoAdjustModeReg] = value ? AutoAdjustModeON : AutoAdjustModeOFF;
             }
        }

        public const byte FlourescentLightFilterReg = 18;
        public const byte FlourescentLightFilterON = 44;
        public const byte FlourescentLightFilterOFF = 40;
        public bool HasFlourescentLightFilter
        {
            get
            {
                ensureExists(FlourescentLightFilterReg, FlourescentLightFilterON);
                return _registers[FlourescentLightFilterReg] == FlourescentLightFilterON;
            } 
            set
            {
                ensureExists(FlourescentLightFilterReg, FlourescentLightFilterON);
                _registers[FlourescentLightFilterReg] = value ? FlourescentLightFilterON : FlourescentLightFilterOFF;
            }
        }
        
        public override void Execute()
        {
            try
            {
                SetState(State.ConnectedBusy);
                if( _registers == null ) throw new ApplicationException("No registers to set");
                if( _registers.Count > 8 ) throw new ApplicationException("Too many registers. 8 is the max");
                StringBuilder sb = new StringBuilder();
                foreach( KeyValuePair< byte, byte > pair in _registers )
                {
                    sb.AppendFormat("{0} {1} ", pair.Key, pair.Value);
                }
                //drop the space
                sb.Remove(sb.Length - 1, 1);
                _request = "CR " + sb;
                SendAndReceive();
                _isCompleted = true;
            }
            catch (Exception ex)
            {
                setError(ex);
            }
            finally
            {
                SetState(State.Connected);
                completeCommand();
            }
        }
    }
}
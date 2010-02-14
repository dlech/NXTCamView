using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

//This asesome code is by Vladimir Afanasyev on CodeProject

//.NET - Diving into System Programming - Part 2

namespace NXTCamView.Comms
{
    class DeviceInfo
    {
        public const int DIGCF_PRESENT = (0x00000002);
        public const int MAX_DEV_LEN = 1000;
        public const int SPDRP_FRIENDLYNAME = (0x0000000C);
        // FriendlyName (R/W)
        public const int SPDRP_DEVICEDESC = (0x00000000);
        // DeviceDesc (R/W)

        [StructLayout(LayoutKind.Sequential)]
        public class SP_DEVINFO_DATA
        {
            public int cbSize;
            public Guid ClassGuid;
            public int DevInst;    // DEVINST handle
            public ulong Reserved;
        };

        [DllImport("setupapi.dll")]//
        public static extern Boolean
            SetupDiClassGuidsFromNameA(string ClassN, ref Guid guids,
                                       UInt32 ClassNameSize, ref UInt32 ReqSize);

        [DllImport("setupapi.dll")]
        public static extern IntPtr                //result HDEVINFO
            SetupDiGetClassDevsA(ref Guid ClassGuid, UInt32 Enumerator,
                                 IntPtr hwndParent, UInt32 Flags);

        [DllImport("setupapi.dll")]
        public static extern Boolean
            SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet, UInt32 MemberIndex,
                                  SP_DEVINFO_DATA DeviceInfoData);

        [DllImport("setupapi.dll")]
        public static extern Boolean
            SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

        [DllImport("setupapi.dll")]
        public static extern Boolean
            SetupDiGetDeviceRegistryPropertyA(IntPtr DeviceInfoSet,
                                              SP_DEVINFO_DATA DeviceInfoData, UInt32 Property,
                                              UInt32 PropertyRegDataType, StringBuilder PropertyBuffer,
                                              UInt32 PropertyBufferSize, IntPtr RequiredSize);



        public static int EnumerateDevices(UInt32 DeviceIndex,
                                           string ClassName,
                                           StringBuilder DeviceName)
        {
            UInt32 RequiredSize = 0;
            Guid[] guids = new Guid[1];
            IntPtr NewDeviceInfoSet;
            SP_DEVINFO_DATA DeviceInfoData = new SP_DEVINFO_DATA();


            bool res = SetupDiClassGuidsFromNameA(ClassName,
                                                  ref guids[0], RequiredSize,
                                                  ref RequiredSize);

            if (RequiredSize == 0)
            {
                //incorrect class name:
                return -2;
            }

            if (!res)
            {
                guids = new Guid[RequiredSize];
                res = SetupDiClassGuidsFromNameA(ClassName, ref guids[0], RequiredSize,
                                                 ref RequiredSize);

                if (!res || RequiredSize == 0)
                {
                    //incorrect class name:
                    return -2;
                }
            }

            //get device info set for our device class
            NewDeviceInfoSet = SetupDiGetClassDevsA(ref guids[0], 0, IntPtr.Zero,
                                                    DIGCF_PRESENT);
            if (NewDeviceInfoSet.ToInt32() == -1)
            {
                //device information is unavailable:
                return -3;
            }

            DeviceInfoData.cbSize = 28;
            //is devices exist for class
            DeviceInfoData.DevInst = 0;
            DeviceInfoData.ClassGuid = Guid.Empty;
            DeviceInfoData.Reserved = 0;

            res = SetupDiEnumDeviceInfo(NewDeviceInfoSet,
                                        DeviceIndex, DeviceInfoData);
            if (!res)
            {
                //no such device:
                SetupDiDestroyDeviceInfoList(NewDeviceInfoSet);
                return -1;
            }


            DeviceName.Capacity = MAX_DEV_LEN;
            if (!SetupDiGetDeviceRegistryPropertyA(NewDeviceInfoSet,
                                                   DeviceInfoData,
                                                   SPDRP_FRIENDLYNAME, 0, DeviceName, MAX_DEV_LEN, IntPtr.Zero))
            {
                res = SetupDiGetDeviceRegistryPropertyA(NewDeviceInfoSet,
                                                        DeviceInfoData, SPDRP_DEVICEDESC, 0, DeviceName, MAX_DEV_LEN,
                                                        IntPtr.Zero);
                if (!res)
                {
                    //incorrect device name:
                    SetupDiDestroyDeviceInfoList(NewDeviceInfoSet);
                    return -4;
                }
            }
            return 0;
        }
    }

    public class SerialHelper
    {            
        static public Dictionary<string, string> GetPorts()
        {
            Dictionary<string, string> friendlyPorts = new Dictionary<string, string>();
            try
            {
                //parse line like - "Prolific USB-to-Serial Comm Port (COM11)"
                //return in keyvalue pairs of like - COM11 -> Prolific USB-to-Serial Comm Port (COM11)
                Regex regex = new Regex(@"^(.*)\((COM\d+)\)");
                UInt32 index = 0;
                while (true)
                {
                    StringBuilder devices = new StringBuilder();
                    int result = DeviceInfo.EnumerateDevices(index, "Ports", devices);
                    if( result < 0 ) break;
                    index++;
                    string name = devices.ToString();
                    Match match = regex.Match(name);
                    if( match.Success )
                    {
                        friendlyPorts.Add(match.Groups[2].ToString().Trim(), match.Groups[1].ToString().Trim());
                    }
                }
            }
            catch( Exception ex)
            {
                Debug.WriteLine(string.Format("Error getting friendly names: {0}", ex.Message));
            }
            return friendlyPorts;
        }
    }
       

    //[STAThread]
    //static void Main(string[] args)
    //{
    //    StringBuilder devices = new StringBuilder("");
    //    UInt32 Index = 0;
    //    int result;

    //    if (args.Length != 1)
    //    {
    //        Console.WriteLine("command line format:");
    //        Console.WriteLine("DevInfo <CLASSNAME>");
    //        return;
    //    }

    //    while (true)
    //    {
    //        result = EnumerateDevices(Index, args[0], devices);
    //        Index++;
    //        if (result == -2)
    //        {
    //            Console.WriteLine("Incorrect name of Class = {0}", args[0]);
    //            break;
    //        }
    //        if (result == -1) break;
    //        if (result == 0)
    //            Console.WriteLine("Device{0} is {1}", Index, devices);
    //    }
    //}
}



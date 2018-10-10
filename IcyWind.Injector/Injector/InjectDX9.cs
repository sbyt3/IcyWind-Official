using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using IcyWind.Injector.Injector;

namespace IcyWind.ExtScript.Injector
{
    // ReSharper disable once InconsistentNaming
    public class InjectDX9
    {
        public Process TargetProcess { get; }
        public IntPtr ProcHandle { get; private set; }

        public InjectDX9(Process targetProcess)
        {
            TargetProcess = targetProcess;
        }

        public bool InjectDll(string dllName)
        {
            ProcHandle = Kernal32Native.OpenProcess(Kernal32Native.PROCESS_CREATE_THREAD |
                                                    Kernal32Native.PROCESS_QUERY_INFORMATION |
                                                    Kernal32Native.PROCESS_VM_OPERATION |
                                                    Kernal32Native.PROCESS_VM_READ |
                                                    Kernal32Native.PROCESS_VM_WRITE,
                false,
                TargetProcess.Id);


            var loadLibraryAddress = Kernal32Native.GetProcAddress(Kernal32Native.GetModuleHandle("kernel32.dll"), "LoadLibraryA");
            var allocMemAddress = Kernal32Native.VirtualAllocEx(ProcHandle, 
                IntPtr.Zero, 
                (uint)((dllName.Length + 1) * Marshal.SizeOf(typeof(char))),
                Kernal32Native.MEM_COMMIT |
                Kernal32Native.MEM_RESERVE,
                Kernal32Native.PAGE_READWRITE);


            Kernal32Native.WriteProcessMemory(ProcHandle, 
                allocMemAddress, 
                Encoding.Default.GetBytes(dllName), 
                (uint)((dllName.Length + 1) * Marshal.SizeOf(typeof(char))), 
                out var bytesWritten);

            Kernal32Native.CreateRemoteThread(ProcHandle, 
                IntPtr.Zero, 
                0, 
                loadLibraryAddress, 
                allocMemAddress, 
                0, 
                IntPtr.Zero);

            return true;
        }
    }
}

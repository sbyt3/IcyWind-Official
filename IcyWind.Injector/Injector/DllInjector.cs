﻿using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace IcyWind.Injector.Injector
{
    public enum DllInjectionResult
    {
        DllNotFound,
        ProcessExit,
        InjectionFailed,
        Success
    }

    public sealed class DllInjector
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, uint size, int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttribute, 
            IntPtr dwStackSize, IntPtr lpStartAddress,
            IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        private static DllInjector _instance;

        public static DllInjector GetInstance => _instance ?? (_instance = new DllInjector());

        public DllInjectionResult Inject(Process process, string sDllPath)
        {
            if (!File.Exists(sDllPath))
            {
                return DllInjectionResult.DllNotFound;
            }
            if (Process.GetProcessById(process.Id).ProcessName != process.ProcessName)
            {
                return DllInjectionResult.ProcessExit;
            }
            return !BInject(process.Id, sDllPath) ? DllInjectionResult.InjectionFailed : DllInjectionResult.Success;
        }

        private static bool BInject(int pToBeInjected, string sDllPath)
        {
            var hndProc = OpenProcess(0x2 | 0x8 | 0x10 | 0x20 | 0x400, 1, (uint)pToBeInjected);

            if (hndProc == IntPtr.Zero)
            {
                return false;
            }

            var lpLlAddress = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");

            if (lpLlAddress == IntPtr.Zero)
            {
                return false;
            }

            var lpAddress = VirtualAllocEx(hndProc, IntPtr.Zero, (uint)sDllPath.Length, 0x1000 | 0x2000, 0X40);

            if (lpAddress == IntPtr.Zero)
            {
                return false;
            }

            var bytes = Encoding.ASCII.GetBytes(sDllPath);

            if (WriteProcessMemory(hndProc, lpAddress, bytes, (uint)bytes.Length, 0) == 0)
            {
                return false;
            }

            if (CreateRemoteThread(hndProc, IntPtr.Zero, IntPtr.Zero, lpLlAddress, lpAddress, 0, IntPtr.Zero) == IntPtr.Zero)
            {
                return false;
            }

            CloseHandle(hndProc);

            return true;
        }
    }
}

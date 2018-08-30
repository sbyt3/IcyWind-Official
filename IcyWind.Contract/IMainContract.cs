using System;
using System.AddIn.Contract;
using System.AddIn.Pipeline;

namespace IcyWind.Contract
{
    [AddInContract]
    public interface IMainContract : IContract
    {
        INativeHandleContract Run(params object[] args);

        bool Close();
    }
}
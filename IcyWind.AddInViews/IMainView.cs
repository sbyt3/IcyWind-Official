using System;
using System.AddIn.Pipeline;
using System.Windows;

namespace IcyWind.AddInViews
{
    [AddInBase]
    public interface IMainView
    {
        FrameworkElement Run(params object[] args);

        bool Close();
    }
}
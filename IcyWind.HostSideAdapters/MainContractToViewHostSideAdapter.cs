using System.AddIn.Pipeline;
using System.Windows;
using IcyWind.Contract;
using IcyWind.HostViews;

namespace IcyWind.HostSideAdapters
{
    [HostAdapter]
    public class MainContractToViewHostSideAdapter : IMainHostView
    {
        private readonly IMainContract _mainContract;
        private ContractHandle _mainContractHandle;

        public MainContractToViewHostSideAdapter(IMainContract mainContract)
        {
            _mainContract = mainContract;
            _mainContractHandle = new ContractHandle(mainContract);
        }

        public FrameworkElement Run(params object[] data)
        {
            return FrameworkElementAdapters.ContractToViewAdapter(_mainContract.Run(data));
        }

        public bool Close()
        {
            return _mainContract.Close();
        }
    }
}
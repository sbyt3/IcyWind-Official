using System;
using System.AddIn.Contract;
using System.AddIn.Pipeline;
using IcyWind.AddInViews;
using IcyWind.Contract;

namespace IcyWind.AddInSideAdapter
{
    [AddInAdapter]
    public class MainViewToContractAddInSideAdapter : ContractBase, IMainContract
    {
        private readonly IMainView _view;

        public MainViewToContractAddInSideAdapter(IMainView view)
        {
            _view = view;
        }
        public virtual INativeHandleContract Run(params object[] para)
        {
            return FrameworkElementAdapters.ViewToContractAdapter(_view.Run(para));
        }

        public virtual bool Close()
        {
            return _view.Close();
        }
    }
}
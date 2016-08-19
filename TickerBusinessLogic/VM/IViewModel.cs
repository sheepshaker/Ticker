using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ticker.VM
{
    public interface IViewModel
    {
        void OnViewLoaded();
        void OnViewUnloaded();
    }
}

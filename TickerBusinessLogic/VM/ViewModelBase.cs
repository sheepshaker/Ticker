using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ticker.VM
{
    public class ViewModelBase : NotificationObject, IViewModel, IDisposable
    {
        public virtual void OnViewLoaded()
        {

        }

        public virtual void OnViewUnloaded()
        {

        }

        public virtual void Dispose()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ticker.VM
{
    public class BaseViewModel : NotificationObject, IViewModel
    {
        public virtual void OnViewLoaded()
        {

        }

        public virtual void OnViewUnloaded()
        {

        }
    }
}

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace FusoEuro5Japan_Client
{
    public abstract class BaseP : INotifyPropertyChanged
    {
        public abstract SynchronizationContext SynchronizeContext { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                SynchronizeContext.Post((o => PropertyChanged(this, new PropertyChangedEventArgs(propertyName))), null);
            }
        }
    }
}

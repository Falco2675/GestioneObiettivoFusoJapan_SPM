using System.Drawing;
using System.Threading;

namespace FusoEuro5Japan_Client
{
    public interface IMainP
    {
        IMainV GetView { get; }
        SynchronizationContext SynchronizeContext { get; set; }

        void Reset();
        //string Orario { get; }
    }
}
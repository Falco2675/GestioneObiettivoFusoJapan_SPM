using System.Threading;

namespace FusoEuro5Japan_Client
{
    public interface IMainP
    {
        IMainV GetView { get; }
        SynchronizationContext SynchronizeContext { get; set; }
    }
}
using System.Threading.Tasks;

namespace ViewLayer.Shared
{
    public interface IActivable
    {
        Task ActivateAsync(object param);
    }
}
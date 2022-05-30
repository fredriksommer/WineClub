using System.Threading.Tasks;

namespace WineClub.Contracts.Services
{
    public interface IActivationService
    {
        Task ActivateAsync(object activationArgs);
    }
}

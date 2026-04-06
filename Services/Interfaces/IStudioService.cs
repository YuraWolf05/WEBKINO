using webkino.ViewModels;

namespace webkino.Services.Interfaces
{
    public interface IStudioService
    {
        List<StudioViewModel> GetAll();

        List<StudioOptionViewModel> GetOptions();
    }
}
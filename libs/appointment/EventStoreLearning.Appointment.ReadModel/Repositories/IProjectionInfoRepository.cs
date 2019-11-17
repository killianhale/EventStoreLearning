using System.Threading.Tasks;
using EventStoreLearning.Appointment.ReadModels.Models;

namespace EventStoreLearning.Appointment.ReadModel.Repositories
{
    public interface IProjectionInfoRepository
    {
        Task<ProjectionReadModel> RetrieveProjectionInfo();
        Task UpdateProjectionInfo(ProjectionReadModel model);
    }
}
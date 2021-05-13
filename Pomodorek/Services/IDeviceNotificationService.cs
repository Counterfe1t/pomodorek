using System.Threading.Tasks;

namespace Pomodorek.Services {
    public interface IDeviceNotificationService {
        Task DisplayNotification(string message);

        Task DisplaySessionOverNotification(string message);
    }
}

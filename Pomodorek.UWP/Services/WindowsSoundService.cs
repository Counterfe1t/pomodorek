using Pomodorek.Services;
using Pomodorek.UWP.Services;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms;

[assembly: Dependency(typeof(WindowsSoundService))]
namespace Pomodorek.UWP.Services {
    public class WindowsSoundService : IDeviceSoundService {

        public async Task PlaySound(string fileName) {
            var mysong = new MediaElement();
            var folder =
                await (await Package.Current.InstalledLocation.GetFolderAsync("Assets"))
                    .GetFolderAsync("Audio");
            var file =
                await folder
                    .GetFileAsync($"{fileName}.wav");
            var stream =
                await file
                    .OpenAsync(FileAccessMode.Read);
            mysong.SetSource(stream, file.ContentType);
            mysong.Play();
        }

        public async Task PlayStartSound()
        {
            await PlaySound("timer_start");
        }
    }
}

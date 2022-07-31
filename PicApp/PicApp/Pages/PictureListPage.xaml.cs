using PicApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;

namespace PicApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PictureListPage : ContentPage
    {

        private ObservableCollection<PictureInfo> _pictureList { get; set; }
        private PictureInfo _currentPicture;

        public PictureListPage()
        {
            _pictureList = new ObservableCollection<PictureInfo>();
            InitializeComponent();
            pictureList.ItemsSource = _pictureList;
            InitializeData();
        }

        private void InitializeData()
        {
            LoadPictureList();
        }

        private void LoadPictureList()
        {
            if (Device.RuntimePlatform != Device.Android)
                return;
            string path = @"/storage/emulated/0/DCIM/Camera/";
            if (!Directory.Exists(path))
                return;
            
            DirectoryInfo dir = new DirectoryInfo(path);

            var files = dir.GetFileSystemInfos("*.jpg");

            foreach (var file in files)
            {
                _pictureList.Add(new PictureInfo(file.Name, file.FullName, file.CreationTime));
            }
        }

        private void OpenPicrureButton_Clicked(object sender, EventArgs e)
        {
            if (pictureList.SelectedItem is null)
                return;

            Navigation.PushAsync(new PicturePage(pictureList.SelectedItem as PictureInfo));
        }

        private async void RemovePictureButton_Clicked(object sender, EventArgs e)
        {
            if (pictureList.SelectedItem is null)
                return;
            PictureInfo pic = pictureList.SelectedItem as PictureInfo;
            var answer = await DisplayAlert("Внимание!", $"Удалить {pic.NameFile}", "Да", "Нет");
            
            if (answer == false)
            {
                return;
            }

            try
            {
                if (File.Exists(pic.PathToPicture))
                {
                    File.Delete(pic.PathToPicture);
                }
                _pictureList.Remove(pic);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка!", ex.Message, "OK");
            }

            
            
        }
    }
}
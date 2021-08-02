using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    public class UnitImageGalleryViewModel : BaseViewModel
    {
        public ObservableCollection<SharePointFile> ImageList { get; set; }
        private SharePointFile _currentImage;
        public SharePointFile CurrentImage
        {
            get => _currentImage;
            set
            {
                if (_currentImage != value)
                {
                    this._currentImage = value;
                    OnPropertyChanged(nameof(CurrentImage));
                }
            }
        }

        private int _noOfColumns;
        public int noOfColumns { get => _noOfColumns<1?1:_noOfColumns; set { _noOfColumns = value; OnPropertyChanged(nameof(noOfColumns)); } }

        private double _widthListView;
        public double widthListView { get => _widthListView<1?660:_widthListView; set { _widthListView = value; OnPropertyChanged(nameof(widthListView)); } }

        private double _heightListView;
        public double heightListView { get => _heightListView; set { _heightListView = value; OnPropertyChanged(nameof(heightListView)); } }

        private DisplayInfo _displayInfo;
        public DisplayInfo displayInfo { get => _displayInfo; set
            {
                if(noOfColumns == 1 || ( _displayInfo.Height != value.Height && _displayInfo.Width == value.Height))
                {
                    noOfColumns = ((int)(value.Width/value.Density)) / 120;
                    widthListView = value.Width/value.Density;
                    heightListView = value.Height / value.Density;
                    _displayInfo = value;
                    OnPropertyChanged(nameof(displayInfo));
                }
            }
        }

        public UnitImageGalleryViewModel()
        {
            IsBusy = true;
            ImageList = new ObservableCollection<SharePointFile>();

            //var list = new List<UnitImageModel>()
            //{
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/03/A-Green-Bright-Day-Wallpaper-1920x1200-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/03/Alps-Meadow-Germany-Wallpaper-1920x1200-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/03/Alps-Switzerland-Alpen-Mountains-Wallpaper-4100x2733-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/03/Amazing-Landscape-Wallpaper-1920x1200-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/03/Balloon-In-Sky-Wallpaper-1680x1050-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/03/Beautiful-Green-Landscape-Wallpaper-2560x1600-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/03/Beautiful-Mountain-Valley-Of-Flowers-Wallpaper-1920x1200-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/03/Blue-Green-Bliss-Wallpaper-1680x1050-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/03/Blue-Skies-And-Green-Pastures-Wallpaper-1600x1200-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/03/Blue-Sky-And-Green-Grass-Wallpaper-1920x1200-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/03/Bright-Day-Light-Wallpaper-1920x1200-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/03/Bright-Landscape-Wallpaper-1920x1080-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/03/Buttercups-Riverside-Wallpaper-1680x1050-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/03/Clouds-Landscapes-Nature-Trees-Grass-Wallpaper-2560x1600-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/03/Clouds-Trees-Field-Of-Grass-Beautiful-Wallpaper-4293x2522-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/03/Derelict-House-Wallpaper-2560x1600-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/03/Distance-Sun-Nature-Rays-Landscape-Wallpaper-4928x3264-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/03/Dream-Landscape-Wallpaper-2560x1600-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/03/Dream-Village-Wallpaper-1680x1050-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/03/Field-Beautiful-Distance-Wallpaper-1920x1080-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/03/Field-Grass-Landscape-Wallpaper-2048x1367-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/03/Field-Path-Wallpaper-2560x1574-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/03/Fields-Calgary-Grass-Yellow-Fence-Wallpaper-2628x1742-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/03/Grass-Sky-Light-Wallpaper-1920x1200-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/03/Big-Almaty-Lake-Wallpaper-2560x1600-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/03/Autumn-Breeze-Wallpaper-1920x1200-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/03/Bench-Autumn-River-Wallpaper-2048x1351-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/03/Autumn-Road-Wallpaper-1600x1200-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/03/Autumn-Wood-Forest-Photography-Wallpaper-2560x1600-1-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/03/Red-Autumn-Branch-Wallpaper-1920x1200-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/06/Table-Apples-Autumn-Harvest-Leaves-2000-X-1333-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/06/Trees-Autumn-Fall-Light-Sun-1920-X-1200-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/06/Trees-Leaves-Foliage-Road-Golden-Autumn-1920-X-1200-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/06/Trees-Park-Trail-Path-Fog-Autumn-Fall-1920-X-1200-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/06/Valley-Sun-Sky-Autumn-Fog-Rocks-1920-X-1200-340x220.jpg"),
            //    new UnitImageModel("https://www.setaswall.com/wp-content/uploads/2017/06/Water-Drops-Leaf-Glass-Autumn-1920-X-1200-340x220.jpg"),
            //};
            //ImageList = new ObservableCollection<UnitImageModel>(list);
        }
    }
}

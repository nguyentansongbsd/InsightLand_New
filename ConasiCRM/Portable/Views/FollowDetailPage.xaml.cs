using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Services;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FollowDetailPage : ContentPage
    {
        public FollowDetailPageViewModel viewModel;
        public Action<bool> OnLoaded;

        public FollowDetailPage(string id)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new FollowDetailPageViewModel();
            Init(id);            
        }

        private async void Init(string id)
        {
            await viewModel.Load(id);

            if (viewModel.FollowDetail.name_quote != null)
            {
                nameWork.Text = "Phiếu đặt cọc: ";
            }
            else if (viewModel.FollowDetail.name_salesorder != null)
            {
                nameWork.Text = "Hợp đồng: ";
            }

            if (viewModel.FollowDetail != null)
            {
                OnLoaded?.Invoke(true);
            }
            else
            {
                OnLoaded?.Invoke(false);
            }
        }           
    }
}
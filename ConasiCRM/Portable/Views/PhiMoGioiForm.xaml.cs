using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
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
	public partial class PhiMoGioiForm : ContentPage
	{
        public Action<bool> CheckPhiMoGioi;
        public PhiMoGioiFormViewModel viewModel;
		public PhiMoGioiForm (Guid idPMG)
		{
			InitializeComponent ();
            BindingContext = viewModel = new PhiMoGioiFormViewModel();
            viewModel.idPhiMoGioi = idPMG;
            Init();
		}

        private async void Init()
        {
            await viewModel.loadData();

            if(viewModel.PhiMoGioi != null)
            {
                labelsoluong.IsVisible = txt_soluongtu.IsVisible = viewModel.PhiMoGioi.bsd_quantityfrom == 0 ? false : true;
                labelsoluongden.IsVisible = txt_soluongden.IsVisible = viewModel.PhiMoGioi.bsd_quantityto == 0 ? false : true;
                CheckPhiMoGioi(true);
            }
                
            else
                CheckPhiMoGioi(false);
        }
    }
}
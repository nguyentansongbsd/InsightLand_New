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
    public partial class MandatorySecondaryForm : ContentPage
    {
        private MandatorySecondaryFormViewModel viewModel;
        public MandatorySecondaryForm(Guid id)
        {
            InitializeComponent();
            Init(id.ToString());
        }

        private async void Init(string id)
        {
            this.BindingContext = viewModel = new MandatorySecondaryFormViewModel();
            await viewModel.GetOneAccountById(id);
        }

        private void AddMandatorySecondary_Clicked(object sender, EventArgs e)
        {

        }
    }
}
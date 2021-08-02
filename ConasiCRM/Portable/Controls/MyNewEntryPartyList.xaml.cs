using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.DataControls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: ExportFont("FontAwesome5Solid.otf", Alias = "FontAwesomeSolid")]
namespace ConasiCRM.Portable.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MyNewEntryPartyList : ContentView
	{
		public MyNewEntryPartyList ()
		{
			InitializeComponent ();
        }
        public static readonly BindableProperty DataProperty = BindableProperty.Create(nameof(Data), typeof(List<StackLayout>), typeof(MyNewEntryPartyList), null, BindingMode.TwoWay);

        public void renderStackLayout(List<StackLayout> newValue)
        {
            this.stackLayout_Content.Children.Clear();
            foreach (var item in newValue)
            {
                this.stackLayout_Content.Children.Add(item);
            }
        }
        public List<StackLayout> Data
        {
            get { return (List<StackLayout>)GetValue(DataProperty); }
            set
            {
                SetValue(DataProperty, value);
                this.renderStackLayout(value);
            }
        }

        private void Handle_Tapped(object sender, EventArgs e)
        {

            SendOnClicked();
        }

        public event EventHandler OnClicked;
        public void SendOnClicked()
        {
            EventHandler eventHandler = this.OnClicked;
            eventHandler?.Invoke((object)this, EventArgs.Empty);
        }
        public event EventHandler OnUnFocused;
        private void sendOnUnFocused()
        {
            EventHandler eventHandler = this.OnUnFocused;
            eventHandler?.Invoke((object)this, EventArgs.Empty);
        }

        private void EntryUnfocused_Focused(object sender, FocusEventArgs e)
        {
            entryLine.Unfocus();
            Handle_Tapped(null, null);
        }
    }
}
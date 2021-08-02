using System;
using System.Collections.Generic;
using System.Text;
using Telerik.XamarinForms.DataGrid;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Controls.BsdGrid
{
    public class BsdRadDataGrid : RadDataGrid
    {
        public BsdRadDataGrid()
        {
            AutoGenerateColumns = false;
            UserGroupMode = DataGridUserGroupMode.Disabled;
            UserFilterMode = DataGridUserFilterMode.Disabled;
            GridLinesVisibility = GridLinesVisibility.Both;

            // cột đậm cột nhạt
            AlternateRowBackgroundStyle = new DataGridBorderStyle()
            {
                BackgroundColor = Color.White,
                BorderColor = Color.Black
            };

            // cột đậm cột nhạt
            RowBackgroundStyle = new DataGridBorderStyle()
            {
                BackgroundColor = Color.FromHex("#eeeeee"),
                BorderColor = Color.Black
            };
            
            // màu khi chọn
            SelectionStyle = new DataGridBorderStyle()
            {
                BorderColor = Color.Black,
                BorderThickness = 1
            };

            // load more...
            LoadOnDemandRowTemplate = new DataTemplate(() => new StackLayout());

            // load more
            LoadOnDemandRowStyle = new DataGridLoadOnDemandRowStyle()
            {
                //TextColor = Color.Transparent,
                //TextMargin = 0,
                //IndicatorAnimationColor = Color.White,
                //Text = "Load More",
                BackgroundColor = Color.Transparent,
                //IndicatorAnimationType = Telerik.XamarinForms.Primitives.AnimationType.Animation9,
                //OverlayOpacity = 0.85
            };
        }
    }
}

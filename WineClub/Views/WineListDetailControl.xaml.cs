using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WineClub.ViewModels;

namespace WineClub.Views
{
    public sealed partial class WineListDetailControl : UserControl
    {
        public AddWineViewModel ListDetailsMenuItem
        {
            get { return GetValue(ListDetailsMenuItemProperty) as AddWineViewModel; }
            set { SetValue(ListDetailsMenuItemProperty, value); }
        }

        public static readonly DependencyProperty ListDetailsMenuItemProperty = DependencyProperty.Register("ListDetailsMenuItem", typeof(AddWineViewModel), typeof(WineListDetailControl), new PropertyMetadata(null, OnListDetailsMenuItemPropertyChanged));

        public WineListDetailControl()
        {
            InitializeComponent();
        }

        private static void OnListDetailsMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WineListDetailControl control = d as WineListDetailControl;
            control.ForegroundElement.ChangeView(0, 0, 1);
        }

    }
}

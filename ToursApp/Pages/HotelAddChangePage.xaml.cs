using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToursApp.Classes;

namespace ToursApp.Pages
{
    /// <summary>
    /// Interaction logic for HotelAddChangePage.xaml
    /// </summary>
    public partial class HotelAddChangePage : Page
    {
        private Hotel _hotel { get; set; }
        public HotelAddChangePage(Hotel hotel)
        {
            InitializeComponent();
            CbCountry.ItemsSource = DbModel.init().Countries.ToList();
            this._hotel = hotel;
            EditedContext.DataContext = _hotel;
        }
        public HotelAddChangePage() {
            InitializeComponent();
            CbCountry.ItemsSource = DbModel.init().Countries.ToList();

            EditedContext.DataContext = _hotel;
        }

        private void BtSave(object sender, RoutedEventArgs e)
        {

            if (_hotel.Id == 0)
            {
                DbModel.init().Hotels.Add(new Hotel());
            }
            DbModel.init().SaveChanges();
            NavigationService.Navigate(new HotelsPage());
        }
    }
}

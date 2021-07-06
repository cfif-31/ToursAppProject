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
    /// Interaction logic for HotelsPage.xaml
    /// </summary>
    public partial class HotelsPage : Page
    {
        int recortcount=0;
        int pagecount=0;
        int curpage = 0;
        const int itemInPage = 10;
        public HotelsPage()
        {
            InitializeComponent();
            HotelGrid.ItemsSource = DbModel.init().Hotels.ToList();

        }
        private async void UpdateData() {
            StackPages.Children.Clear();
            List<Hotel> hotels = await UpdateData(TbSearch.Text);
            if (curpage > 0) {
                Button btFirst = new Button { Content = "|<" };
                Button btBack = new Button { Content = "<" };
                btFirst.Click += BtFirstClick;
                btBack.Click += BtBackClick;
                StackPages.Children.Add(btFirst);
                StackPages.Children.Add(btBack);
            }
            if (curpage < pagecount)
            {
                Button btLast = new Button { Content = ">|" };
                Button btNext = new Button { Content = ">" };
                btLast.Click += BtLastClick;
                btNext.Click += BtNextClick;
                StackPages.Children.Add(btLast);
            }
            for (int i = 0; i < pagecount + 1;i++) {
                
                Button button = new Button();
                button.Content = i.ToString();
                button.Tag = i;

                StackPages.Children.Add(button);
            }

        }
        private void BtLastClick(object sender, RoutedEventArgs e)
        {
            curpage = 0;
            UpdateData();
        }

        private void BtNextClick(object sender, RoutedEventArgs e)
        {
            curpage = 0;
            UpdateData();
        }
        private void BtFirstClick(object sender, RoutedEventArgs e)
        {
            curpage = 0;
            UpdateData();
        }

        private void BtBackClick(object sender, RoutedEventArgs e)
        {
            curpage = 0;
            UpdateData();
        }

        private async Task<List<Hotel>> UpdateData(string search) {
            return await Task.Run(() =>
            {
                IEnumerable<Hotel> hotels = DbModel.init().Hotels;
                hotels = hotels.Where(h => h.Name.Contains(search));
                recortcount = hotels.Count();
                pagecount = (int)Math.Floor((decimal)recortcount / itemInPage);
                
                return hotels.Skip(curpage * itemInPage).Take(itemInPage).ToList();
            });
            
        }

        private void Search(object sender, TextChangedEventArgs e)
        {

        }
    }
}

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
    /// Interaction logic for ToursPage.xaml
    /// </summary>
    public partial class ToursPage : Page
    {
        bool IsEventWorking = true;
        public ToursPage()
        {
            InitializeComponent();
            List<Classes.Type> types = DbModel.init().Types.ToList();
            types.Insert(0, new Classes.Type { Name = "Все типы" });
            CbType.ItemsSource = types;
            List<string> listSort = new List<string>();
            listSort.Add("По возрастанию стоимости тура");
            listSort.Add("По убыванию стоимости тура");
            CbSort.ItemsSource = listSort;

            IsEventWorking = false;
            CbType.SelectedIndex = 0;
            CbSort.SelectedIndex = 0;
            IsEventWorking = true;
            UpdateData();
        }

        private async void UpdateData() {
            List<Tour> tours = await UpdateData(TbSearch.Text, CbType.SelectedIndex == 0 ? null : CbType.SelectedItem as Classes.Type, (bool)CIsActual.IsChecked, CbSort.SelectedIndex);
            LwTours.ItemsSource = tours;
            LbSum.Content = String.Format("Общая стоимость туров {0:###,##0.00}", tours.Sum(t=>t.Price));
        }
        private async Task<List<Tour>> UpdateData(string search, Classes.Type type, bool ActualOnly, int sortBy) {
            return await Task.Run(() =>
            {
                IEnumerable<Tour> tours = DbModel.init().Tours.Where(t => t.Name.Contains(search));
                if (type != null) {
                    tours = tours.Where(t=>t.Types.Any(ty => ty.Id == type.Id));
                }
                if (ActualOnly)
                {
                    tours = tours.Where(t => t.IsActual);
                }

                if (sortBy == 0)
                {
                    tours = tours.OrderBy(t => t.Price);
                }
                else {
                    tours = tours.OrderByDescending(t => t.Price);
                }
                return tours.ToList();
            });
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            if(IsEventWorking)
                UpdateData();
        }

        private void Search(object sender, TextChangedEventArgs e)
        {
            if(IsEventWorking)
                UpdateData();
        }

        private void Search(object sender, SelectionChangedEventArgs e)
        {
            if (IsEventWorking)
                UpdateData();
        }
    }
}

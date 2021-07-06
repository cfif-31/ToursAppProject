using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
using ToursApp.Pages;
using Type = ToursApp.Classes.Type;

namespace ToursApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CultureInfo culture = new CultureInfo("en");
        private void ImportCountryes() {
            TextFieldParser textField = new TextFieldParser(@"import\Страны.csv", Encoding.UTF8);
            textField.TextFieldType = FieldType.Delimited;
            textField.SetDelimiters(",");
            HashSet<Country> countries = new HashSet<Country>();
            while (!textField.EndOfData) {
                string[] fields = textField.ReadFields();
                if (fields.Any(f => f.Length < 1))
                    continue;
                countries.Add(new Country { Code = fields[0].Trim(), Name = fields[1].Trim() });
            }
            DbModel.init().Countries.AddRange(countries);
            DbModel.init().SaveChanges();
        }

        private void ImportHotels()
        {
            TextFieldParser textField = new TextFieldParser(@"import\Otels.csv", Encoding.UTF8);
            textField.TextFieldType = FieldType.Delimited;
            textField.SetDelimiters(";");
            HashSet<Hotel> hotels = new HashSet<Hotel>();
            textField.ReadFields();
            List<Country> countries = DbModel.init().Countries.ToList();
            while (!textField.EndOfData)
            {
                string[] fields = textField.ReadFields();
                if (fields.Any(f => f.Length < 1))
                    continue;
                hotels.Add(
                    new Hotel { 
                        Name = fields[0].Trim(),
                        CountOfStars = Convert.ToInt32(fields[1]),
                        Country = countries.Where(c=>c.Name == fields[2].Trim()).First()
                    }
                );
            }
            DbModel.init().Hotels.AddRange(hotels);
            DbModel.init().SaveChanges();
        }
        private void ImportToursType()
        {
            TextFieldParser textField = new TextFieldParser(@"import\tours.csv", Encoding.UTF8);
            textField.TextFieldType = FieldType.Delimited;
            textField.SetDelimiters(";");
            HashSet<string> typesNames = new HashSet<string>();
            textField.ReadFields();
            while (!textField.EndOfData)
            {
                string[] fields = textField.ReadFields();
                if (fields.Any(f => f.Length < 1))
                    continue;
                foreach (string splited in fields[5].Split(','))
                    typesNames.Add(splited.Trim());
            }
            foreach (string typename in typesNames) {
                DbModel.init().Types.Add(new Type { Name = typename });
            }
            DbModel.init().SaveChanges();
        }
        private void ImportTours() {
            TextFieldParser textField = new TextFieldParser(@"import\tours.csv", Encoding.UTF8);
            textField.TextFieldType = FieldType.Delimited;
            textField.SetDelimiters(";");
            HashSet<Tour> tours = new HashSet<Tour>();
            textField.ReadFields();
            while (!textField.EndOfData)
            {
                string[] fields = textField.ReadFields();
                if (fields.Any(f => f.Length < 1))
                    continue;
                HashSet<Type> types = new HashSet<Type>();
                foreach (string splited in fields[5].Split(','))
                    types.Add(DbModel.init().Types.Where(t => t.Name == splited.Trim()).First());

                byte[] photos = null;
                try {
                    photos = File.ReadAllBytes(@"import\ToursPhoto\" + fields[0].Trim() + ".jpg");
                }
                catch {
                    try
                    {
                        photos = File.ReadAllBytes(@"import\ToursPhoto\" + fields[0].Trim().Replace("й", "й") + ".jpg");
                    }
                    catch
                    {
                        Console.WriteLine(fields[0].Trim());
                    }
                }

                tours.Add(
                    new Tour
                    {
                        Name = fields[0].Trim(),
                        TicketCount = Convert.ToInt32(fields[2].Trim()),
                        Price = Convert.ToDecimal(fields[3].Trim(), culture),
                        IsActual = fields[4].Trim() == "1" ? true : false,
                        Types = types,
                        ImagePreview = photos
                    }
                );
            }
            DbModel.init().Tours.AddRange(tours);
            DbModel.init().SaveChanges();
        }
        public MainWindow()
        {
            /*ImportCountryes();
            ImportHotels();
            ImportToursType();
            ImportTours();*/
            InitializeComponent();
            FrameNav.Navigate(new ToursPage());
        }

        private void HotelsMenu(object sender, RoutedEventArgs e)
        {
            FrameNav.Navigate(new HotelsPage());
        }

        private void ToursMenu(object sender, RoutedEventArgs e)
        {
            FrameNav.Navigate(new ToursPage());
        }
    }
}

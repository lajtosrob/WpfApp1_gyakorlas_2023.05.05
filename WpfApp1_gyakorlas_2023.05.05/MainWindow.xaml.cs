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
using MySqlConnector;

namespace WpfApp1_gyakorlas_2023._05._05
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<Gyarto> gyartok = new List<Gyarto>();
        public MainWindow()
        {
            InitializeComponent();
            txtKategoria.Focus();
        }

        private void btnBetolt_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection SQLkapcsolat = new MySqlConnection("datasource=127.0.0.1;port=3306;database=hardver;username=root;password=;");
            SQLkapcsolat.Open();

            string SQLselect = "SELECT gyártó, COUNT(*) AS darabSzám, MAX(ár) AS maxÁr, AVG(ár) AS Átlag FROM termékek WHERE " +
                $" kategória = '{txtKategoria.Text}'" +
                "GROUP BY gyártó;";

            MySqlCommand SQLparancs = new MySqlCommand(SQLselect, SQLkapcsolat);

            MySqlDataReader dataReader = SQLparancs.ExecuteReader();

            while (dataReader.Read())
            {
                Gyarto ujsor = new Gyarto(
                    dataReader.GetString("gyártó"),
                    dataReader.GetInt32("darabSzám"),
                    dataReader.GetInt32("maxÁr"),
                    dataReader.GetDouble("Átlag")
                    );
                gyartok.Add( ujsor );
            }
            dataReader.Close();
            SQLkapcsolat.Close();
            dgRekordok.ItemsSource = gyartok;
        }

        private void txtKategoria_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MySqlConnection SQLKapcsolat = new MySqlConnection("datasource=127.0.0.1;port=3306;database=hardver;username=root;password=;");
                SQLKapcsolat.Open();

                string SQLSelect = "SELECT gyártó, COUNT(*) AS darabSzám, " +
                    " MAX(ár) AS maxÁr, AVG(ár) AS Átlag " +
                    " FROM termékek WHERE " +
                    $"kategória = '{txtKategoria.Text}' " +
                    " GROUP BY gyártó;";

                MySqlCommand SQLParancs = new MySqlCommand(SQLSelect, SQLKapcsolat);

                MySqlDataReader dataReader = SQLParancs.ExecuteReader();

                while (dataReader.Read())
                {
                    Gyarto ujsor = new Gyarto(dataReader.GetString("gyártó"),
                        dataReader.GetInt32("darabSzám"),
                        dataReader.GetInt32("maxÁr"),
                        Convert.ToInt32(dataReader.GetDouble("Átlag"))
                        ) ;

                    gyartok.Add(ujsor);
                }
                dataReader.Close();
                SQLKapcsolat.Close();
                dgRekordok.ItemsSource = gyartok;
            }
        }
    }
}

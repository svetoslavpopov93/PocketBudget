using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
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
using PocketBudget.Models;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace PocketBudget_desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //AdventureWorksLT2008Entities dataEntities = new AdventureWorksLT2008Entities();

            InitializeComponent();
            FillDataGrid();
        }
        private void FillDataGrid()
        {

            string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;

            string CmdString = string.Empty;

            using (SqlConnection con = new SqlConnection(ConString))
            {

                CmdString = "SELECT Name, Fee FROM Bills";

                SqlCommand cmd = new SqlCommand(CmdString, con);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable("Bills");

                sda.Fill(dt);

                grdBill.ItemsSource = dt.DefaultView;

            }

        }
        //private void Window_Loaded(object sender, RoutedEventArgs e)
        //{
        //    ObjectQuery<Bill> products = dataEntities.Products;

        //    var query =
        //    from product in products
        //    where product.Color == "Red"
        //    orderby product.ListPrice
        //    select new { product.Name, product.Color, CategoryName = product.ProductCategory.Name, product.ListPrice };

        //    dataGrid1.ItemsSource = query.ToList();
        //}
    }
}

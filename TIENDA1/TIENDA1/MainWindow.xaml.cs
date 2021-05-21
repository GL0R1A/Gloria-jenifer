using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace TIENDA1
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string rutaConexion;

        public MainWindow()
        {
            InitializeComponent();

        }



      
        private void agregar_Click(object sender, RoutedEventArgs e)
        {

            string CODIGO = texcodigo.Text;
            string PRODUCTO = texproducto.Text;
            string CANTIDAD= texcantidad.Text;
            string FECHA = Convert.ToDateTime(fecha1.Text).ToString("yyyy/MM/dd");
            string PRECIO = texprecio.Text;
            
            //try
            //{
            MySqlConnection conn = new MySqlConnection("Server = localhost; Database=almacen; Port = 3306; Username = root;  password = Umg$2019;");

                conn.Open();
                var sql = $"INSERT INTO tienda values({CODIGO},'{PRODUCTO}',{CANTIDAD},'{FECHA}',{PRECIO})";
                var cmd = new MySqlCommand();
                cmd.CommandText = sql;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
                conn.Close();
            //}
            //catch (Exception)
            //{

            //    MessageBox.Show(@"LOS ARCHIVOS NO FUERON CARGADOS CORRECTAMENTE A LA BASE");
            //}


        }

        private void BtnBuscarFecha_Click(object sender, RoutedEventArgs e)
        {
            string Fecha2 = Convert.ToDateTime(fecha2.Text).ToString("yyyy/MM/dd");
            string Fecha3 = Convert.ToDateTime(fecha3.Text).ToString("yyyy/MM/dd");
            MySqlConnection conectar = new MySqlConnection("Server = localhost; Database=almacen; Port = 3306; Username = root;  password = Umg$2019;");
            conectar.Open();//{CODIGO},'{PRODUCTO}',{CANTIDAD},'{FECHA}',{PRECIO
            string query = $"Select CODIGO, PRODUCTO, CANTIDAD, FECHA, PRECIO FROM almacen.tienda where FECHA between'{fecha2}' AND '{fecha3}'";
            MySqlCommand Createcommand = new MySqlCommand(query, conectar);
            Createcommand.ExecuteNonQuery();

            MySqlDataAdapter dataAdp = new MySqlDataAdapter(Createcommand);
            DataTable dt = new DataTable("tienda");
            dataAdp.Fill(dt);
            Datosgrid.ItemsSource = dt.DefaultView;
            dataAdp.Update(dt);
            conectar.Close();
        }

        private void BtnEliminar1_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection conectar = new MySqlConnection("Server = localhost; Database=almacen; Port = 3306; Username = root;  password = Umg$2019;");
            conectar.Open();

            string query = $"Delete From almacen.tienda where CODIGO = {Txtcodigo.Text}";
            MySqlCommand Createcommand = new MySqlCommand(query, conectar);
            Createcommand.ExecuteNonQuery();

            MySqlDataAdapter dataAdp = new MySqlDataAdapter(Createcommand);
            DataTable dt = new DataTable("tienda");
            dataAdp.Fill(dt);
            Datosgrid.ItemsSource = dt.DefaultView;
            dataAdp.Update(dt);



            conectar.Close();


        }

        private void BtnActualizar_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection conectar = new MySqlConnection("Server = localhost; Database=almacen; Port = 3306; Username = root;  password = Umg$2019;");
            conectar.Open();

            string query = $"Update almacen.tienda SET PRODUCTO ='{txtActualizar.Text}' where CODIGO = {Txtcodigo.Text}";
            MySqlCommand Createcommand = new MySqlCommand(query, conectar);
            Createcommand.ExecuteNonQuery();

            MySqlDataAdapter dataAdp = new MySqlDataAdapter(Createcommand);
            DataTable dt = new DataTable("tienda");
            dataAdp.Fill(dt);
            Datosgrid.ItemsSource = dt.DefaultView;
            dataAdp.Update(dt);
            conectar.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection conectar = new MySqlConnection("Server = localhost; Database = almacen; Port = 3306; Username = root; password = Umg$2019; ");
            conectar.Open();

            string query = "Select *from almacen.tienda";
            MySqlCommand Createcommand = new MySqlCommand(query, conectar);
            Createcommand.ExecuteNonQuery();

            MySqlDataAdapter dataAdp = new MySqlDataAdapter(Createcommand);
            DataTable dt = new DataTable("tienda");
            dataAdp.Fill(dt);
            Datosgrid.ItemsSource = dt.DefaultView;
            dataAdp.Update(dt);

            conectar.Close();
        }

        private void btnDescargar_Click(object sender, RoutedEventArgs e)
        {

            SaveFileDialog guardar = new SaveFileDialog() { Filter = "Archivo CSV|*.csv" };
            if (guardar.ShowDialog() == true) ruta.Text = guardar.FileName;

            MySqlConnection conectar = new MySqlConnection("Server = localhost; Database = almacen; Port = 3306; Username = root; password = Umg$2019; ");
            conectar.Open();
            MySqlDataAdapter adptador = new MySqlDataAdapter("select *from almacen.tienda ", conectar);
            DataTable tabla = new DataTable();
            adptador.Fill(tabla);

            List<string> lineas = new List<string>(), columnas = new List<string>();
            foreach (DataColumn col in tabla.Columns) columnas.Add(col.ColumnName);
            lineas.Add(string.Join(";", columnas));

            foreach (DataRow fila in tabla.Rows)
            {
                List<string> celdas = new List<string>();
                foreach (object celda in fila.ItemArray) celdas.Add(celda.ToString());
                lineas.Add(string.Join(";", celdas));

            }
            File.WriteAllLines(ruta.Text, lineas);
            MessageBox.Show($"Archivo guardado correctamente en: {ruta} ");

            conectar.Close();
        }
    }
    }


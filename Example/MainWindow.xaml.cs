using Example.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
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

namespace Example
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string connectionString = "Data source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Data\\Persons.mdf;Integrated security=True;";
                                                                                                    // строка подключения к бд

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = AppData.db.Persons.ToList();                                     // загрузка данных из бд
        }


        // добавление
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddPerson addPerson = new AddPerson();
            addPerson.ShowDialog();
        }

        // обновление
        private void Reload_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = AppData.db.Persons.ToList();                                     // вывод всей таблицы из бд в программу
        }

        // удаление
        private void DelPerson_Click(object sender, RoutedEventArgs e)
        {
            var selItem = dataGrid.SelectedItem as Persons;
            
            if (selItem != null)
            {
                MessageBoxResult res = MessageBox.Show(
                    "Вы точно хотите удалить данные?\n" +
                    "Данное действие нельзя будет отменить.", "Подтверждение действия",
                    MessageBoxButton.YesNo, MessageBoxImage.Question
                );

                if (res == MessageBoxResult.Yes)
                {
                    try
                    {
                        AppData.db.Persons.Remove(selItem);
                        AppData.db.SaveChanges();
                        dataGrid.ItemsSource = AppData.db.Persons.ToList();
                    }
                    catch
                    {
                        MessageBox.Show(
                            "Объект не найден", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error
                        );
                    }
                }
            }

            else
            {
                MessageBox.Show(
                    "Не выбрана ни одна строка.\n" +
                    "Выберите строку для удаления и повторите попытку", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error
                );
            }
        }

        // изменение
        private void Change_Click(object sender, RoutedEventArgs e)
        {
            var selItem = dataGrid.SelectedItem as Persons;

            if (selItem != null)
            {
                AppData.db.SaveChanges();

                MessageBox.Show(
                    "Изменения сохранены.", "Уведомление",
                    MessageBoxButton.OK, MessageBoxImage.Information
                );
            }

            else
            {
                MessageBox.Show(
                    "Не выбрана ни одна строка.\n" +
                    "Для изменения данных выберите строку, измените данные и повторите попытку", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error
                );
            }
        }

        // поиск
        private void searchBut_Click(object sender, RoutedEventArgs e)
        {
            string ColumnName = searchTypeBox.Text;                                                 // объявление типа искомого значения
            string Info = searchBox.Text;                                                           // объявление искомого значения

            if (ColumnName == "")                                                                   // проверка искомого типа значения на пустоту
            {
                MessageBox.Show(
                    "Выберите тип данных для поиска.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error
                );
                return;
            }
            if (Info == "")                                                                         // проверка искомого значения на пустоту
            {
                MessageBox.Show(
                    "Введите данные для поиска.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error
                );
                return;
            }

            LoadData(ColumnName, Info);                                                             // вызов метода для загрузки значений и передача ему искомого значения и его типа
        }

        private void LoadData(string ColumnName, string Info)                                       // объявление метода с искомым значением и его типом
        {
            try
            {
                string query = $"SELECT * FROM Persons WHERE {ColumnName} LIKE @Info";              // строка с sql запросом

                using (SqlConnection connection = new SqlConnection(connectionString))              // подключение к бд с помощью строки подключения (31 строка)
                {
                    using (SqlCommand command = new SqlCommand(query, connection))                  // объявление комманды, в которую передается строка с sql запросом и строка подключения
                    {
                        command.Parameters.AddWithValue("@Info", $"%{Info}%");                      // передача введеного значения из поля ввода (searchBox) в запрос (в @Info)

                        SqlDataAdapter adapter = new SqlDataAdapter(command);                       // выполнение запроса

                        DataTable dataTable = new DataTable();                                      // объявление DataTable
                        
                        adapter.Fill(dataTable);                                                    // передача результата в DataTable

                        dataGrid.ItemsSource = dataTable.DefaultView;                               // вывод данных из DataTable в таблицу
                    }
                }
            }
            catch (Exception ex)                                                                    // показ окна ошибки при ошибках
            {
                MessageBox.Show(
                    $"Ошибка подключения. \n{ex}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error
                );
            }
        }

        // отмена поиска
        private void reCon_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = AppData.db.Persons.ToList();                                     // вывод всей таблицы из бд в программу

            searchTypeBox.SelectedIndex = 0;                                                        // очистка значения в списке типов данных
            searchBox.Clear();                                                                      // очистка значения в строке ввода информации
        }
    }
}
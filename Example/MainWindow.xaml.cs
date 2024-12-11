using Example.Data;
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

namespace Example
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = AppData.db.Persons.ToList();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddPerson addPerson = new AddPerson();
            addPerson.ShowDialog();
        }

        private void Reload_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = AppData.db.Persons.ToList();
        }

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
                    AppData.db.Persons.Remove(selItem);
                    AppData.db.SaveChanges();
                    dataGrid.ItemsSource = AppData.db.Persons.ToList();
                }
            }

            else
            {
                MessageBoxResult res = MessageBox.Show(
                    "Не выбрана ни одна строка.\n" +
                    "Выберите строку для удаления и повторите попытку", "Ошибка",
                    MessageBoxButton.YesNo, MessageBoxImage.Error
                );
            }
        }
    }
}

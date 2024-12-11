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
using System.Windows.Shapes;

namespace Example.Data
{
    /// <summary>
    /// Логика взаимодействия для AddPerson.xaml
    /// </summary>
    public partial class AddPerson : Window
    {
        public AddPerson()
        {
            InitializeComponent();
        }

        private void AddPersonButton_Click(object sender, RoutedEventArgs e)
        {
            Persons person = new Persons();


            if (int.TryParse(ageBox.Text, out int age))
            {
                person.ФИО = nameBox.Text;
                person.Возраст = age;
                person.Адрес = adresBox.Text;
                person.Трудоустроен = workBox.IsChecked;

                AppData.db.Persons.Add(person);
                AppData.db.SaveChanges();

                MessageBox.Show(
                    "Данные сохранены", "Уведомление",
                    MessageBoxButton.OK, MessageBoxImage.Information
                );

                this.Close();
            }
            else
            {
                MessageBox.Show(
                    "Введены неправильные данные.\n" +
                    "Проверьте данные на правильность и попытайтесь снова", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error
                );
            }
        }
    }
}

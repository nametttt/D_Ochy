using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MessageBox = System.Windows.MessageBox;

namespace air_project.pages
{
    /// <summary>
    /// Логика взаимодействия для AddUser.xaml
    /// </summary>
    public partial class AddUser : Page
    {
        public AddUser()
        {
            InitializeComponent();
            UpdateTable();
            
        }

        public void UpdateTable()
        {
            using (AirTicketsEntities db = new AirTicketsEntities())
            {
                idUser.Items.Clear();

                foreach (User user in db.User)
                {
                    if (user.Role == "Покупатель" || user.Role == "Администратор" || user.Role == "Бухгалтер")
                    {
                        idUser.Items.Add(user.IdUser);
                    }
                }

                datagrid.ItemsSource = null;

                var query1 = from user in db.User
                             where user.Role == "Покупатель" || user.Role == "Администратор" || user.Role == "Бухгалтер"
                             select new
                             {
                                 Номер = user.IdUser,
                                 Логин = user.Login,
                                 Фамилия = user.Surname,
                                 Имя = user.Name,
                                 Отчество = user.Patronymic,
                                 Телефон = user.Phone,
                                 Роль = user.Role
                             };

                datagrid.ItemsSource = query1.ToList();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UpdateTable();

        }

        public void AddMyUser(string role, string surname, string name, string patronymic, string email, string password)
        {
            try
            {
                using (AirTicketsEntities air = new AirTicketsEntities())
                {
                    GetHash g = new GetHash();
                    User user = new User()
                    {
                        Role = role,
                        Surname = surname,
                        Name = name,
                        Patronymic = patronymic,
                        Login = email,
                        Password = g.GetHashString(password)

                    };

                    air.User.Add(user);
                    MessageBox.Show("Пользователь успешно добавлен!", "Success!");

                    air.SaveChanges();
                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при добавлении пользователя.");
            }
        }

        public void UpdateMyUser(int userId, string role, string surname, string name, string patronymic)
        {
            try
            {
                using (AirTicketsEntities air = new AirTicketsEntities())
                {
                    var user = air.User.FirstOrDefault(u => u.IdUser == userId);
                    if (user != null)
                    {
                        user.Role = role;
                        user.Surname = surname;
                        user.Name = name;
                        user.Patronymic = patronymic;

                        air.SaveChanges();

                        MessageBox.Show("Пользователь успешно обновлен.", "Success!");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при обновлении пользователя.");
            }
        }

        private void DeleteMyUser(int UserId)
        {
            try
            {
                using (AirTicketsEntities air = new AirTicketsEntities())
                {
                    var id = air.User.FirstOrDefault(f => f.IdUser == UserId);
                    if (id != null)
                    {
                        air.User.Remove(id);
                        air.SaveChanges();
                        MessageBox.Show("Пользователь успешно удален.", "Success!");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при удалении пользователя.");
            }
        }

        private void idUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                using (AirTicketsEntities air = new AirTicketsEntities())
                {
                    if (idUser.SelectedItem != null)
                    {
                        int userId = (int)idUser.SelectedItem;
                        foreach(User user in air.User)
                        {
                            if (user.IdUser == userId)
                            {
                                Add.IsEnabled = false;
                                role.Text = user.Role;
                                Surname.Text = user.Surname;
                                CName.Text = user.Name;
                                Patronymic.Text = user.Patronymic;
                                Email.Text = user.Login;
                                Email.IsReadOnly = true;
                                Password.IsEnabled = false;
                            }
                            
                        }
                    }
                    else
                    {
                        role.Text = null;
                        Surname.Text = null;
                        CName.Text = null;
                        Patronymic.Text = null;
                        Email.Text = null;
                        Email.IsReadOnly = false;
                        Password.IsEnabled = true;
                        Add.IsEnabled = true;

                    }

                }
            }
            catch
            {
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                using (AirTicketsEntities air = new AirTicketsEntities())
                {
                    if (Surname.Text == "" || CName.Text == "" || Patronymic.Text == "" || Email.Text == "" || Password.Password == "" || role.Text == "")
                    {
                        MessageBox.Show("Заполните все данные!");
                    }
                    else
                    {

                        AddMyUser(role.Text, Surname.Text, CName.Text, Patronymic.Text, Email.Text, Password.Password);

                        Surname.Text = "";
                        CName.Text = "";
                        Patronymic.Text = "";
                        Email.Text = "";
                        Password.Password = "";
                        role.Text = "";
                        UpdateTable();

                    }
                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка!");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                using (AirTicketsEntities air = new AirTicketsEntities())
                {
                    if (Surname.Text == "" || CName.Text == "" || Patronymic.Text == "" || idUser.SelectedItem == null || role.Text == "")
                    {
                        MessageBox.Show("Заполните все данные и выберите пользователя!");
                    }
                    else
                    {
                        int id = (int)idUser.SelectedItem;

                        UpdateMyUser(id, role.Text, Surname.Text, CName.Text, Patronymic.Text);

                        role.Text = "";
                        Surname.Text = "";
                        CName.Text = "";
                        Patronymic.Text = "";
                        Email.Text = "";
                        Password.Password = "";
                        idUser.SelectedItem = null;
                        UpdateTable();
                    }
                }

            }
            catch
            {
                MessageBox.Show("Произошла ошибка!");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

            try
            {
                using (AirTicketsEntities air = new AirTicketsEntities())
                {
                    if (idUser.SelectedItem == null)
                    {
                        MessageBox.Show("Выберите пользователя для удаления!");
                    }
                    else
                    {
                        int id = (int)idUser.SelectedItem;
                        DeleteMyUser(id);
                        role.Text = "";
                        Surname.Text = "";
                        CName.Text = "";
                        Patronymic.Text = "";
                        Email.Text = "";
                        Password.Password = "";
                        idUser.SelectedItem = null;
                        UpdateTable();

                    }
                }

            }
            catch
            {
                MessageBox.Show("Произошла ошибка!");
            }
        }
    }
}

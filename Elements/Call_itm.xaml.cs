using ClassConnection;
using ClassModule;
using PhoneBook_Klimov.Pages;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PhoneBook_Klimov.Elements
{
    /// <summary>
    /// Логика взаимодействия для Call_itm.xaml
    /// </summary>
    public partial class Call_itm : UserControl
    {
        Call cal_loc;

        public Call_itm(Call _call)
        {
            InitializeComponent();
            cal_loc = _call;

            User user_loc = MainWindow.connect.users.Find(x => x.id == _call.user_id);
            if (user_loc != null)
            {
                category_call_text.Content = user_loc.fio_user;
                number_call_text.Content = "Номер телефона: " + user_loc.phone_num;
            }
            else
            {
                category_call_text.Content = "Неизвестный пользователь";
                number_call_text.Content = "Номер не найден";
            }

            if (!string.IsNullOrEmpty(_call.time_start))
            {
                time_call_text.Content = "Начало: " + _call.time_start;
            }
            else
            {
                time_call_text.Content = "Время начала не задано";
            }

            if (!string.IsNullOrEmpty(_call.time_start) && !string.IsNullOrEmpty(_call.time_end))
            {
                try
                {
                    DateTime start = DateTime.ParseExact(_call.time_start, "dd.MM.yyyy HH:mm", null);
                    DateTime end = DateTime.ParseExact(_call.time_end, "dd.MM.yyyy HH:mm", null);
                    if (end >= start)
                    {
                        TimeSpan duration = end - start;
                        time_call_text.Content += $"\nДлительность: {duration.Hours:00}:{duration.Minutes:00}:{duration.Seconds:00}";
                    }
                }
                catch
                { }
            }

            img_category_call.Source = (_call.category_call == 1)
                ? new BitmapImage(new Uri("/img/out.png", UriKind.RelativeOrAbsolute))
                : new BitmapImage(new Uri("/img/in.png", UriKind.RelativeOrAbsolute));

            border.Opacity = 0;
            var anim = new DoubleAnimation(1, TimeSpan.FromSeconds(0.3));
            border.BeginAnimation(UIElement.OpacityProperty, anim);
        }

        private void Click_redact(object sender, RoutedEventArgs e)
        {
            MainWindow.main.Anim_move(MainWindow.main.scroll_main, MainWindow.main.frame_main, MainWindow.main.frame_main, new Pages.PagesUser.Call_win(cal_loc));
        }

        private void Click_remove(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.connect.LoadData(Connection.tabels.calls);

                string query = "DELETE FROM [calls] WHERE [Код] = " + cal_loc.id.ToString() + "";
                var pc = MainWindow.connect.QueryAccess(query);
                if (pc != null)
                {
                    MainWindow.connect.LoadData(Connection.tabels.calls);
                    MessageBox.Show("Успешное удаление звонка", "Успешное", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainWindow.main.Anim_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.calls);
                }
                else
                {
                    MessageBox.Show("Запрос на удаление звонка не был обработан", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}

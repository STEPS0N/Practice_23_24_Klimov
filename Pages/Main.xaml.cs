using ClassConnection;
using ClassModule;
using PhoneBook_Klimov.Elements;
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

namespace PhoneBook_Klimov.Pages
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Page
    {

        public enum page_main
        {
            users, calls, none
        };

        public static page_main page_select;

        public Main()
        {
            InitializeComponent();

            page_select = page_main.none;
        }

        private void Click_History(object sender, RoutedEventArgs e)
        {
            if (frame_main.Visibility == Visibility.Visible)
            {
                MainWindow.main.Anim_move(MainWindow.main.frame_main, MainWindow.main.scroll_main);
            }
            if (page_select != page_main.calls)
            {
                page_select = page_main.calls;

                DoubleAnimation opgridAnimation = new DoubleAnimation();
                opgridAnimation.From = 0;
                opgridAnimation.To = 1;
                opgridAnimation.Duration = TimeSpan.FromSeconds(0.2);
                opgridAnimation.Completed += delegate
                {
                    parent.Children.Clear();
                    DoubleAnimation opgriAnimation = new DoubleAnimation();
                    opgriAnimation.From = 0;
                    opgriAnimation.To = 1;
                    opgriAnimation.Duration = TimeSpan.FromSeconds(0.2);
                    opgriAnimation.Completed += delegate
                    {
                        Dispatcher.InvokeAsync(async () =>
                        {
                            MainWindow.connect.LoadData(Connection.tabels.calls);

                            foreach (Call call_itm in MainWindow.connect.calls)
                            {
                                if (page_select == page_main.calls)
                                {
                                    parent.Children.Add(new Call_itm(call_itm));
                                    await Task.Delay(90);
                                }
                            }
                            if (page_select == page_main.calls)
                            {
                                var ff = new PagesUser.Call_win(new Call());
                                parent.Children.Add(new Add_itm(ff));
                            }
                        });
                    };
                    parent.BeginAnimation(StackPanel.OpacityProperty, opgriAnimation);
                };
                parent.BeginAnimation(StackPanel.OpacityProperty, opgridAnimation);
            }
        }

        private void Click_Phone(object sender, RoutedEventArgs e)
        {
            if (frame_main.Visibility == Visibility.Visible)
            {
                MainWindow.main.Anim_move(MainWindow.main.frame_main, MainWindow.main.scroll_main);
            }
            if (page_select != page_main.users)
            {
                page_select = page_main.users;

                DoubleAnimation opgridAnimation = new DoubleAnimation();
                opgridAnimation.From = 1;
                opgridAnimation.To = 0;
                opgridAnimation.Duration = TimeSpan.FromSeconds(0.2);
                opgridAnimation.Completed += delegate
                {
                    parent.Children.Clear();
                    DoubleAnimation opgriAnimation = new DoubleAnimation();
                    opgriAnimation.From = 0;
                    opgriAnimation.To = 1;
                    opgriAnimation.Duration = TimeSpan.FromSeconds(0.2);
                    opgriAnimation.Completed += delegate
                    {
                        Dispatcher.InvokeAsync(async () =>
                        {
                            MainWindow.connect.LoadData(Connection.tabels.users);

                            foreach (User user_itm in MainWindow.connect.users)
                            {
                                if (page_select == page_main.users)
                                {
                                    parent.Children.Add(new User_itm(user_itm));
                                    await Task.Delay(90);
                                }
                            }
                            if (page_select == page_main.users)
                            {
                                var ff = new PagesUser.User_win(new User());
                                parent.Children.Add(new Add_itm(ff));
                            }
                        });
                    };
                    parent.BeginAnimation(StackPanel.OpacityProperty, opgriAnimation);
                };
                parent.BeginAnimation(StackPanel.OpacityProperty, opgridAnimation);
            }
        }

        public void Anim_move(Control control1, Control control2, Frame frame_main = null, Page pages = null, page_main page_restart = page_main.none)
        {
            if (page_restart != page_main.none)
            {
                if (page_restart == page_main.users)
                {
                    page_select = page_main.none;
                    Click_Phone(new object(), new RoutedEventArgs());
                }
                else if (page_restart == page_main.calls)
                {
                    page_select = page_main.none;
                    Click_History(new object(), new RoutedEventArgs());
                }
            }
            else
            {
                DoubleAnimation opGridAnimation = new DoubleAnimation();
                opGridAnimation.From = 1;
                opGridAnimation.To = 0;
                opGridAnimation.Duration = TimeSpan.FromSeconds(0.3);
                opGridAnimation.Completed += delegate
                {
                    if (pages != null)
                    {
                        frame_main.Navigate(pages);
                    }

                    control1.Visibility = Visibility.Hidden;
                    control2.Visibility = Visibility.Visible;

                    DoubleAnimation opgriAnimation = new DoubleAnimation();
                    opgriAnimation.From = 0;
                    opgriAnimation.To = 1;
                    opgriAnimation.Duration = TimeSpan.FromSeconds(0.4);

                    control2.BeginAnimation(ScrollViewer.OpacityProperty, opgriAnimation);
                };

                control1.BeginAnimation(ScrollViewer.OpacityProperty, opGridAnimation);
            }
        }

        private void Click_Search_Data(object sender, RoutedEventArgs e)
        {
            if (page_select != page_main.calls)
            {
                MessageBox.Show("Фильтр доступен только в 'Истории звонков'");
                return;
            }

            if (date_start.SelectedDate == null || date_end.SelectedDate == null)
            {
                MessageBox.Show("Укажите обе даты!");
                return;
            }

            MainWindow.connect.LoadData(Connection.tabels.calls);

            var start = date_start.SelectedDate.Value.Date;
            var end = date_end.SelectedDate.Value.Date.AddDays(1).AddTicks(-1);

            parent.Children.Clear();

            foreach (var call in MainWindow.connect.calls)
            {
                if (DateTime.TryParseExact(call.time_start, "dd.MM.yyyy HH:mm", null,
                                           System.Globalization.DateTimeStyles.None, out var callDate))
                {
                    if (callDate >= start && callDate <= end)
                    {
                        parent.Children.Add(new Call_itm(call));
                    }
                }
            }

            parent.Children.Add(new Add_itm(new PagesUser.Call_win(new Call())));
        }

        private void Click_Del(object sender, RoutedEventArgs e)
        {
            if (page_select == page_main.calls)
            {
                MainWindow.connect.LoadData(ClassConnection.Connection.tabels.calls);
                MainWindow.main.Anim_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.calls);
            }

            date_start.SelectedDate = null;
            date_end.SelectedDate = null;
        }
    }
}

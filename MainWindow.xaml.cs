using System.Data;
using System.Text;
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
using ClassConnection;

namespace PhoneBook_Klimov
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Connection connect;
        
        public static Pages.Main main;

        public MainWindow()
        {
            InitializeComponent();

            connect = new Connection();

            connect.LoadData(Connection.tables.users);

            connect.LoadData(Connection.tables.calls);

            main = new Pages.Main();

            OpenPageMain();
        }
        
        public void OpenPageMain()
        {
            DoubleAnimation opgridAnimation = new DoubleAnimation();
            opgridAnimation.From = 1;
            opgridAnimation.To = 0;
            opgridAnimation.Duration = TimeSpan.FromSeconds(0.6);
            opgridAnimation.Completed += delegate
            {
                frame.Navigate(main);

                DoubleAnimation doubleAnimation = new DoubleAnimation();
                doubleAnimation.From = 0;
                doubleAnimation.To = 1;
                doubleAnimation.Duration = TimeSpan.FromSeconds(1.2);

                frame.BeginAnimation(Frame.OpacityProperty, doubleAnimation);
            };

            frame.BeginAnimation(Frame.OpacityProperty, opgridAnimation);
        }
    }
}
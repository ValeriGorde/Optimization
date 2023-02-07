using Optimization.ViewModels;
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

namespace Optimization
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();
            this.DataContext = new AuthorizationVM(this);
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            //это позволяет передать пароль во viewModel
            ((dynamic)this.DataContext).Password = ((PasswordBox)sender).Password;
        }
    }
}

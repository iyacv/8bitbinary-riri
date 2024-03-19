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
using System.Media;

namespace _8bitbinary_riri
{
    public partial class Window1 : Window
    {
        //private SoundPlayer MenuSound;

        public Window1()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            string playerName = playerNameTextBox.Text;
            MainWindow mainWindow = new MainWindow(playerName);
            mainWindow.Show();
            Close();
        }
    }
}
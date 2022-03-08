using RabbitMQSocket;
using SampleDJ;
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

namespace RadioStation
{
    public partial class MainWindow : Window
    {
        MQMusicSocket _socket;
        WAVChopper _file;
        bool _inEther = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartTranslation_Click(object sender, RoutedEventArgs e)
        {
            _socket = new MQMusicSocket("localhost", "mda");
            _socket.StartTranslation();

            _file = new WAVChopper(@"D:\Subjs\1.wav");
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (_inEther)
                _inEther = false;
            else
            {
                _inEther = true;
                Task.Run(StartTranslation);
            }
            _socket.Send(_file.GetSample());
        }

        private void StartTranslation()
        {
            while(_inEther)
            {
                _socket.Send(_file.GetSample());
                _file.WaitOne();
            }
        }
    }
}

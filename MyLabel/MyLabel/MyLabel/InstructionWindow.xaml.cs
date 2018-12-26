using System;
using System.Collections.Generic;
using System.IO;
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

namespace MyLabel
{
    /// <summary>
    /// Interaction logic for InstructionWindow.xaml
    /// </summary>
    public partial class InstructionWindow : Window
    {
        public string InstructionText { get; set; }
        public InstructionWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string text;
            using(StreamReader r = new StreamReader("..\\..\\Data\\instructions.txt"))
            {
                text = r.ReadToEnd();
            }
            TbInstructions.Text = text;
        }
    }
}

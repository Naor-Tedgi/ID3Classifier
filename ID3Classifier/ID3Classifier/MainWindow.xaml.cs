using ID3Project;
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
using System.IO;
using System.Windows.Forms;

namespace ID3Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        ID3Classifier _id3Classifier;
        TreeVis _TreeVis;
        public MainWindow()
        {

            InitializeComponent();
            Display.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// building the Moodle acording to the test  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Build_Button(object sender, RoutedEventArgs e)
        {

            try
            {
                string path = FileName.Text;
                if (FileName.Text != "Path")
                {
                    _id3Classifier = new ID3Classifier(path);
                    _id3Classifier.CreateXMLTree();
                    System.Windows.MessageBox.Show("Building Modle and Xml Tree Finish.");
                    _TreeVis = new TreeVis(path);


                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error : Please check if you insert a liugal input and try again ");

            }
        }
        /// <summary>
        /// dialog to browse to the currant data directory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Browse_Button(object sender, RoutedEventArgs e)
        {

            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            FileName.Text = dialog.SelectedPath.ToString();
        }
        /// <summary>
        /// classify a train and creat an outpot txt file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Classify_Button(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_id3Classifier == null)
                    FileName.Text = "Build Model First";
                else
                {
                    _id3Classifier.Classify();
                    System.Windows.MessageBox.Show("Classification Finish.");
                    Display.Visibility = Visibility.Visible;
                }

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error : Please check if you insert a liugal input and try again ");

            }

        }
        /// <summary>
        /// displays the id3 tree
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Display_Click(object sender, RoutedEventArgs e)
        {
            _TreeVis.Show();
        }




    }
}

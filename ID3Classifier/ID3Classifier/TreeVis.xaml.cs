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

namespace ID3Project
{
    /// <summary>
    /// The TreeVis is a component for displaying xml trees in a visual way.
    /// There is NO need to change this class. IT WORKS FINE AS IS!!!
    /// </summary>
    public partial class TreeVis : Window
    {
        /// <summary>
        /// Constructor for creating a new TreeVis window
        /// </summary>
        /// <param name="path">Path to location of the 'tree.xml' file.</param>
        public TreeVis(string path)
        {
            InitializeComponent();
            XmlDataProvider nodes = new XmlDataProvider { Source = new Uri(path + @"\tree.xml", UriKind.RelativeOrAbsolute), XPath = "Node" };
            this.tv_Tree.DataContext = nodes;
        }
    }
}

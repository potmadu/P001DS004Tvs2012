using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AvalonDock;
using System.Windows.Controls;
using System.Windows.Forms;
using Declarations;
using Declarations.Events;
using Declarations.Media;
using Declarations.Players;
using Implementation;
using System.ComponentModel;
using System.Windows.Controls.Primitives;

namespace Sample3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BuildDockingLayout();
            this.WindowState = System.Windows.WindowState.Maximized;
            System.Windows.Application.Current.Resources["ThemeDictionary"] = new ResourceDictionary();            
        }

        void BuildDockingLayout()
        {
            dockManager.Content = null;

            //TreeView dockable content
            /*
            var trv = new System.Windows.Controls.TreeView();
            trv.Items.Add(new TreeViewItem() { Header = "Item1" });
            trv.Items.Add(new TreeViewItem() { Header = "Item2" });
            trv.Items.Add(new TreeViewItem() { Header = "Item3" });
            trv.Items.Add(new TreeViewItem() { Header = "Item4" });
            ((TreeViewItem)trv.Items[0]).Items.Add(new TreeViewItem() { Header = "SubItem1" });
            ((TreeViewItem)trv.Items[0]).Items.Add(new TreeViewItem() { Header = "SubItem2" });
            ((TreeViewItem)trv.Items[1]).Items.Add(new TreeViewItem() { Header = "SubItem3" });
            ((TreeViewItem)trv.Items[2]).Items.Add(new TreeViewItem() { Header = "SubItem4" });
            var treeviewContent = new DockableContent() { Title = "Explorer", Content = trv 
             */

            //Left TreeView dockable content
            var treeviewContent_left = new DockableContent() { Title = "Explorer Info Left", Content = new System.Windows.Controls.TextBox() { Text = "Explorer Info Text", IsReadOnly = true } };
            treeviewContent_left.Show(dockManager, AnchorStyle.Left);
            treeviewContent_left.ToggleAutoHide();

            //Left TreeView dockable content
            var treeviewContent_right = new DockableContent() { Title = "Explorer Info Right", Content = new System.Windows.Controls.TextBox() { Text = "Explorer Info Text", IsReadOnly = true }, FlyoutWindowSize = new Size(500, 500) };
            treeviewContent_right.Show(dockManager, AnchorStyle.Right);
            
            /*
            treeviewContent.Show(dockManager, AnchorStyle.Bottom);
            //TextBox invo dockable content
            var treeviewInfoContent = new DockableContent() { Title = "Explorer Info", Content = new System.Windows.Controls.TextBox() { Text = "Explorer Info Text", IsReadOnly = true } };
            treeviewContent.ContainerPane.Items.Add(treeviewInfoContent);
             */

            VideoPanel videoPanel = new VideoPanel();
            videoPanel.ShowAsDocument(dockManager);

            VideoPanelTimeline videoPanelTimeline = new VideoPanelTimeline();
            videoPanelTimeline.ShowAsDocument(dockManager);

        }

        private void SetDefaultTheme(object sender, RoutedEventArgs e)
        {
            ThemeFactory.ResetTheme();
        }

        private void ChangeCustomTheme(object sender, RoutedEventArgs e)
        {
            string uri = (string)((System.Windows.Controls.MenuItem)sender).Tag;
            ThemeFactory.ChangeTheme(new Uri(uri, UriKind.RelativeOrAbsolute));
        }

        private void ChangeStandardTheme(object sender, RoutedEventArgs e)
        {
            string name = (string)((System.Windows.Controls.MenuItem)sender).Tag;
            ThemeFactory.ChangeTheme(name);
        }

        private void ChangeColor(object sender, RoutedEventArgs e)
        {
            ThemeFactory.ChangeColors((Color)ColorConverter.ConvertFromString(((System.Windows.Controls.MenuItem)sender).Header.ToString()));
        }

        
    }
}

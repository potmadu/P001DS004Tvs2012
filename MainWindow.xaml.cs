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
        DBController dbController;
        System.Windows.Controls.TreeView trv;
        VideoPanel videoPanel;

        public MainWindow()
        {
            InitializeComponent();
            dbController = new DBController();
            videoPanel = new VideoPanel();
            BuildDockingLayout();
            this.WindowState = System.Windows.WindowState.Maximized;
            System.Windows.Application.Current.Resources["ThemeDictionary"] = new ResourceDictionary();
        }

        void BuildDockingLayout()
        {
            dockManager.Content = null;

            trv = new System.Windows.Controls.TreeView();
            TreeViewItem tvitem = GetTreeView("recorded_vid", "Recorded Video", @"C:\Users\UA\Documents\Project\Displayer\P001DS004Tvs2012\Icon\Video Call-50.png");
            trv.Items.Add(tvitem);
            foreach(var video in dbController.list_video()){
                ((TreeViewItem)trv.Items[0]).Items.Add(new TreeViewItem() { Header = video });
            }
            trv.MouseDoubleClick += trv_NodeMouseDoubleClick;

            var treeviewContent_right = new DockableContent() { Title = "Video Lists", Content = trv };
            treeviewContent_right.Show(dockManager, AnchorStyle.Right);
           
            var treeviewContent_left = new DockableContent() { Title = "Device Lists", Content = new System.Windows.Controls.TextBox() { Text = "Device Lists", IsReadOnly = true } };
            treeviewContent_left.Show(dockManager, AnchorStyle.Left);
            treeviewContent_left.ToggleAutoHide();
            
            videoPanel.ShowAsDocument(dockManager);

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

        private void trv_NodeMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            string title="";

            try
            {

                System.Windows.Controls.TreeView _trv = sender as System.Windows.Controls.TreeView;

                if (_trv.SelectedItem is TreeViewItem)
                {
                    var item = _trv.SelectedItem as TreeViewItem;
                    title = item.Header.ToString();
                }
                else if (_trv.SelectedItem is string)
                {
                    title = _trv.SelectedItem.ToString();
                }

                System.Windows.Forms.MessageBox.Show("Playing "+title);
                string filename = dbController.retrieve_video(title);
                videoPanel.play_video_from_database(filename);
                
            }
            catch (Exception ef)
            {
                System.Windows.Forms.MessageBox.Show("Error.");
            }
        }

        private TreeViewItem GetTreeView(string uid, string text, string imagePath)
        {
            TreeViewItem item = new TreeViewItem();
            item.Uid = uid;
            item.IsExpanded = false;

            // create stack panel
            StackPanel stack = new StackPanel();
            stack.Orientation = System.Windows.Controls.Orientation.Horizontal;

            // create Image
            Image image = new Image();
            image.Source = new BitmapImage
                (new Uri(imagePath));
            image.Width = 16;
            image.Height = 16;
            // Label
            System.Windows.Controls.Label lbl = new System.Windows.Controls.Label();
            lbl.Content = text;


            // Add into stack
            stack.Children.Add(image);
            stack.Children.Add(lbl);

            // assign stack to header
            item.Header = stack;
            return item;
        }

    }
}

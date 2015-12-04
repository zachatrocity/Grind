using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Grind
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            ToDoListManager todoList = new ToDoListManager();
            //initalize boxes
            toDoBox.ItemsSource = todoList.toDoList;
           
            doneBox.ItemsSource = todoList.doneList;

        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof (SettingsPage));
        }

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AboutPage));
        }

        private void layoutButton_Click(object sender, RoutedEventArgs e)
        {
            //to do
        }

        private void toDoBoxPressed(object sender, PointerRoutedEventArgs e)
        {

        }

        private void doneBox_PointerPressed(object sender, PointerRoutedEventArgs e)
        {

        }
    }
}

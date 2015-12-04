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
        ToDoListManager todoList = new ToDoListManager();
        public MainPage()
        {
            this.InitializeComponent();
            
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


        private void doneBox_OnDragOver(object sender, DragEventArgs e)
        {
            var dropTarget = sender as ListView;
            if (dropTarget == null)
            {
                return;
            }

            //dropTarget.Background = listViewDragOverBackgroundBrush;
        }

        private void doneBox_OnDragLeave(object sender, DragEventArgs e)
        {
            var dropTarget = sender as ListView;
            if (dropTarget == null)
            {
                return;
            }

            dropTarget.Background = null;
        }

        private void doneBox_Drop(object sender, DragEventArgs e)
        {
            object doneItems;
            e.Data.Properties.TryGetValue("items", out doneItems);
            var doneList = doneItems as List<String>;
            foreach (var item in doneList){
                todoList.doneList.Add(item);
                todoList.toDoList.Remove(item);
            }
        }

        private void toDoBox_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            List<String> items = new List<String>();
            foreach (var str in e.Items)
            {
                items.Add(str.ToString());
            }

            e.Data.Properties.Add("items", items);
            
        }
    }
}

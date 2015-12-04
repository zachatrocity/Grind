using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;    
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
        WeatherAPI weather = new WeatherAPI();
        DispatcherTimer weatherTimer = new DispatcherTimer();
        public MainPage()
        {
            this.InitializeComponent();
            //initalize boxes
            toDoBox.ItemsSource = todoList.toDoList;
           
            doneBox.ItemsSource = todoList.doneList;

            setWeatherWidget();
        }

        private async void setWeatherWidget()
        {
            //set weather
            string temp = await weather.getCurrentTemp();
            string summary = await weather.getCurrentSummary();
            weatherWidget.Text = temp + "° - " + summary;

            //set it to update every so many minutes
            weatherTimer = new DispatcherTimer();
            weatherTimer.Tick += weatherTimer_Tick;
            weatherTimer.Interval = new TimeSpan(0, 5, 0);
            weatherTimer.Start();
        }

        async void weatherTimer_Tick(object sender, object e)
        {
            //set weather
            string temp = await weather.getCurrentTemp();
            string summary = await weather.getCurrentSummary();
            weatherWidget.Text = temp + "° - " + summary;
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


        private void donebox_OnDragOver(object sender, DragEventArgs e)
        {
            var dropTarget = sender as ListView;
            if (dropTarget == null)
            {
                return;
            }

            //DispatcherTimer timer = new DispatcherTimer();
            //timer.Interval = new System.TimeSpan(0, 0, 0, 10);
            //timer.Tick += checkFadeIn;
            //timer.Start();
            if (checkImage.Opacity == 0)
            {
                fadeIn.Begin();
            }
        }

        private void checkFadeIn(object sender, object e)
        {
            checkImage.Opacity += .1; 
        }


        private void todobox_OnDragOver(object sender, DragEventArgs e)
        {
            var dropTarget = sender as ListView;
            if (dropTarget == null)
            {
                return;
            }

            if (undoImage.Opacity == 0)
                undoFadeIn.Begin();
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

            if(checkImage.Opacity > 0)
                fadeOut.Begin();
        }

        private void box_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            List<String> items = new List<String>();
            foreach (var str in e.Items)
            {
                items.Add(str.ToString());
            }

            e.Data.Properties.Add("items", items);
            
        }

        private void toDoBox_Drop(object sender, DragEventArgs e)
        {
            object doneItems;
            e.Data.Properties.TryGetValue("items", out doneItems);
            var doneList = doneItems as List<String>;
            foreach (var item in doneList)
            {
                todoList.toDoList.Add(item);
                todoList.doneList.Remove(item);
            }

            if (undoImage.Opacity > 0)
                undoFadeOut.Begin();
        }

        private void box_DragLeave(object sender, DragEventArgs e)
        {
            var dropTarget = sender as ListView;
            if (dropTarget == null)
            {
                return;
            }

            if (undoImage.Opacity > 0)
            {
                undoFadeOut.Begin();
            }
        }

        private void doneBox_DragLeave(object sender, DragEventArgs e)
        {
            var dropTarget = sender as ListView;
            if (dropTarget == null)
            {
                return;
            }
            if (checkImage.Opacity > 0)
            {
                fadeOut.Begin();
            }
        }

        private void toDoBox_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            try
            {
                var item = ((ListView)sender).SelectedItem.ToString();
                if (item != null)
                {
                    todoList.doneList.Add(item);
                    todoList.toDoList.Remove(item);
                }
            }
            catch(NullReferenceException)
            {
                //do nothing
            }
        }


        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            todoList.toDoList.Add(addToDoTextBox.Text);
            addToDoTextBox.Text = "";
        }

        private void addToDoTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                todoList.toDoList.Add(addToDoTextBox.Text);
                addToDoTextBox.Text = "";
            }
        }
    }
}

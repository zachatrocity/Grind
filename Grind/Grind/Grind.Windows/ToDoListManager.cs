using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Grind
{
    class ToDoListManager
    {
        public ObservableCollection<string> toDoList = new ObservableCollection<string>();
        public ObservableCollection<string> doneList = new ObservableCollection<string>();

        public ToDoListManager()
        {
            toDoList.Add("Get Milk");
            toDoList.Add("Do Homework");

            doneList.Add("Read bible");
        }
    }
}

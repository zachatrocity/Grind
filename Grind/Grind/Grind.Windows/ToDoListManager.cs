using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grind
{
    class ToDoListManager
    {
        public List<string> toDoList = new List<string>();
        public List<string> doneList = new List<string>();

        public ToDoListManager()
        {
            toDoList.Add("Get Milk");
            toDoList.Add("Do Homework");

            doneList.Add("Read bible");
        }
    }
}

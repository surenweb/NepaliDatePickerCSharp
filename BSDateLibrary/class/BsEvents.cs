using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSDateLibrary
{
    //Event when datePicker value Changed 
    public class DateChangedEventArgs : EventArgs
    {
        public string NewDate { get; set; }

        public DateChangedEventArgs(string newDate)
        {
            NewDate = newDate;
        }
    }
}

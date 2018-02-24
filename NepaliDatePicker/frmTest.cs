using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BSDateLibrary;
namespace NepaliDatePickerTest
{
    public partial class frmTest : Form
    {
        public frmTest()
        {
            InitializeComponent();
        }

        private void bsDatePicker1_DateChanged(object sender, BSDateLibrary.DateChangedEventArgs e)
        {
            
            //## Get date string from EventArgs 
            txtValue.Text = e.NewDate;

            //## Get date of Date picker 
            BsDate bsDate = bsDatePicker1.Value;

        }

        private void btnSetFromBS_Click(object sender, EventArgs e)
        {
            //Setting Datepicker for specific Nepali Date  
            string exampleDate = lblExampleBs.Text; // or   "2046/12/15"; 
            BsDate bsDate = new BsDate(exampleDate);
            bsDatePicker1.Value = bsDate;

            /*  OR We can use below lines too 
            BsDate newBsDate = new BsDate(2046, 12, 15);
            bsDatePicker1.Value = newBsDate;            
             */

        }

        private void btnSetFromAD_Click(object sender, EventArgs e)
        {
            // Setting Datepicker for specific AD Date
            DateTime dateInAD = new DateTime(2017, 10, 22);

            BsDate bsDate = new BsDate(dateInAD);            
            bsDatePicker1.Value = bsDate;
        }

        private void btnADtoBS_Click(object sender, EventArgs e)
        {
            DateTime dateInAD = txtAdDate.Value;
            BsDate bsDate = new BsDate(dateInAD);
            txtBSValue.Text = bsDate.ToString(); //OUTPUT 

            //Properties of BsDate 
            txtYear.Text = bsDate.Year.ToString();
            txtMonth.Text = bsDate.Month.ToString();
            txtDay.Text = bsDate.Day.ToString();
            txtMonthName.Text = bsDate.MonthName; // Readonly Property
            DateTime innerAdDate = bsDate.EngDate;

            //Methods of BsDate 
            txtToString.Text = bsDate.ToString('-');
            txtToStringNepali.Text = bsDate.ToString('/', true);
            txtToStringLong.Text = bsDate.ToStringLong();
            txtToStringLongNepali.Text = bsDate.ToStringLong(true);
            txtToNepaliText.Text = bsDate.ToNepaliText();

        }

        private void btnBStoAD_Click(object sender, EventArgs e)
        {
            string bsDateString = bsDatePicker2.Text;
            BsDate bsDate = new BsDate(bsDateString);
            txtADValue.Text = bsDate.EngDate.ToShortDateString();
        }
    }
}

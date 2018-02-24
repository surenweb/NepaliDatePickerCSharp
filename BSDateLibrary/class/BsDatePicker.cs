// ================================================================
// ==       Nepali Date Picker Library -  BsDateLibrary - C#     ==
// ==              Febraury.2018 © Surendra Adhikari             ==
// ================================================================

using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;

namespace BSDateLibrary
{

    public partial class BsDatePicker : UserControl
    {
        #region Variables
        private readonly BsDate _ucToday = new BsDate(DateTime.Now);
        private BsDate _ucValue;

        private int _currentYear;
        private int _currentMonth;

        private  ToolStripDropDown _popCalender;

        private bool _showStaus = false ;
        private Font prev_font;

        #endregion  // Variables

        #region Overrides
        public override Size MinimumSize
        {
            get { return new Size(txtDate.Size.Width + btnPick.Width, txtDate.Size.Height); } //  return new Size(MinWidth, MinHeight); }
            set { } // this.MinimumSize = new Size(value.Width, minHeight); }
        }

        public override Size MaximumSize
        {
            get { return new Size(txtDate.Size.Width + btnPick.Width, txtDate.Size.Height); } //  return new Size(MinWidth, MinHeight); }
            set { } // this.MinimumSize = new Size(value.Width, minHeight); }
        }

        #endregion  // Overrides

        #region Events
        //Delegate that will be the pointer for the event, contains two arguments        
        public delegate void DateChangedEventHandler(object sender, DateChangedEventArgs e);

        // Declare an event
        [Category("Action")]
        [Description("Fires when the Datepicker value is changed")]
        public event DateChangedEventHandler DateChanged;
        protected virtual void OnDateChanged(DateChangedEventArgs e)
        {            
            if (DateChanged != null)
                DateChanged(this, e); // Raise the event
        }

        #endregion  // Events 

        #region Properties
        public override string Text
        {
            get { return txtDate.Text; }
            set { txtDate.Text = value; }
        } 

        public BsDate Value
        {
            get { return _ucValue ; }
            set
            {
                if (value.Status)
                {
                    _ucValue = value;
                    //Date Changed Event 
                   
                }
                else
                {
                    _ucValue = new BsDate(DateTime.Now);
                }
                txtDate.Text = _ucValue.ToString();
                OnDateChanged(new DateChangedEventArgs(_ucValue.ToString())); // Fire Event 
            }
        }

        #endregion  // Properties

        public BsDatePicker()
        {
            InitializeComponent();
            Setup();
        }

        #region Methods
        private void Setup()
        {
            Value = new BsDate(DateTime.Now);
            //Calender panel is of 235*205
           
            this.Height = txtDate.Height;
            pnlDateBox.Dock = DockStyle.Fill;
            prev_font = this.Font;
      
            
        }
        private void AddItemOnList()
        {
            for (int i = 2000; i < 2101; i++)
            {
                cmbYear.Items.Add(i);
            }
            for (int j = 1; j < 13; j++)
            {
                cmbMonth.Items.Add(BsDate.GetMonthNameByValue(j));
            }

            cmbMonth.MaxDropDownItems = 8;
            cmbYear.MaxDropDownItems = 8;
        }
        private void MakeCalender(int nepYear, int nepMonth)
        {
            //List<BsCalenderRow> foundDateRows =
            //    BsCalender.AllDateRows.Where(x => x.Year == nepYear && x.Month == nepMonth).ToList();

            DataRow[] foundDateRows = BsCalender.TableDate.Select("year=" + nepYear + " and month=" + nepMonth);

            if (foundDateRows.Length > 1)
            {
                MessageBox.Show(" Found more then 1 date In Nepali");
                return;
            }
            if (foundDateRows.Length == 0)
            {
                MessageBox.Show(" No Nepali Date Found For Calender ");
                return;
            }

            DataRow ucRow = foundDateRows.FirstOrDefault();

            Debug.Assert(ucRow != null, "mcRow != null");

            DayOfWeek weekDayOfFirstDay = Convert.ToDateTime(ucRow["EngStart"]).DayOfWeek;

            var weekDayStart = (int)weekDayOfFirstDay; // assign placeOfFirstDay, 0=sunday,1=monday

            //  int startPosition = weekDayStart + 1;        // it means 01 gate => d05
            int totDays = (int)ucRow["TotDays"];        //  1 to 31 gate
            int valueAtLastLabel = 35 - weekDayStart;   // d35 has value == 

            foreach (Control ctrl in pnlDateArray.Controls)
            {
                int dayToDisplay = 0;

                if (ctrl.GetType() != typeof(Label))
                {
                    return;
                }

                var runLabel = ctrl as Label;
                Debug.Assert(runLabel != null, "runLabel != null");
                var labelPosition = Convert.ToInt32(runLabel.Name.Substring(1, runLabel.Name.Length - 1));

                if (labelPosition > weekDayStart)
                {
                    dayToDisplay = labelPosition - weekDayStart;
                }

                else if (valueAtLastLabel < totDays && totDays - valueAtLastLabel >= labelPosition)
                {
                    dayToDisplay = valueAtLastLabel + labelPosition;
                }
                else
                {
                    dayToDisplay = 0;
                }

                if (dayToDisplay == 0 || dayToDisplay > totDays)
                {
                    runLabel.Text = "";
                }
                else if (dayToDisplay < 10)
                {
                    runLabel.Text = @" " + dayToDisplay.ToString();
                }
                else
                {
                    runLabel.Text = dayToDisplay.ToString();
                }


                // HighLight 2Day                
                if (_ucToday.Month == _currentMonth && _ucToday.Year == _currentYear)
                {
                    if (dayToDisplay == _ucToday.Day)
                    {
                        pnlDayToday.Location = new Point(runLabel.Location.X - 3, runLabel.Location.Y - 3);
                    }
                }
                else
                {
                    pnlDayToday.Location = new Point(-100, -100);
                }
            }
            _currentMonth = nepMonth;
        }
        private void ShowHideCalender(bool isShow)
        {
            if (isShow)
            {
                _popCalender.Show(Parent, new Point(Location.X, Location.Y + this.Height - 2)); // anchored from left (fix for border)                             
            }
            else
            {
                _popCalender.Hide();
            }
        }

        #endregion // Methods"

        #region UserControl Events
        private void BsDatePicker_Load(object sender, EventArgs e)        {
           
            AddItemOnList();
            
            _popCalender = new ToolStripDropDown
            {               
                //MaximumSize = new Size(pnlDown.Width, pnlDown.Height),
                //MinimumSize = new Size(pnlDown.Width, pnlDown.Height),                
                Margin = new Padding(0),
                Padding = new Padding(0),  
            };


            var host = new ToolStripControlHost(pnlDown)
            {
                Margin = new Padding(0),   
            };

            _popCalender.Items.Add(host);
            //Events 
            _popCalender.Opened += popCalender_Opened;
            _popCalender.Closed += popCalender_Closed;           
            // host.Dock = DockStyle.Fill;
            //_popCalender.SetBounds(0, 0, txtDate.Width + btnPick.Width, pnlDown.Height);
        }

        private void cmb_changed(object sender, EventArgs e)
        {
            if (cmbYear.SelectedIndex == -1 || cmbMonth.SelectedIndex == -1)
                return;

            _currentMonth = Convert.ToInt32(cmbMonth.SelectedIndex + 1);
            _currentYear = Convert.ToInt32(cmbYear.Text);

            MakeCalender(Convert.ToInt32(cmbYear.Text), cmbMonth.SelectedIndex + 1);
        }
                
        private void popCalender_Opened(object sender,EventArgs e)
        {
            _showStaus = true;
        }
        private void popCalender_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            _showStaus = false;
        }

        #endregion //UserControl Events

        #region btnPickEvents
        private void btnPick_Click(object sender, EventArgs e)
        {
            lblToday.Text = new BsDate(DateTime.Now).ToString();

            _currentYear = _ucValue.Year;
            _currentMonth = _ucValue.Month;

            cmbYear.SelectedItem = _ucValue.Year;
            cmbMonth.SelectedItem = _ucValue.MonthName;

            if (!_showStaus)
                ShowHideCalender(true); // Else will be auto because of tool tip closes on any click 
                         
        }

        private void btnPick_MouseEnter(object sender, EventArgs e)
        {
            btnPick.BackColor = Color.LightSkyBlue;
        }

        private void btnPick_MouseLeave(object sender, EventArgs e)
        {
            btnPick.BackColor = Color.White;
        }

        #endregion //btnPickEvents 

        #region Label Click Events
        private void day_Click(object sender, EventArgs e)
        {
            string m = _currentMonth.ToString();
            string d = ((Label) sender).Text.Trim();

            if (d == "")
                return;

            if (m.Length < 2)
            {
                m = "0" + m;
            }
            if (d.Length < 2)
            {
                d = "0" + d;
            }
            txtDate.Text = cmbYear.SelectedItem + @"/" + m + @"/" + d;
            Value = new BsDate(txtDate.Text);            
            // hide calender
            ShowHideCalender(false);
        }

        private void lblToday_click(object sender, EventArgs e)
        {
            Value= new BsDate( DateTime.Now);           
            txtDate.Text = _ucValue.ToString();
            //close calender
            ShowHideCalender(false);
        }
               
        private void lblMonthNext_Click(object sender, EventArgs e)
        {
            if (_currentMonth == 12)
            {
                cmbYear.SelectedItem = Convert.ToInt32(cmbYear.Text) + 1;
                cmbMonth.SelectedItem = BsDate.GetMonthNameByValue(1);
            }
            else
            {
                cmbMonth.SelectedItem = BsDate.GetMonthNameByValue(_currentMonth + 1);
            }
        }

        private void lblMonthPrev_Click(object sender, EventArgs e)
        {
            if (_currentMonth == 1)
            {
                cmbYear.SelectedItem = Convert.ToInt32(cmbYear.Text) + -1;
                cmbMonth.SelectedItem = BsDate.GetMonthNameByValue(12);
            }
            else
            {
                cmbMonth.SelectedItem = BsDate.GetMonthNameByValue(_currentMonth - 1);
            }
        }

        private void nepDatePicker_Leave(object sender, EventArgs e)
        {
            ShowHideCalender(false);
        }      

        private void txtDate_Leave(object sender, EventArgs e)
        {
            Value = new BsDate(txtDate.Text);
        }
        #endregion

        #region "mouseEnterLeaveColor"
        private void DayMouseEnter(object sender, EventArgs e)
        {
            prev_font = ((Label)sender).Font;
            pnlDayHover.Location = new Point(((Label)sender).Location.X - 3, ((Label)sender).Location.Y - 3);
            ((Label)sender).Font = new Font(((Label)sender).Font.Name, ((Label)sender).Font.Size, FontStyle.Underline);
            pnlDayHover.Visible = true;
        }
      
        private void DayMouseLeave(object sender, EventArgs e)
        {
            pnlDayHover.Visible = false;
            //((Label) sender).Font = new Font(((Label) sender).Font.Name, ((Label) sender).Font.Size, FontStyle.Regular);
            ((Label)sender).Font = prev_font;
        }

        private void monthNav_MouseEnter(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = Color.SteelBlue;
            ((Button)sender).ForeColor = Color.White;
        }

        private void MonthPrev_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = Color.Transparent;
            ((Button)sender).ForeColor = Color.SteelBlue;
        }

        private void lblToday_MouseEnter(object sender, EventArgs e)
        {
            ((Label)sender).BackColor = Color.SteelBlue;
            ((Label)sender).ForeColor = Color.White;
        }

        private void lblToday_MouseLeave(object sender, EventArgs e)
        {
            ((Label)sender).BackColor = Color.Transparent;
            ((Label)sender).ForeColor = Color.SteelBlue;
        }
        
        #endregion  //mouseEnterLeaveColor 
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BSDateLibrary
{
    public class BsDate
    {
        #region Variables
        private int _day;
        private int _month;
        private int _year;
        private DateTime _engDate;

        private bool _status;
        private string _statusMessage = "";
        #endregion //Variables

        #region Constructors

        public BsDate(int yyyy, int mm, int dd)
        {
            _status = false;
            _day = dd;
            _month = mm;
            _year = yyyy;
            SetAd();
        }

        public BsDate(string yyyymmdd)
        {
            if (yyyymmdd.Trim().Length != 10)
            {
                _status = false;
                _statusMessage = "Date should be in format YYYY/MM/DD ";
                return;
            }

            string[] arrDate = yyyymmdd.Trim().Split('/').ToArray();

            if (arrDate.Length != 3)
            {
                _status = false;
                _statusMessage = "Date should be in format YYYY/MM/DD ";
                return;
            }
            if (arrDate[0].Length != 4 || arrDate[1].Length != 2 || arrDate[2].Length != 2)
            {
                _status = false;
                _statusMessage = "Date should be in format YYYY/MM/DD ";
            }

            int y, m, d;

            try
            {
                y = Convert.ToInt32(arrDate[0]);
                m = Convert.ToInt32(arrDate[1]);
                d = Convert.ToInt32(arrDate[2]);
            }
            catch
            {
                _status = false;
                _statusMessage = " Date must be numeric ";
                return;
            }
            if (y < 2000 || y > 2100 || m > 12 || d > 32)
            {
                _status = false;
                _statusMessage = " Date out of range ";
                return;
            }

            _year = y;
            _month = m;
            _day = d;
            SetAd();
        }

        public BsDate(DateTime adDate)
        {
            EngDate = adDate;
            SetBs();
        }

        #endregion

        #region Properties

        public int Day
        {
            get { return _day; } // _nepDate.Substring(7, 2));}
            set
            {
                _day = value;
                SetAd();
            }
        }

        public int Month
        {
            get { return _month; } // _nepDate.Substring(5, 2));}
            set
            {
                _month = value;
                SetAd();
            }
        }

        public int Year
        {
            get { return _year; } // _nepDate.Substring(0, 4)); }
            set
            {
                _year = value;
                SetAd();
            }
        }     
       
       

        //Readonly
        public bool Status
        {
            get { return _status; }
        }

        public string StatusMessage
        {
            get { return _statusMessage; }
        }

        public string MonthName
        {
            get { return GetMonthNameByValue(_month); }
        }

        public DateTime EngDate
        {
            get { return _engDate; }
            private set
            {
                _engDate = value;
                SetBs();
            }
        }

        #endregion

        #region Public Methods 
        public override string ToString()
        {
            return ToString();
        }
        public string ToString(char dateSep='/',bool inNepali=false)
        {
            string y = _year.ToString();
            string m = _month.ToString();
            string d = _day.ToString();

            if (_month < 10)
                m = "0" + m;

            if (_day < 10)
                d = "0" + d;

            y = (inNepali) ? ToUnicode(y) : y;
            m = (inNepali) ? ToUnicode(m) : m;
            d = (inNepali) ? ToUnicode(d) : d;

            return y + dateSep + m + dateSep + d;
        }

        public static string GetMonthNameByValue(int mVal,bool inNepali=true)
        {
            switch (mVal) // should be of unicode charecter later
            {
                case 1:
                    return (inNepali) ?  "बैशाख": "Baisakh";
                case 2:
                    return (inNepali) ?  "जेठ": "Jestha";
                case 3:
                    return (inNepali) ? "आशाढ": "Aashadh";
                case 4:
                    return (inNepali) ?  "श्रावण": "Shrawan";
                case 5:
                    return (inNepali) ?  "भाद्र": "Bhadra";
                case 6:
                    return (inNepali) ?  "आश्विन": "Aashwin";
                case 7:
                    return (inNepali) ? "कार्तिक": "Kartik";
                case 8:
                    return (inNepali) ?  "मंशिर": "Margashir";
                case 9:
                    return (inNepali) ?  "पौष": "Poush";
                case 10:
                    return (inNepali) ? "माघ": "Magh";
                case 11:
                    return (inNepali) ? "फाल्गुन": "Falgun";
                case 12:
                    return (inNepali) ? "चैत्र": "Chaitra";
                default:
                    return "Error";
            }
        }

       

        public string ToStringLong(bool inNepali=false)
        {
            string y = _year.ToString();
            string m = _month.ToString();            
            string d = _day.ToString();
            if (_day < 10)
                d = "0" + d;

            d = (inNepali) ? ToUnicode(d) : d;
            y = (inNepali) ? ToUnicode(y) : y;
            string mn = GetMonthNameByValue(_month, inNepali);
            string wd = (inNepali)? GetDayNameNepali(_engDate.DayOfWeek): _engDate.DayOfWeek.ToString();

            return wd + " ," + mn + " " + d + " ," + y;  
                                
        }

        public string ToNepaliText()
        {
            string y = _year.ToString();
            string m = _month.ToString();
            string d = _day.ToString();
            if (_day < 10)
                d = "0" + d;

            d =  ToUnicode(d) ;
            y =  ToUnicode(y) ;
            string mn = GetMonthNameByValue(_month,true);
            string wd = GetDayNameNepali(_engDate.DayOfWeek);

            return y + " साल "+ mn + " " + d + " गते " + wd ;

        }

        public BsDate AddDays(int noOfDay) // overload to asign to another date
        {
            BsDate newBs = new BsDate(EngDate);
            newBs.EngDate = newBs._engDate.AddDays(noOfDay);
            return newBs;
        }

        public BsDate AddMonth(int noOfMonth)
        {
            BsDate newBs = new BsDate(EngDate);

            int goingMonth = newBs._month + noOfMonth;

            if (goingMonth > 12)
            {
                int yToAdd = goingMonth / 12;
                newBs._year = newBs._year + yToAdd;

                goingMonth = goingMonth % 12 + 1;
            }
            newBs.Month = goingMonth;

            return newBs;
        }

        #endregion

        #region PrivateMethods

        //##Private
        private void SetAd()
        {
            // DataRow[] foundDateRows = BsCalender.TableDate.Select("");  // Condition x => x.Year == _year && x.Month == _month

            IEnumerable<DataRow> foundDateRows = from row in BsCalender.TableDate.AsEnumerable()
                      where row.Field<int>("year") ==_year &&
                      row.Field<int>("month") == _month
                      select row;
              

            //if (foundDateRows.Count() != 1)
            //{
            //    _status = false;
            //    _statusMessage = " this BS Date cant be converted to AD .(only from Bs 2000 to 2100 is availeble)";
            //    return;
            //}

            DataRow dateRow = foundDateRows.FirstOrDefault();

            if (dateRow == null)
            {
                _status = false;
                _statusMessage = " this BS Date cant be converted to AD .(only from Bs 2000 to 2100 is availeble)";
                return;
            }

            int plusdays = Convert.ToInt32(Day) - 1;
            DateTime englishStart =Convert.ToDateTime(dateRow["engStart"]);  // dateRow["engStart"].ToString());
            _engDate = englishStart.AddDays(plusdays);
            _status = true;
        }

        private void SetBs()
        {
          
            IEnumerable<DataRow> foundDateRows = from row in BsCalender.TableDate.AsEnumerable()
                                                 where row.Field<DateTime>("engStart") <= EngDate.Date &&
                                                 row.Field<DateTime>("engEnd") >= EngDate.Date
                                                 select row;
            //Working Another
            //string fil = "engStart <=#" + EngDate.Date + "# AND engEnd >=#" + EngDate.Date + "#";
            //DataRow[] FoRows = BsCalender.TableDate.Select("engStart <#" + EngDate.Date + "# AND engEnd >#" + EngDate.Date +"#");


            //if (FoRows.Count != 1)
            //{
            //    _status = false;
            //    _statusMessage = " this AD Date cant be converted to Bs .(only from Bs 2000 to 2100 is availeble)";
            //    return;
            //}

            DataRow dateRow = foundDateRows.FirstOrDefault();

            if (dateRow == null)
            {
                _status = false;
                _statusMessage = " this AD Date cant be converted to Bs .(only from Bs 2000 to 2100 is availeble)";
                return;
            }

            int plusDays = EngDate.Subtract(Convert.ToDateTime(dateRow["EngStart"])).Days;
            string[] arrNepStart = dateRow["nepStart"].ToString().Split('/').ToArray();

            try
            {
                _year = Convert.ToInt32(arrNepStart[0]);
                _month = Convert.ToInt32(arrNepStart[1]);
                _day = plusDays + 1 ;  // Nepali Start From 1 gate
                _status = true;
            }
            catch
            {
                _status = false;
                _statusMessage = "Date must be numeric";
            }
        }

        private static string GetDayNameNepali(DayOfWeek dayName)
        {
            switch (dayName) // keep unicode charecter later
            {
                case DayOfWeek.Sunday: return "आईतबार";
                case DayOfWeek.Monday: return "सोमबार";
                case DayOfWeek.Tuesday: return "मंगलबार";
                case DayOfWeek.Wednesday: return "बुधबार";
                case DayOfWeek.Thursday: return "बिहिबार";
                case DayOfWeek.Friday: return "शुक्रबार";
                case DayOfWeek.Saturday: return "सनिबार";
                default:
                    return "Error";
            }
        }
        private static string ToUnicode(object n)
        {
            //echo $n;
            if (n == null)
                return "";
            string str = n.ToString();
            if (str == "" || str == "0" || str == "")
                return "";

            str = str.Replace('-', '-')
                .Replace('/', '/')
                .Replace('.', '.')
                .Replace('0', '०')
                .Replace('1', '१')
                .Replace('2', '२')
                .Replace('3', '३')
                .Replace('4', '४')
                .Replace('5', '५')
                .Replace('6', '६')
                .Replace('7', '७')
                .Replace('8', '८')
                .Replace('9', '९');
            return str;
        }

        #endregion //PrivateMethods


    }
}

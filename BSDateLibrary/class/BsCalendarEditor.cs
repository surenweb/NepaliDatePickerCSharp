using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace BSDateLibrary
{
    public partial class BsCalendarEditor : Form
    {

        public BsCalendarEditor()
        {
            InitializeComponent();
            AddItemOnList();
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

        private void BsCalenderEditor_Load(object sender, EventArgs e)
        {
            IEnumerable<DataRow> fdr1 = from row in BsCalender.TableDate.AsEnumerable()
                                                 where row.Field<DateTime>("engStart") <= DateTime.Now.Date &&
                                                 row.Field<DateTime>("engEnd") >= DateTime.Now.Date
                                                 select row;
            IEnumerable<DataRow> fdr2 = from row in BsCalender.TableDate.AsEnumerable()
                                        where row.Field<DateTime>("engStart") <= DateTime.Now.Date.AddDays(20) &&
                                        row.Field<DateTime>("engEnd") >= DateTime.Now.Date.AddDays(18)
                                        select row;

            DataRow dateRow = fdr1.FirstOrDefault();

            if (dateRow == null)
            {
                dateRow = fdr2.FirstOrDefault();
            }

            Debug.Assert(dateRow!=null);

            // ## Set Values
            cmbYear.Text = dateRow["year"].ToString();
            cmbMonth.SelectedIndex = Convert.ToInt32(dateRow["month"])-1;
            txtTotDays.Text = dateRow["totDays"].ToString();
            txtEngStart.Value=  Convert.ToDateTime(dateRow["engStart"]) ;
            txtEngEnd.Value = Convert.ToDateTime(dateRow["engEnd"]) ;

            checkRefresh();
        }

        private void checkRefresh()
        {
            txtNepStart.Text = cmbYear.Text + "/" + (cmbMonth.SelectedIndex+1) + "/01";
            txtNepEnd.Text = cmbYear.Text + "/" + (cmbMonth.SelectedIndex + 1) + "/" + txtTotDays.Text;
            lblError.Text = "";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }


   
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppointmentSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Get the date, time, and name from the controls
            string date = dateTimePicker1.Value.ToString("dd/MM/yyyy");
            string time = comboBox1.SelectedItem.ToString() + ":" + comboBox2.SelectedItem.ToString();
            string name = textBox1.Text;
            // Check if all fields have been entered
            if (string.IsNullOrEmpty(name) || comboBox1.SelectedIndex == -1 || comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Please enter all fields.");
                return;
            }
            // Check if the date and time are valid
            if (!CheckLegalDate(dateTimePicker1.Value, time))
            {
                MessageBox.Show("The date or time is not valid.");
                return;
            }
            // Check for clashes
            if (CheckForClashes(date, time))
            {
                MessageBox.Show("This timeslot is already booked. Please choose another time.");
                return;
            }
            // Add the appointment
            listBox1.Items.Add(date);
            listBox2.Items.Add(time);
            listBox3.Items.Add(name);
        }
        private bool CheckLegalDate(DateTime date, string time)
        {
            // Check if the date is a weekday
            if ((date.DayOfWeek == DayOfWeek.Saturday) || (date.DayOfWeek == DayOfWeek.Sunday))
            {
                return false;
            }
            // Check if the time is within working hours
            int hour = int.Parse(time.Split(':')[0]);
            if ((hour < 9) || (hour > 16) || (hour == 16 && time.Split(':')[1] != "00"))
            {
                return false;
            }
            // Check if the date and time are in the future
            if ((date.Date == DateTime.Today) && (string.Compare(time, DateTime.Now.ToString("HH:mm")) <= 0))
            {
                return false;
            }
            return true;
        }
        private bool CheckForClashes(string date, string time)
        {
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                if ((listBox1.Items[i].ToString() == date) && (listBox2.Items[i].ToString() == time))
                {
                    return true;
                }
            }
            return false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Update the status bar with the current time
            label2.Text = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss tt");
            // Check if there is a meeting currently
            string date = DateTime.Today.ToString("dd/MM/yyyy");
            string time = DateTime.Now.ToString("HH:mm");
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                if (listBox1.Items[i].ToString() == date && listBox2.Items[i].ToString() == time)
                {
                    label3.Text = "Currently meeting with " + listBox3.Items[i].ToString();
                    return;
                }
            }
            label3.Text = "No meeting currently";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dateTimePicker1.MinDate = DateTime.Today;
            dateTimePicker1.MaxDate = DateTime.Today.AddDays(7);
        }
    }
}

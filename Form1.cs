using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Management;

namespace USBPortController
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            buttonCheckDevices.Click += buttonCheckDevices_Click;
        }

        private void buttonCheckDevices_Click(object sender, EventArgs e)
        {
            LoadUSBDevices();
        }

        private void LoadUSBDevices()
        {
            try
            {
                listBox1.Items.Clear();

                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_USBHub");

                foreach (ManagementObject usbDevice in searcher.Get())
                {
                    string deviceName = usbDevice["Caption"].ToString();
                    listBox1.Items.Add(deviceName);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonEnable_Click(object sender, EventArgs e)
        {
            EnableUSBDevice();
        }

        private void buttonDisable_Click(object sender, EventArgs e)
        {
            DisableUSBDevice();
        }

        private void EnableUSBDevice()
        {
            if (listBox1.SelectedIndex >= 0)
            {
                string selectedDevice = listBox1.SelectedItem.ToString();
                string deviceName = selectedDevice.Split('-')[0].Trim();
                ProcessStartInfo startInfo = new ProcessStartInfo("\"C:\\Program Files (x86)\\Windows Kits\\10\\Tools\\10.0.22621.0\\x64\\devcon.exe\"", $"enable \"{deviceName}\"");
                startInfo.Verb = "runas";
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;

                using (Process process = new Process())
                {
                    process.StartInfo = startInfo;
                    process.Start();
                    process.WaitForExit();
                }

                MessageBox.Show("USB cihazı başarıyla etkinleştirildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Lütfen etkinleştirmek istediğiniz USB cihazını seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DisableUSBDevice()
        {
            if (listBox1.SelectedIndex >= 0)
            {
                string selectedDevice = listBox1.SelectedItem.ToString();
                string deviceName = selectedDevice.Split('-')[0].Trim();
                ProcessStartInfo startInfo = new ProcessStartInfo("\"C:\\Program Files (x86)\\Windows Kits\\10\\Tools\\10.0.22621.0\\x64\\devcon.exe\"", $"disable \"{deviceName}\"");
                startInfo.Verb = "runas";
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;

                using (Process process = new Process())
                {
                    process.StartInfo = startInfo;
                    process.Start();
                    process.WaitForExit();
                }

                MessageBox.Show("USB cihazı başarıyla devre dışı bırakıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Lütfen devre dışı bırakmak istediğiniz USB cihazını seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}

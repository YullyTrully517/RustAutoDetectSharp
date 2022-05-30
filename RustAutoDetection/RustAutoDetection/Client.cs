using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RustAutoDetection
{
    public partial class Client : Form
    {
        private bool isEnabled { get; set; } = true;
        public Client()
        {
            InitializeComponent();
        }
        [DllImport("DwmApi")] //System.Runtime.InteropServices
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);

        protected override void OnHandleCreated(EventArgs e)
        {
            if (DwmSetWindowAttribute(Handle, 19, new[] { 1 }, 4) != 0)
                DwmSetWindowAttribute(Handle, 20, new[] { 1 }, 4);
        }
        private async void Client_Load(object sender, EventArgs e)
        {
            //Fix Flickering
            this.DoubleBuffered = false;

            //Putting the weapon thread function on a thread 0-0 ez
            await Task.Run(() => WeaponThread());
        }

        private void WeaponThread()
        {
            while(isEnabled)
            {
                CurrentWeaponLabel.Text = Detect.ReturnWeaponLoop();
            }
        }
    }
}

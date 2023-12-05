using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MemoryLeak_AxHost
{
    public partial class Form1 : Form
    {

        private AxPDFXCviewAxLib.AxCoPDFXCview axCopdfxCview1;
        private readonly List<WeakReference> refs = new List<WeakReference>();

        public Form1()
        {
            this.InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.axCopdfxCview1 == null)
            {
                var resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
                this.axCopdfxCview1 = new AxPDFXCviewAxLib.AxCoPDFXCview();
                ((System.ComponentModel.ISupportInitialize)this.axCopdfxCview1).BeginInit();
                this.axCopdfxCview1.Enabled = true;
                this.axCopdfxCview1.Location = new Point(275, 222);
                this.axCopdfxCview1.Name = "axCopdfxCview1";
                this.axCopdfxCview1.OcxState = (AxHost.State)resources.GetObject("axCopdfxCview1.OcxState");
                this.axCopdfxCview1.Size = new Size(192, 192);
                this.axCopdfxCview1.TabIndex = 2;

                this.Controls.Add(this.axCopdfxCview1);
                ((System.ComponentModel.ISupportInitialize)this.axCopdfxCview1).EndInit();
                this.refs.Add(new WeakReference(this.axCopdfxCview1));
            }
            else
            {
                this.Controls.Remove(this.axCopdfxCview1);
                //this.axCopdfxCview1.ContainingControl = null;
                this.axCopdfxCview1.Dispose();
                this.axCopdfxCview1 = null;
            }

            this.Collect();
        }

        private void Collect()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            this.refs.RemoveAll(a => a?.IsAlive != true);

            this.textBox1.Text = string.Join("\r\n", this.refs.Select(a => $"{a.IsAlive} {a.Target}"));
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = dialog.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = dialog.FileName;
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            using (FileStream fs = new FileStream(textBox1.Text, FileMode.Open))
            {
                progressBar1.Value = 0;
                int readWriteSize = 40;
                double tmp = fs.Length / (double)readWriteSize;
                int offset = 0;

                for (int i = 0; i < (int)((tmp - (int)tmp == 0 && tmp > 0) ? tmp : tmp + 1); i++)
                {
                    int tmpReadWriteSize = ((fs.Length - offset) < readWriteSize ? (int)(fs.Length - offset) : readWriteSize);
                    byte[] bytes = new byte[fs.Length];
                    await fs.ReadAsync(bytes, offset, tmpReadWriteSize);
                    await WriteAsync(bytes, offset, tmpReadWriteSize);
                    offset += readWriteSize;

                    if (progressBar1.Value + (100 / (int)tmp) <= 100)
                        progressBar1.Value += (100 / (int)tmp);
                }
            }
        }

        private async Task WriteAsync(byte[] bytes, int offset, int readWriteSize)
        {
            await Task.Run(async () =>
            {
                using (FileStream fs = new FileStream(textBox2.Text, FileMode.Append))
                {
                    await fs.WriteAsync(bytes, offset, readWriteSize);
                    UpdateProgressBar(offset / bytes.Length);
                }
            });
            
        }

        private void UpdateProgressBar(int v)
        {
            if (progressBar1.InvokeRequired)
                progressBar1.Invoke(new Action<int>(UpdateProgressBar), v);
            else
                progressBar1.Value += v;
        }
    }
}

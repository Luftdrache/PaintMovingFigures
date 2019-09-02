using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThirdLab
{
    public partial class Form2 : Form
    {
        string[] allColors = Enum.GetNames(typeof(KnownColor));
        bool isSaveDotSize;
        bool isSaveLineSize;
        bool isSaveDotColor;
        bool isSaveLineColor;
        int dSize;
        int lSize;
        Color dCol;
        Color lCol;

        bool chaoticMovement;
        bool normalMovement;
        int movementMode; 

        public Form2()
        {
            InitializeComponent();
            CenterToScreen();
            movementMode = 2;

            //Add colors  to comboBoxes
            for (int i = 0; i < allColors.Length; i++)
            {
                comboBox1.Items.Add(allColors[i]);
                comboBox2.Items.Add(allColors[i]);
            }
        }
 
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            setBoolParam();
            dSize = (int)numericUpDown1.Value;
            isSaveDotSize = true;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            setBoolParam();
            lSize = (int)numericUpDown2.Value;
            isSaveLineSize = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            setBoolParam();
            dCol = Color.FromName(comboBox1.SelectedItem.ToString());
            isSaveDotColor = true;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            setBoolParam();
            lCol = Color.FromName(comboBox2.SelectedItem.ToString());
            isSaveLineColor = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            (this.Owner as Form1).moveCounter = movementMode;
            if (isSaveDotSize) (this.Owner as Form1).dotSize = dSize;
            if (isSaveDotColor) (this.Owner as Form1).dotColor = dCol;
            if (isSaveLineSize) (this.Owner as Form1).lineWidth = lSize;
            if (isSaveLineColor) (this.Owner as Form1).lineColor = lCol;
          

            button1.Visible = false;
            button2.Visible = false;
            label6.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void setBoolParam()
        {
            button1.Visible = true;
            button2.Visible = true;
            label6.Visible = false;
        }

        //Normal movement
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            setBoolParam();
            chaoticMovement = false;
            movementMode = 1;
            if (checkBox1.Checked == true)
                checkBox2.Checked = false;
        }

        //Chaotic movement
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            setBoolParam();
            if (checkBox2.Checked == true)
            checkBox1.Checked = false;
            normalMovement = false;
            movementMode = 2;       
        }
    }
}

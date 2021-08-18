using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stitcher360
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeMenu();
        }

        public void InitializeMenu()
		{
			int widthOfButton = 100;
			int heightOfButton = 70;

			//location X, location Y, name of a button, text inside button, width and height of button
			CreateButton(widthOfButton, this.Bottom/2, "button1", "Load images", widthOfButton, heightOfButton);
            CreateButton(this.Width - widthOfButton, this.Top, "button2", "Advanced", widthOfButton, heightOfButton);
			CreateButton(this.Width - widthOfButton, (this.Bottom / 3)*2, "button3", "Repaire Vignetattion", widthOfButton, heightOfButton);
			CreateButton(this.Width - widthOfButton, this.Bottom / 3, "button4", "Repaire Lens Distortion", widthOfButton, heightOfButton);


            int widthOfTextbox = 200;
            int heightOfTextbox = 40;
            //location X, location Y, name of a textbox, text inside textbox, width and height of textbox
            CreateTextbox(this.Width / 2, this.Bottom / 2, "button1", "Textbox", widthOfTextbox, heightOfTextbox);
		}

		private void CreateTextbox(int X, int Y, string name, string text, int width, int height)
		{
            // Create and initialize a Button.
            Button button = new Button();


            button.Location = new System.Drawing.Point(X, Y);
            button.Name = name;
            button.Size = new System.Drawing.Size(width, height);
            button.TabIndex = 4;
            button.Text = text;
            button.UseVisualStyleBackColor = true;

            // Add the button to the form.
            Controls.Add(button);
        }

		public void CreateButton(int X, int Y, string name, string text, int width, int height)
		{
			// Create and initialize a Button.
			Button button = new Button();


			button.Location = new System.Drawing.Point(X, Y);
			button.Name = name;
			button.Size = new System.Drawing.Size(width, height);
			button.TabIndex = 4;
			button.Text = text;
			button.UseVisualStyleBackColor = true;


            // TODO: vyresime delegatama
            switch (text)
            {
                case "Load images":
                    button.Click += new System.EventHandler(this.LoadImage_Click);
                    break;

                case "Advanced":
                    button.Click += new System.EventHandler(this.Advanced_Click);
                    break;

                case "Repaire Vignetattion":
                    button.Click += new System.EventHandler(this.RepaireVignetattion_Click);
                    button.Visible = false;
                    button.Hide();
                    break;

                case "Repaire Lens Distortion":
                    button.Click += new System.EventHandler(this.RepaireLens_Click);
                    button.Visible = false;
                    button.Hide();
                    break;

                default:
                    button.Click += new System.EventHandler(this.Form1_Click);
                    break;
            }


            // Add the button to the form.
            Controls.Add(button);
		}

		private void RepaireLens_Click(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		private void RepaireVignetattion_Click(object sender, EventArgs e)
		{
            //TODO: nejdriv se zeptat na nahrani masky na vignetaci a pak se to magicky opravi

			throw new NotImplementedException();
		}

        //hides extra tools
		private void Advanced_Click(object sender, EventArgs e)
		{
            //Controls[2] and Controls[3] are buttons Repaire Vignetattion and Repaire Lens Distortion
            if (Controls[2].Visible)
			{
                Controls[2].Visible = false;
                Controls[3].Visible = false;
                Controls[2].Refresh();
                Controls[3].Refresh();

            }
			else
			{
                Controls[2].Visible = true;
                Controls[3].Visible = true;
                Controls[2].Refresh();
                Controls[3].Refresh();
            }
		}

		private void LoadImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "Open Image",
                Filter = "Image File|*.jpg;*.png"
            })
            {
                if (ofd.ShowDialog(this) != DialogResult.OK) return;

               /* m_original = Image.FromFile(ofd.FileName);
                m_processed = (Image)m_original.Clone();
                myProcessedLabel.Text = "Original";
                myPictureBox.Image = m_original;*/
            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
			//this.Close();
        }
    }
}

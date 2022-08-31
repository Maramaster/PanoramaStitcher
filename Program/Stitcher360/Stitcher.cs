using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Stitcher360
{
	public partial class Stitcher : Form
	{
		SessionData sessionData = new SessionData();
		public Stitcher()
		{ 
			InitializeComponent();
			InitializeMenu();

			this.Text = "Stitcher";
			this.CenterToScreen();
			FormBorderStyle = FormBorderStyle.FixedSingle;
			MaximizeBox = false;
			MinimizeBox = false;		
		}

        public void InitializeMenu()
		{
			int widthOfButton = 100;
			int heightOfButton = 70;

			//location X, location Y, name of a button, text inside button, width and height of button
			CreateButton(widthOfButton - 40, heightOfButton - 40, "button1", "Load images", widthOfButton, heightOfButton);
			CreateButton(this.Width - widthOfButton - 10, this.Bottom - (heightOfButton * 3 / 2) - 10, "button2", "Stitch Images", widthOfButton, heightOfButton);

			int widthOfTextbox = 100;
			int widthOfText = 150;
			int heightOfTextbox = 40;
			//location X, location Y, name of a textbox, text inside textbox, width and height of textbox, readonly
			CreateTextbox(15, ((this.Bottom / 3) * 1) / 2 + (this.Bottom / 2) - 100, "textBox1", "", widthOfTextbox, heightOfTextbox, false);
			CreateTextbox(15, ((this.Bottom / 3) * 2) / 2 + (this.Bottom / 2) - 100, "textBox2", "", widthOfTextbox, heightOfTextbox, false);
			CreateTextbox(200, ((this.Bottom / 3) * 2) / 2 + (this.Bottom / 2) - 100, "textBox3", "", widthOfTextbox, heightOfTextbox, false);
			CreateCombobox(200, ((this.Bottom / 3) * 1) / 2 + (this.Bottom / 2) - 100, "comboBox", "", widthOfTextbox, heightOfTextbox);

			//not rewritable textboxes for info only
			CreateTextbox(15, ((this.Bottom / 3) * 1) / 2 + (this.Bottom / 2) - 100 - heightOfTextbox + 15, "text1", "Number of Pictures in Row", widthOfText, heightOfTextbox, true);
			CreateTextbox(15, ((this.Bottom / 3) * 2) / 2 + (this.Bottom / 2) - 100 - heightOfTextbox + 15, "text2", "Number of Pictures in Collumn", widthOfText, heightOfTextbox, true);
			CreateTextbox(200, ((this.Bottom / 3) * 2) / 2 + (this.Bottom / 2) - 100 - heightOfTextbox + 15, "text3", "Y angle of photos taken", widthOfText, heightOfTextbox, true);
			CreateTextbox(200, ((this.Bottom / 3) * 1) / 2 + (this.Bottom / 2) - 100 - heightOfTextbox + 15, "text3", "Resolution", widthOfText, heightOfTextbox, true);

			//Creating Title of the Application
			CreateTitle(this.Width/2-250, 0, "text4", "Stitcher from evently spaced photos", 800, 300, true);

			CreateSeparatingLine();
		}



        private void CreateSeparatingLine()
		{
			Label label = new Label();
			label.AutoSize = false;
			label.Location = new System.Drawing.Point(0, this.Bottom / 2-50);
			label.BorderStyle = BorderStyle.Fixed3D;
			label.BackColor = Color.Black;
			label.ForeColor = Color.Black;
			label.Size = new System.Drawing.Size(this.Width, 2);

			// Add the label to the form.
			Controls.Add(label);
		}

		private void CreateTextbox(int X, int Y, string name, string text, int width, int height, bool ReadOnly)
		{
			// Create and initialize a TextBox.
			TextBox textBox = new TextBox();

			textBox.Location = new System.Drawing.Point(X, Y);
			textBox.Name = name;
			textBox.Size = new System.Drawing.Size(width, height);
			textBox.TabIndex = 4;
			textBox.Text = text;
			textBox.ReadOnly = ReadOnly;
			if (ReadOnly) { textBox.BorderStyle = System.Windows.Forms.BorderStyle.None; }

			// Add the textbox to the form.
			Controls.Add(textBox);

			textBox.Leave += new System.EventHandler(this.WriteToSessionDataTextBox);
		}

		private void CreateCombobox(int X, int Y, string name, string text, int width, int height)
		{
			// Create and initialize a TextBox.
			ComboBox comboBox = new ComboBox();

			comboBox.Location = new System.Drawing.Point(X, Y);
			comboBox.Name = name;
			comboBox.Size = new System.Drawing.Size(width, height);
			comboBox.TabIndex = 4;
			comboBox.Text = text;

			comboBox.Items.Add("Low");
			comboBox.Items.Add("Medium");
			comboBox.Items.Add("High");

			// Add the textbox to the form.
			Controls.Add(comboBox);

			comboBox.Leave += new System.EventHandler(this.WriteToSessionDataComboBox);
		}

		private void WriteToSessionDataTextBox(object sender, EventArgs e)
		{
			//firstly check if input data is valid
			if (Validator.IsNumber(((TextBox)sender).Text))
			{
				//once youve finished adding data to Textbox, add information to SessionData
				switch (((TextBox)sender).Name)
				{
					//update rows
					case "textBox1":
						sessionData.NumberOfPicturesInRow = Int32.Parse(((TextBox)sender).Text);
						break;

					//update collumns
					case "textBox2":
						sessionData.NumberOfPicturesInCol = Int32.Parse(((TextBox)sender).Text);
						break;

					//update Y angle
					case "textBox3":
						sessionData.YAngle = Int32.Parse(((TextBox)sender).Text);
						break;
					default:
						break;

				}
			}
			else
			{
				if (((TextBox)sender).Text != "") { MessageBox.Show("Please enter an integer."); }
			}

		}

		private void WriteToSessionDataComboBox(object sender, EventArgs e)
		{		
			var value = ((ComboBox)sender).Text;

			switch (value)
            {
				case "Low":
					sessionData.ImageResolution = ImageResolution.Low;
					break;
				case "Medium":
					sessionData.ImageResolution = ImageResolution.Medium;
					break;
				case "High":
					sessionData.ImageResolution = ImageResolution.High;
					break;
				default:
					MessageBox.Show("Please choose quality from selected values.");
					break;
			}
		}

		private void CreateTitle(int X, int Y, string name, string text, int width, int height, bool ReadOnly)
		{

			// Create and initialize a TextBox.
			TextBox textBox = new TextBox();

			textBox.Location = new System.Drawing.Point(X, Y);
			textBox.Name = name;
			textBox.Size = new System.Drawing.Size(width, height);
			textBox.Font = new Font("Tahoma", 24, FontStyle.Bold);
			textBox.TabIndex = 4;
			textBox.Text = text;
			textBox.ReadOnly = ReadOnly;
			if (ReadOnly) { textBox.BorderStyle = System.Windows.Forms.BorderStyle.None; }

			// Add the button to the form.
			Controls.Add(textBox);
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

			switch (text)
			{
				case "Load images":
					button.Click += new System.EventHandler(this.LoadImage_Click);
					break;

				case "Stitch Images":
					button.Click += new System.EventHandler(this.StitchImages_Click);
					break;

				default:
					break;
			}

			// Add the button to the form.
			Controls.Add(button);
		}

		private void StitchImages_Click(object sender, EventArgs e)
		{
			//checks if there are at least 2 photos to be stitched
			if (sessionData.LoadedImages != null && sessionData.LoadedImages.Length >= 2)
			{
				var finalresult = PhotoAssembler.StitchPhotos(sessionData);
				SaveFileDialog dialog = new SaveFileDialog();
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					finalresult.Save(dialog.FileName, ImageFormat.Png);
				}
			}
			else
			{
				MessageBox.Show("Please enter at least two pictures to be stitched.");
			}

		}

        private void LoadImage_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog()
			{
				Title = "Open Image",
				Filter = "Image File|*.jpg;*.png"
			})
			{
				openFileDialog.Multiselect = true;
				if (openFileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
				{
					//getting selected image
					sessionData.LoadedImages = new Bitmap[openFileDialog.FileNames.Length];

					Image[] images = GetImages(openFileDialog.FileNames);
					
					//variables for in which row and collumn will the picture end up
					int row = 0;
					int col = 0;
					for (int i = 0; i < sessionData.LoadedImages.Length; i++)
					{
						//unifing picture size for all input size, expected fullframe ration (2:3)
						sessionData.LoadedImages[i] = new Bitmap(images[i], new Size(500, 750));

						int widthOfButton = 100;
						int heightOfButton = 90;
						int separatingGap = 7;
						int offsetFromTop = 50;

						int widthOfPicture = (this.Width - widthOfButton) / (sessionData.NumberOfPicturesInRow + 1) - separatingGap;
						int heightOfPicture = ((this.Bottom / 2) - heightOfButton) / (sessionData.NumberOfPicturesInCol + 1) - separatingGap;
						PictureBox picture = new PictureBox
						{
							Width = widthOfPicture,
							Height = heightOfPicture
						};

						picture.SizeMode = PictureBoxSizeMode.StretchImage;
						picture.Image = (Image)images[i];

						picture.Name = "Test PictureBox" + i.ToString();

						if(this.Width-50 < widthOfButton + ((widthOfPicture * (col + 1)) + (col * separatingGap)))
						{
							row++;
							col = 0;
						}
						picture.Location = new Point(widthOfButton + ((widthOfPicture * (col + 1)) + (col * separatingGap)), (offsetFromTop + heightOfPicture * (row)) + (row * separatingGap));

						Controls.Add(picture);
						picture.BringToFront();

						col++;
					}
				}
			}
		}

		private Image[] GetImages(string[] fileNames)
		{
			Image[] images = new Image[fileNames.Length];
			for (int i = 0; i < fileNames.Length; i++)
			{
				images[i] = Image.FromFile(fileNames[i]);
			}
			return images;
		}
	}
}

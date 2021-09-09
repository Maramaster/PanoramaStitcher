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
	public partial class Form1 : Form
	{
		SessionData sessionData = new SessionData();
		public Form1()
		{ 
			InitializeComponent();
			InitializeMenu();

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
			CreateButton(this.Width - widthOfButton - 10, ((this.Bottom / 3) * 1) / 2 + (this.Bottom / 2) - 100 - heightOfButton + 15, "button2", "Advanced", widthOfButton, heightOfButton);;
			CreateButton(this.Width / 2, ((this.Bottom / 3) * 1) / 2 + (this.Bottom / 2) - 100 - heightOfButton + 15, "button3", "Repair Lens Distortion", widthOfButton, heightOfButton);
			CreateButton(this.Width - widthOfButton - 10, this.Bottom - (heightOfButton * 3 / 2) - 10, "button4", "Stitch Images", widthOfButton, heightOfButton);


			int widthOfTextbox = 100;
			int widthOfText = 150;
			int heightOfTextbox = 40;
			//location X, location Y, name of a textbox, text inside textbox, width and height of textbox, readonly
			CreateTextbox(15, ((this.Bottom / 3) * 1) / 2 + (this.Bottom / 2) - 100, "textBox1", "", widthOfTextbox, heightOfTextbox, false);
			CreateTextbox(15, ((this.Bottom / 3) * 2) / 2 + (this.Bottom / 2) - 100, "textBox2", "", widthOfTextbox, heightOfTextbox, false);
			CreateTextbox(200, ((this.Bottom / 3) * 1) / 2 + (this.Bottom / 2) - 100, "textBox3", "", widthOfTextbox, heightOfTextbox, false);
			CreateTextbox(200, ((this.Bottom / 3) * 2) / 2 + (this.Bottom / 2) - 100, "textBox4", "", widthOfTextbox, heightOfTextbox, false);

			//not rewritable textboxes for info only
			CreateTextbox(15, ((this.Bottom / 3) * 1) / 2 + (this.Bottom / 2) - 100 - heightOfTextbox + 15, "text1", "Number of Pictures in Row", widthOfText, heightOfTextbox, true);
			CreateTextbox(15, ((this.Bottom / 3) * 2) / 2 + (this.Bottom / 2) - 100 - heightOfTextbox + 15, "text2", "Number of Pictures in Collumn", widthOfText, heightOfTextbox, true);
			CreateTextbox(200, ((this.Bottom / 3) * 1) / 2 + (this.Bottom / 2) - 100 - heightOfTextbox + 15, "text3", "Resolution in X dimension", widthOfText, heightOfTextbox, true);
			CreateTextbox(200, ((this.Bottom / 3) * 2) / 2 + (this.Bottom / 2) - 100 - heightOfTextbox + 15, "text4", "Y angle of photos taken", widthOfText, heightOfTextbox, true);

			//Creating Title of the Application
			CreateTitle(this.Width/2, 0, "text5", "SCHER", 800, 300, true);

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

			textBox.Leave += new System.EventHandler(this.WriteToSessionData);
		}

		private void WriteToSessionData(object sender, EventArgs e)
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

					//update Resolution X
					case "textBox3":
						// 1.9 is perfect for resolution 1000x500
						// 0.96 is perfect for resolution 2000x1000
						if (Int32.Parse(((TextBox)sender).Text) == 1000) { sessionData.NewBasisScale = 1.9; }
						else { sessionData.NewBasisScale = 0.96; }
						sessionData.OutResolutionX = Int32.Parse(((TextBox)sender).Text);
						sessionData.OutResolutionY = sessionData.OutResolutionX/2;
						sessionData.Radius = sessionData.OutResolutionX / (2 * Math.PI);
						break;

					//update Y angle
					case "textBox4":
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

				case "Advanced":
					button.Click += new System.EventHandler(this.Advanced_Click);
					break;

				case "Repair Lens Distortion":
					button.Click += new System.EventHandler(this.RepaireLens_Click);
					button.Visible = false;
					button.Hide();
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
				Bitmap finalresult = new Bitmap(sessionData.OutResolutionX, sessionData.OutResolutionY);
				finalresult = PhotoAssembler.StitchPhotos(sessionData);
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

		/// <summary>
		/// Additional tool for preprocessing photos, before stitiching. Selecting one by one and repairing its lens distortion
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RepaireLens_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog()
			{
				Title = "Open Image",
				Filter = "Image File|*.jpg;*.png"
			})
			{
				if (openFileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
				{
					Image image = GetImage(openFileDialog.FileName);
					Bitmap finalresult = LensCorrection.CorrectLensDistortion((Bitmap)image,1);

					SaveFileDialog dialog = new SaveFileDialog();
					if (dialog.ShowDialog() == DialogResult.OK)
					{
						finalresult.Save(dialog.FileName, ImageFormat.Png);
					}
				}
					
			}
				
		}

		//hides extra tools
		private void Advanced_Click(object sender, EventArgs e)
		{

			//Name "Repair Lens Distortion" is refered to button Repair Lens Distortion
			foreach (var control in Controls)
			{
				if (control is Button && ((Button)control).Text == "Repair Lens Distortion")
				{
					if (((Button)control).Visible)
					{
						((Button)control).Visible = false;
						((Button)control).Refresh();
						((Button)sender).BackColor = default(Color);

					}
					else
					{
						((Button)control).Visible = true;
						((Button)control).Refresh();
						((Button)sender).BackColor = Color.DarkGray;
					}
				}

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
						sessionData.LoadedImages[i] = new Bitmap(Image.FromFile(openFileDialog.FileNames[i]));

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
		private Image GetImage(string fileName)
		{
			return Image.FromFile(fileName);
		}
	}
}

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
			//testovaci data
			sessionData.OutResolutionX = 1000;
			sessionData.OutResolutionY = 500;

			//TODO: fullscreen on resize
			InitializeComponent();
			InitializeMenu();
			//fullscreen
			WindowState = FormWindowState.Maximized;
		}

		public void InitializeMenu()
		{
			int widthOfButton = 100;
			int heightOfButton = 70;

			//location X, location Y, name of a button, text inside button, width and height of button
			CreateButton(widthOfButton - 40, heightOfButton - 40, "button1", "Load images", widthOfButton, heightOfButton);
			CreateButton(this.Width - widthOfButton - 10, ((this.Bottom / 3) * 1) / 2 + (this.Bottom / 2) - 100 - heightOfButton + 15, "button2", "Advanced", widthOfButton, heightOfButton);
			CreateButton(this.Width / 2, ((this.Bottom / 3) * 1) / 2 + (this.Bottom / 2) - 100 - heightOfButton + 15, "button3", "Remove Vignette", widthOfButton, heightOfButton);
			CreateButton(this.Width / 2, this.Bottom - (heightOfButton * 3 / 2) - 10, "button4", "Repair Lens Distortion", widthOfButton, heightOfButton);
			CreateButton(this.Width - widthOfButton - 10, this.Bottom - (heightOfButton * 3 / 2) - 10, "button5", "Stitch Images", widthOfButton, heightOfButton);


			int widthOfTextbox = 200;
			int heightOfTextbox = 40;
			//location X, location Y, name of a textbox, text inside textbox, width and height of textbox
			CreateTextbox(15, ((this.Bottom / 3) * 1) / 2 + (this.Bottom / 2) - 100, "textBox1", "", widthOfTextbox, heightOfTextbox, false);
			CreateTextbox(15, ((this.Bottom / 3) * 2) / 2 + (this.Bottom / 2) - 100, "textBox2", "", widthOfTextbox, heightOfTextbox, false);
			CreateTextbox(15, ((this.Bottom / 3) * 3) / 2 + (this.Bottom / 2) - 100, "textBox3", "", widthOfTextbox, heightOfTextbox, false);

			//not rewritable textboxes for info only
			CreateTextbox(15, ((this.Bottom / 3) * 1) / 2 + (this.Bottom / 2) - 100 - heightOfTextbox + 15, "text1", "Input Focal Lenght", widthOfTextbox, heightOfTextbox, true);
			CreateTextbox(15, ((this.Bottom / 3) * 2) / 2 + (this.Bottom / 2) - 100 - heightOfTextbox + 15, "text2", "Input Number of Pictures in Row", widthOfTextbox, heightOfTextbox, true);
			CreateTextbox(15, ((this.Bottom / 3) * 3) / 2 + (this.Bottom / 2) - 100 - heightOfTextbox + 15, "text3", "Input Number of Pictures in Collumn", widthOfTextbox, heightOfTextbox, true);

			//CreateSeparatingLine();
		}

		private void CreateSeparatingLine()
		{
			//TODO: nejde videt. zviditelnit
			Label label = new Label();
			label.AutoSize = false;
			//label.Height = 200;
			label.Location = new System.Drawing.Point(100, 100);
			label.BorderStyle = BorderStyle.Fixed3D;
			label.BackColor = Color.Black;
			label.ForeColor = Color.Black;
			label.Size = new System.Drawing.Size(55, 13);
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


			// Add the button to the form.
			Controls.Add(textBox);

			textBox.Leave += new System.EventHandler(this.WriteToSessionData);
		}

		private void WriteToSessionData(object sender, EventArgs e)
		{
			//firstly check if input data is valid
			if (Validator.isNumber(((TextBox)sender).Text))
			{
				//once youve finished adding data to Textbox, add information to SessionData
				switch (((TextBox)sender).Name)
				{
					//update focal lenght
					case "textBox1":
						sessionData.FocalLenght = Int32.Parse(((TextBox)sender).Text);
						break;
					//update rows
					case "textBox2":
						sessionData.NumberOfPicturesInRow = Int32.Parse(((TextBox)sender).Text);
						break;
					//update collumns
					case "textBox3":
						sessionData.NumberOfPicturesInCol = Int32.Parse(((TextBox)sender).Text);
						break;

					default:
						break;

				}
			}
			else
			{
				MessageBox.Show("Please enter an integer.");
			}

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

				case "Remove Vignette":
					button.Click += new System.EventHandler(this.RepaireVignetattion_Click);
					button.Visible = false;
					button.Hide();
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
					button.Click += new System.EventHandler(this.Form1_Click);
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

			//Controls[2] and Controls[3] are buttons Remove Vignette and Repair Lens Distortion
			if (Controls[2].Visible)
			{
				Controls[2].Visible = false;
				Controls[3].Visible = false;
				Controls[2].Refresh();
				Controls[3].Refresh();
				((Button)sender).BackColor = default(Color);

			}
			else
			{
				Controls[2].Visible = true;
				Controls[3].Visible = true;
				Controls[2].Refresh();
				Controls[3].Refresh();
				((Button)sender).BackColor = Color.DarkGray;
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

					Image[] images = getImages(openFileDialog.FileNames);
					
					//variables for in which row and collumn will the picture end up
					int row = 0;
					int col = 0;
					for (int i = 0; i < sessionData.LoadedImages.Length; i++)
					{
						sessionData.LoadedImages[i] = new Bitmap(Image.FromFile(openFileDialog.FileNames[i]));

						int widthOfButton = 100;
						int heightOfButton = 70;
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

		private Image[] getImages(string[] fileNames)
		{
			Image[] images = new Image[fileNames.Length];
			for (int i = 0; i < fileNames.Length; i++)
			{
				images[i] = Image.FromFile(fileNames[i]);
			}
			return images;
		}

		private void Form1_Click(object sender, EventArgs e)
		{
			//this.Close();
		}
	}
}

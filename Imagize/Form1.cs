using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace Imagize
{
    public partial class Form1 : Form
    {
        string FilePath = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OFD.Filter = "Any file (*.*)|*.*";
            OFD.FileName = "";
            DialogResult DR = OFD.ShowDialog();

            if (DR == DialogResult.OK)
            {
                FilePath = OFD.FileName.ToString();
                lblWarning.Visible = true;
                btnImagize.Visible = true;
            }
        }

        void GenerateImage()
        {
            FileInfo FI = new FileInfo(FilePath);
            long FileSize = 0;
            long PaddingSize = 0;
            long ArraySize = 0;
            double SizeX = 0;
            double SizeY = 0;


            FileSize = FI.Length;
            while ((Math.Sqrt(FileSize + PaddingSize) %1) != 0)
            {
                PaddingSize++;
            }

            ArraySize = FileSize + PaddingSize;



            SizeX = Math.Sqrt(ArraySize);
            SizeY = SizeX;

            byte[] TotalArray = new byte[ArraySize];
            byte[] PaddingArray = new byte[PaddingSize];
            byte[] FileBytes = File.ReadAllBytes(FilePath);

            for (int x = 0; x < PaddingSize; x++)
                PaddingArray[x] = (byte) 255;

            for (int i = 0; i < FileSize; i++)
                TotalArray[i] = FileBytes[i];
            
            for(long x = 0; x < (PaddingSize); x++)
                TotalArray[FileSize + x] = PaddingArray[x];

            long ByteCounter = 0;

            Bitmap BM = new Bitmap((int)SizeX, (int)SizeY/3);
            for (int cX = 0; cX < (int)SizeX; cX++)
            {
                for (int cY = 0; cY < (int) SizeY/3; cY++)
                {
                    Color nC = new Color();
                    long R = 0;
                    long G = 0;
                    long B = 0;
                    if (ByteCounter < (ArraySize - 1))
                        R = ByteCounter;
                    else
                        R = (ArraySize - 1);

                    if ((ByteCounter + 1) < (ArraySize - 1))
                        G = ByteCounter + 1;
                    else
                        G = (ArraySize - 1);

                    if ((ByteCounter + 2) < (ArraySize - 1))
                        B = ByteCounter + 2;
                    else
                        B = (ArraySize - 1);

                    nC = Color.FromArgb((int)TotalArray[R], (int)TotalArray[G], (int)TotalArray[B]);
                    BM.SetPixel(cX, cY, nC);
                    ByteCounter += 3;
                }
            }

            SFD.Filter = "Bitmap (*.bmp)|*.bmp";
            SFD.FileName = "";
            DialogResult DR = SFD.ShowDialog();
            if (DR == DialogResult.OK)
            {
                BM.Save(SFD.FileName.ToString());
                for (int i = 0; i < TotalArray.Length; i++)
                    TotalArray[i] = 0;
                for (int i = 0; i < PaddingArray.Length; i++)
                    PaddingArray[i] = 0;
                for (int i = 0; i < FileBytes.Length; i++)
                    FileBytes[i] = 0;
                BM.Dispose();
                MessageBox.Show("File successfully saved in:\n" + SFD.FileName, "Done!",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            else
            {
                for (int i = 0; i < TotalArray.Length; i++)
                    TotalArray[i] = 0;
                for (int i = 0; i < PaddingArray.Length; i++)
                    PaddingArray[i] = 0;
                for (int i = 0; i < FileBytes.Length; i++)
                    FileBytes[i] = 0;
                BM.Dispose();
                MessageBox.Show("Operation Cancelled.\n\nErasing loaded data.", "Cancelled!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnImagize_Click(object sender, EventArgs e)
        {
            GenerateImage();
        }
    }
}

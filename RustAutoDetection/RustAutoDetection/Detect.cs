using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace RustAutoDetection
{
    class Detect
    {
        //Credits Yully1337 (GUI , Modifying Class For Rust, Documentating Every Function, Converting Images To Grayscale (Improves Score), Cleaning Up Code)
        //Credits Ssarkos Base Detection Class

        public static bool DebugMaxScore { get; set; }

        public static string ReturnWeaponLoop()
        {
            //Graphics object
            Graphics graphics;
            //Bitmap object
            Bitmap bitmap;

            //Estimating inventory location (where it shows up)
            bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width / 8, Screen.PrimaryScreen.Bounds.Height / 4);
            // Converting bitmap to graphics
            graphics = Graphics.FromImage(bitmap);


            //Gets Image From Directory
            string currentExe = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string currentDirectory = System.IO.Path.GetDirectoryName(currentExe);
            Image<Rgb, Byte>[] guns = new Image<Rgb, Byte>[] {
                    new Image<Rgb, Byte>($@"{currentDirectory}\\Weapons\\ak.png"),
                             new Image<Rgb, Byte>($"{currentDirectory}\\Weapons\\lr300.png"),
                                      new Image<Rgb, Byte>($"{currentDirectory}\\Weapons\\customsmg.png"),
                                               new Image<Rgb, Byte>($"{currentDirectory}\\Weapons\\mp5.png"),
                                                        new Image<Rgb, Byte>($"{currentDirectory}\\Weapons\\m249.png")
            };

             
            //Gun Names Array
            string[] gunNames = new string[]
            {
                 "Ak47",
                 "LR300",
                 "Custom SMG",
                 "MP5",
                 "M249"
            };


            //The Most Scored Image 
            List<double> mostScored = new List<double>();


            while (true)
            {
                //Clearing List Before Checking
                mostScored.Clear();

                //Getting Image From Screen
                graphics.CopyFromScreen(Screen.PrimaryScreen.Bounds.Width / 6 * 5, Screen.PrimaryScreen.Bounds.Height / 24 * 15, 0, 0, new Size(bitmap.Width, bitmap.Height));

                //Creating Object Of Image From Screen Graphics G -> Bitmap Bmp
                Image<Rgb, Byte> sourceImage = bitmap.ToImage<Rgb, Byte>();

                //The List Of Scores
                List<double> scoreList = new List<double>();

                //Iterates Through Gunlength
                for (int i = 0; i < guns.Length; i++)
                {
                    //Matching image from source image (screen) to the gun array index
                    Image<Gray, float> resultImage = sourceImage.MatchTemplate(guns[i], TemplateMatchingType.CcoeffNormed);
                    float[,,] matches = resultImage.Data;
                    for (int y = 0; y < matches.GetLength(0); y++)
                    {
                        for (int x = 0; x < matches.GetLength(1); x++)
                        {
                            double matchScore = matches[y, x, 0];
                            //Adding Scores To List After Checking For Matches(Similarities)
                            scoreList.Add(matchScore);
                        }
                    }

                    //Getting The Max Score And adding it to list
                    mostScored.Add(scoreList.Max());
                    //Cleaning score list
                    scoreList.Clear();
                }

                //More Performance by sleeping thread
                Thread.Sleep(300);

                //creating variable of max list 
                double max = mostScored.Max();

                //doing a lil integrity check because sometimes emguCV might mistake the image
                //you also debug this by printing out the "normal" rate of a correct scan and compare.
                if (DebugMaxScore)
                    MessageBox.Show($"Current Score -> {max.ToString()}");

                if (max > 0.9)
                    return gunNames[mostScored.IndexOf(max)];
            }
        }
    }
}

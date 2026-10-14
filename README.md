# OSI
OSI: OS Information, a Windows operating system and hardware information tool



## Author

* [@remusrigo](https://github.com/RemusRigo)
* [@remusrigo](https://sourceforge.net/u/remusrigo/profile)



## Features

* Display information (read with WMI):
  * Operating System: version, codename, release date, end of support
  * Windows
    * Environment Variables
    * User Account (all users of the computer)
  * Hardware
    * BaseBoard (MotherBoard)
      * BIOS
    * Battery
    * Disk Drive
    * KeyBoard
    * Memory
    * Processor
* Modular: each category is a separate module (DLL)
* Progress bar with percent while the information is read
* Planned: read the information from a remote computer (command line `/host:` `/user:` `/pass:`)



## User's Guide

See the [User's Guide](UsersGuide.md) for downloads, runtimes, installation and usage.



## Statistics

![VB.NET](https://img.shields.io/badge/Language-VB.NET-blue)
![WMI](https://img.shields.io/badge/API-WMI-lightblue)
![VS](https://img.shields.io/badge/IDE-Visual%20Studio-purple)
<br>
![status](https://img.shields.io/badge/status-in%20progress-orange) 
![GitHub Downloads (all assets, all releases)](https://img.shields.io/github/downloads/RemusRigo/OSI-VB.NET/total)



## Roadmap

* 2026-08-18: Add: Battery info
* 2026-08-18: Add: BaseBoard (MotherBoard) BIOS info
* 2026-08-17: Fix: Optimize code (build TreeView nodes at OnLoad, organize forms into categories)
* 2026-08-15: Add: KeyBoard info
* 2026-08-15: Fix: Optimize code (caption)
* 2026-08-09: Add: MMC icon for WMI data
* 2026-08-09: Add: BaseBoard (MotherBoard) info
* 2026-08-08: Add: Processor info
* 2026-08-08: Fix: background worker
* 2026-06-21: Add: Disk Drive info
* 2026-06-20: Add: Progress Bar with percent \& adjusted the code
* 2026-06-19: Add: command line parameters (replaced controls on the form)
* 2025-11-20: Fix: some warnings about platform compatibility
* 2025-11-20: Add: Version/Codename/Release Date/End of support
* 2025-08-30: Add: Environment variables (local and remote)
* 2025-08-30: Add: Users: list all users from remote computer
* 2025-08-23: Add: Users: list all users from host computer
* 2025-08-17: OS Information module completed
* 2025-03-26: Project started from scratch (old project was lost)
* 2011-08-11: Project started from scratch (based on an old project written in Delphi in 2009)



## History/Versions

* 2025-03-26: OSI written in VB.NET (new start)
* 2011-08-11: OSI written in VB.NET (new start)
* 2009-12-07: OSI written in Delphi



## Reference

* [Windows versions](https://en.wikipedia.org/wiki/List_of_Microsoft_Windows_versions)


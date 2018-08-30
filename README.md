# IcyWind
Current Status: Exams, See you on May 18.

Screw voice chat. Ranked disabled. UI looks like shit, I'll work on it.


New System, transfering files over. See below if you want to know what it is.

Something new, MAF instances within an MAF instance. What will happen, lets find out if I can do it or if I will just use MEF 
inside of an MAF instance. The purpose of this project is to prevent crashes.

MAF functionality will be disabled on launch. There will also be an option to download one without this functinality in the future.

I will not push code until most of it works. Thank you for waiting.

Do not advertise IcyWind

IcyWind's Icon is not attached in this project, but it will be in the compiled version due to licencing of the icon

# Questions you may have
### Is this project dead?
Here yes. The source is completely closed source and will be pushed here when it is complete.

### Why haven't there been many updates to this source code?
It is too hard to try to help those asking on how to build the code. Everything is closed source right now.

### I wanna help out. How can I help?
Uhh, I'll think about it.

### I want the source code now. How can I get it?
Sorry you can't unless you help with something...

Also, if you somehow get the source code or compiled version, I am not responsibe for bans

### When will a compiled version come out and can I get it earlier?
It will come out someday maybe and the source code will be released on the same date. You can get bother earlier by signing up for the early access program


### What desktop OS do you use? 
Windows and macOS. Thats why there is planned support for both.

### Any apps for android or iOS
No. Yes. No.

### Why are you using squarespace instead of programming a website yourself
Its gets the job done and gets a website running faster then if I programmed it myself.

# Why Icywind
## Features

### Always Update
IcyWind will update League of Legends and the IcyWind Client itself (Why MAF is used instead of MEF). Just keep
IcyWind running at all times. This feature will be seen in later versions of IcyWind

### Plugins
Make the client your client. The power is in your hands to create something magical. The plugin database will be comming soon and the API will made public once I find out how to run plugins more efficiently with native C#.

### Replays
Want to have your replays download right away? How about only ranked games where you have lost? Icywind allows you to select which games to download.

### Multi-user
Have 10 accounts on different regions? Create an account at <ThisUrlIsRemoved> and login to the icywind client and save all the accounts. You can also add your accounts on our website; however, this is not recommended (higher stress on servers). 
 
You can also store accounts offline for free. The server may be made free in the future.
 
### Filters
Tired of elo boosters spamming your account? I am, and by default these messages are blocked

### RUI
You have no idea what this is.

### Multi Language Support
Planned feature

# Warning
If a user tells you to add a file into the "core" folder, please report this user and do not follow their actions. 

The core folder is an extremly critical folder that can give some plugins unrestricted access to the system.

If you add a plugin into the "core" folder, IcyWind's safety features are designed to prevent IcyWind from starting up.

All plugins should go into the "plugin" folder which will allow you to restrict the functions that each plugin gets.

## What does the core folder do?
The core folder is responsible for everything that the program does. The application (exe) actually does not really do anything but is used for grabbing the updates and installing them and seamlessly switching over to a newer version when the user accepts an update.

This method of updating will not be in releases any time soon; it is too buggy and can cause corruption needing the program to be reinstalled. This function can only be enabled by devs (dev mode on server set to true)

# Building the project
Right Click the solution and click rebuild the solution. 
This project uses MAF so just building IcyWind will result in an error. 
Do not modify any of the files in the AddIn folder, unless you know how to use MAF (and won't screw it up for youself).

# UI Images
## Login Page
### Windows
Soon
### MacOS
Planned

## Main Page
### Windows
Soon
### MacOS
Planned

## All other pages
### Windows
Soon
### MacOS
Todo

# Planned OS support
## Tested working
- [x] Windows 10
- [ ] macOS 10.13 [Planned]

## Planned
- [ ] Windows 8.1 (Should work)
- [ ] Windows 7 (Should work)
- [ ] Linux (You must install wine to run league of legends)

# Licenses
| Library/application name       | License used                         |
|:------------------------------:|:------------------------------------:|
| IcyWind *                      | GNU GENERAL PUBLIC LICENSE Version 3 |
| agsXMPP                        | GNU GENERAL PUBLIC LICENSE Version 2 |
| rtmp-Sharp                     | MIT License                          |
 *The IcyWind License applies to all libraries and applications included in this project that have been written by me
 
 # Important Dates
 ##### Note these are not finalized dates. They are plans.
August 13 2018 (closed beta)
Note to testers: You will be able download the src as well. It is a seperate private repo.

September 13 2018 (open beta)

# New Dev System
I have gotten a new system. These are the specs of my dev computer.
 - AMD Ryzen 5 2400g
 - Corsair H60
 - GA-AX370 Gaming K5
 - HyperX Fury (1x8gb)
 - Samsung 960 EVO (250 gb)
 - Corsair CXM 550

If you experience bugs on Intel based systems, notify me.

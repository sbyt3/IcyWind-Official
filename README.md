# IcyWind
Current Status: Finalizing features. Almost ready for beta testing

Join the official discord: https://discord.gg/veYtxju

#### Warning: If you forget your password, all saved accounts will be removed. There is no possible way to recover account passwords and the leauge of legends account credentials that are stored on the server. You should also create a strong UNIQUE password for IcyWind (Use a password manager) as IcyWind acts like a password manager

#### Warning: MultiUserAccount support is in development still. Currently, the way IcyWind is setup, may result in slightly higher latancy as it makes a connection per account. This will not impact those with around ~5 accounts, but those with more than 20 may experience issues.

#### Warning: A certificate IcyWindRootCA will be downloaded when this program is run. It should not be added to the windows certificate store. This certificate is only for use in verifiying IcyWind components and plugins

IcyWind includes some paid components. This is because I am paying for a server. This is why if you have a free
account you are limited to 3 accounts. Cost will than become a reduced $5 for early beta testers. Anyone not falling into this catagory will have to pay $10 USD every month to be able to add more accounts (up to 20) accounts. Any users that need more accounts will need to contact me to remove this limitation.

New System, transfering files over. See below if you want to know what it is.

Something new, MAF instances within an MAF instance. What will happen, lets find out if I can do it or if I will just use MEF 
inside of an MAF instance. The purpose of this project is to prevent crashes.

MAF functionality will be disabled on launch. There will also be an option to download one without this functinality in the future.

I will not push code until most of it works. Thank you for waiting.

Do not advertise IcyWind

IcyWind's Icon is not attached in this project, but it will be in the compiled version due to licencing of the icon

# Questions you may have
### I wanna help out. How can I help?
Just wait. There isn't much you can help with unless you can help contribute to server costs 

I'm paying almost $100 now as services are being added for release. This included a CDN (With WAF), Account server, Registration Server, Database servers, and an experemental server for future features

These costs can change, as all of the webservers are auto-scaling to avoid issues seen on other small league of legends projects

### I want the source code now. How can I get it?
The source code will be made available at a different date. For now (during the open beta) the project will remain closed source, because there will be massive changes that may still occour and new features will be made available.

When it comes to the date that I feel I have everything that rito gems has and more, you will be able to see the source code for this project. If you are worried about your account information, I will be publishing another program completly opensource at launch that will allow you to see how your account data is kept safe. In addition, feel free to proxy the entire connection and look at all the connections that IcyWind makes.

### Why should I pay the montly subscription?
It helps to support the project, and allows you to add as many accounts as you want. In the future, there will also be a remote connection feature only for those paying. This will allow you to connect to our website and queue up, accept, and pick champs, without needing to run to the computer (IcyWind must be running on your desktop, and it will launch the game on your desktop)

Also, your account gets marked as a subscriber, and you are able to display this (optional) to all of those using IcyWind, and you can create custom chat icons for those on icywind.

Access to the GameAnalyzer when released. GameAnalyzer looks at your team's recent match history, ranks (highest ranked account for those using IcyWind) and analyzes how they do verses different chamions, and which suggests ban chamions and pick champion for you. This is uses IcyWind's api, making it exclusive only to those who have IcyWind perks.

Those who are priority, will also recieve better support, and will be able to download the latest client with the latest features without delay.

Finally, all of the subscribers will get access to ALL plugins (some plugins must be paid for), more game data, and can also enable RUI (but why would you?).

### When will a compiled version come out and can I get it earlier?
It will come out someday maybe and the source code will be released on the same date. You can get bother earlier by signing up for the early access program.

### What desktop OS do you use? 
Windows and macOS. That's why there is planned support for both.

### Any apps for android or iOS
These are planned.

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
Have 10 accounts on different regions? Create an account at <ThisUrlIsRemoved> and login to the icywind client and save all the accounts. You cannot add accounts on our servers (webpage) because of the way accounts are stored. 
 
You can store up to 3 accounts with no cost on the IcyWind website.

You can add as many local accounts as you want (these will be deleted after the session)
 
### Filters
Tired of elo boosters spamming your account? I am, and by default these messages are blocked

### RUI
You have no idea what this is.

### Multi Language Support
Planned feature. All I need is people to help with translations. 

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
##### IcyWind Login Page
![loginImg](https://i.imgur.com/7IwItLp.jpg)
##### Riot Login Page
![loginRiotImg](https://i.imgur.com/mLmJaMg.png)
Same picture with a lot of accounts (duplicated)
![RiotLoginALot](https://i.imgur.com/xEpLmge.png)

### MacOS
Sorry nothing here

# Planned OS support
## Tested working
- [x] Windows 10

## Planned
- [ ] macOS

# Licenses
## IcyWind Licenses
| Component Name                 | License used                         |
|:------------------------------:|:------------------------------------:|
| IcyWind.Languages              | GNU Affero General Public License    |
| IcyWind.Mobile                 | GNU GENERAL PUBLIC LICENSE           |
| IcyWind.*                      | Private License                      |

* This license applies to all IcyWind Components unless otherwise noted

** This licese will be changed in the future. Likely to AGPL

### Private License
The private license means that the SourceCode is not provided to the user (ClosedSource). 
It is illigal to use any of these projects in any any of your projects without permission from me. The reason for this stance is because these libraries can interact with the database, and I would like to avoid any user data from being leaked (even though every single account is encrypted and decrypted locally).

## Used Libraries    
| Library name                   | License used                                           |
|:------------------------------:|:------------------------------------------------------:|
| DotNetty                       | MIT License                                            |
| Dragablz                       | MIT License                                            |
| log4net                        | Apache License, Version 2.0                            |
| MaterialDesignInXamlToolkit    | MIT License                                            |
| Home (asp.net)                 | Apache License, Version 2.0                            |
| corefx (dotnet)                | MIT License                                            |
| Microsoft.*                    | MICROSOFT SOFTWARE LICENSE TERMS/Apache/MIT            |
| System.*                       | MICROSOFT SOFTWARE LICENSE TERMS/Apache/MIT            |
| standard (dotnet)              | MIT License                                            |
| Newtonsoft.Json                | MIT License                                            |
| sharpziblib                    | MIT License                                            |
| YamlDotNet                     | MIT License                                            |
| zlib.net                       | http://www.componentace.com/data/ZLIB_.NET/license.txt |
| rtmp-Sharp                     | MIT License                                            |
| CEFSharp                       | BSD                                                    | 
| LegendaryClient                | MIT Licnese                                            |
| Hardcodet.NotifyIcon.Wpf       | The Code Project Open License                          |
| Costura                        | MIT License                                            |
| Fody                           | MIT License                                            |


This program is not sponsored, endorsed, administered by, or otherwise associated with any of the above projects.


Riot Games does not endorse or sponsor this project.

 
 # Important Dates
 ##### Note these are not finalized dates. They are plans.
August 13 2018 (closed beta)
xD missed like MISSED this
Let me try Sept 1st

September 13 2018 (open beta)

# Riot Contact
Riot can contact me by creating an issue or emailing me if they have any concerns about the project.

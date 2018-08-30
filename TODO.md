```cs
//Change this
var hostview = mainpage.Activate<IMainHostView>(AddInSecurityLevel.FullTrust);
//To this (well not exactly)
//This will allow the process to crash and to be restarted if needed
//Learn how to do this because this will have to be done for each plugin, should a user wish to terminate a plugin
var hostview = mainpage.Activate<IMainHostView>(new AddInProcess(), AddInSecurityLevel.FullTrust);
```

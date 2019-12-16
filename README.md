[![Logo](https://www.creatio.com/sites/default/files/2019-10/creatio-main-logo.svg)](https://github.com/sindresorhus/awesome#readme)
# Creatio Guided Learning Dec 18-20, 2019  
This is a training project for the Guided Learning class of Dec 17-20, 2019

## Agenda

### Convert Creatio to development in FileSystem Mode. 

- Step 1 - Update [AppPath]\web.config file
```xml
<fileDesignMode enabled="true"/>
<add key="UseStaticFileContent" value="false" />
```
- Step 2 - Download packages to file system
<img src="https://academy.creatio.com/sites/default/files/documents/docs_en/technic/SDK/7.15.0/Screenshots/WorkingWithIDE/confguration_buttons.png">

- Step 3 Enable Debugging mode for client side source code
[Academy Article](https://academy.creatio.com/documents/technic-sdk/7-15/introduction-9) - Enable File System Mode
[IsDebug](https://academy.creatio.com/documents/technic-sdk/7-15/isdebug-mode) - Used to get additional debugging info.

## Tools
- [Clio](https://github.com/Advance-Technologies-Foundation/clio) - CLI Library to create packages.
- [Bpmonline.SDK](https://www.nuget.org/packages/BpmonlineSDK/) - Provides project template for development code for bpm'online platform.

## Documentation
- [Creatio](https://academy.creatio.com/documents/technic-sdk/7-15/creatio-development-guide) - Creatio Development Guide
- [Clio](https://github.com/Advance-Technologies-Foundation/clio/blob/master/README.md) - Clio

## Instructor
<a href="mailto:k.krylov@creatio.com?subject=Guided%20Learning%20Dec%2017-20,%202019">Kirill Krylov, CPA</a><br />
Direct Line: +1(617)765-7997 x.2134


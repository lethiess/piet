# Piet 

[![Build](https://github.com/lethiess/piet/actions/workflows/build.yml/badge.svg?branch=main)](https://github.com/lethiess/piet/actions/workflows/build.yml) 
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=lethiess_piet&metric=alert_status&token=a723caac6e13b3c59c89a7417d4a7cb9c2859878)](https://sonarcloud.io/summary/new_code?id=lethiess_piet) [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=lethiess_piet&metric=coverage&token=a723caac6e13b3c59c89a7417d4a7cb9c2859878)](https://sonarcloud.io/summary/new_code?id=lethiess_piet) [![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=lethiess_piet&metric=code_smells&token=a723caac6e13b3c59c89a7417d4a7cb9c2859878)](https://sonarcloud.io/summary/new_code?id=lethiess_piet) [![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=lethiess_piet&metric=ncloc&token=a723caac6e13b3c59c89a7417d4a7cb9c2859878)](https://sonarcloud.io/summary/new_code?id=lethiess_piet)


> Piet is an esoteric progrmming language invented by David Morgen-Mar where the code looks like abstract art. 
For more language specific information please refer to the original [language specification](https://www.dangermouse.net/esoteric/piet.html) or 
take a look at [my documentaion](https://github.com/lethiess/piet/blob/main/docs/PIET.md) in the ```docs``` folder.

### Overview 

This project provides an impelmentaion of a piet interpreter in combination with a simple browser based IDE written in C# using .NET and Blazor.
Blazor provides a build as WebAssembly, so you can run this project witout any additional infrastructure setup, or simply check out the 
deployed version [here](https://lethiess.github.io/piet). 

## Development

### Current status

The current version is a minimal working prototype with a small feature set, only the essential components for 
creating and executing a Piet program are available.

### Future steps

Feature                  | Description | Status
------------------------ | - | --
Improve error handling   | Catch interpreter exceptions and enable a rerun. Exceptions should be displayed via a modal | in progress
Command history          | Display the command history of an exexuted program | in progress 
Show program stack and states | Display the internal program stack and the status of CodelChooser and DirectionPointer | ready for work
Add debugging capabilities #1 | Enable a stepwise execution of the program | ready for work
Add debugging capabilities #2 | Enable backward steps during debugging | blocked by ```Add debugging capabilities #1```
Add debugging capabilities #3 | Add breakpoints to the Piet program and adapt debugger | blocked by ```Add debugging capabilities #1```
Save and load Piet programs | Add the functionality to save and load programs created with the UI. | ready for work
Import random images | Import random images (png, jpg, ...) and convert them into a Piet program | blocked by ```Save and load Piet programs```
Create programs automatically | Automate the creation of __valid__ piet programs by using math, brain and a lots of fancy buzzwords | ready for work








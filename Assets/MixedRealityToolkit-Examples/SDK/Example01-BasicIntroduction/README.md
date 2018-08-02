# Mixed Reality Toolkit - SDK - Example Scenes - Example Scene 01

![](/Assets/MixedRealityToolkit/_Core/Resources/Textures/MRTK_Logo.png)

Welcome to the Mixed Reality Toolkit. Through this first eample scene you will learn the basics for building your own Mixed Reality projects from start to finish.

## Prerequisites
To start building Mixed Reality Toolkit projects you will need:

* [Unity 2018](https://unity3d.com/get-unity/download/archive) (recommended 2018.1.9) 
* [Visual Studio 2017 Community](https://visualstudio.microsoft.com/free-developer-offers/) edition or higher
> It is possible to create standalone projects using [Visual Studio Code](https://code.visualstudio.com/), Microsofts lightweight cross-platform editor. For Windows 10 UWP projects you will need community or higher
* The Mixed Reality Toolkit assets (or download the code from the GitHub site or clone the project)

## Key concepts
With the next generation of the Mixed Reality Toolkit, there are a few things you need to understand first, primarily, most customization aspects of the toolkit are onfigured through a set of configuration profiles, each managing a specific feature of the toolkit.

### Main Mixed Reality Toolkit Configuration
![]()
This is the main configuration point for the Mixed Reality Toolkit, on selecting this profile, the Mixed Reality manager will also be added to your scene, which is required for operation.

In this profile, you will configure which systems are active in your project, as well as project wide options.

### Mixed Reality Camera Configuration
![]()
This profile sets up the default rendering options for the camera when running a Mixed Reality project.

### Mixed Reality Input Actions Configuration
![]()
This profile sets up the logical actions for your project, e.g. Fire, Run, Pickup, etc.  These are then mapped to Specific controller inputs in the Controller configuration.

### Mixed Reality Pointers Configuration
![]()
This profile defines the available pointers that will be assocated to each type of controller

### Mixed Reality Controller Configuration
![](/External/HowTo/ControllerConfigurationProfile/01-MixedRealityControllerConfigurationProfileInspector.png)
Through this profile, you will configure all the different types of controllers and SDK's your project will support

For more details, check the [Mixed Reality Controller Configuration](/Assets/MixedRealityToolkit-SDK/Profiles/MixedRealityControllerConfigurationProfile.md) documentation


## Getting started
Once you have everything listed above, create your new Unity project or a new scene in an existing project to begin.

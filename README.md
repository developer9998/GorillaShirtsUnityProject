# GorillaShirtsUnityProject
This is a Unity Project for my mod GorillaShirts which lets you put on custom shirt cosmetics in the game. With this Unity project it will let you create your own custom shirt cosmetics with a variety of different options.

## Requirements
To run the project you are going to need two things:     
- Unity Hub ([Download & Installer](https://public-cdn.cloud.unity3d.com/hub/prod/UnityHubSetup.exe))
- Unity 2022.3.2 ([Download & Changelog](https://unity.com/releases/editor/whats-new/2022.3.2))

## Installing the project
To install the project, go to the top of this page, click on the “Code” button and then click on the “Download ZIP” button that appears in the dropdown menu. Extract the ZIP folder that was downloaded onto your device, we will now call this file the project directory.

## Opening the project
In Unity Hub, click on the “Open” button, and locate the folder extracted from the ZIP you just downloaded. I recommend using 7Zip to extract your folders but you can use anything you want.

## Locating the scene
Once the project is opened, the first thing you need to do is go into the correct scene. In the bottom "Assets" tab on default, double click on the "Scenes" folder and then the "Shirt Creation" option.

## About the ShirtDescriptor
Once your scene is opened, you should see a wide selection of shirts you can adjust (Includes Hoodies, Turtlenecks, T-Shirts, Croptops) or you can use one of the four template stands to create your own shirt. Each stand has the "ShirtDescriptor" component applied to it, using this component you are able to tell the project information relating to your shirt which includes:
- Name, Author, Description
- Objects (Body, Upper Arms, Lower Arms, etc.)
- Custom Colour (All colours will match the player wearing the shirt)

<img src="https://github.com/developer9998/GorillaShirtsUnityProject/assets/81720436/4dee3310-6bbd-41a9-9f72-d5c1d7e0019a" width=30%; height=auto;><br>

## About the ShirtExporter
On the top of the Unity window, you should see a button labeled as "GorillaShirts", and then an option labeled "Shirt Exporter", once you click on the option it should open the ShirtExporter. The ShirtExporter is where you export your shirt as a ``.shirt`` file that can be used by GorillaShirts to include your shirt into the mod.

<img src="https://github.com/developer9998/GorillaShirtsUnityProject/assets/81720436/6bb64495-7d42-4750-ad0c-72e54f1d8c90" width=70%; height=auto;><br>

## Creating your shirt
To create your shirt, you must include the correct descriptor information, the correct parents for the objects, and the correct references.  

## Correct descriptor information
When creating your shirt you can edit the ShirtDescriptor data at any time by adjusting the variables. Here is a chart which contains the variable and how it plays into how your shirt is exported.
| Variable | Action |
| --- | --- |
| Name | The name of the shirt |
| Author | The author of the shirt |
| Description | The description of the shirt |
| Custom Colors | If enabled, all colours will match the player wearing the shirt |

<br><img src="https://github.com/developer9998/GorillaShirtsUnityProject/assets/81720436/2fb35cb4-dd18-4f98-bf07-07a90330261f" width=60%; height=auto;><br>

## Correct parents
For your shirt to be mapped correctly, the objects used for your shirt have to be in a certain object inside of the Gorilla rig. Here is a chart which contains the object type and what the parent of the object should be in the Gorilla rig.
| Type | Parent | Path |
| --- | --- | --- |
| Body | body | ``Gorilla Rig/body`` |
| Left Upper Arm | upper_arm.L | ``Gorilla Rig/body/shoulder.L/upper_arm.L`` |
| Left Lower Arm | forearm.L | ``Gorilla Rig/body/shoulder.L/upper_arm.L/forearm.L`` |
| Right Upper Arm | upper_arm.R | ``Gorilla Rig/body/shoulder.R/upper_arm.R`` |
| Right Lower Arm | forearm.R | ``Gorilla Rig/body/shoulder.R/upper_arm.R/forearm.R`` |              

<br><img src="https://github.com/developer9998/GorillaShirtsUnityProject/assets/81720436/658c34a2-d2f3-4cb0-bc83-1d58817547d0" width=30%; height=auto;><br>

## Correct references
Finally, you will need to connect the shirt objects to the descriptor, they can't be found on their own. For instance the Body object will go into the Body object variable, the Right Upper Hand object for instance will go into the Right Upper Hand object variable.   

<br><img src="https://github.com/developer9998/GorillaShirtsUnityProject/assets/81720436/b89aa24d-99ed-47aa-80cd-b88b44d4292c" width=40%; height=auto;><br>

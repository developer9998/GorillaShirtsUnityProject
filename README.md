# GorillaShirtsUnityProject
This is a Unity Project for my mod GorillaShirts which lets you put on custom shirt cosmetics in the game. With this Unity project it will let you create your own custom shirt cosmetics with a variety of different options.

## Requirements
To run the project you are going to need two things:     
- Unity Hub (https://public-cdn.cloud.unity3d.com/hub/prod/UnityHubSetup.exe)
- Unity 2019.3.15 (https://unity.com/releases/editor/whats-new/2019.3.15)

## Installing the project
To install the project, go to the top of this page, click on the “Code” button and then click on the “Download ZIP” button that appears in the dropdown menu. Extract the ZIP folder that was downloaded onto your device, we will now call this file the project directory.

## Opening the project
In Unity Hub, click on the “Open” button, first select the project directory, then the folder inside of it, then the folder inside the folder you just clicked onto, or just ``GorillaShirtsUnityProject-main > GorillaShirtsUnityProject-main > GorillaShirtsUnityProject``.

## Locating the scene
Once the project is opened, the first thing you need to do is go into the correct scene. In the bottom "Assets" tab on default, double click on the "Scenes" folder and then the "SampleScene" option.

## About the ShirtDescriptor
Once your scene is opened, you should see a Gorilla rig in origin with a ShirtDescriptor component, this component is very important for creating your shirts as it contains several key information about your shirt. 
- Name
- Author
- Information (Description)
- Options (Custom Colours, Fur Materials, etc.)
- Objects (Body, Upper Arms, Lower Arms, etc.)  
    
![image](https://user-images.githubusercontent.com/81720436/211389140-69117d06-aaf9-4297-a2ae-e8dd3d049393.png)

## About the ShirtExporter
On the top of the Unity window, you should see a button labeled as "GorillaShirts", and then an option labeled "Shirt Exporter", once you click on the option it should open the ShirtExporter. The ShirtExporter is where you export your shirt as a ``.shirt`` file that can be used by GorillaShirts to include your shirt into the mod.
     
![image](https://user-images.githubusercontent.com/81720436/211388818-9dc29af0-9dab-4d63-a445-6a717e47d0ef.png)

## Creating your shirt
To create your shirt, you must include the correct descriptor information, the correct parents for the objects, and the correct references.  

## Correct descriptor information
When creating your shirt you can edit the ShirtDescriptor data at any time by adjusting the variables. Here is a chart which contains the variable and how it plays into how your shirt is exported.
| Variable | Action |
| --- | --- |
| Name | The name of the shirt |
| Author | The author of the shirt |
| Info | The information/description of the shirt |
| Custom Colors | If the shirts colour should match the player colour |
     
![image](https://user-images.githubusercontent.com/81720436/211389386-6c73124d-cbce-4be3-a882-0562edffc5ca.png)

## Correct parents
For your shirt to be mapped correctly, the objects used for your shirt have to be in a certain object inside of the Gorilla rig. Here is a chart which contains the object type and what the parent of the object should be in the Gorilla rig.   
**NOTE:** Boob objects get parented to the Body object.
| Type | Parent | Path |
| --- | --- | --- |
| Body | body | ``Gorilla/metarig/body`` |
| Left Upper Arm | upper_arm.L | ``Gorilla/metarig/body/shoulder.L/upper_arm.L`` |
| Left Lower Arm | forearm.L | ``Gorilla/metarig/body/shoulder.L/upper_arm.L/forearm.L`` |
| Right Upper Arm | upper_arm.R | ``Gorilla/metarig/body/shoulder.R/upper_arm.R`` |
| Right Lower Arm | forearm.R | ``Gorilla/metarig/body/shoulder.R/upper_arm.R/forearm.R`` |              
    
![image](https://user-images.githubusercontent.com/81720436/211389085-32f59a8e-3c7c-453c-af8b-9e56bf3cc553.png)

## Correct references
Finally, you will need to connect the shirt objects to the descriptor, they can't be found on their own. For instance the Body object will go into the Body object variable, the Right Upper Hand object for instance will go into the Right Upper Hand object variable.   
    
![image](https://user-images.githubusercontent.com/81720436/211388866-1da3798a-3e14-412a-b39e-5fb99f20de47.png)

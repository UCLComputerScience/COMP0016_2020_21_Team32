# HoloRepository Portable 2021
The HoloRepository Portable 2021 is a portable, Windows-based, 3D medical model viewer application built in Unity. 
It aims to provide a convenient way for clinicians and educators to view, manipulate and store information about 3D models.

# Running the application as a user
## Requirements
* Windows 10
## Instructions
* Download [latestBuild.zip](https://github.com/UCLComputerScience/COMP0016_2020_21_Team32/releases/tag/v1.0), extract the files and run the executable.<br><br>

# Manual Deployment and Development
## Requirements
* Windows 10
* [Unity Hub](https://unity3d.com/get-unity/download) <br>
* Unity (Project was developed using Version 2020.1.17f1. Should work with all future releases. Can be installed from within Unity Hub.)<br>
## Instructions:
* Clone this repository onto your device.<br>
* In Unity hub, click "ADD" in the top right-hand corner and select the "HoloRepositoryPortable2021" directory in the cloned repo.<br>
* Click the project in Unity hub to open it in the Unity Editor. Changes can then be made to the project from within the Unity Editor.<br>
* Download the necessary example GLB models and DLLs from (here)[https://liveuclac-my.sharepoint.com/:f:/g/personal/zcabbll_ucl_ac_uk/EmdPud2uiV1KvW6wj8BSKssBgHTN3bGQObZnM8Fcpdk-UQ?e=02ebeg]
	*Copy all glb files into Assets/StreamingAssets
	*Copy all .dlls from the SITK32 directory into Assets/SimpleITK/SimpleITK-1.2.3-CSharp-win32-x86
	*Copy all .dlls from the SITK64 direction into Assets/SimpleITK/SimpleITK-2.0.2-CSharp-win64-x64
* To build the project, either navigate to:  
    * File->"build settings"->build. You will automatically be taken to the directory in which the executable is generated, which can be run.
    * File->"build and run". This will compile the project and run the executable automatically.

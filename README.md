# Unity Normal Map Editor [![License](https://img.shields.io/badge/License-MIT-lightgrey.svg?style=flat)](http://mit-license.org)
A Normal map editor built into the Unity Editor.

This repository is a Unity Editor tool (package) that allows the user to modify a Normal Map textures to achieve the following:
- inverting any of the RGB channels
- rotating the Normal Map content by 90, 180 and 270 degrees
- flipping the Normal Map content horizontally or vertically

The tool has been verified on the following versions of Unity:
- 2019.4.X
- 2019.3.X

*  *  *  *  *

## Setup
##### Option A) Clone or download the repository and drop it in your Unity project.
##### Option B) Add the repository to the package manifest (go in YourProject/Packages/ and open the "manifest.json" file and add "com..." line in the depenencies section). If you don't have Git installed, Unity will require you to install it.
```
{
  "dependencies": {
      ...
      "com.razluta.normalmapeditor": "https://github.com/razluta/UnityNormalMapEditor.git"
      ...
  }
}
```
*  *  *  *  *

![](/Screenshots/NormalMapEditor_Screenshot001.png)

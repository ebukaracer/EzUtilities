# EzUtilities
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-blue)](http://makeapullrequest.com) [![License: MIT](https://img.shields.io/badge/License-MIT-blue)](https://ebukaracer.github.io/ebukaracer/md/LICENSE.html)

**EzUtilities** is a versatile collection of utility scripts designed to streamline your Unity development workflow, making your game development experience faster and more efficient. With tools that cater to common game development needs, **EzUtilities** gives you the power to focus on creativity and iteration.

[Read Docs](https://ebukaracer.github.io/EzUtilities)

## Features
- 🚀 **Boost Development Speed**: Achieve faster iteration times with ready-to-use scripts.
- 🔧 **Versatile Tools**: Includes utilities for gameplay mechanics, editor extensions, and more.
- 🧩 **Modular Design**: Install only what you need—each utility is available independently.
- 🎯 **Editor Enhancements**: Improve your workflow with helpful editor scripts.

## Installation
There are two ways to install this package: 
- via the package manager 
- by importing it directly into your project's asset directory.

The former will include all the scripts in this package, while the latter will let you choose scripts to be imported. 

 **Method 1:** 
*In unity editor inside package manager:*
- Hit `(+)`, choose `Add package from Git URL`(Unity 2019.4+)
- Paste the `URL` for this package inside the box: https://github.com/ebukaracer/EzUtilities.git#upm
- Hit `Add`

**Method 2:**
*After you have downloaded the [latest version](https://github.com/ebukaracer/EzUtilities/releases) of this package:*
- Drag and drop the `.unitypackage` file inside your project's asset directory
- Tick/untick any script you may want to include or not
- Ensure the `.asmdef` file for this package is also ticked
- Hit on the **Import** button

![img](https://raw.githubusercontent.com/ebukaracer/ebukaracer/unlisted/EzUtilities-Images/IMPORT.png)

After successful installation or import, check out [this](https://ebukaracer.github.io/ebukaracer/md/SETUPGUIDE.html)

## Notes
- If you're using assembly definition in your project, be sure to add this package's reference, except for editor scripts.

- It's forbidden to use both installation methods, use either of the two. In the case where you may want to switch to another method, ensure no previous traces of this package are available in your project.

- Use the **Remove Package** menu option when uninstalling this package, only if it was installed using the first method above.

- If this package was imported to your project's asset directory using the second method above, simply delete the root folder `EzUtilities/`.

## [Contributing](https://ebukaracer.github.io/ebukaracer/md/CONTRIBUTING.html) 
Contributions are welcome! Please open an issue or submit a pull request.
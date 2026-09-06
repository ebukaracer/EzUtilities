# EzUtilities
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-blue)](http://makeapullrequest.com) [![License: MIT](https://img.shields.io/badge/License-MIT-blue)](https://ebukaracer.github.io/ebukaracer/md/LICENSE.html)

**EzUtilities** is a versatile collection of utility scripts designed to streamline your Unity development workflow, making your game development experience faster and more efficient. With tools that cater to common game development needs, **EzUtilities** gives you the power to focus on creativity and iteration.

[View in DocFx](https://ebukaracer.github.io/EzUtilities)

## Features
- 🚀 **Boost Development Speed**: Achieve faster iteration times with ready-to-use scripts.
- 🔧 **Versatile Tools**: Includes utilities for gameplay mechanics, editor extensions, and more.
- 🧩 **Modular Design**: Install only what you need—each utility is available independently.
- 🎯 **Editor Enhancements**: Improve your workflow with helpful editor scripts.

## Installation
| Name               | Package URL                                          | How to Install from Package Manager                                                |
| ------------------ | ---------------------------------------------------- | ---------------------------------------------------------------------------------- |
| EzUtilities.Core   | https://github.com/ebukaracer/EzUtilities.git#core   | Select **Install package from git URL** and paste the *package url* inside the box |
| EzUtilities.Common | https://github.com/ebukaracer/EzUtilities.git#common | ''                                                                                 |
| EzUtilities.Extras | https://github.com/ebukaracer/EzUtilities.git#extras | ''                                                                                 |

## Setup
Unlike `EzUtilities.Common` and `EzUtilities.Core`, `EzUtilities.Extras` is bundled with a `.unitypackage` containing various utility scripts that can be imported into your project. Some of these scripts depend on `EzUtilities.Common` and `EzUtilities.Core` to work, while some depend on the `DOTween` package. You must install these dependency packages before installing the `EzUtilities.Extras` package to avoid compile-time errors. 

After successfully installing the `EzUtilities.Extras` package through the package manager, navigate to the menu option:\
`Racer > EzUtilities.Extras > Import Scripts (force)`\
This will allow you to include and import the scripts in your project. If there are any updates to the package, repeat this operation to incorporate those updates. 

Additionally, include the `asmdef` file to save compilation time and improve the organization of the imported scripts:

![img](https://raw.githubusercontent.com/ebukaracer/ebukaracer/unlisted/EzUtilities-Images/IMPORT.png)

These scripts depend on the `DOTween` package to work:
- LoopTweenUI.cs
- TextCycler.cs 
- TextTypeWriter.cs 
- ToastUI.cs

Make sure to install `DOTween` and create its module's `asmdef` before importing any of the scripts above.

---
To remove `EzUtilities.Extras` completely (leaving no trace), navigate to: `Racer > EzUtilities.Extras > Remove package`. 

However, `EzUtilities.Core` and `EzUtilities.Common` can be manually removed from the package manager.

## [Contributing](https://ebukaracer.github.io/ebukaracer/md/CONTRIBUTING.html) 
Contributions are welcome! Please open an issue or submit a pull request.

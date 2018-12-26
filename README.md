MyLabel
=======

MyLabel is a C# software for 'two-track' class label/annotation which can be used to annotate class labels, aka ground truths, in the reference videos.

This software also utilize [MediaToolkit](https://github.com/AydinAdn/MediaToolkit), a free framework under LGPLv2.1 license which also works as a .NET wrapper for FFmpeg for audio/video processing, for trimming the reference videos. 

Content
-------

- [License](#license)
- [Features](#features)
- [How to Use](#how-to-use)

Features
--------


License
-------
- MyLabel is under MIT license.
- MediaToolkit framework, which is installed as an NuGet package, is under LGPLv2.1 license.

How to Use
----------
1. Download/Clone the MyLabel project.

2. As MediaToolkit is under different license, I do not include the MediaToolkit package with the source codes. Please install the package through [NuGet](https://www.nuget.org/packages/MediaToolkit) or visit the [project page](https://github.com/AydinAdn/MediaToolkit) for more information.

3. Once MediaToolkit is installed, you can make changes to the types of class labels and then build the project with "Release" configuration and be done! You can down then start using the software via the executable version.

How to Change Type of Class Labels
----------------------------------
TBC

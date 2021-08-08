BUILDLet Class Library for JBIG Image
=====================================

Introduction
------------

This project provides *.net* class library for JBIG (Joint Bi-level Image experts Group) Imageing.

Description
-----------

**BUILDLet.Imaging.Jbig** allows your *.net* project to read JBIG image file, by the following process.

1. Process of *jbigtopnm.exe* is invoked, and its standard output stream is redirected and captured.
As a result, the **PBM** image is captured from standard output stream of *jbigtobnm.exe*.

2. Process of *ppmtobmp.exe* is invoked, and standard input stream is redirected and **PBM** image captured from standard output stream of *jbigtopnm.exe* is input to it.
Also its standard output stream is redirected and captured.
As a result, the **Bitmap** image is captured from standard output stream of *ppmtobmp.exe*.

Therefore, **BUILDLet.Imaging.Jbig** is containing the following files.

- jbigtopnm.exe
- ppmtobmp.exe
- jbig1.dll (required for *jbigtopnm.exe*)
- libnetpbm.dll (required for *jbigtopnm.exe* and *ppmtobmp.exe*)

Getting Started
---------------

This project is provided as the following NuGet Package(s).

- [BUILDLet.Imaging.Jbig](https://www.nuget.org/packages/BUILDLet.Imaging.Jbig/)

Please refer to the help document ([BUILDLet.Imaging.Jbig.chm](./BUILDLet.Imaging.Jbig.Documentation/Help/BUILDLet.Imaging.Jbig.chm)) for the change history and API references.

Build and Test
--------------

This project (Visual Studio Solution) is built and tested on Visual Studio.

License
-------

This project is licensed under the [MIT](https://opensource.org/licenses/MIT) License.

Also, this project is using **[JBIT-KIT](https://www.cl.cam.ac.uk/~mgk25/jbigkit/)** (*jbig1.dll*) and **[NetPbm for Windows](http://gnuwin32.sourceforge.net/packages/netpbm.htm)** (*jbigtopnm.exe*, *ppmtobmp.exe* and *libnetpbm10.dll*).

- **JBIG-KIT** (*jbig1.dll*) is free software under the [GNU General Public License](http://www.gnu.org/licenses/gpl.html).

- **Netpbm** has a very complicated licensing setup, as it is a collection of hundreds of small utility programs, each with an individual copyright/license. So, though we believe that *jbigtopnm.exe* is licensed under [GNU General Public License v2](http://www.gnu.org/licenses/old-licenses/gpl-2.0.html) at least, please refer to [copyright_summary](http://netpbm.svn.code.sourceforge.net/p/netpbm/code/trunk/doc/copyright_summary) for detailed information and regarding other modules (*ppmtobmp.exe* and *libnetpbm10.dll*).

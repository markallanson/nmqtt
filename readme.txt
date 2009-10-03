Welcome to the nMQTT .Net MQTT Library.

Table of Contents
-----------------
1. Getting Source
2. Building Source
3. Running Unit Tests

1. Getting Source
-----------------
If you don't have Git on your machine, visit http://git-scm.com and grab a copy for your platform.

The source for the nMqtt library is stored on github (http://github.com). You can get a local copy 
by issuing the following command from your terminal window / command prompt / powershell session.

> git clone git://github.com/markallanson/nmqtt.git nmqtt

This command will clone a copy of the source to a new directory, "nmqtt" under your current working 
directory. If you don't have Git on your platform, visit http://git-scm.com and grab a copy 


2. Build Source
---------------

On Windows--
Install the .Net SDK and run msbuild against the Build.sln or BuildWithTests.sln.
or
Run Visual Studio, load BuildWithTests.sln and build.

On Mac OS X or Linux--

1. Install the latest Mono release ( http://www.go-mono.com/mono-downloads/download.html )
2. Install the MonoDevelop IDE ( http://monodevelop.com/ )
3. Load the .sln into MonoDevelop and build.


3. Running Unit Tests
---------------------
The unit tests for nMqtt are written using the xUnit framework ( http://www.codeplex.com/xunit ). 

The xUnit framework test runners run on both the .Net Framework and Mono platform (To run on mono 
prefix the xunit runner executables with "mono ", then issue your command line as normal (ie. the
path to the nMQTTTests.dll assembly)




## UnityOSC v1.2.


## Modifications in this Fork

- Does not initialize clients
- Server adds all new messages to a buffer, lastPackets, which is read from and emptied by the Handler into a list of packets and logs. When these packets and logs are used by the program, they must be manually emptied or they will be emptied so there are no more than 25 at any one time.

## Description

Open Sound Control classes and API for the Unity 3d game engine

Based on Bespoke Open Sound Control Library by Paul Varcholik (pvarchol@bespokesoftware.org).
Licensed under MIT license.

##How to use

Copy the src/Editor folder contents to the corresponding Editor/ folder of your Unity project. The rest can go to your e.g. Assets/ folder of the same project.

## Documentation and examples of usage

docs/doxygen/html/index.html

docs/UnityOSC_manual.pdf

docs/UnityOSC & TouchOSC Integration.pdf 

Please head to the tests/ folder for examples of usage and a TouchOSC test Unity project.

## TODO

07.11 Change string concatenations to C# string builders.

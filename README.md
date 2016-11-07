# SimpleVRBlinders
A unity project which shows a very basic implementation of VR blinders that dim peripheral vision when tilting and when flying close to scenery. 

Uses a shader that draws the blinders. it work by having a "focus" spot that 
is not dark, this focus spot moves and changes size based on response from
the player / gameworld.

You can tweak a lot of the parameters. good luck. 

It is not very complicated and is inteded as a starting point. 
If you have opinions on how to improve , please send me feedback. 
Use as you please. 

Created at a Samsung hackathon and tested on the GearVR.

Probably needs some slight adjustment to work with other headsets.

the Script TakeFlight does all the work. This must reference the "camera" rig.

When not in VR one can use a controller to fly.



README VSS 4 Swarms

Documentation
More than you need, and a lot of tutorials about how to use VSS to make data driven sound. But there are clear instructions about how to handle vss client-server setup at the code level. 
The main top-level man pages are vss3.0ref.html and vss3.0usersguide.html 
I included a lot of other doc material but you may not need it. 

VSS Source -- truly ancient archives but maintained. 
3 version zip files: 
DirectSound 
original linux (Irix) version. 
I am not sure what is in the tar/gnuzip file. 
VSS is written in C++.

Running VSS
Try to launch vss.exe at the top level of the folder. If this does not work, then go down into the vss folder and launch vss from there. If you need to run vss from the vss folder it will give you a pathname problem for the soundfile in SOUNDS. This pathname is defined in the file "Soundfile Segment and Localize.aud" as follows:
     SetDirectory samp "./SOUNDS";
This sets the pathname from the vss app launch folder to the SOUNDS folder location. 

JAudPanel is a separate application that opens a control panel with GUI widgets to control vss. JAudPanel is therefor a client of vss. You may recall the demos I presented where I first would run VSS then launch the control panel as a separate application. 
JudPanel is written in java.
The code is useful because it shows the message passing between VSS and a client. 

Message Passing
When the client and VSS run on two machines and communicate over the network, the udp serial protocol is used. This later evolved into the OSC protocol, but we can bag OSC because it is not native to VSS and when running on one machine you probably do not need it if you can use the VSS client side library. 

When the client and VSS run on the same machine, the message passing is essentially shared memory. You should be able to use the JAudPanel example to determine the simplest way to have the Swarm app send messages to the server using the native message passing library. There is a client side library that the client links to that contains the message passing calls. The message format is very simple.

Sounds
The files in the SOUNDS folder are to support the demo

Demo Apps
The applications are from the windows version of VSS. 

1. Launch vss.exe. Best to run it from a console window so that you can see the debug reports when it receives messages from the client.
2. Launch JAudPanel (double-click on jar file). This should open up a blank control panel.
3. The demo files for JAudPanel are in the folder VSS_Demo.
4. in JAudPanel open the file "PoemPhrases.ap"in VSS_Demo
5. in JAudPanel go to "Load and file" and go to the folder VSS_Demo and select the file "Soundfile Segment and Localize.aud". This is the sound synthesis config file. 

When you load the .aud file, you should see a lot of config messages stream past in the vss console window. When this stream stops you should be able to control the sound from JAudPanel. Every control action in the JAudPanel GUI should immediately create a stream of messages in the VSS console window. 

To see more details of the control data, you may replace "PoemPhrasesDemo.ap" with "Soundfile Segment and Localize.ap". The same .aud file applies.

The above app uses a sound file as a source sound. 
A simpler demo that uses a synthesized sound and makes a beat pattern: logtest.ap with logtest.aud





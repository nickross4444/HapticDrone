This repo uses Unity to create a digital twin of a drone, that is flown with mid-air haptic feedback provided by the Ultraleap Stratos Explore. Drone control is performed with QUARC and Simulink on Quanser QDrones, with tracking handled by optitrack.

To run:
Set up Qdrones, Quarc, and Optitrack as per Quanser's directions.
Open Unity project or built executable and Simulink Models.
Plug in, power on, and test Stratos Explore.
Run Mission Server. Once the timer is running, run the Commander Stabilizer.
In the Unity Haptic Drone Controller, enter the port the simulink Mission server is running on(should be on default), and click connect. Click the networking button when it highlights(this indicates a succesful connection).
Turn transmitter switches to their flight positions, and follow on-screen instrucitons to fly.

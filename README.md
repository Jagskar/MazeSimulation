# MazeSimulation
Creating a static maze in Unity to simulate a remote environment. This project uses ProGrids and ProBuilder for constructing the environment and Cinemachine was eventually used for camera implementation.

# Important Scripts:
- Assets/GameManager.cs: GameManager class that listens for when the user wants to change camera angles, checks the world state at each epoch to see if the game is in a completed state i.e. all collectables in the environment have been collected. Class also times each run and measures how many times user switches between the different camera views.
- Assets/CameraZoom.cs: Defines the camera zooming in/out functionality.
- Assets/CollectableBehaviour.cs: Defines the behaviour of the collectables in the environment - rotation speed and collision detection
- Assets/MainMenu.cs: Class for setting the Main Menu scene as the active scene for when the simulation begins
- Assets/MouseLook.cs: Logic for rotating the camera using the mouse. DeltaTime used for accurate time calculation.
- Assets/OptionsMenu.cs: Class for setting the Options Menu scene as the active scene for when the options menu button is clicked from the main menu. It's used to select the camera mode: First-person, Third-person, Birds-eye view and free camera (user can switch between all 3 modes whenever they choose)
- Assets/PauseMenu.cs: Class for setting the Pause Menu scen as the active scene for when the simulation is paused. This happens at different times e.g. when a set number of collectables have been collected by the player. While paused, the player would be asked a number of questions based on the SAGAT technique to measure Situational Awareness during the simulation.
- Assets/PlayerMovement.cs: Logic for robot movement. Setting velocity with respect to gravity and time.
- Assets/ThirdPersonCameraController.cs: Logic for controlling the camera while in third-person camera mode.
- Assets/ThirdPersonMovement.cs: Logic for controlling player movement while in third-person camera mode.
- Assets/TopDownMouseLook.cs: Camera controls for the top-down/birds-eye camera mode - user can pan the camera up, down, left or right using the arrow keys.


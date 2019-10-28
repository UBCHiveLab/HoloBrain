the bulk of the functionality (moving the brain around, rotating, expanding, isolating) is provided in monobehaviours attached to the HologramCollection -> Brain GameObject. Here I will overview what each one does, and describe some pain points.

IsolateStructures
* This is probably the largest file in terms of lines of code, but a large portion of this is dedicated to declaring structures that will make it easier to decide what to do with each structure in the different states of isolation. This is what MovingStructure and MovingStructureWithDirection (just a decorated MovingStructure) do.
* The bulk of what this class does is 1. take messages for when we enter isolate mode and exit isolate mode and change internal variables. 2. the update function, depending on what the internal state is, will move structures towards their designated position, and if any reach their destination within a bounds, "clicks" them into place.
* When a structure is isolated, this script will instantiate a new copy of this structure to us in the isolation animation. When isolate mode is exited, all copies are destroyed.

RotateStructures
* Takes care of rotating everything that is inside the HologramCollection/Brain GameObject by rotating the gameobject itself.
* takes messages to handle an internal enumeration of states that the update function then uses to see whether or not the rotation should continue.

ExplodingCommands
* Uses a similar private class as IsolateStructures to store where each of the structures is going as the final position for each structure will be different based on where they are in reference to the center of the brain.
* once all of those final positions are decided and stored, the monobehaviour takes messages to update internal state based on an enumeration. Update uses this enumeration to iterate through the structures and update their positions

HologramPlacement
* Has an internal boolean for whether or not the position of the brain has been decided. If it has not, then update will move the brain to a variable distance in front of the camera, and handles a click (implements IInputClickHanlder) for positioning the brain.
* There is some code in here that goes outside of the responsibility and thus needs to be cleaned up.

ResetState
* This script is supposed to put the brain back into its default state (not isolated, not expanded). Since the way that the different states are supposed to interact with each other is not well defined, this script is a pain point and for now it has been unlinked.

StateAccessor
* This script is supposed to provide an interface for changing app states, but for the same reason as above, it is unused.

ObjectsToRecord and ApplyAppConfiguration are parts of seperate systems, refere to dataRecording.md for ObjectsToRecord and configuration.md for details.


VoiceCommandPrompt.cs
* monobehaviour attached to any gameobject which represents a button that has a voice command also assigned to it. It implements IFocusable and uses OnFocusEnter and OnFocusExit to change the text of the PromptArea parameter to whatever the appropriate voice command is.

PlayVoiceOverCommand.cs
* Any gameobjects that we want to trigger voiceovers for, this script will attach a handler that plays the audio file in the paramater to the gameobject's ButtonCommands script

PreviewInteraction.cs
* this monobehaviour takes a list of sprites. Implements IFocusable and uses these events to pass its set of frames to PreviewPlayer, which then takes care of animating the frames. This script is used to give users a preview to what is going to happen if they click that button

ButtonAppearance.cs
* MonoBehaviour that changes the sprite on the button it is attached to depending on what we're doing
* Right now it only implements IFocusable to give the button it is attached to some hover feedback, but ButtonCommands actually will call ButtonAppearance.SetButtonActive().
* Need a way to decide when buttons will become inactive (they stay active)

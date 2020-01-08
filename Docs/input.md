There have been a few occasions where we have had to change the code for handling inputs on buttons as a result of changes to holotoolkit so i have decided to encapsulate using the ButtonCommands script.

Critical scripts for input events are attached to the GameObject PreLoad -> InputManager

ButtonCommands exposes a function to add handlers to its multidelegate (AddCommand). (to add more click functionality to a button, attach a script and interit from abstract class CommandToExecute, which inherits from MonoBehaviour but has a virtual Start function that will look for ButtonCommands on the gameObject and add call AddCommand with the abstract function Action Command(). Override this function to provide custom fxnality. 

in the case of future changes of the input interface, the place to change is ButtonCommands. As it is now, ButtonCommands implements IFocusable and IInputClickHanlder. IFocusable declares OnFocusEnter and OnFocusExit (no params) and IInputClickHandler declares OnInputClicked(InputClickedEventData)

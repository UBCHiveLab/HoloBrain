a large part of the functionality of the menu itself lies in enabling or disabling certain groups of gameObject.
For this purpose, we use the SwitchRoomUI.cs Monobehaviour.
	* Attach this script to parent, it has two lists as public, the first is a list of buttons and the second is a list of the gameobject groups you want to control through those buttons.
	* The scripts works as follows: the index in the list of the button that gets pressed will disable all gameobjects in the second list and enable the one with the same index. 
	* this script is very generalized (though all buttons in the first list must have ButtonCommands as this is where we add our click handler.)
	* more complex menu behaviour can be composed by nesting SwitchRoomUI, and by using empty gameObjects as masks

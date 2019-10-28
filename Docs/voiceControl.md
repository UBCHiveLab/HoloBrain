Following the ideology that every interaction should be accessible through multiple inputs, VoiceControl.cs uses handlers to execute a "click" on the correct gameObject.

Depending on the what the best way to find the button to click is, HandleCommand is overridden with a variety of parameters. Predicate which should be satisfied (to check that the app is in the correct state before executing click, or an arbitrary condition), a Type to help the handler find the right button, and a string to further help the handler identify the gameobject by name as a last resort, should there be multiple buttons with the same monobehaviour attached.

HandleClearMarker and HandleAddMarker have not been transitioned into this method of executing voice commands in part because there is not button to click and also because the functionality has not yet been incorporated into the app


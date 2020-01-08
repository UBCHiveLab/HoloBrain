DataRecorder.md is a MonoBehaviour attached to PreLoad, and it handles writing a text file that represents all activity in that session. It exposes a function void QueueMessage(string) which it then adds to a queue of messages along with a time stamp.
After a variable amount of time the queue will be flushed and all messages will be written to the text file.
Attach the ObjectsToRecords.cs script to any gameobjects for which you want to record position and rotation, and it will call QueueMessage every frame (we might want to make how often messages are sent variable but for now it works fine)


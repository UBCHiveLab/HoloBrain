using HoloToolkit.Sharing;
using HoloToolkit.Unity;
using System.Collections.Generic;
using UnityEngine;

public class CustomMessages : Singleton<CustomMessages>
{
    /// <summary>
    /// Message enum containing our information bytes to share.
    /// The first message type has to start with UserMessageIDStart
    /// so as not to conflict with HoloToolkit internal messages.
    /// </summary>
    public enum TestMessageID : byte
    {
        HeadTransform = MessageID.UserMessageIDStart,
        StageTransform,
        ResetStage,
        ToggleHighLight,
        scaleChange,
        ToggleRotate,
        ToggleExplode,
        ToggleOpacity,
        ToggleMRI,
        ToggleMRIButton,
        ToggleMRIImages,
        ResetState,
        InitiateIsolateMessage,
        TryToIsolateStructure,
        TryToReturnIsolatedStructure,
        ConcludeIsolate,
        SetPositionOfGazeMarker,
        ClearGazeMarker,
        Max
    }

    public enum UserMessageChannels
    {
        Anchors = MessageChannel.UserMessageChannelStart,
    }

    /// <summary>
    /// Cache the local user's ID to use when sending messages
    /// </summary>
    public long localUserID
    {
        get; set;
    }

    public delegate void MessageCallback(NetworkInMessage msg);
    private Dictionary<TestMessageID, MessageCallback> _MessageHandlers = new Dictionary<TestMessageID, MessageCallback>();
    public Dictionary<TestMessageID, MessageCallback> MessageHandlers
    {
        get
        {
            return _MessageHandlers;
        }
    }

    /// <summary>
    /// Helper object that we use to route incoming message callbacks to the member
    /// functions of this class
    /// </summary>
    NetworkConnectionAdapter connectionAdapter;

    /// <summary>
    /// Cache the connection object for the sharing service
    /// </summary>
    NetworkConnection serverConnection;

    void Start()
    {
        InitializeMessageHandlers();
    }

    void InitializeMessageHandlers()
    {
        SharingStage sharingStage = SharingStage.Instance;
        if (sharingStage != null)
        {
            serverConnection = sharingStage.Manager.GetServerConnection();
            connectionAdapter = new NetworkConnectionAdapter();
        }

        connectionAdapter.MessageReceivedCallback += OnMessageReceived;

        // Cache the local user ID
        this.localUserID = SharingStage.Instance.Manager.GetLocalUser().GetID();

        for (byte index = (byte)TestMessageID.HeadTransform; index < (byte)TestMessageID.Max; index++)
        {
            if (MessageHandlers.ContainsKey((TestMessageID)index) == false)
            {
                MessageHandlers.Add((TestMessageID)index, null);
            }

            serverConnection.AddListener(index, connectionAdapter);
        }

        //// Add the message handler for toggling highlight
        MessageHandlers[CustomMessages.TestMessageID.ToggleHighLight] = this.ToggleHighLightMessageReceived;
        MessageHandlers[CustomMessages.TestMessageID.ToggleMRI] = this.ToggleMRIMessageReceived;
    }

    private NetworkOutMessage CreateMessage(byte MessageType)
    {
        NetworkOutMessage msg = serverConnection.CreateMessage(MessageType);
        msg.Write(MessageType);
        // Add the local userID so that the remote clients know whose message they are receiving
        msg.Write(localUserID);
        return msg;
    }

    public void SendHeadTransform(Vector3 position, Quaternion rotation, byte HasAnchor)
    {
        // If we are connected to a session, broadcast our head info
        if (this.serverConnection != null && this.serverConnection.IsConnected())
        {
            // Create an outgoing network message to contain all the info we want to send
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.HeadTransform);

            AppendTransform(msg, position, rotation);

            msg.Write(HasAnchor);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.UnreliableSequenced,
                MessageChannel.Avatar);
        }
    }

    public void SendToggleRotateMessage(bool isRotating)
    {
        if (this.serverConnection != null && this.serverConnection.IsConnected())
        {
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.ToggleRotate);

            // We can't directly write bools to the message so we have to write bytes
            msg.Write(isRotating ? (byte)1 : (byte)0);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);

            Debug.Log("Toggle rotate message was sent");
        }
    }

    public void SendMRIButtonMessage(bool isInMRIMode)
    {
        // If we are connected to a session, send the test message
        if (this.serverConnection != null && this.serverConnection.IsConnected())
        {
            // Create an outgoing network message to contain all the info we want to send.
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.ToggleMRIButton);

            // We can't directly write bools to the message so we have to write bytes
            msg.Write(isInMRIMode ? (byte)1 : (byte)0);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);

            Debug.Log("Toggle MRI message was sent");
        }
    }

    public void SendChangeMRIImageButtonMessage(bool isOutlinedMRIImages)
    {
        // If we are connected to a session, send the test message
        if (this.serverConnection != null && this.serverConnection.IsConnected())
        {
            // Create an outgoing network message to contain all the info we want to send.
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.ToggleMRIImages);

            // We can't directly write bools to the message so we have to write bytes
            msg.Write(isOutlinedMRIImages ? (byte)1 : (byte)0);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);

            Debug.Log("Toggle MRI Images was sent");
        }
    }


    public void ToggleHighLightMessageReceived(NetworkInMessage msg)
    {
        // This reads the user ID which we do not need
        msg.ReadInt64();

        string toggledStructure = msg.ReadString();
        Debug.Log("ToggleHighlight message received for " + toggledStructure);
        GameObject.Find(toggledStructure).GetComponent<HighlightAndLabelCommands>().ToggleHighlightMessageReceived(msg);
    }

    public void ToggleMRIMessageReceived(NetworkInMessage msg)
    {
        msg.ReadInt64();

        string toggledMRI = msg.ReadString();
        bool isSectionSelected = msg.ReadByte() == 1;

        MRIManager.Instance.ProcessMRISelectionReceived(toggledMRI, isSectionSelected);
    }

    public void SendToggleHighlightMessage(string modelName, bool isLocked)
    {
        if (this.serverConnection != null && this.serverConnection.IsConnected())
        {
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.ToggleHighLight);
            msg.Write(modelName);

            // We can't directly write bools to the message so we have to write bytes
            msg.Write(isLocked ? (byte) 1 : (byte) 0);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);

            Debug.Log("Toggle highlight message for '" + modelName + "' was sent");
        }
    }
    public void SendToggleMRIMessage(string mriName, bool isSectionSelected)
    {
        // If we are connected to a session, send the test message
        if (this.serverConnection != null && this.serverConnection.IsConnected())
        {
            // Create an outgoing network message to contain all the info we want to send.
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.ToggleMRI);

            // We can't directly write bools to the message so we have to write bytes
            msg.Write(mriName);
            msg.Write(isSectionSelected ? (byte)1 : (byte)0);


            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);

            Debug.Log("Toggle highlight message for '" + mriName + "' was sent");
        }
    }

    public void ToggleHighlightMessageReceived(NetworkInMessage msg)
    {
        // This reads the user ID which we do not need
        msg.ReadInt64();

        string toggledStructure = msg.ReadString();
        Debug.Log("ToggleHighlight message received for " + this.name + " from " + toggledStructure);
        GameObject.Find(toggledStructure).GetComponent<HighlightAndLabelCommands>().ToggleLockedHighlight();
    }

    public void SendScaleChangeMessage(byte newScale)
    {
        if (this.serverConnection != null && this.serverConnection.IsConnected())
        {
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.scaleChange);
            msg.Write(newScale);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);

            Debug.Log("Scale change message was sent");
        }
    }

    public void SendToggleExplodeMessage(byte lastState)
    {
        if (this.serverConnection != null && this.serverConnection.IsConnected())
        {
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.ToggleExplode);
            msg.Write(lastState);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);

            Debug.Log("Toggle explode message was sent");
        }
    }

    public void SendToggleOpacityMessage(byte lastState)
    {
        if (this.serverConnection != null && this.serverConnection.IsConnected())
        {
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.ToggleOpacity);
            msg.Write(lastState);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);

            Debug.Log("Toggle opacity message was sent");
        }
    }

    public void SendResetStateMessage()
    {
        if (this.serverConnection != null && this.serverConnection.IsConnected())
        {
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.ResetState);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);

            Debug.Log("Reset state message was sent");
        }
    }

    public void SendInitiateIsolateMessage()
    {
        if (this.serverConnection != null && this.serverConnection.IsConnected())
        {
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.InitiateIsolateMessage);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);

            Debug.Log("Initiate isolate message was sent");
        }
    }

    public void SendTryToIsolateStructureMessage(string structureName)
    {
        if (this.serverConnection != null && this.serverConnection.IsConnected())
        {
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.TryToIsolateStructure);
            msg.Write(structureName);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);

            Debug.Log("Try to isolate structure message was sent for structure " + structureName);
        }
    }

    public void SendTryToReturnIsolatedStructureMessage(string structureName)
    {
        if (this.serverConnection != null && this.serverConnection.IsConnected())
        {
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.TryToReturnIsolatedStructure);
            msg.Write(structureName);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);

            Debug.Log("Try to return isolated structure message was sent for structure " + structureName);
        }
    }

    public void SendConcludeIsolateMessage()
    {
        if (this.serverConnection != null && this.serverConnection.IsConnected())
        {
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.ConcludeIsolate);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);

            Debug.Log("Conclude isolate message was sent");
        }
    }

    public void SendSetGazeMarkerPositionMessage(Vector3 position, string markedPartName)
    {
        if (this.serverConnection != null && this.serverConnection.IsConnected())
        {
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.SetPositionOfGazeMarker);

            AppendVector3(msg, position);
            msg.Write(markedPartName);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);

            Debug.Log("Set gaze marker position message was sent");
        }
    }

    public void SendClearGazeMarkerMessage()
    {
        if (this.serverConnection != null && this.serverConnection.IsConnected())
        {
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.ClearGazeMarker);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);

            Debug.Log("Clear gaze marker message was sent");
        }
    }

    public void SendStageTransform(Vector3 position, Quaternion rotation)
    {
        if (this.serverConnection != null && this.serverConnection.IsConnected())
        {
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.StageTransform);

            AppendTransform(msg, position, rotation);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);
        }
    }

    public void SendResetStage()
    {
        if (this.serverConnection != null && this.serverConnection.IsConnected())
        {
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.ResetStage);

            // Send the message as a broadcast, which will cause the server to forward it to all other users in the session.
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.ReliableOrdered,
                MessageChannel.Avatar);
        }
    }

    void OnDestroy()
    {
        if (this.serverConnection != null)
        {
            for (byte index = (byte)TestMessageID.HeadTransform; index < (byte)TestMessageID.Max; index++)
            {
                this.serverConnection.RemoveListener(index, this.connectionAdapter);
            }
            this.connectionAdapter.MessageReceivedCallback -= OnMessageReceived;
        }
    }

    void OnMessageReceived(NetworkConnection connection, NetworkInMessage msg)
    {
        byte messageType = msg.ReadByte();
        MessageCallback messageHandler = MessageHandlers[(TestMessageID)messageType];
        if (messageHandler != null)
        {
            messageHandler(msg);
        }
    }

    #region HelperFunctionsForWriting

    void AppendTransform(NetworkOutMessage msg, Vector3 position, Quaternion rotation)
    {
        AppendVector3(msg, position);
        AppendQuaternion(msg, rotation);
    }

    void AppendVector3(NetworkOutMessage msg, Vector3 vector)
    {
        msg.Write(vector.x);
        msg.Write(vector.y);
        msg.Write(vector.z);
    }

    void AppendQuaternion(NetworkOutMessage msg, Quaternion rotation)
    {
        msg.Write(rotation.x);
        msg.Write(rotation.y);
        msg.Write(rotation.z);
        msg.Write(rotation.w);
    }

    #endregion HelperFunctionsForWriting

    #region HelperFunctionsForReading

    public Vector3 ReadVector3(NetworkInMessage msg)
    {
        return new Vector3(msg.ReadFloat(), msg.ReadFloat(), msg.ReadFloat());
    }

    public Quaternion ReadQuaternion(NetworkInMessage msg)
    {
        return new Quaternion(msg.ReadFloat(), msg.ReadFloat(), msg.ReadFloat(), msg.ReadFloat());
    }

    #endregion HelperFunctionsForReading
}
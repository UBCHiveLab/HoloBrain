// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using UnityEngine.SceneManagement;

namespace HoloToolkit.Sharing
{
    public class AutoJoinSession : MonoBehaviour
    {
        /// <summary>
        /// Name of the session to join.
        /// </summary>
        public string SessionName = "1234";

        /// <summary>
        /// Cached pointer to the sessions tracker.
        /// </summary>
        private ServerSessionsTracker sessionsTracker;

        private bool sessionCreationRequested = false;
        private string previousSessionName;
        private string mode;

        private void OnLevelWasLoaded(int level)
        {
            mode = PlayerPrefs.GetString("mode");
            if (mode == "professor")
            {
                SessionName = GenerateRandomNo() + "";
                Debug.Log("The session name is " + SessionName);                
            }
            PlayerPrefs.SetString("session", SessionName);


        }

        void Start()
        {
            SessionName = PlayerPrefs.GetString("session");

            // Get the ServerSessionsTracker to use later.  Note that if this processes takes the role of a secondary client,
            // then the sessionsTracker will always be null
            if (SharingStage.Instance != null && SharingStage.Instance.Manager != null)
            {
                this.sessionsTracker = SharingStage.Instance.SessionsTracker;
            }
            if (mode == "professor")
            {
                while (sessionsTracker.SessionExists(SessionName))
                {
                    Debug.Log("the session " + SessionName + "exists will generate a new one");
                    SessionName = GenerateRandomNo() + "";
                }
                PlayerPrefs.SetString("session", SessionName);
                Debug.Log("in auto join start: playerprefs mode is set to " + SessionName);
            }

            if (mode != null)
            {
                Debug.Log(" app state manager , the mode is "+ mode);
            }
           
           
        }

        void Update()
        {
            if (previousSessionName != SessionName)
            {
                sessionCreationRequested = false;
                previousSessionName = SessionName;
            }

            // If we are a Primary Client and can join sessions...
            if (this.sessionsTracker != null && this.sessionsTracker.Sessions.Count > 0)
            {
                // Check to see if we aren't already in the desired session
                Session currentSession = this.sessionsTracker.GetCurrentSession();

                if (currentSession == null ||                                                       // We aren't in any session
                    currentSession.GetName().GetString() != this.SessionName ||                     // We're in the wrong session
                    currentSession.GetMachineSessionState() == MachineSessionState.DISCONNECTED)    // We aren't joined or joining the right session
                {
                    Debug.Log("Session conn " + this.sessionsTracker.IsServerConnected + " sessions: " + this.sessionsTracker.Sessions.Count);
                    Debug.Log("Looking for " + SessionName);
                    bool sessionFound = false;

                    for (int i = 0; i < this.sessionsTracker.Sessions.Count; ++i)
                    {
                        Session session = this.sessionsTracker.Sessions[i];

                        if (session.GetName().GetString() == this.SessionName)
                        {
                            this.sessionsTracker.JoinSession(session);
                            sessionFound = true;
                            break;
                        }
                    }
                    if (this.sessionsTracker.IsServerConnected && !sessionFound && !sessionCreationRequested) {
                        Debug.Log("Didn't find session, making a new one");
                        if (mode == "student")
                        {
                            Debug.Log("Student, and couldn't find session");
                            SceneManager.LoadScene("EnterSessionIDScene");
                        }
                        if (this.sessionsTracker.CreateSession(new XString(SessionName)))
                        {
                            sessionCreationRequested = true;
                        }
                    }
                }
            }            
        }

        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            System.Random _rdm = new System.Random();
            int theNewSessionNumber = _rdm.Next(_min, _max);
            Debug.Log("the new random session number is " + theNewSessionNumber);
            return theNewSessionNumber;
        }

        public bool SessionExists(string id)
        {
            Debug.Log("iNSIDE SESSION EXISTS, ID IS " + id);
            return this.sessionsTracker.SessionExists(id);
        }

        public int CurrentUserCount()
        {
            return this.sessionsTracker.GetCurrentSession().GetUserCount();
        }
    }
}
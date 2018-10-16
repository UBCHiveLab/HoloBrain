namespace UIConfig
{
    using UnityEngine;
    using System.Collections.Generic;
    using System.IO;

    public class ConfigReader
    {
        private static ConfigReader instance = null;
        private string configPath = "Assets\\Resources\\config.csv";

        //Singleton pattern
        public static ConfigReader Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new ConfigReader();
                }
                return instance;
            }
        }

        public delegate void ConfigReadyEventHandler(object source, StdConfig conf);

        public event ConfigReadyEventHandler ConfigReady;

        //use configPath to interpret config file and fill out a StdConfig object,
        //pass StdConfig in event to listeners
        public void ReadConfigFile()
        {
            TextAsset configData = (TextAsset)Resources.Load("config");
            string fileData = configData.text;
            string[] lines = fileData.Split('\n');
            string[] data;
            StdConfig test = new StdConfig();
            foreach (string line in lines)
            {
                data = (line.Trim()).Split(',');
                test.AddParameter(data[0], data[1]);
            }
            OnConfigReady(test);
        }

        protected virtual void OnConfigReady(StdConfig conf)
        {
            if (ConfigReady != null)
            {
                ConfigReady(this, conf);
            }
        }

        //use the settings obj to fill out a StdConfig object
        public StdConfig RcvConfigUI(object settings)
        {
            return new StdConfig();
        }

        //NEED TO RESEARCH how to pass in a config object from a network endpoint
        public StdConfig RcvConfigNetwork()
        {
            return new StdConfig();
        }
        
    }
    /*
    public class ConfigDisseminator
    {
        private static ConfigDisseminator instance = null;
        private GameObject UI;

        private ConfigDisseminator()
        {
            //find the UI parent GameObject
            UI = GameObject.Find("ControlsUI");
        }

        //Singleton pattern
        public static ConfigDisseminator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConfigDisseminator();
                }
                return instance;
            }
        }

        //iterate through all ui elements that have a ConfigListener and invoke the UpdateEvent;
        public void Disseminate()
        {
            return;
        }
    }
    */
    public class StdConfig
    {
        IDictionary<string, object> parameters;
        //standardized config parameters go here after being interpreted by ConfigReader
        public StdConfig()
        {
            parameters = new Dictionary<string, object>();
        }

        public void AddParameter(string name, object val)
        {
            if(parameters.ContainsKey(name))
            {
                parameters[name] = val;
            } else
            {
                parameters.Add(name, val);
            }
        }
    }
}
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
using System.Collections.Generic;
namespace HolobrainConstants {

    public class Names { 
        public const string BRAIN_PARTS_GAMEOBJECT_NAME = "BrainParts";
        public const string BRAIN_GAMEOBJECT_NAME = "Brain";
        public const string BRAIN_MINIMAP_GAMEOBJECT_NAME = "MinimapPositionObject";
        public const string HOLOGRAM_COLLECTION_GAMEOBJECT_NAME = "HologramCollection";
        public const string CONTROLS_UI_GAMEOBJECT_NAME = "ControlsUI";
        public const string LOADING_SCREEN_GAMEOBJECT_NAME = "LoadingScreen";

        public const string CORTEX_GAMEOBJECT_NAME = "Cortex";
        public const string CEREBELLUM_GAMEOBJECT_NAME = "Cerebellum";
        public const string MAMMILLARY_BODIES_GAMEOBJECT_NAME = "Mammillary Bodies";
        public const string THALAMUS_GAMEOBJECT_NAME = "Thalamus";
        public const string VENTRICLES_GAMEOBJECT_NAME = "Ventricles";
        public const string GLOBUS_PALLIDUS_GAMEOBJECT_NAME = "Globus Pallidus";
        public const string PUTAMEN_GAMEOBJECT_NAME = "Putamen";
        public const string SUBSTANTIA_NIGRA_GAMEOBJECT_NAME = "Substantia Nigra";
        public const string SUBTHALAMIC_NUCLEI_GAMEOBJECT_NAME = "Subthalamic Nuclei";
        public const string AMYGDALA_GAMEOBJECT_NAME = "Amygdala";
        public const string CAUDATE_GAMEOBJECT_NAME = "Caudate";
        public const string MAMMILLOTHALAMIC_TRACT_GAMEOBJECT_NAME = "Mammillothalamic Tract";
        public const string HIPPOCAMPUS_GAMEOBJECT_NAME = "Hippocampus";
        public const string FORNIX_GAMEOBJECT_NAME = "Fornix";
        public const string ARTERIES_GAMEOBJECT_NAME = "Arteries";
        public const string SINUSES_GAMEOBJECT_NAME = "Sinuses";

        public static List<string> GetStructureNames()
        {
            string[] names = {CORTEX_GAMEOBJECT_NAME, CEREBELLUM_GAMEOBJECT_NAME, MAMMILLARY_BODIES_GAMEOBJECT_NAME, THALAMUS_GAMEOBJECT_NAME, VENTRICLES_GAMEOBJECT_NAME,
            GLOBUS_PALLIDUS_GAMEOBJECT_NAME, PUTAMEN_GAMEOBJECT_NAME, SUBSTANTIA_NIGRA_GAMEOBJECT_NAME, SUBTHALAMIC_NUCLEI_GAMEOBJECT_NAME, AMYGDALA_GAMEOBJECT_NAME,
            CAUDATE_GAMEOBJECT_NAME, MAMMILLOTHALAMIC_TRACT_GAMEOBJECT_NAME, HIPPOCAMPUS_GAMEOBJECT_NAME, FORNIX_GAMEOBJECT_NAME, ARTERIES_GAMEOBJECT_NAME, SINUSES_GAMEOBJECT_NAME};
            List<string> result = new List<string>();
            result.AddRange(names);
            return result;
        }
    }
}
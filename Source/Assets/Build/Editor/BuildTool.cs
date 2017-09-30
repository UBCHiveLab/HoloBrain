// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.IO;

using UnityEditor;

using UnityEngine;


public class BuildTool : MonoBehaviour {


    public const string VS_BUILD_DIR_NAME = "VSBuild";


    [MenuItem("Hololens Build With Postprocess")]
    

    public static void BuildGame() {


        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.WSAPlayer);

        EditorUserBuildSettings.wsaSDK = WSASDK.UWP;

        EditorUserBuildSettings.wsaUWPBuildType = WSAUWPBuildType.D3D;

        EditorUserBuildSettings.wsaBuildAndRunDeployTarget = WSABuildAndRunDeployTarget.LocalMachine;

        EditorUserBuildSettings.wsaSubtarget = WSASubtarget.HoloLens;


        
	EditorBuildSettingsScene[] projectScenes =  EditorBuildSettings.scenes;

        string outPath = Path.GetDirectoryName(Path.GetFullPath(Application.dataPath)) + "/" + VS_BUILD_DIR_NAME;


        BuildPipeline.BuildPlayer(projectScenes, outPath, BuildTarget.WSAPlayer, BuildOptions.Development);
    
    }

}

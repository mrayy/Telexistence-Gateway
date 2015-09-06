using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(OVRMirror))]
public class OVRMirrorInspector : Editor
{
	public override void OnInspectorGUI()
	{
		OVRMirror ovrMirror = (OVRMirror)target;

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		ovrMirror._inspectorAdvanced = GUILayout.Toggle( ovrMirror._inspectorAdvanced, "Advanced" );
		EditorGUILayout.EndHorizontal();

		ovrMirror.oculusEnabled = EditorGUILayout.Toggle( "Oculus Enabled", ovrMirror.oculusEnabled );
		GUI.enabled = ovrMirror.oculusEnabled;
		ovrMirror.oculusCameraBinded = EditorGUILayout.Toggle( "Oculus Camera Binded", ovrMirror.oculusCameraBinded );
		GUI.enabled = true;

		if( ovrMirror._inspectorAdvanced ) {
			EditorGUILayout.Separator();
			EditorGUILayout.LabelField( "Window", EditorStyles.boldLabel );
			ovrMirror.openWindowOnEnable = EditorGUILayout.Toggle( "Open Window On Enable", ovrMirror.openWindowOnEnable );
			ovrMirror.openWindowWithMaximized = EditorGUILayout.Toggle( "Open Window With Maximized", ovrMirror.openWindowWithMaximized );
			GUI.enabled = !(ovrMirror.oculusEnabled && ovrMirror.oculusCameraBinded);
			ovrMirror.disableCameraOnCloseWindow = EditorGUILayout.Toggle( "Disable Camera On Close Window", ovrMirror.disableCameraOnCloseWindow );
			GUI.enabled = !(ovrMirror.oculusEnabled && ovrMirror.oculusCameraBinded);
			ovrMirror.overwriteViewportRect = EditorGUILayout.Toggle( "Overwrite Viewport Rect", ovrMirror.overwriteViewportRect );
			GUI.enabled = true;

			EditorGUILayout.Separator();
			EditorGUILayout.LabelField( "Oculus", EditorStyles.boldLabel );
			GUI.enabled = ovrMirror.oculusEnabled;
			ovrMirror.oculusRenderEventID = EditorGUILayout.IntField( "Render Event ID", ovrMirror.oculusRenderEventID );
			GUI.enabled = ovrMirror.oculusEnabled && ovrMirror.oculusCameraBinded;
			ovrMirror.oculusCameraViewportHeight = EditorGUILayout.FloatField( "Viewport Height", ovrMirror.oculusCameraViewportHeight );
			GUI.enabled = true;

			EditorGUILayout.Separator();
			EditorGUILayout.LabelField( "Plugin", EditorStyles.boldLabel );
			GUI.enabled = !ovrMirror.oculusEnabled;
			ovrMirror.issuePluginEventEnabled = EditorGUILayout.Toggle( "Issue Plugin Event", ovrMirror.issuePluginEventEnabled );
			ovrMirror.renderEventIDEnabled = EditorGUILayout.Toggle( "Render Event ID Enabled", ovrMirror.renderEventIDEnabled );
			GUI.enabled = !ovrMirror.oculusEnabled && ovrMirror.renderEventIDEnabled;
			ovrMirror.renderEventID = EditorGUILayout.IntField( "Render Event ID", ovrMirror.renderEventID );
			GUI.enabled = true;

			EditorGUILayout.Separator();
			EditorGUILayout.LabelField( "Objects", EditorStyles.boldLabel );
			ovrMirror.targetCamera = (Camera)EditorGUILayout.ObjectField( "Target Camera", ovrMirror.targetCamera, typeof(Camera), true );
			ovrMirror.targetTexture = (Texture)EditorGUILayout.ObjectField( "Target Texture", ovrMirror.targetTexture, typeof(Texture), true );
		}

		//ovrMirror.
		//public bool overwriteViewportRect = true;
	}
}

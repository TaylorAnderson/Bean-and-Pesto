//Copyright (c) 2016-2017 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(SuperCameraAnchor))]
[CanEditMultipleObjects]
public class SuperCameraAnchorEditor : Editor{
	override public void OnInspectorGUI(){
		serializedObject.Update(); //for onvalidate stuff!
		var anchor = target as SuperCameraAnchor; //get this text mesh as a component

		//SerializedProperty manualRefresh = serializedObject.FindProperty("manualRefresh");
		SerializedProperty checkForResolutionChange = serializedObject.FindProperty("checkForResolutionChange");
		SerializedProperty followCameraMovement = serializedObject.FindProperty("followCameraMovement");
		SerializedProperty cam = serializedObject.FindProperty("cam");
		SerializedProperty horizontalPosition = serializedObject.FindProperty("horizontalPosition");
		SerializedProperty verticalPosition = serializedObject.FindProperty("verticalPosition");
		SerializedProperty offset = serializedObject.FindProperty("offset");
		//SerializedProperty pivotOffset = serializedObject.FindProperty("pivotOffset");
		SerializedProperty distance = serializedObject.FindProperty("distance");
		SerializedProperty lookAtCamMode = serializedObject.FindProperty("lookAtCamMode");
		SerializedProperty lookOffset = serializedObject.FindProperty("lookOffset");

		//Undo.RecordObject(this, "Super Camera Anchor Transform");
		EditorGUILayout.PropertyField(cam);
		EditorGUILayout.PropertyField(horizontalPosition);
		EditorGUILayout.PropertyField(verticalPosition);
		EditorGUILayout.PropertyField(distance);
		EditorGUILayout.PropertyField(offset);
		//EditorGUILayout.PropertyField(pivotOffset);
		EditorGUILayout.PropertyField(lookAtCamMode);
		Undo.RecordObject(anchor.transform, "Changed Anchor Transform");
		if(lookAtCamMode.enumValueIndex == 2){
			anchor.transform.localEulerAngles = EditorGUILayout.Vector3Field("Rotation", anchor.transform.localEulerAngles);
		}else{
			EditorGUILayout.PropertyField(lookOffset);
		}
		anchor.transform.localScale = EditorGUILayout.Vector3Field("Scale", anchor.transform.localScale);
		EditorGUILayout.PropertyField(checkForResolutionChange);
		if(anchor.cam != null && !anchor.transform.IsChildOf(anchor.cam.transform)){
			EditorGUILayout.PropertyField(followCameraMovement);
		}
		//EditorGUILayout.PropertyField(manualRefresh);

		serializedObject.ApplyModifiedProperties();
	}
}
#endif

[HelpURL("Assets/Clavian/SuperCameraAnchor/Documentation/SuperCameraAnchor.html")] //make the help open local documentation
[AddComponentMenu("Utility/Super Camera Anchor", 0)] //allow it to be added as a component
[ExecuteInEditMode]
public class SuperCameraAnchor : MonoBehaviour {
	private Vector2 res;
	private Vector3 lastCamPos;
	private Quaternion lastCamRot;
	private Vector3 camPosition;

	[Tooltip("Will always follow a change in edit mode. This effects play mode.")]
	public bool checkForResolutionChange = false;
	public bool followCameraMovement = false;
	//public bool manualRefresh = false;
	[HideInInspector] public Camera cam;
	[Range(0f,1f)]
	public float horizontalPosition = 0f; //start in top left
	[Range(0f,1f)]
	public float verticalPosition = 1f;
	public Vector3 offset;
	//[Tooltip("Assumes pivot is in the center of object.")]
	//public Vector3 pivotOffset;
	[Tooltip("Distance from the camera.")]
	public float distance = 10f;
	public enum LookAtCamMode{
		Match,
		Stare,
		None
	}
	public LookAtCamMode lookAtCamMode;
	public Vector3 lookOffset;
	#if UNITY_EDITOR
	private Mesh anchorMesh; //for the gizmo
	//[Tooltip("This is just for the editor. Call Refresh() instead in-game.")]
	//public bool manualRefresh;
	#endif
	private Transform _t;
	public Transform t{
		get{
			if(_t == null) _t = this.transform;
			return _t;
		}
	}
	void OnEnable() {
    cam = Camera.main;
		_t = null;
		//Refresh();
	}
	/*
	void OnValidate(){
		#if UNITY_EDITOR
		if(manualRefresh){
			manualRefresh = false;
			Refresh();
		}
		#endif
	}
	*/
	#if UNITY_EDITOR
	void OnDisable(){
		HideInspectorStuff(false);
	}
	#endif
	void FindCamera(){
		if(cam == null){ //parent is a camera?
			cam = GetComponentInParent<Camera>();
		}
		if(cam == null){ //get main camera
			cam = Camera.main;
		}
		if(cam == null){ //get any camera
			cam = FindObjectOfType<Camera>();
		}
		if(cam == null){ //oops!! no cameras
			Debug.Log("No camera in this scene!");
		}
	}
	void LateUpdate(){
		if(checkForResolutionChange && (res.x != cam.pixelWidth || res.y != cam.pixelHeight)){
			Refresh();
		}else if(!t.IsChildOf(cam.transform) && followCameraMovement && (cam.transform.position != lastCamPos || cam.transform.rotation != lastCamRot)){ //update if camera moved and is not a child
			Refresh();
		}
		#if UNITY_EDITOR
		else if(!Application.isPlaying){ //if it's edit mode
			Refresh();
		}
		#endif
	}
	void RememberResolution(){
		res = new Vector2(cam.pixelWidth,cam.pixelHeight);
		lastCamPos = cam.transform.position;
		lastCamRot = cam.transform.rotation;
	}
	public static void RefreshAll(){ //only call when loading a scene, as FindObjectsOfType is very intensive.
		SuperCameraAnchor[] anchors = FindObjectsOfType(typeof(SuperCameraAnchor)) as SuperCameraAnchor[];
		for(int i=0, iL=anchors.Length; i<iL; i++){
			anchors[i].Refresh();
		}
	}
	public void Refresh(){
		FindCamera();
		if(cam){
			camPosition = new Vector3(horizontalPosition, verticalPosition, distance);

			t.position = cam.ViewportToWorldPoint(camPosition);
			t.position += cam.transform.rotation * offset;

			if(lookAtCamMode != LookAtCamMode.None){
				if(lookAtCamMode == LookAtCamMode.Stare){
					t.LookAt(cam.transform.position);
				}
				if(lookAtCamMode == LookAtCamMode.Match){
					t.rotation = cam.transform.rotation;
				}
				t.Rotate(lookOffset);
			}

			RememberResolution();
			#if UNITY_EDITOR
			HideInspectorStuff(true);
			#endif
		}
	}
	#if UNITY_EDITOR
	void OnDrawGizmosSelected(){
		if(cam){
			Vector3 anchorPoint = cam.ViewportToWorldPoint(camPosition);
			Vector3 offsetPoint = anchorPoint + cam.transform.rotation * offset;

			Gizmos.color = Color.green;
			Gizmos.DrawLine(anchorPoint, offsetPoint);

			//anchor point
			Gizmos.color = Color.white;
			if(anchorMesh == null) anchorMesh = AssetDatabase.LoadAssetAtPath("Assets/Clavian/SuperCameraAnchor/Anchor.obj", typeof(Mesh)) as Mesh;
			Gizmos.DrawMesh(anchorMesh, anchorPoint, cam.transform.rotation, Vector3.one);

			//border
			//Gizmos.color = Color.white;
			Vector3 topLeft = cam.ViewportToWorldPoint(new Vector3(0,1,distance));
			Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1,1,distance));
			Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0,0,distance));
			Vector3 bottomRight = cam.ViewportToWorldPoint(new Vector3(1,0,distance));
			Gizmos.DrawLine(topLeft, topRight); //top
			Gizmos.DrawLine(bottomLeft, bottomRight); //bottom
			Gizmos.DrawLine(topLeft, bottomLeft); //left
			Gizmos.DrawLine(topRight, bottomRight); //right
		}
	}
	public void HideInspectorStuff(bool doIt){
		t.hideFlags = doIt ? HideFlags.HideInInspector : HideFlags.None; //hide transform
	}
	#endif
}

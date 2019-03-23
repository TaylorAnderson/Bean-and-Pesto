using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SerializerExample : MonoBehaviour {
	public enum FileType {JSON,XML}
	[Header("Settings")]
	public bool obfuscate = false;
	public FileType fileType = FileType.JSON;
	[Header("File Info")]
	public string fileName = "myobject";
	public string fileExtention = ".txt";
	public string FileName{
		get{
			return fileName + fileNumber + fileExtention;
		}
	}
	public int fileNumber = 0;
	
	[System.Serializable]
	public class MyObjectType{
		public int timesPressed;
		public Vector3 myPosition;
	}
	[Header("Data")]
	public MyObjectType myObject;

	private Transform t;
	private SpriteRenderer spr;
	private string output;

	public TextMesh countMesh;
	public Text outputText;

	public void Prepare(){ //save monobehaviour data to myObject
		myObject.myPosition = t.position;
	}
	public void Apply(){ //apply myObject settings to this monobehaviour
		t.position = myObject.myPosition;
	}

	void Awake () {
		t = this.transform;
		spr = t.GetComponent<SpriteRenderer>();
	}
	public void SetMode(int newMode){ //change mode thru function
		fileType = (FileType)newMode;
	}
	public void SetFileNumber(int toWhat){
		fileNumber = toWhat;
	}
	public void Setobfuscate(bool newValue){
		obfuscate = newValue;
	}
	public void SetFileName(string newName){
		fileName = newName;
	}
	public void SetFileExtention(string newExt){
		fileExtention = newExt;
	}
	public void UpdateUI () { //update UI to reflect spacebar count (Don't use GameObject.Find)
		countMesh.text = myObject.timesPressed.ToString();
		outputText.text = output;
	}
	public void Save(){
		Prepare();
		if(fileType == FileType.JSON){
			SSL.SaveJSON(myObject,FileName, obfuscate);
		}else{
			SSL.SaveXML(myObject, FileName, obfuscate);
		}
		
		spr.color = Color.green; //physically show it was saved
		output = SSL.LoadTXT(FileName);
		UpdateUI();
	}
	public void Load(){
		if(fileType == FileType.JSON){
			//SSL.LoadJSON(ref myObject,FileName, obfuscate);
			myObject = SSL.LoadJSON<MyObjectType>(FileName, obfuscate);
		}else{
			myObject = SSL.LoadXML<MyObjectType>(FileName, obfuscate);
			//SSL.LoadXML(ref myObject, FileName, obfuscate);
		}
		Apply();
		spr.color = Color.blue; //physically show it was loaded
		output = SSL.LoadTXT(FileName);
		UpdateUI();
	}
	
	void Update () {
		spr.color = Color.Lerp(spr.color,new Color(1f,1f,1f,0.5f),Time.deltaTime * 8.0f);
		
		if(Input.anyKeyDown && !(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) ||
								Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow) ||
								Input.GetKeyDown(KeyCode.Mouse0))){
			myObject.timesPressed++;
			UpdateUI();
		}
		if(Input.GetKey(KeyCode.LeftArrow)){
			t.position += Vector3.left * Time.deltaTime * 2.0f;
		}
		if(Input.GetKey(KeyCode.RightArrow)){
			t.position += Vector3.right * Time.deltaTime * 2.0f;
		}
		if(Input.GetKey(KeyCode.DownArrow)){
			t.position += Vector3.down * Time.deltaTime * 2.0f;
		}
		if(Input.GetKey(KeyCode.UpArrow)){
			t.position += Vector3.up * Time.deltaTime * 2.0f;
		}
	}
}

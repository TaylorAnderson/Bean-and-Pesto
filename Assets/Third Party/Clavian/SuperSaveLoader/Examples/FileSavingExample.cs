using UnityEngine;
using System.Collections;

public class FileSavingExample : MonoBehaviour {
	[Header("Objects")]
	public SpriteRenderer saveThisSprite;
	public SpriteRenderer loadThisSprite;
	public TextMesh saveText;
	public TextMesh loadText;

	[Header("Variables")]
	public string fileName = "myImage.png";
	public bool obfuscate;

	[Header("Returns")]
	public string[] directoryFiles;

	void Update () {
		//save when "S" is pressed
		if(Input.GetKeyDown(KeyCode.S)){
			Save();
		}
		//load when "L" is pressed
		if(Input.GetKeyDown(KeyCode.L)){
			Load();
		}
		//delete
		if(Input.GetKeyDown(KeyCode.D)){
			Delete();
		}
		//check if it exists
		if(Input.GetKeyDown(KeyCode.E)){
			Exists();
		}
		//clear a directory
		if(Input.GetKeyDown(KeyCode.C)){
			ClearDirectory();
		}
		if(Input.GetKeyDown(KeyCode.G)){
			directoryFiles = SSL.GetFilesInDirectory(fileName);
		}
	}
	public void Exists(){
		SSL.Exists(fileName);
	}
	public void Delete(){
		SSL.Delete(fileName);
	}
	public void ClearDirectory(){
		SSL.ClearDirectory(fileName);
	}
	public void Save(){
		SSL.SavePNG(saveThisSprite.sprite.texture, fileName, obfuscate); //save using settings
		saveText.text = fileName; //update name on-screen
	}
	public void Load(){
		Texture2D loadedTex = SSL.LoadPNG(fileName, obfuscate); //load using settings
		loadThisSprite.sprite = Sprite.Create(loadedTex, new Rect(0, 0, loadedTex.width, loadedTex.height), new Vector2(0.5f,0.5f)); //create a sprite from loaded texture
		loadThisSprite.sprite.name = loadedTex.name; //assign name to sprite object
		loadText.text = loadedTex.name; //show name on screen
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;
using System;
using System.IO;

public class SendImageToServer : MonoBehaviour {
	
    public GameObject dataPanel;
	public Text nameText;
	public RawImage photoPanel;

	private Camera _camera;
	private Texture2D myTexture;
	private SocketIOComponent _socket;

	void Start(){
		_camera = GetComponent<Camera>();
		_socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();

		_socket.On("open", TestOpen);
		_socket.On("reply", OnReply);
		_socket.On("error", TestError);
		_socket.On("close", TestClose);

		StartCoroutine("BeepBoop");
	}

	// void Update(){
	// 	if(Input.GetMouseButtonDown(0)){
	// 		StartCoroutine("WriteAndSendPng");
	// 	}
	// }

	void receiveMessage(string msg){
		if(msg == "click"){
			StartCoroutine("WriteAndSendPng");
		}
	}

	IEnumerator WriteAndSendPng(){
		RenderTexture currentRT = RenderTexture.active;
		var rTex = new RenderTexture(_camera.pixelHeight, _camera.pixelHeight, 16);
		_camera.targetTexture = rTex;
		RenderTexture.active = _camera.targetTexture;
		_camera.Render();
		Texture2D tex = new Texture2D(_camera.targetTexture.width, _camera.targetTexture.height, TextureFormat.RGB24, false);
		tex.ReadPixels(new Rect(0,0, _camera.targetTexture.width, _camera.targetTexture.height), 0, 0);
		tex.Apply(false);
		RenderTexture.active = currentRT;
		byte[] bytes = tex.EncodeToPNG();

		string s =  Convert.ToBase64String(bytes);

		JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
		j.AddField("dado", "from c# kid");
		j.AddField("img", s);
		_socket.Emit("chat message", j);
		Destroy(tex);
		//System.IO.File.WriteAllBytes("jar:file://" + Application.dataPath + "!/assets/Resources/Images/face.png", bytes);
		NativeGallery.SaveImageToGallery(bytes, "FacialRecognitionAR", "face.png");
		yield return new WaitForSeconds(0.2f);
		string path = "storage/emulated/0/FacialRecognitionAR/face.png";
		byte[] fileByteData = File.ReadAllBytes(path);
		myTexture.LoadImage(fileByteData);
		photoPanel.texture = myTexture;
	}

	IEnumerator DataResultCoroutine(JSONObject dataResult){
		if(dataResult[2].ToString().Equals("\"Found\"")){
			dataPanel.SetActive(true);
			dataPanel.GetComponent<Animator>().SetTrigger("found");
			nameText.text = dataResult[0].ToString();
			yield return new WaitForSeconds(5f);
			dataPanel.SetActive(false);
			nameText.text = "";
		}
		else if(dataResult[2].ToString().Equals("\"Not Found\"")){
			dataPanel.SetActive(true);
			dataPanel.GetComponent<Animator>().SetTrigger("notFound");
			nameText.text = dataResult[0].ToString();
			yield return new WaitForSeconds(5f);
			dataPanel.SetActive(false);
			nameText.text = "";
		}		
	}



	private IEnumerator BeepBoop(){
		yield return new WaitForSeconds(1);
		_socket.Emit("beep");
		yield return new WaitForSeconds(3);
		_socket.Emit("beep");
		yield return new WaitForSeconds(2);
		_socket.Emit("beep");
		yield return null;
		_socket.Emit("beep");
		_socket.Emit("beep");
	}

	public void TestOpen(SocketIOEvent e){}
	
	public void TestBoop(SocketIOEvent e){ if (e.data == null) { return; } }
	
	public void TestError(SocketIOEvent e){}
	
	public void TestClose(SocketIOEvent e){}

	public void OnReply(SocketIOEvent e){
		StartCoroutine(DataResultCoroutine(e.data));
	}
}

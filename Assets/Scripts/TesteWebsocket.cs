using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;
using System;
using System.IO;

public class TesteWebsocket : MonoBehaviour {
	
	private SocketIOComponent socket;
	private JSONObject dataResult;
	
	void Start(){
		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();

		socket.On("open", TestOpen);
		socket.On("reply", OnReply);
		socket.On("error", TestError);
		socket.On("close", TestClose);

		StartCoroutine("BeepBoop");
	}

	// Update is called once per frame
	void Update () {

		if(Input.GetMouseButtonDown(0)){
			StartCoroutine("WriteAndSendPng");
		}
		
	}


	IEnumerator WriteAndSendPng(){
		Renderer renderer = gameObject.GetComponent<Renderer>();
        Texture2D texture = (Texture2D)renderer.material.mainTexture;
        byte[] bytes = texture.EncodeToPNG();
        string base64encoded = Convert.ToBase64String(bytes);
		
		JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
		j.AddField("dado", "from c# kid");
		j.AddField("img", base64encoded);
		socket.Emit("chat message", j);
		System.IO.File.WriteAllBytes("test.png", bytes);
		yield return new WaitForSeconds(0.2f);
	}


	private IEnumerator BeepBoop(){
		// wait 1 seconds and continue
		yield return new WaitForSeconds(1);
		
		socket.Emit("beep");
		
		// wait 3 seconds and continue
		yield return new WaitForSeconds(3);
		
		socket.Emit("beep");
		
		// wait 2 seconds and continue
		yield return new WaitForSeconds(2);
		
		socket.Emit("beep");
		
		// wait ONE FRAME and continue
		yield return null;
		
		socket.Emit("beep");
		socket.Emit("beep");
	}

	public void TestOpen(SocketIOEvent e){
		//Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
	}
	
	public void TestBoop(SocketIOEvent e){
		//Debug.Log("[SocketIO] Boop received: " + e.name + " " + e.data);

		if (e.data == null) { return; }

		// Debug.Log(
		// 	"#####################################################" +
		// 	"THIS: " + e.data.GetField("this").str +
		// 	"#####################################################"
		// );
	}
	
	public void TestError(SocketIOEvent e){
		//Debug.Log("[XXXXXXXX] Error received: " + e.name + " " + e.data);
	}
	
	public void TestClose(SocketIOEvent e){	
		//Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
	}

	public void OnReply(SocketIOEvent e){
		print(e.data.ToString());
	}
}

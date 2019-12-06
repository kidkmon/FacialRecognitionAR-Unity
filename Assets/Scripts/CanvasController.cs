using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour{

    public GameObject cameraStart;
    public GameObject cameraAR;
    
    public GameObject canvasStart;
    public GameObject canvasAR;

    public GameObject modalPanel;
    public GameObject ipConfigurationBtn;

    public GameObject socketIO;
    public Text ipServerInput;

    // Start is called before the first frame update
    void Start(){
        cameraStart.SetActive(true);
        canvasStart.SetActive(true);
        cameraAR.SetActive(false);
        canvasAR.SetActive(false);

        ipConfigurationBtn.SetActive(true);
        modalPanel.SetActive(false);
    }

    public void StartGame(){
        cameraStart.SetActive(false);
        canvasStart.SetActive(false);
        cameraAR.SetActive(true);
        canvasAR.SetActive(true);
        //Instantiate(socketIO, socketIO.transform.position, socketIO.transform.rotation);
    }

    public void ActiveModal(){
        ipConfigurationBtn.SetActive(false);
        modalPanel.SetActive(true);
    }

    public void DeactiveModal(){
        string _url = "ws://" +  ipServerInput.text.ToString() + "/socket.io/?EIO=4&transport=websocket";
        //SocketIO.SocketIOComponent.url = _url;
        ipConfigurationBtn.SetActive(true);
        modalPanel.SetActive(false);
    }

}

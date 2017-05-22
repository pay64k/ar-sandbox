using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMove : MonoBehaviour {

    public float cameraRotateSpeed = 2f;
    private FileSaver fileSaver = new FileSaver();

	// Use this for initialization
	void Start () {
		
	}


    // Update is called once per frame
    void Update () {
        var move = new Vector3();
        float moveSpeed = 1;
        if (Input.GetKey("q"))
        {
            move = new Vector3(Input.GetAxis("Horizontal"), 0.5f, Input.GetAxis("Vertical"));
        }

        else if (Input.GetKey("z"))
        {
            move = new Vector3(Input.GetAxis("Horizontal"), -0.5f, Input.GetAxis("Vertical"));
        }
        else
        {
            move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        }
        if (Input.GetKey("space"))
        {
            moveSpeed = 5f;
            cameraRotateSpeed = 4;
        }
        else
        {
            moveSpeed = 1f;
            cameraRotateSpeed = 2;
        }
        this.transform.position += move * moveSpeed * Time.deltaTime;

        if (Input.GetKey("k")) { Rotate2(new Vector3(2, 0, 0)); }
        if (Input.GetKey("i")) { Rotate2(new Vector3(-2, 0, 0)); }
        if (Input.GetKey("l")) { Rotate2(new Vector3(0, 2, 0)); }
        if (Input.GetKey("j")) { Rotate2(new Vector3(0, -2, 0)); }

        //RotateWithKeyboard();
        //this.GetComponent<Camera>().fieldOfView = 50f;
        if (Input.GetKeyDown("f5")) {
            fileSaver.saveCameraPosition(this.transform.position, this.transform.rotation);
        }
        if (Input.GetKeyDown("f9"))
        {
            ArrayList read = fileSaver.loadCameraPosition();
            this.transform.position = (Vector3)read[0];
            this.transform.rotation = (Quaternion)read[1];
        }
    }

    void Rotate2(Vector3 value)
    {
        //transform.Rotate(0, value * 1f * Time.deltaTime, 0);
        transform.Rotate(value * Time.deltaTime * cameraRotateSpeed);
    }


}

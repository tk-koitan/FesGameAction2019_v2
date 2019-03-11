using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFreezzRotation : MonoBehaviour {

    public Transform target;
    private Vector3 offset;
    private Vector3 followingPos;
    private Vector3 followOffset;
    public bool followFlag;
    public Transform defaultTf;
    public GameObject lookObj;
    public float rotationSpeed;

    //カメラの場所
    public enum CameraPosName {
        longDistanceView,
        shortDistanceView,
        lookAtBall
    }
    public CameraPosName cameraPosName;
    public Transform cameraLongDistance;
    public Transform cameraShortDistance;
    private Transform lookAtRotation;

	private void Awake()
	{
        //フレームレート固定
        Application.targetFrameRate = 60;
	}

	// Use this for initialization
	void Start () {
        offset = transform.position - target.position;
        followOffset = offset;
        //followOffset.y += 10;
        lookAtRotation = cameraLongDistance;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //向くべき方向
        lookObj.transform.position = transform.position;
        lookObj.transform.LookAt(target);

        //いるべき場所
        switch(cameraPosName){
            case CameraPosName.longDistanceView:
                followingPos = cameraLongDistance.position;
                lookAtRotation = cameraLongDistance;
                break;
            case CameraPosName.shortDistanceView:
                followingPos = cameraShortDistance.position;
                lookAtRotation = cameraShortDistance;
                break;
            case CameraPosName.lookAtBall:
                followingPos = target.position + followOffset;
                lookAtRotation = lookObj.transform;
                break;
            default:
                break;
        }
        if(followFlag){
            transform.position += (followingPos - transform.position) / 64f;
            //方向を向く
            //transform.LookAt(target);
            /*
            Vector3 currentAngleVec = lookObj.transform.rotation.eulerAngles - transform.rotation.eulerAngles;
            Vector3 tmpAngle = transform.rotation.eulerAngles + currentAngleVec / 32f;
            transform.rotation = Quaternion.Euler(tmpAngle);
            */
            transform.rotation = Quaternion.Slerp(transform.rotation, lookAtRotation.rotation, Time.time * rotationSpeed);

        }
        //Debug.Log(lookObj.transform.rotation.eulerAngles);
    }

    public void CameraReset(){
        //transform.position = target.position + offset;
        //transform.position = defaultTf.position;
        //transform.rotation = defaultTf.rotation;
        cameraPosName = CameraPosName.longDistanceView;
    }
}

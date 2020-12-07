using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;

[ExecuteInEditMode]
public class CameraRig : MonoBehaviour
{
    public Transform target;
    public bool autoTargetPlayer;
    public LayerMask wallLayers;
    public GameObject follow;
    Transform cam;

    private PhotonView pv;
    //public enum Shoulder
    //{
    //    Right, Left
    //}
  //  public Shoulder shoulder;
    float rotationSpeed = 1;
   // public Transform Target, Player;
    float mouseX, mouseY;
    [System.Serializable]
    public class CameraSettings
    {
        [Header("-Positioning-")]
        public Vector3 camPositionOffsetLeft;
        public Vector3 camPositionOffsetRight;

        [Header("-Camera Options-")]
        public Camera UICamera;
        public float mouseXSensitivity = 5.0f;
        public float mouseYSensitivity = 5.0f;
        public float minAngle = -30.0f;
        public float maxAngle = 70.0f;
        public float rotationSpeed = 5.0f;
        public float maxCheckDist = 0.1f;

        [Header("-Zoom-")]
        public float fieldOfView = 0.0f;
        public float zoomFieldOfView = 60.0f;
        public float zoomSpeed = 3.0f;

        [Header("-Visual Options-")]
        public float hideMeshWhenDistance = 0.5f;
    }
    [SerializeField]
    public CameraSettings cameraSettings;

    [System.Serializable]
    public class InputSettings
    {
        public string verticalAxis = "Mouse X";
        public string horizontalAxis = "Mouse Y";
        public string aimButton = "Fire2";
        public string switchShoulderButton = "Fire4";
    }
    [SerializeField]
    public InputSettings input;

    [System.Serializable]
    public class MovementSettings
    {
        public float movementLerpSpeed = 5.0f;
    }
    [SerializeField]
    public MovementSettings movement;

    private float newX = 0.0f;
    private float newY = 0.0f;

    public Camera mainCamera { get; protected set; }
    public Transform pivot;

    // Use this for initialization
    void Start()
    {
        mainCamera = Camera.main;
      //  pivot = GameObject.FindGameObjectWithTag("Khal").transform;
            pv = GetComponent<PhotonView>();
            // cam = pivot.transform.GetChild(0);     
    }

    // Update is called once per frame
    void Update()
    {
       
      
      
            if (pv.IsMine)
            {
                LockandUnlock();
                if (Cursor.lockState == CursorLockMode.Locked)
                {
                    if (target)
                    {
                        if (Application.isPlaying)
                        {
                            RotateCamera();
                            //CheckWall();
                            CheckMeshRenderer();
                            Zoom(Input.GetButton(input.aimButton));

                            // if (Input.GetButtonDown(input.switchShoulderButton))
                            // {
                            //     SwitchShoulders();
                            //}
                        }
                    }
                    //           cam.LookAt(pivot);
                }

            }
        
       
    }

    void LockandUnlock()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    void LateUpdate()
    {
        if (!target)
        {
            TargetPlayer();
        }
        else
        {
            //Vector3 targetPostion = target.position;
            //Quaternion targetRotation = target.rotation;

            //FollowTarget(targetPostion,targetRotation);
        }
    }

    //Finds the plater gameObject and sets it as target
    void TargetPlayer()
    {
        if (autoTargetPlayer)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player)
            {
                Transform playerT = player.transform;
                target = playerT;
            }
        }
    }

    //Following the target with Time.deltaTime smoothly
    //void FollowTarget(Vector3 targetPosition, Quaternion targetRotation)
    //{
    //    if (!Application.isPlaying)
    //    {
    //        transform.position = targetPosition;
    //        transform.rotation = targetRotation;
    //    }
    //    else
    //    {
    //        Vector3 newPos = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * movement.movementLerpSpeed);
    //        transform.position = newPos;
    //    }
    //}

   // Rotates the camera with input
    void RotateCamera()
    {
        if (!pivot)
           return;
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        mouseY = Mathf.Clamp(mouseY, -60, 75);

            newX += cameraSettings.mouseXSensitivity * Input.GetAxis(input.verticalAxis);
            newY += cameraSettings.mouseYSensitivity * Input.GetAxis(input.horizontalAxis);

            Vector3 eulerAngleAxis = new Vector3();
            eulerAngleAxis.x = newY;
            eulerAngleAxis.y = newX;

            newX = Mathf.Repeat(newX, 360); // yeh apky camery ko nechy nh jany dein ga
            newY = Mathf.Clamp(newY, cameraSettings.minAngle, cameraSettings.maxAngle);
            transform.LookAt(pivot);

            pivot.rotation = Quaternion.Euler(mouseY, mouseX, 0);
            target.rotation = Quaternion.Euler(0, mouseX, 0);

           //Quaternion newRotation = Quaternion.Slerp(pivot.transform.localRotation, Quaternion.Euler(eulerAngleAxis), Time.deltaTime * cameraSettings.rotationSpeed);
           // //float pos = 0f;
           // pivot.transform.localRotation = newRotation;
           // //pivot.rotation = Quaternion.Euler(transform.rotation.eulerAngles., pos, pos);
            
           
       
    }

    
    //Checks the wall and moves the camera up if we hit
    //void CheckWall()
    //{
    //    if (!pivot || !mainCamera)
    //        return;

    //    RaycastHit hit;

    //    Transform mainCamT = mainCamera.transform;
    //    Vector3 mainCamPos = mainCamT.position;
    //    Vector3 pivotPos = pivot.position;

    //    Vector3 start = pivotPos;
    //    Vector3 dir = mainCamPos - pivotPos;

    //  //  float dist = Mathf.Abs(shoulder == Shoulder.Left ? cameraSettings.camPositionOffsetLeft.z : cameraSettings.camPositionOffsetRight.z);

    //    if (Physics.SphereCast(start, cameraSettings.maxCheckDist, dir, out hit, dist, wallLayers))
    //    {
    //        MoveCamUp(hit, pivotPos, dir, mainCamT);
    //    }
    //    else
    //    {
    //        switch (shoulder)
    //        {
    //            case Shoulder.Left:
    //                PostionCamera(cameraSettings.camPositionOffsetLeft);
    //                break;
    //            case Shoulder.Right:
    //               PostionCamera(cameraSettings.camPositionOffsetRight);
    //                break;
    //        }
    //    }
    //}

    //This moves the camera forward when we hit a wall
    void MoveCamUp(RaycastHit hit, Vector3 pivotPos, Vector3 dir, Transform cameraT)
    {
        float hitDist = hit.distance;
        Vector3 sphereCastCenter = pivotPos + (dir.normalized * hitDist);
        cameraT.position = sphereCastCenter;
    }

    //Postions the cameras localPosition to a given location
    void PostionCamera(Vector3 cameraPos)
    {
        if (!mainCamera)
            return;

        Transform mainCamT = mainCamera.transform;
        Vector3 mainCamPos = mainCamT.localPosition;
        Vector3 newPos = Vector3.Lerp(mainCamPos, cameraPos, Time.deltaTime * movement.movementLerpSpeed);
        mainCamT.localPosition = newPos;
    }

    //Hides the mesh targets mesh renderers when too close
    void CheckMeshRenderer()
    {
        if (!mainCamera || !target)
            return;

        SkinnedMeshRenderer[] meshes = target.GetComponentsInChildren<SkinnedMeshRenderer>();
        Transform mainCamT = mainCamera.transform;
        Vector3 mainCamPos = mainCamT.position;
        Vector3 targetPos = target.position;
        float dist = Vector3.Distance(mainCamPos, (targetPos + target.up));

        if (meshes.Length > 0)
        {
            for (int i = 0; i < meshes.Length; i++)
            {
                if (dist <= cameraSettings.hideMeshWhenDistance)
                {
                    meshes[i].enabled = false;
                }
                else
                {
                    meshes[i].enabled = true;
                }
            }
        }
    }

  //zoomsthe camera in and out
    void Zoom(bool isZooming)
    {
        if (!mainCamera)
            return;

        if (isZooming)
        {
            float newFieldOfView = Mathf.Lerp(mainCamera.fieldOfView, cameraSettings.zoomFieldOfView, Time.deltaTime * cameraSettings.zoomSpeed);
            mainCamera.fieldOfView = newFieldOfView;

            if (cameraSettings.UICamera != null)
            {
                cameraSettings.UICamera.fieldOfView = newFieldOfView;
            }
        }
        else
        {
            float originalFieldOfView = Mathf.Lerp(mainCamera.fieldOfView, cameraSettings.fieldOfView, Time.deltaTime * cameraSettings.zoomSpeed);
            mainCamera.fieldOfView = originalFieldOfView;

            if (cameraSettings.UICamera != null)
            {
                cameraSettings.UICamera.fieldOfView = originalFieldOfView;
            }
        }
    }

    //Switches the cameras shoulder view
    //public void SwitchShoulders()
    //{
    //    switch (shoulder)
    //    {
    //        case Shoulder.Left:
    //            shoulder = Shoulder.Right;
    //            break;
    //        case Shoulder.Right:
    //            shoulder = Shoulder.Left;
    //            break;
    //    }
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Camera camera;
    [SerializeField] private CharacterController cameraController;
    [SerializeField] private int movementSpeed;

    private Vector3 _previousMousePos;
    private bool _yToggle = true;
    private Vector3 _target;
    
    [SerializeField]
    private Transform target;
    // The distance in the x-z plane to the target
    [SerializeField]
    private float distance = 10.0f;
    // the height we want the camera to be above the target
    [SerializeField]
    private float height = 5.0f;

    [SerializeField]
    private float rotationDamping;
    [SerializeField]
    private float heightDamping;


    // Update is called once per frame
    void Update()
    {
        OrdinaryMovement();
        RotationalMovement();
        ClickObject();

    }

    void OrdinaryMovement()
    {
        var direction = new Vector3(0, 0, 0);

        //WASD-movement as well as arrows
        if (Input.GetKey("w") || Input.GetKey("up"))
        {
            direction += camera.transform.forward;
        }

        if (Input.GetKey("s") || Input.GetKey("down"))
        {
            direction -= camera.transform.forward;
        }

        if (Input.GetKey("a") || Input.GetKey("left"))
        {
            direction -= camera.transform.right;
        }

        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            direction += camera.transform.right;
        }

        //travel up and down the y-axis using space, press left shift once to change direction.
        if (Input.GetKeyDown("left shift")) _yToggle = !_yToggle;
        if (Input.GetKey("space"))
        {
            if (_yToggle)
            {
                direction.y += 1;
            }
            else
            {
                direction.y -= 1;
            }
        }

        direction.Normalize();
        camera.transform.position += direction * (movementSpeed * Time.deltaTime);
    }

    void RotationalMovement()
    {
        //rotational movement using right mouse button
        if (Input.GetMouseButtonDown(1))
        {
            _previousMousePos = camera.ScreenToViewportPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 mouseDirection = _previousMousePos - camera.ScreenToViewportPoint(Input.mousePosition);

            camera.transform.Rotate(new Vector3(1, 0, 0), mouseDirection.y * 180);
            camera.transform.Rotate(new Vector3(0, 1, 0), -mouseDirection.x * 180, Space.World);

            _previousMousePos = camera.ScreenToViewportPoint(Input.mousePosition);
        }
    }

    void ClickObject()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitTarget = new RaycastHit();
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitTarget))
            {
                target = hitTarget.transform;
                if (target.parent != null)
                {
                    target = target.parent;
                }
            }
        }

        var hasArrived = (target.position - transform.position).magnitude < distance + 1;
        if (!hasArrived)
        {
            var direction = (target.position - transform.position).normalized;
            cameraController.Move(direction * (movementSpeed * Time.deltaTime));
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        else
        {
            if (!target)
                return;

            // Calculate the current rotation angles
            var wantedRotationAngle = target.eulerAngles.y;
            var wantedHeight = target.position.y + height;

            var currentRotationAngle = transform.eulerAngles.y;
            var currentHeight = transform.position.y;

            // Damp the rotation around the y-axis
            currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

            // Damp the height
            currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

            // Convert the angle into a rotation
            var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

            // Set the position of the camera on the x-z plane to:
            // distance meters behind the target
            transform.position = target.position;
            transform.position -= currentRotation * Vector3.forward * distance;
            

            // Set the height of the camera
            transform.position = new Vector3(transform.position.x ,currentHeight , transform.position.z);

            // Always look at the target
            transform.LookAt(target);
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            camera.Reset();
            target = null;
        }
    }

    
}

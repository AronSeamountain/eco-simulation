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
            RaycastHit target = new RaycastHit();
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out target))
            {
                // Check if arrived
                /*var hasArrived = (target.transform.position - camera.transform.position).magnitude < 10;
                if (!hasArrived)
                {
                    var direction = (target.transform.position - transform.position).normalized;

                    camera.transform.position += direction * (movementSpeed * Time.deltaTime);
                    transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
                }*/
                _target = target.transform.position;

            }
        }

        var hasArrived = (Target - transform.position).magnitude < 10;
        if (!hasArrived)
        {
            var direction = (Target - transform.position).normalized;
            cameraController.Move(direction * (movementSpeed * Time.deltaTime));
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }

        // Move


    }

    public Vector3 Target
    {
        get => _target;
        set
        {
            _target = value;
            HasTarget = true;
        }
    }

    public bool HasTarget { get; set; }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        

        var newPos = new Vector3(Random.Range(-1,2), 0, Random.Range(-1,2)) * Time.deltaTime;
        
        controller.Move(newPos);
    }

    private Transform GetClosestFood()
    {
        

        return null;
    }

}

   

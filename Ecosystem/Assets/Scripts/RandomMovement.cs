using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class RandomMovement : MonoBehaviour
{
  [SerializeField] private CharacterController controller;

  [SerializeField] private FoodDetector foodDetector;

  private Food _foodTarget;
  [SerializeField] private int movementSpeed;


  // Start is called before the first frame update
  private void Start()
  {
    foodDetector.OnFoodFoundListeners += OnFoodFound;
  }

  // Update is called once per frame
  private void Update()
  {
    //var newPos = new Vector3(Random.Range(-1, 2), 0, Random.Range(-1, 2)) * Time.deltaTime;

    //controller.Move(newPos);

    if (_foodTarget == null) return;
    if (Distance(_foodTarget) < 2)
    {
      foodDetector.Eat(_foodTarget);
    }
      
    var direction = (_foodTarget.transform.position - transform.position).normalized;
    controller.Move(direction * movementSpeed * Time.deltaTime);
    transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
  }

  private Transform GetClosestFood()
  {
    return null;
  }

  private void OnFoodFound(List<Food> foods)
  {
    Food closestFood = GetClosestF(foods);
    _foodTarget = closestFood;
    Debug.Log(closestFood.transform.position);
    Debug.Log("omnomnom");
  }

  private Food GetClosestF(List<Food> foods)
  {
    var closest = foods.First();
    foreach (var o in foods)
    {
      if (Distance(o) < Distance(closest))
      {
        closest = o;
      }
    }
    return closest;
  }

  private float Distance(Food f)
  {
    return (f.transform.position - this.transform.position).magnitude;
  }
  
}
using UnityEngine;

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

    var direction = (_foodTarget.transform.position - transform.position).normalized;
    controller.Move(direction * movementSpeed * Time.deltaTime);
    transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
  }

  private Transform GetClosestFood()
  {
    return null;
  }

  private void OnFoodFound(Food food)
  {
    _foodTarget = food;
    Debug.Log(food.transform.position);
    Debug.Log("omnomnom");
  }
}
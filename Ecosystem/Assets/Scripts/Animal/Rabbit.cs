using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Animal
{
  public class Rabbit : MonoBehaviour
  {
    [SerializeField] private Animator animator;
    [SerializeField] private Herbivore animal;
    void Update()
    {
      animator.SetBool("isJumping", animal.IsHungry);
      animator.SetBool("isDead_1", !animal.IsAlive);
    }
  }
}
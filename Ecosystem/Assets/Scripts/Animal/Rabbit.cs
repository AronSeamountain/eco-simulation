using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Animal
{
  public class Rabbit : Herbivore
  {
    [SerializeField] private Animator animator;
    void Update()
    {
      movement.SpeedFactor = 10;
      animator.SetBool("isJumping", IsHungry);
      animator.SetBool("isDead_1", !IsAlive);
    }
  }
}
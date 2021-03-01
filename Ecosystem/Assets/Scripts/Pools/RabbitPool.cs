using System.Collections.Generic;
using System.Linq;
using Animal;
using UnityEngine;

namespace Pools
{
  public sealed class RabbitPool : MonoBehaviour
  {
    private const int AmountToPool = 10;
    [SerializeField] private GameObject rabbitPrefab;

    private readonly ISet<Herbivore> _pool;

    protected RabbitPool()
    {
      _pool = new HashSet<Herbivore>();
    }

    public Herbivore Get()
    {
      return _pool.Count > 0 ? _pool.First() : CreateObject();
    }

    private Herbivore CreateObject()
    {
      var instance = Instantiate(rabbitPrefab, Vector3.zero, Quaternion.identity).GetComponent<Herbivore>();
      return instance;
    }

    public void Pool(Herbivore herbivore)
    {
      if (_pool.Count < AmountToPool)
        _pool.Add(herbivore);
      else
        Destroy(herbivore);
    }
  }
}
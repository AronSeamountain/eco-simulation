using System.Collections.Generic;
using UnityEngine;

namespace Pools
{
  public abstract class AbstractMonoBehaviourPool<T> : MonoBehaviour, IPool<T> where T : MonoBehaviour
  {
    private const int AmountToPool = 10;
    public static AbstractMonoBehaviourPool<T> SharedInstance;
    [SerializeField] private GameObject objectPrefab;
    private Stack<T> _pool;

    private void Awake()
    {
      _pool = new Stack<T>();
      SharedInstance = this;
    }

    public T Get()
    {
      return _pool.Count > 0 ? _pool.Pop() : CreateObject();
    }

    private T CreateObject()
    {
      var instance = Instantiate(objectPrefab, Vector3.zero, Quaternion.identity).GetComponent<T>();
      return instance;
    }

    public void Pool(T instance)
    {
      if (_pool.Count < AmountToPool)
        _pool.Push(instance);
      else
        Destroy(instance);
    }
  }
}
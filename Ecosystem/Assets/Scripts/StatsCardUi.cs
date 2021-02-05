using UnityEngine;

public class StatsCardUi : MonoBehaviour
{
  /// <summary>
  /// TODO: Can all of this be moved into DisplayCharacterStats.cs?
  /// </summary>
  [SerializeField] private GameObject cardPrefab;

  /// <summary>
  ///   Putting the card at the bottom at the screen to start with, will adjust later.
  /// </summary>
  private Vector2 _bottomCenterScreenSpace;
  private Vector3 _bottomCenterWorldSpace;

  private void Start()
  {
    this._bottomCenterScreenSpace = new Vector2(Screen.width / 2,0);
    this._bottomCenterWorldSpace = Camera.main.ScreenToWorldPoint(new Vector3(_bottomCenterScreenSpace.x, _bottomCenterWorldSpace.y, Camera.main.nearClipPlane));
  }
  private void Update()
  {
    //OnCardDrawn();
  }
  
  private void OnCardDrawn(Card card)
  {
    var cardPosition = _bottomCenterWorldSpace;
    GameObject newCard = GameObject.Instantiate(this.cardPrefab, this._bottomCenterWorldSpace, Quaternion.identity);
  }
  
}
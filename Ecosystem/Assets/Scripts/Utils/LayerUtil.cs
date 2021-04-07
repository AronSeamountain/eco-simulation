using UnityEngine;

namespace Utils
{
  public static class LayerUtil
  {
    public static int HerbivoreMale => LayerMask.NameToLayer("HerbivoreMale");
    public static int HerbivoreMaleVision => LayerMask.NameToLayer("HerbivoreMaleVision");
    public static int HerbivoreMaleHearing => LayerMask.NameToLayer("HerbivoreMaleHearing");
    
    public static int HerbivoreFemaleInfertile => LayerMask.NameToLayer("HerbivoreFemaleInfertile");
    public static int HerbivoreFemaleFertile => LayerMask.NameToLayer("HerbivoreFemaleFertile");
    public static int HerbivoreFemaleVision => LayerMask.NameToLayer("HerbivoreFemaleVision");
    public static int HerbivoreFemaleHearing => LayerMask.NameToLayer("HerbivoreFemaleHearing");
    
    public static int CarnivoreMale => LayerMask.NameToLayer("CarnivoreMale");
    public static int CarnivoreMaleVision => LayerMask.NameToLayer("CarnivoreMaleVision");
    public static int CarnivoreMaleHearing => LayerMask.NameToLayer("CarnivoreMaleHearing");
    
    public static int CarnivoreFemaleInfertile => LayerMask.NameToLayer("CarnivoreFemaleInfertile");
    public static int CarnivoreFemaleFertile => LayerMask.NameToLayer("CarnivoreFemaleFertile");
    public static int CarnivoreFemaleVision => LayerMask.NameToLayer("CarnivoreFemaleVision");
    public static int CarnivoreFemaleHearing => LayerMask.NameToLayer("CarnivoreFemaleHearing");
  }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card", order = 1)]
public class CardScriptableObject : ScriptableObject
{
    public string cardName;
    [TextArea]
    public string cardDescription;
    [TextArea]
    public string cardLore;

    public Sprite cardImage;

    public int defencePoints;
    public int attackPoints;
    public int cardCost;

}

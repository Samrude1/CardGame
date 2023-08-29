using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardScriptableObject cardSO;

    public int defencePoints;
    public int attackPoints;
    public int cardCost;

    public Image cardImage;

    //public TMP_Text defenceText;
    //public TMP_Text attackText;
    public TMP_Text costText;
    public TMP_Text nameText;
    public TMP_Text actionText;
    public TMP_Text loreText;

    // Start is called before the first frame update
    void Start()
    {
        CardBuild();
    }

    public void CardBuild()
    {
        defencePoints = cardSO.defencePoints;
        attackPoints = cardSO.attackPoints;
        cardCost = cardSO.cardCost;

        costText.text = cardCost.ToString();

        nameText.text = cardSO.cardName;
        actionText.text = cardSO.cardDescription;
        loreText.text = cardSO.cardLore;
        cardImage.sprite = cardSO.cardImage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

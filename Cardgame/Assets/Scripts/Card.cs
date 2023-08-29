using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    private Vector3 currentPlace;

    public CardScriptableObject cardSO;

    public int defencePoints;
    public int attackPoints;
    public int cardCost;
    public float speed;
    public float rotationSpeed;
    public bool inHand;
    public int handPosition;
    public GameObject card;

    public Image cardImage;
    //public TMP_Text defenceText;
    //public TMP_Text attackText;
    public TMP_Text costText;
    public TMP_Text nameText;
    public TMP_Text actionText;
    public TMP_Text loreText;

    private Vector3 targetP;
    private Quaternion targetRotation;
    private HandManager handM;
    private Vector3 scaleChange;

    // Start is called before the first frame update
    void Start()
    {
        CardBuild();
        handM = FindObjectOfType<HandManager>();
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
        transform.position = Vector3.Lerp(transform.position, targetP, speed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void MoveTo(Vector3 moveTo, Quaternion rotateTo)
    {
        targetP = moveTo;
        targetRotation = rotateTo;
    }

    private void OnMouseOver()
    {
        if(inHand)
        {
            currentPlace = transform.position;
            Debug.Log("Touching...");
            // Gradually scale up the card to the target scale over 0.5 seconds
            StartCoroutine(ScaleCardOverTime(new Vector3(2.5f, 2.5f, 2.5f), 0.1f));
            transform.position = currentPlace + new Vector3(0f, 0.01f, -0.01f);
        }
    }

    private void OnMouseExit()
    {
        if(inHand)
        {
            Debug.Log("NOT Touching...");
            MoveTo(handM.cardPositions[handPosition], handM.leftBound.rotation);
            // Gradually scale down the card to the original scale over 0.5 seconds
            StartCoroutine(ScaleCardOverTime(new Vector3(1f, 1f, 1f), 0.5f));
            
        }
    }

    private IEnumerator ScaleCardOverTime(Vector3 targetScale, float duration)
    {
        float elapsedTime = 0f;
        Vector3 startScale = card.transform.localScale;

        while (elapsedTime < duration)
        {
            card.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        card.transform.localScale = targetScale;
    }
}

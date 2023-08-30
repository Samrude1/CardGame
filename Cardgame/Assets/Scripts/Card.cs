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
    public LayerMask desktopLayermask;
    public LayerMask placementLayermask;
    public float rayLength;
    public CardPlacement assignedPlace;

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
    private bool isSelected;
    private Collider2D cardCollider;
    private bool justPressed;

    // Start is called before the first frame update
    void Start()
    {
        CardBuild();
        handM = FindObjectOfType<HandManager>();
        cardCollider = GetComponent<Collider2D>();
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

        if(isSelected)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = -Camera.main.transform.position.z; // Set the z-coordinate to match your camera's depth

            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction, rayLength, desktopLayermask);

            if (hit2D.collider != null)
            {
                Debug.Log("RAY HITS");
                MoveTo(new Vector3(hit2D.point.x, hit2D.point.y, transform.position.z), transform.rotation);
            }
            if(Input.GetMouseButtonDown(1))
            {
                ReturnToHand();
            }
            if(Input.GetMouseButtonDown(0) && justPressed == false)
            {
                Ray ray2 = Camera.main.ScreenPointToRay(mousePosition);

                RaycastHit2D hit2D2 = Physics2D.Raycast(ray2.origin, ray2.direction, rayLength, placementLayermask);
                if (hit2D2.collider != null) 
                {
                    Debug.Log("RAY HITS");
                    CardPlacement selectedPoint = hit2D2.collider.GetComponent<CardPlacement>();
                    if (selectedPoint.activeCard == null && selectedPoint.isPlayerPoint)
                    {
                        selectedPoint.activeCard = this;
                        assignedPlace = selectedPoint;

                        MoveTo(selectedPoint.transform.position, transform.rotation);
                        inHand = false;
                        isSelected = false;
                    }
                    else
                    {
                        ReturnToHand() ;
                    }
                }
                else
                {
                    ReturnToHand();
                }
            }
        }
        justPressed = false;
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
            //Debug.Log("Touching...");
            // Gradually scale up the card to the target scale over 0.5 seconds
            StartCoroutine(ScaleCardOverTime(new Vector3(2.5f, 2.5f, 2.5f), 0.1f));
            transform.position = currentPlace + new Vector3(0f, 0.01f, -0.01f);
        }
    }

    private void OnMouseExit()
    {
        if(inHand)
        {
            //Debug.Log("NOT Touching...");
            MoveTo(handM.cardPositions[handPosition], handM.leftBound.rotation);
            // Gradually scale down the card to the original scale over 0.5 seconds
            StartCoroutine(ScaleCardOverTime(new Vector3(1f, 1f, 1f), 0.5f));
            transform.position = currentPlace + new Vector3(0f, 0.01f, 1.01f);
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

    private void OnMouseDown()
    {
        if(inHand)
        {
            isSelected = true;
            cardCollider.enabled = false;
            justPressed = true;
        }
        
    }

    public void ReturnToHand()
    {
        isSelected = false;
        cardCollider.enabled = true;

        MoveTo(handM.cardPositions[handPosition], handM.leftBound.rotation);
    }
}

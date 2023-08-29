using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public List<Card> cards = new List<Card>();
    public Transform leftBound, rightBound;
    public List<Vector3> cardPositions = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        SetUpCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUpCards()
    {
        cardPositions.Clear();

        Vector3 distanceBetween = Vector3.zero;
        if(cards.Count > 1 )
        {
            distanceBetween = (rightBound.position - leftBound.position) / (cards.Count - 1);
        }

        for(int i = 0; i < cards.Count; i++)
        {
            cardPositions.Add(leftBound.position + (distanceBetween * i));
            cards[i].transform.position = cardPositions[i];
            cards[i].transform.rotation = leftBound.rotation;
        }
    }

}

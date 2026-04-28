using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardGame : MonoBehaviour
{
    public int pairCount;                 // 인스펙터 입력
    public GameObject cardPrefab;
    public Transform cardParent;
    public List<Sprite> sprites;

    private List<Card> cards = new List<Card>();

    private Card FirstCard = null;
    private Card SecondCard = null;
    private bool isChecking = false;

    void Start()
    {
        GenerateCards();
    }

    void GenerateCards()
    {
        List<int> numbers = new List<int>();

        // 페어 생성
        for (int i = 0; i < pairCount; i++)
        {
            numbers.Add(i);
            numbers.Add(i);
        }

        Shuffle(numbers);

        // 카드 생성
        foreach (int num in numbers)
        {
            GameObject obj = Instantiate(cardPrefab, cardParent);
            Card card = obj.GetComponent<Card>();

            card.cardGame = this;
            card.SetCardNumber(num);
            card.SetFrontImage(sprites[num]);

            cards.Add(card);
        }
    }

    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            int temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }

    public void OnClickCard(Card card)
    {
        if (isChecking) return;

        card.Flip(true);

        if (FirstCard == null)
        {
            FirstCard = card;
        }
        else
        {
            SecondCard = card;
            StartCoroutine(CheckCard());
        }
    }

    IEnumerator CheckCard()
    {
        isChecking = true;

        yield return new WaitForSeconds(0.35f);

        if (FirstCard.newNumber == SecondCard.newNumber)
        {
            FirstCard.isMatched = true;
            SecondCard.isMatched = true;
        }
        else
        {
            FirstCard.Flip(false);
            SecondCard.Flip(false);

            yield return new WaitForSeconds(0.35f);
        }

        FirstCard = null;
        SecondCard = null;
        isChecking = false;
    }

   
}
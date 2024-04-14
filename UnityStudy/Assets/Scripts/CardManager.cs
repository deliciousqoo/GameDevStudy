using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardManager : MonoBehaviour
{
    private const int HANDFUL = 5;
    enum cardType { CLUB, HEART, DIAMOND, SPADE, RANK_TYPE };
    enum cardNum { SIX = 6, SEVER, EIGHT, NINE, TEN, JACK, QUEEN, KING, ACE, RANK_NUM };
    enum deckResult { Top, JTop, OnePair, TwoPair, Triple, Straight, FullHouse, BackStraight, Mountain, Flush, FourCard, StraightFlush, BackStraightFlush, RoyalFlush, RANK_RESULT };

    public Card initialValue;
    public List<Card> cardList = new List<Card>();
    public List<Card> myDeck = new List<Card>();

    private List<Card> sortedCard;


    public void Start()
    {
        for (int i = 0; i < HANDFUL; i++) myDeck.Add(initialValue);
        for(int i=0;i<10000000;i++)
        {
            for(int j=0;j<myDeck.Count;j++) 
            {
                CardChange(j);
            }
            sortedCard = myDeck.OrderByDescending(x => x.cardNum).ToList();
            Debug.Log(ComputeResult(sortedCard, 5));
            for (int j = 0; j < cardList.Count; j++) cardList[j].isChoice = false;
        }
    }
    public void CardChange(int cardOrder)
    {
        int check, i;

        for (i = 0; i < cardList.Count; i++)
        {
            check = Random.Range(0, cardList.Count);
            if (!cardList[check].isChoice)
            {
                cardList[check].isChoice = true;


                CardRemoveAndInsert(cardOrder, check);
                break;
            }
        }
    }

    public void CardRemoveAndInsert(int myCardOrder, int cardListOrder) // 내덱에서 해당 Order에 있는 카드 제거후 바뀐카드 삽입하기 
    {
        myDeck.RemoveAt(myCardOrder);
        myDeck.Insert(myCardOrder, cardList[cardListOrder]);
    }

    private deckResult ComputeResult(List<Card> cards, int handful) // ���� ���
    {
        bool straight = false, royal = false, flush = false;
        bool FoK = false, ToK = false, back = false;
        int numPair = 0, i = 0, straightCounter = 0;
        int[] numRanks = new int[15];
        deckResult handType = deckResult.Top;


        for (i = 1; i < handful; i++)
        {
            if (cards[i].cardNum == cards[i - 1].cardNum - 1) straightCounter++;
        }

        if (straightCounter == handful - 1)
        {
            straight = true;
            if (cards[0].cardNum == (int)cardNum.ACE) royal = true;
        }

        if (straightCounter == handful - 2 && cards[0].cardNum == (int)cardNum.ACE && cards[1].cardNum == (int)cardNum.NINE)
        {
            back = true;
        }

        for (i = 1; i < handful; i++)
        {
            if (cards[i].cardType != cards[i - 1].cardType) break;
        }
        if (i == handful) flush = true;


        for (i = 0; i < handful; i++) numRanks[cards[i].cardNum]++;
        for (i = 0; i < (int)cardNum.RANK_NUM; i++)
        {
            switch (numRanks[i])
            {
                case 2: numPair++; break;
                case 3: ToK = true; break;
                case 4: FoK = true; break;
            }
        }

        if (royal && flush) handType = deckResult.RoyalFlush;
        else if (back && flush) handType = deckResult.BackStraightFlush;
        else if (straight && flush) handType = deckResult.StraightFlush;
        else if (FoK) handType = deckResult.FourCard;
        else if (flush) handType = deckResult.Flush;
        else if (back) handType = deckResult.BackStraight;
        else if (straight && cards[0].cardNum == (int)cardNum.ACE) handType = deckResult.Mountain;
        else if (ToK && numPair == 1) handType = deckResult.FullHouse;
        else if (straight) handType = deckResult.Straight;
        else if (ToK) handType = deckResult.Triple;
        else if (numPair == 2) handType = deckResult.TwoPair;
        else if (numPair == 1) handType = deckResult.OnePair;
        else if (cards[0].cardNum == (int)cardNum.JACK) handType = deckResult.JTop;

        return handType;
    }

    [System.Serializable]
    public class Card
    {
        public int cardType;
        public int cardNum;
        public bool isChoice;
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Card : MonoBehaviour
{
    public GameObject front;   // 앞면
    public GameObject back;    // 뒷면


    public Transform visual;
    public TextMeshProUGUI text;
    public int newNumber;

    public bool isFront = false;
    public bool isMatched = false;
    public bool isAnimating = false;

    public CardGame cardGame;

    public void Clickcard()
    {
        if (isMatched || isFront || isAnimating) return;
        cardGame.OnClickCard(this);
    }

    public void Flip(bool showFront)
    {
        if (!isAnimating)
            StartCoroutine(FlipAnimation(showFront));
    }

    IEnumerator FlipAnimation(bool showFront)
    {
        isAnimating = true; // 돌아가는 동안 클릭 못하게

        float duration = 0.3f; //애니메이션 0.3초 진행
        float time = 0f;

        Quaternion startRot = visual.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(0, 180, 0);

        bool swapped = false;

        while (time < duration)  //0.3초 동안 계속 반복하면서 조금씩 움직임
        {
            time += Time.deltaTime; // 한 프레임마다 지난 시간을 더해 전체 진행도 계산
            float t = Mathf.SmoothStep(0, 1, time / duration); //회전을 부드럽게 하기 위함

            visual.rotation = Quaternion.Lerp(startRot, endRot, t);

            if (!swapped && t >= 0.5f) // 카드가 절반정도 돌았을 때 이미지 변경
            {
                front.SetActive(showFront);
                back.SetActive(!showFront);

                // 텍스트 표시 제어 추가
                text.gameObject.SetActive(showFront);

                swapped = true;
            }

            yield return null; // 한 프레임 쉬었다가 다시 실행
        }

        visual.rotation = endRot;
        isFront = showFront;
        isAnimating = false;
    }

    public void SetCardNumber(int number)
    {
        newNumber = number;
        text.text = number.ToString();
    }

    public void SetFrontImage(Sprite sprite)
    {
        front.GetComponent<Image>().sprite = sprite;
    }
}
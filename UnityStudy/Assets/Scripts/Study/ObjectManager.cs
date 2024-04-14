using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class ObjectManager : MonoBehaviour
{
    [SerializeField]
    private GameObject target1, target2;
    private bool check = false;

    private Color orgColor = new Color(1f, 1f, 1f, 1f);

    public void SetBlink()
    {
        if (!check)
        {
            StartCoroutine("Blink", target1);
            check = true;
        }
        else
        {
            StopCoroutine("Blink");
            target1.GetComponent<Image>().color = orgColor;
            check = false;
        }
    }

    public void SetEmphasize()
    {
        if (!check)
        {
            StartCoroutine("Emphasis", target1);
            check = true;
        }
        else
        {
            StopCoroutine("Emphasis");
            check = false;
        }
    }

    public void SetMovement_X()
    {
        if (!check)
        {
            target1.transform.position = new Vector3(1195f, 430f, 0f);
            StartCoroutine(UIMovement_X(target1, new Vector3(520f, 430f, 0f), target1.transform.position, 0));
            check = true;
        }
        else
        {
            StartCoroutine(UIMovement_X(target1, new Vector3(-115f, 430f, 0f), target1.transform.position, 2));
            check = false;
        }
    }

    public void SetMovement_Y()
    {
        if (!check)
        {
            target2.transform.position = new Vector3(840f, 835f, 0f);
            StartCoroutine(UIMovement_Y(target2, new Vector3(840f, 360f, 0f), target2.transform.position, 0));
            check = true;
        }
        else
        {
            StartCoroutine(UIMovement_Y(target2, new Vector3(840f, -165f, 0f), target2.transform.position, 2));
            check = false;
        }
    }

    private IEnumerator Emphasis(GameObject gameObject)
    {
        float increase = 0.1f;

        while (true)
        {
            while (gameObject.GetComponent<Transform>().localScale.x > 0.5f)
            {
                gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x - increase
                                                                , gameObject.transform.localScale.y - increase
                                                                , gameObject.transform.localScale.z - increase);

                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(0.05f);
            while (gameObject.GetComponent<Transform>().localScale.x < 1f)
            {
                gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x + increase
                                                                , gameObject.transform.localScale.y + increase
                                                                , gameObject.transform.localScale.z + increase);
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator UIMovement_X(GameObject target, Vector3 targetPos, Vector3 orgPos, int count)
    {
        //Set Origin Position
        target.transform.position = orgPos;
        Vector3 temp_pos = target.transform.position;

        //Set Direction
        float dir;
        if (targetPos.x < orgPos.x) dir = -1;
        else dir = 1;

        //Move to Target Position
        float x = 1, slope = 0.04f;
        yield return new WaitForSecondsRealtime(0.03f);
        while ((targetPos.x - target.transform.position.x) * dir > 0)
        {
            temp_pos.x = dir * slope * Mathf.Pow(x, 2) + orgPos.x;
            target.transform.position = temp_pos;
            x += 1f;
            yield return new WaitForSecondsRealtime(0.001f);
        }

        target.transform.position = targetPos;

        //Repeat for Vibrate
        if (count == 0) StartCoroutine(UIMovement_X(target, new Vector3(targetPos.x + 30f * -dir, target.transform.position.y, 0f), target.transform.position, count + 1));
        else if (count == 1) StartCoroutine(UIMovement_X(target, new Vector3(targetPos.x + 10f * -dir, target.transform.position.y, 0f), target.transform.position, count + 1));
    }

    private IEnumerator UIMovement_Y(GameObject target, Vector3 targetPos, Vector3 orgPos, int count)
    {
        //Set Origin Position
        target.transform.position = orgPos;
        Vector3 temp_pos = target.transform.position;

        //Set Direction
        float dir;
        if (targetPos.y < orgPos.y) dir = -1;
        else dir = 1;

        //Move to Target Position
        float x = 1, slope = 0.04f;
        yield return new WaitForSecondsRealtime(0.03f);
        while ((targetPos.y - target.transform.position.y) * dir > 0)
        {
            temp_pos.y = dir * slope * Mathf.Pow(x, 2) + orgPos.y;
            target.transform.position = temp_pos;
            x += 1f;
            yield return new WaitForSecondsRealtime(0.001f);
        }

        target.transform.position = targetPos;

        //Repeat for Vibrate
        if (count == 0) StartCoroutine(UIMovement_Y(target, new Vector3(target.transform.position.x, target.transform.position.y + 30f * -dir, 0f), target.transform.position, count + 1));
        else if (count == 1) StartCoroutine(UIMovement_Y(target, new Vector3(target.transform.position.x, target.transform.position.y + 10f * -dir, 0f), target.transform.position, count + 1));
    }

    private IEnumerator Blink(GameObject gameObject)
    {
        Color tempColor = orgColor;

        gameObject.GetComponent<Image>().color = tempColor;
        while (true)
        {

            while (gameObject.GetComponent<Image>().color.a > 0f)
            {
                tempColor.a -= 0.1f;
                gameObject.GetComponent<Image>().color = tempColor;
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(0.5f);
            while (gameObject.GetComponent<Image>().color.a < 1f)
            {
                tempColor.a += 0.1f;
                gameObject.GetComponent<Image>().color = tempColor;
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}

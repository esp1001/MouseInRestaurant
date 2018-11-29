using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {


    void Update()
    {
        if (Input.touchCount > 0)
        {
            foreach (var curTouch in Input.touches)
            {
                if (curTouch.phase == TouchPhase.Began)
                {
                    CheckWhatUnderTouch(curTouch);
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            CheckWhatUnderMouse(Input.mousePosition);
        }
    }

    void CheckWhatUnderTouch(Touch touch)
    {
        var touchPosWorld = Camera.main.ScreenToWorldPoint(touch.position);

        Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

        RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

        if (hitInformation.collider != null && hitInformation.collider.tag == "Mouse")
        {
            GameObject touchedObject = hitInformation.transform.gameObject;
            StartCoroutine(DatabaseManager.Instance.SendToTelegram("Touched " + touchedObject.transform.name + "/" + touchedObject.tag));
        }
    }

    void CheckWhatUnderMouse(Vector3 mouseClickPosition)
    {

        var touchPosWorld = Camera.main.ScreenToWorldPoint(mouseClickPosition);

        Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

        RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

        if (hitInformation.collider != null && hitInformation.collider.tag == "Mouse")
        {
            GameObject touchedObject = hitInformation.transform.gameObject;
            //StartCoroutine(DatabaseManager.Instance.SendToTelegram("Touched " + touchedObject.transform.name + "/" + touchedObject.tag));
            touchedObject.GetComponent<BezierFollow>().OnMouseClick();
        }
    }

}

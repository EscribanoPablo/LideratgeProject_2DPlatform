using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private RectTransform _telon;

    [SerializeField] private Vector2 _telonHidePos;
    [SerializeField] private Vector2 _telonShownPos;
    [SerializeField] private float _telonAppearTime;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static void ShowTelon()
    {
        Instance.StartCoroutine(Instance.COTelonMovement(Instance._telonHidePos, 
                                                        Instance._telonShownPos, 
                                                        Instance._telonAppearTime));
    }

    public static void HideTelon()
    {
        Instance.StartCoroutine(Instance.COTelonMovement(Instance._telonShownPos, 
                                                        Instance._telonHidePos, 
                                                        Instance._telonAppearTime));
    }


    private IEnumerator COTelonMovement(Vector2 from, Vector2 to, float time)
    {
        float elapsedTime = 0;
        Vector2 currentPos = from;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;

            float x = Mathf.Lerp(from.x, to.x, elapsedTime / time);
            float y = Mathf.Lerp(from.y, to.y, elapsedTime / time);

            currentPos = new Vector2(x, y);
            _telon.anchoredPosition = currentPos;

            yield return null;
        }
    }
}

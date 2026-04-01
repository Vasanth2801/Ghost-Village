using UnityEngine;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private GameObject winScreenUI;

    void OnTriggerEnter2D(Collider2D collision)
    {
        winScreenUI.SetActive(true);
        Time.timeScale = 0f;
    }
}
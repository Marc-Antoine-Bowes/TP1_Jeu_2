using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private AudioClip gameOverClip;
    [SerializeField] private AudioClip gameClip;

    private AudioSource audioSource;
    private bool isGameOver = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = gameClip;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(isGameOver)
        {
            audioSource.Stop();
            audioSource.clip = gameOverClip;
            audioSource.Play();
            gameOverUI.SetActive(true);
        }
    }

    public void GameOver()
    {
        isGameOver = true;
    }
}

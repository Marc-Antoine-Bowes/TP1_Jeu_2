using UnityEngine;

public class Spawner_Life_Manager : MonoBehaviour
{
    [SerializeField] private int hp = 1;
    public void hurt()
    {
        hp -= 1;

        if (hp < 0)
        {
            gameObject.SetActive(false);
        }
    }

}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToPlay : MonoBehaviour
{
    public List<GameObject> playArray;
    public int currentIndex = 0;
    public void NextButton()
    {
        currentIndex++;
        for (int i = 0; i < playArray.Count; i++)
        {
            playArray[i].SetActive(false);
            if (i == currentIndex)
            {
                playArray[i].SetActive(true);
            }
        }
        if (currentIndex >= playArray.Count)
        {
            SceneManager.LoadScene("Game");
        }
    }
}

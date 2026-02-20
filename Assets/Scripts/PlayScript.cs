using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayScript : MonoBehaviour, IPointerEnterHandler
{
    public AudioClip[] hoverbtnSFX;
    public AudioClip[] startbtnSFX;

    public Image Fade;

    private void Start()
    {
        SoundManager.Instance.PlayMusicClip(MusicType.MainMenu);
    }
    public void StartGame()
    {
        Fade.gameObject.SetActive(true);
        SoundManager.Instance.PlayRandomSFXClip(startbtnSFX, transform, true, 1f);
        SoundManager.Instance.TransitionMusicClip(MusicType.Game, 1f);
        LMotion.Create(0f, 1f, 1f)
            .WithOnComplete(() => SceneManager.LoadScene("Game"))
            .BindToColorA(Fade);
    }
    
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Hover();
    }
    private void Hover()
    {
        SoundManager.Instance.PlayRandomSFXClip(hoverbtnSFX, transform, true, 1f);
    }
}

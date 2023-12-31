using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VNCanvasController : MonoBehaviour
{
    public static VNCanvasController Shared { get; private set; }

    public GameObject messageHolder;
    public Image avatarField;
    public TextMeshProUGUI nameField;
    public TypewriterEffect messageField;
    public VerticalLayoutGroup buttonsHolder;
    public Button buttonPrefab;
    public Image bgImage;
    public Image bgPicImage;
    
    public AudioSource audioSource;
    
    public RawImage videoImage;
    public VideoPlayer videoPlayer;

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Shared = this;
        messageHolder.SetActive(false);
    }

    public static void ShowMessageCanvas(bool show)
    {
        Shared.messageHolder.SetActive(show);
    }
    
    public static void SetAvatar(Sprite avatar)
    {
        Shared.avatarField.enabled = avatar != null;
        Shared.avatarField.sprite = avatar;
    }

    public static void VNTest(VNDialogItemModel item)
    {
        
    }

    public static void SetName(string name)
    {
        Shared.nameField.SetText(name);
    }

    public static IEnumerator SetMessage(string message)
    {
        yield return Shared.messageField.SetText(message);
    }

    public static void StartTekkenVideo(VideoClip clip)
    {
        Shared.videoPlayer.targetTexture.Release();
        Shared.videoPlayer.clip = clip;
        Shared.videoPlayer.enabled = true;
        Shared.videoPlayer.Play();
        Shared.videoImage.enabled = true;
    }

    public static void StopTekkenVideo()
    {
        Shared.videoPlayer.Stop();
        Shared.videoImage.enabled = false;
        Shared.videoPlayer.enabled = false;
    }

    public IEnumerator ChangeBackgroundFading(bool fade, float seconds)
    {
        bgImage.enabled = true;
        var startImageColor = bgImage.color;
        var targetImageColor = startImageColor;
        targetImageColor.a = fade ? 0 : 1;
    
        if (seconds == 0)
        {
            bgImage.color = targetImageColor;
        }
        else
        {
            var delta = Time.fixedDeltaTime;
            float counter = 0;

            while (counter <= seconds)
            {
                bgImage.color = Color.Lerp(startImageColor, targetImageColor, counter / seconds);
                counter += delta;
                yield return new WaitForSeconds(delta);
            }

            bgImage.color = targetImageColor;
        }
    }
    
    public IEnumerator SetBackgroundImage(Sprite image, bool fit, float seconds)
    {
        bgPicImage.enabled = true;
        var startImageColor = bgPicImage.color;
        var targetImageColor = startImageColor;
        targetImageColor.a = image == null ? 0 : 1;
        bgPicImage.sprite = image;

        bgPicImage.GetComponent<AspectRatioFitter>().aspectMode =
            fit ? AspectRatioFitter.AspectMode.FitInParent : AspectRatioFitter.AspectMode.EnvelopeParent;
        
        if (seconds == 0)
        {
            bgPicImage.color = targetImageColor;
        }
        else
        {
            var delta = Time.fixedDeltaTime;
            float counter = 0;
        
            while (counter <= seconds)
            {
                bgPicImage.color = Color.Lerp(startImageColor, targetImageColor, counter / seconds);
                counter += delta;
                yield return new WaitForSeconds(delta);
            }
        
            bgPicImage.color = targetImageColor;
        }
    }
}

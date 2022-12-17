using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeButton : MonoBehaviour
{
    //public SpriteRenderer VolumeSpriteRenderer;
    public Sprite MuteSprite;
    public Sprite UnmuteSprite;
    public Image VolumeImage;
    //bool muted;
    [SerializeField] Slider VolumeSlider;

    // Start is called before the first frame update
    void Start()
    {


        VolumeImage.sprite = UnmuteSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (VolumeSlider.value == 0)
        {

            VolumeImage.sprite = MuteSprite;
        }
        else {
      
            VolumeImage.sprite = UnmuteSprite;
        }
    }
}

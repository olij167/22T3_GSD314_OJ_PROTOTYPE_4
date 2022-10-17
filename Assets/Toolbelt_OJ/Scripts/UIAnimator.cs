using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class UIAnimator : MonoBehaviour
{
    public SpriteAnimation uiAnimation;
    public float timePerFrame;
    //public string animationID;

    private Image uiDisplay;

    public int count;
    void Start()
    {
        timePerFrame = uiAnimation.timePerFrameReset;
        uiDisplay = GetComponent<Image>();
    }

    private void Update()
    {
        Animate();
    }

    public void Animate()
    {
        timePerFrame -= Time.deltaTime;

        if (timePerFrame <= 0)
        {
            count++;

            if (count >= uiAnimation.animationFrames.Count)
            {
                count = 0;
            }

            uiDisplay.sprite = uiAnimation.animationFrames[count];
            timePerFrame = uiAnimation.timePerFrameReset;

        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public Transform[] images;

    public RectTransform parent;

    public int imagesOnScreen = 3;
    public float spaceBetweenImages;
    public float imageHeight;

    public float moveSpeed;

    public float maxThreshold;


    private void Start()
    {
        Init();
        SetImagesPosition();
    }

    private void Init()
    {
        float totalHeight = parent.rect.height;
        imageHeight = images[0].GetComponent<RectTransform>().rect.height;

        float freeHeight = totalHeight - (imageHeight * imagesOnScreen);

        spaceBetweenImages = freeHeight / ((imagesOnScreen - 1) + 2);

        maxThreshold = 2 * (imageHeight + spaceBetweenImages);
    }

    private void SetImagesPosition()
    {
        images[0].localPosition = new Vector3(0, -(imageHeight + spaceBetweenImages), 0);
        images[1].localPosition = Vector3.zero;

        for (int i = 2; i < images.Length; i++)
        {
            images[i].localPosition = new Vector3(0, (imageHeight + spaceBetweenImages) * (i - 1), 0);
        }
    }

    private void MoveImages()
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].localPosition += moveSpeed * Time.deltaTime * Vector3.down;  

            if(images[i].localPosition.y < -maxThreshold)
            {
                if(i == 0)
                {
                    images[i].localPosition = images[images.Length - 1].localPosition + new Vector3(0, imageHeight + spaceBetweenImages, 0);
                }
                else
                {
                    images[i].localPosition = images[i - 1].localPosition + new Vector3(0, imageHeight + spaceBetweenImages, 0);
                }
            }
        }
    }

    private void StopImages()
    {

    }

    private void Update()
    {
        MoveImages();
    }
}

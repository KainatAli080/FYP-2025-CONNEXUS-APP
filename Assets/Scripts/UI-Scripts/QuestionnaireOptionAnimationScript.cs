using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionnaireOptionAnimationScript : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private Image currentBgImage;

    [SerializeField] private Sprite checkedBgImage;
    [SerializeField] private Sprite uncheckedBgImage;


    // Start is called before the first frame update
    void Start()
    {
        toggle = GetComponent<Toggle>();
        currentBgImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (toggle.isOn)
        {
            currentBgImage.sprite = checkedBgImage;
        }
        else
        {
            currentBgImage.sprite = uncheckedBgImage;
        }
    }
}

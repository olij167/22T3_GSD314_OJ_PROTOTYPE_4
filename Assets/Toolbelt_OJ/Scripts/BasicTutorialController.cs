using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Toolbelt_OJ
{
    public class BasicTutorialController : MonoBehaviour
    {
        [TextArea(3, 10)] // for inspector
        public List<string> tutorialInfoList;

        public TextMeshProUGUI tutorialText;

        public GameObject menuUI, tutorialUI;

        public int count;

        void Start()
        {
            tutorialUI.SetActive(false);
        }

        public void StartTutorial()
        {
            menuUI.SetActive(false);
            tutorialUI.SetActive(true);

            tutorialText.text = tutorialInfoList[0];
            //foreach (TextMeshProUGUI text in tutorialTextList)
            //{
            //    text.enabled = false;
            //}

            count = 0;
        }

        public void NextTutorialSection()
        {
            if (count > tutorialInfoList.Count - 1)
            {
                menuUI.SetActive(true);
                tutorialUI.SetActive(false);
            }
            else
            {
                count++;

                if (count > tutorialInfoList.Count - 1)
                {
                    menuUI.SetActive(true);
                    tutorialUI.SetActive(false);
                }

                if (count <= tutorialInfoList.Count - 1)
                {
                    tutorialText.text = tutorialInfoList[count];
                }
            }
        }

        public void SkipToEnd()
        {
            tutorialText.text = tutorialInfoList[tutorialInfoList.Count - 1];
            tutorialUI.SetActive(false);
            menuUI.SetActive(true);

        }
    }
}

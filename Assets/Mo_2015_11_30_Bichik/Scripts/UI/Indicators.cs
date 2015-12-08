using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace UI
{
    public class Indicators : MonoBehaviour
    {

        private List<GameObject> heroIndicators;
        private List<GameObject> helpIndicators;

        // Use this for initialization
        void Start()
        {
            heroIndicators = new List<GameObject>();
            helpIndicators = new List<GameObject>();

            for (int i = 0; i < int.MaxValue; i++)
            {
                var ind = GameObject.Find("HeroInd_" + i.ToString());
                if (ind == null)
                    break;
                heroIndicators.Add(ind);
            }

            for (int i = 0; i < int.MaxValue; i++)
            {
                var ind = GameObject.Find("HelpInd_" + i.ToString());
                if (ind == null)
                    break;
                helpIndicators.Add(ind);
            }

        }

        public void setHeroIndicatorChecked(int number)
        {
            setAllHeroIndicatorsUnchecked();
            heroIndicators[number].GetComponent<Toggle>().isOn = true;
        }

        private void setAllHeroIndicatorsUnchecked()
        {
            foreach (var ind in heroIndicators)
            {
                ind.GetComponent<Toggle>().isOn = false;
            }
        }

        public void setHelpIndicatorChecked(int number)
        {
            setAllHelpIndicatorsUnchecked();
            helpIndicators[number].GetComponent<Toggle>().isOn = true;
        }

        private void setAllHelpIndicatorsUnchecked()
        {
            foreach (var ind in helpIndicators)
            {
                ind.GetComponent<Toggle>().isOn = false;
            }
        }
    }
}
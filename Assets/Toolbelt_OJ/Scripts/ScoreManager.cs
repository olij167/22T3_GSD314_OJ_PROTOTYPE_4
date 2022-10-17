using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolbelt_OJ
{
    [System.Serializable]
    public class ScoreManager : MonoBehaviour
    {
        public float score, startScore;
        public List<float> previousScores;

        public void Initialise()
        {
            CreateScoreList();
            ResetScore();
        }

        public void ChangeScore(float amount)
        {
            score += amount;
        }

        public void ResetScore()
        {
            score = startScore;
        }

        public void CreateScoreList()
        {
            previousScores = new List<float>();
        }

        public void TrackPreviousScore()
        {
            previousScores.Add(score);
        }

    }
}

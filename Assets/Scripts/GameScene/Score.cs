using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static Score instance;


    public int score { get; private set; }

    private Text _textComp;
    private Animator _animatorComp;

    private void Awake()
    {
        instance = this;

        _textComp = GetComponent<Text>();
        _animatorComp = GetComponent<Animator>();
    }

    public void ChangeScoreWhenHit(int scoreAdd, Hit ingredientHit)
    {
        score += scoreAdd;
        _textComp.text = score.ToString();
        PlayeAnimationScoreHit(ingredientHit);

        if (score < 0)
        {
            _textComp.text = "0";
            GameOver.instance.GameOverFunk();
            print("Score less 0");
        }
    }


    private void PlayeAnimationScoreHit(Hit ingredientHit)
    {
        if (ingredientHit == Hit.Perfect)
        {
            _animatorComp.Play("ScoreTextAnimPerfect");
        }
        else if(ingredientHit == Hit.Normal)
        {
            _animatorComp.Play("ScoreTextAnimNormal");
        }
        else
        {
            _animatorComp.Play("ScoreTextFall");
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void ResetScore()
    {
        score = 0;
        _textComp.text = "0";
    }
}

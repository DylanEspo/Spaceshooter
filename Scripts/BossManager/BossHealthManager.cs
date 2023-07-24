using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthManager : MonoBehaviour
{
    [SerializeField] int health = 0;
    [SerializeField] ParticleSystem hitEffect;

    [SerializeField] BossMainSection mMainSection;

    //TO DO - FUTURE Improvement - Generate a list of boss-subsections so we don't limit it to just two
    //TO D0 - Create a listener system so we don't have to constantly iterate through health each time, object just waits
    [SerializeField] BossSubSection mLeftWing;
    [SerializeField] BossSubSection mRightWing;

    AudioPlayer mAudioPlayer;
    ScoreKeeper mScoreKeeper;
    LevelManager mLevelManager;

    void Awake()
    {
        mAudioPlayer = FindObjectOfType<AudioPlayer>();
        mScoreKeeper = FindObjectOfType<ScoreKeeper>();
        mLevelManager = FindObjectOfType<LevelManager>();
        if(mMainSection != null)
        {
            mMainSection.SetAudioPlayer(mAudioPlayer);
            mMainSection.SetScoreKeeper(mScoreKeeper);
            health += mMainSection.GetHealth();
        }
        if (mLeftWing != null)
        {
            mLeftWing.SetAudioPlayer(mAudioPlayer);
            mLeftWing.SetScoreKeeper(mScoreKeeper);
            health += mLeftWing.GetHealth();
        }
        if (mRightWing != null)
        {
            mRightWing.SetAudioPlayer(mAudioPlayer);
            mRightWing.SetScoreKeeper(mScoreKeeper);
            health += mRightWing.GetHealth();
        }
    }

    private void Update()
    {
        if(mMainSection == null)
        {
            Die();
        }
        if(mMainSection != null)
        {
            if(mMainSection.GetHealth() <= 0)
            {
                Debug.Log("Main section destroyed");
                mMainSection.Die();
                mMainSection = null;
                Die();
            }
            else
            {
                health = mMainSection.GetHealth();
            }
        }
        if(mLeftWing != null)
        {
            if (mLeftWing.GetHealth() <= 0)
            {
                Debug.Log("Left wing destroyed");
                mLeftWing.Die();
                mLeftWing = null;
            }
            else
            {
                health += mLeftWing.GetHealth();
            }
        }
        if(mRightWing != null)
        {
            if (mRightWing.GetHealth() <= 0)
            {
                Debug.Log("Right wing destroyed");
                mRightWing.Die();
                mRightWing = null;
            }
            else
            {
                health += mRightWing.GetHealth();
            }
        }
    }

    private void Die()
    {
        if (mScoreKeeper != null)
        {
            mScoreKeeper.UpdateScore(2000);
        }
        if(mLeftWing != null)
        {
            mLeftWing.Die();
            mLeftWing = null;
        }
        if(mRightWing != null)
        {
            mRightWing.Die();
            mRightWing = null;
        }
        Destroy(gameObject);
    }

    public int GetHealth()
    {
        return health;
    }

    public void SetVulnerability(bool pState)
    {
        mMainSection.SetVulnerability(pState);
        mLeftWing.SetVulnerability(pState);
        mRightWing.SetVulnerability(pState);
    }
}


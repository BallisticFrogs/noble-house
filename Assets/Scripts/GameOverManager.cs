using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager INSTANCE;
    
    public AudioClip defeatSound;
    public AudioClip victorySound;

    public Sprite gameOverDefeatSprite;
    public Sprite gameOverVictorySprite;

    public SpriteRenderer gameOverSpriteRenderer;

    void Awake() {
        INSTANCE = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: launch a new game
    }

    public void GameOverDefeat() {
        SoundManager.INSTANCE.PlayFx(defeatSound);
        gameOverSpriteRenderer.sprite = gameOverDefeatSprite;
        gameOverSpriteRenderer.gameObject.active = true;
    }

    public void GameOverVictory() {
        SoundManager.INSTANCE.PlayFx(victorySound);
        gameOverSpriteRenderer.sprite = gameOverVictorySprite;
        gameOverSpriteRenderer.gameObject.active = true;
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GolfGameController golfController;

    [SerializeField]
    private TextMeshPro levelText;
    [SerializeField]
    private TextMeshPro highScoreText;
    [SerializeField]
    private TextMeshPro parText;
    [SerializeField]
    private TextMeshPro scoreText;
    [SerializeField]
    private TextMeshPro strokeText;

    //TextMeshPro for distance to hole
    [SerializeField]
    private TextMeshPro distanceToHoleText;

    

    public TextMeshPro timeText; // time text will be public to allow disabling it within GolfGameController

    public TextMeshPro timeLabel;


    // initialize variables in ediot
    private int _currentLevel;
    private int _par;
    private int _gameScore;
    private int _totalStrokes;
    private float _distanceToHole;
    private int _highScore;
    private float _time;



    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        _currentLevel = golfController.currentLevel;
        _par = golfController.par;
        _gameScore = golfController.GameScore;
        _totalStrokes = golfController.totalStrokes;
        _highScore = golfController.HighScore;
        _distanceToHole = golfController.distanceToHole;
        _time = golfController.CurrentTime;



        levelText.text = _currentLevel.ToString("0");
        parText.text = _par.ToString("00");
        strokeText.text = _totalStrokes.ToString("00");
        scoreText.text = _gameScore.ToString("00");
        highScoreText.text = _highScore.ToString("0000");
        timeText.text = _time.ToString("000");
        //Set the distance to hole text to the distance to hole to string with 1 decimal place
        distanceToHoleText.text = _distanceToHole.ToString("00.0");
    }

    //void GameUI()
    //{
        
    //}
}

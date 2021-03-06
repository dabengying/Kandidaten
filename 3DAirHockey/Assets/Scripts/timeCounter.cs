﻿using UnityEngine;
using UnityEngine.UI;

/* Author: Pontus Söderqvist && Malin Ejdbo
 * Last change date: 2018-05-24
 * Checked by: Malin Ejdbo
 * Date of check: 2018-04-13
 * Comment: More documentation needed.
 * Date of correction: 
*/

public class timeCounter : MonoBehaviour {

    //Variables
    public Text timerTxt, timerTxt2, countDownBlue, CountDownRed;      //Text elements to show information to user, time left of game, count down in the begining on both sides
    public bool countDown = true, repeat = true;            //booleans to determine if the times should repeat and if it should count down or not
    public float gameTime = 1.0f;                           //the length of a game in minutes
    private float minutesLeft; //the minutes left of the game
    private float secondsLeft; //the seconds left of the game
    private string minutes;
    private string seconds;
    private float miliSeconds = 0;

    public GameObject countDownTxt;
    
    public float CountDownTime = 10;
    private bool restart = true;

    public Light Light1;
    public Light Light2;
    public Light Light3;
    public Light Light4;

    public UiManager uiManager; //connect to UiManager to access restart functions
    public ScoreScript scoreScript; //connect to scoreScript to decide winner
    public GameObject puck;
    


    // Use this for initialization
    void Start()
    {
        timerTxt.text = "00:00"; //initialize the timer to "0"'
        timerTxt2.text = timerTxt.text;
        if (countDown)
        {
            minutesLeft = Mathf.Floor(gameTime); //the time left is initalized to the gameTime
            secondsLeft = (gameTime*60.0f )% 60.0f; //the seconds left are initialized
        }
        else
        {
            minutesLeft = 0; 
            secondsLeft = 0; 
        }
        
        minutes = Mathf.Floor(minutesLeft).ToString();
        seconds = Mathf.Floor(secondsLeft).ToString();

        if (minutesLeft < 10)
            minutes = "0" + minutes;
        if (secondsLeft < 10)
            seconds = "0" + seconds;

        timerTxt.text = string.Concat(string.Concat(minutes, ":"), seconds);
        timerTxt2.text = timerTxt.text;
    }

    // Update is called once per frame
    void Update()
    {
        if(restart)
        {
            
            countDownTxt.SetActive(true);            

            countDownBlue.text = Mathf.Floor(CountDownTime+1).ToString();
            countDownBlue.fontSize = 300;
            CountDownRed.text = Mathf.Floor(CountDownTime+1).ToString();
            CountDownRed.fontSize = 300;

            CountDownTime = (CountDownTime - Time.deltaTime) ;

            if (CountDownTime < 0)
            {
                restart = false;
                countDownTxt.SetActive(false);
                puck.SetActive(true);
            }

        }
        else if (countDown)
        {
            if (minutesLeft < 0 || (minutesLeft == 0 && secondsLeft == 0)) //time is out
            {
                secondsLeft = 0;
                minutesLeft = 0;
                timerTxt.text = "00:00";
                timerTxt2.text = timerTxt.text;
                if (repeat)
                {
                    minutesLeft = Mathf.Floor(gameTime); //the time lis reest to the gameTime
                    secondsLeft = (gameTime * 60.0f) % 60.0f; //the seconds are reset
                }


                //Send an event/trigger to the game to notify about the end of the round
               
                uiManager.ShowRestartCanvas();

            }
            else
            {
                if (secondsLeft <= 0) //1 minute has passed
                {
                    secondsLeft = 59.0f;//reset seconds
                    minutesLeft = minutesLeft - 1;//decrement minutes
                }

                secondsLeft = (secondsLeft - Time.deltaTime) % 60.0f; //the seconds left are calculated
                
                minutes = Mathf.Floor(minutesLeft).ToString();
                seconds = Mathf.Floor(secondsLeft+1).ToString();
                if (minutesLeft < 10) //add "0" beofre minutes
                {
                    minutes = string.Concat("0", minutes);
                }
                if (secondsLeft < 10) // add "0" before seconds
                {
                    seconds = string.Concat("0", seconds);
                }

                //Start the countdown
                if (minutesLeft < 1 && secondsLeft < 5)
                {
                    countDownTxt.SetActive(true);

                    Light1.color = Color.red;
                    Light2.color = Color.red;
                    Light3.color = Color.red;
                    Light4.color = Color.red;

                    Light1.intensity = 0.5f;
                    Light2.intensity = 0.5f;
                    Light3.intensity = 0.5f;
                    Light4.intensity = 0.5f;

                    countDownBlue.text = Mathf.Floor(secondsLeft + 1).ToString();
                    countDownBlue.fontSize = 600;
                    CountDownRed.text = Mathf.Floor(secondsLeft + 1).ToString();
                    CountDownRed.fontSize = 600;

                    // countDownTxt.SetActive(false);
                }

                timerTxt.text = string.Concat(string.Concat(minutes, ":"), seconds);     //save the output to the text
                timerTxt2.text = timerTxt.text;
            }
        }
        else //count up
        {
            if ( repeat && (minutesLeft*60 + secondsLeft) > gameTime*60) //time is out
            {
                secondsLeft = 0;
                minutesLeft = 0;
                timerTxt.text = "00:00";
                timerTxt2.text = timerTxt.text;
                //Send an event/trigger to the game to notify about the end of the round
                uiManager.ShowRestartCanvas();
            }
            else
            {                
                secondsLeft = (secondsLeft + Time.deltaTime); //the seconds left are calculated

                if (secondsLeft >= 60) //1 minute has passed
                {
                    secondsLeft = 0.0f;//reset seconds
                    minutesLeft = minutesLeft + 1.0f;//increment minutes                    
                }
                if (minutesLeft < 1) //change to ss:ms
                {
                    miliSeconds = (secondsLeft * 60 + Time.deltaTime * 60) % 60; // claculate milisecodns
                    minutes = Mathf.Floor(secondsLeft).ToString();
                    seconds = Mathf.Floor(miliSeconds).ToString();
                    if (secondsLeft < 10) //add "0" before minutes if secondsLeft < 10
                    {
                        minutes = string.Concat("0", minutes);
                    }
                    if (miliSeconds < 10) //add "0" before seconds if miliseconds  < 10
                    {
                        seconds = string.Concat("0", seconds);
                    }

                    timerTxt.text = string.Concat(string.Concat(minutes, ":"), seconds);  //save the output to the text
                    timerTxt2.text = timerTxt.text;
                }
                else // mm:ss
                {
                    minutes = Mathf.Floor(minutesLeft).ToString();
                    seconds = Mathf.Floor(secondsLeft).ToString();
                    if (minutesLeft < 10) //add "0" beofre minutes
                    {
                        minutes = string.Concat("0", minutes);
                    }
                    if (secondsLeft < 10) // add "0" before seconds
                    {
                        seconds = string.Concat("0", seconds);
                    }

                    timerTxt.text = string.Concat(string.Concat(minutes, ":"), seconds);     //save the output to the text
                    timerTxt2.text = timerTxt.text;
                }
            }            
        }
    }
}

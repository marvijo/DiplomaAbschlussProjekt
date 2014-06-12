﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InGameGUI : MonoBehaviour {

    public PlayerController[] playerList;

    private float[] currentExperienceGUI = { 0f, 0f, 0f, 0f };
    private float[] currentHealthGUI = { 0f, 0f, 0f, 0f };

    public float healthChange = 4.0f;
    public float experienceChange = 2.0f;

	// Update is called once per frame
	void Update () {
        if (!Game.Paused)
        {
            for (int i = 0; i < playerList.Length; i++)
            {
                PlayerController item = playerList[i];

                currentExperienceGUI[i] = Mathf.Lerp(currentExperienceGUI[i], item.currentExperience, experienceChange * Time.deltaTime);
                currentHealthGUI[i] = Mathf.Lerp(currentHealthGUI[i], item.playerClass.Health, healthChange * Time.deltaTime);
            }
            if (InputController.GetClicked("LEVELUP"))
            {
                LevelUpGUI.OpenPlayer(playerList[0]);
            }
            if (InputController.GetClicked("ESCAPE"))
            {

            }
        }
        else
        {
            //Paused only in Menus so check them
            //If Escape is pressed close current menü
            if (InputController.GetClicked("ESCAPE"))
            {
                if(LevelUpGUI.IsOpened() && LevelUpGUI.WantsToClose())
                {
                    LevelUpGUI.Close();
                }
            }
        }
       
	}

    public EnemieSpawner spawner;

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 200, 200));

        for (int i = 0; i < playerList.Length; i++)
        {
            PlayerController item = playerList[i];

            GUILayout.BeginHorizontal();
            GUILayout.Label(string.Format("{0} LvL:{1}", item.Name, item.level.ToString()));
            GUILayout.Label(string.Format("Money: {0}", item.Money.ToString()));
            if (spawner)
            {
                GUILayout.Label(string.Format("Difficulty: {0}", spawner.timedValue.ToString()));
            }
            GUILayout.EndHorizontal();
            GUILayout.HorizontalSlider(currentExperienceGUI[i], item.prevNeededExperience, item.neededExperience);
            GUILayout.Label("Health: " + item.playerClass.Health.ToString() + " of " + item.playerClass.GetAttributeValue(AttributeType.HEALTH));
            GUILayout.HorizontalSlider(currentHealthGUI[i], 0f, item.playerClass.GetAttributeValue(AttributeType.HEALTH));
        }
        
        GUILayout.EndArea();
    }
}
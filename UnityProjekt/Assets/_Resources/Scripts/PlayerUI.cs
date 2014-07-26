﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class ValueChances
{
    public int value;
    public float weight;
}

[System.Serializable]
public class PlayerUI : MonoBehaviour {

    public PlayerController playerControl;

    public float UpdateTimer = 0.5f;

    public UIEditorPanel panel;

    public UIText level;
    public UIRect HealthBar;
    public UIText HealthText;
    public UIText Money;

    public UIText Exp;
    public UIRect ExpBar;


    public Tab_Control SkillAndItemMenu;
    public UIButton SkillTabButton;

    public UIRect skillPanel;

    public UIText CheckPointText;
    [SerializeField]
    private float currentHealth = 0, wantedHealth = 0;
    [SerializeField]
    private float currentMaxHealth = 0;

    [SerializeField]
    private float currentExp = 0, wantedExp = 0;
    [SerializeField]
    private float currentMoney = 0, wantedMoney = 0;

    public float speed = 2.0f;

    public UIText skillCD1, skillCD2, skillCD3, skillCD4;

    public string UIItemPoolName = "UIItem";

    public GameObject UIItemHolder;

    public int MoneyPerItem25 = 300, MoneyPerItem50 = 800, MoneyPerItem75 = 1500, MoneyPerItem100 = 3000;

    public ValueChances[] Chances25;
    public ValueChances[] Chances50;
    public ValueChances[] Chances75;
    public ValueChances[] Chances100;

    public UIButton Button25, Button50, Button75, Button100;

	// Use this for initialization
	void Start () {
        SkillAndItemMenu.GetComponent<UIRect>().Visible = showMenu;

        Button25.OnButtonClicked += GenerateItems25;
        Button50.OnButtonClicked += GenerateItems50;
        Button75.OnButtonClicked += GenerateItems75;
        Button100.OnButtonClicked += GenerateItems100;
	}

    public void GenerateItems25(UIRect rect)
    {
        //float money = playerControl.Money * 0.25f;
        //int amount = (int)(money / MoneyPerItem25);
        playerControl.Money -= MoneyPerItem25;
        GenerateItems(Chances25, 1);
    }
    public void GenerateItems50(UIRect rect)
    {
        //float money = playerControl.Money * 0.50f;
        //int amount = (int)(money / MoneyPerItem50);
        playerControl.Money -= MoneyPerItem50;
        GenerateItems(Chances50, 1);
    }
    public void GenerateItems75(UIRect rect)
    {
        //float money = playerControl.Money * 0.75f;
        //int amount = (int)(money / MoneyPerItem75);
        playerControl.Money -= MoneyPerItem75;
        GenerateItems(Chances75, 1);
    }
    public void GenerateItems100(UIRect rect)
    {
        //float money = playerControl.Money * 1f;
        //int amount = (int)(money / MoneyPerItem100);
        playerControl.Money -= MoneyPerItem100;
        GenerateItems(Chances100, 1);
    }

    public void GenerateItems(ValueChances[] values, int amount)
    {
        for (int c = 0; c < amount; c++)
        {
            var rnd = Random.value;
            for (int i = 0; i < values.Length; i++)
            {
                if (rnd < values[i].weight)
                {
                    AddItem(values[i].value);
                    return;
                }
                rnd -= values[i].weight;
            }
        }
        
    }

    public void AddItem(int value)
    {
        Item newItem = ItemGenerator.GenerateItem(value);

        playerControl.PlayerClass.AddItem(newItem);

        GameObject go = GameObjectPool.Instance.Spawn(UIItemPoolName, Vector3.zero, Quaternion.identity);
        go.transform.parent = UIItemHolder.transform;

        float y = (int)((playerControl.PlayerClass.items.Count-1) / 4) * 0.05f;
        float x = (int)((playerControl.PlayerClass.items.Count-1) % 4) * 0.25f;
        go.GetComponent<UIButton>().Text = newItem.Description;
        go.GetComponent<UIButton>().RelativePosition.x = x;
        go.GetComponent<UIButton>().RelativePosition.y = y;

        UIItemHolder.GetComponent<UIRect>().AddChild(go.GetComponent<UIButton>());
        go.GetComponent<UIButton>().UpdateChildren();
    }

    public bool showMenu = false;

    void Update()
    {
        UpdateUI();

        currentExp = Mathf.Lerp(currentExp, wantedExp, Time.deltaTime * speed);
        currentMoney = Mathf.Lerp(currentMoney, wantedMoney, Time.deltaTime * speed);
        currentHealth = Mathf.Lerp(currentHealth, wantedHealth, Time.deltaTime * speed);

        ChangeUI();

        if (InputController.GetClicked(playerControl.PlayerID() + "_SKILLMENU"))
        {
            showMenu = !showMenu;
            SkillAndItemMenu.GetComponent<UIRect>().Visible = showMenu;
            if (showMenu)
            {
                GameEventHandler.TriggerOnPause();
                SkillAndItemMenu.ActivateTab();
            }
            else
            {
                GameEventHandler.TriggerOnResume();
            }
        }
    }

    private void ChangeUI()
    {
        level.Text = playerControl.Level.ToString("##0");
        HealthBar.RelativeSize.x = currentHealth / currentMaxHealth;

        HealthText.Text = currentHealth.ToString("###0") + "/" + currentMaxHealth.ToString("###0");

        Money.Text = "Money:" + currentMoney.ToString("#####0");

        Exp.Text = "Experience:" + (currentExp * 100).ToString("##0") + "%";
        ExpBar.RelativeSize.x = currentExp;

        skillCD1.Text = playerControl.PlayerClass.playerSkills[0].Cooldown.ToString("#0.0");
        skillCD2.Text = playerControl.PlayerClass.playerSkills[1].Cooldown.ToString("#0.0");
        skillCD3.Text = playerControl.PlayerClass.playerSkills[2].Cooldown.ToString("#0.0");
        skillCD4.Text = playerControl.PlayerClass.playerSkills[3].Cooldown.ToString("#0.0");

        CheckPointText.Text = "Checkpoint: " + playerControl.ProcentageCheckpointTimer.ToString("##0%");

        Button25.Text = "Buy Item for " + MoneyPerItem25.ToString();
        Button50.Text = "Buy Item for " + MoneyPerItem50.ToString();
        Button75.Text = "Buy Item for " + MoneyPerItem75.ToString();
        Button100.Text = "Buy Item for " + MoneyPerItem100.ToString();

        Button25.Enabled = false;
        Button50.Enabled = false;
        Button75.Enabled = false;
        Button100.Enabled = false;

        if (playerControl.Money >= MoneyPerItem25)
            Button25.Enabled = true;
        if (playerControl.Money >= MoneyPerItem50)
            Button50.Enabled = true;
        if (playerControl.Money >= MoneyPerItem75)
            Button75.Enabled = true;
        if (playerControl.Money >= MoneyPerItem100)
            Button100.Enabled = true;
    }

    public void UpdateUI()
    {
        wantedHealth = playerControl.PlayerClass.CurrentHealth;
        currentMaxHealth = playerControl.PlayerClass.GetAttributeValue(AttributeType.HEALTH);

        wantedMoney = playerControl.Money;

        wantedExp = ((playerControl.CurrentExperience - playerControl.PrevNeededExperience) / (playerControl.NeededExperience - playerControl.PrevNeededExperience));
        wantedExp = Mathf.Clamp(wantedExp, 0f, 1f);

        UpdateSkillPoints();

        if (playerControl.PlayerClass.skillPoints > 0)
            skillPanel.Enabled = true;
        else
            skillPanel.Enabled = false;
    }

    public void UpdateSkillPoints()
    {
        SkillTabButton.Text = "Skill/Attribute(" + playerControl.PlayerClass.skillPoints.ToString() + ")";
    }
}

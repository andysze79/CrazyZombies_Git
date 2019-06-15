// Version 0.1

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CounterManager : MonoBehaviour
{
    public static CounterManager Instance = null;

    public bool isDebugActive = false;
    public CounterData Rock;
    public CounterData Paper;
    public CounterData Scissor;

    [Header("UI Images")]
    public Sprite infantrySprite = null;
    public Sprite archerSprite = null;
    public Sprite cavalrySprite = null;
    public Image rockImage = null;
    public Image paperImage = null;
    public Image scissorImage = null;

    [System.Serializable]
    public struct CounterData
    {
        public Troops TroopType;
        [Range(1f, 10f)]
        public float Strength;
        [Range(1f, 10f)]
        public float Weakness;
    }

    public enum Troops { Infantry, Cavalry, Archer }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        ChooseUISprites();
    }

    private void ChooseUISprites()
    {
        if (!rockImage || !paperImage || !scissorImage || !infantrySprite || !archerSprite || !cavalrySprite)
            return;

        if(Rock.TroopType == Troops.Infantry)
        {
            rockImage.sprite = infantrySprite;
        }
        else if(Rock.TroopType == Troops.Archer)
        {
            rockImage.sprite = archerSprite;
        }
        else if (Rock.TroopType == Troops.Cavalry)
        {
            rockImage.sprite = cavalrySprite;
        }

        if (Paper.TroopType == Troops.Infantry)
        {
            paperImage.sprite = infantrySprite;
        }
        else if (Paper.TroopType == Troops.Archer)
        {
            paperImage.sprite = archerSprite;
        }
        else if (Paper.TroopType == Troops.Cavalry)
        {
            paperImage.sprite = cavalrySprite;
        }

        if (Scissor.TroopType == Troops.Infantry)
        {
            scissorImage.sprite = infantrySprite;
        }
        else if (Scissor.TroopType == Troops.Archer)
        {
            scissorImage.sprite = archerSprite;
        }
        else if (Scissor.TroopType == Troops.Cavalry)
        {
            scissorImage.sprite = cavalrySprite;
        }
    }

    public (int, float) GetCounterModifier(AIDataHolder.TroopType _troopType, AIDataHolder _targetAI, float _damage)
    {
        // We multiply the damage with the deliver damage to emphasize the power of troop1 over troop2
        // We divide the damage with the receive damage to emphasize the weakness of troop1 over troop3

        if (Rock.TroopType == (Troops)_troopType)
        {
            if (Paper.TroopType == (Troops)_targetAI.troopType)
            {
                if (isDebugActive) Debug.Log("STRONG vs WEAK: " + _troopType + " dealt " + _damage + " * " + Rock.Strength + " (" + _damage * Rock.Strength + ") " + " to " + _targetAI.troopType);
                return (0, Rock.Strength);
            }
            else if (Scissor.TroopType == (Troops)_targetAI.troopType)
            {
                if (isDebugActive) Debug.Log("WEAK vs STRONG: " + _troopType + " dealt " + _damage + " / " + Scissor.Weakness + " (" + _damage / Scissor.Weakness + ") " + " to " + _targetAI.troopType);
                return (1, Scissor.Weakness);
            }
            else
            {
                if (isDebugActive) Debug.Log("NO COUNTER: " + _troopType + " dealt " + _damage + " to " + _targetAI.troopType);
                return (2, 1);
            }
        }
        else if (Paper.TroopType == (Troops)_troopType)
        {
            if (Scissor.TroopType == (Troops)_targetAI.troopType)
            {
                if (isDebugActive) Debug.Log("STRONG vs WEAK: " + _troopType + " dealt " + _damage + " * " + Paper.Strength + " (" + _damage * Paper.Strength + ") " + " to " + _targetAI.troopType);
                return (0, Paper.Strength);
            }
            else if (Rock.TroopType == (Troops)_targetAI.troopType)
            {
                if (isDebugActive) Debug.Log("WEAK vs STRONG: " + _troopType + " dealt " + _damage + " / " + Rock.Weakness + " (" + _damage / Rock.Weakness + ") " + " to " + _targetAI.troopType);
                return (1, Rock.Weakness);
            }
            else
            {
                if (Instance.isDebugActive) Debug.Log("NO COUNTER: " + _troopType + " dealt " + _damage + " to " + _targetAI.troopType);
                return (2, 1);
            }
        }
        else if (Instance.Scissor.TroopType == (Troops)_troopType)
        {
            if (Instance.Rock.TroopType == (Troops)_targetAI.troopType)
            {
                if (isDebugActive) Debug.Log("STRONG vs WEAK: " + _troopType + " dealt " + _damage + " * " + Scissor.Strength + " (" + _damage * Scissor.Strength + ") " + " to " + _targetAI.troopType);
                return (0, Scissor.Strength);
            }
            else if (Paper.TroopType == (Troops)_targetAI.troopType)
            {
                if (isDebugActive) Debug.Log("WEAK vs STRONG: " + _troopType + " dealt " + _damage + " / " + Paper.Weakness + " (" + _damage / Paper.Weakness + ") " + " to " + _targetAI.troopType);
                return (1, Paper.Weakness);
            }
            else
            {
                if (isDebugActive) Debug.Log("NO COUNTER: " + _troopType + " dealt " + _damage + " to " + _targetAI.troopType);
                return (2, 1);
            }
        }
        else
        {
            if (isDebugActive) Debug.Log("This Shouldn't Have Happened.. Check Your Troop Settings!");
            return (2, 1);
        }
    }
}

using System.Collections;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour {
    public GameObject Menu, Difficulty, Stats;
    public OrbManager OrbManager;
    public TMP_Text NextOrbType;

    void Start() {
        Menu.SetActive(true);
        Difficulty.SetActive(false);
        Stats.SetActive(false);
    }

    void Update() {
        NextOrbType.text = $"Score: {Mathf.Floor(OrbManager.Score * 100) / 100}\nNext Orb: {(OrbManager.NextBomb ? "Bomb" : OrbManager.NextOrbName)}";
    }

    public void MenuSetActive(bool active) => Menu.SetActive(active);
    public void DifficultySetActive(bool active) => Difficulty.SetActive(active);
    public void StatsSetActive(bool active) => Stats.SetActive(active);

    public void ShowCustomiseTooltip(GameObject text) {
        text.SetActive(true);
    }

    public void StartGame(string difficulty) {
        Debug.Log("Started game with difficulty " + difficulty);
        
        switch (difficulty) {
            case "Easy":
                OrbManager.ChainLength = 4;
                OrbManager.BombChance = 0.1f;
                OrbManager.GenerateField(16, 6);
                break;
            case "Normal":
                OrbManager.ChainLength = 5;
                OrbManager.BombChance = 0.075f;
                OrbManager.GenerateField(12, 8);
                break;
            case "Hard":
                OrbManager.ChainLength = 6;
                OrbManager.BombChance = 0.05f;
                OrbManager.GenerateField(8, 10);
                break;
        }
    } 
}

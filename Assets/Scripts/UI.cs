using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    public GameObject Menu, Difficulty, Stats, Settings;
    public OrbManager OrbManager;
    public TMP_Text NextOrbType;
    public TMP_Text Timer;
    public Slider SfxSlider;

    void Start() {
        Menu.SetActive(true);
        Difficulty.SetActive(false);
        Stats.SetActive(false);
        Settings.SetActive(false);
    }

    void Update() {
        int ms = Mathf.FloorToInt((OrbManager.Timer - Mathf.Floor(OrbManager.Timer)) * 1000);
        NextOrbType.text = $"Score: {Mathf.Floor(OrbManager.Score * 100) / 100}\nNext Orb: {(OrbManager.NextBomb ? "Bomb" : OrbManager.NextOrbName)} - Bomb Chance: {Mathf.Round(OrbManager.BombChance*100)}%";
        Timer.text = $"Time Left: {Mathf.FloorToInt(OrbManager.Timer)}:{ms:D3}";
    }

    public void MenuSetActive(bool active) => Menu.SetActive(active);
    public void DifficultySetActive(bool active) => Difficulty.SetActive(active);
    public void StatsSetActive(bool active) => Stats.SetActive(active);
    public void SettingsSetActive(bool active) => Settings.SetActive(active);

    public void ShowCustomiseTooltip(GameObject text) {
        text.SetActive(true);
    }

    public void StartGame(string difficulty) {
        OrbManager.GameStarted = true;
        
        switch (difficulty) {
            case "Easy":
                OrbManager.ChainLength = 4;
                OrbManager.BombChance = 0.04f;
                OrbManager.Timer = 90f;
                OrbManager.GenerateField(16, 4);
                break;
            case "Normal":
                OrbManager.ChainLength = 5;
                OrbManager.BombChance = 0.02f;
                OrbManager.Timer = 75f;
                OrbManager.GenerateField(12, 5);
                break;
            case "Hard":
                OrbManager.ChainLength = 6;
                OrbManager.BombChance = 0.01f;
                OrbManager.Timer = 60f;
                OrbManager.GenerateField(8, 6);
                break;
        }
    }
    
    public void SfxSliderValueChange() {
        float volumeToSet = SfxSlider.value / 100;

        OrbManager.PopSound.volume = volumeToSet;
        OrbManager.PopSoundSpecial.volume = volumeToSet;
        OrbManager.ExplosionSound.volume = volumeToSet;
    }
}

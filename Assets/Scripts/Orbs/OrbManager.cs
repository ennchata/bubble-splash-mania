using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class OrbManager : MonoBehaviour {
    public List<GameObject> OrbTemplates;
    public Camera Camera;
    public List<Orb> CurrentField;
    public int NextOrbIndex;
    public string NextOrbName {
        get { return OrbTemplates[NextOrbIndex].GetComponent<Renderer>().sharedMaterial.name; }
    }
    public bool NextBomb = false;
    public float BombChance = 0.1f;
    public float Score = 0f;
    public int ChainLength = 4;
    public int MaxHeightForLose = 11;
    public float Timer = 0f;
    public AudioSource PopSound, ExplosionSound, PopSoundSpecial;
    public ParticleSystem Particles;
    public GameObject WinScreen;
    public TMP_Text ScoreText;
    public GameObject LoseScreen;
    public bool GameStarted = false;

    void Start() {
        if (OrbTemplates.Count == 0) throw new Exception("No orb templates have been provided.");
        if (Camera == null) throw new Exception("Camera was not provided.");

        NextOrbIndex = Random.Range(0, OrbTemplates.Count);
    }

    private void Update() {
        Timer = Mathf.Max(0f, Timer - Time.deltaTime);
        if (Timer == 0 && GameStarted) ShowWinScreen();
    }

    public List<Orb> GenerateField(int width, int height) {
        List<Orb> field = new List<Orb>();

        List<Orb> previous = null;
        for (int i = 0; i < height; i++) {
            List<Orb> current = GenerateRow(width, i);
            field.AddRange(current);

            if (previous != null) {
                for (int j = 0; j < width; j++) {
                    current[j].Bottom = previous[j];
                    previous[j].Top = current[j];
                }
            }
            previous = current;
        }

        CurrentField = field;
        return field;
    }

    public List<Orb> GenerateRow(int size, float y) {
        List<Orb> row = new List<Orb>();

        Orb previous = null;
        for (float i = -size/2; i < size/2; i += 1) {
            Orb current = GenerateOrbRandom(new Vector3(i, y));
            row.Add(current);

            if (previous != null) {
                current.Left = previous;
                previous.Right = current;
            }
            previous = current;
        }

        return row;
    }

    public Orb GenerateOrb(Vector3 position) => GenerateOrb(OrbTemplates[NextOrbIndex], position);
    public Orb GenerateOrbRandom(Vector3 position) => GenerateOrb(OrbTemplates[Random.Range(0, OrbTemplates.Count)], position);

    public Orb GenerateOrb(GameObject template, Vector3 position) {
        GameObject clone = Instantiate(template, transform);
        Orb component = clone.AddComponent<Orb>();

        clone.transform.SetLocalPositionAndRotation(position, Quaternion.identity);
        component.OrbType = GetMaterialName(clone.GetComponent<Renderer>().material.name);
        component.Particle = Particles;
        component.Manager = this;

        return component;
    }

    private string GetMaterialName(string input) {
        Match match = Regex.Match(input, @"\b(\w+)\s+\(\w+\)");

        if (!match.Success) throw new Exception("Failed extracting material name. " + input);
        return match.Groups[1].Value;
    }

    public void CycleOrbPick() {
        if (Random.Range(0f, 1f) <= BombChance) {
            NextBomb = true;
            return;
        }

        NextOrbIndex = Random.Range(0, OrbTemplates.Count);
    }

    public void ShowWinScreen() {
        WinScreen.SetActive(true);
        ScoreText.text = $"Your Score: {Mathf.Floor(Score * 100) / 100}";

        foreach (Orb orb in CurrentField) Destroy(orb.gameObject);
        CurrentField.Clear();
    }

    public void ShowLoseScreen() {
        LoseScreen.SetActive(true);

        foreach (Orb orb in CurrentField) Destroy(orb.gameObject);
        CurrentField.Clear();
    }
}

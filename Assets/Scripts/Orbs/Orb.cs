using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Orb : MonoBehaviour {
    public OrbManager Manager;
    public Orb Top, Bottom, Left, Right = null;
    public string OrbType;

    private void OnMouseUp() {
        if (Manager.NextBomb) {
            int count = 0;

            Manager.NextBomb = false;
            if (Top != null) {
                Top.GracefulDestroy();
                count++;
            };
            if (Bottom != null) {
                Bottom.GracefulDestroy();
                count++;
            };
            if (Left != null) {
                Left.GracefulDestroy();
                count++;
            };
            if (Right != null) {
                Right.GracefulDestroy();
                count++;
            };
            GracefulDestroy();

            BoostScoreExp(count);
            BoostScore(1);
            Manager.ExplosionSound.Play();
            return;
        }

        if (Top != null) return;

        List<Orb> orbs = this.FindAdjacentSpecifiedType(Manager.NextOrbName);
        if (orbs.Count < Manager.ChainLength) {
            Orb newOrb = Manager.GenerateOrb(transform.localPosition + Vector3.up);
            Manager.CurrentField.Add(newOrb);
            newOrb.BindConnections();

            List<Orb> same = newOrb.FindAdjacentSameType();
            if (same.Count >= Manager.ChainLength) {
                foreach (Orb orb in same) orb.GracefulDestroy();
                BoostScoreExp(same.Count);
            }
        } else {
            foreach (Orb orb in orbs) orb.GracefulDestroy();
            BoostScoreExp(orbs.Count);
        }

        BoostScore(1);
        Manager.PopSound.Play();
        Manager.CycleOrbPick();
    }

    public void GracefulDestroy() {
        this.UnbindConnections();
        Manager.CurrentField.Remove(this);
        Destroy(gameObject);
    }

    private void BoostScore(int amount) => Manager.Score += amount;
    private void BoostScoreExp(int amount) => Manager.Score += Mathf.Pow(10f + LogBoost(), 1f + amount / 10f) - 1;
    private float LogBoost() => Manager.Score == 0 ? 0 : Mathf.Log10(Manager.Score);


}

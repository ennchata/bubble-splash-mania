using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class OrbExtensions {
    public static List<Orb> FindAdjacentSameType(this Orb orb) {
        GameObject detectorObj = new GameObject("TempOrbDetector");
        OrbDetector detector = detectorObj.AddComponent<OrbDetector>();

        List<Orb> results = detector.FindMatchingAdjacent(orb);

        GameObject.Destroy(detectorObj);

        return results;
    }

    public static List<Orb> FindAdjacentSpecifiedType(this Orb orb, string orbType) {
        GameObject detectorObj = new GameObject("TempOrbDetector");
        OrbDetector detector = detectorObj.AddComponent<OrbDetector>();

        List<Orb> results = detector.FindMatchingSpecified(orb, orbType);

        GameObject.Destroy(detectorObj);

        return results;
    }

    public static void BindConnections(this Orb orb) {
        OrbManager manager = orb.Manager;

        Orb top = manager.CurrentField.Find(o => o.transform.localPosition == orb.transform.localPosition + Vector3.up);
        Orb bottom = manager.CurrentField.Find(o => o.transform.localPosition == orb.transform.localPosition + Vector3.down);
        Orb left = manager.CurrentField.Find(o => o.transform.localPosition == orb.transform.localPosition + Vector3.left);
        Orb right = manager.CurrentField.Find(o => o.transform.localPosition == orb.transform.localPosition + Vector3.right);
    
        if (top != null) {
            orb.Top = top;
            top.Bottom = orb;
        }
        if (bottom != null) {
            orb.Bottom = bottom;
            bottom.Top = orb;
        }
        if (left != null) {
            orb.Left = left;
            left.Right = orb;
        }
        if (right != null) {
            orb.Right = right;
            right.Left = orb;
        }
    }

    public static void UnbindConnections(this Orb orb) {
        if (orb.Left != null) orb.Left.Right = null;
        if (orb.Right != null) orb.Right.Left = null;
        if (orb.Top != null) orb.Top.Bottom = null;
        if (orb.Bottom != null) orb.Bottom.Top = null;
    }
}
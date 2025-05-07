using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class OrbDetector : MonoBehaviour {
    private List<Orb> matchingOrbs = new List<Orb>();
    private HashSet<Orb> visitedOrbs = new HashSet<Orb>();

    public List<Orb> FindMatchingSpecified(Orb startingOrb, string targetType) {
        matchingOrbs.Clear();
        visitedOrbs.Clear();

        FindMatchingAdjacentRecursive(startingOrb, targetType);

        return matchingOrbs;
    }


    public List<Orb> FindMatchingAdjacent(Orb startingOrb) => FindMatchingSpecified(startingOrb, startingOrb.OrbType);

    private void FindMatchingAdjacentRecursive(Orb currentOrb, string targetType) {
        if (currentOrb == null || visitedOrbs.Contains(currentOrb))
            return;

        visitedOrbs.Add(currentOrb);

        if (currentOrb.OrbType == targetType) 
            matchingOrbs.Add(currentOrb);
        else return;

        FindMatchingAdjacentRecursive(currentOrb.Top, targetType);
        FindMatchingAdjacentRecursive(currentOrb.Bottom, targetType);
        FindMatchingAdjacentRecursive(currentOrb.Left, targetType);
        FindMatchingAdjacentRecursive(currentOrb.Right, targetType);
    }
}

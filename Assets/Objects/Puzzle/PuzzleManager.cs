using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public enum Mode {
        combination,
        sequence    
    }

    [Header("Config")]
    [Tooltip("Combination -> only checks final state. Sequence -> checks order of states, resets if wrong")]
    public Mode mode;
    [Tooltip("Combination -> '0,2,3,...', where 0,2,3 are interactables (index) turned on and the rest are turned off\nSequence -> '1.1,2.1,1.0,...', where 1,2 are interactables and .0,.1 is the state. Solution must follow the order correctly")]
    public string solution;

    [Header("Objects")]
    [Tooltip("Interactables that are included in the puzzle")]
    public Interactable[] interactables;
    [Tooltip("The targets that will be affected when the puzzle is completed")]
    public Target[] targets;

    private List<int[]> decodedSolution;
    private int step;

    void Start() {
        // decode solution
        if (mode == Mode.combination) {
            decodedSolution = new List<int[]> {solution.Split(',').OrderBy(x=>x).Select(i=>interactables[int.Parse(i)].active?1:0).ToArray()};
        } else if (mode == Mode.sequence) {
            decodedSolution = new List<int[]>();
            
            Tuple<int,int>[] steps = solution.Split(',').Select(x=>new Tuple<int,int>(int.Parse(x.Split('.')[0]),int.Parse(x.Split('.')[1]))).ToArray();
            List<int> cur = Enumerable.Repeat(0, interactables.Length).ToList();
            
            decodedSolution.Add(cur.ToArray());
            foreach (Tuple<int,int> s in steps) {
                cur[s.Item1] = s.Item2;
                decodedSolution.Add(cur.ToArray());
            }
        }
        step = 0;
    }

    void Update() {
        if (mode == Mode.combination) {
            int[] currentState = interactables.Select(x=>x.active?1:0).ToArray();
            if (currentState.SequenceEqual(decodedSolution[0])) {
                foreach (Target t in targets) { t.Activate(); }
                gameObject.SetActive(false);
            }
        } else if (mode == Mode.sequence) {
            int[] cur = interactables.Select(x=>x.active?1:0).ToArray();
            int[] curStep = decodedSolution[step];
            int[] nextStep = decodedSolution[step+1];

            if (cur.SequenceEqual(nextStep)) {
                step += 1;
                if (step == decodedSolution.Count-1) {
                    foreach (Target t in targets) { t.Activate(); }
                    enabled = false;
                }
            } else if (!cur.SequenceEqual(curStep)) {
                step = 0;
                foreach (Interactable i in interactables) {
                    i.ReturnToDefault();
                }
            }
        }
    }
}

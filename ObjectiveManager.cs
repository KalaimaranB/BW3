using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using NaughtyAttributes;

public class ObjectiveManager : MonoBehaviour
{
    public List<Objective> Objectives;

    public GameObject PrimaryGoldStar;
    public GameObject SecondarySilverStar;

    [ReadOnly]
    public List<Objective> primaryObjectives;
    [ReadOnly]
    public List<Objective> secondaryObjectives;

    // Start is called before the first frame update
    private void Start()
    {
        for (int i = 0; i < Objectives.Count; i++)
        {
            Objectives[i].IntializeObjective();
            if (Objectives[i].objectiveLevel == Objective.ObjectiveLevel.Primary)
            {
                primaryObjectives.Add(Objectives[i]);
            }
            else if (Objectives[i].objectiveLevel == Objective.ObjectiveLevel.Secondary)
            {
                secondaryObjectives.Add(Objectives[i]);
            }
        }

        if (Objectives.Count==0)
        {
            Debug.LogWarning("There are no objectives for this scene. Assign some!");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        for (int i = 0; i < Objectives.Count; i++)
        {
            Objectives[i].UpdateObjectiveStatus();
        }
    }

    public Objective.ObjectiveState primaryObjectiveState()
    {
        //If any of the primary objectives are failed, then return failed
        if (primaryObjectives.Any(i=>i.objectiveState == Objective.ObjectiveState.Failed))
        {
            return Objective.ObjectiveState.Failed;
        }
        //If any of the objecives are inprogress, then return InProgress 
        else if (primaryObjectives.Any(i=>i.objectiveState == Objective.ObjectiveState.InProgress))
        {
            return Objective.ObjectiveState.InProgress;
        }
        //If no objectives are failed and no objectives are in progress, then all are complete
        else
        {
            return Objective.ObjectiveState.Complete;
        }
    }

    public Objective.ObjectiveState secondaryObjectiveState()
    {
        //If any of the primary objectives are failed, then return failed
        if (secondaryObjectives.Any(i => i.objectiveState == Objective.ObjectiveState.Failed))
        {
            return Objective.ObjectiveState.Failed;
        }
        //If any of the objecives are inprogress, then return InProgress 
        else if (secondaryObjectives.Any(i => i.objectiveState == Objective.ObjectiveState.InProgress))
        {
            return Objective.ObjectiveState.InProgress;
        }
        //If no objectives are failed and no objectives are in progress, then all are complete
        else
        {
            return Objective.ObjectiveState.Complete;
        }
    }

    [System.Serializable]
    public class Objective
    {
        public string ObjectiveName;
        public enum ObjectiveType { DestroyTargets, ProtectTargets, ReachPosition };
        public enum ObjectiveState { Failed, InProgress, Complete };
        public enum ObjectiveLevel { Primary, Secondary };

        public ObjectiveType objectiveType;

        public ObjectiveState objectiveState = ObjectiveState.InProgress;
        public ObjectiveLevel objectiveLevel;

        public List<GameObject> targets;

        public GameObject reachPosition;
        private ReachPosition rp;

        public void IntializeObjective()
        {
            if (objectiveType == ObjectiveType.ReachPosition)
            {
                if (reachPosition.GetComponent<ReachPosition>() == false)
                {
                    ReachPosition r = reachPosition.AddComponent<ReachPosition>();
                    rp = r;
                    Debug.LogWarning(reachPosition + " was missing Reach Position script so it was added");
                }
                else
                {
                    rp = reachPosition.GetComponent<ReachPosition>();
                }
                if (reachPosition.GetComponent<Collider>() == false)
                {
                    Debug.LogWarning("No collider detected on " + reachPosition + " . Add one!");
                }
            }

            if (objectiveState == ObjectiveState.Failed)
            {
                Debug.Log("Well, it seems that this objective on start was failed. You should check out why.");
            }

        }

        public void UpdateObjectiveStatus()
        {
            switch (objectiveType)
            {
                case ObjectiveType.ReachPosition:
                    ManageReachPositionObjective();
                    break;

                case ObjectiveType.ProtectTargets:
                    ManageProtectTargetsObjective();
                    break;

                case ObjectiveType.DestroyTargets:
                    ManageDestroyTargetsObjective();
                    break;
                default:
                    break;
            }
        }

        private void ManageReachPositionObjective()
        {
            if (rp.reached == false)
            {
                objectiveState = ObjectiveState.InProgress;
            }
            else if (rp.reached == true)
            {
                objectiveState = ObjectiveState.Complete;
            }
        }

        private void ManageProtectTargetsObjective()
        {
            if (targets.Any(i => i) == false)
            {
                objectiveState = ObjectiveState.Failed;
            }
            else if (targets.Any(i => i.activeSelf == false))
            {
                objectiveState = ObjectiveState.Failed;
            }
            else
            {
                objectiveState = ObjectiveState.Complete;
            }
        }

        private void ManageDestroyTargetsObjective()
        {
            if (targets.Any(i => i))
            {
                objectiveState = ObjectiveState.InProgress;
            }
            else if (targets.Any(i => i.activeSelf == false))
            {
                objectiveState = ObjectiveState.InProgress;
            }
            else
            {
                objectiveState = ObjectiveState.Complete;
            }
        }
    }

}
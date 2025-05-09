using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfoSo", menuName = "ScriptableObjects/QuestInfoSO", order = 1)]
public class QuestInfoSO : ScriptableObject
{
    [field: SerializeField] public string id { get; private set; }

    [Header("General")]
    public string displayName;

    [Header("Requirements")]
    public int levelRequirement;
    public QuestInfoSO[] questPrerequisits;

    [Header("Steps")]
    public GameObject[] questStepPrefabs;

    [Header("Rewards")]
    public GameObject[] questReward;
    public int screwReward;

    //ensure the id is always the same as the scriptable object
    private void OnValidate()
    {
#if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif

    }
}

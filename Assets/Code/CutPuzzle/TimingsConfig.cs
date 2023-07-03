
using UnityEngine;

[CreateAssetMenu(menuName = "Modules/CT/TimingsConfig")]
public class TimingsConfig : ScriptableObject
{
        [Range(0f, 20f)]public float FailedAfterCutAllRopesTime = 5f;
        [Range(0f, 20f)]public float FailedAfterPlayerDeadTime = 2f;
        [Range(0f, 20f)]public float CompletedUITime = 2f;
}

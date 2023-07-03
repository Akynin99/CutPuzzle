using System;
using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.Utils;
using Modules.ViewHub;
using UnityEditor;
using UnityEngine;

public class EnemyVisualSwitcher : MonoBehaviour
{
    public EnemyVisual[] EnemyVisuals;
    public Renderer[] AllRenderers;
    public GameObject[] AllGameObjects;
    public Animator Animator;
    public string EnemyTypeKey;

    public void SetVisual(int enemyType)
    {
        if (EnemyVisuals == null || enemyType < 0 || enemyType >= EnemyVisuals.Length)
        {
            Debug.LogError("Can't find enemy visual");
            return;
        }

        EnemyVisual newVisual = EnemyVisuals[enemyType];
        
        foreach (var renderer in AllRenderers)
        {
            renderer.gameObject.SetActive(false);
        }
        
        foreach (var go in AllGameObjects)
        {
            go.SetActive(false);
        }

        foreach (var renderer in newVisual.Renderers)
        {
            renderer.gameObject.SetActive(true);
            renderer.enabled = true;
        }
        
        foreach (var go in newVisual.GameObjects)
        {
            go.SetActive(true);
        }
        
        Animator.SetFloat(EnemyTypeKey, newVisual.EnemyTypeNumber);
    }

}

[Serializable]
public class EnemyVisual
{
    public string Name;
    public float EnemyTypeNumber;
    public Renderer[] Renderers;
    public GameObject[] GameObjects;
}
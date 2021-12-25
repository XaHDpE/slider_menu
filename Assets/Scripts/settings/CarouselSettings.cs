using System;
using System.Collections.Generic;
using states.controllers;
using UnityEngine;

namespace settings
{
    [CreateAssetMenu]
    public class CarouselSettings : ScriptableObject
    {
        // sizes
        [SerializeField] public Vector3 itemIdleSize;
        [SerializeField] public Vector3 itemSelectedSize;
        [SerializeField] public int idleToSelectedRelation;
        
        
        [SerializeField] public Vector3 topForwardVector;
        [SerializeField] public Vector3 selectedPoint;
        [SerializeField] public int itemsTotal;
        [SerializeField] public float angle;
        [SerializeField] public float radius;

        [SerializeField] public List<CubeController> carouselItems;
        [SerializeField] public List<CubeController> carouselItemsFiltered;

        [SerializeField] public float Idle2SelectedChangeTime;
        [SerializeField] public float Selected2IdleChangeTime;
        
        private void Awake()
        {
            Debug.Log($"CarouselSettings initialized. {this}");
        }
    }
}

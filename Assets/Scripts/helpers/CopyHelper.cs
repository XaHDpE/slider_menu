using System;
using System.Reflection;
using UnityEngine;

namespace helpers
{
    public static class CopyHelper
    {
        public static T GetCopyOf<T>(this Component comp, T other) where T : Component
        {
            Type type = comp.GetType();
            if (type != other.GetType()) return null; // type mis-match
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
            PropertyInfo[] pinfos = type.GetProperties(flags);
            foreach (var pinfo in pinfos) {
                if (pinfo.CanWrite) {
                    try {
                        pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                    }
                    catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
                }
            }
            FieldInfo[] finfos = type.GetFields(flags);
            foreach (var finfo in finfos) {
                finfo.SetValue(comp, finfo.GetValue(other));
            }
            return comp as T;
        }     
    
        public static T AddComponent<T>(this GameObject go, T toAdd) where T : Component
        {
            return go.AddComponent<T>().GetCopyOf(toAdd) as T;
        }
    
        public static Component CopyComponent(Component original, GameObject destination)
        {
            var type = original.GetType();
            var copy = destination.AddComponent(type);
            // Copied fields can be restricted with BindingFlags
            var fields = type.GetFields(); 
            foreach (var field in fields)
            {
                field.SetValue(copy, field.GetValue(original));
            }
            return copy;
        }
        
        public static void MoveHierarchyToLayer(Transform tr, int layer)
        {
            foreach (var child in tr.GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = layer;
            }
        }
    
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class Axis
{
    public static readonly Axis X = new Axis(Vector3.right);
    public static readonly Axis Y = new Axis(Vector3.up);
    public static readonly Axis Z = new Axis(Vector3.forward);
    
    public static IEnumerable<Axis> Values
    {
        get
        {
            yield return X;
            yield return Y;
            yield return Z;
        }
    }
    
    public Vector3 Direction  { get; private set; }
    
    Axis(Vector3 direction) => (Direction) = (direction);

    public override string ToString()
    {
        return Direction.ToString();
    }
}

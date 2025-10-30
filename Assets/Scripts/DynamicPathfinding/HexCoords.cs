using UnityEngine;
using UnityEditor;
using System.Linq;

/*
 * 
 * Created by Arija Hartel (@cocoatehcat)
 * HEAVILY references CatLikeCoding
 * Purpose: Coordinate specific stuff for placement
 * 
 */

[System.Serializable]
public struct HexCoords
{
    [SerializeField]
    private int x, z;

    public int X { 
        get
        {
            return x;
        }
    }

    public int Z { 
        get {
            return z;
        } 
    }

    public int Y
    {
        get
        {
            return -X - Z;
        }
    }

    public HexCoords (int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public static HexCoords FromOffsetCoords (int x, int z)
    {
        return new HexCoords (x - z / 2, z);
    }

    public override string ToString()
    {
        return "(" + X.ToString () + " ," + Y.ToString () + ", " + Z.ToString () + ")";
    }

    public string ToStringOnSepLines()
    {
        return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
    }

}

[CustomPropertyDrawer(typeof(HexCoords))]
public class HexCoordDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        HexCoords coords = new HexCoords(
            property.FindPropertyRelative("x").intValue,
            property.FindPropertyRelative("z").intValue
            );

        position = EditorGUI.PrefixLabel(position, label);
        GUI.Label(position, coords.ToString());
    }
}
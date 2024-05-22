using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Inventory", menuName = "My Project/Inventory")]
public class InventorySO : ScriptableObject
{
    public int age = 10;
    public string m_name = "Toto";

    public bool keyObtained = false;

    
    public void Hello()
    {
        Debug.Log( $"Bonjour je suis {m_name} et j'ai {age}ans" );
    }

    public void KeyPickUp()
    {
        keyObtained = true;
    }

    [ContextMenu("Rizet")]
    public void Rizet()
    {
        Debug.Log( "Fak u" );
    }
}

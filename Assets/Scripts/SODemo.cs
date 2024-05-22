using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SODemo : MonoBehaviour
{
    public InventorySO myObject;
    // Start is called before the first frame update
    void Start()
    {
        myObject.Hello();
        myObject.m_name = "Bonjour";
        myObject.age = Random.Range( 0, 9999 );

        if( SceneManager.GetActiveScene().name != "Goodbye" )
        {
            SceneManager.LoadScene( "Goodbye" );
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

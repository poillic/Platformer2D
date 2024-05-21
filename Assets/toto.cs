using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toto : MonoBehaviour
{
    public GameObject hurtbox;
    public PlayerStateMachine player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Test()
    {
        Debug.Log( "C moi" );
    }

    public void Activate()
    {
        hurtbox.SetActive( true );
    }

    public void Deactivate()
    {
        hurtbox.SetActive( false );
        player.attackFinish = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerController P_Control;
    PlayerStat P_Stat;
    WeaponManager W_Manager;


    // Start is called before the first frame update
    void Start()
    {

        P_Stat = GetComponent<PlayerStat>();
        P_Control = GetComponent<PlayerController>();
        W_Manager = GetComponent<WeaponManager>();
      
    }

    // Update is called once per frame
    void Update()
    {
        P_Control.GetMoveInput();
        P_Control.Dodge();
        P_Control.Move();
        P_Control.Turn();
        
        P_Control.Dash();
        P_Control.DamageEffect();

        
    }



}

using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class curlingStone : NetworkBehaviour {
    [SyncVar]
    public GameObject owner;

    // Start is called before the first frame update

    void Start() {
        if (isServer) {
            GetComponent<Rigidbody2D>().simulated = true;
        }
    }

    // Update is called once per frame
    void Update() {
        
    }

    [Command]
    void CmdSetPos() {
        
    }

    [ClientRpc]
    void RpcSetPos() {
        
    }
}

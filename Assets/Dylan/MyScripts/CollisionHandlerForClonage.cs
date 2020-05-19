﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandlerForClonage : MonoBehaviour
{
    public Collider[] hitColliders;
    public bool isColliding = false;

    public float minDistance = 1.0f;
    public float maxDistance = 4.0f;
    public float smooth = 10.0f;
    Vector3 dollyDir;
    public Vector3 dollyDirAdjusted;
    public float distance;
    public float lerp;


    private void Awake()
    {
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    private void Update()
    {
        if (GameManager.s_Singleton.gameState == GameState.PlayMode)
        {
            #region Square/X
            if (Player.s_Singleton.isUsingASpell && (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_Square") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_X")))
            {
                Debug.Log("Square pressed");
                if (/*PlayerSpellsInventory.s_Singleton.spellCompartmentIsActive &&*/ !isColliding)
                {
                    ClonePlayerCharacter();
                    return;
                }
                else
                    PlayerSpellsInventory.s_Singleton.CantUseASpell();

            }
            #endregion

            Vector3 desiredObjPosition = transform.parent.TransformPoint(dollyDir * maxDistance);
            RaycastHit hit;

            if (Physics.Linecast(transform.parent.position, /*Player.s_Singleton.posToInstantiateTheClone.position*/desiredObjPosition, out hit))
            {
                //transform.position = Vector3.Lerp(transform.parent.GetComponent<Collider>().ClosestPointOnBounds(transform.parent.localPosition), hit.collider.ClosestPointOnBounds(hit.collider.transform.localPosition), Time.deltaTime* smooth);
                distance = Mathf.Clamp((hit.distance * lerp), minDistance, maxDistance);
            }
            else
            {
                distance = maxDistance;
                //transform.position = Player.s_Singleton.posToInstantiateTheClone.position;
            }

            transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);

            CollisionCheck();
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.black;
        //Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

    void CollisionCheck()
    {
        hitColliders = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity);
        int i = 0;
        Renderer playerCharacterRenderer = transform.GetChild(0).GetComponent<Renderer>();

        if (i < hitColliders.Length)
        {
            i++;
            isColliding = true;

            if (transform.GetChild(0).gameObject.activeInHierarchy)
                playerCharacterRenderer.material.color = new Color(255, 0, 0, 100);
        }
        else
        {
            isColliding = false;

            if (transform.GetChild(0).gameObject.activeInHierarchy)
                playerCharacterRenderer.material.color = new Color(255, 255, 255, 100);
        }
    }

    void ClonePlayerCharacter()
    {
        PlayerSpellsInventory.s_Singleton.spellsCompartments[0].MyCompartmentSpell.Clonage(Player.s_Singleton.defaultCharacterModelClone, Player.s_Singleton.posToInstantiateTheClone);
        transform.GetChild(0).gameObject.SetActive(false);
        PlayerSpellsInventory.s_Singleton.UseTheSpellInTheSpellCompartment();
    }
}

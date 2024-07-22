using AssemblyCSharp.Assets.Logic.InputSystem.Components;
using AssemblyCSharp.Assets.Logic.Unit.Abstract;
using AssemblyCSharp.Assets.Logic.Unit.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(IMovement))]
[RequireComponent(typeof(IRotatement))]
public class PlayerController : MonoBehaviour
{
    private IMovement movement;
    private IRotatement rotatement;
    private UnitMonoBehaviour playerUnit;

    public GameObject InventoryObject;

    private bool _isMoving;
    public bool IsMoving { get { return _isMoving; } }
    private float HitSaver = 0;
    private float EvadingSaver = 0;
    public bool IsEvading { get { return EvadingSaver > 0; } }

    void Start()
    {
        movement = GetComponent<IMovement>();
        rotatement = GetComponent<IRotatement>();
        playerUnit = GetComponent<UnitMonoBehaviour>();
    }

    RaycastHit hit;
    void Update()
    {
        Physics.Raycast(Camera.main.ScreenPointToRay(InputComponent.PlayerInput.CursorPosition), out hit, Mathf.Infinity, LayerMask.GetMask("PlayerMouse"));
        if (HitSaver > 0)
            HitSaver -= Time.deltaTime;

        Move();
        Rotate();
        //Evade();
        InventoryControll();

        playerUnit.StatContainer.OnHit += StatContainer_OnHit;
    }

    private void StatContainer_OnHit()
    {
        HitSaver = 0.6f;
    }

    void Move()
    {
        _isMoving = false;
        if (isInventoryOpen)
            return;
        if (InputComponent.PlayerInput["LMB"].Value < 0.5f)
            return;
        _isMoving = true;
        if (HitSaver > 0)
            return;

        Vector3 dir = hit.point - transform.position;
        movement.Move(dir, false);
    }
    void Rotate()
    {
        if (isInventoryOpen)
            return;
        if (HitSaver > 0)
            return;
        if (IsEvading)
            return;
        rotatement.LookAt(hit.point);
    }
    void Evade()
    {
        if (IsEvading)
        {
            EvadingSaver -= Time.deltaTime;
            return;
        }
        if (isInventoryOpen)
            return;
        if (InputComponent.PlayerInput["RMB"].Value < 0.5f)
            return;
        Vector3 dir = hit.point - transform.position;
        movement.Move(dir, true);
        EvadingSaver = 0.6f;
    }
    void InventoryControll()
    {
        if (InputComponent.PlayerInput["Inventory"].Value < 0.5f)
            return;
        InvetorySwitch();
    }

    bool isInventoryOpen = false;
    public bool IsInventoryOpen
    {
        get
        {
            return isInventoryOpen;
        }
    }
    public void InvetorySwitch()
    {
        isInventoryOpen = !isInventoryOpen;
        InventoryObject.SetActive(isInventoryOpen);
        if (InventoryObject.GetComponent<AssemblyCSharp.Assets.Logic.UI.Components.UI_Inventory>())
            InventoryObject.GetComponent<AssemblyCSharp.Assets.Logic.UI.Components.UI_Inventory>().UpdateContent();
        else
            InventoryObject.GetComponentsInChildren<AssemblyCSharp.Assets.Logic.UI.Components.UI_Inventory>()
                .Select(x => { x.UpdateContent(); return x; });
    }
}

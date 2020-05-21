using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    private PhotonView PV;
    public int selectedWeapon = -1; // -1 for no weapon
    public float attackSelectionRate = 2f;
    private float nextSelectionTime = 0f;
    public string currentWeapon;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponentInParent<PhotonView>();
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextSelectionTime)
        {
            int previousSelectedWeapon = selectedWeapon;

            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                if (selectedWeapon >= transform.childCount - 1)
                    selectedWeapon = -1;
                else
                    selectedWeapon++;
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                if (selectedWeapon <= -1)
                    selectedWeapon = transform.childCount - 1;
                else
                    selectedWeapon--;
            }
            
            if (Input.anyKeyDown)
            {
                for (int i = 1; i <= transform.childCount; i++)
                {
                    if (Input.inputString == (i + 48).ToString())   //(i+48) gets the alpha key code (for top keyboard numkeys)
                    {
                        selectedWeapon = i - 2;     // i - 2 since character has no weapon mode
                    }
                }
            }

            if (previousSelectedWeapon != selectedWeapon)
            {
                SelectWeapon();
                nextSelectionTime = Time.time + 1f / attackSelectionRate;
            }
        }
    }

    //To activate or deactivate the weapon
    void SelectWeapon()
    {
        if (selectedWeapon == -1)
            currentWeapon = "Punch";

        int i = 0;
        foreach (Transform weapon in transform)
        {
            Debug.Log(weapon.name);

            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
                currentWeapon = weapon.gameObject.name;
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }

    public string CurrentWeapon
    {
        get { return currentWeapon; }
    }
}

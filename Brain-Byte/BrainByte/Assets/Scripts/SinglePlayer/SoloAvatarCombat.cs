using UnityEngine;

public class SoloAvatarCombat : MonoBehaviour
{
    public Transform rayOrigin;
    public Animator animator;

    // for attack
    public Transform weaponHolder;
    public float attackRange = 0.6f;
    public float attackRate = 1.25f;
    private float nextAttackTime = 0f;
    public LayerMask enemyLayers;
    public int playerDamage;

    public int selectedWeapon = -1; // -1 for no weapon
    public float attackSelectionRate = 2f;
    private float nextSelectionTime = 0f;
    public string currentWeapon;

    void Start()
    {
        rayOrigin = transform;
        animator = GetComponent<Animator>();

        RPC_SelectWeapon();



    }

    void Update()
    {
        // attack
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RPC_Shooting();
                nextAttackTime = Time.time + 1f / attackRate;
            }
            if (Input.GetMouseButtonDown(1))
            {
                RPC_Hit();
                nextAttackTime = Time.time + 1f / attackRate;
            }

        }

        // weapon switch
        if (Time.time >= nextSelectionTime)
        {
            int previousSelectedWeapon = selectedWeapon;

            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                if (selectedWeapon >= weaponHolder.childCount - 1)
                    selectedWeapon = -1;
                else
                    selectedWeapon++;
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                if (selectedWeapon <= -1)
                    selectedWeapon = weaponHolder.childCount - 1;
                else
                    selectedWeapon--;
            }

            if (Input.anyKeyDown)
            {
                for (int i = 1; i <= weaponHolder.childCount; i++)
                {
                    if (Input.inputString == i.ToString())
                    {
                        selectedWeapon = i - 2;     // i - 2 since character has no weapon mode
                    }
                }
            }

            if (previousSelectedWeapon != selectedWeapon)
            {
                RPC_SelectWeapon();
                nextSelectionTime = Time.time + 1f / attackSelectionRate;
            }
        }

    }

    void RPC_Hit()
    {
        // Play attack animation
        animator.SetTrigger(currentWeapon);

        // Detect enemy
        Collider[] hitEnemies = Physics.OverlapSphere(weaponHolder.position, attackRange, enemyLayers);

        // Damage enemy
        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log("Hit an enemy");
            
            // Deduct health
            EnemyCombat enemyState = enemy.gameObject.GetComponent<EnemyCombat>();

            if (enemyState != null)
            {
                enemyState.WasHit(playerDamage);
                Debug.Log("Set new enemy health");
                Debug.Log($"Enemy's health is now: {enemyState.enemyHealth}");
            }
        }

    }

    void OnDrawGizmosSelected()
    {
        if (weaponHolder == null)
            return;

        Gizmos.DrawWireSphere(weaponHolder.position, attackRange);
    }

    //To activate or deactivate the weapon
    void RPC_SelectWeapon()
    {
        if (selectedWeapon == -1)
            currentWeapon = "Punch";

        int i = 0;
        foreach (Transform weapon in weaponHolder)
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

    void RPC_Shooting()
    {
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward), out hit, 1000))
        {
            Debug.DrawRay(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward) * hit.distance, Color.red);

            if (hit.transform.tag == "Enemy")
            {
                Debug.Log($"Shot an enemy");
                EnemyCombat enemyState = hit.transform.gameObject.GetComponent<EnemyCombat>();

                if (enemyState != null)
                {
                    enemyState.WasHit(playerDamage);
                    Debug.Log("Set new health");
                    Debug.Log($"Enemy's local health is now: {enemyState.enemyHealth}");
                }
                else
                {
                    // Display all availble components of enemy
                    Debug.Log("Enemy AvatarSetup is null");

                    Component[] enemyComponents = hit.collider.gameObject.GetComponents<Component>();

                    Debug.Log("Available ennemy components are: ");

                    foreach (Component comp in enemyComponents)
                        Debug.Log("Supposed to be Enemy avatar's comps: " + comp.name + " " + comp);

                }

            }
            else
            {
                Debug.Log($"Hit a random {hit.transform.tag} object");
            }
        }
        else
        {
            Debug.DrawRay(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not hit anyone");
        }

    }
}

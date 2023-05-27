using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public PlayerController playerController;

    public int maxHealt;
    public int maxHunger;
    public int maxStamina;

    public Image Healt;
    public Image Hunger;
    public Image Stamina;

    public float HealInCreasedPerSecond;
    public float HungerInCreasedPerSecond;
    public float StaminaInCreasedPerSecond;

    public Text HealthText;
    public Text HungerText;
    public Text StaminaText;

    public float UpdatedHealth;
    public float UpdatedHunger;
    public float UpdatedStamina;

    // Start is called before the first frame update
    public void Start()
    {
        HealInCreasedPerSecond = 5f;
        HungerInCreasedPerSecond = -1f;
        StaminaInCreasedPerSecond = 1f; 

        maxHealt = 1000;
        maxHunger = 1000;
        maxStamina = 8;
    }

    public void Update()
    {
        UpdatedHealth += HealInCreasedPerSecond * Time.deltaTime;
        Healt.fillAmount = UpdatedHealth / maxHealt;

        UpdatedHunger += HungerInCreasedPerSecond * Time.deltaTime;
        Hunger.fillAmount = UpdatedHunger / maxHunger;

        UpdatedStamina += StaminaInCreasedPerSecond * Time.deltaTime;
        Stamina.fillAmount = UpdatedStamina / maxStamina;

        HealthText.text = (int)UpdatedHealth + " ";
        HungerText.text = (int)UpdatedHunger + " ";
        StaminaText.text = (int)UpdatedStamina + " ";

        if(UpdatedHealth >= maxHealt)
        {
            UpdatedHealth = maxHealt;
        }
        
        if(UpdatedHunger >= maxHunger)
        {
            UpdatedHunger = maxHunger;
        }
        
        if(UpdatedStamina >= maxStamina)
        {
            UpdatedStamina = maxStamina;
        }
        
        Stamina.fillAmount = UpdatedStamina / maxStamina;
        playerController.UpdateMovement();
        
    }
}

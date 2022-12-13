using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemPurchase : MonoBehaviour
{
    [SerializeField] private TMP_Text _costText;
    [SerializeField] private TMP_Text _rewardText;
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCostText(string cost)
    {
        _costText.text = cost;
    }

    public void UpdateRewardText(string reward)
    {
        _rewardText.text = reward;
    }
}

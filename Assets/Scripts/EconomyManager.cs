using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    private GetBalancesResult m_LatestGetBalancesResult;
    int m_ItemsPerFetch = 20;

    [SerializeField] private TMP_Text m_GetBalancesText;
    [SerializeField] private TMP_Text _currencyCoin;
    [SerializeField] private TMP_Text _currencyPearl;
    [SerializeField] private TMP_Text _currencyStar;
    [SerializeField] private TMP_Text _currencyGem;

    [SerializeField] private GameObject _itemPrefabs;
    [SerializeField] private GameObject _itemContainer;

    // Start is called before the first frame update
    void Start()
    {
        FetchBalances();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void FetchBalances()
    {
        if (!IsAuthenticationSignedIn())
        {
            return;
        }

        string outputString = "";

        GetBalancesOptions options = new GetBalancesOptions
        {
            ItemsPerFetch = m_ItemsPerFetch
        };
        GetBalancesResult result = await EconomyService.Instance.PlayerBalances.GetBalancesAsync(options);

        m_LatestGetBalancesResult = result;

        if (result.Balances.Count == 0)
        {
            m_GetBalancesText.text = "No balances";
        }
        else
        {
            foreach (var balance in result.Balances)
            {
                CurrencyDefinition currency = await EconomyService.Instance.Configuration.GetCurrencyAsync(balance.CurrencyId);
                if (currency != null)
                {
                    var maxBalance = currency.Max > 0 ? currency.Max.ToString() : "Unlimited";
                    outputString += $"{currency.Id}: {balance.Balance} / {maxBalance}\n";
                }
                else
                {
                    outputString += $"{balance.CurrencyId}: {balance.Balance} (no longer exists in config)\n";
                }

                switch (balance.CurrencyId)
                {
                    case "COIN":
                        _currencyCoin.text = $"{balance.CurrencyId}: {balance.Balance}";
                        break;
                    case "PEARL":
                        _currencyPearl.text = $"{balance.CurrencyId}: {balance.Balance}";
                        break;
                    case "STAR":
                        _currencyStar.text = $"{balance.CurrencyId}: {balance.Balance}";
                        break;
                    case "GEM":
                        _currencyGem.text = $"{balance.CurrencyId}: {balance.Balance}";
                        break;

                    default:
                        break;
                }

                //ClearOutputTextBoxes();
                m_GetBalancesText.text = outputString;
            }
        }
    }

    private TMP_Text textText;
    ///*
    public async void FetchPurchases()
    {
        if (!IsAuthenticationSignedIn())
        {
            return;
        }

        string outputString = "";
        //foreach (var item in await EconomyService.Instance.Configuration.GetVirtualPurchasesAsync())
        //{
        //    Debug.Log("Item --->>>");
        //    outputString += $"{FormatPurchase(item)}\n";
        //}

        foreach (var item in await EconomyService.Instance.Configuration.GetVirtualPurchasesAsync())
        {
            outputString += $"{FormatPurchase(item)}\n";

            GameObject itemm = Instantiate(_itemPrefabs, transform.position, Quaternion.identity);
            TMP_Text text = itemm.GetComponentInChildren<TMP_Text>();
            text.text = $"{outputString}";
            itemm.transform.parent = _itemContainer.transform;

            outputString = "";

            //foreach (var cost in item.Costs)
            //{
            //    Debug.Log($"Item Spawn {cost.Amount}");

            //    GameObject itemm = Instantiate(_itemPrefabs, transform.position, Quaternion.identity);
            //    TMP_Text[] text = itemm.GetComponentsInChildren<TMP_Text>();
            //    text[0].text = $"{cost.Amount}";
            //    text[1].text = $"asdf";

            //    itemm.transform.parent = _itemContainer.transform;

            //}

            //foreach (var reward in item.Rewards)
            //{
            //    GameObject itemm = Instantiate(_itemPrefabs, transform.position, Quaternion.identity);
            //    TMP_Text[] text = itemm.GetComponentsInChildren<TMP_Text>();
            //    text[1].text = $"{reward.Amount}";
            //    itemm.transform.parent = _itemContainer.transform;

            //}
        }


        //m_MakePurchaseText.text = "";
        //m_GetConfigsText.text = outputString;
    }

    static string FormatPurchase(VirtualPurchaseDefinition purchase)
    {
        string outputString = "";
        outputString += "\n**Costs:**\n";
        foreach (var cost in purchase.Costs)
        {
            outputString += $"- {cost.Amount} {cost.Item.GetReferencedConfigurationItem().Name}\n";
        }

        outputString += "\n**Rewards:**\n";
        foreach (var reward in purchase.Rewards)
        {
            outputString += $"- {reward.Amount} {reward.Item.GetReferencedConfigurationItem().Name}\n";
        }
        return outputString;
    }
    //*/

    static bool IsAuthenticationSignedIn()
    {
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            Debug.Log("Wait until sign in is done");
            return false;
        }

        return true;
    }

    //Buttons
    public void OnClickBackBtn()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}

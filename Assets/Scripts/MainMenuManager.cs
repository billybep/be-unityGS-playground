using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerNameText;
    [SerializeField] private TMP_Text _desctiptionText;

    [SerializeField] private string _playerName = "Guest";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        string playerName = PlayerPrefs.GetString("playerName");
        if (playerName == "Guest")
        {
            _playerNameText.text = "Welcome: Guest-" + PlayerPrefs.GetString("playerID");
            _desctiptionText.text = "Please Bind your account to save your data!!!";
        }
        else
        {
            _desctiptionText.text = "null";
        }
    }
}

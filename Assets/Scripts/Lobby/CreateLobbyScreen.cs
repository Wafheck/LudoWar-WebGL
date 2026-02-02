using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.Netcode.Components;

public class CreateLobbyScreen : MonoBehaviour {
    [SerializeField] private TMP_Dropdown _noOfPlayers, _typeDropdown, _avatarDropdown,_playerColor,_coins;
    [SerializeField]
    private TMP_Text  statusTxt,title;
    [SerializeField]
    private Button play, close;
    [SerializeField] private Image avatarImage;

    public TMP_Dropdown Name1O, Name2O, Name3O;
    private Vector3 playerPoint;
    private AuthenticationManager manager;
    [SerializeField] private NetworkManager nManager;

    private void Awake()
    {
        manager = AuthenticationManager.Instance;
#if UNITY_WEBGL
        // WebGL only supports offline modes, don't activate NetworkManager
        Debug.Log("WebGL: Disabling NetworkManager - only offline modes supported");
        if (nManager != null)
            nManager.gameObject.SetActive(false);
#else
        if (manager.GameMode == 1 || manager.GameMode == 4)
        {
            nManager.gameObject.SetActive(true);
        }
#endif

    }

    private void Start()
    {
        

        SetOptions(_typeDropdown, Constants.GameTypes,1);
        SetOptions(_avatarDropdown, Constants.Avatars, Constants.Avatars.IndexOf(manager.Avatar));
        SetOptions(_noOfPlayers, Constants.NoOfPlayers,manager.NoOfPlayers);
        SetOptions(_playerColor, Constants.playerColors,manager.PlayerColor);
        SetOptions(_coins, Constants.playerCoins);

        SetOptions(Name1O, Constants.Avatars,1);
        SetOptions(Name2O, Constants.Avatars,2);
        SetOptions(Name3O, Constants.Avatars,3);

        void SetOptions(TMP_Dropdown dropdown, IEnumerable<string> values, int val = default)
        {
            dropdown.options = values.Select(type => new TMP_Dropdown.OptionData { text = type }).ToList();
            if (val != null)
                dropdown.value = val;
        }

        if (manager.GameMode == 1)
        {
            title.text = "Play Online";
        }
        else if (manager.GameMode == 2)
        {
            title.text = "Play With Computer";
        }
        else if (manager.GameMode == 3)
        {
            title.text = "Pass & Play";
        }
        else if (manager.GameMode == 4)
        {
            title.text = "Play With Friends";
        }



        _avatarDropdown.onValueChanged.AddListener(OnAvatarChange);
        _playerColor.onValueChanged.AddListener(OnColorChange);
        _noOfPlayers.onValueChanged.AddListener(OnNoOfPlayerChange);



        if (player != null)
        {
            Destroy(player.GetComponent<NetworkPlayer>());
            Destroy(player.GetComponent<NetworkObject>());
            Destroy(player.GetComponent<NetworkTransform>());

            playerPoint = player.transform.position;
        }


        if (manager.GameMode != 3)
        {
                Name1O.transform.parent.gameObject.SetActive(false);
                Name2O.transform.parent.gameObject.SetActive(false);
                Name3O.transform.parent.gameObject.SetActive(false);
        }

        OnColorChange(0);
        OnNoOfPlayerChange(2);
        OnAvatarChange(Constants.Avatars.IndexOf(manager.Avatar));


    }

   

    public void OnUpdateStatus(String sts)
    {
        statusTxt.text = sts;
    }

    public GameObject player = null;
    public void OnAvatarChange(int i)
    {
        Utils.Log("OnAvatarChange", "New Value = "+i) ;
      
        if (player != null)
            Destroy(player);

        player = Instantiate(Resources.Load("Prefabs2/" + Constants.Avatars[i]) as GameObject, playerPoint, Quaternion.Euler(0f, 140f, 0f));
       // player.transform.localScale = new Vector3(3, 3, 3);
        player.GetComponent<Animator>().Play("Attack", -1, 1000f);
        player.GetComponent<Animator>().Play("Idle", -1, 1000f);
        player.SetActive(true);
        
        OnColorChange(_playerColor.value);
    }

    public void OnColorChange(int i)
    {
        Utils.Log("OnColorChange", "New Value = " + i);
        ChangeSwordColor chc = player.GetComponentInChildren<ChangeSwordColor>();
        //if (chc != null)
        //{
            chc.ChangeSword(i);
        //}
    }

   
    public void OnNoOfPlayerChange(int i)
    {

        if (manager.GameMode == 3)
        {
           
            if (i == 0)
            {
                Name1O.transform.parent.gameObject.SetActive(true);
                Name2O.transform.parent.gameObject.SetActive(false);
                Name3O.transform.parent.gameObject.SetActive(false);
               
                
            }
            else if (i == 1)
            {
                Name1O.transform.parent.gameObject.SetActive(true);
                Name2O.transform.parent.gameObject.SetActive(true);
                Name3O.transform.parent.gameObject.SetActive(false);
               
            }
            else if (i == 2)
            {
                Name1O.transform.parent.gameObject.SetActive(true);
                Name2O.transform.parent.gameObject.SetActive(true);
                Name3O.transform.parent.gameObject.SetActive(true);
                
            }

            
        }



    }

    public static event Action<LobbyData> LobbyCreated;

    public void OnExitClicked()
    {
        SceneManager.LoadSceneAsync("GameMenu");
    }

    public void OnCreateClicked() {

        //TODO commented for testing
        //if (int.Parse(Constants.playerCoins[_coins.value]) > manager.Coins)
        //{
        //    statusTxt.text = "More coins needed to play ...";
        //    return;
        //}

     
        play.enabled = false;
        close.enabled = false;
       
       
        LobbyData lobbyData = new ();
        lobbyData.Name = manager.UserName;
        lobbyData.MaxPlayers = int.Parse(Constants.NoOfPlayers[_noOfPlayers.value]);
        lobbyData.Avatar = _avatarDropdown.value;
        lobbyData.Type = _typeDropdown.value;
        lobbyData.Mode = manager.GameMode;
        lobbyData.Color = _playerColor.value;
        lobbyData.Coins = _coins.value;// int.Parse(Constants.playerCoins[_coins.value]);
        lobbyData.Human = 1;

       

        if (manager.GameMode == 3)
        {
            lobbyData.Name1 = Name1O.value+"";// != -1 ? Name1O.GetComponent<TMP_InputField>().text : "Player 2";
            lobbyData.Name2 = Name2O.value+"";// != -1 ? Name2O.GetComponent<TMP_InputField>().text : "Player 3";
            lobbyData.Name3 = Name3O.value+"";// != -1 ? Name3O.GetComponent<TMP_InputField>().text : "Player 4";
        }
        else
        {
            lobbyData.Name1 = "Computer 1";
            lobbyData.Name2 = "Computer 2";
            lobbyData.Name3 = "Computer 3";
        }

        Utils.Log("No of Player index: " + _noOfPlayers.value, " Result = " + Constants.NoOfPlayers[_noOfPlayers.value]);

        manager.NoOfPlayers = int.Parse(Constants.NoOfPlayers[_noOfPlayers.value]);
        manager.CoinBet = int.Parse(Constants.playerCoins[_coins.value]);
        manager.PlayerColor = _playerColor.value;
        manager.GameType = _typeDropdown.value;
        manager.SetPlayerData(lobbyData);
        manager.Coins = manager.Coins - lobbyData.Coins;
        manager.SaveData();

        LobbyCreated?.Invoke(lobbyData);
    }


}


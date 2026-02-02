using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;


public class AuthenticationManager : MonoBehaviour {

   public string GUID,UserId;
    public string UserName = "Guest";
    public string Avatar = "Anika";
    public int Coins = 0,Diamonds = 0, Level = 1, NoOfGamesWin, NoOfGamesLost, CoinBet=0,Reward = 0;
    public int PlayerType;
    public int PlayerColor;
    public int GameType;
    public int NoOfPlayers;
    public int PlayerCountry;
    public int PlayerGender;


    public DateTime LoggedInTime = DateTime.Now, LastLoggeInTime;

    public static AuthenticationManager Instance;

    /*
     * 1 = Network Multiplayer
     * 2 = Play with computer
     * 3 = Pass & Play
     * 
     */
    public int GameMode = 0;

    private void Awake()
    {
       
        if (Instance == null) { 
            Instance = this;
            PlayerPrefs.DeleteAll();
            LoadData();
            LoggedInTime = DateTime.Now;

        }
        else
        {
            Utils.Log("Authentication Manager", "Reload = "+GameMode);

        }
    
    }


    public void AddBonus(int bcoins)
    {
        Coins = Coins + bcoins;

        SaveData();

   
    }

    public void AddDia(int dia)
    {
        Diamonds = Diamonds + dia;

        SaveData();


    }

    public bool AddCoins()
    {
        int coin_won = CoinBet * NoOfPlayers;
        Coins = Coins + coin_won;
        NoOfGamesWin++;
        bool levelup = false;
        int l = Math.Abs(NoOfGamesWin / 10);

        if (l > Level)
            levelup = true;

        if (l <= 0)
            Level = 1;
        else
            Level = l;

        SaveData();

        return levelup;
    }

    public void MinusCoins()
    {
        Coins = Coins - CoinBet;
        NoOfGamesLost++;

        SaveData();
    }

    public bool IsCurrentUser(string name)
    {
        if (UserName == name)
            return true;

        return false;
    }

    public void UpdateDiamonds(int inp)
    {
        if(inp<0)
            Diamonds =  Diamonds - inp;
        else
            Diamonds = inp + Diamonds;
        SaveData();
    }

    GameObject alert = null;

    private void Start()
    {
       
    }



    internal void SaveUserSetting(string user, int gender, int country, int color,string avatar,int noOfPlayers)
    {
        UserName = user;

        PlayerPrefs.SetString("User", UserName);
        PlayerPrefs.SetInt("PlayerGender", gender);
        PlayerPrefs.SetInt("PlayerCountry", country);
        PlayerPrefs.SetInt("PlayerColor", color);
        PlayerPrefs.SetInt("NoOfPlayers", noOfPlayers);
        PlayerPrefs.SetString("Avatar", avatar);

    }

    internal void AnimateCoin()
    {
       GameObject source = GameObject.Find("GUI/Canvas/Bottem/Alert/Gold");
       GameObject target = GameObject.Find("GUI/Canvas/TopRight/Panel/Gold/Dark/CoinsTxt");
        Vector3 oldPos = source.transform.position;
        Sequence mySequence = DOTween.Sequence();
        mySequence.Join(source.transform.DOJump(target.transform.position, 2f, 1, 4f));
        mySequence.Join(source.transform.DORotate(new Vector3(12f, 38f, 60f), 1));
        mySequence.Append(source.transform.DOMove(oldPos,0f));
       
        InPlay = false;
    }

  

    public string CurrentScene = "GameMenu";

   
   

    public void ShowCoinCollectionAnim()
    {
        AddBonus(200);
        AnimateCoin();
        GameObject.Find("MenuControl").GetComponent<GameMenuControl>().RefreshUI();
    }

    private void OnEnable()
    {
        LoadData();

    }

    private void OnDisable()
    {
        SaveData();
    }

  

    bool alert_set = false;

   

    public void CloseAlertOnClick()
    {
        alert.SetActive(false);
    }

    public void SaveData()
    {

       
        PlayerPrefs.SetString("User", UserName);
        PlayerPrefs.SetString("UserId", UserId);
        PlayerPrefs.SetString("Avatar", Avatar);
        PlayerPrefs.SetInt("Coins", Coins);
        PlayerPrefs.SetInt("Diamonds", Diamonds);
        PlayerPrefs.SetInt("Level", Level);
        PlayerPrefs.SetInt("NoOfGamesWin", NoOfGamesWin);
        PlayerPrefs.SetInt("NoOfGamesLost", NoOfGamesLost);
       // PlayerPrefs.SetString("LastLogin", LoggedInTime.ToLongDateString());

    }

    

    public void LoadData()
    {

        UserId = PlayerPrefs.GetString("UserId", UserId);
        UserName = PlayerPrefs.GetString("User", UserName);
        Coins = PlayerPrefs.GetInt("Coins", 1000);
        Diamonds = PlayerPrefs.GetInt("Diamonds", 10);
        Level = PlayerPrefs.GetInt("Level", Level);
        NoOfGamesWin = PlayerPrefs.GetInt("NoOfGamesWin", NoOfGamesWin);
        NoOfGamesLost = PlayerPrefs.GetInt("NoOfGamesLost", NoOfGamesLost);
       // LastLoggeInTime = DateTime. PlayerPrefs.GetString("LastLogin", LoggedInTime.ToLongDateString());
        Avatar = PlayerPrefs.GetString("Avatar", Avatar);
        PlayerColor = PlayerPrefs.GetInt("PlayerColor", PlayerColor);
        NoOfPlayers = PlayerPrefs.GetInt("NoOfPlayers", NoOfPlayers);
        PlayerCountry = PlayerPrefs.GetInt("PlayerCountry", PlayerCountry);
        PlayerGender = PlayerPrefs.GetInt("PlayerGender", PlayerGender);

     
    }

   

    LobbyData PData;

    public string WindowName { get; internal set; }

    public void SetPlayerData(LobbyData lobby)
    {
        this.PData = lobby;
    }

    public LobbyData[] PlayerData()
    {
        int playerNo = 0;
        List<LobbyData> dataList = new ();

       
        playerNo = PData.Color;
        PData.Diamonds = Diamonds + 4;
        dataList.Add(PData);

        if ((GameMode == 2||GameMode==3) && PData.MaxPlayers == 2)
        {
           
                int oppositeNo = 0;
                if (playerNo == 0)
                    oppositeNo = 2;
                else if (playerNo == 1)
                    oppositeNo = 3;
                else if (playerNo == 2)
                    oppositeNo = 0;
                else if (playerNo == 3)
                    oppositeNo = 1;

                int avNo = Math.Abs(3 - PData.Avatar);

                LobbyData lobbyData = new();
                lobbyData.Color = oppositeNo;
                lobbyData.Avatar = avNo;
                lobbyData.Type = PData.Type;
                lobbyData.Diamonds = 4;

            if (GameMode == 2)
            {
                lobbyData.Human = 0;
            }
            else
            {
                lobbyData.Human = 1;
            }

            if (PData.Name1.ToString().Contains("Computer"))
            {
                lobbyData.Name = new FixedString64Bytes(Constants.Avatars[avNo]);
                lobbyData.Avatar = avNo;
            }
            else
            {
                lobbyData.Name = new FixedString64Bytes(Constants.Avatars[int.Parse(PData.Name1.ToString())]);
                lobbyData.Avatar = int.Parse(PData.Name1.ToString());
            }

            dataList.Add(lobbyData);

            }
            else if ((GameMode == 2 || GameMode == 3) && PData.MaxPlayers > 2)
            {
                int k = 1;
                List<int> avRef = new List<int>() { 0, 1, 2, 3, 4, 5 };
                List<int> avList = new();
                avList.Add(PData.Avatar);

                for (int i = 0; i < PData.MaxPlayers; i++)
                {
                    if (i != playerNo)
                    {
                        int avNo = avRef.Where(i => !avList.Contains(i)).First<int>();
                        avList.Add(avNo);

                        LobbyData lobbyData = new();
                        lobbyData.Color = i;
                        lobbyData.Avatar = avNo;
                        lobbyData.Human = 0;
                        lobbyData.Diamonds = 4;
                        lobbyData.Type = PData.Type;

                    if (GameMode == 3)
                    {
                        lobbyData.Human = 1;

                        if (k == 1)
                            lobbyData.Avatar = int.Parse(PData.Name1.ToString());
                        else if (k == 2)
                            lobbyData.Avatar = int.Parse(PData.Name2.ToString());
                        else if (k == 3)
                            lobbyData.Avatar = int.Parse(PData.Name3.ToString());
                    }
                    

                        if(k==1)
                            lobbyData.Name = PData.Name1.ToString().Contains("Computer")?new FixedString64Bytes(Constants.Avatars[avNo]): new FixedString64Bytes(Constants.Avatars[int.Parse(PData.Name1.ToString())]);
                        else if(k==2)
                            lobbyData.Name = PData.Name2.ToString().Contains("Computer") ? new FixedString64Bytes(Constants.Avatars[avNo]) : new FixedString64Bytes(Constants.Avatars[int.Parse(PData.Name2.ToString())]);
                        else if (k == 3)
                            lobbyData.Name = PData.Name3.ToString().Contains("Computer") ? new FixedString64Bytes(Constants.Avatars[avNo]) : new FixedString64Bytes(Constants.Avatars[int.Parse(PData.Name3.ToString())]);


                    dataList.Add(lobbyData);
                    k++;
                    }
                   
                }


                
            }


        return dataList.OrderBy(o => o.Color).ToList().ToArray();
    }

    public Boolean ShowAds = true;

    internal void LoadSceneWithAds(string scene)
    {
        ShowAds = true;
       
        SceneManager.LoadScene(scene);

    }

    internal void LoadSceneWithoutAds(string scene)
    {
        ShowAds = false;
        SceneManager.LoadSceneAsync(scene);
    }


    public async void PlayOnline() {

        //  GameObject.Find("Buttons/DiceButton_PO").transform.DOShakeRotation(4f).SetEase(Ease.InBounce).SetLoops(-1, LoopType.Yoyo);

        Utils.Log("3D Button Click", "Play Online");

#if UNITY_WEBGL
        ShowAlert("Not Available", "Online play is not available in browser version. Try 'Play vs Computer' or 'Pass & Play'!", "FailHit");
        return;
#endif

        await Authentication.Login();

        this.GameMode = 1;

        Utils.Log("3D Button Click", "Play Online");

        if(ShowAds)
           GetComponent<ShowInterstitialScript>().ShowInterstitialButtonClicked("Lobby");
        else
           SceneManager.LoadScene("Lobby");


    }

    public async void PlayFriends()
    {

        //     GameObject.Find("Buttons/DiceButton_WF").transform.DOShakeRotation(1f).SetEase(Ease.InBounce).SetLoops(-1, LoopType.Yoyo);

        Utils.Log("3D Button Click", "Play Friends");

#if UNITY_WEBGL
        ShowAlert("Not Available", "Play with friends is not available in browser version. Try 'Play vs Computer' or 'Pass & Play'!", "FailHit");
        return;
#endif

        await Authentication.Login();

        this.GameMode = 4;


        if (ShowAds)
            GetComponent<ShowInterstitialScript>().ShowInterstitialButtonClicked("Lobby");
        else
            SceneManager.LoadScene("Lobby");


    }


    bool InPlay = false;

    public  void PlayOffline()
    {
       
        if (InPlay)
            return;

        InPlay = true;

        Utils.Log("3D Button Click", "Play Offline");

        this.GameMode = 2;
    
        if(ShowAds)
            GetComponent<ShowInterstitialScript>().ShowInterstitialButtonClicked("Lobby");
        else
            SceneManager.LoadScene("Lobby");

      
        
    }

    public void PassAndPlay()
    {
        if (InPlay)
            return;

        InPlay = true;

        Utils.Log("3D Button Click", "Pass & Play");

        this.GameMode = 3;

      if(ShowAds) 
           GetComponent<ShowInterstitialScript>().ShowInterstitialButtonClicked("Lobby");
      else  
           SceneManager.LoadScene("Lobby");
    }

    public void GetMoreCoins()
    {
        if (InPlay)
            return;

        InPlay = true;

        Utils.Log("3D Button Click", "GetMoreCoins");
          GetComponent<ShowRewardedVideoScript>().ShowRewardedVideoButtonClicked();
      
    }



    public void ShowAlert(string title_txt, string message_txt, string audio)
    {

        GameObject obj = GameObject.Find("MenuControl");
        if (obj != null)
        {
            obj.GetComponent<GameMenuControl>().ShowAlert(title_txt,message_txt,audio);
        }

        

    }


    }
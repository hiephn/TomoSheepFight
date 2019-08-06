using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Nethereum.Web3;
using Nethereum.Hex.HexTypes;

public class GameUI : MonoBehaviour, IListener
{
    public Text wScore;
    public Text bScore;
    public Text account;
    public Text balance;

    public Button playButton;
    public GameObject gameOverPanel;
    public GameObject winText;
    public GameObject loseText;

    public GameController controller;
    public SheepContract contract;

    public GameObject mainMenu;

    void Start()
    {
        GameManager.Instance.AddListener(EVENT_TYPE.ACCOUNT_READY, this);
        GameManager.Instance.AddListener(EVENT_TYPE.WHITE_FINISH, this);
        GameManager.Instance.AddListener(EVENT_TYPE.BLACK_FINISH, this);
        GameManager.Instance.AddListener(EVENT_TYPE.GAMEOVER, this);
        playButton.onClick.AddListener(OnPlay);
        gameOverPanel.SetActive(false);
        UpdateScore();
    }

    void UpdateScore()
    {
        wScore.text = "" + GameManager.Instance.wScore;
        bScore.text = "" + GameManager.Instance.bScore;
    }

    public void SetAccount(string address)
    {
        account.text = address;
    }

    public void SetBalance(string balanceText)
    {
        balance.text = balanceText;
    }

    public async void OnPlay()
    {
        mainMenu.SetActive(false);
        string tx = await contract.Play();
        controller.Play(tx);
    }

    public void GameOver(bool isWon)
    {
        gameOverPanel.SetActive(true);
        winText.SetActive(isWon);
        loseText.SetActive(!isWon);
    }

    public void OnEvent(EVENT_TYPE eventType, Component sender, object param = null)
    {
        switch (eventType)
        {
            case EVENT_TYPE.ACCOUNT_READY:
                break;
            case EVENT_TYPE.WHITE_FINISH:
                UpdateScore();
                break;
            case EVENT_TYPE.BLACK_FINISH:
                UpdateScore();
                break;
            case EVENT_TYPE.GAMEOVER:
                bool isWon = (bool)param;
                GameOver(isWon);
                break;
            default:
                break;
        }
    }

}

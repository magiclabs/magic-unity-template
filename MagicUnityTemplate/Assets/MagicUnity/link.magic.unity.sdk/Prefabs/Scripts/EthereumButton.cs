using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using link.magic.unity.sdk;
using link.magic.unity.sdk.Provider;
using link.magic.unity.sdk.Relayer;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Transactions;
using Nethereum.RPC.Personal;
using Nethereum.Util;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EthereumButton : MonoBehaviour
{
    private string _account;
    
    public Text result;
    public Text status;
    public TMP_InputField _inputField;
    public TMP_InputField _outputField;

    public TMP_InputField _transactionTo;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    // Update is called once per frame
    public async void SendTransaction()
    {
        result.text = "";
        var transaction = new EthSendTransaction(Magic.Instance.Provider);
        var transactionInput = new TransactionInput
            { To = _transactionTo.text, Value = new HexBigInteger(UnitConversion.Convert.ToWei(0.01)), From = _inputField.text};
        var hash = await transaction.SendRequestAsync(transactionInput);
        Debug.Log(hash);
        result.text = hash;
    }

    public async void GetBalance()
    {
        var account = _inputField.text;
        var ethSync = new EthSyncing(Magic.Instance.Provider);
        var sync = await ethSync.SendRequestAsync();
        sync.IsSyncing = true;

        var ethBalance = new EthGetBalance(Magic.Instance.Provider);
        var balance = await ethBalance.SendRequestAsync(account, sync.CurrentBlock);
        result.text = balance.Value.ToString();
    }

    public async void GetAccount()
    {
        result.text = "";
        var ethAccounts = new EthAccounts(Magic.Instance.Provider);
        var accounts = await ethAccounts.SendRequestAsync();
        _account = accounts[0];
        _outputField.text = accounts[0];
        Debug.Log(accounts[0]);
        result.text = _account;
    }

    public async void EthSign()
    {
        result.text = "";
        
        var ethAccounts = new EthAccounts(Magic.Instance.Provider);
        var accounts = await ethAccounts.SendRequestAsync();
        var personalSign = new EthSign(Magic.Instance.Provider);
        var transactionInput = new TransactionInput{Data = "Hello world"};
        var res = await personalSign.SendRequestAsync(accounts[0], "hello world");
        result.text = res;
    }
}

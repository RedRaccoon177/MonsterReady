using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[System.Serializable] 
public class PlayerData
{
    public Vector3 playerPos; // �÷��̾� ��ġ
    public int playerGold; // �÷��̾� ���
    public int playergem; // �÷��̾� ����
    public int playerPassLevel; // ��Ʋ �н� ����
    public int playerSpeedLevel; // �̵� �ӵ� ����
    public int playerHoldMaxLevel; // ��� �뷮 ����
    public int playerMakeMoneyLevel; // ���ͷ� ����

    public PlayerData(
        Vector3 _playerPos, int _playerGold, int _playergem, 
        int _playerPassLevel, int _playerSpeedLevel, int _playerHoldMaxLevel, 
        int _playerMakeMoneyLevel) 
    {
        playerPos = _playerPos;
        playerGold = _playerGold;
        playergem = _playergem;
        playerPassLevel = _playerPassLevel;
        playerSpeedLevel = _playerSpeedLevel;
        playerHoldMaxLevel = _playerHoldMaxLevel;
        playerMakeMoneyLevel = _playerMakeMoneyLevel;
    }
     
}

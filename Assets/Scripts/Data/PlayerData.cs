using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[System.Serializable] 
public class PlayerData
{
    public Vector3 playerPos; // 플레이어 위치
    public int playerGold; // 플레이어 골드
    public int playergem; // 플레이어 보석
    public int playerPassLevel; // 배틀 패스 레벨
    public int playerSpeedLevel; // 이동 속도 레벨
    public int playerHoldMaxLevel; // 드는 용량 레벨
    public int playerMakeMoneyLevel; // 수익률 레벨

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

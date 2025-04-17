using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// FloatingJoystick은 기존 Joystick 클래스(아마도 JoystickAsset에서 온 클래스)를 상속받아 조이스틱 UI를 화면 터치 위치에 따라 띄우는 기능을 추가한 클래스
public class FloatingJoystick : Joystick
{
    // Start()는 컴포넌트가 활성화될 때 한 번 호출됨
    // 조이스틱 배경을 비활성화시켜 기본적으로 보이지 않게 설정
    protected override void Start()
    {
        base.Start(); // 부모 클래스의 Start 메서드 호출
        background.gameObject.SetActive(false); // 처음엔 조이스틱 UI를 안 보이게 함
    }

    // 화면을 터치했을 때 호출되는 함수
    public override void OnPointerDown(PointerEventData eventData)
    {
        // 터치한 위치를 기준으로 조이스틱 배경 위치 설정
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);

        // 조이스틱 배경을 보이게 설정
        background.gameObject.SetActive(true);

        // 부모 클래스의 OnPointerDown 호출 → 조이스틱 조작 처리 시작
        base.OnPointerDown(eventData);
    }

    // 터치에서 손을 뗐을 때 호출되는 함수
    public override void OnPointerUp(PointerEventData eventData)
    {
        // 조이스틱 배경을 다시 안 보이게 설정 (손 뗐으니 사라짐)
        background.gameObject.SetActive(false);

        // 부모 클래스의 OnPointerUp 호출 → 조이스틱 조작 종료 처리
        base.OnPointerUp(eventData);
    }
}

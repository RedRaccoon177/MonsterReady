using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPageManager : MonoBehaviour
{
    public GameObject loadingImage1;
    public GameObject loadingImage2;
    public GameObject loadingText;
    public GameObject touchToStartText;

    private bool canTouch = true;

    void Start()
    {
        // 처음에 로딩 관련은 꺼두기
        loadingImage1.SetActive(false);
        loadingImage2.SetActive(false);
        loadingText.SetActive(false);

        // Touch to Start는 처음에 켜두기
        touchToStartText.SetActive(true);
    }

    void Update()
    {
        // 터치 가능할 때 클릭(또는 터치) 감지
        if (canTouch && Input.GetMouseButtonDown(0))
        {
            // 터치 시 Touch to Start 비활성화
            touchToStartText.SetActive(false);

            // 로딩 이미지/텍스트 활성화
            loadingImage1.SetActive(true);
            loadingImage2.SetActive(true);
            loadingText.SetActive(true);

            // 터치 불가능 상태로 변경
            canTouch = false;

            // 로딩 코루틴 시작
            StartCoroutine(LoadingRoutine());
        }
    }

    IEnumerator LoadingRoutine()
    {
        // 10초 동안 로딩 진행
        float timer = 0f;
        while (timer < 5f)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // 로딩 완료 후 게임 씬 이동
        SceneManager.LoadScene("MergeMap");
    }
}

<!-- ===== HEADER ===== -->
<h1 align="center">MonsterReady</h1>
<p align="center">
  Pizza Ready 모작 경영 시뮬레이션 / 하이퍼 캐주얼 모바일 게임
</p>

<br>

<!-- 유튜브/링크 버튼 영역 -->
<p align="center">
  <a href="https://www.youtube.com/shorts/4Ec7ibARAys">
    <img src="https://img.shields.io/badge/Portfolio%20Video-YouTube-red?logo=youtube&logoColor=white" />
  </a>
  <a href="https://www.notion.so/1d68c79bfc3f80059f14cd13f8e2000a">
    <img src="https://img.shields.io/badge/Dev%20Notes-Notion-black?logo=notion&logoColor=white" />
  </a>
</p>

<br>

<!-- 스크린샷 영역 (모바일 세로 비율 기준) -->
<p align="center">
  <img src="https://github.com/user-attachments/assets/0f3d1813-3c84-4c94-b245-bff4a9dd69bb"
       alt="MonsterReady Screenshot 1"
       style="width:30%; height:520px; object-fit:cover; object-position:center;" />
  <img src="https://github.com/user-attachments/assets/f47178fd-0345-4a16-84fc-1ee2be21ae30"
       alt="MonsterReady Screenshot 2"
       style="width:30%; height:520px; object-fit:cover; object-position:center;" />
  <img src="https://github.com/user-attachments/assets/81513741-3bca-4430-987a-168bab86bc4e"
       alt="MonsterReady Screenshot 3"
       style="width:30%; height:520px; object-fit:cover; object-position:center;" />
</p>

<br>

<!-- 핵심 포인트 (내 역할 중심) -->
- ⭐ Pizza Ready 모작 모바일 프로젝트 (개발 2명)
- ⭐ 담당: 손님 NPC, 고기, 골드, 박스, 카운터, 좌석, 화덕, 플레이어 캐릭터 등등
- ⭐ 핵심 구현: 손님 NPC 흐름(이동-주문-좌석-식사-퇴장) + 오브젝트 상호작용 + A* 길찾기

<br>

---

<br>

## 📋 목차
- [🎯 게임 소개](#game-intro)
- [🧾 프로젝트 정보](#project-info)
- [👤 내 역할](#my-role)
- [✅ 내가 구현한 핵심](#what-i-built)
  - [손님 NPC 로직](#customer-ai)
  - [A* 길찾기](#astar)
  - [좌석 할당 및 대기 재시도](#seat-wait)
  - [오브젝트 상호작용](#interaction)
  - [화덕/카운터 루프](#loop)
  - [골드/박스/고기 오브젝트 처리](#objects)
- [🧩 기술 스택](#tech-stack)
- [🗂️ 개발 문서](#docs)
- [👨‍💻 개발자 소개](#developer)

<br>

---

<br>

<a name="game-intro"></a>
## 🎯 게임 소개
MonsterReady는 Pizza Ready를 참고해 제작한 **경영 시뮬레이션 / 하이퍼 캐주얼** 모바일 모작 게임입니다.  
플레이어는 매장 내 오브젝트와 상호작용하며 생산/적재/포장 루프를 진행하고, 손님 주문을 처리해 재화를 획득합니다.
본 README는 포트폴리오 용도로 **제가 구현한 시스템 중심**으로 정리했습니다.

<br>

---

<br>

<a name="project-info"></a>
## 🧾 프로젝트 정보
- 장르: 경영 시뮬레이션, 하이퍼 캐주얼
- 개발 인원: 개발 2명
- 개발 엔진: Unity 3D
- 플랫폼: 모바일

- 1차 제작 기간: 2025.04.16 ~ 2025.04.30 (11일)
- 2차 제작 기간: 2025.07.04 ~ 2025.07.15 (8일)

<br>

---

<br>

<a name="my-role"></a>
## 👤 내 역할
✅ 담당(제가 구현한 범위)
- 손님 NPC, 플레이어 캐릭터
- 고기, 골드, 박스
- 카운터, 좌석, 화덕

<br>

---

<br>

<a name="what-i-built"></a>
## ✅ 내가 구현한 핵심

<a name="customer-ai"></a>
### 1) 손님 NPC 로직
손님은 아래 흐름으로 동작하도록 구성했습니다.

- 이동 -> 주문 -> 좌석 이동 -> 식사 -> 퇴장
- 주문 수량/상태에 따라 카운터 상호작용을 수행
- 좌석이 없을 때는 즉시 종료하지 않고 대기 후 재시도

<br>

<a name="astar"></a>
### 2) A* 길찾기
그리드 기반 노드 맵 위에서 A*로 경로를 탐색합니다.

- 노드 맵 구성(이동 가능/불가능)
- 탐색(Open/Closed)과 최종 경로 추적(parent 역추적)
- 손님 이동 로직에서 동일한 경로 탐색 파이프라인을 재사용

<br>

<a name="seat-wait"></a>
### 3) 좌석 할당 및 대기 재시도
좌석 부족 상황에서도 흐름이 끊기지 않도록 설계했습니다.

- 빈 좌석 탐색
- 없으면 대기 모드로 전환
- 일정 주기로 다시 탐색 후 이동 재개

<br>

<a name="interaction"></a>
### 4) 오브젝트 상호작용
트리거 진입 + 입력 이벤트 기반으로 상호작용을 제어합니다.

- 플레이어가 범위 안에 들어오면 상호작용 가능 상태 활성화
- 상태에 따라 상호작용 실행/차단
- 손님 주문 처리와 연결하여 카운터의 상호작용 흐름을 구성

<br>

<a name="loop"></a>
### 5) 화덕/카운터 루프
매장 핵심 루프를 오브젝트 단위로 연결했습니다.

- 화덕: 생산 흐름(쿨타임/최대 적재량 등)과 시각적 개수 표현
- 카운터: 손님 주문 처리(수량 차감/충족 판정)와 연동

<br>

<a name="objects"></a>
### 6) 골드/박스/고기 오브젝트 처리
반복 생성/표시가 많은 오브젝트들은 안정적으로 관리되도록 구성했습니다.

- 개수 변화에 따라 시각적 스택 표현 동기화
- 골드 획득 및 표시 UI 연동
- 박스/고기 흐름(생성-이동-소비)의 상태 전이 연결

<br>

---

<br>

<a name="tech-stack"></a>
## 🧩 기술 스택
- Unity 3D (2022.3.21f1)
- C#
- A* Pathfinding (그리드 노드 기반)
- 상태 기반 NPC 로직
- 트리거 + 입력 이벤트 기반 상호작용

<br>

---

<br>

<a name="docs"></a>
## 🗂️ 개발 문서
개발 과정에서 사용한 문서(개발 사양서/클래스 다이어그램/작업 규칙/커밋 메시지 규칙 등)는 아래 노션에 정리했습니다.

- Notion: https://www.notion.so/1d68c79bfc3f80059f14cd13f8e2000a

<br>

---

<br>

<a name="developer"></a>
## 👨‍💻 개발자 소개
- GitHub: [https://github.com/RedRaccoon177]
- Tistory: [https://wearelast99.tistory.com/]
- YouTube: [유튜브 채널](https://www.youtube.com/@%EC%9D%B4%EC%9C%A0-z9c)
- Canva 포트폴리오: [포트폴리오](https://www.canva.com/design/DAGusJR6Rj8/BOtICI6F1raShPyHHewjxg/view?utm_content=DAGusJR6Rj8&utm_campaign=designshare&utm_medium=link2&utm_source=uniquelinks&utlId=h691958bd9a)
- Canva 이력서: [이력서](https://www.canva.com/design/DAGj7YKBoc8/YPk_CLe8B1taKTE-nneUJA/view?utm_content=DAGj7YKBoc8&utm_campaign=designshare&utm_medium=link2&utm_source=uniquelinks&utlId=ha914d97458)

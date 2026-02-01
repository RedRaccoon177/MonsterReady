

<!-- ===== HEADER ===== -->
<h1 align="center">MonsterReady</h1>
<p align="center">
  Pizza Ready 모작 하이퍼 캐주얼 3D 타이쿤 게임
</p>

<br>

<!-- 유튜브/링크 버튼 영역 -->
<p align="center">
  <a href="YOUR_VIDEO_LINK">
    <img src="https://img.shields.io/badge/Portfolio%20Video-YouTube-red?logo=youtube&logoColor=white" />
  </a>
  <a href="YOUR_PPT_LINK">
    <img src="https://img.shields.io/badge/PPT-GoogleDrive-blue" />
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
- ⭐ 모바일 게임 Pizza Ready 모작 프로젝트
- ⭐ 손님 NPC 전반(이동-주문-좌석-식사-퇴장)과 오브젝트 상호작용 시스템 구현
- ⭐ 그리드 기반 A* 길찾기(Node 맵, 탐색, 경로 추적)로 손님 이동 처리

<br>

---

<br>

## 📋 목차
- [🎯 게임 소개](#game-intro)
- [🧾 프로젝트 정보](#project-info)
- [👤 내 역할](#my-role)
- [✅ 내가 구현한 핵심](#what-i-built)
  - [손님 NPC AI 상태 전이](#customer-ai)
  - [A* 길찾기 시스템](#astar)
  - [좌석 할당 및 대기 재시도](#seat-wait)
  - [오브젝트 상호작용(트리거 + 입력 이벤트)](#interaction)
  - [오브젝트 풀링 및 스택 표현](#pooling)
- [🧩 기술 스택](#tech-stack)
- [👨‍💻 개발자 소개](#developer)

<br>

---

<br>

<a name="game-intro"></a>
## 🎯 게임 소개
MonsterReady는 Pizza Ready를 참고해 제작한 하이퍼 캐주얼 3D 타이쿤 모작 게임입니다.  
플레이어는 매장 내 오브젝트와 상호작용하며 생산/적재/포장 루프를 진행하고, 손님 주문을 처리해 재화를 획득합니다.

본 README는 게임 홍보용 설명보다, 포트폴리오 용도로 “내가 구현한 시스템(코드)” 중심으로 정리했습니다.

<br>

---

<br>

<a name="project-info"></a>
## 🧾 프로젝트 정보
- 개발 기간: 5일
- 개발 인원: 3명
- 플랫폼: 모바일
- 엔진/언어: Unity, C#

<br>

---

<br>

<a name="my-role"></a>
## 👤 내 역할
제가 맡은 범위는 아래 제외 항목을 뺀 대부분입니다.

✅ 제가 구현한 영역(핵심)
- 손님 NPC 전반: 이동, 주문, 좌석, 식사, 퇴장 흐름
- A* 길찾기: Node 그리드 기반 경로 탐색 및 이동 처리
- 오브젝트 상호작용: 트리거 진입 + 입력 이벤트 기반 상호작용 제어
- 오브젝트 풀링 및 스택 표현: 반복 생성/파괴 비용 최소화 + 시각적 스택 정리

❌ 제가 하지 않은 영역(팀원이 담당)
- 데이터 저장/불러오기 파트
- 종업원 NPC 파트
- 업그레이드(플레이어/종업원) UI 및 로직 일부

<br>

---

<br>

<a name="what-i-built"></a>
## ✅ 내가 구현한 핵심

<a name="customer-ai"></a>
### 1) 손님 NPC AI 상태 전이
손님은 상태 기반으로 동작하도록 구성했습니다. 전체 흐름은 아래와 같습니다.

- 카운터로 이동: `CustomerMoveToCounterState`
  - A* 경로를 따라 이동
  - 도착하면 주문/대기 상태로 전이
- 주문 및 대기: `CustomerOrderAndWait`
  - 주문 수량을 생성하고 UI를 표시
  - 상호작용 가능 상태일 때 주문 수량만큼 수령 처리
  - 주문을 충족하면 좌석 이동으로 전이
- 좌석으로 이동: `CustomerMoveToTable`
  - 빈 좌석을 탐색하고 A*로 이동
  - 좌석이 없으면 일정 주기로 재탐색(대기/재시도)
- 식사 처리: `CustomerEating`
  - 일정 시간 간격으로 소비 로직을 처리하고 종료 조건을 만족하면 퇴장으로 전이
- 퇴장 처리: `CustomerGoingHome`
  - 목표 지점까지 이동 후 제거

🎯 의도
- 주문-이동-좌석-식사-퇴장이 끊기지 않도록 “상태 전이”로 루프를 명확히 고정했습니다.

<br>

<a name="astar"></a>
### 2) A* 길찾기 시스템
그리드 기반 노드 맵 위에서 A*로 경로를 탐색합니다.

- 노드: `Node`
  - 이동 가능/불가능 플래그
  - 8방향 인접 노드 연결
  - 점유 상태(손님이 서 있는 노드) 관리
- 노드 관리: `NodeManager`
  - 노드 맵 생성 및 인접 연결
  - 월드 좌표에서 가장 가까운 노드를 빠르게 찾는 함수 제공
- 경로 탐색: `AStarPathfinder`
  - Open/Closed 리스트 기반 탐색
  - F = G + H 기준으로 최적 후보 선택
  - parent 역추적으로 최종 경로 리스트 구성

🎯 의도
- 손님 이동은 동일한 탐색 파이프라인으로 통일하고, 노드 점유로 겹침/끼임을 제어합니다.

<br>

<a name="seat-wait"></a>
### 3) 좌석 할당 및 대기 재시도
좌석이 없을 때 즉시 실패시키지 않고, “대기 후 재시도”로 설계했습니다.

- 빈 좌석이 없으면 대기 모드로 전환
- 일정 주기(예: 1초)로 다시 좌석을 탐색
- 좌석이 생기면 즉시 경로를 재계산하고 이동 재개

🎯 의도
- 손님이 많은 상황에서도 흐름이 끊기지 않고 자연스럽게 이어지도록 구성했습니다.

<br>

<a name="interaction"></a>
### 4) 오브젝트 상호작용(트리거 + 입력 이벤트)
상호작용은 트리거 기반으로 “플레이어가 범위 안에 있는지”를 판단하고, 입력 이벤트와 결합해 실행합니다.

- 트리거 진입/이탈로 상호작용 가능 여부를 전환
- NPC가 해당 오브젝트를 사용하는 경우 플레이어 입력을 분리해 우선순위를 조정
- 상호작용 가능 상태는 시각 요소(표시 색상 등)로 피드백

✅ 관련 스크립트 예
- `ObjectInteration`
- 트리거 계열: `MeatInputTrigger`, `MeatToBoxTrigger`, `BoxDeliverTrigger` 등

<br>

<a name="pooling"></a>
### 5) 오브젝트 풀링 및 스택 표현
반복 생성/파괴가 많은 오브젝트는 풀링으로 관리했습니다.

- 풀링: `ObjectPooling`
  - 고기/뼈/박스/재화 오브젝트를 큐로 관리
  - 필요 시 꺼내고, 사용 후 반환하여 재사용
- 스택 표현
  - 순번 기반으로 로컬 위치를 계산해 정렬
  - 개수 변경 시 표시 오브젝트를 추가/제거하여 동기화

🎯 의도
- 성능(가비지, Instantiate/Destroy 비용)을 줄이고, 시각적 개수 표현을 안정적으로 유지합니다.

<br>

---

<br>

<a name="tech-stack"></a>
## 🧩 기술 스택
- Unity
- C#
- A* Pathfinding (그리드 노드 기반)
- 상태 기반 AI(손님 NPC)
- 트리거 기반 상호작용
- Object Pooling

<br>

---

<br>

<a name="developer"></a>
## 👨‍💻 개발자 소개
- GitHub: https://github.com/RedRaccoon177


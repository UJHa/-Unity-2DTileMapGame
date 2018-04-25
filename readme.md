-Unity-2DTileMapGame

2018-04-18 todoList 작성 시작
- 장애물 없는 맵 생성
​- playerIdle 시 이동 가능 거리 다른 색으로 표시(0420완료)
- playerIdle에서 이동 가능 타일 클릭 시 이동(0423완료)
- 각 캐릭터 행동에 대한 actionCooltime 적용(0423완료)
- player idle, move state 생성(0423완료)
- 이동 후 (공격, 대기) 선택UI 생성(0424완료)
  - select state 생성(0424완료)
  - collider 컴포넌트 가진 탄의 GameObject 생성(0425완료)
  - 공격, 대기 sprite 리소스 가져오기(0424완료)
- attack state 생성
  - 공격 방법
    - 캐릭터를 시작점으로 지정한 방향으로 날아가는 탄 발사(0425완료)
    - 각 캐릭터 충돌체 생성
    - 충돌 시 탄 사라지고 피격한 캐릭터 체력 감소
    - 충돌체는 타일이 아닌 각 캐릭터에 collider 생성
  - 쿨타임 적용
    - 공격 시 공격 방향 선택하여 공격 후 쿨타임
    - 대기 시 쿨타임(공격보단 짧게)
- selectUI character 앞 layer에 위치하도록 하기(0425완료)


- charator idle 애니메이션 추가
- moveCooltime 이동 보간

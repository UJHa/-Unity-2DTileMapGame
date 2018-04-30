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
    - 카메라 밖으로 탄 나가면 사라지게 하기(0426완료)
    - 각 캐릭터 충돌체 생성(0426완료)
    - 충돌 시 탄 사라지고 피격한 캐릭터 체력 감소(0426완료)
    - 충돌체는 타일이 아닌 각 캐릭터에 collider 생성(0426완료)
  - 쿨타임 적용
    - 공격 시 공격 방향 선택하여 공격 후 쿨타임(0426완료)
    - 대기 시 쿨타임(공격보단 짧게)(0426완료)
- selectUI character 앞 layer에 위치하도록 하기(0425완료)
===================================================
2018-04-26 todoList 작성 시작
- 탄막 발사 방향 UI 추가(0427완료)
- 탄막 충돌 시 damamge 이펙트 띄우기(State없이)
- 체력 0되면 deadState로 변경(0428완료)
- Monster 탄 발사
  - Monster 상태 구성
    - idle : 행동 쿨타임 대기 후 이동 가능 타일 중 랜덤한 타겟 지정하여 moveState로 변경(0428완료)
    - move : 정해진 타겟으로 moveCooltime에 맞춰 이동 / 이동 완료 후 공격(attack으로), 대기(idle로) 중 랜덤하게 상태 변경(0428완료)
    - attack : 360도 탄 발사(0428완료)
    - 몬스터 1마리로 상태 변화 확인할 것(0430완료)
- idle, playerIdle에서 캐릭터 movePossibleTiles 세팅을 1회만 하도록 변경(0429완료)
  - SettingTilePath()함수 기능 분리(0429완료)
    - 현재 이동 가능 타일배열을 세팅하는 기능, 각 tile들의 이전 경로를 저장시키는 기능이 합쳐져 있다.(0429완료)
- playerIdle을 idleState 상속받도록 리펙토링(0429완료)
- 탄 카메라 이동해도 따라가지 않게 하기(0430완료)
- moveState시 이동할 타일 이동 불가 시 몬스터 텔레포트 현상 고치기
- damageState로 처리할지 character에서 처리할지 정할 것
- 충돌 시 처리를 메세지 시스템을 사용할지 정하기
- character idle 애니메이션 추가
- moveCooltime 이동 보간

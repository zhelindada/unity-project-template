# Unity Prototype Patterns

## 脚本模板

### PlayerController（最小可移动角色）

```csharp
using UnityEngine;

// 占位 PlayerController — 验证移动手感
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravity = -20f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
            controller = gameObject.AddComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = transform.right * h + transform.forward * v;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
```

### CameraFollow（第三人称跟随）

```csharp
using UnityEngine;

// 占位 CameraFollow — 平滑跟随目标
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -8);
    [SerializeField] private float smoothSpeed = 5f;

    void Start()
    {
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void LateUpdate()
    {
        if (target == null) return;
        Vector3 targetPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
        transform.LookAt(target);
    }
}
```

### EnemyAI（最简巡逻/追击）

```csharp
using UnityEngine;

// 占位 EnemyAI — 验证战斗循环
public class EnemyAI : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float health = 100f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float detectRange = 10f;
    [SerializeField] private float attackRange = 2f;

    private Transform player;
    private CharacterController controller;
    private Vector3 startPos;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        controller = GetComponent<CharacterController>();
        if (controller == null)
            controller = gameObject.AddComponent<CharacterController>();
        startPos = transform.position;
    }

    void Update()
    {
        if (player == null) return;
        float distToPlayer = Vector3.Distance(transform.position, player.position);

        if (distToPlayer <= detectRange)
            ChasePlayer();
        else
            Patrol();

        // 简易重力
        if (!controller.isGrounded)
            controller.Move(Vector3.down * 10f * Time.deltaTime);
    }

    void Patrol()
    {
        // TODO: 实现巡逻逻辑
    }

    void ChasePlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        controller.Move(dir * moveSpeed * Time.deltaTime);
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0) Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
```

### GameManager（最小游戏管理器）

```csharp
using UnityEngine;

// 占位 GameManager — 管理游戏状态
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Player")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint;

    private GameObject playerInstance;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        if (playerPrefab != null)
            playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        else
            Debug.LogWarning("[GameManager] playerPrefab 未设置，请将 Player prefab 拖入 Inspector");
    }

    public GameObject GetPlayer() => playerInstance;
}
```

## Unity 场景搭建序列（使用 gladekit-unity MCP）

```
1. create_primitive Plane → "Ground" (scale: 20,1,20)
2. create_primitive Capsule → "Player" (position: 0,1,0)
   → add_component CharacterController
   → set_tag "Player"
3. create_primitive Cube → "Enemy_Spawn_01" (position: 5,1,0)
   → add_component CharacterController
   → set_tag "Enemy"
   → create_material 红色材质 → assign_material_to_renderer
4. create_game_object "GameManager"
   → create_script GameManager.cs
   → create_script PlayerController.cs → add_component PlayerController to Player
   → create_script CameraFollow.cs → add_component CameraFollow to MainCamera
   → create_script EnemyAI.cs → add_component EnemyAI to Enemy
5. create_primitive Cube walls → "Walls" (围成竞技场)
6. create_ui_element TMP → "HUD_Canvas"
   → 子元素: HealthBar (Image filled), ScoreText (TMP)
7. compile_scripts (等待编译完成)
8. enter Play Mode → get_runtime_events 验证
```

## 编译后验证

每次 `compile_scripts` 返回 `status='idle'` 后：
- 如果 `hasErrors=true` → 用返回的 error 信息修复脚本
- 如果 `errorCount=0` → 进入 Play Mode，`start_runtime_observation` 监控运行时错误
- Play Mode 下定期 `get_runtime_events` 拉取报错

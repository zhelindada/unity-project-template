using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace Dada.Cores;

/// <summary>
/// 按键绑定配置 — 将逻辑动作映射到物理按键。
/// 游戏层可通过 GameLifetimeScope 传入自定义绑定，无需修改框架代码。
/// </summary>
public class InputBindings
{
    // ── 移动 / 视角 ──────────────────────────────
    public Key MoveUp      = Key.W;
    public Key MoveDown    = Key.S;
    public Key MoveLeft    = Key.A;
    public Key MoveRight   = Key.D;
    public Key LookUp      = Key.UpArrow;
    public Key LookDown    = Key.DownArrow;
    public Key LookLeft    = Key.LeftArrow;
    public Key LookRight   = Key.RightArrow;

    // ── 交互 ────────────────────────────────────
    public Key Interact    = Key.F;
    public Key Cancel      = Key.Escape;
    public Key Menu        = Key.R;
    public Key Pause       = Key.Space;
    public Key Sprint      = Key.LeftShift;

    // ── 旋转 ────────────────────────────────────
    public Key RotateLeft  = Key.Q;
    public Key RotateRight = Key.E;

    // ── 时间速度 ────────────────────────────────
    public Key TimeSpeed1  = Key.Digit1;
    public Key TimeSpeed2  = Key.Digit2;
    public Key TimeSpeed3  = Key.Digit3;

    // ── 通用动作键 (游戏层语义) ─────────────────
    public Key Action1 = Key.A;
    public Key Action2 = Key.S;
    public Key Action3 = Key.D;
    public Key Action4 = Key.F;
    public Key Action5 = Key.E;
    public Key Action6 = Key.R;
    public Key Action7 = Key.L; // 角色日志

    public static InputBindings Default => new();

    /// <summary>
    /// 返回所有 TimeSpeed 按键，按顺序排列
    /// </summary>
    public IReadOnlyList<Key> GetTimeSpeedKeys() =>
        new[] { TimeSpeed1, TimeSpeed2, TimeSpeed3 };
}

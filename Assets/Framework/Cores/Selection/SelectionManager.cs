using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dada.Cores
{
    public enum SelectionMode
    {
        /// <summary>单选模式 — 同时只能有一个选中实体</summary>
        Single,
        /// <summary>多选模式 — 策略/经营类游戏，可同时选中多个实体</summary>
        Multi
    }

    /// <summary>
    /// 选择管理器 — 框架层可注入服务，持有当前选中/预选状态。
    ///
    /// 两种模式：
    ///   Single — 单选，配合 PreSelection 实现"靠近预览→确认选中"流程
    ///   Multi  — 多选，通过 MaxSelectedCount 控制上限
    ///
    /// PreSelection 仅在 Single 模式下生效，用于预览靠近玩家的可选实体。
    /// 可通过 PreSelectionEnabled 开关。
    /// </summary>
    public class SelectionManager
    {
        // ── 模式配置 ──────────────────────────────

        public SelectionMode Mode { get; set; } = SelectionMode.Single;

        /// <summary>多选模式下最大选中数量</summary>
        public int MaxSelectedCount { get; set; } = 10;

        /// <summary>是否启用预选（仅 Single 模式生效）</summary>
        public bool PreSelectionEnabled { get; set; } = true;

        // ── 选中状态 ──────────────────────────────

        /// <summary>单选模式下的当前选中实体</summary>
        public ISelectable SelectedEntity { get; private set; }

        private readonly List<ISelectable> _selectedEntities = new();

        /// <summary>多选模式下的所有选中实体</summary>
        public IReadOnlyList<ISelectable> SelectedEntities => _selectedEntities;

        // ── 预选状态 ──────────────────────────────

        private readonly List<ISelectable> _preSelectedEntities = new();

        /// <summary>
        /// 预选实体列表 — 靠近玩家、按下确认键后可被选中的实体。
        /// 由 SelectionController 每帧更新，UI 层可监听 OnPreSelectionChanged 做高亮。
        /// </summary>
        public IReadOnlyList<ISelectable> PreSelectedEntities => _preSelectedEntities;

        // ── 事件 ──────────────────────────────────

        public event Action<ISelectable> OnEntitySelected;
        public event Action<ISelectable> OnEntityDeselected;
        public event Action OnSelectionChanged;
        public event Action OnPreSelectionChanged;

        // ── 选中操作 ──────────────────────────────

        public void Select(ISelectable entity)
        {
            if (entity == null || !entity.CanBeSelected)
            {
                Debug.LogWarning($"[SelectionManager] 无法选中: {(entity == null ? "null" : entity.DisplayName)}");
                return;
            }

            if (Mode == SelectionMode.Single)
            {
                if (SelectedEntity != null && SelectedEntity != entity)
                    Deselect(SelectedEntity);

                SelectedEntity = entity;
                _selectedEntities.Clear();
                _selectedEntities.Add(entity);
            }
            else
            {
                if (_selectedEntities.Contains(entity)) return;

                if (_selectedEntities.Count >= MaxSelectedCount)
                    Deselect(_selectedEntities[0]);

                _selectedEntities.Add(entity);
            }

            entity.OnSelected();
            OnEntitySelected?.Invoke(entity);
            OnSelectionChanged?.Invoke();
            Debug.Log($"[SelectionManager] 选中: {entity.DisplayName}");
        }

        public void Deselect(ISelectable entity)
        {
            if (entity == null) return;

            if (Mode == SelectionMode.Single && SelectedEntity == entity)
                SelectedEntity = null;

            _selectedEntities.Remove(entity);
            entity.OnDeselected();
            OnEntityDeselected?.Invoke(entity);
            OnSelectionChanged?.Invoke();
            Debug.Log($"[SelectionManager] 取消选中: {entity.DisplayName}");
        }

        public void DeselectAll()
        {
            if (Mode == SelectionMode.Single)
            {
                if (SelectedEntity != null)
                    Deselect(SelectedEntity);
                return;
            }

            var copy = new List<ISelectable>(_selectedEntities);
            foreach (var e in copy)
                Deselect(e);
        }

        // ── 预选操作 ──────────────────────────────

        /// <summary>
        /// 更新预选列表。传入已按距离排序的实体列表，null 或空列表等同于清空。
        /// </summary>
        public void SetPreSelection(List<ISelectable> entities)
        {
            _preSelectedEntities.Clear();
            if (entities != null && entities.Count > 0)
                _preSelectedEntities.AddRange(entities);
            OnPreSelectionChanged?.Invoke();
        }

        public void ClearPreSelection()
        {
            if (_preSelectedEntities.Count == 0) return;
            _preSelectedEntities.Clear();
            OnPreSelectionChanged?.Invoke();
        }

        // ── 查询 ──────────────────────────────────

        public bool IsSelected(ISelectable entity)
            => Mode == SelectionMode.Single
                ? SelectedEntity == entity
                : _selectedEntities.Contains(entity);

        public bool IsPreSelected(ISelectable entity)
            => _preSelectedEntities.Contains(entity);
    }
}

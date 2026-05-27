---
name: "gdd-monetization"
description: "从基础 GDD 扩展出商业、盈利与 Live Ops 文档——盈利设计、Live Service 路线图、数据分析 KPI、市场与社区策略。Use when 你的游戏是商业产品（F2P 或付费+DLC），需要在设计阶段就规划好盈利和长期运营策略。原型阶段可跳过。"
---

# GDD Business, Monetization & Live Ops

**Tier:** POWERFUL
**Category:** Game Design
**Tags:** GDD, monetization, live ops, F2P, battle pass, analytics, KPIs, community
**Position in pipeline:** 7 of 8
**Status:** 可选（原型阶段可跳过）

## Overview

这是 GDD 扩展流水线的第七步。如果游戏是商业产品（F2P 或付费+DLC），你需要在设计阶段就规划盈利策略和发售后运营。这些决策会反向影响前面的系统设计——例如，如果计划做 Battle Pass，那么任务系统和奖励结构就需要能承载它。

> **注意：** 基础 GDD 中标注为 "skipped for prototype" 的部分——此技能在原型阶段可以跳过。但如果用户需要完整的商业化 GDD，此步骤是必须的。

## Input Requirements

**Required:**
- 基础 GDD（用户指定路径）

**Recommended (如果存在则必须读取):**
- `GDD/02-gameplay/03-progression-economy.md` — 内经济系统直接影响定价和盈利设计
- `GDD/02-gameplay/04-abilities-catalog.md` — 技能/能力如果可付费解锁需在此规划
- `GDD/04-ui-ux/02-menu-map.md` — 商店/通行证 UI 需要在此规划

## Output Documents

### Document 1: Monetization Design (`GDD/07-monetization/01-monetization-design.md`)

**Business Model Selection**
- 明确商业模式（Premium 买断 / F2P + IAP / 订阅制 / 混合）
- 选择理由（对标哪些成功产品、目标用户画像）

**Pricing Strategy**
- 如果是买断制：价格和各区域定价
- 如果是 F2P：

**In-Game Shop Layout**
- 商店分类结构：
```
商店
├── 推荐/精选
├── 皮肤/外观
├── 道具/消耗品
├── 礼包/捆绑包
└── 货币购买
```
- 每个分类的详细内容和定价
- 商店刷新机制（每日刷新/每周精选/限时）

**Premium Currency Design**
- 硬通货（付费获得）vs 软通货（游戏内获得）的区分
- 硬通货的购买档位表（$/¥ + 数量 + 赠送比例）
- 硬通货的心理学定价（如 6/30/68/128/328/648 元档位）
- 首次购买奖励

**F2P Psychology**
- 付费转化路径设计（什么时候引导玩家看商店）
- 付费痛点的合理化（什么内容应该免费、什么应该付费）
- "Pay to win" vs "Pay to progress" vs "Pay to express" 的边界
- 免费玩家的完整体验保障

**DLC / Expansion Strategy**
- DLC 类型（皮肤包/剧情扩展/角色包/通行证）
- DLC 发布节奏
- 免费更新 vs 付费 DLC 的内容边界

**Cosmetic-Only Promise**
- 如果承诺"仅外观付费"：明确"外观"的定义范围
- 什么可能被玩家认为破坏了承诺

### Document 2: Live Service Roadmap (`GDD/07-monetization/02-live-service-roadmap.md`)

**Post-Launch Timeline**
```
月份 1: 首发版本
月份 2: 小型活动 + Bug 修复
月份 3: 第一次内容更新（新角色/新区域）
月份 4: 季节性活动 #1
月份 5: ...
月份 6: 大型扩展
...
```

- 每个节点的内容清单
- 内容的开发排期依赖

**Seasonal Events**
- 年度活动日历：
  - 春节活动
  - 周年庆
  - 夏季活动
  - 万圣节活动
  - 圣诞/年底活动
- 每个活动的典型内容结构（限定任务/限定奖励/限定商店）

**Battle Pass Structure** (如适用)
- 通行证周期（月度/赛季）
- 免费/付费双轨设计
- 等级数量和每级所需经验
- 奖励分布设计（关键奖励放在哪些等级）
- 通行证定价

**Community Challenges**
- 全服目标的类型（击杀数/收集数/捐款数）
- 阶段性奖励设计
- 防止搭便车机制

**Economy Evolution**
- 新内容引入如何影响经济平衡
- 通胀控制策略
- 旧物品的退役/折价机制

### Document 3: Analytics & KPIs (`GDD/07-monetization/03-analytics-kpis.md`)

**Core KPIs**
- Retention（留存）:
  - D1/D3/D7/D14/D30 留存率目标
  - 行业对标数据
- Engagement（参与度）:
  - 日均游戏时长
  - 日均登录次数
  - 核心循环完成率
- Monetization（变现）:
  - 付费率（% paying users）
  - ARPU/ARPPU
  - LTV（用户生命周期价值）
  - CAC（如果投放）

**Funnel Analysis**
- 下载→安装→注册→首次核心循环→首次付费→持续付费 的转化漏斗
- 每个环节的目标转化率
- 流失点假设

**Telemetry Events**
- 需要埋点的关键事件清单：
  - 关卡开始/完成/失败
  - Boss 击杀
  - 物品获得/使用/丢弃
  - 商店打开/浏览/购买
  - 货币产出/消耗
  - 功能使用情况
- 事件的数据结构定义

**A/B Testing Plan**
- 哪些游戏参数适合 A/B 测试
- 测试框架需求

**Balancing Feedback Loop**
- 玩家数据如何反馈到数值调整
- 数据驱动平衡的流程

### Document 4: Marketing & Community (`GDD/07-monetization/04-marketing-community.md`)

**Shareable Moments**
- 内置分享功能设计（截图分享/战绩分享/创作分享）
- 病毒传播机制（邀请奖励/合作奖励）

**Spectator & Streamer Mode**
- 直播模式需求：
  - 隐藏个人信息
  - 聊天显示优化
  - 观众互动功能
- 观战模式设计

**In-Game Social Features**
- 好友系统
- 公会/战队系统
- 聊天系统
- 排行榜

**Community Management Strategy**
- 官方社区平台（Discord/QQ群/贴吧）
- 创作者支持计划
- 玩家反馈渠道和响应机制

## Workflow

1. **Determine necessity** — 先确认游戏是否需要商业化设计（如果是纯单机买断制则大幅简化）
2. **Read upstream inputs** — 重点读 progression-economy 和 abilities-catalog
3. **Design business model** — 选择并明确商业模式
4. **Plan live ops** — 发售后 12 个月的路线图
5. **Define KPIs** — 可衡量的成功指标
6. **Plan community features** — 社交和传播机制
7. **Report** — 总结盈利策略、关键 KPI 目标、发布后路线图

## Design Principles

- **商业化服务于体验，而非反过来**：盈利设计不应该破坏游戏核心乐趣
- **F2P 也是内容设计**：免费玩家的体验本身就是一种内容——他们为付费玩家提供了社区和对手
- **Live Ops 不能是事后补充**：系统设计阶段就需要考虑内容更新的接入点
- **KPI 要可行动**：不能只定义"D7 留存应该更高"，要明确"如果 D7 < X%，我们应该检查什么"

## Output Rules

- 保存到 `GDD/07-monetization/` 目录
- 数值用表格
- 时间线用 mermaid gantt chart
- 中文输出 + 英文术语

# JGameFrame 游戏框架
这是我大学学习游戏开发使用的游戏框架，适合于 2.5D 和 3D 场景下俯视角类RPG游戏。
用它做过双人合作、地图解谜、轻RPG解谜等游戏。

>### 目录概述
- Base 基础库
Module 借鉴自GF模块化设计，进行了轻量化设计。
EC 是轻量化的EC框架，用于摆脱Unity GameObject实现地图逻辑。

- Extern 拓展功能
QuickUI 一种 UI 快速绑定生成绑定代码的工具。

- Frame 游戏框架（必要的模块）
Timer 定时器模块，实现游戏内各种定时器。
Event 全局事件收发。
UI 自己设计的UI框架，栈管理，支持常驻、互斥、共存等UI窗口打开模式。
Resource 资源加载，用Token加载，底层还是Unity Resource，学校项目没必要AB包，之后把加载的实现改掉就行。
FSM 借鉴自GF的简易状态机

- GameApp 业务层
上传版本保留了一部分业务代码，包括地图逻辑和任务逻辑

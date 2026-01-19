# WPF 现代化通用开发架构示例 (WpfAppDemo)

这是一个基于 .NET 8 构建的工业级 WPF 快速开发架构。本项目深度集成了 **Prism** 导航框架与 **CommunityToolkit.Mvvm**，旨在提供一个优雅、高效且高度可扩展的桌面应用开发模板。

## 核心技术栈

- **UI 框架**: WPF (.NET 8)
- **MVVM 核心**: 
  - **Prism.DryIoc**: 处理依赖注入、视图导航及模块化。
  - **CommunityToolkit.Mvvm**: 利用 C# 源生成器实现响应式属性 (`[ObservableProperty]`) 和命令 (`[RelayCommand]`)。
- **UI 组件库**: Material Design In XAML (提供现代化、美观的质感设计界面)。
- **数据库 ORM**: SqlSugar (高性能、易用的国产 ORM，完美支持 SQLite 及多种主流数据库)。
- **日志系统**: Serilog (支持结构化日志，自动输出至控制台及本地文件)。
- **Excel 处理**: MiniExcel (极速处理海量数据导出)。
- **配置文件**: 基于 `appsettings.json` 的标准化配置管理。

## 架构优化亮点

### 1. 极简的 MVVM 开发体验
- **零模板代码**: 通过 `CommunityToolkit.Mvvm`，开发者无需再手动编写繁琐的 `INotifyPropertyChanged` 实现，代码量缩减超过 50%。
- **优雅的密码绑定**: 自研 `PasswordBoxHelper` 附加属性，彻底解决了原生 `PasswordBox` 无法进行 MVVM 数据绑定的历史难题，实现了 100% 的后台代码清理。

### 2. 高度抽象的基类设计
- **ViewModelBase**: 统一管理页面标题、忙碌状态逻辑（IsBusy）以及 Prism 导航生命周期（Navigation Awareness）。
- **ServiceBase**: 预配置 SqlSugar 客户端与 Serilog 实例，业务服务仅需关注核心逻辑，无需重复初始化数据库连接。

### 3. 智能代码生成器 (CodeGen)
- **实时预览**: 支持根据数据库表结构（直连 `sqlite_master` 绕过缓存）实时生成全套 CRUD 源码。
- **一键集成**: 自动生成符合本项目架构规范的 Model、Service、ViewModel 及 XAML View。
- **手动刷新**: 支持手动刷新数据库表结构，实时捕捉数据库变更。

### 4. 工业级服务封装
- **多语言 (L10n)**: 动态切换中英文，支持 XAML 资源与后台代码的同步实时更新。
- **消息服务 (Message)**: 封装了基于 Material Design 样式的异步提示、确认对话框，调用方式极其简洁。
- **主题管理 (Theme)**: 支持明亮/深色模式的运行时一键切换。

## 目录规范

- `Common`: 存放通用工具类（如转换器、Helper）与架构基类。
- `Models`: 业务实体模型。
- `Services`: 定义并实现业务逻辑层，支持 DI 注入。
- `ViewModels`: 处理视图逻辑与数据绑定。
- `Views`: 纯净的 XAML 视图界面。
- `Templates`: 用于代码生成的 T4 风格源码模板。

## 开发环境与运行

1. **环境依赖**: 已安装 .NET 8 SDK。
2. **启动程序**: 直接运行 `WpfAppDemo` 项目。
3. **默认凭据**: 管理员账号 `admin` / 密码 `admin`。
4. **注释支持**: 全项目所有 `.cs`、`.xaml.cs` 及 XAML 资源文件均已包含详尽的 **中文注释**，极易上手。
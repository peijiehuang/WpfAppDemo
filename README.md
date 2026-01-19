# WPF 现代化通用开发架构示例 (WpfAppDemo)

这是一个基于 .NET 8 构建的工业级 WPF 快速开发架构。本项目通过深度集成 **Prism** 导航框架与 **CommunityToolkit.Mvvm**，并引入了**泛型业务基类设计**，旨在提供一个极简、优雅且高度规范的桌面应用开发模板。

## 核心技术栈

- **UI 框架**: WPF (.NET 8)
- **MVVM 核心**: 
  - **Prism.DryIoc**: 负责依赖注入、视图导航及模块化解耦。
  - **CommunityToolkit.Mvvm**: 利用 C# 源生成器实现响应式属性 (`[ObservableProperty]`) 和异步命令 (`[RelayCommand]`)。
- **UI 组件库**: Material Design In XAML (提供工业级美观、易用的质感设计界面)。
- **数据库 ORM**: SqlSugar (国产高性能 ORM，支持 SQLite 及多种主流数据库，内置 CodeFirst)。
- **日志系统**: Serilog (结构化日志记录，自动输出至控制台及本地文件)。
- **Excel 处理**: MiniExcel (高性能数据导入导出)。
- **配置文件**: 基于 `appsettings.json` 的标准 Microsoft.Extensions.Configuration 体系。

## 架构优化亮点

### 1. 深度泛型重构 (Generic Base Service)
- **IBaseService<T>**: 统一定义了标准的 CRUD 接口（Insert, Update, Delete, GetById, GetPageList）。
- **ServiceBase<T>**: 自动化实现了所有基础数据库操作。业务服务（如 `UserService`）仅需关注核心业务逻辑（如特殊过滤、导出），代码量减少 80%。
- **SOLID 原则**: 严格遵守开闭原则与接口隔离原则，架构更具工业级水准。

### 2. 极致的 MVVM 开发体验
- **零模板代码**: 通过源生成器技术，彻底告别繁琐的 `INotifyPropertyChanged` 样板代码。
- **100% 纯净后台**: 利用 `PasswordBoxHelper` 附加属性，实现了 `PasswordBox` 的 MVVM 绑定，真正做到 View 后台代码（Code-Behind）零逻辑。

### 3. 智能代码生成器 (CodeGen 2.0)
- **泛型适配**: 生成的代码自动继承 `ServiceBase<T>`，结构紧凑且完全符合项目规范。
- **实时刷新**: 直连 `sqlite_master` 绕过 ORM 缓存，实时捕捉数据库表结构变更。
- **UI 增强**: 侧边栏宽度优化与主内容区边框装饰，提供了更佳的交互视觉体验。

### 4. 工业级服务封装
- **多语言 (L10n)**: 支持运行时一键切换中英文，资源文件包含清晰的分组注释。
- **异步对话框**: 基于 Material Design 风格封装的 `MessageService`，调用简洁且支持 Task 等待。
- **主题管理**: 运行时支持明亮/深色模式的平滑切换。

## 目录规范

- `Common`: 架构基类（ViewModelBase, ServiceBase）与全局工具类（如 PasswordBoxHelper）。
- `Models`: 纯净的数据实体与业务模型定义。
- `Services`: 业务逻辑接口及其实现层（现已全面转向泛型驱动）。
- `ViewModels`: 各页面的控制逻辑，集成忙碌状态管理。
- `Views`: 纯净的 XAML 视图，采用现代化布局。
- `Templates`: 精心调优的代码生成模板。

## 开发环境与运行

1. **环境依赖**: 已安装 .NET 8 SDK。
2. **启动程序**: 直接运行 `WpfAppDemo` 项目。
3. **默认凭据**: 管理员账号 `admin` / 密码 `admin`。
4. **全量注释**: 每一个 `.cs`、`.xaml` 文件均包含详尽的 **中文注释**。


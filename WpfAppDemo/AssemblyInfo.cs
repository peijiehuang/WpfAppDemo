using System.Windows;

// 设置程序集资源及其查找方式
[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, // 主题特定资源字典所在位置
                                     // (当资源在页面、应用程序或任何主题特定资源字典中找不到时使用)
    ResourceDictionaryLocation.SourceAssembly // 常规资源字典所在位置
                                              // (当资源在页面、应用程序或任何主题特定资源字典中找不到时使用)
)]

/* 
 * 项目名称: WpfAppDemo
 * 项目描述: 基于 Prism 和 Material Design 的工业级 WPF 快速开发通用模板。
 * 核心功能: 
 *   - 动态多语言切换
 *   - 响应式主题切换 (Light/Dark)
 *   - 自动代码生成 (CRUD 模板)
 *   - 结构化日志记录 (Serilog)
 *   - 异步对话框服务
 */
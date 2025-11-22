# 发布步骤

本文档介绍发布新的发行版的步骤。

## 修改版本信息

您需要更新程序内记录的版本信息。该信息存储在以下两个地方：

- `SWTools.Core.Constants.Version` 字段。此信息将在程序内供各处逻辑使用。

  ```csharp
  /* 版本 */
  public static readonly SemVersion Version = SemVersion.Parse("2.0.0-rc.2");
  ```

- `SWTools.WPF` 项目的 `.csproj` 文件。可以在 Visual Studio 中双击项目打开。此信息将在生成的 `SWTools.WPF.exe` 的详细信息中展示。（即程序集信息）

  ```xaml
  <!--版本-->
  <Version>2.0.0-rc.2</Version>

当然，还要更新托管 API：

```json
{
    ...
    "PreRelease": "2.0.0-rc.2",
    ...
}
```

上述例子中的版本号应替换为实际发布的版本号。关于版本号制定规则，见 [开发者手册/版本管理](../DEVELOPMENT.md#版本管理)。

## 发布前准备

在构建发布包前，需要检查：

- 程序中的测试逻辑是否已经删除。
- SWTools.WPF 的输出类型设为 “Windows 应用程序”；SWTools.ViewModel、SWTools.Core 设为 “类库”。
- 把解决方案配置改为 “Release”。

如果创建了开发分支，应先合并开发分支再发布新的发行版。

## 构建发布包

按以下步骤构建发布包：

- 执行 “重新生成解决方案”（`Ctrl+Alt+F7`）。

- 转到 SWTools.WPF 的生成目录。

  ---

- 删除生成目录（如 `\bin\Release\net10.0-windows7.0\`）下的所有 `.pdb` 文件。这些文件是调试符号，不应包含在发布包中。

- 目录下应该包含`LICENSE.txt`、`THIRD-PARTY-NOTICE.md`，这些文件会在生成时自动从 `SWTools.WPF/` 复制到生成目录下。

  ---

- 将生成目录下**所有文件**（包括 `SWTools.deps.json` 和 `SWTools.runtimeconfig.json`）压缩，然后即可在 GitHub 上发布。

> [!WARNING]
>
> 注意不要误打包 `cache/`、`log/` 等文件夹。
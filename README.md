# 极简浏览器
极简浏览器是基于IE的浏览器

## 适用用途

1.剩余性能不足
2.没有浏览器
3.没打算大动干戈

## Bug修复

+ WaringFix #3 修复了函数内部代码冗余的问题
+ BugFix #4 修复了快捷键失效的错误
+ WaringFix #2 修复了History.xaml.cs的函数名不规范
+ WaringFix #1 修复了FileApi.cs的代码质量问题
+ BugFix #3 修复了写入日志错误
+ BugFix #2 修复了无法删除历史记录&书签的错误
+ BugFix #1 修复了无法清除历史记录&书签的错误

## 内核更换计划

1. 安装CefSharp Nuget程序包
2. 引用CefSharp.Wpf名称空间
3. 用ChrominumBrowser替换WebBrowser
4. 更新BrowserCore Api
5. 编译并消除错误
6. 删除冗余代码
7. 根据ChrominumBrowser的特性增加功能
8. 进行黑盒测试
9. 发布到GitHub
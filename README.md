[toc]

# 前言

- 说明

  非原创游戏，是跟着b站M_Studio老师一步一步做的，可以去B站看原教程，不过原教程无项目链接，所以这个项目可以给予减少学习时间，可以快速过一遍项目。

  除了视频教程的基本功能，也完善了玩法、操作人物、音效等。

- 效果截图

  ![](C:/Users/Administer/Desktop/笔记/typorafiles/上传项目到GitHub/3DRpg/运行截图2.png)

  ![](C:/Users/Administer/Desktop/笔记/typorafiles/上传项目到GitHub/3DRpg/运行截图1.png)

# 如何运行

## 版本对应

- 我使用的Unity版本

  Unity 2020.3.25f1c1 (64-bit)

  可以大于等于这个版本

## 用UnityHub打开

- 警告无视

  ![](C:/Users/Administer/Desktop/笔记/typorafiles/上传项目到GitHub/3DRpg/1.1项目警告.png)

- 点开Mains场景

  ![](C:/Users/Administer/Desktop/笔记/typorafiles/上传项目到GitHub/3DRpg/1.2开始场景.png)

## 物体显示粉色

- 显示效果

  ![](C:/Users/Administer/Desktop/笔记/typorafiles/上传项目到GitHub/3DRpg/2.1物体粉色.png)

- 原因

  项目没有**URP**

- 点开Window/Package Manager

  搜索Univer，显示Univeral RP，点击右下角的Install（安装）后显示Remove

  ![](C:/Users/Administer/Desktop/笔记/typorafiles/上传项目到GitHub/3DRpg/2.2安装URP.png)

- 重新启动项目

  一般不会显示粉色了，若还是粉色

- 在Edit-Project Settings

  应用URP渲染管道

  ![](C:/Users/Administer/Desktop/笔记/typorafiles/上传项目到GitHub/3DRpg/2.3选择URP管线.png)

## 脚本错误

- 显示效果

  ![](C:/Users/Administer/Desktop/笔记/typorafiles/上传项目到GitHub/3DRpg/3.1命名空间报错.png)

- 原因

  缺少cinemachine插件

- 点开Window->PackageManager

  搜索cine，然后安装

  ![](C:/Users/Administer/Desktop/笔记/typorafiles/上传项目到GitHub/3DRpg/3.2安装包.png)

  

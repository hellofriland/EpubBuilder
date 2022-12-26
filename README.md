# EpubBuild

将md编译为epub需要填写一个`makefile.txt`文件，在运行时将该文件的路径填入其中



## makefile.txt

在makefile文件中，空行和以`"`开头的行会被忽略

并且，关键字不区分大小写，在最终执行的时候，所有关键字都会被转为小写的格式

例如`CoverPath`会被转换成`coverpath`

```
" 封面图片目前只支持jpg，请保证封面命名为cover.jpg
CoverPath: 
MdPath:
EpubBuildPath:

Title:
Creator:
Description:
Type:
Date:
Language:
```







## Function

- [x] `#...`转换为html
- [x] `**粗体**`转换为html
- [x] 普通文本转换为html

- [ ] 

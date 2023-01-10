::If you want to use it,please edit it:
@echo off
set startGamePath=
::Your game link here↑(not among us application's path,it's Among Us.url's path)
set gamePath=
::Your game path here↑
set sourcePath=
::Your compile path file(a .sln file is in it)
cd /d %sourcePath%
dotnet build
pause
cd /d %gamePath%

if exist BepInEx copy /y "%sourcePath%\bin\Debug\netstandard2.1\TownOfThem.dll" %gamePath%\BepInEx\plugins\ &echo Press any key to start game...&pause>nul&cd /d %startGamePath% &"Among Us.url" &echo game is starting...&pause
::You maybe need edit the first path in this command↑
cd /d %gamePath%
if not exist BepInEx echo BepInEx missing!&pause
exit

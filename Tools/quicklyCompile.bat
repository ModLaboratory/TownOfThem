@echo off
set startGamePath=H:\Win7_Desktop\Games
set gamePath=H:\amgus\AmongUs
set sourcePath=G:\Among Us Mod\自制模组\TownOfThem
cd /d %sourcePath%
dotnet build
pause
cd /d %gamePath%

if exist BepInEx copy /y "%sourcePath%\bin\Debug\netstandard2.1\TownOfThem.dll" %gamePath%\BepInEx\plugins\ &echo Press any key to start game...&pause>nul&cd /d %startGamePath% &"Among Us.url" &echo game is starting...&pause

cd /d %gamePath%
if not exist BepInEx echo BepInEx missing!&pause

echo ......
pause
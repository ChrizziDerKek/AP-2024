@echo off
IF EXIST Output (
	rmdir /s /q Output
)
IF NOT EXIST Output (
	mkdir Output
)
cd Bin/net5.0

for %%i in (../../Input/*.*) do (
	echo Teste Datei %%i
	Controller ../../Input/%%i ../../Output/%%i
	echo:
)

echo:
echo Beliebige Taste druecken, um Ausgaben auf Fehler ueberpruefen
pause
echo:
cd ../../Hilfsmittel

for %%i in (../Input/*.*) do (
	echo Test Datei %%i
	java -jar PuzzleTester.jar ../Input/%%i ../Output/%%i
	echo:
	echo:
)

pause
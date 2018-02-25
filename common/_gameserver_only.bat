FOR /F "tokens=4 delims= " %%P IN ('netstat -a -n -o ^| findstr :2050') DO @ECHO TaskKill.exe /PID %%P
taskkill /f /im LoESoft.GameServer.exe
FOR /F "tokens=4 delims= " %%P IN ('netstat -a -n -o ^| findstr :2050') DO @ECHO TaskKill.exe /PID %%P
taskkill /f /im LoESoft.GameServer.exe
cd bin-server
start LoESoft.GameServer.exe
exit
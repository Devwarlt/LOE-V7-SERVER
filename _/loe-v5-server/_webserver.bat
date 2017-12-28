FOR /F "tokens=4 delims= " %%P IN ('netstat -a -n -o ^| findstr :3333') DO @ECHO TaskKill.exe /PID %%P
taskkill /f /im webserver.exe
FOR /F "tokens=4 delims= " %%P IN ('netstat -a -n -o ^| findstr :3333') DO @ECHO TaskKill.exe /PID %%P
taskkill /f /im webserver.exe
FOR /F "tokens=4 delims= " %%P IN ('netstat -a -n -o ^| findstr :3333') DO @ECHO TaskKill.exe /PID %%P
taskkill /f /im webserver.exe
FOR /F "tokens=4 delims= " %%P IN ('netstat -a -n -o ^| findstr :3333') DO @ECHO TaskKill.exe /PID %%P
taskkill /f /im webserver.exe
cd bin
start webserver.exe
exit
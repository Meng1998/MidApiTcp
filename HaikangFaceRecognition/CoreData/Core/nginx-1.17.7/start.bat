echo off
cd %nginx_home%


	echo "nginx is not running, starting"
	start "" nginx.exe
	echo "start ok"
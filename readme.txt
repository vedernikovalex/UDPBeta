UDP Server Client

This application is C# based using UDP Connetion to comunicate between 
hosts

To use configuration by coonfig file, app.config should be predefined and 
filled with data.

Example:
UDPClient app.coonfig

<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<appSettings>
		<add key="ip" value="127.0.0.1"/>
		<add key="port" value="7777"/>
		<add key="portClient" value="1111"/>
	</appSettings>
</configuration>


UDPServer app.config

<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<appSettings>
		<add key="ip" value="127.0.0.1"/>
		<add key="port" value="7777"/>
	</appSettings>
</configuration>


To run an application, choose which side do you want to be.

For server - ./app/UDPServer/bin/Debug/net6.0/UDPServer.exe
For client - ./app/UDPClient/bin/Debug/net6.0/UDPClient.exe

Both apps use Sliding Window Protocol which is located as DLL in
- ./app/SlidingWindow/bin/Debug/net6.0/SlidingWindow.dll

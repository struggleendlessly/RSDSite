# RSDSite

https://themes.getbootstrap.com/preview/?theme_id=4231
https://htmlstream.com/preview/front-v4.3.1/documentation/index.html

dotnet ef migrations add Initial -o Data/Migrations --project shared --startup-project web/web
dotnet ef database update --project shared --startup-project web/web

Connect-AzAccount -UseDeviceAuthentication

 sudo reboot
 sudo poweroff

////////////////////////////////////////////////////////

sudo systemctl daemon-reload

sudo nano /etc/systemd/system/appbackground-domainsetup-dev.service

sudo systemctl start appbackground-domainsetup-dev.service

sudo systemctl stop appbackground-domainsetup-dev.service

sudo systemctl status appbackground-domainsetup-dev.service


//////////////////////////////////////////////////////// auto start on boot

sudo systemctl enable appbackground-domainsetup-dev.service

////////////////////////////////////////////////////////

sudo chmod -R 777 /home/strug/buildir-api-dev

////////////////////////////////////////////////////////

[Unit]
Description=domainsetup-dev 

[Service]
Type=notify
WorkingDirectory=/home/strug/buildir-api-dev

ExecStart=pwsh -file H:\RSDSite\powershell/azure-add-custom-domain.ps1 -WebAppName devrsd -ContainerName new-domains-dev
Restart=always
RestartSec=3

Environment=DOTNET_ENVIRONMENT=Production
[Install]
WantedBy=multi-user.target

////////////////////////////////////////////////////////


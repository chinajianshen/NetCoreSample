%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe D:\QuartzService\JobSchedule.exe 
Net Start JobSchedule
sc config JobSchedule start=auto
pause
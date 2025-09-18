# MartaPol (.NET MAUI, Android)

Minimalna aplikacja do skanowania kodów z wagą, grupowania w arkusze, eksportu CSV/XLSX oraz wysyłki mailowej.

## Build
```bash
dotnet workload install maui
dotnet restore
dotnet build MartaPol.sln -c Debug
dotnet build App/MartaPol.App.csproj -t:Run -f net8.0-android
```

## Funkcje
- Podgląd kamery i skanowanie (ZXing.Net.Maui)
- Parser GS1 AI (310x/320x) + EAN-13 2xxxxx z regułami
- SQLite (sqlite-net-pcl)
- Eksport CSV / XLSX (MiniExcel)
- Wysyłka przez SMTP (MailKit)
- Ustawienia (SecureStorage, motyw, limity, dźwięk/wibracja)

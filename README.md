# IZOTOFF — веб-сайт

Сайт семейной эко-фермы **IZOTOFF** (сыроварня, виноградник, дегустации и мероприятия). Калининградская область.

Создан на базе структуры проекта Waldau Castle: ASP.NET Core MVC, SQLite, запись на экскурсии, админка мероприятий, Telegram/VK боты (интеграция — отдельный этап).

## Запуск локально

### Обычный запуск (без автоперезапуска)

```powershell
cd Izotoff
dotnet run --launch-profile http
```

Сайт: http://localhost:5210

### Разработка с автоперезапуском (ASP.NET)

```powershell
.\dev.ps1
```

Сайт: http://localhost:5210 — формы, запись, боты, данные из БД.

### Live Server — только вёрстка (HTML/CSS)

Для **визуальной** правки без запуска ASP.NET:

1. Расширение **Live Server** (Cursor предложит установить).
2. Откройте **`preview/home.html`**.
3. **Go Live** → http://127.0.0.1:5500/preview/home.html

CSS берётся из **`wwwroot/css/main.css`** — правите CSS, сохраняете, браузер обновляется сам.  
HTML-структуру правите в **`preview/home.html`**, затем переносите в **`Views/**/*.cshtml`**.

Подробнее: [`preview/README.md`](preview/README.md)

### dotnet watch (альтернатива для полного сайта)

```powershell
dotnet watch run --launch-profile http
```

## Конфигурация

- `appsettings.json` — домен, ключевые слова, токены ботов (пока пустые)
- `Models/SiteInfo.cs` — тексты контактов и брендинг
- `Models/ExcursionCatalog.cs` — форматы экскурсий и цены

## Деплой

После `git push` в `main` GitHub Actions собирает сайт и выкладывает на сервер:

- каталог: `/var/www/izotoff/`
- systemd: `izotoff` (Kestrel `http://127.0.0.1:5010`)
- nginx: `http://188.225.45.211:8080` (пока нет своего домена; Waldau на 80/443 не трогаем)

Secrets репозитория: `SSH_KEY`, `SSH_HOST`, `SSH_USER`.

Локально, минуя GitHub: `.\deploy.ps1`.

## TODO от заказчика

- Домен, точный адрес, телефоны, координаты на карте
- Логотип и изображения
- Токены Telegram/VK для нового сайта
- Финальные тексты политик и команды

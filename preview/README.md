# Live Server — визуальное превью HTML/CSS

Статические страницы для **Live Server** (без ASP.NET).  
Подключают те же CSS/JS, что и боевой сайт: `wwwroot/css/main.css`, `wwwroot/js/main.js`.

## Как запустить

1. Установите расширение **Live Server** (ritwickdey.LiveServer) — Cursor предложит его при открытии проекта.
2. Откройте папку **`Izotoff`** как корень workspace (File → Open Folder).
3. Откройте файл **`preview/home.html`**.
4. Нажмите **Go Live** в правом нижнем углу (или правый клик → *Open with Live Server*).

Адрес: **http://127.0.0.1:5500/preview/home.html**

При сохранении **`wwwroot/css/main.css`** или **`preview/home.html`** страница обновится автоматически.

## Рабочий процесс

| Редактируете | Где смотреть |
|--------------|--------------|
| CSS (`wwwroot/css/main.css`) | Live Server — сразу |
| HTML-структуру, тексты блоков | `preview/home.html` → потом переносите в `Views/**/*.cshtml` |
| C#, формы, запись, боты | `dotnet watch run` на http://localhost:5210 |

**Preview — только вёрстка.** Кнопка «Записаться», формы и данные из БД в Live Server не работают.

## Файлы

- `home.html` — главная страница (полный макет)
- `preview.css` — маленький баннер «режим превью»

Добавляйте `news.html`, `rental.html` по тому же шаблону при необходимости.

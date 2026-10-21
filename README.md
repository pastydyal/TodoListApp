# TodoListApp

## Описание проекта

TodoListApp — веб-приложение для управления личным списком задач, разработанное с использованием ASP.NET Core MVC, Entity Framework Core и SQLite.

## Используемые технологии

- C#
- ASP.NET Core MVC
- Entity Framework Core
- SQLite
- HTML, CSS, Bootstrap

## Функциональность

- Просмотр списка задач.
- Создание новых задач.
- Редактирование названия, описания и статуса выполнения.
- Удаление задач с подтверждением.
- Отметка задач как выполненных и невыполненных.
- Фильтрация по статусу.
- Сортировка по дате создания.
- Подсчёт невыполненных задач.

## Модель данных

TaskItem содержит следующие поля:

- Id — идентификатор задачи.
- Title — название задачи.
- Description — описание задачи.
- IsCompleted — статус выполнения.
- CreatedAt — дата создания.

## Запуск проекта

1. Установить .NET SDK и Visual Studio.
2. Открыть решение проекта в Visual Studio.
3. Установить пакеты Microsoft.EntityFrameworkCore.Sqlite и Microsoft.EntityFrameworkCore.Tools.
4. Открыть Package Manager Console.
5. Выполнить команду:

   Add-Migration InitialCreate

6. Создать базу данных:

   Update-Database

7. Запустить проект клавишей F5.

## База данных

Для хранения данных используется SQLite. Структура базы данных создаётся с помощью миграций Entity Framework Core.
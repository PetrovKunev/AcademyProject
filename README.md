# Academy Project

Уеб приложение за академия с курсове по програмиране, дизайн и бизнес.

## Архитектура

Проектът използва **Clean Architecture** с следните слоеве:

### 🏗️ **Структура на проекта**

```
AcademyProject/
├── Academy.Core/           # Домейн модели и интерфейси
├── Academy.Application/    # Бизнес логика и услуги
├── Academy.Infrastructure/ # База данни и репозитории
└── Academy.Web/           # Уеб приложение (Razor Pages + API Controllers)
```

### 📋 **Описание на слоевете**

#### **Academy.Core**
- **Модели**: `Course`, `ContactMessage`
- **Интерфейси**: `ICourseRepository`, `IContactRepository`

#### **Academy.Application**
- **Услуги**: `CourseService`, `ContactService`
- **Интерфейси**: `ICourseService`, `IContactService`

#### **Academy.Infrastructure**
- **DbContext**: `AcademyDbContext`
- **Репозитории**: `CourseRepository`, `ContactRepository`
- **Инициализация**: `DbInitializer`

#### **Academy.Web**
- **Razor Pages**: Потребителски интерфейс
- **API Controllers**: REST endpoints за AJAX заявки
- **Статични файлове**: CSS, JavaScript, изображения

## 🚀 **Стартиране на проекта**

### Предварителни изисквания
- .NET 9.0 SDK
- SQL Server (или SQL Server Express)
- Visual Studio 2022 или VS Code

### Стъпки за стартиране

1. **Клониране на проекта**
   ```bash
   git clone [repository-url]
   cd AcademyProject
   ```

2. **Възстановяване на пакети**
   ```bash
   dotnet restore
   ```

3. **Конфигуриране на базата данни**
   - Отвори `Academy.Web/appsettings.json`
   - Актуализирай connection string според твоята SQL Server инсталация

4. **Стартиране на приложението**
   ```bash
   dotnet run --project Academy.Web
   ```

5. **Отваряне в браузъра**
   - Отиди на `https://localhost:5001` или `http://localhost:5000`

## 🛠️ **Функционалности**

### 📚 **Курсове**
- Преглед на всички курсове
- Филтриране по категория и ниво
- Детайлна информация за всеки курс

### 📧 **Контакти**
- Контактна форма за изпращане на съобщения
- Валидация на входните данни
- Успешно съобщение след изпращане

### 🎨 **Дизайн**
- Responsive дизайн с Tailwind CSS
- Модерен и красив интерфейс
- Оптимизиран за мобилни устройства

## 🔧 **Технологии**

- **Backend**: ASP.NET Core 9.0
- **Frontend**: Razor Pages, HTML, CSS, JavaScript
- **Styling**: Tailwind CSS
- **Database**: Entity Framework Core, SQL Server
- **Architecture**: Clean Architecture, Repository Pattern

## 📁 **Файлова структура**

```
Academy.Web/
├── Controllers/           # API контролери
│   ├── CoursesController.cs
│   └── ContactController.cs
├── Pages/                # Razor Pages
│   ├── Index.cshtml      # Начална страница
│   ├── Courses.cshtml    # Страница с курсове
│   ├── Contact.cshtml    # Контактна страница
│   └── Shared/           # Споделени компоненти
├── wwwroot/              # Статични файлове
│   ├── css/
│   ├── js/
│   └── lib/
└── Program.cs           # Конфигурация на приложението
```

## 🌐 **API Endpoints**

Приложението включва REST API endpoints за AJAX заявки:

### Курсове
- `GET /api/courses` - Взима всички курсове
- `GET /api/courses/{id}` - Взима конкретен курс
- `GET /api/courses/category/{category}` - Филтрира по категория
- `GET /api/courses/categories` - Взима всички категории
- `GET /api/courses/levels` - Взима всички нива

### Контакти
- `POST /api/contact` - Изпраща ново съобщение
- `GET /api/contact/{id}` - Взима конкретно съобщение
- `GET /api/contact` - Взима всички съобщения
- `GET /api/contact/unread` - Взима непрочетени съобщения

## 🚀 **Развъртване**

### Локално развъртване
```bash
dotnet publish -c Release -o ./publish
```

### Хостинг
Проектът е готов за развъртване на:
- **SmartASP.NET**
- **Azure App Service**
- **AWS Elastic Beanstalk**
- **Любой .NET хостинг**

## 📝 **Промени в архитектурата**

### Премахнати компоненти
- ❌ **Academy.API** - Отделно API приложение
- ❌ **HTMX** - Динамично зареждане на данни

### Запазени компоненти
- ✅ **CORS** - Cross-origin resource sharing (прехвърлен в Web приложението)

### Добавени компоненти
- ✅ **API Controllers** в Web приложението
- ✅ **Директно зареждане** на данни в Razor Pages
- ✅ **Server-side rendering** за по-добра производителност

## 🎯 **Предимства на новата архитектура**

1. **По-проста архитектура** - Едно приложение вместо две
2. **По-добра производителност** - Директно свързване с БД
3. **По-лесно развъртване** - Само един проект за качване
4. **По-ниски разходи** - Един сървър вместо два
5. **По-лесно поддържане** - По-малко код за поддръжка
6. **Запазена функционалност** - CORS поддръжка за cross-origin заявки

## 🤝 **Принос**

1. Fork проекта
2. Създай feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit промените (`git commit -m 'Add some AmazingFeature'`)
4. Push към branch-а (`git push origin feature/AmazingFeature`)
5. Отвори Pull Request

## 📄 **Лиценз**

Този проект е лицензиран под MIT License - виж [LICENSE](LICENSE) файла за детайли.
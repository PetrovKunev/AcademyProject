# Академия - ASP.NET Core 8.0 Web Application

Модерен уеб сайт за академия, използващ ASP.NET Core 8.0, Entity Framework Core, HTMX и Tailwind CSS.

## 🏗️ Архитектура

Проектът следва Clean Architecture принципите с разделение на слоевете:

```
Academy.Solution/
├── Academy.Core/         # Domain модели и интерфейси
├── Academy.Application/  # Business логика и services
├── Academy.Infrastructure/ # Data access и external services
├── Academy.API/          # Web API контролери
├── Academy.Web/          # Razor Pages, HTMX, UI
├── tests/                # Unit тестове
└── docs/                 # Документация
```

## 🚀 Технологии

### Backend
- **ASP.NET Core 8.0** - Web framework
- **Entity Framework Core** - ORM с SQL Server
- **Repository Pattern** - Data access abstraction
- **Clean Architecture** - Separation of concerns

### Frontend
- **ASP.NET Core Razor Pages** - Server-side rendering
- **HTMX** - Dynamic interactions без JavaScript
- **Tailwind CSS** - Utility-first CSS framework
- **Alpine.js** - Minimal JavaScript framework

## 📋 Изисквания

- .NET 8.0 SDK
- SQL Server (LocalDB или Express)
- Visual Studio 2022 или VS Code

## 🛠️ Инсталация

1. **Клонирайте репозиторията**
   ```bash
   git clone <repository-url>
   cd AcademyProject
   ```

2. **Възстановете NuGet пакетите**
   ```bash
   dotnet restore
   ```

3. **Конфигурирайте базата данни**
   - Отворете `Academy.API/appsettings.json`
   - Променете connection string според вашата SQL Server инсталация

4. **Стартирайте приложението**
   ```bash
   # За Web API
   cd Academy.API
   dotnet run
   
   # За Web приложение
   cd Academy.Web
   dotnet run
   ```

## 🌐 Endpoints

### API Endpoints
- `GET /api/courses` - Всички активни курсове
- `GET /api/courses/{id}` - Конкретен курс
- `GET /api/courses/category/{category}` - Курсове по категория
- `GET /api/courses/categories` - Всички категории
- `GET /api/courses/levels` - Всички нива
- `POST /api/contact` - Изпращане на контактно съобщение

### Web Pages
- `/` - Начална страница
- `/Courses` - Списък с курсове
- `/About` - За нас
- `/Contact` - Контакти

## 🎨 Функционалности

### Начална страница
- Hero секция с call-to-action
- Популярни курсове (HTMX зареждане)
- Преимущества на академията
- Testimonials от студенти

### Курсове
- Grid layout с курсове
- Филтриране по категория и ниво
- HTMX за динамично зареждане
- Responsive design

### Контакти
- Контактна форма с HTMX
- Валидация на клиентска и сървърна страна
- Google Maps интеграция
- Информация за контакт

### За нас
- История на академията
- Екип от преподаватели
- Мисия и визия
- Ценности

## 🗄️ База данни

### Модели
- **Course** - Курсове с категория, ниво, цена
- **ContactMessage** - Контактни съобщения

### Seed Data
При първо стартиране се създават примерни курсове:
- JavaScript за начинаещи
- React.js - Пълно ръководство
- Node.js и Express.js
- UX/UI Дизайн основи
- И други...

## 🔧 Конфигурация

### Environment Variables
- `ConnectionStrings:DefaultConnection` - SQL Server connection string
- `Logging:LogLevel` - Logging нива
- `Cache:DefaultExpirationMinutes` - Cache настройки

### CORS
Конфигуриран за localhost портове:
- http://localhost:5000
- https://localhost:5001
- http://localhost:3000

## 🚀 Deployment

### За SmarterASP.NET
1. Публикувайте `Academy.Web` проекта
2. Конфигурирайте connection string за SQL Server
3. Уверете се, че всички зависимости са инсталирани

### За други хостинг провайдери
1. Използвайте `dotnet publish`
2. Конфигурирайте environment variables
3. Настройте IIS или други web server

## 🧪 Тестване

```bash
# Unit тестове
dotnet test

# API тестове
cd Academy.API
dotnet run
# Отворете https://localhost:7001/swagger
```

## 📱 Responsive Design

Сайтът е напълно responsive с:
- Mobile-first подход
- Tailwind CSS breakpoints
- Оптимизирани изображения
- Touch-friendly интерфейс

## 🔒 Security

- Input валидация
- SQL injection protection (EF Core)
- XSS protection
- CSRF protection
- Secure headers

## 📈 Performance

- Response caching
- Memory caching
- Database connection pooling
- Optimized images
- Minified CSS/JS

## 🔄 HTMX Интеграция

- Lazy loading на курсове
- Form submissions без page refresh
- Loading states
- Error handling
- Progressive enhancement

## 🎯 SEO

- Meta tags за всички страници
- Structured data markup
- Open Graph tags
- Semantic HTML
- Fast loading times

## 🤝 Принос

1. Fork проекта
2. Създайте feature branch
3. Направете промените
4. Добавете тестове
5. Създайте Pull Request

## 📄 Лиценз

Този проект е под MIT лиценз.

## 📞 Поддръжка

За въпроси и поддръжка:
- Email: info@academy.bg
- Телефон: +359 888 123 456

---

**Академия** - Вашият партньор за професионално обучение и развитие на умения.
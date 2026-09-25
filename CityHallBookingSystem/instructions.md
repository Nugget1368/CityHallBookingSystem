# City Hall Booking System
Booking System for making private bookings and reservations for City Hall facilities. This system allows users to view available time slots, make reservations, and manage their bookings efficiently.

## Features
- View available time slots for City Hall facilities.
- Make reservations for specific time slots.
- Manage existing bookings, including cancellations and modifications.
- User authentication and account management.
- Admin panel for managing bookings and user accounts.
- Generate reports on bookings and facility usage.

## Installation
Vi börjar med att starta upp ett nytt projekt `ASP.NET Core Web API` som vi kan döpa till `CityHallBookingSystem`.

Vi installerar därefter Entitiý Framework Core och SqlServer som databas. Detta gör vi genom att köra följande kommandon i terminalen:
```bash
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools
Microsoft.EntityFrameworkCore.Design
```

## Database Setup
1. Skapa våra modeller som representerar de olika entiteterna i systemet, såsom `Booking`, `Customer`, och `Resource`.
2. Skapa en databas i SQL Server som heter `CityHallBookingDB`.
3. Skapa en `DbContext`-klass som representerar databasen och innehåller DbSet-egenskaper för varje entitet (t.ex. Booking, User, Facility).
4. Konfigurera anslutningssträngen i `appsettings.json` för att peka på den skapade databasen.
5. Skapa migrations och uppdatera databasen.

### 1. Skapa Modeller
Vi börjar med de modeller och relationer som bör finnas i systemet:

#### Customer Model
Vi börjar med vår Customer-modell som representerar en kund som kan göra bokningar. Den innehåller egenskaper för Id, Name, Email, Phone och en samling av Bookings som kunden har gjort.

```csharp
	public class Customer
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Phone { get; set; } = string.Empty;
		public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
	}
```

#### Booking Model
Vi skapar sedan vår Booking-modell som representerar en bokning som görs av en kund för en specifik resurs. Den innehåller egenskaper för Id, StartTime, EndTime, CustomerId, Customer och ResourceId, Resource.

```csharp
	public class Booking
	{
		public int Id { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public int CustomerId { get; set; }
		public Customer Customer { get; set; } = null!;
		public int ResourceId { get; set; }
		public Resource Resource { get; set; } = null!;
	}
```

Vi ser redan relationen mellan Booking och Customer, samt Booking och Resource. En kund kan ha flera bokningar, och en resurs kan ha flera bokningar. `public int CustomerId { get; set; }` är vår **foreign key** medans `public Customer Customer { get; set; }` är vår navigeringsproperty.

#### Resource Model
Vi skapar sedan vår Resource-modell som representerar en resurs som kan bokas, såsom ett konferensrum eller en hall. Den innehåller egenskaper för Id, Name, Description och en samling av Bookings som är kopplade till resursen.

```csharp
	public class Resource
	{
		public int Id { get; set; }
		public ReservationType Reservation { get; set; }
		public string Comments { get; set; } = string.Empty;
		public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
	}
```

### 2. och 3. Skapa DbContext
Vi skapar en `DbContext`-klass som representerar databasen och innehåller DbSet-egenskaper för varje entitet (t.ex. Booking, Customer, Resource). Vi kan döpa den till `BookingDbContext`.

```csharp
public class BookingDbContext : DbContext
	{
		public BookingDbContext(DbContextOptions<BookingDbContext> options)
			: base(options)
		{
		}

		// Create Tables: Customers, Resources, Bookings
		public DbSet<Customer> Customers { get; set; }

		public DbSet<Resource> Resources { get; set; }

		public DbSet<Booking> Bookings { get; set; }

		// Configure the model relationships and properties
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			// Resource is Enum but save as string in SQL
			modelBuilder.Entity<Resource>()
				.Property(r => r.Reservation)
				.HasConversion<string>();

			// Booking.Status is an enum but is saved as a string in SQL
			modelBuilder.Entity<Booking>()
				.Property(b => b.Status)
				.HasConversion<string>();
		}
	}
```

`public DbSet<Customer> Customers { get; set; }` *DbSet* innebär att *den här modellen* kommer att representera en tabell i databasen. Vi har även konfigurerat våra enum-egenskaper för att sparas som strängar i databasen med hjälp av `HasConversion<string>()`, Vi talar om för EntityFramework att konvertera enum-värdena till strängar när de sparas i databasen och konvertera dem tillbaka till enum-värden när de hämtas från databasen.

### 4. Konfigurera anslutningssträngen
Vi behöver registrera `BookingDbContext` hos ASP.NET Core. I *program.cs* lägger vi till följande kod:
```csharp
using BookingSystem.Data;
using Microsoft.EntityFrameworkCore;
```
Och sedan:
```csharp
builder.Services.AddDbContext<BookingDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("BookingDatabase")));
```

**Notera att detta måste läggas till innan `r app = builder.Build();`**

Vi lägger nu till *Connection strängen* i`appsettings.json`:
```json
{
  "ConnectionStrings": {
    "BookingDatabase": "Server=(localdb)\\MSSQLLocalDB;Database=BookingSystemDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

`Server=(localdb)\MSSQLLocalDB` är *LocalDb-instansen* och `Database=BookingSystemDb` är namnet på databasen som vi vill skapa. `Trusted_Connection=True` innebär att vi använder Windows-autentisering för att ansluta till databasen.

### 5. Skapa migrations och uppdatera databasen

Nu ska vi låta Entity Framework skapa databasschemat. Öppna *Package Manager Console* i Visual Studio och kör följande kommando:

```powershell
Add-Migration InitialCreate
```

Vi kommer få en ny mapp som heter `Migrations` i vårt projekt. Den innehåller en fil som heter `InitialCreate.cs` som innehåller koden för att skapa databasschemat.

Om vi gör förändringar i våra modeller och vill uppdatera databasen, kan vi skapa en ny migration med ett nytt namn, t.ex. `AddNewColumnToCustomer`, och sedan köra `Update-Database` igen.

Nu skapar vi själva databasen:

```powershell
Update-Database
```

Vi bör nu finna vår databas under *SQL Server Object Explorer* i Visual Studio. Vi kan expandera databasen och se att tabellerna `Customers`, `Resources` och `Bookings` har skapats.

## API Controllers
Vi kan nu börja skapa våra API-kontroller för att hantera bokningar, kunder och resurser. Vi kan skapa en `BookingsController`, `CustomersController` och `ResourcesController` för att hantera CRUD-operationer (Create, Read, Update, Delete) för respektive entitet.

### Resources Controller

```csharp
using BookingSystem.Data;
using BookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResourcesController : ControllerBase
{
    private readonly BookingDbContext _context;

    public ResourcesController(BookingDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Resource>>> GetResources()
    {
        var resources = await _context.Resources.ToListAsync();

        return Ok(resources);
    }
}
```

`[ApiController]` Talar om för ASP.NET att denna klass är en API-kontroller och aktiverar vissa funktioner som automatisk modellvalidering och routing.

`[Route("api/[controller]")]` Anger routemönstret för denna kontroller. `[controller]` ersätts med namnet på kontrollerklassen utan "Controller"-suffixet, i detta fall "resources". Eftersom klassen heter `ResourcesController`, kommer routen att bli `api/resources`

`private readonly BookingDbContext _context;` Sparar en referens till databaskontexten som används för att interagera med databasen. Detta fungerar eftersom vi tidigare la till:

```csharp
builder.Services.AddDbContext<BookingDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("BookingDatabase")));
```

I `BookingDbContext.cs`.

#### HTTP-Post

##### Skapa DTO (Data Transfer Object)

Vi börjar med att skapa en DTO (Data Transfer Object) som representerar de data som vi vill ta emot från klienten när vi skapar en ny resurs. Vi kan döpa den till `CreateResourceDto`.

```csharp
using BookingSystem.Models;

namespace BookingSystem.DTOs;

public class CreateResourceDto
{
    public ReservationType Reservation { get; set; }

    public string Comments { get; set; } = string.Empty;
}
```

###### Varför inte bara andvända `Resource-modellen`?
Eftersom vi **inte** vill att klienten ska kunna skicka med ett Id (som är auto-genererat av databasen) eller andra egenskaper som **inte** ska sättas av klienten, skapar vi en separat DTO som endast innehåller de egenskaper som klienten får skicka med. Klienten ska alltså endast kunna bestämma *Reservationstypen* och *kommentarer* för en reservation.

##### POST metoden

```csharp
[HttpPost]
    public async Task<ActionResult<Resource>> CreateResource(
        CreateResourceDto resourceDto)
    {
        var resource = new Resource
        {
            Reservation = resourceDto.Reservation,
            Comments = resourceDto.Comments
        };

        _context.Resources.Add(resource);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetResources),
            new { id = resource.Id },
            resource);
    }
```
`[HttpPost]` Anger att denna metod hanterar HTTP POST-förfrågningar. Den används för att skapa en ny resurs. `_context.Resources.Add(resource);` Talar om för EF Core att *denna Entitet ska läggas till i databasen*. `await _context.SaveChangesAsync();` skickar ändringarna till SQL Server och sparar ändringarna i databasen. `CreatedAtAction` Returnerar en HTTP 201 Created-statuskod tillsammans med den skapade resursen och dess plats (URL) i svaret.


#### HTTP-GET/{id}

```csharp
[HttpGet("{id}")]
public async Task<ActionResult<Resource>> GetResource(int id)
{
    var resource = await _context.Resources
        .FirstOrDefaultAsync(r => r.Id == id);

    if (resource == null)
    {
        return NotFound();
    }

    return Ok(resource);
}
```

Detta låter oss anropa `GET api/resources/{id}` för att hämta en specifik resurs baserat på dess Id. Om resursen inte hittas returneras en HTTP 404 Not Found-statuskod.

Just nu använder vi `FirstOrDefaultAsync` för att hämta resursen. Detta gör att vi inte hämtar bokningarna som är kopplade till resursen `public ICollection<Booking> Bookings { get; set; } = new List<Booking>();`, hade vi velat hämta bokningarna också hade vi kunnat använda `Include` för att inkludera bokningarna i anropet:
```csharp
var resource = await _context.Resources
	.Include(r => r.Bookings)
	.FirstOrDefaultAsync(r => r.Id == id);
```

# Fortsättning - Färdigställ all CRUD-funktionalitet

Just nu har vi följande:

```
Customer
├── GET all
├── GET by id
└── POST

Resource
├── GET all
├── GET by id
├── POST
└── DELETE

Booking
├── GET all
├── GET by id
├── POST
└── CANCEL
```
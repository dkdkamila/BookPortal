using Microsoft.AspNetCore.Mvc;
using BookPortal.Models;
using Microsoft.AspNetCore.Identity;
using BookPortal.Data;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;



public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _context = context;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        // Visa tre kort: Registrera konto, Logga in, Bläddra bland böcker.
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult AllBooks()
    {
        // Visa en lista med alla böcker (titel och författare).
        var books = _context.Books!.ToList();
        return View(books);
    }
    public IActionResult Details(int id)
    {
        var book = _context.Books!.FirstOrDefault(b => b.Id == id);
        if (book == null)
        {
            return NotFound();
        }

        var reviews = _context.Reviews!.Where(r => r.BookId == id).ToList();
        foreach (var review in reviews)
        {
            // Laddar manuellt användaren för varje recension om den inte är null.
            if (review.UserId != null)
            {
                review.User = _context.Users.FirstOrDefault(u => u.Id == review.UserId);
            }
        }

        var ratings = _context.Ratings!.Where(rt => rt.BookId == id).ToList();
        foreach (var rating in ratings)
        {
            // Laddar manuellt användaren för varje betyg om den inte är null.
            if (rating.UserId != null)
            {
                rating.User = _context.Users.FirstOrDefault(u => u.Id == rating.UserId);
            }
        }

        var viewModel = new BookDetailsViewModel
        {
            Book = book,
            Reviews = reviews,
            Ratings = ratings
        };

        return View(viewModel);
    }


    [HttpGet]
    public IActionResult Search(string searchString)
    {
        // Hantera sökning efter böcker.
        var books = _context.Books!.Where(b => b.Title!.Contains(searchString) || b.Author!.Contains(searchString)).ToList();
        return View("AllBooks", books);
    }

    public IActionResult Login()
    {
        // Formulär för inloggning.
        return View();
    }


    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> Login(string InputEmail, string InputPassword, bool RememberMe)
    {
        // Hantera inloggning av användare.
        var result = await _signInManager.PasswordSignInAsync(InputEmail, InputPassword, RememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            return RedirectToAction("LoggedIn");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Ogiltigt försök till inloggning.");
            return View();
        }
    }


    public IActionResult Register()
    {
        // Formulär för registrering av nytt konto.
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(string InputEmail, string InputPassword, string ConfirmPassword)
    {
        if (InputPassword != ConfirmPassword)
        {
            ModelState.AddModelError(string.Empty, "Lösenorden matchar inte.");
            return View();
        }

        var user = new ApplicationUser { UserName = InputEmail, Email = InputEmail };
        var result = await _userManager.CreateAsync(user, InputPassword);

        if (result.Succeeded)
        {
            ViewBag.SuccessMessage = "Ditt konto har skapats framgångsrikt!";
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Login"); // Eller annan action som visar ett framgångsmeddelande
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View();
    }
    public async Task<IActionResult> Logout()
    {
        // Logga ut användaren.
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> AddReview(int bookId, string reviewText)
    {
        // Använder _userManager för att hämta den aktuellt inloggade användaren asynkront
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            // Om ingen användare är inloggad, redirecta till inloggningssidan 
            return RedirectToAction("Login");
        }

        // Skapa en ny recension med den inloggade användarens ID
        var review = new Review
        {
            BookId = bookId,
            Text = reviewText,
            UserId = user.Id
        };
        _context.Reviews!.Add(review);
        await _context.SaveChangesAsync(); // Spara ändringarna asynkront
        return RedirectToAction(nameof(Details), new { id = bookId });
    }
    [HttpGet]
    public IActionResult EditReview(int id)
    {
        var review = _context.Reviews!.Find(id);
        if (review == null)
        {
            return NotFound();
        }
        return View(review);
    }


    [HttpPost]
    public IActionResult UpdateReview(int reviewId, string updatedReviewText)
    {
        // Uppdatera en befintlig recension.
        var review = _context.Reviews!.Find(reviewId); // Hitta recensionen baserat på id
        if (review != null)
        {
            review.Text = updatedReviewText; // Uppdatera texten för recensionen
            _context.SaveChanges(); // Spara ändringar i databasen
        }
        return RedirectToAction(nameof(LoggedIn), new { id = review!.BookId }); // Redirect till detaljvyn för boken
    }



    [HttpPost]
    public IActionResult DeleteReview(int reviewId)
    {
        var review = _context.Reviews!.Find(reviewId);
        if (review != null)
        {
            _context.Reviews.Remove(review);
            _context.SaveChanges();
            TempData["Message"] = "Recensionen har tagits bort.";
        }
        else
        {
            TempData["ErrorMessage"] = "Recensionen kunde inte hittas.";
        }
        return RedirectToAction("LoggedIn");
    }

    [HttpPost]
    public async Task<IActionResult> AddRating(int bookId, int ratingValue)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var rating = new Rating
        {
            BookId = bookId,
            Value = ratingValue,
            UserId = user.Id // Antagande att Rating-modellen har en UserId-egenskap
        };
        _context.Ratings!.Add(rating);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id = bookId });
    }


    [HttpGet]
    public IActionResult LoggedIn()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // Inkludera den relaterade Book-entiteten vid hämtning av recensioner
        var reviews = _context.Reviews!.Include(r => r.Book).Where(r => r.UserId == userId).ToList();
        return View(reviews);
    }




    [HttpPost]
    public IActionResult AddBook(string title, string author, string genre, string description)
    {
        try
        {
            var existingBook = _context.Books!.FirstOrDefault(b => b.Title == title && b.Author == author);
            if (existingBook != null)
            {
                TempData["Message"] = "Boken finns redan i databasen.";
                return RedirectToAction(nameof(LoggedIn));
            }

            var newBook = new Book
            {
                Title = title,
                Author = author,
                Genre = genre,
                Description = description
            };
            _context.Books!.Add(newBook);
            _context.SaveChanges();
            TempData["Message"] = "Boken har lagts till framgångsrikt.";
        }
        catch (Exception ex)
        {

            Console.WriteLine(ex.ToString());
            // Sätt ett felmeddelande som sedan kan visas för användaren
            TempData["ErrorMessage"] = "Ett fel uppstod när boken skulle läggas till. Vänligen försök igen.";
        }

        return RedirectToAction(nameof(LoggedIn));
    }

}



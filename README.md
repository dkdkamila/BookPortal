# BookPortal
Om BookPortal
BookPortal är en webbapplikation utvecklad med ASP.NET Core MVC som syftar till att förena bokälskare genom att tillhandahålla en plattform för recensioner och betyg av böcker. Användare kan skapa ett konto, logga in, lägga till böcker, skriva recensioner, och betygsätta böcker som de har läst.

Funktioner
Användarregistrering och inloggning: Säker autentisering med ASP.NET Core Identity.
Bokhantering: Användare kan lägga till och visa böcker.
Recensioner: Möjlighet att skriva, visa, uppdatera och ta bort recensioner för varje bok.
Betyg: Användare kan betygsätta böcker på en skala från 1 till 5.
Responsiv design: Webbapplikationen är mobilvänlig tack vare Bootstrap.
Sökfunktionen: Enkel sökning av böcker i databasen.

Tekniska Krav
.NET 5.0 eller senare
Visual Studio 2019 eller Visual Studio Code
SQL Server (för lokal utveckling) eller en kompatibel databastjänst

Installation och Konfiguration
Klona repot:

bash
Copy code
git clone https://github.com/dittgithubkonto/BookPortal.git
Öppna projektet i Visual Studio eller Visual Studio Code.

Installera beroenden:

Öppna en terminal och navigera till projektets rotmapp.
Kör kommandot dotnet restore för att installera alla nödvändiga NuGet-paket.

Konfigurera databasen:

Uppdatera appsettings.json med din databasanslutningssträng.
Använd Entity Framework Core-migreringar för att skapa databasen: dotnet ef database update.
Starta applikationen:

Kör dotnet run för att starta applikationen.
Öppna en webbläsare och navigera till http://localhost:5000.
Utveckling
Projektet är uppbyggt enligt MVC-arkitekturen, vilket gör det enkelt att vidareutveckla och underhålla. För att lägga till nya funktioner, följ MVC-mönstret genom att skapa motsvarande modeller, vyer och controllers.



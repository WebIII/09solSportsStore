# Oefening - Hoofdstuk 9 SportsStore: Validatie & Authenticatie

In dit deel van de oefening bouwen we validatie in voor de CRUD operaties van product en bouwen we authenticatie en autorisatie in. De starter is gebaseerd op een .NET Core, ASP.NET Core 3.0 webapplicatie, MVC template met Individual User Account authenticatie. Er werd een nieuw project aangemaakt. De Data, Controllers, Models en Views en Filters uit [08solSportsStore_Part2](https://github.com/WebIII/08solSportsStore_Part2) werden aan dit project toegevoegd.

## Quickstart
Clone [deze repository](https://github.com/WebIII/09exSportsStore.git)

### Connectiestring 

Pas de connectionstring in de appSettings.json aan, zorg dat er gebruik wordt gemaakt van `localhost` en benoem de databank `SportsStore09`.

## Validatie
### ProductController – Index 
Voeg `Display` annotaties toe zodat de `Index` pagina als volgt wordt weergegeven.
- De Display annotaties voor de enum `Availability` voeg je toe in de `Enum` klasse zelf. 
- Maak in de `Index` view gebruik van de gegeven extension methode `GetDisplayName()` in de statische klasse `EnumHelpers` in de Helpers namespace voor het ophalen van de annotatie. 
- Vergeet hierbij geen `using` statement te gebruiken in de view!

![Product_Index.png](https://github.com/WebIII/portal/raw/master/docs/H09/images/product-index.PNG)

Bekijk de code in de `EnumHelpers`. Zoek onderstaande op via de Object Browser of maak gebruik van F12: Goto definition 
- Wat doet `typeof()`. 
- Wat doet de extension methode `GetMember` van het geretourneerde type? Wat retourneert deze methode? 
- Wat doet Methode `GetCustomAttribute<DisplayAttribute>`? 
- Tot welke `namespace` behoren deze methodes en klassen?

### ProductController - Edit  
Voeg annotaties toe voor de **weergave**.
- Pas de view aan zodat de Property `AvailableTill` ook wordt weergegeven.  
- Voeg annotaties toe zodat de labels correct worden weergegeven en `AvailableTill` als `Date` input field wordt weergegeven. 

![Product_Edit.png](https://github.com/WebIII/portal/raw/master/docs/H09/images/product-edit.PNG)

Voeg annotaties toe voor volgende validatie. Geef gepaste foutmeldingen. 
- `Name`, `price`, `availability` en `category` zijn verplicht 
- `Name` is minstens `5` karakters lang en maximum `100` 
- `Price` is gelegen tussen `1` en `3000`. 

Voor de client-side validatie zijn er nog enkele wijzigingen nodig aan de view.
- Pas de View aan zodat de foutmeldingen getoond worden (een overzicht van de foutmelding en de foutmelding per property). 
- Maak gebruik van de validation tag helpers. 
- Voeg de nodige scripts toe.

> Opgelet: de decimale , die gebruikt wordt voor de Price zal niet naar behoren werken indien je een locale verschillend van en-US gebruikt. Deze issue werd in deze oplossing niet aangepakt, je geeft dus gehele getallen in voor de Price. Zie https://github.com/aspnet/Docs/issues/4076 en https://github.com/aspnet/Docs/issues/8585 

![Product_Edit_Val.png](https://github.com/WebIII/portal/raw/master/docs/H09/images/product-edit-val.PNG)

Schrijf eerst de volgende unit testen voor de `Edit` methode. Train indien nodig verder de `mock`. Bekijk ook de overige unit testen binnen de region Edit 
- `EditHttpPost_ProductNotFound_ReturnsNotFoundResult()`
- `EditHttpPost_ModelStateErrors_DoesNotChangeNorPersistTheProduct()` 
- `EditHttpPost_ModelStateErrors_PassesEditViewModelInViewResultModelAndPassesSelectListsInViewData()` 
- Wijzig de `Edit-HttpPost` methode zodat de testen slagen. 

> Indien je hulp nodig hebt, vind je deze in de slides van hoofdstuk 8 en 9. 

### ProductController - Create (analoog)
Schrijf volgende unit testen 
- `CreateHttpPost_ModelStateErrors_DoesNotChangeNorPersistTheProduct()` 
- `CreateHttpPost_ModelStateErrors_PassesEditViewModelInViewResultModelAndPassesSelectListsInViewData()` 

- Vervolledig dan de `Create-HttpPost` methode in de `ProductController` zodat de unit testen slagen.

Je merkt dat we heel vaak in het `Assert` gedeelte van onze test methodes dezelfde training voor onze mocks moeten instellen. Refactor de `ProductControllerTest` klasse en verplaats de herhalende mock trainingen naar de `constructor` van de klasse. 

### ProductController - Delete (analoog)
Schrijf volgende unit test 
- `DeleteHttpPost_ProductNotFound_ReturnsNotFound()`

- Pas de `DeleteConfirmed` methode aan zodat de test slaagt 

---

## Authenticatie 

Merk op dat het Identity Framework reeds werd geïntegreerd in het project. Pas `SportsStoreDataInitializer` aan, zodat bij de `seeding` ook de `IdentityUsers` worden aangemaakt:
- Injecteer de `UserManager` via de constructor in de `SportsStoreDataInitializer`. (De methode `InitializeData` zal moeten gebruik maken van de geïnjecteerde klasse).
- Maak een `IdentityUser` met email `admin@sportsstore.be` aan met wachtwoord `P@ssword1`. 
    > Merk op: de `UserManager` maakt gebruik van `async` methodes. Pas de signatuur van de methode `InitializeData` aan naar het volgende `public async Task InitializeData()`.  
    wijzig daarnaast de aanroep vanuit de `StartUp` Configure methode naar: 
`sportsStoreDataInitializer.InitializeData().Wait();`

- Maak voor elke klant uit de `Initializer` een `IdentityUser` aan met als `Username`, de `CustomerName` van de klant gevolgd door `@hogent.be`. Zorg dat zijn `Email` **identiek** is aan zijn `Username` en stel het `wachtwoord` in op `P@ssword1`.
- Run de applicatie en log in eens als **admin** en eens als **student1**.


---

## Authorisatie
- Definieer de authorizatie `policy` in de `Startup` klasse. Voorzie 2 rollen: `admin` en `customer`.
- Pas de `SportsStoreDataInitializer` aan zodat de `admin` tot de rol `admin` behoort en de `klanten` tot de rol `customer`.
    >  In principe moet een nieuwe klant zich ook kunnen registreren, zie volgend hoofdstuk. 
- Pas de Controllers aan: 
    - Enkel een `admin` kan de `producten` **zien** en **beheren**. 
    - De shop is voor **iedereen** toegankelijk. 
    - **Enkel gebruikers van de rol customer** kunnen een **checkout** doen van het winkelmandje. Voeg hiervoor eerst een methode `Checkout` toe aan de `CartController`. Deze methode retourneert **voorlopig** een empty `View without model`. (maak ook de view aan met enkel een pagina titel) 
- Pas de `_Layout` view aan zodat het menu item `Products` enkel zichtbaar is als een `Administrator` is aangemeld. Meer info op https://docs.microsoft.com/en-us/aspnet/core/security/authorization/views?tabs=aspnetcore2x 

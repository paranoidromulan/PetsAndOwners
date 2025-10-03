using PetsAndOwners;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<PetsAndOwnersDB>();

var app = builder.Build();

// POST: Add pet and owner
app.MapPost("/pets", (PetWithOwner pet, PetsAndOwnersDB db) =>
{
    if (pet == null) return Results.BadRequest("Missing body");
    if (string.IsNullOrWhiteSpace(pet.PetName)) return Results.BadRequest("PetName required");

    db.AddOwner(pet.OwnerName, pet.Phone);
    db.AddPet(pet.PetName, pet.Species, pet.OwnerName);

    return Results.Created($"/pets/{pet.PetName}", pet);
});

// GET: Get owner's phone by pet name
app.MapGet("/owners/phone/{petName}", (string petName, PetsAndOwnersDB db) =>
{
    var results = db.SearchPhoneByPetName(petName);
    if (results == null || results.Count == 0) return Results.NotFound();

    return Results.Ok(new { phone = results.First().phone });
});

app.Run();


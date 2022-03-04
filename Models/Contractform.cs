using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace DC_CONTRACTFORM.Models;

public class Contractform {

    // Allows us to work with  Id as a string within .net core application
    // Saved as an objectId in MongoDB
    // If it doesnt exist, it will create one for us
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set;}
    
    public string firstName { get; set;} = "";
    public string lastName { get; set;} = "";

    public string phoneNumber { get; set;} = "";

    public string addressLine1 { get; set;} = "";

    public string? addressLine2 { get; set;}

    public string city { get; set;} = "";

    public string state { get; set;} = "";

    public string zipcode { get; set;} = "";

    public string? email { get; set;}

    public string vetName { get; set;} = "";

    public string? vetPhoneNumber { get; set;}

    public string dogBreed { get; set;} = "";

    public string dogName { get; set;} = "";

    public string dogAge { get; set;} = "";

    public string? notes { get; set;}

    public string signature { get; set;} = "";

}

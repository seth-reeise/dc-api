using DC_CONTRACTFORM.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace DC_CONTRACTFORM.Services;

public class MongoDBServicce {

    private readonly IMongoCollection<Contractform> _contractformCollection;

}


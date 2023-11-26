// See https://aka.ms/new-console-template for more information

using JsonDiffPatchDotNet;
using LiteDocumentStore.Cli.Model;
using LiteDocumentStore.Core;
using Newtonsoft.Json.Linq;

Console.WriteLine("Hello, World!");

var jdp = new JsonDiffPatch();
var left = JToken.Parse(@"{ ""key"": false, ""id"": 1  }");
var right = JToken.Parse(@"{ ""key"": true }");

JToken patch = jdp.Diff(left, right);

Console.WriteLine(patch.ToString());


var customerId = Guid.NewGuid();
var customer = new Customer("A1001", "Max", "Muster", false);

var store = new Store("/Users/maddin/Documents/Development/temp");
var collection = await store.GetCollectionAsync<Customer>("customer");

await collection.InsertAsync(customerId, customer);


// store.db
// [GUID] [SHA1-1] 1
// [GUID] [SHA1-2] 2
// [GUID] [SHA1-3] 3

//store.changes.db
// [SHA1-1] [JSON]
// [SHA1-2] 1 [JSON-Property:Value]
// [SHA1-2] 2 [JSON-Property:Value]
// [SHA1-2] 3 [JSON-Property] 

// 1: Add
// 2: Update
// 3: Delete
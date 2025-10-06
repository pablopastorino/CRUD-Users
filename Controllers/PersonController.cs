using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace UserManagement.Controllers
{
    public class Person
    {
        public required string UserName { get; set; }
        public int UserAge { get; set; }
    }

    [ApiController]

    [Route("api/[controller]")]

    public class PersonController : ControllerBase

    {

        [HttpGet]

        public ActionResult<List<Person>> Get()

        {

            return new List<Person>

            {

                new() { UserName = "Jhon", UserAge = 52 },

                new() { UserName = "Mary", UserAge = 33 },

                new() { UserName = "Bety", UserAge = 40 }

            };

        }

        [HttpPost]

        public ActionResult<string> Post([FromBody] Person newPerson)

        {
            // Binary serialization
            using (FileStream fs = new("person.dat", FileMode.Create))
            {
                using BinaryWriter writer = new(fs);
                writer.Write(newPerson.UserName);
                writer.Write(newPerson.UserAge);
            }
            Console.WriteLine("Binary serialization complete.");

            // XML serialization
            XmlSerializer xmlSerializer = new(typeof(Person));

            using (StreamWriter writer = new("person.xml"))
            {
                xmlSerializer.Serialize(writer, newPerson);
            }
            Console.WriteLine("XML serialization complete.");

            // JSON serialization
            string jsonString = JsonSerializer.Serialize(newPerson);
            System.IO.File.WriteAllText("person.json", jsonString);

            Console.WriteLine("JSON serialization complete.");

            return $"Added: {newPerson.UserName}";

        }

        [HttpPut("{id}")]

        public ActionResult<string> Put(int id, [FromBody] Person updatedPerson)

        {

            return $"Updated person {id} to: {updatedPerson.UserName}";

        }

        [HttpDelete("{id}")]

        public ActionResult<string> Delete(int id)

        {

            return $"Deleted product with ID: {id}";

        }

    }

}
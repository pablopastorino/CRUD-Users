using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;

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
        [HttpGet("binary")]
        public ActionResult<Person> GetFromBinary()
        {
            Person binaryDeserializedPerson;
            using (var fs = new FileStream("person.dat", FileMode.Open))
            using (var reader = new BinaryReader(fs))
            {
                binaryDeserializedPerson = new Person
                {
                    UserName = reader.ReadString(),
                    UserAge = reader.ReadInt32()
                };
            }

            Console.WriteLine($"Binary Deserialization - UserName: {binaryDeserializedPerson.UserName}, UserAge: {binaryDeserializedPerson.UserAge}");
            return binaryDeserializedPerson;
        }

        [HttpGet("xml")]
        public ActionResult<Person> GetFromXml()
        {
            XmlSerializer serializer = new(typeof(Person));
            using var fs = new FileStream("person.xml", FileMode.Open);
            Console.WriteLine($"XML Deserialization");
            return (Person)serializer.Deserialize(fs)!;
        }

        [HttpGet("json")]
        public ActionResult<Person> GetFromJson()
        {
            string jsonString = System.IO.File.ReadAllText("person.json");
            Console.WriteLine($"JSON Deserialization");
            return JsonSerializer.Deserialize<Person>(jsonString)!;
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
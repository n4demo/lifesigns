﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSigns
{
    internal class PersonGenerator
    {
        public Person GetThomas()
        {
            Person personDetails = new Person
            {
                Id = "1234567890",
                Firstname = "Thomas",
                LastName = "AnderSon"
            };

            personDetails.Addresses.Add(
                new Address
                {
                    Line1 = "100 Some Street",
                    Line2 = "Unit 1",
                    City = "Seattle",
                    State = "WA",
                    Zip = "98012"
                }
            );

            personDetails.ContactDetails.Add
                (
                    new ContactDetail
                    {
                        Email = "thomas@andersen.com",
                        Phone = "+1 555 555-5555",
                        Extension = "5555"

                    }
                );

            personDetails.Logins.Add
                (
                    new Login
                    {
                        When = DateTime.Now.Ticks.ToString()
                    }
                );

            return personDetails;
        }

        public Person GetRandomPerson()
        {
            Person person = new Person
            {
                Id = RandomString(),
                Firstname = $"{RandomString()}",
                LastName = $"{RandomString()}"
            };

            person.Addresses.Add(
                new Address
                {
                    Line1 = $"{RandomString()} Some Street",
                    Line2 = "Unit 1",
                    City = "Seattle",
                    State = "WA",
                    Zip = "98012"
                }
            );

            person.ContactDetails.Add
                (
                    new ContactDetail
                    {
                        Email = $"{RandomString()}@acme.com",
                        Phone = "+1 555 555-5555",
                        Extension = "5555"

                    }
                );

            person.Logins.Add
                (
                    new Login { When = DateTime.Now.ToLongDateString() }
                );

            return person;
        }

        private static Random random = new Random();

        private static string RandomString()
        {
            return RandomString(6);
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var mystring = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return mystring.ToLower();
        }
    }
}
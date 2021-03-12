using System;

namespace DatabaseFirstLINQ
{
    class Program
    {
        static void Main(string[] args)
        {
            Problems problems = new Problems();
            problems.RunLINQQueries();
            // each var query =
            //people.Join(pets,
            //            person => person,
            //            pet => pet.Owner,
            //            (person, pet) =>
            //                new { OwnerName = person.Name, Pet = pet.Name });
        }
    }
}

using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DatabaseFirstLINQ.Models;
using System.Collections.Generic;

namespace DatabaseFirstLINQ
{
    class Problems
    {
        private ECommerceContext _context;

        public Problems()
        {
            _context = new ECommerceContext();
        }
        public void RunLINQQueries()
        {
            //ProblemOne();
            //ProblemTwo();
            //ProblemThree();
            //ProblemFour();
            //ProblemFive();
            //ProblemSix();
            //ProblemSeven();
            //ProblemEight();
            //ProblemNine();
            //ProblemTen();
            //ProblemEleven();
            //ProblemTwelve();
            //ProblemThirteen();
            //ProblemFourteen();
            //ProblemFifteen();
            //ProblemSixteen();
            //ProblemSeventeen();
            //ProblemEighteen();
            //ProblemNineteen();
            //ProblemTwenty();
            //BonusOne();
            //BonusTwo();
            BonusThree();
        }

        // <><><><><><><><> R Actions (Read) <><><><><><><><><>
        private void ProblemOne()
        {
            // Write a LINQ query that returns the number of users in the Users table.
            // HINT: .ToList().Count
            var numOfUsersInTable = _context.Users.Count();
        }

        private void ProblemTwo()
        {
            // Write a LINQ query that retrieves the users from the User tables then print each user's email to the console.
            var users = _context.Users;

            foreach (User user in users)
            {
                Console.WriteLine(user.Email);
            }

        }

        private void ProblemThree()
        {
            // Write a LINQ query that gets each product where the products price is greater than $150.
            // Then print the name and price of each product from the above query to the console.
            var productsWithTheirPrices = _context.Products.Where(product => product.Price > 150).Select(product => product);

            foreach(var product in productsWithTheirPrices)
            {
                Console.WriteLine($"Product name: {product.Name} - price = ${product.Price}");
            }

        }

        private void ProblemFour()
        {
            // Write a LINQ query that gets each product that contains an "s" in the products name.
            // Then print the name of each product from the above query to the console.
            var products = _context.Products.Where(p => p.Name.ToLower().Contains("s")).Select(p => p.Name);
            foreach (string name in products)
            {
                Console.WriteLine($"{name} contains an \"s\"");
            }

        }

        private void ProblemFive()
        {
            // Write a LINQ query that gets all of the users who registered BEFORE 2016
            // Then print each user's email and registration date to the console.
            var usersRegisteredBefore2016 = _context.Users.Where(user => user.RegistrationDate.Value.Year < 2016).Select(user => user);

            foreach (var user in usersRegisteredBefore2016)
            {
                Console.WriteLine($" UserEmail: {user.Email} , Registration Date {user.RegistrationDate}");
            }
        }

        private void ProblemSix()
        {
            // Write a LINQ query that gets all of the users who registered AFTER 2016 and BEFORE 2018
            // Then print each user's email and registration date to the console.
            var usersRegisteredBefore2016 = _context.Users.Where(user => user.RegistrationDate.Value.Year > 2016
                && user.RegistrationDate.Value.Year < 2018);

            foreach (var user in usersRegisteredBefore2016)
            {
                Console.WriteLine($" UserEmail: {user.Email} , Registration Date {user.RegistrationDate}");
            }
        }

        // <><><><><><><><> R Actions (Read) with Foreign Keys <><><><><><><><><>

        private void ProblemSeven()
        {
            // Write a LINQ query that retreives all of the users who are assigned to the role of Customer.
            // Then print the users email and role name to the console.
            var customerUsers = _context.UserRoles.Include(ur => ur.Role).Include(ur => ur.User).Where(ur => ur.Role.RoleName == "Customer");
            foreach (UserRole userRole in customerUsers)
            {
                Console.WriteLine($"Email: {userRole.User.Email} Role: {userRole.Role.RoleName}");
            }
        }

        private void ProblemEight()
        {
            // Write a LINQ query that retreives all of the products in the shopping cart of the user who has the email "afton@gmail.com".
            // Then print the product's name, price, and quantity to the console.
            int userAftonID = _context.Users.Where(user => user.Email == "afton@gmail.com").Select(user => user.Id).ToList()[0]; //user with email, shoppingcart
            var productsList = _context.Products.Join(_context.ShoppingCarts.Where(cart => cart.UserId == userAftonID), 
                product => product.Id, cart => cart.ProductId, (product, cart) => new { name = product.Name, price = product.Price, quantity = cart.Quantity });



            foreach (var product in productsList)
            {
                Console.WriteLine($" Product Name: {product.name} - Price = ${product.price} - Quantity = {product.quantity}");
            }
            // we have the list of shopping carts<carts> for each seperate productID, we want name and price of the product that matches the productID
            //
        }

        private void ProblemNine()
        {
            // Write a LINQ query that retreives all of the products in the shopping cart of the user who has the email "oda@gmail.com" and returns the sum of all of the products prices.
            // HINT: End of query will be: .Select(sc => sc.Product.Price).Sum();
            // Then print the total of the shopping cart to the console.
            int userOdaID = _context.Users.Where(user => user.Email == "oda@gmail.com").Select(user => user.Id).ToList()[0]; //user with email, shoppingcart
            var totalCost = _context.Products.Join(_context.ShoppingCarts.Where(cart => cart.UserId == userOdaID),
                product => product.Id, cart => cart.ProductId, (product, cart) => new { price = product.Price, quantity = cart.Quantity }).Select(sc => sc.price).Sum();
            Console.WriteLine($"Total price is ${totalCost}");
        }

        private void ProblemTen()
        {
            // Write a LINQ query that retreives all of the products in the shopping cart of users who have the role of "Employee".
            // Then print the user's email as well as the product's name, price, and quantity to the console.
            var employeeMatchingRoleID = _context.Roles.Where(role => role.RoleName == "Employee").Select(role => role.Id).ToList()[0];
            var employeeUserIDs = _context.UserRoles.Where(user => user.RoleId == employeeMatchingRoleID).Select(user => user.UserId).ToList();
            var productsList = _context.Products.Join(
                _context.ShoppingCarts.Join(_context.Users.Where(user => employeeUserIDs.Contains(user.Id)), c => c.UserId, user => user.Id,
                (c, user) => new { email = user.Email, Product = c.Product, ProductID = c.ProductId, Quantity = c.Quantity }),
                product => product.Id, cart => cart.Product.Id, (product, cart) => new { email = cart.email, name = product.Name, price = product.Price, quantity = cart.Quantity });

            foreach (var product in productsList)
            {
                Console.WriteLine($"User Email:{product.email} Product Name: {product.name} - Price = ${product.price} - Quantity = {product.quantity}");
            }
        }

        // <><><><><><><><> CUD (Create, Update, Delete) Actions <><><><><><><><><>

        // <><> C Actions (Create) <><>

        private void ProblemEleven()
        {
            // Create a new User object and add that user to the Users table using LINQ.
            User newUser = new User()
            {
                Email = "david@gmail.com",
                Password = "DavidsPass123"
            };
            _context.Users.Add(newUser);
            _context.SaveChanges();
        }

        private void ProblemTwelve()
        {
            // Create a new Product object and add that product to the Products table using LINQ.
            Product newProduct = new Product()
            {
                Name = "PlayStation 5",
                Description = "The gaming system that you cannot buy because it is bought out by scalpers.",
                Price = 600
            };
            _context.Products.Add(newProduct);
            _context.SaveChanges();

        }

        private void ProblemThirteen()
        {
            // Add the role of "Customer" to the user we just created in the UserRoles junction table using LINQ.
            var roleId = _context.Roles.Where(r => r.RoleName == "Customer").Select(r => r.Id).SingleOrDefault();
            var userId = _context.Users.Where(u => u.Email == "david@gmail.com").Select(u => u.Id).SingleOrDefault();
            UserRole newUserRole = new UserRole()
            {
                UserId = userId,
                RoleId = roleId
            };
            _context.UserRoles.Add(newUserRole);
            _context.SaveChanges();
        }

        private void ProblemFourteen()
        {
            // Add the product you create to the user we created in the ShoppingCart junction table using LINQ.
            var productId = _context.Products.Where(p => p.Name == "PlayStation 5").Select(p => p.Id).SingleOrDefault();
            var userId = _context.Users.Where(u => u.Email == "david@gmail.com").Select(u => u.Id).SingleOrDefault();
            ShoppingCart newShoppingCart = new ShoppingCart()
            {
                UserId = userId,
                ProductId = productId,
                Quantity = 1
            };
            _context.ShoppingCarts.Add(newShoppingCart);
            _context.SaveChanges();

        }

        // <><> U Actions (Update) <><>

        private void ProblemFifteen()
        {
            // Update the email of the user we created to "mike@gmail.com"
            var user = _context.Users.Where(u => u.Email == "david@gmail.com").SingleOrDefault();
            user.Email = "mike@gmail.com";
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        private void ProblemSixteen()
        {
            // Update the price of the product you created to something different using LINQ.
            var product = _context.Products.Where(p => p.Name == "PlayStation 5").SingleOrDefault();
            product.Price = 1200;
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        private void ProblemSeventeen()
        {
            // Change the role of the user we created to "Employee"
            // HINT: You need to delete the existing role relationship and then create a new UserRole object and add it to the UserRoles table
            // See problem eighteen as an example of removing a role relationship
            var userRole = _context.UserRoles.Include(ur => ur.User).Where(ur => ur.User.Email == "mike@gmail.com").SingleOrDefault();
            _context.UserRoles.Remove(userRole);
            UserRole newUserRole = new UserRole()
            {
                UserId = _context.Users.Where(u => u.Email == "mike@gmail.com").Select(u => u.Id).SingleOrDefault(),
                RoleId = _context.Roles.Where(r => r.RoleName == "Employee").Select(r => r.Id).SingleOrDefault()
            };
            _context.UserRoles.Add(newUserRole);
            _context.SaveChanges();
        }

        // <><> D Actions (Delete) <><>

        private void ProblemEighteen()
        {
            // Delete the role relationship from the user who has the email "oda@gmail.com" using LINQ.
            var userRole = _context.UserRoles.Include(ur => ur.User).Where(ur => ur.User.Email == "oda@gmail.com").SingleOrDefault();
            _context.UserRoles.Remove(userRole);
            _context.SaveChanges();
        }

        private void ProblemNineteen()
        {
            // Delete all of the product relationships to the user with the email "oda@gmail.com" in the ShoppingCart table using LINQ.
            // HINT: Loop
            var shoppingCartProducts = _context.ShoppingCarts.Include(sc => sc.User).Where(sc => sc.User.Email == "oda@gmail.com");
            foreach (ShoppingCart userProductRelationship in shoppingCartProducts)
            {
                _context.ShoppingCarts.Remove(userProductRelationship);
            }
            _context.SaveChanges();
        }

        private void ProblemTwenty()
        {
            // Delete the user with the email "oda@gmail.com" from the Users table using LINQ.
            var user = _context.Users.Where(ur => ur.Email == "oda@gmail.com").SingleOrDefault();
            _context.Users.Remove(user);
            _context.SaveChanges();

        }

        // <><><><><><><><> BONUS PROBLEMS <><><><><><><><><>

        private void BonusOne()
        {
            // Prompt the user to enter in an email and password through the console.
            // Take the email and password and check if the there is a person that matches that combination.
            // Print "Signed In!" to the console if they exists and the values match otherwise print "Invalid Email or Password.".
            string userEmailInput, userPasswordInput;

            Console.WriteLine("Enter your email below: ");
            userEmailInput = Console.ReadLine();
            Console.WriteLine("Password: ");
            userPasswordInput = Console.ReadLine();

            var userMatch = _context.Users.Where(user => user.Email.Equals(userEmailInput) && user.Password.Equals(userPasswordInput)).ToList();
            
            if (userMatch.Count == 0)
            {
                Console.WriteLine("\n Invalid Email or Password.");
                BonusOne();
            }
            else
            {
                Console.WriteLine("Signed In!");
            }
        }

        private void BonusTwo()
        {
            // Write a query that finds the total of every users shopping cart products using LINQ.
            // Display the total of each users shopping cart as well as the total of the totals to the console.
            var userCartPairs = _context.ShoppingCarts.Include(cart => cart.User).Include(cart => cart.Product).Select(
                cart => new { Email = cart.User.Email, Total = Convert.ToInt32(cart.Quantity.GetValueOrDefault()) * Convert.ToInt32(cart.Product.Price)}).ToList();
            var groupByUser = userCartPairs.GroupBy(user => user.Email, user => user.Total, (person, products) => new {PersonEmail = person, Products = products.Sum()});
            int total = 0;
            foreach (var pair in groupByUser)
            {
                Console.WriteLine($"{pair.PersonEmail}'s total: ${pair.Products}");
                total += pair.Products;
            }
            Console.WriteLine($"Total for every user: ${total}");
        }

        // BIG ONE
        private void BonusThree()
        {
            var userMatch = SignIn();
            // 2. If the user succesfully signs in
            // a. Give them a menu where they perform the following actions within the console
            // View the products in their shopping cart
            // View all products in the Products table
            // Add a product to the shopping cart (incrementing quantity if that product is already in their shopping cart)
            // Remove a product from their shopping cart

            // 3. If the user does not succesfully sing in
            // a. Display "Invalid Email or Password"
            // b. Re-prompt the user for credentials
            MainLoop(userMatch);
            
        }

        public void MainLoop(User user)
        {
            bool done = false;
            string userMenuChoice;
            while (!done)
            {
                Console.WriteLine("Enter the following number for the action wanted;");
                Console.WriteLine("1: View the products in your shopping cart");
                Console.WriteLine("2: View products available");
                Console.WriteLine("0: EXIT");
                userMenuChoice = Console.ReadLine();

                switch (userMenuChoice)
                {
                    case "0":
                        done = true;
                        break;
                    case "1":
                        ShoppingCartMenu(user);
                        break;
                    case "2":
                        ProductListMenu(user);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again");
                        break;
                }
            }
        }

        public bool ProductIDExists(int targetProductID)
        {
            return _context.Products.Where(p => p.Id == targetProductID).ToList().Count > 0;
        }

        public void ShoppingCartMenu(User user)
        {
            var productsList = _context.Products.Join(_context.ShoppingCarts.Where(cart => cart.UserId == user.Id),
                    product => product.Id, cart => cart.Product.Id, (product, cart) => new { pid = cart.ProductId, name = product.Name, price = product.Price, quantity = cart.Quantity });
            Console.WriteLine("The products in your cart are;");
            foreach (var product in productsList)
            {
                Console.WriteLine($"{product.pid}: Product Name: {product.name} - Price = ${product.price} - Quantity = {product.quantity}");
            }

            string yesOrNo = "";
            do
            {
                Console.WriteLine("Would you like to remove an item? (y/n)");
                yesOrNo = Console.ReadLine();
            } while (yesOrNo != "y" && yesOrNo != "n");
            
            if (yesOrNo == "y")
            {
                Console.WriteLine("Please enter the product id to remove, or 0 to go back to previous step");
                string userProductInput = Console.ReadLine();
                int productIDNumber = -1;

                if (userProductInput == "0")
                {
                    return;
                }
                else
                {
                    if (Int32.TryParse(userProductInput, out productIDNumber))
                    {
                        if (ProductIDExists(productIDNumber))
                        {
                            ModifyCart(user.Id, productIDNumber, false);
                        }
                        else
                        {
                            Console.WriteLine("Product does not exist. Please try again");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input, please try again");
                    }
                }
            }
        }

        public void ProductListMenu(User user)
        {
            Console.WriteLine("The products available to purchase are;");
            var productsWithTheirPrices = _context.Products.Select(product => product);

            foreach (var product in productsWithTheirPrices)
            {
                Console.WriteLine($"{product.Id}: Product name: {product.Name} - price = ${product.Price}");
            }
            Console.WriteLine("Type the item number you want to add: (Enter 0 to EXIT)");
            string userProductInput = Console.ReadLine();
            int productIDNumber = -1;

            if (userProductInput == "0")
            {
                return;
            }
            else
            {
                if (Int32.TryParse(userProductInput, out productIDNumber))
                {
                    if (ProductIDExists(productIDNumber))
                    {
                        ModifyCart(user.Id, productIDNumber);
                    }
                    else
                    {
                        Console.WriteLine("Invalid input, please try again");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input, please try again");
                }
            }
        }

        public User SignIn()
        {
            string userEmailInput, userPasswordInput;

            Console.WriteLine("Enter your email below: ");
            userEmailInput = Console.ReadLine();
            Console.WriteLine("Password: ");
            userPasswordInput = Console.ReadLine();

            var userMatch = _context.Users.Where(user => user.Email.Equals(userEmailInput) && user.Password.Equals(userPasswordInput)).ToList();

            if (userMatch.Count == 0)
            {
                Console.WriteLine("\n Invalid Email or Password.");
                return SignIn();
            }
            else
            {
                Console.WriteLine("Signed In!");
                return userMatch[0];
            }
        }
        public void ModifyCart(int userID, int productID, bool addOne=true)
        {
            var productToAdd = _context.ShoppingCarts.Where(cart => cart.UserId == userID && cart.ProductId == productID).ToList();

            if (productToAdd.Count == 0 && addOne)
            {
                ShoppingCart newShoppingCart = new ShoppingCart()
                {
                    UserId = userID,
                    ProductId = productID,
                    Quantity = 1
                };
                _context.ShoppingCarts.Add(newShoppingCart);
                _context.SaveChanges();
                Console.WriteLine("Product successfully added to the shopping cart");
                
            }
            else if (addOne || productToAdd.Count > 0)
            {

                int newQuantity = RemoveProductFromCart(userID, productID, addOne);

                ShoppingCart existingShoppingCart = new ShoppingCart()
                {
                    UserId = userID,
                    ProductId = productID,
                    Quantity = newQuantity
                };
                if (addOne)
                {
                    _context.ShoppingCarts.Add(existingShoppingCart);
                    Console.WriteLine("Product successfully added to shopping cart");
                }
                else
                {
                    if (newQuantity > 0)
                    {
                        _context.ShoppingCarts.Add(existingShoppingCart);
                    }
                    Console.WriteLine("Product removed from shopping cart");
                }
                _context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Product not in shopping part");
            }

        }
        public int RemoveProductFromCart(int userID, int productID, bool add = false)
        {
            List<ShoppingCart> productToRemove = _context.ShoppingCarts.Where(cart => cart.UserId == userID && cart.ProductId == productID).ToList();
            int originalQuantity = productToRemove[0].Quantity.GetValueOrDefault();
            _context.ShoppingCarts.Remove(productToRemove[0]);

            if (add == true)
            {
                originalQuantity++;
                return originalQuantity;
            }
            originalQuantity--;
            return originalQuantity;
        }
    }
}

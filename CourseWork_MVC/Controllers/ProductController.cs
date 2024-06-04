using CourseWork_MVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace courseWork_MVC.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        private readonly string Baseurl = "http://ec2-16-171-36-122.eu-north-1.compute.amazonaws.com/";
        public async Task<ActionResult> Index()
        {
            List<Product> ProdInfo = new();
            using var client = new HttpClient();
            //Passing service base url
            client.BaseAddress = new Uri(Baseurl);
            client.DefaultRequestHeaders.Clear();

            //Define request data format
            client.DefaultRequestHeaders.Accept.Add(new
            MediaTypeWithQualityHeaderValue("application/json"));

            //Sending request to find web api REST service resource using HttpClient
            HttpResponseMessage Res = await client.GetAsync("api/Product");

            //Checking the response is successful or not which is sent HttpClient
            if (Res.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api
                var PrResponse = Res.Content.ReadAsStringAsync().Result;

                //Deserializing the response recieved from web api and storing the Product list
                ProdInfo = JsonConvert.DeserializeObject<List<Product>>(PrResponse);
            }
            //returning the Product list to view
            return View(ProdInfo);
        }

        // GET: Product/Details/5
        public async Task<ActionResult> Details(int id)
        {

            Product product = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("api/Product/" + id); // Add 'id' for single product
                if (Res.IsSuccessStatusCode)
                {
                    var PrResponse = Res.Content.ReadAsStringAsync().Result;
                    product = JsonConvert.DeserializeObject<Product>(PrResponse);
                }
            }

            if (product == null)
            {
                return HttpNotFound(); // Or another way to handle a non-existent product  
            }

            return View(product);
        }

        private ActionResult HttpNotFound()
        {
            throw new NotImplementedException();
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            var product = new Product();
            return View(product);
        }

        // POST: Product/Create
        [HttpPost]
        public async Task<ActionResult> Create(Product product)
        {
            if (!ModelState.IsValid) // Basic validation
            {
                return View(product); // Return to form if invalid
            }

            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(Baseurl);

                // Prepare product data as JSON
                var content = new StringContent(JsonConvert.SerializeObject(product), System.Text.Encoding.UTF8, "application/json");

                // Send the POST request to the API
                HttpResponseMessage Res = await client.PostAsync("api/Product", content);

                if (Res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    // Handle API error (log it, show user a message)
                    // For now, redisplay the form
                    return View(product);
                }
            }
            catch (Exception ex)
            {
                // Log the exception and handle it gracefully
                ModelState.AddModelError("", "Error creating product: " + ex.Message); // Simplified error display
                return View(product);
            }
        }

        // GET: Product/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Get the product from the API
                HttpResponseMessage Res = await client.GetAsync($"api/Product/{id}");

                if (Res.IsSuccessStatusCode)
                {
                    var PrResponse = Res.Content.ReadAsStringAsync().Result;
                    Product product = JsonConvert.DeserializeObject<Product>(PrResponse);
                    return View(product);
                }
                else
                {
                    // Handle API error (log it, show user a message)
                    return RedirectToAction("Index"); // Or show an error page
                }
            }
            catch (Exception ex)
            {
                // Log the exception and handle it gracefully
                ModelState.AddModelError("", "Error fetching product: " + ex.Message); // Simplified error display
                return View();
            }
        }

        // POST: Product/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product); // Return to form if invalid
            }

            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(Baseurl);

                // Prepare product data as JSON
                var content = new StringContent(JsonConvert.SerializeObject(product), System.Text.Encoding.UTF8, "application/json");

                // Send the PUT request to the API
                HttpResponseMessage Res = await client.PutAsync($"api/Product/{id}", content);

                if (Res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    // Handle API error (log it, show user a message)
                    ModelState.AddModelError("", "Error updating product."); // Simplified error display
                    return View(product);
                }
            }
            catch (Exception ex)
            {
                // Log the exception and handle it gracefully
                ModelState.AddModelError("", "Error updating product: " + ex.Message); // Simplified error display
                return View(product);
            }
        }



        // GET: Product/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Get the product from the API
                HttpResponseMessage Res = await client.GetAsync($"api/Product/{id}");

                if (Res.IsSuccessStatusCode)
                {
                    var PrResponse = Res.Content.ReadAsStringAsync().Result;
                    Product product = JsonConvert.DeserializeObject<Product>(PrResponse);
                    return View(product); // Show confirmation view with product details
                }
                else
                {
                    // Handle API error (log it, show user a message)
                    return RedirectToAction("Index"); // Or show an error page
                }
            }
            catch (Exception ex)
            {
                // Log the exception and handle it gracefully
                ModelState.AddModelError("", "Error fetching product: " + ex.Message);
                return View();
            }
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(Baseurl);

                // Send the DELETE request to the API
                HttpResponseMessage Res = await client.DeleteAsync($"api/Product/{id}");

                if (Res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    // Handle API error (log it, show user a message)
                    ModelState.AddModelError("", "Error deleting product."); // Simplified error display
                    return View(); // You might want to redirect to an error page instead
                }
            }
            catch (Exception ex)
            {
                // Log the exception and handle it gracefully
                ModelState.AddModelError("", "Error deleting product: " + ex.Message);
                return View(); // You might want to redirect to an error page instead
            }
        }
    }
}

using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace JsonPlaceholderApiTests
{
    //    Write a test in C#/Java for the following REST API
    //URL: https://jsonplaceholder.typicode.com/

    //Requirements:
    //1. Get a random user(userID), print out its email address to console.
    //2. Using this userID, get this user’s associated posts and verify they contain valid Post IDs (an Integer between 1 and 100) and print the title and the id for each post to the console.
    //3. By using a random post id from the previous step, modify the title for the post and print the id and title from the response to the console.
    //4. Do a post using the same userID with a non-empty title and body, verify the correct response is returned(since this is a mock API, it might not return Response code 200, so check the documentation - https://jsonplaceholder.typicode.com/).
    public class ApiTests
    {
        private HttpClient _client;
        private const string BaseUrl = "https://jsonplaceholder.typicode.com/";

        [SetUp]
        public void Setup()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [Test]
        public async Task JsonPlaceholderApiTest()
        {
            // Step 1: Get a random user and print their email address
            var usersResponse = await _client.GetAsync("users");
            usersResponse.EnsureSuccessStatusCode();
            var usersContent = await usersResponse.Content.ReadAsStringAsync();
            var users = JArray.Parse(usersContent);

            var randomUser = users[new Random().Next(users.Count)];
            var userId = randomUser["id"].Value<int>();
            var userEmail = randomUser["email"].Value<string>();
            Console.WriteLine($"User ID: {userId}, Email: {userEmail}");

            // Step 2: Get the user's posts and verify Post IDs, print title and ID
            var postsResponse = await _client.GetAsync($"posts?userId={userId}");
            postsResponse.EnsureSuccessStatusCode();
            var postsContent = await postsResponse.Content.ReadAsStringAsync();
            var posts = JArray.Parse(postsContent);

            foreach (var post in posts)
            {
                int postId = post["id"].Value<int>();
                Assert.That(postId, Is.InRange(1, 100), $"Post ID {postId} is not in the range 1-100");
                Console.WriteLine($"Post ID: {postId}, Title: {post["title"].Value<string>()}");
            }

            // Step 3: Modify the title of a random post
            var randomPost = posts[new Random().Next(posts.Count)];
            var postIdToUpdate = randomPost["id"].Value<int>();
            var newTitle = "Updated Title";
            var updateContent = new StringContent(
                $"{{\"title\": \"{newTitle}\"}}",
                System.Text.Encoding.UTF8,
                "application/json");

            var updateResponse = await _client.PutAsync($"posts/{postIdToUpdate}", updateContent);
            updateResponse.EnsureSuccessStatusCode();
            var updatedPostContent = await updateResponse.Content.ReadAsStringAsync();
            var updatedPost = JObject.Parse(updatedPostContent);

            Console.WriteLine($"Updated Post ID: {updatedPost["id"].Value<int>()}, Title: {updatedPost["title"].Value<string>()}");

            // Step 4: Create a new post for the user and verify the response
            var newPostContent = new StringContent(
                $"{{\"userId\": {userId}, \"title\": \"New Post Title\", \"body\": \"This is a new post body\"}}",
                System.Text.Encoding.UTF8,
                "application/json");

            var createResponse = await _client.PostAsync("posts", newPostContent);
            Assert.AreEqual(System.Net.HttpStatusCode.Created, createResponse.StatusCode, "Expected status code 201 (Created) for a new post.");

            var createdPostContent = await createResponse.Content.ReadAsStringAsync();
            var createdPost = JObject.Parse(createdPostContent);
            Console.WriteLine($"Created Post ID: {createdPost["id"].Value<int>()}, Title: {createdPost["title"].Value<string>()}, Body: {createdPost["body"].Value<string>()}");
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
        }
    }
}
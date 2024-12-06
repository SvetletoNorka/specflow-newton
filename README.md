# Automate the following using C#/Java and Selenium:

1. Navigate to “http://blankfactor.com”. Accept our policy.
2. Open Insights menu and then click on “Blog” section
3. Scroll down to the bottom and continue scrolling until the article with title “Why Fintech in Latin America Is Booming” is displayed and click the post by Sofia Gonzalez
4. Make validation (that the user is on the correct page, by verifying URL, some of the text on the page)
5. Subscribe to the newsletter using the subscribe form
6. Go back to the Blog section and print a list of the all posts titles and their links.

# Write a test in C#/Java for the following REST API
URL: https://jsonplaceholder.typicode.com/

Requirements:
1. Get a random user (userID), print out its email address to console.
2. Using this userID, get this user’s associated posts and verify they contain valid Post IDs (an Integer between 1 and 100) and print the title and the id for each post to the console.
3. By using a random post id from the previous step, modify the title for the post and print the id and title from the response to the console.
4. Do a post using the same userID with a non-empty title and body, verify the correct response is returned (since this is a mock API, it might not return Response code 200, so check the documentation - https://jsonplaceholder.typicode.com/).

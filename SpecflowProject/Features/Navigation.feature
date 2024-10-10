Feature: Navigation
  1. Navigate to “http://blankfactor.com”. Accept our policy.
2. Open Insights menu and then click on “Blog” section
3. Scroll down to the bottom and continue scrolling until the article with title “Why Fintech in Latin America Is Booming” is displayed and click the post by Sofia Gonzalez
4. Make validation (that the user is on the correct page, by verifying URL, some of the text on the page)
5. Subscribe to the newsletter using the subscribe form
6. Go back to the Blog section and print a list of the all posts titles and their links.
  
  Scenario: Navigates to Blog and Subscribes
    Given I Navigate to BlankFactor page
        And I accept the policy
    When I click on Insights menu
        And I click on Blog menu
        And I scrolls down to the end of the page with posts
        And I sroll up to article "Why fintech in Latin America is booming"
        And I click on the article "Why fintech in Latin America is booming"
    Then I verify the URL = "https://blankfactor.com/insights/blog/fintech-in-latin-america/" of the page
        And I verify the post date = "August 13, 2021 - 10:04 am"
        And I subscribe to the newsletter
    When I go back
    Then I print all post titles and their links

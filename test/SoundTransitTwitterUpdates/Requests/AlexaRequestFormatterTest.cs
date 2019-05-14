using System;
using SoundTransitTwitterUpdates.Requests;
using Xunit;

namespace SoundTransitTwitterUpdates.Tests
{
    public class AlexaRequestFormatterTest
    {
        /// <summary>
        /// Tests that URL's get removed.
        /// </summary>
        [Fact]
        public void Format_HandlesExampleTweetWithUrlAtEnd_True()
        {
            // Arrange
            var exampleText = "Rider Alert:  Sounder south line # 1509 (3:15p Sea dep) delayed approx. 10 mins departing https://t.co/kgvCgWJMLm";
            var formatter = new AlexaRequestFormatter(exampleText);
            string resultText = null;

            // Act
            resultText = formatter.Format();

            // Assert
            Assert.Contains("15 09", resultText);
            Assert.Contains("Seattle", resultText);
            Assert.Contains("departure", resultText);
            Assert.DoesNotContain("http", resultText);
        }

        /// <summary>
        /// Tests that requests with the word 'Sea' at the end become 'Seattle'
        /// </summary>
        [Fact]
        public void Format_HandlesExampleTweetWithSeaAtEnd_True()
        {
            // Arrange
            var exampleText = "Rider Alert:  Sounder south line # 1509 3:15p Sea";
            var formatter = new AlexaRequestFormatter(exampleText);
            string resultText = null;

            // Act
            resultText = formatter.Format();

            // Assert
            Assert.Contains("15 09", resultText);
            Assert.Contains("Seattle", resultText);
        }

        /// <summary>
        /// Tests that requests with the work 'Dep)' at the end become 'Departure)'
        /// </summary>
        [Fact]
        public void Format_HandlesExampleTweetWithDepAtEnd_True()
        {
            // Arrange
            var exampleText = "Rider Alert:  Sounder south line # 1509 (3:15p Sea dep)";
            var formatter = new AlexaRequestFormatter(exampleText);
            string resultText = null;

            // Act
            resultText = formatter.Format();

            // Assert
            Assert.Contains("15 09", resultText);
            Assert.Contains("Seattle", resultText);
            Assert.Contains("departure)", resultText);
        }

        /// <summary>
        /// Tests that numbers at the end, result in numbers being spaced out by groups no larger than 2
        /// </summary>
        [Fact]
        public void Format_HandlesExampleTweetWithNumericAtEnd_True()
        {
            // Arrange
            var exampleText = "Rider Alert:  Sounder south line # 1509";
            var formatter = new AlexaRequestFormatter(exampleText);
            string resultText = null;

            // Act
            resultText = formatter.Format();

            // Assert
            Assert.Contains("15 09", resultText);
        }

        /// <summary>
        /// Tests that the formatter spaces out numbers in groups no larger than 2, switches the word
        /// 'Sea' to 'Seatle' and switches 'dep)' to 'departure)'
        /// </summary>
        [Fact]
        public void Format_HandlesExampleTweet_True()
        {
            // Arrange
            var exampleText = "Rider Alert:  Sounder south line # 1509 (3:15p Sea dep)";
            var formatter = new AlexaRequestFormatter(exampleText);
            string resultText = null;

            // Act
            resultText = formatter.Format();

            // Assert
            Assert.Contains("15 09", resultText);
            Assert.Contains("Seattle", resultText);
            Assert.Contains("departure)", resultText);
        }

        /// <summary>
        /// Tests that a tweet comprised most of a URL get's truncated.
        /// </summary>
        [Fact]
        public void Format_HandlesOnlyUrl_True()
        {
            // Arrange
            var exampleText = "TeeHee https://t.co/kgvCgWJMLm";
            var formatter = new AlexaRequestFormatter(exampleText);
            string resultText = null;

            // Act
            resultText = formatter.Format();

            // Assert
            Assert.True(resultText.Length <= 7);
        }

        /// <summary>
        /// Tests that a tweet can mention 'http' without triggering actions reserved for hyper links.
        /// </summary>
        [Fact]
        public void Format_HandlesHttpMention_True()
        {
            // Arrange
            var exampleText = "Rider Alert:  Sounder south line # 1509 (3:15p Sea dep) delayed approx. 10 mins departing. http client is for rest calls";
            var formatter = new AlexaRequestFormatter(exampleText);
            string resultText = null;

            // Act
            resultText = formatter.Format();

            // Assert
            Assert.Contains("http", resultText);
        }

        /// <summary>
        /// Tests that the formatter can handle a URL in the middle of a tweet.
        /// </summary>
        [Fact]
        public void Format_HandlesExampleTweetWithUrlInMiddle()
        {
            // Arrange
            var exampleText = "Rider Alert:  Sounder south line # 1509 (3:15p Sea dep) delayed approx. 10 mins departing https://t.co/kgvCgWJMLm, also I hate fornite";
            var formatter = new AlexaRequestFormatter(exampleText);
            string resultText = null;

            // Act
            resultText = formatter.Format();

            // Assert
            Assert.DoesNotContain("https", resultText);
        }
    }
}

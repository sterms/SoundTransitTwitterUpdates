using System;
using System.Collections.Generic;
using System.IO;
using SoundTransitTwitterUpdates.Utilities;
using Newtonsoft.Json;
using Xunit;

namespace SoundTransitTwitterUpdates.Tests
{
    public class LambdaRestSerializerTest
    {
        private LambdaRestSerializer serializer;

        public LambdaRestSerializerTest() => serializer = new LambdaRestSerializer();    

        /// <summary>
        /// Tests that Serializing on complex type, with an internal class, throws no error.
        /// </summary>
        [Fact]
        public void Serialize_ThrowsNoErrorOnComplexType_True()
        {
            // Arrange
            var testObject = new TestClass()
            {
                TestMemberOne = "Pizza",
                TestMemberTwo = 53,
                TestMemberThree = new InternalTestClass()
                {
                    TestMemberOne = "Gumbo"
                }
            };
            Exception exception = null;

            // Act
            try 
            {
                serializer.Serialize<TestClass>(testObject, new MemoryStream());
            }
            catch (Exception e)
            {
                exception = e;
            }

            // Assert
            Assert.Null(exception);
        }

        /// <summary>
        /// Tests that deserializing a complex type, with an internal class, throws no error.
        /// </summary>
        [Fact]
        public void Deserialize_ThrowsNoErrorOnComplexType_True()
        {
            // Arrange
            var testObject = new TestClass()
            {
                TestMemberOne = "Pizza",
                TestMemberTwo = 53,
                TestMemberThree = new InternalTestClass()
                {
                    TestMemberOne = "Gumbo"
                }
            };
            var stream = new MemoryStream();
            TestClass result = null;

            // Act
            serializer.Serialize<TestClass>(testObject, stream);
            stream.Position = 0;
            result = serializer.Deserialize<TestClass>(stream);

            // Assert
            Assert.Equal(testObject.TestMemberOne, result.TestMemberOne);
            Assert.Equal(testObject.TestMemberTwo, result.TestMemberTwo);
            Assert.Equal(testObject.TestMemberThree.TestMemberOne, result.TestMemberThree.TestMemberOne);
        }

        /// <summary>
        /// Tests Serializing with a primitive type throws no error.
        /// </summary>
        [Fact]
        public void Serialize_ThrowsNoErrorOnSimpleType_True()
        {
            // Arrange
            var testObject = "Hello Man!";
            Exception exception = null;

            // Act
            try
            {
                serializer.Serialize<string>(testObject, new MemoryStream());
            }
            catch (Exception e)
            {
                exception = e;
            }

            // Assert
            Assert.Null(exception);
        }

        /// <summary>
        /// Tests that deserializing a primitive type throws no error.
        /// </summary>
        [Fact]
        public void Deserialize_ThrowsNoErrorOnSimpleType_True()
        {
            // Arrange
            var testObject = "Huey Lewis and The News";
            var stream = new MemoryStream();
            string result = null;

            // Act
            serializer.Serialize<string>(testObject, stream);
            stream.Position = 0;
            result = serializer.Deserialize<string>(stream);

            // Assert
            Assert.Equal(testObject, result);
        }

        /// <summary>
        /// Tests that serializing converts propertis to camelCase
        /// </summary>
        [Fact]
        public void Serialize_ContainsCamelCase_True()
        {
            // Arrange
            var testObject = new TestClass()
            {
                TestMemberOne = "Pizza",
                TestMemberTwo = 53,
                TestMemberThree = new InternalTestClass()
                {
                    TestMemberOne = "Gumbo"
                }
            };
            var stream = new MemoryStream();
            string result = null;

            // Act
            serializer.Serialize<TestClass>(testObject, stream);
            stream.Position = 0;
            result = new StreamReader(stream).ReadToEnd();

            // Assert
            Assert.Contains("testMemberOne", result);
        }

        /// <summary>
        /// Tests that serializing includes all expected properties.
        /// </summary>
        [Fact]
        public void Serialize_GetsAllPropertiesAndValues_True()
        {
            // Arrange
            var testObject = new TestClass()
            {
                TestMemberOne = "Pizza",
                TestMemberTwo = 53,
                TestMemberThree = new InternalTestClass()
                {
                    TestMemberOne = "Gumbo"
                }
            };
            var stream = new MemoryStream();
            string result = null;

            // Act
            serializer.Serialize<TestClass>(testObject, stream);
            stream.Position = 0;
            result = new StreamReader(stream).ReadToEnd();

            // Assert
            Assert.Contains("testMemberOne", result);
            Assert.Contains("Pizza", result);
            Assert.Contains("testMemberTwo", result);
            Assert.Contains("53", result);
            Assert.Contains("testMemberThree", result);
            Assert.Contains("Gumbo", result);
        }

        private class TestClass
        {
            public string TestMemberOne { get; set; }

            public int TestMemberTwo { get; set; }

            public InternalTestClass TestMemberThree { get; set; }
        }

        private class InternalTestClass
        {
            public string TestMemberOne { get; set; }
        }
    }
}

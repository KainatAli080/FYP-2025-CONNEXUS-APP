using NUnit.Framework;
using UnityEngine;
using MyUtils;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.TestTools;

[TestFixture]
public class InputVaildationScript
{
    private UtilFunctionality util; // for my other logical functions besides validation
    private ValidationFunctions validator;
    private int maxLength = 25;

    [SetUp]
    public void Setup()
    {
        util = new UtilFunctionality();
        validator = new ValidationFunctions();
    }

    // -------------------------------------------------------------------------------- //
    //                                  EMAIL TESTS                                     //
    // -------------------------------------------------------------------------------- //

    [Test]
    public void TestValidEmail()
    {
        Assert.IsTrue(validator.isValidEmail("test@example.com", maxLength));
    }

    [Test]
    public void TestInvalidEmail_NoAtSymbol()
    {
        Assert.IsFalse(validator.isValidEmail("testexample.com", maxLength));
    }

    [Test]
    public void TestInvalidEmail_EmptyString()
    {
        Assert.IsFalse(validator.isValidEmail("", maxLength));
    }

    [Test]
    public void TestInvalidEmail_TooLong()
    {
        var longEmail = new string('a', 30) + "@example.com";
        Assert.IsFalse(validator.isValidEmail(longEmail, maxLength));
    }

    // -------------------------------------------------------------------------------- //
    //                                  PASSWORD TESTS                                  //
    // -------------------------------------------------------------------------------- //

    [Test]
    public void TestInvalidPassword_RemovesSpecialCharacters()
    {
        // Arrange
        var input = "P@ssw0rd!";
        var expected = "Pssw0rd";

        // Act
        var result = validator.isValidPassword(input, maxLength);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void TestInvalidPassword_TruncatesWhenExceedingMaxLength()
    {
        // Arrange
        var input = "P@ssw0rd!123456789000000000eee";
        var expected = "Pssw0rd123456789000000000";

        // Act
        var result = validator.isValidPassword(input, maxLength);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void TestValidPassword_ReturnsUnchangedWhenWithinLimit()
    {
        // Arrange
        var input = "Password123";
        var expected = "Password123";

        // Act
        var result = validator.isValidPassword(input, maxLength);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void TestInvalidPassword_HandlesEmptyInput()
    {
        // Arrange
        var input = "";
        var expected = "";

        // Act
        var result = validator.isValidPassword(input, maxLength);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void TestInvalidPassword_HandlesInputWithOnlySpecialCharacters()
    {
        // Arrange
        var input = "@#$%^&*()";
        var expected = "";

        // Act
        var result = validator.isValidPassword(input, maxLength);

        // Assert
        Assert.AreEqual(expected, result);
    }

    // -------------------------------------------------------------------------------- //
    //                                  USERNAME TESTS                                  //
    // -------------------------------------------------------------------------------- //

    [Test]
    public void TestInvalidUsername_RemovesInvalidCharacters()
    {
        // Arrange
        var input = "Valid@User#Name";
        var expected = "ValidUserName";

        // Act
        var result = validator.isValidUsername(input, maxLength);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void TestInvalidUsername_TruncatesWhenExceedingMaxLength()
    {
        // Arrange
        var input = "ValidUserName123456789012345";
        var expected = "ValidUserName123456789012"; // Truncated to maxLength

        // Act
        var result = validator.isValidUsername(input, maxLength);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void TestValidUsername_ReturnsUnchangedWhenWithinLimit()
    {
        // Arrange
        var input = "ValidUserName";
        var expected = "ValidUserName";

        // Act
        var result = validator.isValidUsername(input, maxLength);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void TestInvalidUsername_HandlesEmptyInput()
    {
        // Arrange
        var input = "";
        var expected = ""; // Empty input should remain empty

        // Act
        var result = validator.isValidUsername(input, maxLength);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void TestInvalidUsername_HandlesOnlyInvalidCharacters()
    {
        // Arrange
        var input = "@#$%^&*()!";
        var expected = ""; // Invalid characters should be removed

        // Act
        var result = validator.isValidUsername(input, maxLength);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void TestValidUsername_HandlesExactMaxLength()
    {
        // Arrange
        var input = "ValidUserName123456789012"; // Exactly 25 characters
        var expected = "ValidUserName123456789012";

        // Act
        var result = validator.isValidUsername(input, maxLength);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void TestInvalidUsername_HandlesWhitespaceInInput()
    {
        // Arrange
        var input = "User Name With Spaces";
        var expected = "UserNameWithSpaces"; // Spaces should be removed

        // Act
        var result = validator.isValidUsername(input, maxLength);

        // Assert
        Assert.AreEqual(expected, result);
    }

    // -------------------------------------------------------------------------------- //
    //                             INTROVERSION SCORE TESTS                             //
    // -------------------------------------------------------------------------------- //

    [Test]
    public void CalculateIntroversionScore_ValidInput_ReturnsCorrectScore()
    {
        int[] answers = new int[] { 0, 0, 2, 3, 4, 5, 1 }; // 2+3+4+5+1 = 15
        List<int> multiAnswers = new List<int> { 1, 2 };  // Sum = 3
        int expectedScore = 15 + 3;

        int result = util.calculateIntroversionScore(answers, multiAnswers);

        Assert.AreEqual(expectedScore, result);
    }

    [Test]
    public void CalculateIntroversionScore_NullAnswers_ReturnsMinusOne()
    {
        // Arrange
        List<int> multiAnswers = new List<int> { 1, 2 };

        // Expect an error log
        LogAssert.Expect(LogType.Error, "Answers are empty.");

        // Act
        int result = util.calculateIntroversionScore(null, multiAnswers);
        TestContext.WriteLine($"Result for null answers: {result}");

        // Assert
        Assert.AreEqual(-1, result);
    }


    // -------------------------------------------------------------------------------- //
    //                              CONFIDENCE SCORE TESTS                              //
    // -------------------------------------------------------------------------------- //

    [Test]
    public void CalculateConfidencePercentage_ValidInput_ReturnsCorrectAverage()
    {
        float previous = 80f; // percent
        float current = 90f;
        int totalPrev = 4;

        // Old total = 80*4 = 320
        // New total = 320+90 = 410
        // New avg = 410 / 5 = 82

        float result = util.calculateConfidencePercentage(previous, current, totalPrev);

        Assert.AreEqual(82f, result, 0.001f);
    }


    // -------------------------------------------------------------------------------- //
    //                                CLARITY SCORE TESTS                               //
    // -------------------------------------------------------------------------------- //

    [Test]
    public void CalculateClarityPercentage_ZeroPrevious_ReturnsCurrentAsPercentage()
    {
        float current = 8.5f;

        float result = util.calculateClarityPercentage(0f, current, 0);

        Assert.AreEqual(85f, result, 0.001f);
    }

    [Test]
    public void CalculateClarityPercentage_ValidInput_ReturnsCorrectPercentage()
    {
        float previous = 70f;  // = avg of 7
        float current = 9f;
        int totalPrev = 3;

        // previous = 70% = 7.0 avg
        // old total = 7.0 * 3 = 21
        // new total = 21 + 9 = 30
        // avg = 30 / 4 = 7.5
        // percent = 7.5 * 10 = 75%

        float result = util.calculateClarityPercentage(previous, current, totalPrev);

        Assert.AreEqual(75f, result, 0.001f);
    }


    // -------------------------------------------------------------------------------- //
    //                                BLOCKER SCORE TESTS                               //
    // -------------------------------------------------------------------------------- //

    [Test]
    public void CalculateBlockerPercentage_ValidInput_ReturnsCorrectPercentage()
    {
        int blocked = 2;
        int attempted = 5;

        float result = util.calculateBlockerPercentage(blocked, attempted);

        Assert.AreEqual(40f, result, 0.001f);
    }

    [Test]
    public void CalculateBlockerPercentage_ZeroAttempted_ReturnsZero()
    {
        float result = util.calculateBlockerPercentage(3, 0);

        Assert.AreEqual(0f, result);
    }


}

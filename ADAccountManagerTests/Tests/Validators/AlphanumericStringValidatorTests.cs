namespace ADAccountManagerTests.Tests.Validators
{
    public class AlphanumericStringValidatorTests
    {
        private readonly IStringValidator _validator;

        public AlphanumericStringValidatorTests()
        {
            _validator = new AlphanumericStringValidator();
        }

        [Theory]
        [InlineData("Hello,World")]
        [InlineData("abc123")]
        [InlineData("my-string'")]
        [InlineData("A1b2C3,d4")]
        public void IsValid_WithValidStrings_ReturnsTrue(string input)
        {
            // Act
            var result = _validator.IsValid(input);

            // Assert
            Assert.True(result.IsValid);
            Assert.Null(result.ErrorMessage);
        }

        [Theory]
        [InlineData("Hello@World")]
        [InlineData("abc!123")]
        [InlineData("my-string#")]
        [InlineData("INVALID @&*")]
        public void IsValid_WithInvalidStrings_ReturnsFalse(string input)
        {
            // Act
            var result = _validator.IsValid(input);

            // Assert
            Assert.False(result.IsValid);
            Assert.NotNull(result.ErrorMessage);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void IsValid_WithNullOrEmptyStrings_ReturnsFalse(string input)
        {
            // Act
            var result = _validator.IsValid(input);

            // Assert
            Assert.False(result.IsValid);
            Assert.NotNull(result.ErrorMessage);
        }
    }
}
using System;
using Xunit;
using Xunit.Extensions;

namespace MusicCloud.UnitTest
{
    public class SlugConverterTest
    {
        [Fact]
        public void ConvertWithNullInputThrows()
        {
            var sut = new SlugConverter();
            Assert.Throws<ArgumentNullException>(() => sut.Convert(null));
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void ConvertWithEmptyInputThrows(string input)
        {
            var sut = new SlugConverter();
            Assert.Throws<ArgumentException>(() => sut.Convert(input));
        }

        [Theory]
        [InlineData("foo", "foo")]
        [InlineData("bar", "bar")]
        [InlineData("Bar", "bar")]
        [InlineData("foo bar", "foo-bar")]
        [InlineData("foo  bar", "foo-bar")]
        [InlineData("foo   bar", "foo-bar")]
        [InlineData(" foo bar ", "foo-bar")]
        [InlineData("foo-bar ", "foo-bar")]
        [InlineData("foo_bar ", "foo-bar")]
        [InlineData("foo__bar ", "foo-bar")]
        [InlineData("__foo__", "foo")]
        [InlineData("foo@", "foo")]
        [InlineData("*foo", "foo")]
        [InlineData("*fo0", "fo0")]
        public void ConvertReturnsCorrectSlug(string input, string expected)
        {
            var sut = new SlugConverter();
            var actual = sut.Convert(input);
            Assert.Equal(expected, actual);
        }
    }
}

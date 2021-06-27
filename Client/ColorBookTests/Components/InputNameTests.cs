using Bunit;
using ColorBook.Components;
using Xunit;

namespace ColorBookTests.Components
{
    public class InputNameTests : TestContext
    {
        [Fact]
        public void InputName_WhenColorHasName_DisplaysName()
        {
            var colorHex = "#000000";
            var expected = "black";

            JSInterop.Setup<string>("colorTools.getName", colorHex).SetResult(expected);

            var cut = RenderComponent<InputName>(param => param
                .Add(p => p.Value, colorHex));

            var nameElement = cut.Find("input");
            var actual = nameElement.GetAttribute("value");

            Assert.Equal(expected, actual, ignoreCase: true);
        }

        [Fact]
        public void InputName_WhenColorWithoutName_DisplaysEmpty()
        {
            var colorHex = "#123456";
            var expected = string.Empty;

            JSInterop.Setup<string>("colorTools.getName", colorHex).SetResult(expected);

            var cut = RenderComponent<InputName>(param => param
                .Add(p => p.Value, colorHex));

            var nameElement = cut.Find("input");
            var actual = nameElement.GetAttribute("value");

            Assert.Equal(expected, actual, ignoreCase: true);
        }
    }
}

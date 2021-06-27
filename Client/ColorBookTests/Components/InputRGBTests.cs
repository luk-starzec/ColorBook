using Bunit;
using ColorBook.Components;
using Xunit;

namespace ColorBookTests.Components
{
    public class InputRGBTests : TestContext
    {
        private string initHex = "#000000";
        private (int r, int g, int b) initRgb = (0, 0, 0);

        private IRenderedComponent<InputRGB> cut;

        public InputRGBTests()
        {
            JSInterop.Setup<string>("colorTools.rgbToHex", initRgb.r, initRgb.g, initRgb.b).SetResult(initHex);

            JSInterop.Setup<int>("colorTools.getRed", _ => true).SetResult(initRgb.r);
            JSInterop.Setup<int>("colorTools.getGreen", _ => true).SetResult(initRgb.g);
            JSInterop.Setup<int>("colorTools.getBlue", _ => true).SetResult(initRgb.b);

            cut = RenderComponent<InputRGB>(param => param
                .Add(p => p.Value, initHex));
        }

        [Fact]
        public void InputHSL_WhenChangedH_SetsValue()
        {
            var expectedHex = "#4095bf";
            var expectedRgb = (r: 255, g: initRgb.g, b: initRgb.b);

            JSInterop.Setup<string>("colorTools.rgbToHex", expectedRgb.r, expectedRgb.g, expectedRgb.b).SetResult(expectedHex);

            var hElement = cut.FindAll("input")[0];
            hElement.Input(expectedRgb.r.ToString());
            
            var actual = cut.Instance.Value;

            Assert.Equal(expectedHex, actual, ignoreCase: true);
        }

        [Fact]
        public void InputHSL_WhenChangedS_SetsValue()
        {
            var expectedHex = "#00ff00";
            var expectedRgb = (r: initRgb.r, g: 255, b: initRgb.b);

            JSInterop.Setup<string>("colorTools.rgbToHex", expectedRgb.r, expectedRgb.g, expectedRgb.b).SetResult(expectedHex);

            var hElement = cut.FindAll("input")[1];
            hElement.Input(expectedRgb.g.ToString());
            
            var actual = cut.Instance.Value;

            Assert.Equal(expectedHex, actual, ignoreCase: true);
        }

        [Fact]
        public void InputHSL_WhenChangedL_SetsValue()
        {
            var expectedHex = "#0000ff";
            var expectedRgb = (r: initRgb.r, g: initRgb.g, b: 255);

            JSInterop.Setup<string>("colorTools.rgbToHex", expectedRgb.r, expectedRgb.g, expectedRgb.b).SetResult(expectedHex);

            var hElement = cut.FindAll("input")[2];
            hElement.Input(expectedRgb.b.ToString());
           
            var actual = cut.Instance.Value;

            Assert.Equal(expectedHex, actual, ignoreCase: true);
        }
    }
}

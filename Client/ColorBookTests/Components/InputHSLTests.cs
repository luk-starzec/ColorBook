using Bunit;
using ColorBook.Components;
using Xunit;

namespace ColorBookTests.Components
{
    public class InputHSLTests : TestContext
    {
        private string initHex = "#bf4040";
        private (int h, int s, int l) initHsl = (0, 50, 50);

        private IRenderedComponent<InputHSL> cut;

        public InputHSLTests()
        {
            JSInterop.Setup<string>("colorTools.hslToHex", initHsl.h, initHsl.s, initHsl.l).SetResult(initHex);
            JSInterop.Setup<string>("colorTools.hslToHex", 0, 0, 0).SetResult("#000000");

            JSInterop.Setup<int>("colorTools.getHue", _ => true).SetResult(initHsl.h);
            JSInterop.Setup<int>("colorTools.getSaturation", _ => true).SetResult(initHsl.s);
            JSInterop.Setup<int>("colorTools.getLightness", _ => true).SetResult(initHsl.l);

            cut = RenderComponent<InputHSL>(param => param
                .Add(p => p.Value, initHex));
        }

        [Fact]
        public void InputHSL_WhenChangedH_SetsValue()
        {
            var expectedHex = "#4095bf";
            var expectedHsl = (h: 200, s: initHsl.s, l: initHsl.l);

            JSInterop.Setup<string>("colorTools.hslToHex", expectedHsl.h, expectedHsl.s, expectedHsl.l).SetResult(expectedHex);

            var hElement = cut.FindAll("input")[0];
            hElement.Input(expectedHsl.h.ToString());

            var actual = cut.Instance.Value;

            Assert.Equal(expectedHex, actual, ignoreCase: true);
        }

        [Fact]
        public void InputHSL_WhenChangedS_SetsValue()
        {
            var expectedHex = "#ff0000";
            var expectedHsl = (h: initHsl.h, s: 100, l: initHsl.l);

            JSInterop.Setup<string>("colorTools.hslToHex", expectedHsl.h, expectedHsl.s, expectedHsl.l).SetResult(expectedHex);

            var hElement = cut.FindAll("input")[1];
            hElement.Input(expectedHsl.s.ToString());

            var actual = cut.Instance.Value;

            Assert.Equal(expectedHex, actual, ignoreCase: true);
        }

        [Fact]
        public void InputHSL_WhenChangedL_SetsValue()
        {
            var expectedHex = "#ffffff";
            var expectedHsl = (h: initHsl.h, s: initHsl.s, l: 100);

            JSInterop.Setup<string>("colorTools.hslToHex", expectedHsl.h, expectedHsl.s, expectedHsl.l).SetResult(expectedHex);

            var hElement = cut.FindAll("input")[2];
            hElement.Input(expectedHsl.l.ToString());

            var actual = cut.Instance.Value;

            Assert.Equal(expectedHex, actual, ignoreCase: true);
        }
    }
}

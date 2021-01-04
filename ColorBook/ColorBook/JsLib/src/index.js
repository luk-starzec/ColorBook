import Color from 'color-js';

export function colorTest() {
    var green = Color("#00FF00");
    console.log(green);
    console.log(green.toHSV())
    console.log(green.getHue())
    return green.toString();
}
